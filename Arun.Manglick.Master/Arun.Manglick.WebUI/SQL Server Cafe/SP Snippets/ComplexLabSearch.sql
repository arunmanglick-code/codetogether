if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VOC_CST_getSearchMsgsForLab]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	Drop procedure [dbo].[VOC_CST_getSearchMsgsForLab]
	PRINT 'Dropped Stored Procedure: dbo.[VOC_CST_getSearchMsgsForLab]'
END
GO


set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


CREATE Procedure [dbo].[VOC_CST_getSearchMsgsForLab]

@groupID int,
@institutionID int ,
@startDt varchar (100),
@endDt varchar (100) ,
@ocName varchar (100) = null,
@rcName varchar (100) = null,
@findingName varchar (100),
@dob varchar (100) = null,
@mrn varchar(100) = null,
@accession varchar(150) = null,
@msgStatus varchar(10),
@nurseName  varchar (100),
@UnitName  varchar (100),
@ctName varchar (100) = null

AS


DECLARE @isRadDeptLab INT
DECLARE @startDate DateTime,
		@endDate DateTime,
		@dob1 DateTime

SET @isRadDeptLab = 2 -- for Lab
SELECT @startDate = cast(@startDt as Datetime)
SELECT @endDate = cast(@endDt as dateTime) + 1
if (@dob != '') 
  Select @dob1 = Cast(@dob as DateTime)


