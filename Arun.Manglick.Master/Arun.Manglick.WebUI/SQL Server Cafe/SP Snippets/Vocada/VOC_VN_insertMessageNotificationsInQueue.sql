Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/******************************************************************************
**		File: VOC_VN_insertMessageNotificationsInQueue.sql
**		Name: VOC_VN_insertMessageNotificationsInQueue
**		Desc: Inserts Message Notifications in Queue.
**
**		Events
**		OC
**		1	Primary
**		2	Backup
**		3	Fail-Safe
**		
**		Group
**		1	On Received
**		2	On Reply
**		3	On End Escalation
**		4	On Backup Notify
**		5	On Compliance Alert
**		6	On Message Declined
**		7	On Message Readback
**		
**		Subscriber
**		1	On Read
**		2	On Reply
**		3	On End Escalation
**		4	On Message Declined
**		5	On Message Readback
**		
**		unit
**		1	Primary
**		2	Backup
**		3	Failsafe
**		
**		CT
**		1	Primary	1
**		2	BackUp	1
**		3	Fail-Safe	1
**		
**		RecipientType
**		1- OC
**		2- Group
**		3- Subscriber
**		4- Unit
**		5- CT
**		
**		MessageType
**		1 - Radiology OC Message
**		2 - Radiology CT Message
**		3 - Lab Message
**		
**		Called by:   Veriphy Notifier web service
**              
**		Parameters:
**		Input																		Output
**     ----------															-----------
**		messsageID INT
**		messageType	INT 
**		@recipientType INT
**		
**		Auth: Nitin Mankar
**		Date: 16th July 2008
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			-------------------------------------------
**		08-10-2008	NDM					Modified for Agent message notifications.
**		11-12-2008	NDM					Long escalation embargo issues fixes.
**		11-19-2008	NDM					Embargo issue fixes
**    	12-04-2008  Suhas       		Embargo Calculation. 
**    	16-12-2008  Suhas       		Production 3.0 issue -TimeZone added in the Embargo Calculation
**    	02-02-2009  Suhas       		TTP Defect #368 - Escalations not firing
**    	25-02-2009  Suhas       		TTP Defect #428 - Notification Sent during Embargo Period-Compliance Calculation Modified.
**    	26-02-2009  Suhas       		Revised - Compliance Calculation Modified.
**    	27-02-2009  Suhas       		Error handling done if Finding settings are zero value.
**	  	04-13-2009  Suhas       		Rad DB Rearchitecture changes.
**	  	07-28-2009  Suhas		  		Fetching timezone settings from facility preferences.
**		08-31-2009	Suhas				DST changes for CST time zone.
**		09-09-2009	Suhas				DST changes for Embargo.
**		11-02-2009	Suhas				TTP #688 - Embargo for Unit Message
*******************************************************************************/

