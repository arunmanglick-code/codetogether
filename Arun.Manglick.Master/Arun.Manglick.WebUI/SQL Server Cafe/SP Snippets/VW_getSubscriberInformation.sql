set ANSI_NULLS ON
set QUOTED_IDENTIFIER OFF
GO
  
/******************************************************************************
**		File: VOC_VW_getSubscriberInformation.sql
**		Name: VOC_VW_getSubscriberInformation
**		Desc: Retrieves the subscriber info by subscriberId 
**
**		This template can be customized:
**              
**		Return values: Version number.
** 
**		Called by:   
**              
**		Parameters:
**		Input							Output
**     ----------							-----------
**
**		Auth: Indrajeet A. Ketkale
**		Date: 02.02.2007
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			-------------------------------------------
		23 Feb 07	IAK					Added isnull constrain while retriving records
		01 Mar 07	IAK					New Stored procedure
		05 Mar 07	IAK					GroupPreferences join added
		13 Mar 07	IAK					Added IsActive filter
		16 Apr 07	RS					Modified condition for Roleid 1, 2, 3
		15 May 07	IAK					Added Column VOCUserID
		25 Aug 07	IAK					Added DirectoryID
		17 June 08	Suhas				Added Condition for Agent Role 15,16,17
		21 Jul 08	ZK					Added for Allow Lab Test Filtering preferences.		
		21 Jul 08	ZK					Added for Allow Directory Synchronization preferences.			
		21 Jul 08	ZK					Print Statement added
        17 Oct 08	Suhas				Added Condition for Agent Role 18
        24 Oct 08   Suhas				Added "Removed" filter for Agent Roles
       11 Mar 09	Sheetal				Changed for amcom pager URL
		20 Mar 09	SD					Added Amcom pager for live agent also.
		17 June 09	AM					Added AllowCriticalRuleManagement
*******************************************************************************/
ALTER PROCEDURE [dbo].[VOC_VW_getSubscriberInformation]
@SubscriberID INT,
@RoleID INT
AS
BEGIN
  DECLARE @AllowDownload AS BIT
  SET @AllowDownload = 0
  IF (@RoleID = 2 OR @RoleID = 5 OR @RoleID = 8)
    SELECT @AllowDownload = 1

  DECLARE @NotAllowDirectorySync AS BIT  
  SET @NotAllowDirectorySync = 0  

  /* If Roles are 5 = Unit Admin 
				  6 =	Charge Nurse
  */	
  IF(@RoleID = 5 OR @RoleID = 6)
	  BEGIN
		  SELECT
		    USR.VOCUserID,	
		    NRS.NurseID AS [SubscriberID],
		    0 AS [GroupID],
		    '' AS [GroupName],
		    0 AS [DirectoryID],	
		    INS.InstitutionID,
		    INS.InstitutionName,
			AmcomPagerURL = (SELECT Top 1 ISNULL(AmcomPagerURL, '') FROM VOC_Facility WHERE InstitutionID = INS.InstitutionID),
		    USR.LoginID,
		    USR.Password,
		    USR.RoleID,
		    USR.Active,
		    ROL.RoleDescription, 
		    ISNULL(NRS.FirstName, '') AS [FirstName],
		    ISNULL(NRS.LastName, '') AS [LastName],
		    ISNULL(SPC.SpecialistID, 0) AS [SpecialistID],
		    @AllowDownload AS AllowDownload,	
		    @NotAllowDirectorySync As AllowDirectorySynchronization,
			AllowCriticalRuleManagement = 0
		    FROM
			  VOC_VL_Nurses AS NRS
			  LEFT OUTER JOIN Specialists AS SPC ON SPC.SubscriberID = NRS.NurseID
			  INNER JOIN VOC_Users AS USR ON USR.UserID = NRS.NurseID
			  INNER JOIN Roles AS ROL ON ROL.RoleID = USR.RoleID
			  INNER JOIN Institutions AS INS ON INS.InstitutionID =  USR.InstitutionID
		    WHERE
			  NRS.NurseID = @SubscriberID AND ROL.RoleID = @RoleID AND NRS.Active = 1
	  END
  ----------------------------------------------------------------------------------------
  IF(@RoleID = 1 OR @RoleID = 2 OR @RoleID = 3 OR @RoleID = 4 OR @RoleID = 8)
	  /* If Roles are 
					  1 = Specialists (RC)
					  2 = Group Admin
					  3 = Group Representation
					  4 = Lab Technician
					  8 = Lab Group Admin
	  */
	  BEGIN
		  SELECT
		    USR.VOCUserID,		  
		    SUB.SubscriberID,
		    GRP.GroupID,
		    GRP.GroupName,
		    GRP.DirectoryID,
		    INS.InstitutionID,
		    INS.InstitutionName,
		    AmcomPagerURL = (SELECT Top 1 ISNULL(AmcomPagerURL, '') FROM VOC_Facility WHERE FacilityID IN (SELECT FacilityID FROM VOC_GroupFacility WHERE GroupID = GRP.GroupID)),
		    USR.LoginID,
		    USR.Password,
		    USR.RoleID,
		    USR.Active,
		    ROL.RoleDescription, 
		    ISNULL(SUB.FirstName, '') AS [FirstName],
		    ISNULL(SUB.LastName, '') AS [LastName],
		    ISNULL(SPC.SpecialistID, 0) AS [SpecialistID],
		    @AllowDownload AS [AllowDownload],
			AllowDirectorySynchronization = (SELECT Top 1 EnableDirectorySynchronization FROM VOC_Facility WHERE FacilityID IN (SELECT FacilityID FROM VOC_GroupFacility WHERE GroupID = GRP.GroupID)),
--			AllowCriticalRuleManagement = (SELECT Count(EnableCriticalTest) FROM VOC_Facility WHERE EnableCriticalTest = 1 AND FacilityID IN (SELECT FacilityID FROM VOC_GroupFacility WHERE GroupID = SUB.GroupID)),
--			AllowCriticalRuleManagement = (SELECT Count(EnableCriticalTest) FROM VOC_Facility WHERE EnableCriticalTest = 1 AND FacilityID IN 
--											(SELECT GFAC.FacilityID FROM VOC_GroupFacility GFAC 
--											INNER JOIN Voc_GroupAdminFacility GADMINFAC ON GFAC.FacilityID = GADMINFAC.FacilityID 
--											WHERE GADMINFAC.SubscriberID = @SubscriberID AND GFAC.GroupID = SUB.GroupID)
--										 ),
			AllowCriticalRuleManagement = (SELECT Count(EnableCriticalTest) FROM VOC_Facility FAC
											INNER JOIN VOC_GroupFacility GFAC ON FAC.FacilityID = GFAC.FacilityID
											INNER JOIN Voc_GroupAdminFacility GADMFAC ON FAC.FacilityID = GADMFAC.FacilityID
											WHERE GADMFAC.SubscriberID = @SubscriberID AND GroupID = SUB.GroupID AND EnableCriticalTest = 1
											)


			
		    FROM
			  Subscribers AS SUB
			  LEFT OUTER JOIN Specialists AS SPC ON SPC.SubscriberID = SUB.SubscriberID
			  INNER JOIN VOC_Users AS USR ON USR.UserID = SUB.SubscriberID
			  INNER JOIN Groups AS GRP ON GRP.GroupID = SUB.GroupID
			  INNER JOIN Roles AS ROL ON ROL.RoleID = USR.RoleID
			  INNER JOIN Institutions AS INS ON INS.InstitutionID =  USR.InstitutionID
			  LEFT OUTER JOIN GroupPreferences AS GPF ON GPF.GroupID = GRP.GroupID
  			  INNER JOIN VOC_Facility AS FAC ON FAC.InstitutionID =  USR.InstitutionID
			  	
		    WHERE
			  SUB.SubscriberID = @SubscriberID AND ROL.RoleID = @RoleID AND SUB.Active = 1
	  END	
----------------------------------------------------------------------------------------
  IF(@RoleID = 15 OR @RoleID = 16 OR @RoleID = 17 OR @RoleID = 18)  
	  -- Agent Roles
	  --15	Live Agent
	  --16	Power Agent
	  --17	Agent Admin
    --18  Desktop Agent
	  BEGIN  
		  SELECT      USR.VOCUserID,      
					  CU.UserID AS [SubscriberID],  
					  0 AS [GroupID],  
					  '' AS [GroupName],  
					  0 AS [DirectoryID],   
					  ACC.InstitutionID,  
					  I.InstitutionName As [InstitutionName], 
					  AmcomPagerURL = (SELECT Top 1 ISNULL(AmcomPagerURL, '') FROM VOC_Facility WHERE InstitutionID = I.InstitutionID),
					  USR.LoginID,  
					  USR.Password,  
					  USR.RoleID,  
					  USR.Active,  
					  R.RoleDescription,   
					  ISNULL(CU.FirstName, '') AS [FirstName],  
					  ISNULL(CU.LastName, '') AS [LastName],  
					  0 AS [SpecialistID],  
					  @AllowDownload AS [AllowDownload],
					  @NotAllowDirectorySync  AS AllowDirectorySynchronization,
					  AllowCriticalRuleManagement = 0
					  FROM  VOC_ACC_CallCenter_Users AS CU INNER JOIN VOC_Users AS USR ON CU.UserID = USR.UserID 
					  INNER JOIN VOC_ACC_CallCenters ACC ON ACC.CallCenterID = CU.CallCenterID
					  INNER JOIN Roles AS R ON USR.RoleID = R.RoleID 
					  INNER JOIN Institutions I ON I.InstitutionID = ACC.InstitutionID
		  WHERE       CU.UserID = @SubscriberID AND USR.Active=1 AND ACC.IsActive=1 AND CU.Removed = 0 AND USR.RoleID in (15,16,17,18)  
            AND 1 = (SELECT Top 1 EnableAgentTeam FROM VOC_Facility WHERE InstitutionID = I.InstitutionID)
	  END
----------------------------------------------------------------------------------------

END