SELECT DISTINCT 
	M.MessageID,        
	M.PassCode1,
	SpecialistDisplayName = SU.FirstName + ' ' + SU.LastName,
	M.RecipientTypeID,
	M.RecipientID,        
	RecipientDisplayName = CASE WHEN M.RecipientID = -1 THEN NSR.RecipientName
						   ELSE dbo.fnVoc_vl_GetRecipient(M.RecipientID,M.RecipientTypeID,M.CreatedOn) END,  
	NurseID = CASE WHEN RecipientTypeID=3 THEN 
				(
					dbo.fnVoc_vl_GetNurseIDFromAssignment (M.RecipientID, M.CreatedOn)
				 )
				  ELSE 0 END,	
	CreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, G.TimeZoneID),
	CASE 
		WHEN M.DOB IS NULL THEN 
			CASE WHEN LEN(M.MRN) = 0 THEN '' ELSE M.MRN END 
		ELSE Convert(varchar(10),M.DOB,101) 
	END AS DOBorMRN,
	G.GroupName,
	PatientVoiceURL,
	ImpressionVoiceURL,
	FindingDescription,
	M.ReadOn,
	EscalationsComplete, 
	ComplianceEscalationComplete, 
	--reply                        
	MR.MessageReplyID,
	ReplyCreatedOn = dbo.fnVO_VD_getDateByUsersTime(MR.CreatedOn, G.TimeZoneID),
	MR.ReplyVoiceURL,        
	MR.ReadOn as ReplyReadOn,
	-- Readback 
	ReadbackID = NULL,
	ReadbackCreatedOn = NULL,
	AcceptRejectOn = NULL,
	Accepted = NULL,
	ReadbackVoiceURL = NULL,
	BedNumber = CASE WHEN IsNull(RB.BedNumber,'') <> '' THEN
					RB.RoomNumber + ': ' + RB.BedNumber
				ELSE
					RB.RoomNumber
			    END,
	(select count(*) from dbo.voc_vl_messagelabresults ML where ML.MessageID=M.MessageID) as LabResultCount,				
	RB.UnitID,
	0 AS [IsDepartmentMessage],
	M.IsDocumented,
	Note = (SELECT max(CreatedOn) FROM  VOC_Vl_MessageNotes WHERE Messageid = M.MessageID AND NoteType <> 2 ),
	Stage = (Select SequenceID  
			From NotificationEventsSequence  
			Where EventDescription  =  
			(
			  SELECT TOP 1   
				  CASE     
				   WHEN (CHARINDEX ('Message closed by',EventDescription) > 0) THEN 'Message closed'    
				   WHEN  (CHARINDEX ('A result has been documented by',EventDescription) > 0)   THEN 'Documented Message'
				   WHEN (CHARINDEX ('-Outbound call',EventDescription) > 0 OR 
						 CHARINDEX ('-Partners Page',EventDescription) > 0 OR 
						 CHARINDEX ('-Unit Fax Notification Sent to',EventDescription) > 0 OR 
						 CHARINDEX ('-Clinical Team Fax Notification Sent to',EventDescription) > 0)
				   THEN SUBSTRING (EventDescription, 1, CHARINDEX('-',EventDescription) - 1)    
				   ELSE EventDescription  
				  END   
			  FROM VOC_VL_NotificationHistories  NH 
			  WHERE MessageID = M.MessageID AND NH.CreatedOn <= Getdate() AND
			  EventDescription NOT IN   
			  (
				'Outbound Call To Unit Has Been Accepted',
				'Outbound Call To Department Has Been Accepted',
				'Outbound Call To Nurse Has Been Accepted',
				'Ordering Physician Call Start',  
				'Call Start',
				'RP After Hours Notification',  
				'Unit Notification',
				'Clinial Team Notification'
			   )  
			   ORDER BY NotificationHistoryID DESC    
			)),
	SecondStage = (Select SequenceID  
					From NotificationEventsSequence  
					Where EventDescription  =  
					(
						Select top 1 col1 from (
						SELECT  top 2  NotificationHistoryID,
						  CASE     
						   WHEN (CHARINDEX ('Message closed by',EventDescription) > 0) THEN 'Message closed'    
						   WHEN  (CHARINDEX ('A result has been documented by',EventDescription) > 0)   THEN 'Documented Message'
						   WHEN (CHARINDEX ('-Outbound call',EventDescription) > 0 OR 
								 CHARINDEX ('-Partners Page',EventDescription) > 0 OR 
								 CHARINDEX ('-Unit Fax Notification Sent to',EventDescription) > 0 OR 
								 CHARINDEX ('-Clinical Team Fax Notification Sent to',EventDescription) > 0)
						   THEN SUBSTRING (EventDescription, 1, CHARINDEX('-',EventDescription) - 1)    
						   ELSE EventDescription  
						  END  as col1 
					  FROM VOC_VL_NotificationHistories  NH   
					  WHERE MessageID = M.MessageID AND NH.CreatedOn <= Getdate() AND
					  EventDescription NOT IN   
					  (
						'Outbound Call To Unit Has Been Accepted',
						'Outbound Call To Department Has Been Accepted',
						'Outbound Call To Nurse Has Been Accepted',
						'Ordering Physician Call Start',  
						'Call Start',
						'RP After Hours Notification',  
						'Unit Notification',
						'Clinial Team Notification'
					   )  
					   ORDER BY NotificationHistoryID DESC
		) aa ORDER BY NotificationHistoryID)),
	MessageState = (CASE WHEN (ComplianceEscalationComplete = 1) THEN 4 
					 ELSE (CASE WHEN (EscalationsComplete = 1) THEN 3 
					 ELSE (CASE WHEN (BackupnotifyStarted = 1) THEN 2 
					 ELSE 1 END) END) END),
	Failures = (SELECT COUNT(NotificationHistoryID) FROM dbo.Voc_VL_NotificationHistories  
				WHERE NotificationStatusID=3 AND MessageID = M.MessageID ),
	Starts = (SELECT COUNT(messageID) FROM VOC_vl_outboundnotificationhistories WHERE MessageID = M.MessageID AND Recipient != 'Department'),
	IsLabMessage = 1,
	MessageStatus = CASE WHEN M.ReadOn IS NOT NULL THEN 'Closed' 
						 ELSE  dbo.fnVoc_DateTimeDifference(M.CreatedOn)
					END + 
					CASE WHEN MR.MessageReplyID IS NOT NULL THEN 
						(CASE WHEN MR.ReadOn IS NOT NULL THEN '<br><br> Reply Closed' 
							  ELSE '<br><br>' + dbo.fnVoc_DateTimeDifference(MR.CreatedOn)
	     END) 
						ELSE '' 
					END,
	MessageStatusDateTime =   
			CASE 
				WHEN (@msgStatus = 'OPEN') OR (@msgStatus = 'EMBARGO') THEN  
					CASE 
						WHEN M.ReadOn IS NULL THEN M.CreatedOn --Original Message Open   									 
						WHEN M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NULL THEN MR.CreatedOn   --Original Message Closed but reply open
						WHEN M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NOT NULL THEN '1/1/1960' --Original Message Closed and reply also Closed   
						WHEN M.ReadOn IS NOT NULL THEN  '1/1/1960' 					
						ELSE '1/1/1960' 									 
					END
				ELSE '1/1/1960'
			END,     
	MRN,
	Accession,
	DOB,
	--V.DepartmentName as DepartmentName
	DeclinedAt,
	DeclinedMessageVoiceURL
								
	INTO  #TMP1 	 
	FROM 	Voc_VL_Messages AS M
		INNER JOIN Specialists AS S ON S.SpecialistID = M.SpecialistID
		INNER JOIN Subscribers AS SU ON SU.SubscriberID = S.SubscriberID AND SU.GroupID =  CASE WHEN (@groupID = -1) Then SU.GroupID ELSE @groupID END 
		INNER JOIN Groups AS G ON G.GroupID = SU.GroupID  AND G.InstitutionID = CASE WHEN (@institutionID = -1) THEN G.InstitutionID ELSE @institutionID END   
		INNER JOIN Findings AS F ON F.FindingID = M.FindingID
		LEFT OUTER JOIN Voc_VL_MessageReplies as MR on M.MessageId = MR.MessageID    
		LEFT OUTER JOIN VOC_VL_MessageDeclined AS MD ON MD.MessageID = M.MessageID
		LEFT OUTER JOIN Voc_VL_UnitRoomBedDetails RB ON RB.RoomBedID = M.RoomBedID	
		LEFT OUTER JOIN Voc_VL_Units UN1 ON UN1.UnitID = 
		CASE 
			WHEN M.RecipientTypeID = 2 THEN
			M.RecipientID ELSE RB.UnitID
		END
		LEFT OUTER JOIN VOC_ACC_NonSystemRecipients NSR ON NSR.MessageID = M.MessageId AND NSR.MessageType = 3 
	WHERE M.CreatedOn <= GETDATE() 
	AND  ((M.CreatedOn BETWEEN @startDate AND @endDate) OR (M.AgentCreatedOn BETWEEN @startDate AND @endDate))
	AND  CASE WHEN (@msgStatus = 'OPEN') THEN
		CASE WHEN (M.ReadOn IS NULL AND M.Active = 1) THEN 1
		     WHEN (M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NULL) THEN 1
		     ELSE 0
		END
		   WHEN (@msgStatus = 'CLOSE') THEN 
				CASE  WHEN (M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND  MR.ReadOn IS NOT NULL) THEN 1
					  WHEN (M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND  MR.ReadOn IS NULL) THEN 0
					  WHEN (M.ReadOn IS NOT NULL) THEN 1
				END
		   WHEN (@msgStatus = 'DOCUMENTED')THEN
			   CASE  WHEN (M.ReadOn IS NOT NULL AND M.IsDocumented = 1) THEN 1
					 ELSE 0
			   END
		   WHEN (@msgStatus = 'REPLIES') THEN    
				CASE WHEN (M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NULL) THEN 1  
					 ELSE 0  
				END 		
		   ELSE 1    
		  END =1 
	AND G.GroupID NOT IN (SELECT GroupID FROM GroupsToExcludeFromBigBoard)


