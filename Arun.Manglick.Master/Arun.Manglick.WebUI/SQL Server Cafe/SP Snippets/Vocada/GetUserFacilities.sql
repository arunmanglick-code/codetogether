IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[VOC_CST_getUserFacilities]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[VOC_CST_getUserFacilities]
GO

/******************************************************************************          
**  File: VOC_CST_getUserFacilities.sql          
**  Name: VOC_CST_getUserFacilities        
**  Desc: Gets the Facilities for a subscriber/radiologist  
**  Called by:         
**                     
**  Auth: Jeeshan Kazi  
**  Date: 01-July-2009  
*******************************************************************************          
**  Change History          
*******************************************************************************          
**  Date:   Author:    Description:          
**  --------  --------   -------------------------------------------          
*******************************************************************************/           
    
CREATE PROCEDURE [dbo].[VOC_CST_getUserFacilities]  
	@vocUserID INT
AS  
BEGIN  
	DECLARE @subscriberID INT  
	DECLARE @roleID INT  

	SELECT @subscriberID = UserID FROM Voc_Users WHERE VocUserID = @vocUserID

	SELECT @roleID = RoleID FROM Subscribers WHERE SubscriberID = @subscriberID  
	
	SELECT   
		FacilityDescription,  
		F.FacilityID,  
		'EnableAgentTeam' = CASE WHEN (SELECT CallCenterID FROM Groups WHERE GroupID IN (SELECT GroupID FROM Subscribers WHERE SubscriberID = @subscriberID)) > 0 THEN   
		F.EnableAgentTeam ELSE '0' END,  
		F.EnableCriticalTest  
	FROM  Voc_GroupFacility AS GF   
		INNER JOIN Voc_Facility AS F  
		ON GF.FacilityID = F.FacilityID   
	WHERE   
		GF.GroupID = (SELECT GroupID FROM Subscribers WHERE SubscriberID = @subscriberID)   
		AND GF.IsActive = 1  
		AND CASE WHEN (@roleId <> 2 AND @roleId <> 8)THEN 1  
			ELSE  ( CASE WHEN ( (SELECT SubscriberID FROM Voc_GroupAdminFacility  
								 WHERE SubscriberID = @subscriberId AND   
									FacilityID = F.FacilityID) IS NOT NULL )THEN 1  
			ELSE 0 END )  
			END = 1  
	ORDER BY FacilityDescription  
END

GO
