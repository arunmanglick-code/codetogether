Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

 /******************************************************************************************************************************************    
**  Name: VOC_VUI_getUnreadDeclinedMessagesForSpecialist    
**  Desc: Sp to mark readback as accept or reject     
**  Input Parameters:  @specialistID         
**  Output Parameters:  None    
**  Auth: Rashmi N    
**  Date: 15th - Jan - 2007    
**  Change History    
**  Date:   Author:    Description:    
**  --------  --------   -------------------------------------------   
**  9th April 2008  Rashmi		applied fn fnRemoveInvalidCharFromName to remove special char from columns 
**  29 april 2008	Rashmi		changed order by clause to use alias instead of column name to support level 90
**  20 Oct 2008		Rashmi		FR - Non Veriphy Recipient - checked for recipient ID as "-1".
**	18 May 2009		Suhas		Rad DB Re-Design changes
** 18 Jul 2009  PKS       TTP#65 - Made Change for using Facility TimeZoneID
**	21-08-2009		Raju G			Remove code to remove ' from PatientVoiceURL
**	25-09-2009		Jeeshan		FindingDescription Single Quote Issue
**	25-09-2009		Jeeshan		FindingDescription Single Quote Issue
*********************************************************************************************************************************************/    
    
CREATE PROCEDURE [dbo].[VOC_VUI_getUnreadDeclinedMessagesForSpecialist]     
  @specialistID int    
AS    
    
 Select 'MessageDeclinedCount'   = (Select Count(MD.MessageID)   
 FROM VOC_MessagesDeclined AS MD     
  INNER JOIN VOC_Messages AS M  ON M.SpecialistID = @specialistID     
     AND M.MessageID = MD.MessageID     
     AND MD.DelinedMessageReadOn IS NULL    
  INNER JOIN Specialists S ON M.SpecialistID = S.SpecialistID  ) 
    
 Select MD.MessageID,    
  dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, FAC.TimeZoneID) as 'CreatedOn',    
  dbo.fnVO_VD_getDateByUsersTime(MD.DeclinedAt, FAC.TimeZoneID) as 'DeclinedAt',    
  'OrderingClinician' = Case When M.RecipientID = -1 Then ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',NSR.RecipientName ) > 0 Then dbo.fnRemoveInvalidCharFromName (NSR.RecipientName) Else NSR.RecipientName End)) 
									Else
									ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',R.FirstName) > 0 Then dbo.fnRemoveInvalidCharFromName (R.FirstName) Else R.FirstName End)) + 
											' ' + 
									ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',R.LastName) > 0 Then dbo.fnRemoveInvalidCharFromName (R.LastName) Else R.LastName End))
									End,   
  'NickName' = ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',R.NickName) > 0 Then dbo.fnRemoveInvalidCharFromName (R.NickName) Else R.NickName End)),    
  Replace(IsNull (' ' + R.Specialty,''),'''',' ')  as 'Specialty',    
  REPLACE(IsNull(FindingDescription, ''), '''', '') as 'FindingDescription',
  IsNull(F.FindingVoiceOverURL,'') as 'FindingVoiceOverURL',    
  IsNull (M.PatientVoiceURL,'') as 'PatientVoiceURL',        
  CASE WHEN M.MRN IS NULL THEN '' ELSE M.MRN END as PatientMRN,    
  M.DOB as 'PatientDOB',    
  'ReportingClinician' = ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.FirstName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.FirstName) Else SU.FirstName End)) + 
  						' ' + 
						ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.LastName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.LastName) Else SU.LastName End)) +
						' ' + 
						ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.NickName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.NickName) Else SU.NickName End)),    
IsNull(S.VoiceoverURL,'') as 'ReportingClinicianVoiceURL',     
  Case When IsNull(R.VoiceOverApproved,0) != 0 Then IsNull(R.VoiceOverURL,'') Else '' End   as 'OrderingClinicianVoiceURL',    
  IsNull(MD.DeclinedMessageVoiceURL,'') as 'DeclinedMessageVoiceURL'  ,  
  'IsDeptMsg' = 0  
 FROM VOC_MessagesDeclined AS MD     
  INNER JOIN VOC_Messages AS M  ON M.SpecialistID = @specialistID     
      AND M.MessageID = MD.MessageID     
      AND MD.DelinedMessageReadOn IS NULL    
  INNER JOIN ReferringPhysicians R ON M.RecipientID = R.ReferringPhysicianID    
  INNER JOIN Specialists S ON M.SpecialistID = S.SpecialistID    
  INNER JOIN Subscribers SU ON S.SubscriberID = SU.SubscriberID    
  INNER JOIN Findings F ON M.FindingID = F.FindingID     
  INNER JOIN Groups AS G ON SU.GroupID = G.GroupID    
  LEFT OUTER JOIN VOC_ACC_NonSystemRecipients NSR ON NSR.MessageID = M.MessageID AND NSR.MessageType = 1   
  LEFT OUTER JOIN VOC_Facility AS FAC ON M.FacilityID = FAC.FacilityID
 WHERE 
	M.RecipientTypeID = 1  
   
UNION  
 Select MD.MessageID,    
  dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, FAC.TimeZoneID) as 'CreatedOn',    
  dbo.fnVO_VD_getDateByUsersTime(MD.DeclinedAt, FAC.TimeZoneID) as 'DeclinedAt',    
  Replace(D.DepartmentName,'''',' ') as 'OrderingClinician',    
  ''  as 'NickName',    
  ''  as 'Specialty',    
  REPLACE(IsNull(FindingDescription, ''), '''', '') as 'FindingDescription',
  IsNull(F.FindingVoiceOverURL,'') as 'FindingVoiceOverURL',    
  IsNull (M.PatientVoiceURL,'') as 'PatientVoiceURL',        
  CASE WHEN M.MRN IS NULL THEN '' ELSE M.MRN END as PatientMRN,    
  M.DOB as 'PatientDOB',  
  'ReportingClinician' = ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.FirstName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.FirstName) Else SU.FirstName End)) + 
  						' ' + 
						ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.LastName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.LastName) Else SU.LastName End)) +
						' ' + 
						ltrim(rtrim(Case  When patindex('%[^a-zA-Z0-9 ]%',SU.NickName) > 0 Then dbo.fnRemoveInvalidCharFromName (SU.NickName) Else SU.NickName End)),    
  IsNull(S.VoiceoverURL,'') as 'ReportingClinicianVoiceURL',     
  IsNull(D.VoiceOverURL,'') as 'OrderingClinicianVoiceURL',    
  IsNull(MD.DeclinedMessageVoiceURL,'') as 'DeclinedMessageVoiceURL'  ,  
  'IsDeptMsg' = 1  
 FROM VOC_MessagesDeclined AS MD     
  INNER JOIN VOC_Messages AS M  ON M.SpecialistID = @specialistID     
      AND M.MessageID = MD.MessageID     
      AND MD.DelinedMessageReadOn IS NULL    
  INNER JOIN VOC_Departments D ON D.DepartmentID = M.RecipientID   
  INNER JOIN Specialists S ON M.SpecialistID = S.SpecialistID    
  INNER JOIN Subscribers SU ON S.SubscriberID = SU.SubscriberID    
  INNER JOIN Findings F ON M.FindingID = F.FindingID     
  INNER JOIN Groups AS G ON SU.GroupID = G.GroupID
  LEFT OUTER JOIN VOC_Facility AS FAC ON M.FacilityID = FAC.FacilityID   
 WHERE 
	M.RecipientTypeID = 4

  ORDER BY DeclinedAt DESC    



