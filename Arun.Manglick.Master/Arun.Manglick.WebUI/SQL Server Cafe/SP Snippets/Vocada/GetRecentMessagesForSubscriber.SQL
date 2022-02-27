
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VOC_VW_getRecentMessagesForSubscriber]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[VOC_VW_getRecentMessagesForSubscriber]
	PRINT 'Dropped Stored Procedure: dbo.[VOC_VW_getRecentMessagesForSubscriber]'
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[VOC_VW_getRecentMessagesForSubscriber]
	@subscriberID INT,
	@startDate DATETIME,
	@endDate DATETIME,
	@OPhysician VARCHAR(100) = NULL,
	@RClinician VARCHAR(100) = NULL,
	@PatientName VARCHAR(100) = NULL,
	@MRN VARCHAR(100) = NULL,
	@DOB DATETIME = NULL,
	@Accession VARCHAR(150) = NULL,
	@FindingID INT = NULL,
	@SearchBy BIT,
	@oPhyID INT,
	@groupID INT,
	@ctName VARCHAR(100) = NULL,
	@RecipientTypeID INT,
	@facilityID INT = -1,
	@MessageStatus VARCHAR(100) = 'ALL',
	@MessageType VARCHAR(100) = 'ALL'

AS



DECLARE @description varchar
DECLARE @timeZoneID int
IF(@groupID = '-1')
  SELECT @groupID = GroupID FROM Subscribers WHERE SubscriberID = @subscriberId

SELECT @timeZoneID  = TimeZoneID FROM Groups WHERE GroupID = @groupID
SELECT @description = Description FROM TimeZones WHERE TimeZoneID = @timeZoneID
 

SELECT DISTINCT M.MessageID,
    isnull(MR.MessageReplyID,0) as MessageReplyID,
    M.CreatedOn,
	M.PassCode1,
    M.PassCode2,
	IsDepartmentMessage = 0,
    M.SpecialistID,
    SpecialistDisplayName = SU.FirstName + ' ' + SU.LastName,
	SpecialistFirstName = SU.FirstName,
	SpecialistLastName = SU.LastName,
	M.RecipientTypeID,
    M.RecipientID, 
	ReferringPhysicianDisplayName = CASE WHEN M.RecipientID = -1 THEN NSR.RecipientName
							ELSE dbo.fnVoc_GetRecipient(M.RecipientID, M.RecipientTypeID) END,          
    RecipientDisplayName = CASE WHEN M.RecipientID = -1 THEN NSR.RecipientName
							ELSE dbo.fnVoc_GetRecipient(M.RecipientID, M.RecipientTypeID) END, 
	RecipientFirstName  = CASE WHEN M.RecipientID = -1 THEN NSR.RecipientName
						ELSE dbo.fnVoc_GetRecipientFirstName(M.RecipientID, M.RecipientTypeID) END, 
	RecipientLastName = CASE WHEN M.RecipientID = -1 THEN ''
						ELSE dbo.fnVoc_GetRecipientLastName(M.RecipientID, M.RecipientTypeID) END, 		
	GroupTimeZone = @description,
    CreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, @timeZoneID),
    @timeZoneID TimeZoneID,
    PatientVoiceURL,
    ImpressionVoiceURL,
	M.FindingID,
    FindingDescription,
    M.ReadOn,
    ReadOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime( M.ReadOn, @timeZoneID),
    M.ReadBy,
    M.ReadComment,
    MR.CreatedOn as ReplyCreatedOn,
    ReplyCreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR.CreatedOn, @timeZoneID),
    MR.ReadOn as ReplyReadOn,
	ReplyReadOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR.ReadOn, @timeZoneID),
	MR.ReadBy as ReplyReadBy,
	MR.ReadComment as ReplyReadComment,
	MR.ReplyVoiceURL, 
	MRB.ReadbackID,  
	ReadbackCreatedOn = dbo.fnVO_VD_getDateByUsersTime(MRB.CreatedOn, @timeZoneID),  
	MRB.AcceptRejectOn,  
	MRB.Accepted,  
	MRB.Rejected,  
	AcceptRejectOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MRB.AcceptRejectOn, @timeZoneID),  
	MRB.ReadbackVoiceURL,  
	MRB.ReadbackText,  
	M.MRN,
	M.DOB,
	M.Accession,
    EscalationsComplete, 
    ComplianceEscalationComplete,
    SU.RoleID,             
	0 BedNumber,
	DeclinedAt,
	DeclinedMessageVoiceURL,
	M.FacilityID,
	FAC.FacilityDescription FacilityName,
	M.HasCriticalTest HasCriticalTest	
			 
