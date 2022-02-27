Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
/******************************************************************************************************************************************            
**  Name: VOC_VUI_GetMessage            
**  Desc: Sp to get message details            
**  Input Parameters:  @messageID : message ID             
**     @messageID                 
**  Output Parameters:  None            
**  Auth: Ravi S            
**  Date: 21st - Dec - 2006            
**  Change History            
**  Date:			Author:			Description:            
**  --------		--------		-------------------------------------------            
**  19-06-2007		Zeeshan K.		Retrieved DOB Identifier for DOB Enhancement.       
**  10-07-2007		Zeeshan K.		Added Conditions when Backup Message is present, gets Alternate MessageID with either Primary/Backup MessageID      
**  08-08-2007		Zeeshan K.		Added Require Readback by Finding
**  17-10-2007		Zeeshan K.		Added fields required by Outbound Call flows.
**  15-11-2007		Rashmi N.		Added column for voiceoverurl for OC/Deptt.
**  11 Feb 2008		RN				Replaced Inner JOIN by left outer join for perfernces table,
**									did initial assignment for vars.
**  9 April 2008	Rashmi			applied FN to remove spl chars instead of calling multiple replace
**  6 May 2008		Rashmi			CR - RAD VUI Forwarding, added forwarded by information in select list
**  09 June 2008	Jeeshan			Updated SP to get GroupVoiceURL as 'abc.wav' (default) if the value is null or blank
**  4 August 2008	Rashmi			PT Defect # 3532.
**  2 Sept 2008		RAshmi			New vui Logging approach , added institutionID in Select List
**  2 Sept 2008		Jeeshan			New vui Logging approach , added Subscriber in Select List for OB Call logging
**  11 Sept 2008	Rashmi			FR #275 - OC PIN Identifier for CT- Henry Ford - get "PromptForPin"
**  20 Oct 2008		Rashmi			FR - Non Veriphy Recipient - checked for recipient ID as "-1".
**	11-05-2008		Rashmi			FR CT-Unit short name - added columns short name
**	30-03-2009		Raju G			TTP 474 - Updated for Outbound call
**	19-05-2009		Suhas			Rad DB Re-Design changes
**  8 Jun 2009    Prakash	    TTP#65 - Facility Changes - Made change for Preferences
**	19 June 2009  Raju G		PT #155 - Defect Fixed - Get RequireOutboundAcceptence from Group RequireRPAcceptence
** 18 Jul 2009  PKS       TTP#65 - Made Change for using Facility TimeZoneID
**	21-07-2009		Jeeshan			TTP#65- Multisite Facility
**	21-08-2009		Raju G			Remove code to remove ' from PatientVoiceURL
**	10-09-2009		Jeeshan			Multisite facility- message origin single column
**	25-09-2009		Jeeshan			Nebraska Defect # 405 fix
**  12-10-2009		Jeeshan			Nebraska Defect # 467- The forward message comment is not played in the VUI flow. (Added column ForwardCommentURL in select).
*********************************************************************************************************************************************/            
            
CREATE PROCEDURE [dbo].[VOC_VUI_GetMessage]                
                
@messageID INT
AS                  
                  
DECLARE @alternateMessageID int,          
  @recipientTypeID int,
  @isCCBackupActive bit,          
  @primaryMessageID int,          
  @backupMessageID int,
  @instituteID int,
  @nextMessageID int,
  @recipientID int,
  @groupId int,
  @forwardedByID int,  
  @forwardedByContact varchar(15),
  @forwardedByName varchar (100),
  @roleID int,
  @fwdCommentUrl varchar (500)

Select	@primaryMessageID = 0,
		@backupMessageID = 0,
		@alternateMessageID = 0,   
		@isCCBackupActive = 0,
		@forwardedByID = 0,	
		@forwardedByContact = '' ,
		@forwardedByName = '',
		@roleID = 0		

SELECT @recipientTypeID = M.RecipientTypeID FROM VOC_Messages M WHERE M.MessageID = @messageID

	Select  @forwardedByID = ForwardBy				
	From VOC_MessageForward
	Where ForwardMessageID = @messageID				

