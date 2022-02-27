IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'VOC_VW_getMessagesForSubscriber')
BEGIN
	DROP  Procedure  VOC_VW_getMessagesForSubscriber
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/************************************************************************************************************************************        
**  File: VOC_VW_getMessagesForSubscriber.sql        
**  Name: VOC_VW_getMessagesForSubscriber        
**  Desc: Gets the recently created messages for the logged in user between given date range based on its role for Message Center in Veriphy Web.        
**        
**  Date: 16 Apr 2007        
*****************************************************************************************************************************************        
**  Change History        
*******************************************************************************        
**  Date:			Author:		Description:        
**  --------		--------	-------------------------------------------        
**  20 June 2007	IAK			Selection of nurse ID Defect 1150        
**  16 July 2007	IAK			Select Department messages also        
**  21 Aug  2007	IAK			Added filter of isactive if nurse is selected        
**  11 Jan  2007	IAK			Retrive Read msg as per user level configutarion
**  11 Jan  2007	IAK			Remove readback readon filter
**  16 Jan  2007	IAK			Readback accepted declined  and origional msg read the gray icon for origional msg
**  16 Jan  2007	IAK			Readback open  and origional msg read the gray icon for origional msg need to be view in open only msg
**  14 Apr  2008	IAK			Column added MessageStatusDateTime, sort order change to MessageStatusDateTime desc
**  16 Apr  2008	IAK			Removed Commented Code
**  17 Apr  2008	IAK			Sort order Changed: AllMessages.MessageStatusDateTime DESC, CreatedOn_UsersTime DESC 
**	17 Sept	2008	SSK			Added option to Show the messages sent by the agent
**	21 Oct 2008		RG			Modified to concatenate AgentCreatedOn date with CreatedOn / Handle Non System Recipient Name
**	11/06/2008		SSK			Show Agent name for Specialist Display Name, if it is created on behalf of RC	
**  05 May 2009		Suhas		Rad DB re-design Changes
**	05/04/2009		SSK			TTP #172 - Declined message icon
**  15 June 2009	SD			TTP -181, flter option changed & multifacility changes done

** Parameter        
** @Subscriberid INT-         
** @Startdate DATETIME        
** @endDate DATETIME        
** @roleId INT        
*******************************************************************************/      
CREATE PROCEDURE [dbo].[VOC_VW_getMessagesForSubscriber]      
  @subscriberId INT,  
  @roleId INT,  
  @isOpenMessages INT = 0,  
  @isUserSetting BIT = '0'  
