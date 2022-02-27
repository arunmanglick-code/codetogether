IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[VOC_CTM_updateCriticalRule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[VOC_CTM_updateCriticalRule]

PRINT 'DROPPED Procedure [dbo].[VOC_CTM_updateCriticalRule]'

GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************  
**  File: VOC_CTM_updateCriticalRule.sql  
**  Name: VOC_CTM_updateCriticalRule  
**  Desc: Update a Critical Rule Information
**  
**  This template can be customized:  
**                
**  Return values:   
**   
**  Called by:     
**                
**  Parameters:  
**  Input       Output  
**  
**  
**  Auth: Arun M
**  Date: 11 May 2009
*******************************************************************************  
**  Change History  
*******************************************************************************  
**  Date:			  Author:     Description:  
**  --------		--------	  -------------------------------------------  
*******************************************************************************/  

CREATE PROCEDURE [VOC_CTM_updateCriticalRule] 

@ruleId int = 0,
@instituteId int = 0,
@groupId int = 0,
@ruleName varchar(100) = '',
@precedence bigint = 0,
@facilityId int = 0,
@findingId int = 0,
@tatOrderPlacedToComplete int = 0,
@tatProcCompleteToMsgSent int = 0,
@tatMsgSentToMsgReceived int = 0,
@tatOrderPlacedToMsgReceived int = 0,
@updatedBy int = 0,
@ruleParameters XML,
@ruleDeviceList XML

AS

DECLARE @updatedOn DateTime
SET @updatedOn = getdate()

DECLARE @tblRuleParameters TABLE 
( 
	[RuleID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[OperatorID] [int] NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[MeasureUnitID] [int] NULL,
	[RuleParameterID] [int] NOT NULL
)

DECLARE @tblRuleDeviceList TABLE 
( 
	[RuleID] [int] NOT NULL,
	[GroupNotificationID] [int] NOT NULL
)

BEGIN
   
	 UPDATE [VOC_CTM_Rules]
	 SET
	    RuleName = @ruleName,
	    Precedence = @precedence,
	    FacilityID = @facilityId,
	    FindingID = @findingId,
	    TAT_OrderPlacedToComplete = @tatOrderPlacedToComplete,
	    TAT_ProcCompleteToMsgSent = @tatProcCompleteToMsgSent,
	    TAT_MsgSentToMsgReceived = @tatMsgSentToMsgReceived,
	    TAT_OrderPlacedToMsgReceived = @tatOrderPlacedToMsgReceived,
		UpdatedOn = @updatedOn,
		[UpdatedBy] = @updatedBy		

	WHERE	RuleID = @ruleId AND 
			InstitutionID = @instituteId AND 
			GroupID = @groupId

	---------------------------------------------------------

	INSERT INTO @tblRuleParameters ( [RuleID], [ParameterID], [OperatorID], [Value], [MeasureUnitID],[RuleParameterID]) (
	SELECT 
		@ruleId,
		ExternalInformation.Details.query('ParameterID').value('.', 'INTEGER') AS [ParameterID],
		ExternalInformation.Details.query('OperatorID').value('.', 'INTEGER') AS [OperatorID],
		ExternalInformation.Details.query('Value').value('.', 'VARCHAR(100)') AS [Value],
		ExternalInformation.Details.query('MeasureUnitID').value('.', 'INTEGER') AS [MeasureUnitID],
		ExternalInformation.Details.query('RuleParameterID').value('.', 'INTEGER') AS [RuleParameterID]
	FROM
		@ruleParameters.nodes('/DocumentElement/RuleParameters') AS ExternalInformation([Details]) )
	
	
	
	UPDATE VOC_CTM_RuleParameters
	SET
		[ParameterID] = TEMP.[ParameterID],
		[OperatorID] = TEMP.[OperatorID],
		[Value] = TEMP.[Value],
		[MeasureUnitID] = TEMP.[MeasureUnitID]
	FROM [VOC_CTM_RuleParameters] AS ORIG INNER JOIN @tblRuleParameters AS TEMP 
		 ON ORIG.[RuleParameterID] = TEMP.[RuleParameterID]
	WHERE ORIG.[RuleID] = @ruleId AND TEMP.[ParameterID] <> 0

	INSERT INTO [VOC_CTM_RuleParameters]
           ([RuleID]
           ,[ParameterID]
           ,[OperatorID]
           ,[Value]
           ,[MeasureUnitID])
	SELECT [RuleID],[ParameterID],[OperatorID],[Value],[MeasureUnitID]
	FROM @tblRuleParameters
	WHERE [RuleParameterID] = 0 AND [ParameterID] <> 0

	

	DELETE FROM VOC_CTM_RuleParameters
	WHERE [RuleID] = @ruleId AND
		  [RuleParameterID] IN (SELECT [RuleParameterID] 
								FROM @tblRuleParameters 
								WHERE [ParameterID] = 0
								)

	
	----------------------------------------------------------
	INSERT INTO @tblRuleDeviceList ( [RuleID], [GroupNotificationID]) (
	SELECT 
		@ruleId,
		ExternalInformation.Details.query('GroupNotificationID').value('.', 'INTEGER') AS [GroupNotificationID]
	FROM
		@ruleDeviceList.nodes('/DocumentElement/RuleAssignDevicesList') AS ExternalInformation([Details]) )
	
	
--	DELETE FROM [VOC_CTM_RuleNotificationDevices]
--	WHERE [RuleID] IN ( 
--						SELECT [RuleID] FROM @tblRuleDeviceList
--					   )

	DELETE FROM [VOC_CTM_RuleNotificationDevices]
	WHERE [RuleID] = @ruleId

	INSERT INTO [VOC_CTM_RuleNotificationDevices]
           (
			 [RuleID],
			 [GroupNotificationID],
			 [IsActive]
			)       
	SELECT [RuleID],[GroupNotificationID],1
	FROM @tblRuleDeviceList
	--------------------------------------------------------

	
		  
	
	
	
END
GO

PRINT 'Create Procedure [dbo].[VOC_CTM_updateCriticalRule]'
GO