SELECT 
  MessageID,  
  PassCode1,    
  SpecialistDisplayName,  
  RecipientTypeID ,  
  RecipientID ,    
  RecipientDisplayName ,  
  NurseID ,  
  CreatedOn_UsersTime,
  DOBorMRN,      
  GroupName,          
  PatientVoiceURL,  
  ImpressionVoiceURL,  
  FindingDescription,  
  ReadOn,    
  EscalationsComplete,   
  ComplianceEscalationComplete,  
  --reply  
  MessageReplyID,  
  ReplyCreatedOn ,  
  ReplyVoiceURL,  
  ReplyReadOn ,    
  -- Readback  
  ReadbackID,  
  ReadbackCreatedOn,  
  AcceptRejectOn,  
  Accepted,    
  ReadbackVoiceURL,  
     -- Lab Bed Number  
  BedNumber ,    
  LabResultCount ,  
  UnitID ,  
  IsDepartmentMessage,  
  IsDocumented,  
  Note,  
  Stage,
  Failures,  
  Starts,  
  IsLabMessage,
  MessageStatus,
  MessageStatusDateTime,
  MRN,
  Accession,
  DOB,
  SecondStage,
  MessageState,
  BackupStartedAt = NULL,
  DeclinedAt,
  DeclinedMessageVoiceURL			 
  --DepartmentName 			 
FROM #TMP1
WHERE
   CASE WHEN ISNULL(@ocName, '') = ''   THEN 1 
		WHEN (@ocName != '') AND RecipientDisplayName = @ocName THEN 1
		ELSE 0        
		END = 1
AND CASE WHEN ISNULL(@rcName, '') = ''   THEN 1 
		WHEN (@rcName != '') AND SpecialistDisplayName = @rcName THEN 1
		ELSE 0        
		END = 1
AND CASE WHEN ISNULL(@findingName, '') = ''   THEN 1 
		WHEN (@findingName != '') AND FindingDescription = @findingName THEN 1
        ELSE 0
		END = 1
AND CASE WHEN ISNULL(@mrn, '') = ''   THEN 1 
		WHEN (@mrn != '') AND MRN = @mrn THEN 1
		ELSE 0        
		END = 1
AND CASE WHEN ISNULL(@dob1, '') = ''   THEN 1 
		WHEN (@dob1 != '') AND DOB = @dob1 THEN 1
		ELSE 0        
		END = 1
AND CASE WHEN ISNULL(@accession, '') = ''   THEN 1 
		WHEN (@accession != '') AND Accession = @accession THEN 1
		ELSE 0
        END = 1
AND CASE WHEN ISNULL(@nurseName, '') = ''   THEN 1 
		WHEN (@nurseName != '') AND RecipientDisplayName = @nurseName THEN 1
		ELSE 0
        END = 1
AND CASE WHEN ISNULL(@UnitName, '') = ''   THEN 1 
		WHEN (@UnitName != '') AND RecipientDisplayName = @UnitName THEN 1
		ELSE 0
        END = 1
AND CASE WHEN ISNULL(@ctName, '') = ''   THEN 1 
		WHEN (@ctName != '') AND RecipientDisplayName = @ctName THEN 1
		ELSE 0
        END = 1


DROP TABLE #TMP1
GO

PRINT 'Created Stored Procedure: dbo.[VOC_CST_getSearchMsgsForLab]' + CHAR(13)
GO