AS        
BEGIN        
  SET NOCOUNT ON   

  DECLARE @numberOfDays INT,  
          @groupID INT,  
          @timeZoneID INT,  
          @description VARCHAR(40)  

  SELECT @numberOfDays = numberOfDays FROM VOC_VLR_UserConfiguration WHERE SubscriberID = @subscriberId    
  SELECT @groupID = GroupID FROM Subscribers WHERE SubscriberID = @subscriberId        
  SELECT @timeZoneID  = TimeZoneID FROM Groups WHERE GroupID = @groupID     
  SELECT @description = Description FROM TimeZones WHERE TimeZoneID = @timeZoneID    

  DECLARE @startDate DATETIME, @endDate DATETIME   
  SET @startDate = DATEADD(day, -@numberOfDays, GETDATE())  
  SET @endDate = GETDATE()  

  /* Message Status Icon Variables */  
  DECLARE @greenIcon VARCHAR(50), @redIcon VARCHAR(50), @yellowIcon VARCHAR(50), @grayIcon VARCHAR(50), 
          @replyIcon VARCHAR(50), @declinedGreen VARCHAR(50), @declinedRed VARCHAR(50), @declinedGray VARCHAR(50)

  SET @redIcon = 'ic_baloon_red'  
  SET @greenIcon = 'ic_ballon_green'  
  SET @yellowIcon = 'ic_baloon_yellow'  
  SET @grayIcon = 'ic_baloon_gray'  
  SET @replyIcon = 'icon_reply'  
  SET @declinedGreen = 'ic_ballon_green16_declined'
  SET @declinedRed = 'ic_ballon_red16_declined'
  SET @declinedGray = 'ic_baloon_gray_declined'

  IF @roleId = 1 OR @roleId = 2        
  BEGIN        
       --Get all open messages and all msgs of given date        
      SELECT * FROM        
      (        
        SELECT DISTINCT 
        M.MessageID,        
        M.PassCode1,
        SpecialistDisplayName = CASE  WHEN M.VeriphyAgentID > 0 THEN 
                                          CCU.FirstName + ' ' + CCU.LastName + ' for ' + SU.FirstName + ' ' + SU.LastName
                                      ELSE SU.FirstName + ' ' + SU.LastName 
                                END,
        M.RecipientTypeID,        
        RecipientID = M.RecipientID,  
        RecipientDisplayName = 
            (CASE WHEN M.RecipientID = -1 THEN NSR.RecipientName 
                  ELSE (  CASE  WHEN LEN(dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn)) > 25 
                                    THEN (SUBSTRING(dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn),0, 25) + '...')   
                                ELSE dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn) 
                          END) + 
                       (  CASE  WHEN (M.RecipientTypeID =3 AND dbo.fnVoc_vl_GetNurseIDFromAssignment (M.RecipientID, M.CreatedOn) > 0)   
                                    THEN ' (Nurse)' 
                                WHEN (M.RecipientTypeID=2 OR M.RecipientTypeID=3) THEN ' (Unit)' 
                                WHEN (M.RecipientTypeID=4) THEN ' (Clinical Team)' 
                                ELSE '' 
                          END) 
            END),          
        NurseID = NULL,
        CreatedOn_UsersTime = Dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, @timeZoneID),
        GP.OverdueThreshold,        
        PatientVoiceURL = 
            (CASE WHEN SUBSTRING(M.PatientVoiceURL,1,4) like 'http' 
                      THEN '<a href=' + M.PatientVoiceURL + 
                            CASE  WHEN (@isUserSetting = '1') THEN '>Play</a>' 
                                  ELSE '><img src=img/ic_play_msg.gif border=0></a>' 
                            END 
                  ELSE M.PatientVoiceURL 
            END),
        ImpressionVoiceURL,        
        FindingDescription = (SELECT FindingDescription FROM Findings AS F WHERE F.FindingID = M.FindingID), 
        ReadOn_UsersTime = Dbo.fnVO_VD_getDateByUsersTime(M.ReadOn, @timeZoneID),
        --reply        
        MR.MessageReplyID,        
        ReplyCreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR.CreatedOn, @timeZoneID),        
        MR.ReplyVoiceURL,
        ReplyReadOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR.ReadOn, @timeZoneID),
        -- Readback        
        MR1.ReadbackID,        
        ReadbackCreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR1.CreatedOn, @timeZoneID),
        AcceptRejectOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR1.AcceptRejectOn, @timeZoneID),        
        MR1.ReadbackVoiceURL,
        -- Lab Bed Number        
        BedNumber = NULL,        
        SU.RoleID,         
        LabResultCount = NULL,        
        UnitID = NULL,
        F.FacilityID,
        ISNULL(F.FacilityDescription, '') AS FacilityDescription,
        M.EscalationsComplete,
        M.ComplianceEscalationComplete,
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
                  FROM Voc_NotificationHistories  
                  WHERE MessageID = M.MessageID AND
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
                )
              ),
        SecondStage = 
              ( SELECT SequenceID  
                FROM NotificationEventsSequence  
                WHERE EventDescription  =  
                (
                  SELECT TOP 1 col1 
                  FROM 
                  (
                      SELECT  TOP 2  
                        NotificationHistoryID,
                        CASE  WHEN (CHARINDEX ('Message closed by',EventDescription) > 0) THEN 'Message closed'    
                              WHEN (CHARINDEX ('A result has been documented by',EventDescription) > 0)   THEN 'Documented Message'
                              WHEN (CHARINDEX ('-Outbound call',EventDescription) > 0 OR 
                                    CHARINDEX ('-Partners Page',EventDescription) > 0 OR 
                                    CHARINDEX ('-Unit Fax Notification Sent to',EventDescription) > 0 OR 
                                    CHARINDEX ('-Clinical Team Fax Notification Sent to',EventDescription) > 0)
                                      THEN SUBSTRING (EventDescription, 1, CHARINDEX('-',EventDescription) - 1)    
                              ELSE EventDescription  
                              END  as col1 
                      FROM Voc_NotificationHistories  
                      WHERE MessageID = M.MessageID AND
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
                  ) aa 
                  ORDER BY NotificationHistoryID
                )
              ),
        (dbo.fnVO_CST_MessageInEmbargo(M.MessageID,1)) as IsEmbargo,
        CASE WHEN M.RecipientTypeID = 4 THEN 1 ELSE 0 END AS [IsDepartmentMessage],       
        M.IsDocumented,  
        MessageIcon = 
            (CASE  WHEN @isUserSetting = '0' THEN   
                    (CASE  WHEN M.IsDocumented = '1' THEN @yellowIcon   
                            WHEN (MR.MessageReplyID > 0 OR MR1.ReadbackID > 0) THEN 
                                (CASE WHEN ((MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NULL) OR 
                                            (MR1.ReadbackID IS NOT NULL AND MR1.AcceptRejectOn IS NULL)) THEN @greenIcon 
                                      ELSE @grayIcon 
                                 END)  
                            WHEN M.ReadOn IS NULL THEN 
                                (CASE WHEN EscalationsComplete = '1' OR ComplianceEscalationComplete = '1' THEN 
                                          (CASE WHEN (MD.MessageID > 0) THEN @declinedRed 
                                                ELSE @redIcon 
                                          END) 
                                      ELSE 
                                          (CASE WHEN (MD.MessageID > 0) THEN @declinedGreen 
                                                ELSE @greenIcon 
                                          END) 
                                END) 
                            ELSE   
                                (CASE WHEN (MD.MessageID > 0) THEN @declinedGray 
                                      ELSE @grayIcon 
                                END) 
                    END) 
                  ELSE  
                     (CASE  WHEN M.IsDocumented = '1' THEN '<b>D</b>'  
                            WHEN (MR.MessageReplyID > 0 OR MR1.ReadbackID > 0) THEN   
                                (CASE WHEN (MR.ReadOn IS NULL OR MR1.AcceptRejectOn IS NULL) THEN   
                                        (CASE WHEN MR.MessageReplyID > 0 THEN '<br><br>r' 
                                              ELSE '<br><br>R' 
                                        END) 
                                      ELSE 
                                        (CASE WHEN MR.MessageReplyID > 0 THEN 'x<br><br>r' 
                                              ELSE 'x<br><br>R' 
                                        END) 
                                END)  
                            WHEN M.ReadOn IS NULL THEN 
                                (CASE WHEN (EscalationsComplete = '1' OR ComplianceEscalationComplete = '1') THEN 
                                        (CASE WHEN (MD.MessageID > 0) THEN '<b>DO</b>' 
                                              ELSE '<b>O</b>' 
                                        END) 
                                      ELSE 
                                        (CASE WHEN (MD.MessageID > 0) THEN '<b>DO</b>' ELSE '' END) 
                                END) 
                            ELSE 
                                (CASE WHEN (MD.MessageID > 0) THEN 'DC' ELSE 'x' END) 
                      END)  
            END),  
        MessageReplyIcon = (CASE WHEN (MR.MessageReplyID > 0 OR MR1.ReadbackID > 0) THEN @replyIcon ELSE NULL END),  
        MessageStatus = 
            CASE  WHEN M.ReadOn IS NOT NULL THEN 'Closed' 
                  ELSE dbo.fnVoc_DateTimeDifference(M.CreatedOn) 
            END +   
            (CASE WHEN MR.MessageReplyID IS NOT NULL THEN 
                    (CASE WHEN MR.ReadOn IS NOT NULL THEN '<br><br> Reply Closed' 
                          ELSE '<br><br>' + dbo.fnVoc_DateTimeDifference(MR.CreatedOn) 
                    END) 
                  ELSE '' 
            END) +   
            CASE  WHEN MR1.ReadbackID IS NOT NULL THEN   
                    (CASE WHEN MR1.AcceptRejectOn IS NOT NULL THEN 
                            (CASE WHEN MR1.Accepted='1' THEN '<br><br> Readback Accepted' 
                                  WHEN MR1.Rejected='1' THEN '<br><br> Readback Rejected' 
                            END)   
                          ELSE '<br><br>' + dbo.fnVoc_DateTimeDifference(MR1.CreatedOn) 
                    END) 
                  ELSE '' 
            END,
        MessageStatusDateTime =   
            CASE  WHEN (@isOpenMessages <> 4) THEN  
                    CASE  WHEN M.ReadOn IS NULL 
                                THEN M.CreatedOn --Original Message Open   									 
                          WHEN M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NULL 
                                THEN MR.CreatedOn   --Original Message Closed but reply open
                          WHEN M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NOT NULL 
                                THEN '1/1/1960' --Original Message Closed and reply also Closed   
                          WHEN M.ReadOn IS NOT NULL AND MR1.ReadbackID IS NOT NULL AND MR1.AcceptRejectOn IS NULL 
                                THEN MR1.CreatedOn  --Original Message Closed and readback Open     
                          WHEN M.ReadOn IS NOT NULL AND MR1.ReadbackID IS NOT NULL AND MR1.AcceptRejectOn IS NOT NULL 
                                THEN '1/1/1960' --Original Message Closed and readback also Closed     
                          WHEN M.ReadOn IS NOT NULL 
                                THEN  '1/1/1960' 					
                          ELSE '1/1/1960' 															 
                    END
                  ELSE '1/1/1960'
            END      
        FROM  VOC_Messages AS M
        LEFT OUTER JOIN  Voc_Facility  AS F ON F.FacilityID = M.FacilityID
        INNER JOIN  Specialists AS S ON S.SpecialistID = M.SpecialistID        
        INNER JOIN  Subscribers AS SU ON SU.SubscriberID = S.SubscriberID        
        INNER JOIN GroupPreferences AS GP ON @groupID = GP.GroupID        
        INNER JOIN  ReferringPhysicians AS RP ON RP.ReferringPhysicianID = M.RecipientTypeID        
        LEFT OUTER JOIN VOC_MessageReplies as MR on M.MessageId = MR.MessageId  
        LEFT OUTER JOIN VOC_MessagesDeclined as MD on M.MessageId = MD.MessageId       
        LEFT OUTER JOIN VOC_MessageReadbacks as MR1 on M.MessageId = MR1.MessageId       
        LEFT OUTER JOIN VOC_ACC_WorklistMessageForward as WF on M.MessageId = WF.MessageId AND WF.MessageTypeId = 1
        LEFT OUTER JOIN VOC_ACC_NonSystemRecipients NSR ON NSR.MessageID = M.MessageId AND NSR.MessageType = 1  
        LEFT OUTER JOIN VOC_ACC_CallCenter_Users AS CCU ON CCU.UserID = M.VeriphyAgentID			
        WHERE  M.Active = 1 
        AND SU.GROUPID = @groupID --AND SU.SubscriberID = @subscriberId  
        AND M.CreatedOn <= GETDATE() 
        AND ((
                M.ReadOn IS NULL 
                OR 
                (
                  M.ReadOn IS NOT NULL  AND 
                  ((M.CreatedOn BETWEEN @startDate AND @endDate) OR (M.AgentCreatedOn BETWEEN @startDate AND @endDate))
                )
            )) 

		AND CASE WHEN (
						@roleId <> 2 AND @roleId <> 8
					   )THEN 1
				ELSE  (
						CASE WHEN (
											(SELECT SubscriberID FROM Voc_GroupAdminFacility
												WHERE SubscriberID = @subscriberId AND 
											FacilityID = F.FacilityID) IS NOT NULL
								  											
								 )THEN 1
							 ELSE 0					
						END 
					 )
			 END = 1

        AND
            CASE  WHEN (
                          @isOpenMessages = 0 
                          AND
                          ( 
                            M.ReadOn IS NULL OR
                            (M.ReadOn IS NOT NULL AND 
                              (
                                (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL) OR 
                                (MR1.CreatedOn IS NOT NULL AND MR1.AcceptRejectOn Is NULL)
                              )
                            )
                          )
                        ) THEN 1 --ALL OPEN
                  WHEN (
                          @isOpenMessages = 1 
                          AND
                          ( 
                            M.ReadOn IS NULL OR
                            (M.ReadOn IS NOT NULL AND 
                              (
                                (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL) OR 
                                (MR1.CreatedOn IS NOT NULL AND MR1.AcceptRejectOn Is NULL)
                              )
                            )
                          )
                          AND M.EscalationsComplete = 1 AND M.ComplianceEscalationComplete = 0
                        ) THEN 1 --END ESCALTION
                  WHEN (
                          @isOpenMessages = 2
                          AND
                          ( 
                            M.ReadOn IS NULL OR
                            (M.ReadOn IS NOT NULL AND 
                              (
                                (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL) OR 
                                (MR1.CreatedOn IS NOT NULL AND MR1.AcceptRejectOn Is NULL)
                              )
                            )
                          )
                          AND M.ComplianceEscalationComplete = 1 
                        ) THEN 1 --PAST COMP
                  WHEN (
                          @isOpenMessages = 3
                          AND
                          ( 
                            M.ReadOn IS NULL OR
                            (M.ReadOn IS NOT NULL AND 
                              (
                                (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL) OR 
                                (MR1.CreatedOn IS NOT NULL AND MR1.AcceptRejectOn Is NULL)
                              )
                            )
                          )
                          AND MD.MessageID IS NOT NULL
                        ) THEN 1 --DECLINED
                  WHEN (
                          @isOpenMessages = 4
                          AND
                          ( 
                            M.ReadOn IS NULL OR
                            (M.ReadOn IS NOT NULL AND 
                              (
                                (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL) OR 
                                (MR1.CreatedOn IS NOT NULL AND MR1.AcceptRejectOn Is NULL)
                              )
                            )
                          )
                          AND dbo.fnVO_CST_MessageInEmbargo(M.MessageID, 1) = 1
                        ) THEN 1 --EMBARGOED
                  WHEN (
                          @isOpenMessages = 5 AND
      		                (
                            (M.ReadOn IS NOT NULL AND (DATEDIFF(Hour, M.ReadOn, getdate())) <= 24)  
                             OR  
                            (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NOT NULL AND (DATEDIFF(Hour, MR.ReadOn, getdate())) <= 24)
                              OR  
                            (MR1.CreatedOn IS NOT NULL AND MR1.AcceptRejectOn Is NOT NULL AND (DATEDIFF(Hour, MR1.AcceptRejectOn, getdate())) <= 24)
                          )
                        ) THEN 1 --CLOSED IN LAST 24 HRS
                  ELSE 0
            END = 1 
      )AllMessages 
      ORDER BY AllMessages.MessageStatusDateTime DESC, CreatedOn_UsersTime DESC    
  END        
  ELSE        
  BEGIN        
       --Get all open messages and all msgs of given date        
      SELECT DISTINCT 
      M.MessageID,        
      M.PassCode1,        
      SpecialistDisplayName = CASE  WHEN M.VeriphyAgentID > 0 THEN
                                        CCU.FirstName + ' ' + CCU.LastName + ' for ' + SU.FirstName + ' ' + SU.LastName
                                    ELSE SU.FirstName + ' ' + SU.LastName 
                              END,
      RecipientTypeID = M.RecipientTypeID,        
      RecipientID = dbo.fnVoc_vl_GetRecipientID(M.RecipientID,RecipientTypeID,M.CreatedOn),        
      RecipientDisplayName = 
          (CASE WHEN M.RecipientID = -1 THEN NSR.RecipientName 
                ELSE (  CASE  WHEN LEN(dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn)) > 25 
                                  THEN (SUBSTRING(dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn),0, 25) + '...')   
                              ELSE dbo.fnVoc_vl_GetRecipient(M.RecipientID,RecipientTypeID,M.CreatedOn) 
                        END) + 
                     (  CASE  WHEN (M.RecipientTypeID=3 AND dbo.fnVoc_vl_GetNurseIDFromAssignment (M.RecipientID, M.CreatedOn) > 0)   
                                  THEN ' (Nurse)' 
                              WHEN (M.RecipientTypeID=2 OR M.RecipientTypeID=3) THEN ' (Unit)' 
                              WHEN (M.RecipientTypeID=4) THEN ' (Clinical Team)' 
                              ELSE '' 
                          END) 
          END),          
      NurseID = CASE  WHEN RecipientTypeID=3 THEN  (dbo.fnVoc_vl_GetNurseIDFromAssignment (M.RecipientID, M.CreatedOn))        
                      ELSE 0 
                END,         
      CreatedOn_UsersTime = Dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, @timeZoneID),          
      OverdueThreshold = NULL,        
      PatientVoiceURL = 
          (CASE WHEN SUBSTRING(M.PatientVoiceURL,1,4) like 'http' 
                      THEN '<a href=' + PatientVoiceURL + 
                          CASE  WHEN (@isUserSetting = '1') THEN '>Play</a>' 
                                ELSE '><img src=img/ic_play_msg.gif border=0></a>' 
                          END 
                ELSE M.PatientVoiceURL 
          END),          
      ImpressionVoiceURL,        
      FindingDescription = (SELECT FindingDescription FROM Findings AS F WHERE F.FindingID = M.FindingID),        
      ReadOn_UsersTime = Dbo.fnVO_VD_getDateByUsersTime(M.ReadOn, @timeZoneID),        
      --reply        
      MR.MessageReplyID,        
      ReplyCreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR.CreatedOn, @timeZoneID),        
      MR.ReplyVoiceURL,        
      ReplyReadOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(MR.ReadOn, @timeZoneID),        
      -- Readback        
      ReadbackID = NULL,        
      ReadbackCreatedOn_UsersTime = NULL,        
      AcceptRejectOn_UsersTime = NULL,        
      ReadbackVoiceURL = NULL,        
      ReadbackText = NULL,        
      -- Lab Bed Number        
      /* To display Bed in the format RoomNumber: BedNumber on Message Center page */          
      BedNumber = 
          CASE  WHEN IsNull(RB.BedNumber,'') <> '' THEN RB.RoomNumber + ': ' + RB.BedNumber        
                ELSE RB.RoomNumber        
          END,        
      SU.RoleID,         
      (SELECT COUNT(ML.MessageID) FROM dbo.voc_vl_messagelabresults ML WHERE ML.MessageID=M.MessageID) AS LabResultCount,        
      RB.UnitID,
      F.FacilityID,
      ISNULL(F.FacilityDescription, '') AS FacilityDescription,
      M.EscalationsComplete,
      M.ComplianceEscalationComplete,
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
      CASE WHEN M.RecipientTypeID = 4 THEN 1 ELSE 0 END AS [IsDepartmentMessage],        
      M.IsDocumented,  
      MessageIcon = 
          CASE  WHEN @isUserSetting = '0' THEN   
                    (CASE WHEN M.IsDocumented = '1' THEN @yellowIcon   
                          WHEN MR.MessageReplyID > 0 THEN 
                              (CASE WHEN MR.ReadOn IS NULL THEN @greenIcon ELSE @grayIcon END)  
                          WHEN M.ReadOn IS NULL THEN 
                              (CASE WHEN EscalationsComplete = '1' OR ComplianceEscalationComplete = '1' THEN 
                                      (CASE WHEN (MD.MessageID > 0) THEN @declinedRed ELSE @redIcon END) 
                                    ELSE 
                                      (CASE WHEN (MD.MessageID > 0) THEN @declinedGreen ELSE @greenIcon END) 
                              END) 
                          ELSE   
                              (CASE WHEN (MD.MessageID > 0) THEN @declinedGray ELSE @grayIcon END) 
                    END) 
                ELSE  
                    (CASE WHEN M.IsDocumented = '1' THEN '<b>D</b>'  
                          WHEN MR.MessageReplyID > 0 THEN   
                              (CASE WHEN MR.ReadOn IS NULL THEN '<br><br>r' ELSE 'x<br><br>r' END)  
                          WHEN M.ReadOn IS NULL THEN 
                              (CASE WHEN (EscalationsComplete = '1' OR ComplianceEscalationComplete = '1') THEN 
                                        (CASE WHEN (MD.MessageID > 0) THEN '<b>DO</b>' ELSE '<b>O</b>' END) 
                                    ELSE 
                                        (CASE WHEN (MD.MessageID > 0) THEN '<b>DO</b>' ELSE '' END) 
                              END) 
                          ELSE 
                              (CASE WHEN (MD.MessageID > 0) THEN 'DC' ELSE 'x' END) 
                    END)  
          END,
      MessageReplyIcon = (CASE WHEN MR.MessageReplyID > 0 THEN @replyIcon ELSE NULL END),  
      MessageStatus = 
          CASE  WHEN M.ReadOn IS NOT NULL THEN 'Closed' 
                ELSE dbo.fnVoc_DateTimeDifference(M.CreatedOn) 
          END +   
          (CASE WHEN MR.MessageReplyID IS NOT NULL THEN 
                    (CASE WHEN MR.ReadOn IS NOT NULL THEN '<br><br> Reply Closed' 
                          ELSE '<br><br>' + dbo.fnVoc_DateTimeDifference(MR.CreatedOn) 
                    END) 
                ELSE '' 
          END),
      MessageStatusDateTime =   
          CASE WHEN (@isOpenMessages <> 4) THEN  
                  CASE  WHEN M.ReadOn IS NULL 
                              THEN M.CreatedOn --Original Message Open   									 
                        WHEN M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NULL 
                              THEN MR.CreatedOn   --Original Message Closed but reply open
                        WHEN M.ReadOn IS NOT NULL AND MR.MessageReplyID IS NOT NULL AND MR.ReadOn IS NOT NULL 
                              THEN '1/1/1960' --Original Message Closed and reply also Closed   
                        WHEN M.ReadOn IS NOT NULL 
                              THEN  '1/1/1960' 					
                        ELSE '1/1/1960' 															 
                  END
               ELSE '1/1/1960'
          END        
      FROM  Voc_VL_Messages AS M         
      INNER JOIN  Specialists AS S ON S.SpecialistID = M.SpecialistID
      LEFT OUTER JOIN  Voc_Facility  AS F ON F.FacilityID = M.FacilityID        
      INNER JOIN  Subscribers AS SU ON SU.SubscriberID = S.SubscriberID      
      LEFT OUTER JOIN Voc_VL_MessageReplies as MR on M.MessageId = MR.MessageID 
      LEFT OUTER JOIN VOC_VL_MessageDeclined as MD on M.MessageId = MD.MessageId         
      LEFT OUTER JOIN Voc_VL_UnitRoomBedDetails RB ON M.RoomBedID = RB.RoomBedID        
      LEFT OUTER JOIN VOC_ACC_WorklistMessageForward as WF on M.MessageId = WF.MessageId AND WF.MessageTypeId = 2 
      LEFT OUTER JOIN VOC_ACC_NonSystemRecipients NSR ON NSR.MessageID = M.MessageId AND NSR.MessageType = 3 
      LEFT OUTER JOIN VOC_ACC_CallCenter_Users AS CCU ON CCU.UserID = M.VeriphyAgentID			
      WHERE  M.Active = 1 
      AND SU.GROUPID = @groupID --AND SU.SubscriberID = @subscriberId  
      AND M.CreatedOn <= GETDATE()  
      AND ((
              M.ReadOn IS NULL 
              OR 
              (
                M.ReadOn IS NOT NULL  AND 
                ((M.CreatedOn BETWEEN @startDate AND @endDate) OR (M.AgentCreatedOn BETWEEN @startDate AND @endDate))
              )
          ))
	 
      AND CASE WHEN (
						@roleId <> 2 AND @roleId <> 8
					   )THEN 1
				ELSE  (
						CASE WHEN (
											(SELECT SubscriberID FROM Voc_GroupAdminFacility
												WHERE SubscriberID = @subscriberId AND 
											FacilityID = F.FacilityID) IS NOT NULL
								  											
								 )THEN 1
							 ELSE 0					
						END 
					 )
			 END = 1
 
      AND
          CASE  WHEN (
                        @isOpenMessages = 0 
                          AND
                          ( 
                            M.ReadOn IS NULL OR
                            (M.ReadOn IS NOT NULL AND (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL))
                          )
                      ) THEN 1
                WHEN (
                        @isOpenMessages = 1 
                        AND
                        ( 
                          M.ReadOn IS NULL OR
                          (M.ReadOn IS NOT NULL AND (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL))
                        )
                        AND M.EscalationsComplete = 1 AND M.ComplianceEscalationComplete = 0
                      ) THEN 1
                WHEN (
                        @isOpenMessages = 2
                        AND
                        ( 
                          M.ReadOn IS NULL OR
                          (M.ReadOn IS NOT NULL AND (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL))
                        )
                        AND M.ComplianceEscalationComplete = 1 
                      ) THEN 1
                WHEN (
                        @isOpenMessages = 3
                        AND
                        ( 
                          M.ReadOn IS NULL OR
                          (M.ReadOn IS NOT NULL AND (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL))
                        )
                        AND MD.MessageID IS NOT NULL
                      ) THEN 1
                WHEN (
                        @isOpenMessages = 4
                        AND
                        ( 
                          M.ReadOn IS NULL OR
                          (M.ReadOn IS NOT NULL AND (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NULL))
                        )
                        AND dbo.fnVO_CST_MessageInEmbargo(M.MessageID, 3) = 1
                      ) THEN 1
                WHEN (
                        @isOpenMessages = 5
                        AND
		                    (
                          (M.ReadOn IS NOT NULL AND (DATEDIFF(Hour, M.ReadOn, getdate())) <= 24)  
		                      OR  
                          (MR.CreatedOn IS NOT NULL AND MR.ReadOn Is NOT NULL AND (DATEDIFF(Hour, MR.ReadOn, getdate())) <= 24)
                        ) 
                      ) THEN 1
                ELSE 0
        END = 1
      ORDER BY MessageStatusDateTime DESC, CreatedOn_UsersTime DESC
  END   
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO