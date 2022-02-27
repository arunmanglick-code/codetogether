
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'VOC_VW_getRecentMessagesForUnitAdmin')
BEGIN
	DROP  Procedure  VOC_VW_getRecentMessagesForUnitAdmin
END
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[Voc_VW_getRecentMessagesForUnitAdmin]    
  @subscriberID INT,  
  @roleId INT,  
  @isOpenMessages INT = 0,  
  @isUserSetting BIT = '0'  
AS    
    
/******************************************************************************    
**  File: VOC_VW_getRecentMessagesForUnitAdmin.sql    
**  Name: VOC_VW_getRecentMessagesForUnitAdmin    
**  Desc: Gets the recently created messages for the unit admin/charge nurse between given date range.    
**    
**     
**  Called by:       
**                  
**  Parameters:    
**  Input       Output    
**     ----------       -----------    
**    
**  Auth: Swapnil Kurdukar    
**  Date: 25th Apr 2007    
*******************************************************************************    
**  Change History    
*******************************************************************************    
**  Date:   Author:    Description:    
**  --------  --------   -------------------------------------------    
  21 Aug 07  IAK     Added filter of isactive if nurse is selected    
  21 Jan 08  IAK     Filter records if specific unit is provided  
  24 Jan 08  IAK     Where Clause updated defect 2667  
  14 Apr 08  IAK  Column added MessageStatusDateTime, sort order change to MessageStatusDateTime desc  
  16 Apr 08  IAK  Removed Commented Code  
  17 Apr 08  IAK  Sort Order Changed : AllMessages.MessageStatusDateTime DESC, CreatedOn_UsersTime DESC   
  21 Oct 08  RG      Modified to concatenate AgentCreatedOn date with CreatedOn / Handle Non System Recipient Name  
  11/06/2008 SSK  Show Agent name for Specialist Display Name, if it is created on behalf of RC   
** 05/13/2009	  SSK			TTP #172 - Declined message icon
**  15 June 2009	SD			TTP -181, New columns added 

*******************************************************************************/    
   
SET NOCOUNT ON   
  
DECLARE @numberOfDays INT,  
        @institutionID INT,    
  @nurseID INT,  
  @timeZoneID INT,  
  @description VARCHAR(40)  
  
SELECT @numberOfDays = numberOfDays FROM VOC_VLR_UserConfiguration WHERE SubscriberID = @subscriberId    
  
SELECT @institutionID = InstitutionID, @nurseID = UserID FROM Voc_Users WHERE UserID = @subscriberID AND RoleID = @roleID    
  
SELECT @timeZoneID = TimeZoneID FROM Institutions WHERE InstitutionID = @institutionID  
  
SELECT @description = Description FROM TimeZones WHERE TimeZoneID = @timeZoneID    
  
DECLARE @startDate DATETIME,     
  @endDate DATETIME   
  
SET @startDate = DATEADD(day, @numberOfDays, GETDATE())  
SET @endDate = GETDATE()  
  
/* Message Status Icon Variables */  
DECLARE @greenIcon VARCHAR(50), @redIcon VARCHAR(50), @yellowIcon VARCHAR(50),   
 @grayIcon VARCHAR(50), @replyIcon VARCHAR(50), @declinedGreen VARCHAR(50), @declinedRed VARCHAR(50), @declinedGray VARCHAR(50)   
  
