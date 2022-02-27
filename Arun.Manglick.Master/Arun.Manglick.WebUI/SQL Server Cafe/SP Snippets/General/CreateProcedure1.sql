IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[VOC_CTM_getCriticalRules]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[VOC_CTM_getCriticalRules]

PRINT 'Dropped Procedure [dbo].[VOC_CTM_getCriticalRules]'

GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [VOC_CTM_getCriticalRules] 
	@instituteId int = 0,
	@groupId int = 0
AS
BEGIN
   
SELECT RuleID,Precedence, RuleName,FAC.FacilityDescription Facility, FIND.FindingDescription Finding,RUL.Active
FROM [VOC_CTM_Rules] AS RUL
INNER JOIN [VOC_Facility] as FAC ON RUL.FacilityID = FAC.FacilityID
INNER JOIN [Findings] as FIND ON RUL.FindingID = FIND.FindingID
WHERE RUL.InstitutionID = @instituteId AND
	  RUL.GroupID = @groupId AND
	  RUL.Active = 1

SELECT RuleID,Precedence, RuleName, 
CASE 
	WHEN FAC.FacilityDescription IS NULL THEN '' 
	ELSE FAC.FacilityDescription 
END Facility, 
CASE 
	WHEN FIND.FindingDescription IS NULL THEN '' 
	ELSE FIND.FindingDescription 
END Finding,
RUL.Active
FROM [VOC_CTM_Rules] AS RUL
LEFT OUTER JOIN [VOC_Facility] as FAC ON RUL.FacilityID = FAC.FacilityID
LEFT OUTER JOIN [Findings] as FIND ON RUL.FindingID = FIND.FindingID
WHERE RUL.InstitutionID = @instituteId AND
	  RUL.GroupID = @groupId AND
	  RUL.Active = 1

SELECT  RuleID,Precedence, RuleName, 
		IsNull(FAC.FacilityDescription, '')	Facility,
		IsNull(FIND.FindingDescription, '')	Finding, 
		RUL.Active
FROM [VOC_CTM_Rules] AS RUL
LEFT OUTER JOIN [VOC_Facility] as FAC ON RUL.FacilityID = FAC.FacilityID
LEFT OUTER JOIN [Findings] as FIND ON RUL.FindingID = FIND.FindingID
WHERE RUL.InstitutionID = @instituteId AND
	  RUL.GroupID = @groupId AND
	  RUL.Active = 1

END
GO

PRINT 'Create Procedure [dbo].[VOC_CTM_getCriticalRules]'
GO