CREATE PROCEDURE VOC_VN_insertMessageNotificationsInQueue 
@messageID int,
@messageType int,
@recipientType int,
@notificationHistoryID int
AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @msgExists INT
	SELECT @msgExists = count(*) FROM Voc_MessageNotificationQueue WHERE MessageID = @messageID AND MessageType = @messageType
	IF @msgExists > 0
		return;
	DECLARE @timeZoneOffset INT  
	DECLARE @timeZoneID INT 
	DECLARE @timeZoneRegKey VARCHAR(250)  
	DECLARE @groupID INT  
	DECLARE @findingID INT, @escalateEvery INT, @startBackupAt INT, @endAfterMinutes INT, @complianceGoal INT
	DECLARE @escalationsRequired INT
	DECLARE @sendAT DateTime
	DECLARE @DSTCheckSendAt DateTime
	DECLARE @escalationNumber INT	
	DECLARE @sendComplianceAT DateTime
	DECLARE @continueToSendPrimary BIT
	DECLARE @embargoSpanWeekend INT, @embargoStartHour INT, @embargoEndHour INT, @embargo INT
	DECLARE @totalHoursInEmbargo INT
	DECLARE @createdOn DateTime
	DECLARE @sendTimeMinutes INT
	DECLARE @actualSendAt DateTime -- if notification in embargoe then insert history should have embargoed notification entry on correct time
	DECLARE @inEmbargo BIT
	DECLARE @eventID INT
	DECLARE @outStandingTime INT
	DECLARE @backupEscalationNumber INT
	DECLARE @checkEmbargo BIT
	DECLARE @tempDate DateTime
	DECLARE @tempMin INT
	DECLARE @lastSendAt DateTime
	DECLARE @totalMinutesoutsideEmbargo INT
	DECLARE @requireEmbargoForUnitMessage BIT
	SET @totalMinutesoutsideEmbargo = 0
	SET @checkEmbargo = 1
	SET @eventID = 1
	SET @tempMin = 0
	
	IF @messageType = 1
    BEGIN
      SELECT @groupID = SU.GroupID, @findingID = M.FindingID, @createdOn = M.CreatedOn, @timeZoneOffset = OffsetFromCST, @timeZoneRegKey = Regkey, @timeZoneID = TZ.TimeZoneID
      FROM VOC_Messages AS M
      INNER JOIN Specialists AS SP ON M.SpecialistID = SP.SpecialistID 
      INNER JOIN Subscribers AS SU ON SP.SubscriberID = SU.SubscriberID    
	  LEFT OUTER JOIN VOC_Facility AS FAC ON M.FacilityID = FAC.FacilityID
	  LEFT OUTER JOIN TimeZones AS TZ ON FAC.TimeZoneID = TZ.TimeZoneID
      WHERE MessageID = @messageID   

      --SELECT @timeZoneOffset = OffsetFromCST FROM TimeZones AS TZ JOIN Groups AS G on G.TimeZoneID = TZ.TimeZoneID WHERE GroupID = @groupID   
    END
	ELSE
	  BEGIN
      SELECT @groupID = SU.GroupID, @findingID = M.FindingID, @createdOn = M.CreatedOn, @timeZoneOffset = OffsetFromCST, @timeZoneRegKey = Regkey, @timeZoneID = TZ.TimeZoneID
      FROM Voc_VL_Messages AS M
      INNER JOIN Specialists AS SP ON M.SpecialistID = SP.SpecialistID 
      INNER JOIN Subscribers AS SU ON SP.SubscriberID = SU.SubscriberID 
	  LEFT OUTER JOIN VOC_Facility AS FAC ON M.FacilityID = FAC.FacilityID
	  LEFT OUTER JOIN TimeZones AS TZ ON FAC.TimeZoneID = TZ.TimeZoneID  
      WHERE MessageID = @messageID
    
      --SELECT @timeZoneOffset = OffsetFromCST FROM TimeZones AS TZ JOIN Groups AS G on G.TimeZoneID = TZ.TimeZoneID WHERE GroupID = @groupID
	END

	SELECT @requireEmbargoForUnitMessage  = RequireEmbargoForUnitMessage FROM GroupPreferences WHERE GroupID = @groupID
	SELECT @findingID = FindingID, @escalateEvery = EscalateEvery, @startBackupAt =StartBackupAt, 
				 @endAfterMinutes = EndAfterMinutes, @complianceGoal = ComplianceGoal, @continueToSendPrimary = ContinueToSendPrimary,
				 @embargoSpanWeekend = EmbargoSpanWeekend, @embargoStartHour = EmbargoStartHour, @embargoEndHour = EmbargoEndHour, @embargo = Embargo
	FROM Findings
	WHERE FindingID = @findingID

  SELECT @timeZoneOffset = dbo.fnVO_TimeZoneOffset(GETDATE(),@timeZoneRegKey)
  SET @timeZoneOffset = @timeZoneOffset/60

  SET @embargoStartHour = @embargoStartHour - @timeZoneOffset
  SET @embargoEndHour = @embargoEndHour - @timeZoneOffset
  
  IF @embargoStartHour < 0 
    SET @embargoStartHour = 24 + @embargoStartHour
  IF @embargoEndHour < 0 
    SET @embargoEndHour = 24 + @embargoEndHour

  IF @embargoStartHour > 23 
    SET @embargoStartHour =  @embargoStartHour - 24
  IF @embargoEndHour > 23 
    SET @embargoEndHour = @embargoEndHour - 24

	--Unit Message Embargo settings 
	IF @recipientType = 4 AND @requireEmbargoForUnitMessage = 0
		SET @checkEmbargo = 0
    ELSE
		SET @checkEmbargo = @embargo

	IF @escalateEvery != 0
	BEGIN
		SELECT @escalationsRequired = @endAfterMinutes / @escalateEvery
	END
	
	SET @escalationNumber = 0
	SET @sendAt = getdate()
	SET @actualSendAt = @sendAt;
	SET @lastSendAt = @sendAt;
	SET @sendComplianceAT = dbo.fnVO_AddMinutesToDate(@sendAt, @complianceGoal) --DateAdd(mi, @complianceGoal, @sendAt)
	SET @outStandingTime = DateDiff(mi, getdate(), @sendAt)
	SET @backupEscalationNumber = 0
	IF( @checkEmbargo = 1  AND dbo.fnVO_InEmbargo (@sendAt,@embargoSpanWeekend, @embargoStartHour,@embargoEndHour,@embargo, @timeZoneID) = 1)
	BEGIN
							--insert embargo notification for history
			INSERT INTO Voc_MessageEmbargoedNotificationQueue (MessageID, EventID, MessageType, SendAt, Completed, NotificationHistoryID)
			VALUES	(@messageID, @eventID, @messageType, @sendAt, 0, @notificationHistoryID)
			SET @notificationHistoryID = -1;
			SELECT @totalHoursInEmbargo = dbo.fnVO_VN_calculateEmbargoMinutes(@sendAt, @embargoSpanWeekend,@embargoEndHour,@embargoStartHour, @embargo, @timeZoneID)
			SET @sendAt = dbo.fnVO_AddMinutesToDate(@sendAt, @totalHoursInEmbargo) --DateAdd(mi,@totalHoursInEmbargo, @sendAt)
			IF (@timeZoneID = 8)
				SET @sendAt = dbo.fnVO_AddMinutesToDate(@sendAt, -30) --DateAdd(mi,@totalHoursInEmbargo, @sendAt)
			SET @lastSendAt = @sendAt;
			SET @inEmbargo = 1
	END
	
  DECLARE @complianceEsc INT
  SELECT @complianceEsc = -1
  IF @escalateEvery <> 0
		SET @complianceEsc = @complianceGoal / @escalateEvery

	WHILE (@escalationNumber <= @complianceEsc)
	BEGIN
	  IF @escalationNumber > 0 
		SET @notificationHistoryID = 0
	  IF(@escalationNumber <= @escalationsRequired)
      BEGIN 
          IF @escalationNumber < @startBackupAT OR @continueToSendPrimary = 1
			    BEGIN		
					    INSERT INTO Voc_MessageNotificationQueue
									    (MessageID, EventID, MessageType, SendAt, EscalationNumber, RecipientType, Completed, NotificationHistoryID, OutStandingTime)
					    VALUES	(@messageID, 1, @messageType, @sendAt, @escalationNumber, @recipientType, 0, @notificationHistoryID, @outStandingTime)
					    SET @eventID = 1
			    END
			    IF @escalationNumber >= @startBackupAT
			    BEGIN
					    --Group backup notification
					    INSERT INTO Voc_MessageNotificationQueue
									    (MessageID, EventID, MessageType, SendAt, EscalationNumber, RecipientType, Completed, OutStandingTime)
					    VALUES	(@messageID, 2, @messageType, @sendAt, @backupEscalationNumber, @recipientType, 0, @outStandingTime)
					    SET @backupEscalationNumber = @backupEscalationNumber + 1

					    SET @eventID = 2
			    END
			    IF @escalationNumber = @startBackupAt
			    BEGIN
					    --backup notification
					    INSERT INTO Voc_MessageNotificationQueue
									    (MessageID, EventID, MessageType, SendAt, EscalationNumber, RecipientType, Completed, OutStandingTime)
					    VALUES	(@messageID, 4, @messageType, @sendAt, @escalationNumber, 2, 0, @outStandingTime)
					    SET @eventID = 4
			    END

			    IF @escalationNumber = @escalationsRequired
			    BEGIN
					    --end escalation/failsafe notification
					    INSERT INTO Voc_MessageNotificationQueue
									    (MessageID, EventID, MessageType, SendAt, EscalationNumber, RecipientType, Completed, OutStandingTime)
					    VALUES	(@messageID, 3, @messageType, @sendAt, @escalationNumber, @recipientType, 0, @outStandingTime)

					    INSERT INTO Voc_MessageNotificationQueue
									    (MessageID, EventID, MessageType, SendAt, EscalationNumber, RecipientType, Completed, OutStandingTime)
					    VALUES	(@messageID, 3, @messageType, @sendAt, @escalationNumber, 2, 0, @outStandingTime)

					    INSERT INTO Voc_MessageNotificationQueue
									    (MessageID, EventID, MessageType, SendAt, EscalationNumber, RecipientType, Completed, OutStandingTime)
					    VALUES	(@messageID, 3, @messageType, @sendAt, @escalationNumber, 3, 0, @outStandingTime)
					    SET @eventID = 3
			    END

			    IF(  @escalationNumber > 0 AND  @checkEmbargo = 1  AND dbo.fnVO_InEmbargo (@actualSendAt,@embargoSpanWeekend, @embargoStartHour,@embargoEndHour,@embargo, @timeZoneID) = 1 AND @escalationNumber <= @escalationsRequired)
			    BEGIN
				    --insert embargo notification for history
				    INSERT INTO Voc_MessageEmbargoedNotificationQueue (MessageID, EventID, MessageType, SendAt, Completed)
				    VALUES	(@messageID, @eventID, @messageType, @actualSendAt, 0)
			    END
		  END

			SET @escalationNumber = @escalationNumber + 1
			SET @outStandingTime = @outStandingTime + @escalateEvery
			
			SET @sendAt = dbo.fnVO_AddMinutesToDate(@sendAt, @escalateEvery) --DateAdd(mi, @escalateEvery , @sendAt)
			SET @DSTCheckSendAt = @sendAt
			SET @actualSendAt = dbo.fnVO_AddMinutesToDate(@actualSendAt, @escalateEvery) --DateAdd(mi, @escalateEvery, @actualSendAt)
			IF(  @escalationNumber > 0 AND  @checkEmbargo = 1  AND (dbo.fnVO_InEmbargo (@actualSendAt,@embargoSpanWeekend, @embargoStartHour,@embargoEndHour,@embargo, @timeZoneID) = 0) AND (dbo.fnVO_InEmbargo (@sendAt,@embargoSpanWeekend, @embargoStartHour,@embarg
oEndHour,@embargo, @timeZoneID) = 1	)  AND @escalationNumber <= @escalationsRequired)
				SET @actualSendAt = @sendAt
			DECLARE @embargostartDate Datetime
			DECLARE @embargoendDate Datetime
			DECLARE @tempHour INT
			SET @tempHour = 0
			SET @tempMin = 0

			IF ( @checkEmbargo = 1)
			  BEGIN
			    IF @embargostarthour < @embargoEndHour
				    SET @tempMin = @embargoEndHour - @embargostarthour
			    ELSE
				    SET @tempMin = (24 % @embargostarthour) + @embargoEndHour
			    SET @tempHour = @tempMin
			    SET @tempMin = @tempMin * 60
			  END

			DECLARE @counter INT
			DECLARE @totalEmbargoedTime INT
			SET @totalEmbargoedTime = 0
			SET @counter = 1
			IF( @checkEmbargo = 1 )
			  BEGIN
				  IF (dbo.fnVO_InEmbargo (@sendAt,@embargoSpanWeekend, @embargoStartHour,@embargoEndHour,@embargo, @timeZoneID) = 1)
				    BEGIN
					    IF DatePart(hh, @lastSendAt) < @embargostarthour AND DatePart(hh, @lastSendAt) != @embargoEndHour
						    SET @totalMinutesoutsideEmbargo = (@embargostarthour - DatePart(hh, @lastSendAt)) 
					    ELSE
						    SET @totalMinutesoutsideEmbargo = 0
				    END
      
			  SELECT @embargostartDate = dbo.fnVO_AddMinutesToDate(CONVERT(varchar, @lastSendAt, 101), @embargoStartHour*60) --Dateadd(hh, @embargoStartHour,CONVERT(varchar, @lastSendAt, 101) )
			  SELECT @embargoendDate = dbo.fnVO_AddMinutesToDate(CONVERT(varchar, @lastSendAt, 101), (@embargoStartHour + @tempHour)*60) --Dateadd(hh, @embargoStartHour + @tempHour,CONVERT(varchar, @lastSendAt, 101) )
			  SET @totalEmbargoedTime = @tempMin
			  IF @totalMinutesoutsideEmbargo > 0
				  SET @totalMinutesoutsideEmbargo = (@totalMinutesoutsideEmbargo * 60) - DatePart(mi, @lastSendAt)
  			
			  IF ((DateDiff(mi, @lastSendAt, @sendAt) - @totalEmbargoedTime < @escalateEvery AND (@escalateEvery + @tempMin >= (24*60)) OR (@embargostartDate>@lastSendAt and @sendAt>@embargoendDate))) 
				  BEGIN
					  While @counter < 10
					  BEGIN
						  SET @sendAt = DateAdd(mi, @tempMin, @sendAt)
              IF(@embargoSpanWeekend = 1 )
              BEGIN
                DECLARE @Dayname VARCHAR(10)
                SET @Dayname = UPPER(DATENAME(WEEKDAY, @sendAt))   
                SET @sendAt = DATEADD(DAY , CASE @Dayname WHEN 'SATURDAY' THEN 2  
                       WHEN 'SUNDAY' THEN 1  
                       ELSE 0  
                       END, @sendAt)     
              END
						  IF (DateDiff(mi, @lastSendAt, @sendAt) - @totalEmbargoedTime >= @escalateEvery AND (dbo.fnVO_InEmbargo (@sendAt,@embargoSpanWeekend, @embargoStartHour,@embargoEndHour,@embargo, @timeZoneID) = 0))
						    BEGIN	
							    SET @counter = 11
							    break
						    END
						  SET @totalMinutesoutsideEmbargo = 0
						  SET @totalEmbargoedTime = @totalEmbargoedTime + @tempMin
					  END
				  END
			  ELSE IF (dbo.fnVO_InEmbargo (@sendAt,@embargoSpanWeekend, @embargoStartHour,@embargoEndHour,@embargo, @timeZoneID) = 1)
				  BEGIN
              IF(@escalationNumber = @complianceEsc)
                BEGIN
                  INSERT INTO Voc_MessageEmbargoedNotificationQueue (MessageID, EventID, MessageType, SendAt, Completed)
		              VALUES	(@messageID, 5, @messageType, @sendAT, 0)
                END
						  SET @sendTimeMinutes = DatePart(mi, @sendAt)
				      SELECT @totalHoursInEmbargo = dbo.fnVO_VN_calculateEmbargoMinutes(@sendAt, @embargoSpanWeekend,@embargoEndHour,@embargoStartHour, @embargo, @timeZoneID)
			        IF @totalMinutesoutsideEmbargo > 0
				        SET @sendAt = dbo.fnVO_AddMinutesToDate(@sendAt, @totalHoursInEmbargo + (@escalateEvery - @totalMinutesoutsideEmbargo)) --DateAdd(mi,@totalHoursInEmbargo + (@escalateEvery - @totalMinutesoutsideEmbargo)  , @sendAt)
			        ELSE
				        SET @sendAt = dbo.fnVO_AddMinutesToDate(@sendAt, @totalHoursInEmbargo + @sendTimeMinutes) --DateAdd(mi,@totalHoursInEmbargo  , @sendAt)
						  SET @totalMinutesoutsideEmbargo = 0
				  END

        END
			SET @counter = @counter + 1
			SET @tempMin = 0
			SET @tempHour = 0
      
			IF (dbo.fnVO_InEmbargo (@DSTCheckSendAt,@embargoSpanWeekend, @embargoStartHour,@embargoEndHour,@embargo, @timeZoneID) = 1)
				BEGIN
					SET @sendAt = dbo.fnVO_AddMinutesToDate(@sendAt, dbo.fnVO_DSTOffset(@lastSendAt, @sendAt))
				END

			SET @lastSendAt = @sendAt
      IF(@escalationNumber = @complianceEsc)
        BEGIN
            SET @sendComplianceAT = @SendAt
      END
  END
	
  SET @outStandingTime = @complianceGoal
  
   IF(@complianceEsc > 0)
    BEGIN
	    INSERT INTO Voc_MessageNotificationQueue
					    (MessageID, EventID, MessageType, SendAt, EscalationNumber, RecipientType, Completed, OutStandingTime)
	    VALUES	(@messageID, 5, @messageType, @sendComplianceAT, @complianceEsc, 2, 0, @outStandingTime)
    END
END