SET @redIcon = 'ic_baloon_red'  
SET @greenIcon = 'ic_ballon_green'  
SET @yellowIcon = 'ic_baloon_yellow'  
SET @grayIcon = 'ic_baloon_gray'  
SET @replyIcon = 'icon_reply'  
SET @declinedGreen = 'ic_ballon_green16_declined'
SET @declinedRed = 'ic_ballon_red16_declined'
SET @declinedGray = 'ic_baloon_gray_declined'

    
SELECT * FROM  
(  
SELECT DISTINCT     
    M.MessageID,            
    M.PassCode1,    
    SpecialistDisplayName = CASE WHEN M.VeriphyAgentID > 0 THEN  
           CCU.FirstName + ' ' + CCU.LastName + ' for ' + SU.FirstName + ' ' + SU.LastName  
          ELSE   
           SU.FirstName + ' ' + SU.LastName   
         END,  
    M.RecipientTypeID,    
    M.RecipientID,     
    RecipientDisplayName = CASE WHEN M.RecipientID = -1 THEN (CASE WHEN LEN(NSR.RecipientName) > 25 THEN (SUBSTRING(NSR.RecipientName,0, 25) + '...') ELSE NSR.RecipientName END)  
         ELSE  
        (CASE WHEN LEN(dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn)) > 25 THEN (SUBSTRING(dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn),0, 25) + '...')   
        ELSE dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn) END) + (CASE WHEN (M.RecipientTypeID=3 AND dbo.fnVoc_vl_GetNurseIDFromAssignment (M.RecipientID, M.CreatedOn) > 0)   
        THEN ' (Nurse)' WHEN (M.RecipientTypeID=2 OR M.RecipientTypeID=3) THEN ' (Unit)' WHEN (M.RecipientTypeID=4) THEN ' (Clinical Team)' ELSE '' END)   
         END,          
    NurseID = CASE WHEN RecipientTypeID=3 THEN     
       (    
        dbo.fnVoc_vl_GetNurseIDFromAssignment (M.RecipientID, M.CreatedOn)    
        )    
         ELSE 0 END,     
    CreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, @timeZoneID),    
    OverdueThreshold = NULL,    
    PatientVoiceURL = (CASE WHEN SUBSTRING(M.PatientVoiceURL,1,4) like 'http' THEN '<a href=' + M.PatientVoiceURL + CASE WHEN (@isUserSetting = '1') THEN '>Play</a>' ELSE '><img src=img/ic_play_msg.gif border=0></a>' END ELSE M.PatientVoiceURL END),      
    
    ImpressionVoiceURL,    
    FindingDescription = (SELECT FindingDescription FROM Findings AS F WHERE F.FindingID = M.FindingID),  
    ReadOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime( M.ReadOn, @timeZoneID),    
    MR.MessageReplyID,    
    ReplyCreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR.CreatedOn, @timeZoneID),    
    MR.ReplyVoiceURL,            
    ReplyReadOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR.ReadOn, @timeZoneID),    
    ReadbackID = NULL,    
    ReadbackCreatedOn_UsersTime = NULL,        
    AcceptRejectOn_UsersTime = NULL,    
    ReadbackVoiceURL = NULL,   
	VF.FacilityID,	
	ISNULL(VF.FacilityDescription, '') AS FacilityDescription,
    M.EscalationsComplete,
    M.ComplianceEscalationComplete,


  -- For embargoed messages
	MessageState = 
            (CASE  WHEN (M.ComplianceEscalationComplete = 1) THEN 4 
                  WHEN (M.EscalationsComplete = 1) THEN 3 
                  WHEN (M.BackupnotifyStarted = 1) THEN 2 
                  ELSE 1 
            END),
      Stage = 
            ( SELECT SequenceID  
              FROM NotificationEventsSequence  
              WHERE EventDescription  =  
                (
                  SELECT TOP 1   
                  CASE  WHEN (CHARINDEX ('Message closed by',EventDescription) > 0) THEN 'Message closed'    
                        WHEN (CHARINDEX ('A result has been documented by',EventDescription) > 0)   THEN 'Documented Message'
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
                      'Clinial Team Notification',
                      'OP - VUI'
                    )  
                  ORDER BY NotificationHistoryID DESC    
                )
            ),
      SecondStage = 
            ( SELECT SequenceID  
              FROM NotificationEventsSequence  
              WHERE EventDescription  =  
              (
                SELECT TOP 1 col1 FROM 
                (
                  SELECT  TOP 2  NotificationHistoryID,
                  CASE  WHEN (CHARINDEX ('Message closed by',EventDescription) > 0) THEN 'Message closed'    
                        WHEN (CHARINDEX ('A result has been documented by',EventDescription) > 0)   THEN 'Documented Message'
                        WHEN (CHARINDEX ('-Outbound call',EventDescription) > 0 OR 
                              CHARINDEX ('-Partners Page',EventDescription) > 0 OR 
                              CHARINDEX ('-Unit Fax Notification Sent to',EventDescription) > 0 OR 
                              CHARINDEX ('-Clinical Team Fax Notification Sent to',EventDescription) > 0)
                                  THEN SUBSTRING (EventDescription, 1, CHARINDEX('-',EventDescription) - 1)    
                        ELSE EventDescription  
                  END  as col1 
                  FROM VOC_VL_NotificationHistories NH  
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
                        'Clinial Team Notification',
                        'OP - VUI'
                      )  
                  ORDER BY NotificationHistoryID DESC
                ) aa 
                ORDER BY NotificationHistoryID
              )
            ),
      dbo.fnVO_CST_MessageInEmbargo(M.MessageID,3) AS IsEmbargo,  


    BedNumber = CASE WHEN IsNull(RB.BedNumber,'') <> '' THEN    
        RB.RoomNumber + ': ' + RB.BedNumber    
      ELSE    
        RB.RoomNumber    
      END,    
    SU.RoleID,     
    (SELECT COUNT(*) FROM dbo.voc_vl_messagelabresults ML WHERE ML.MessageID=M.MessageID) AS LabResultCount,        
    'UnitID' = CASE WHEN (M.RecipientTypeID = 2) THEN M.RecipientID ELSE RB.UnitID END,      
    0 AS [IsDepartmentMessage],    
    M.IsDocumented,  
    MessageIcon = CASE WHEN @isUserSetting = '0' THEN   
 (CASE WHEN M.IsDocumented = '1' THEN @yellowIcon   
  WHEN (MR.MessageReplyID > 0 ) THEN (CASE WHEN (M.ReadOn IS NULL AND MR.CreatedOn IS NOT NULL AND MR.ReadOn IS NULL) THEN @greenIcon ELSE @grayIcon END)  
  WHEN M.ReadOn IS NULL THEN (CASE WHEN EscalationsComplete = '1' OR ComplianceEscalationComplete = '1' THEN (CASE WHEN (MD.MessageID > 0) THEN @declinedRed ELSE @redIcon END) ELSE (CASE WHEN (MD.MessageID > 0) THEN @declinedGreen ELSE @greenIcon END) END)  ELSE   
   (CASE WHEN (MD.MessageID > 0) THEN @declinedGray ELSE @grayIcon END) END) ELSE  
 (CASE WHEN M.IsDocumented = '1' THEN '<b>D</b>'  
  WHEN MR.MessageReplyID > 0 THEN   
   (CASE WHEN MR.ReadOn IS NULL THEN '<br><br>r' ELSE 'x<br><br>r' END)  
  WHEN M.ReadOn IS NULL THEN (CASE WHEN (EscalationsComplete = '1' OR ComplianceEscalationComplete = '1') THEN (CASE WHEN (MD.MessageID > 0) THEN '<b>DO</b>' ELSE '<b>O</b>' END) ELSE (CASE WHEN (MD.MessageID > 0) THEN '<b>DO</b>' ELSE '' END) END) ELSE (CASE WHEN (MD.MessageID > 0) THEN 'DC' ELSE 'x' END) END)  
  END,
 MessageReplyIcon = (CASE WHEN MR.MessageReplyID > 0 THEN @replyIcon ELSE NULL END),  
    MessageStatus = CASE WHEN M.ReadOn IS NOT NULL THEN 'Closed' ELSE dbo.fnVoc_DateTimeDifference(M.CreatedOn) END +   
  CASE WHEN MR.MessageReplyID IS NOT NULL THEN (CASE WHEN MR.ReadOn IS NOT NULL THEN '<br><br> Reply Closed' ELSE '<br><br>' +  
  dbo.fnVoc_DateTimeDifference(M.CreatedOn) END) ELSE '' END,  
 MessageStatusDateTime =     
    CASE   
     WHEN (@isOpenMessages <> 4) THEN    
      CASE   
       WHEN M.ReadOn IS NULL THEN M.CreatedOn --Original Message Open               
       WHEN M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NULL THEN MR.CreatedOn   --Original Message Closed but reply open  
       WHEN M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NOT NULL THEN '1/1/1960' --Original Message Closed and reply also Closed     
       WHEN M.ReadOn IS NOT NULL THEN  '1/1/1960'        
       ELSE '1/1/1960'                   
      END  
     ELSE '1/1/1960'  
    END        
 FROM  Voc_VL_Messages AS M    
  LEFT OUTER JOIN  Voc_Facility  AS VF ON VF.FacilityID = M.FacilityID 
  Left outer join Voc_VL_MessageReplies as MR on M.MessageId = MR.MessageID    
  LEFT OUTER JOIN VOC_VL_MessageDeclined as MD on M.MessageId = MD.MessageId
  JOIN  Voc_VL_Units AS UN ON UN.InstitutionID = @institutionID    
  JOIN  Voc_VL_UnitRoomBedDetails AS URB ON URB.UnitID = UN.UnitID     
  JOIN Institutions INS ON UN.InstitutionID = INS.InstitutionID    
  JOIN Specialists AS S ON S.SpecialistID = M.SpecialistID    
  JOIN Subscribers AS SU ON SU.SubscriberID = S.SubscriberID    
  JOIN Findings AS F ON F.FindingID = M.FindingID    
  Left outer join Voc_VL_UnitRoomBedDetails RB ON RB.RoomBedID = M.RoomBedID     
  Left outer join Voc_VL_Units UN1 ON UN1.UnitID = RB.UnitID   
  LEFT OUTER JOIN VOC_ACC_NonSystemRecipients NSR ON NSR.MessageID = M.MessageId AND NSR.MessageType = 3    
  LEFT OUTER JOIN VOC_ACC_CallCenter_Users AS CCU ON CCU.UserID = M.VeriphyAgentID     
 WHERE M.Active = 1 AND         
    (M.RecipientID = UN.UnitID or M.RecipientID = URB.RoomBedID)      
  AND ( M.ReadOn IS NULL      
  OR  (MR.CreatedOn Is not NULL AND MR.ReadOn Is NULL AND M.ReadOn is not NULL   
  AND ((M.CreatedOn BETWEEN @startDate AND @endDate) OR (M.AgentCreatedOn BETWEEN @startDate AND @endDate))))      
  AND (M.RecipientTypeID = 3 OR M.RecipientTypeID = 2)    
) MsgData  
WHERE   
  (   @isOpenMessages = -1 AND MsgData.unitID IN (  
   SELECT DISTINCT UN.UnitID FROM Voc_VL_Units UN    
   JOIN  Voc_VL_UnitNurses AS UNN ON UNN.UnitID = UN.UnitID  AND NurseID = @nurseID   
   WHERE InstitutionID = @institutionID And IsActive = 1)) OR (@isOpenMessages <> -1 AND MsgData.unitID = @isOpenMessages)  
ORDER BY MessageStatusDateTime DESC, CreatedOn_UsersTime DESC  


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