--	IF (@recipientTypeID = 1)
--	Begin				
		Select @groupId = Su.GroupID, @recipientID = M.RecipientID, @instituteID = G.InstitutionID
		From VOC_Messages M
		INNER JOIN Specialists S ON M.specialistID = S.specialistID And M.MessageID = @messageID
		INNER JOIN Subscribers Su ON Su.subscriberID = 	S.subscriberID
		INNER JOIN Groups G on Su.GroupID = G.GroupID
		
		SET @nextMessageID = (Select Top 1 messageID 
		From VOC_Messages M
		INNER JOIN Specialists S ON M.specialistID = S.specialistID
		INNER JOIN Subscribers Su ON Su.subscriberID = 	S.subscriberID And Su.GroupID = @groupId
		INNER JOIN Findings F ON F.findingID = M.findingID 
		Where M.messageid != @messageID
			And M.RecipientID = @recipientID
			And M.Active = 1
			And M.ReadOn Is Null
			And M.CreatedOn < getdate()
			And M.messageid Not In (Select Messageid from VOC_MessagesDeclined)			
		Order By F.Priority Asc, M.CreatedOn Asc)			


	IF @forwardedByID <> 0 
	BEGIN
		SELECT @roleID = RoleID from voc_Users where VocUserID = @forwardedByID

		IF @roleID = 1 Or @roleID = 2 Or @roleID = 3
		BEGIN	
			SELECT	
				@forwardedByName = Replace(IsNull(FirstName,''),'''',' ') + ' '  + Replace(IsNull(LastName,''),'''',' ') ,
				@forwardedByContact = IsNull(PrimaryPhone,'')
			FROM
				Subscribers AS S
				INNER JOIN Voc_Users U on U.UserID = S.SubscriberID
				LEFT OUTER JOIN Specialists AS SP ON SP.SubscriberID = S.SubscriberID
			WHERE
				U.VocUserID = @forwardedByID
		END				
		ELSE IF @roleID = 10 OR @roleID = 12 OR @roleID = 13 OR @roleID = 14
		BEGIN
			SELECT 			
				@forwardedByName = Replace(IsNull(FirstName,''),'''',' ') + ' '  + Replace(IsNull(LastName,''),'''',' ') ,
				@forwardedByContact = IsNull(Phone,'')	
			FROM 
				VOC_admin_users  AU
				INNER JOIN Voc_Users U on U.UserID = AU.UserID
			WHERE 
			U.VocUserID = @forwardedByID
		END

		SELECT
			@fwdCommentUrl = Note  
		FROM
			Voc_MessageNotes
		WHERE
			MessageId = @messageID   
			AND NoteType = 2  
			AND ( Note LIKE 'http://%.wav' OR Note LIKE 'https://%.wav')  
	END

IF (@recipientTypeID = 1)       
BEGIN        
	SELECT @isCCBackupActive = ClosePrimaryAndBackupMessages           
	FROM GroupPreferences             
	WHERE GroupID = @groupId         

	SELECT @primaryMessageID = PrimaryMessageID FROM VOC_BackupMessages WHERE BackupMessageID = @messageID            
	SELECT @backupMessageID = BackupMessageID FROM VOC_BackupMessages WHERE PrimaryMessageID = @messageID           

/**** Get Alternate MessageID for the Primary/Backup MessageID ****/                    
	IF @isCCBackupActive = '1'          
	BEGIN          
		IF (IsNull(@primaryMessageID,0) != 0)           
		BEGIN          
			SET @alternateMessageID = @primaryMessageID          
		END          
	ELSE IF (IsNull(@backupMessageID,0) != 0)           
		BEGIN          
			SET @alternateMessageID = @backupMessageID          
		END          		           
	END           
        
 /*********   Get All Messages for the MessageID   *********/
   SELECT
   M.MessageID,           
   'AlternateMessageID' = @alternateMessageID,                 
   G.GroupID,                  
   G.ReferringPhysician800Number,
   G.GroupName,
   'GroupVoiceURL' = CASE WHEN G.GroupVoiceURL IS NULL THEN 'abc.wav' ELSE (CASE WHEN ((LTRIM(RTRIM(G.GroupVoiceURL)))='') THEN 'abc.wav' ELSE G.GroupVoiceURL END) END,
   M.PassCode1,                  
   M.PassCode2,    
   M.SpecialistID,                  
   'SpecialistDisplayName' = ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.FirstName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.FirstName) Else SU.FirstName End)) + 
									' ' + 
							ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.LastName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.LastName) Else SU.LastName End)),                        
   'SpecialistNickname' = ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.NickName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.NickName) Else SU.NickName End)), 
   'SpecialistPrimary' = SU.PrimaryPhone,                 
   'SpecialistAffiliation' = S.Affiliation,                  
   'SpecialistVoiceOverURL' = S.VoiceOverURL,                  
   'ReferringPhysicianID' = M.RecipientID,                  
   'ReferringPhysicianDisplayName' = Case When M.RecipientID = -1 Then ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',NSR.RecipientName ) > 0 Then dbo.fnRemoveInvalidCharFromName (NSR.RecipientName) Else NSR.RecipientName End)) 
									Else
									ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',RP.FirstName) > 0 Then dbo.fnRemoveInvalidCharFromName (RP.FirstName) Else RP.FirstName End)) + 
											' ' + 
									ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',RP.LastName) > 0 Then dbo.fnRemoveInvalidCharFromName (RP.LastName) Else RP.LastName End))
									End,  
	'ReferringPhysicianName' =  Case When M.RecipientID = -1 Then NSR.RecipientName   
								Else RP.FirstName + ' ' + RP.LastName End,                  
   'ReferringPhysicianNickname' = ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',RP.NickName) > 0 Then dbo.fnRemoveInvalidCharFromName (RP.NickName) Else RP.NickName End)),                  
   'ReferringPhysicianAffiliation' = RP.Affiliation,                  
   CreatedOn,                  
   LastEscalationNotifyAt,                  
   BackupNotifyStarted,                  
   ComplianceEscalationComplete,                  
   EscalationsComplete,     
   'ForwardedByName' = @forwardedByName,    
   'ForwardedByContact' = @forwardedByContact,         
   'GroupTimeZone' = (SELECT Description FROM TimeZones WHERE TimeZoneID = FAC.TimeZoneID),                  
   'CreatedOn_UsersTime' = dbo.fnVO_VD_getDateByUsersTime(CreatedOn, FAC.TimeZoneID),
    PatientVoiceURL as 'PatientVoiceURL',                  
   ImpressionVoiceURL,                  
   M.FindingID,                  
   'FindingDescription' = Replace(FindingDescription,'''',' '),                  
   FindingVoiceOverURL,                  
   ReadOn,                  
 'IsReadOn' = Case When ReadOn is null  Then 'true'  Else 'false' End ,                  
 'IsFTC' =  Case When (RP.VoiceOverURL is null AND GP.RequireCallBackVoiceOver = 1) Then 'true' When (rtrim(ltrim(RP.VoiceOverURL))  = '' AND GP.RequireCallBackVoiceOver = 1)  Then 'true' Else 'false' End,                   
   'ReadOn_UsersTime' = dbo.fnVO_VD_getDateByUsersTime(ReadOn, FAC.TimeZoneID),
   ReadBy,                  
   'ReadComment' = Replace(ReadComment,'''',' '),                  
  'IsDeclined' = CASE WHEN MD.DeclinedAt is null  THEN 'false' WHEN rtrim(ltrim(MD.DeclinedAt))  = ''  THEN 'false' ELSE 'true'  END,                  
   DeclinedAt,                  
   DeclinedMessageVoiceURL,                  
   'NumberOfReplies' = (SELECT COUNT(*) FROM VOC_MessageReplies WHERE MessageID = M.MessageID),                  
   'ServerTime' = getdate(),                  
   MRN,                
   DOB,              
   M.RequireReadback,                
   IsNull(GP.RequireRPAcceptance,0) as 'RequireRPAcceptance',
   IsNull(GP.RequireRPAcceptance,0) as 'RequireAcceptanceOutbound',
   'RPVoiceOverURL' =  Case When RP.VoiceOverURL is null  Then 'abc.wav' When rtrim(ltrim(RP.VoiceOverURL))  = ''  Then 'abc.wav' When RP.VoiceOverApproved =1  Then RP.VoiceOverURL Else 'abc.wav' End,
   'NextMessageID' = IsNull(@nextMessageID,0),
   'InstitutionID' = @instituteID,
   SU.SubscriberID,
	GP.PromptForPin,
   'IncludeMessageOriginGroup' = CASE WHEN (IncludeMessageOrigin = 0 OR IncludeMessageOrigin = 2) THEN 1 ELSE 0 END,		-- 0-Group; 1-Facility; 2-Both
   'IncludeMessageOriginFacility' = CASE WHEN (IncludeMessageOrigin = 1 OR IncludeMessageOrigin = 2) THEN 1 ELSE 0 END,
    FAC.FacilityDescription,
   'FacilityVoiceOverURL' = CASE WHEN FAC.FacilityVoiceOverURL IS NULL THEN 'abc.wav' ELSE (CASE WHEN ((LTRIM(RTRIM(FAC.FacilityVoiceOverURL)))='') THEN 'abc.wav' ELSE FAC.FacilityVoiceOverURL END) END,
   'ForwardCommentURL' = IsNull(@fwdCommentUrl,'')
   FROM                   
  VOC_Messages AS M                  
  INNER JOIN Specialists AS S ON S.SpecialistID = M.SpecialistID AND M.MessageID = @messageID                   
  INNER JOIN Subscribers AS SU ON SU.SubscriberID = S.SubscriberID                  
  INNER JOIN Groups AS G ON G.GroupID = SU.GroupID AND G.GroupID = @groupId                 
  INNER JOIN ReferringPhysicians AS RP ON RP.ReferringPhysicianID = M.RecipientID                  
  INNER JOIN Findings AS F ON F.FindingID = M.FindingID                  
  LEFT OUTER JOIN GroupPreferences GP ON GP.GroupID = G.GroupId
  LEFT OUTER JOIN VOC_MessagesDeclined AS MD ON MD.MessageID = M.MessageID
  LEFT OUTER JOIN VOC_ACC_NonSystemRecipients NSR ON NSR.MessageID = @messageID AND NSR.MessageType = 1
  LEFT OUTER JOIN VOC_Facility AS FAC ON M.FacilityID = FAC.FacilityID
   WHERE                  
  M.MessageID = @messageID AND M.RecipientTypeID = 1              
  END        
ELSE IF (@recipientTypeID = 4)       
BEGIN        
	SELECT @isCCBackupActive = ClosePrimaryAndBackupMessages           
	FROM GroupPreferences          
	WHERE GroupID=@groupId

	SELECT @primaryMessageID = PrimaryMessageID FROM VOC_BackupMessages WHERE BackupMessageID = @messageID            
	SELECT @backupMessageID = BackupMessageID FROM VOC_BackupMessages WHERE PrimaryMessageID = @messageID           
         
/**** Get Alternate MessageID for the Primary/Backup MessageID ****/           
	IF @isCCBackupActive = '1'          
	BEGIN          
		IF (IsNull(@primaryMessageID,0) != 0)           
		BEGIN          
			SET @alternateMessageID = @primaryMessageID          
		END          
		ELSE IF (IsNull(@backupMessageID,0) != 0)           
		BEGIN          
			SET @alternateMessageID = @backupMessageID          
		END             
	END            
          
    /*********** Get Department Messages *******************************/        
 SELECT                   
   M.MessageID,           
   AlternateMessageID = @alternateMessageID,                 
   G.GroupID,                  
   G.ReferringPhysician800Number,
   G.GroupName,
   'GroupVoiceURL' = CASE WHEN G.GroupVoiceURL IS NULL THEN 'abc.wav' ELSE (CASE WHEN ((LTRIM(RTRIM(G.GroupVoiceURL)))='') THEN 'abc.wav' ELSE G.GroupVoiceURL END) END,
   M.PassCode1,                  
   M.PassCode2,                  
   M.SpecialistID,                  
   'SpecialistDisplayName' = ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.FirstName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.FirstName) Else SU.FirstName End)) + 
									' ' + 
							ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.LastName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.LastName) Else SU.LastName End)),                        
   'SpecialistNickname' = ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.NickName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.NickName) Else SU.NickName End)),             
   'SpecialistPrimary' = SU.PrimaryPhone,                  
   'SpecialistAffiliation' = S.Affiliation,                  
   'SpecialistVoiceOverURL' = S.VoiceOverURL,                  
   'ReferringPhysicianID' = M.RecipientID,        
   'ReferringPhysicianDisplayName' = Replace(DP.DepartmentName,'''',' '),  
	'ReferringPhysicianName' = DP.DepartmentName,	              
   'ReferringPhysicianNickname' = '',                  
   'ReferringPhysicianAffiliation' = '',                  
   CreatedOn,                  
   LastEscalationNotifyAt,                  
   BackupNotifyStarted,                  
   ComplianceEscalationComplete,                  
   EscalationsComplete,   
   'ForwardedByName' = @forwardedByName,    
   'ForwardedByContact' = @forwardedByContact,                        
   'GroupTimeZone' = (SELECT Description FROM TimeZones WHERE TimeZoneID = FAC.TimeZoneID),                  
   'CreatedOn_UsersTime' = dbo.fnVO_VD_getDateByUsersTime(CreatedOn, FAC.TimeZoneID),                  
   PatientVoiceURL as 'PatientVoiceURL',                  
   ImpressionVoiceURL,                  
   M.FindingID,                  
   'FindingDescription' = Replace(FindingDescription,'''',' '),                  
   FindingVoiceOverURL,                  
   ReadOn,                  
   'IsReadOn' = Case When ReadOn is null  Then 'true'  Else 'false' End ,                  
   'IsFTC' =  Case When (DP.VoiceOverURL is null AND GP.RequireCallBackVoiceOver = 1) Then 'true' When ( rtrim(ltrim(DP.VoiceOverURL))  = '' AND GP.RequireCallBackVoiceOver = 1) Then 'true' Else 'false' End,                  
   'ReadOn_UsersTime' = dbo.fnVO_VD_getDateByUsersTime(ReadOn, FAC.TimeZoneID),
   ReadBy,                  
   'ReadComment' = Replace(ReadComment,'''',' '),                  
   'IsDeclined' = CASE WHEN MD.DeclinedAt is null  THEN 'false' WHEN rtrim(ltrim(MD.DeclinedAt))  = ''  THEN 'false' ELSE 'true'  END,                  
   DeclinedAt,                  
   DeclinedMessageVoiceURL,                  
   'NumberOfReplies' = (SELECT COUNT(*) FROM VOC_MessageReplies WHERE MessageID = M.MessageID),                  
   'ServerTime' = getdate(),                  
   MRN,                
   DOB,              
   M.RequireReadback,                
   IsNull(GP.RequireRPAcceptance,0) as 'RequireRPAcceptance',
   IsNull(GP.RequireRPAcceptance,0) as 'RequireAcceptanceOutbound',
   'RPVoiceOverURL' =  Case When DP.VoiceOverURL is null  Then 'abc.wav' When rtrim(ltrim(DP.VoiceOverURL))  = ''  Then 'abc.wav' Else DP.VoiceOverURL End,
   'NextMessageID' = IsNull(@nextMessageID,0),
   'InstitutionID' = @instituteID,
   SU.SubscriberID,
	GP.PromptForPin,
   'IncludeMessageOriginGroup' = CASE WHEN (IncludeMessageOrigin = 0 OR IncludeMessageOrigin = 2) THEN 1 ELSE 0 END,		-- 0-Group; 1-Facility; 2-Both
   'IncludeMessageOriginFacility' = CASE WHEN (IncludeMessageOrigin = 1 OR IncludeMessageOrigin = 2) THEN 1 ELSE 0 END,
   FAC.FacilityDescription,
   'FacilityVoiceOverURL' = CASE WHEN FAC.FacilityVoiceOverURL IS NULL THEN 'abc.wav' ELSE (CASE WHEN ((LTRIM(RTRIM(FAC.FacilityVoiceOverURL)))='') THEN 'abc.wav' ELSE FAC.FacilityVoiceOverURL END) END,
   'ForwardCommentURL' = IsNull(@fwdCommentUrl,'')
   FROM                   
  VOC_Messages AS M                  
  INNER JOIN Specialists AS S ON S.SpecialistID = M.SpecialistID AND M.MessageID = @messageID
  INNER JOIN Subscribers AS SU ON SU.SubscriberID = S.SubscriberID              
  INNER JOIN Groups AS G ON G.GroupID = SU.GroupID AND G.GroupID = @groupId                 
  INNER JOIN VOC_Departments AS DP ON DP.DepartmentID = M.RecipientID                  
  INNER JOIN Findings AS F ON F.FindingID = M.FindingID                  
  LEFT OUTER JOIN GroupPreferences GP ON G.GroupID = GP.GroupID                  
  LEFT OUTER JOIN VOC_MessagesDeclined AS MD ON MD.MessageID = M.MessageID
  LEFT OUTER JOIN VOC_Facility AS FAC ON M.FacilityID = FAC.FacilityID
   WHERE                  
  M.MessageID = @messageID AND M.RecipientTypeID = 4            
  END    