INTO #TMP1
FROM VOC_Messages AS M 
	Left outer join VOC_MessageReplies as MR on M.MessageId = MR.MessageID    
	Left outer join VOC_MessageReadbacks as MRB on M.MessageId = MRB.MessageId 
	LEFT OUTER JOIN VOC_MessagesDeclined AS MD ON MD.MessageID = M.MessageID 
	JOIN Specialists AS S ON S.SpecialistID = M.SpecialistID
	JOIN Subscribers AS SU ON SU.SubscriberID = S.SubscriberID
	JOIN GroupPreferences AS GP ON @groupID = GP.GroupID
	JOIN Voc_Facility AS FAC ON M.FacilityID = FAC.FacilityID
	JOIN Voc_GroupFacility AS GF ON GF.FacilityID = FAC.FacilityID
	--JOIN 	ReferringPhysicians AS RP ON RP.ReferringPhysicianID = M.ReferringPhysicianID
	JOIN Findings AS F ON F.FindingID = M.FindingID  
	LEFT OUTER JOIN VOC_ACC_NonSystemRecipients NSR ON NSR.MessageID = M.MessageId AND NSR.MessageType = 1    
WHERE --M.Active = 1
		SU.GROUPID = @groupID AND
		(
			(M.CreatedOn BETWEEN @startDate AND @endDate + 1) OR 
			(M.AgentCreatedOn BETWEEN @startDate AND @endDate + 1)		
		) AND
	    (
			CASE 
				WHEN @MessageStatus = 'OPEN' AND		 
					(
						 M.ReadOn IS NULL OR
						 (M.ReadOn IS NOT NULL AND 
						  (
							(MR.CreatedOn IS NOT NULL AND MR.ReadOn IS NULL) OR 
							(MRB.CreatedOn IS NOT NULL AND MRB.AcceptRejectOn IS NULL)
						  )
						 )
					) THEN (1)															
				WHEN @MessageStatus = 'CLOSE' THEN
				    (
						CASE 
							WHEN MR.MessageReplyID IS NOT NULL AND (M.ReadOn IS NOT NULL AND MR.CreatedOn IS NOT NULL AND MR.ReadOn IS NOT NULL) THEN (1)
							WHEN MRB.ReadbackID	   IS NOT NULL AND (M.ReadOn IS NOT NULL AND MRB.CreatedOn IS NOT NULL AND MRB.AcceptRejectOn IS NOT NULL) THEN (1)
							WHEN MR.MessageReplyID IS NULL AND MRB.ReadbackID IS NULL AND (M.ReadOn IS NOT NULL ) THEN (1)
						 END 
					 ) 
				WHEN @MessageStatus = 'OPENDECLINED'  AND
					 (
						 M.ReadOn IS NULL AND MD.DeclinedAt IS NOT NULL 
					 ) THEN (1)

				WHEN @MessageStatus = 'OPENEMBARGO'  AND
					 (
						 (
							M.ReadOn IS NULL OR
							 (M.ReadOn IS NOT NULL AND 
							  (
								(MR.CreatedOn IS NOT NULL AND MR.ReadOn IS NULL) OR 
								(MRB.CreatedOn IS NOT NULL AND MRB.AcceptRejectOn IS NULL)
							  )
							 )
						 ) AND
						dbo.fnVO_CST_MessageInEmbargo(M.MessageID, 1) = 1
					) THEN (1)				
				WHEN @MessageStatus = 'ALL' THEN 1

			END = 1
		) AND 					
		(
			CASE 
				WHEN @MessageType = 'CT' AND		 
					(
						 M.HasCriticalTest = 1						 
					) THEN (1)															
				WHEN @MessageType = 'CR' AND		 
					(
						 M.HasCriticalTest = 0						 
					) THEN (1)															
				WHEN @MessageType = 'DOCUMENTONLY' AND		 
					(
						 M.IsDocumented = 1						 
					) THEN (1)															
				WHEN @MessageType = 'READBACK' AND		 
					(
						 MRB.ReadbackID IS NOT NULL AND
						 MR.MessageReplyID IS NULL
						 						 
					) THEN (1)															
				WHEN @MessageType = 'REPLY' AND		 
					(
						 MR.MessageReplyID IS NOT NULL AND
						 MRB.ReadbackID IS NULL
						 						 
					) THEN (1)	
				WHEN @MessageType = 'ALL' THEN 1
														
			END = 1
		) AND		
		M.CreatedOn IS NOT NULL  -- SHOULD THERE BE A MESSAGE 

	ORDER BY M.CreatedOn DESC

	SELECT 	MessageID,
			MessageReplyID,
			CreatedOn,
			PassCode1,   
			PassCode2,  
			IsDepartmentMessage,  
			SpecialistID, 
			SpecialistDisplayName,
			SpecialistFirstName,
			SpecialistLastName,
			RecipientID ,
			RecipientTypeID, 
			RecipientDisplayName,
			RecipientFirstName,
			RecipientLastName,			
			GroupTimeZone,
			CreatedOn_UsersTime,
			TimeZoneID,
			PatientVoiceURL,
			ImpressionVoiceURL,
			FindingID,
			FindingDescription,
			ReadOn,                  
			ReadOn_UsersTime,        
			ReadBy,                                             
			ReadComment,                                                                                                                                                                                                                                                  
			ReplyCreatedOn,          
			ReplyCreatedOn_UsersTime,
			ReplyReadOn,             
			ReplyReadOn_UsersTime,   
			ReplyReadBy,                                        
			ReplyReadComment,                                                                                                                                                                                                                                                
			ReplyVoiceURL,  
			ReadbackID,  
			ReadbackCreatedOn,  
			AcceptRejectOn,  
			Accepted,  
			Rejected,  
			AcceptRejectOn_UsersTime,  
			ReadbackVoiceURL,  
			ReadbackText,                                                                                                                                         
			MRN,
			DOB, 
			Accession,                                               
			EscalationsComplete, 
			ComplianceEscalationComplete, 
			RoleID,
			BedNumber,
			DeclinedAt,
			DeclinedMessageVoiceURL,
			FacilityID,
			FacilityName,
			HasCriticalTest		
	FROM 	#TMP1

	WHERE 	
	 	CASE 
			WHEN ISNULL(@OPhysician, '') = ''   THEN 1
			ELSE CASE WHEN (RecipientFirstName LIKE  @OPhysician + '%'  OR RecipientLastName LIKE @OPhysician + '%') AND RecipientTypeID = 1 THEN 1
				WHEN substring(@OPhysician,1,CHARINDEX( ' ', @OPhysician)) <> '' THEN
					CASE WHEN (RecipientFirstName LIKE + '%' + ltrim(rtrim(substring(@OPhysician,1,CHARINDEX( ' ', @OPhysician)))) + '%' 
						AND RecipientLastName LIKE + '%' + ltrim(rtrim(substring(@OPhysician,CHARINDEX( ' ', @OPhysician)+1,len(@OPhysician)))) + '%') 
						AND RecipientTypeID = 1 THEN 1 ELSE 0 END
				ELSE 0 END
			END = 1
	
	AND	CASE 
			WHEN ISNULL(@RClinician, '') = '' THEN 1
			ELSE CASE WHEN SpecialistFirstName LIKE  @RClinician + '%'  OR SpecialistLastName LIKE @RClinician + '%' THEN 1
				WHEN substring(@RClinician,1,CHARINDEX( ' ', @RClinician)) <> '' THEN
					CASE WHEN (SpecialistFirstName LIKE + '%' + ltrim(rtrim(substring(@RClinician,1,CHARINDEX( ' ', @RClinician)))) + '%' 
						AND SpecialistLastName LIKE + '%' + ltrim(rtrim(substring(@RClinician,CHARINDEX( ' ', @RClinician)+1,len(@RClinician)) + '%'))) 
						THEN 1 ELSE 0 END
				ELSE 0 END
			END = 1
	AND CASE 
			WHEN ISNULL(@ctName, '') = ''   THEN 1
			WHEN (@ctName != '') AND RecipientDisplayName = @ctName THEN 1
			ELSE 0
			END = 1

	AND 	CASE 
			WHEN ISNULL(@PatientName, '') = '' THEN 1
			ELSE CASE WHEN PatientVoiceURL LIKE + '%' + @PatientName + '%' THEN 1 ELSE 0 END
			END = 1
	AND	CASE 
			WHEN ISNULL(@MRN, '') = '' THEN 1
			ELSE CASE WHEN MRN LIKE + '%' + @MRN + '%' THEN 1 ELSE 0 END
			END = 1 
	AND	CASE 
			WHEN ISNULL(@DOB, '') = '' THEN 1
			ELSE CASE WHEN DOB = @DOB THEN 1 ELSE 0 END
			END = 1          
 	AND CASE   
   			WHEN ISNULL(@Accession, '') = '' THEN 1  
   			ELSE CASE WHEN Accession = @Accession THEN 1 ELSE 0 END  
   			END = 1 
	AND 	CASE 
			WHEN ISNULL(@FindingID, 0) = 0 THEN 1
			ELSE CASE WHEN FindingID = @FindingID THEN 1 ELSE 0 END
			END = 1	
	
	AND (	CASE
			WHEN @oPhyID = -1 THEN 1
			ELSE CASE WHEN RecipientID = @oPhyID THEN 1  ELSE 0 END
			END = 1
	)
	AND
			CASE
			WHEN @RecipientTypeID = -1 THEN 1
			ELSE CASE WHEN RecipientTypeID = @RecipientTypeID THEN 1  ELSE 0 END
			END = 1
	AND
			CASE
			WHEN @facilityID = -1 THEN 1
			ELSE CASE WHEN FacilityID = @facilityID THEN 1  ELSE 0 END
			END = 1

	ORDER BY CreatedOn DESC

Go

PRINT 'Created Stored Procedure: dbo.[VOC_VW_getRecentMessagesForSubscriber]'  + CHAR(13)

GO