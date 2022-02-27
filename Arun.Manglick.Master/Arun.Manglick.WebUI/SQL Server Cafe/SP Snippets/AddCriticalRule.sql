/*

Code used in ASP.NET
------------------------

arParams[12] = new SqlParameter(RULE_PARAMETERS, System.Data.SqlDbType.Xml);
DataTable dtParameters = ctRuleDto.RuleParameters;
dtParameters.TableName = "RuleParameters";
using (MemoryStream memoryStream = new MemoryStream())
{
    ctRuleDto.RuleParameters.WriteXml(memoryStream);
    UTF8Encoding encoding = new UTF8Encoding();
    arParams[12].Value = encoding.GetString(memoryStream.ToArray());
}

*/

SQL Server SP
------------------------

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[VOC_CTM_addCriticalRule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[VOC_CTM_addCriticalRule]

PRINT 'DROPPED Procedure [dbo].[VOC_CTM_addCriticalRule]'

GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************  
**  File: VOC_CTM_addCriticalRule.sql  
**  Name: VOC_CTM_addCriticalRule  
**  Desc: Add a Critical Rule Information
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

CREATE PROCEDURE [VOC_CTM_addCriticalRule] 

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

DECLARE @createdOn DateTime
SET @createdOn = getdate()

DECLARE @tblRuleParameters TABLE 
( 
	[RuleID] [int] NOT NULL,
	[ParameterID] [int] NOT NULL,
	[OperatorID] [int] NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[MeasureUnitID] [int] NULL,
	[RuleParameterID] [int] NULL
)

DECLARE @tblRuleDeviceList TABLE 
( 
	[RuleID] [int] NOT NULL,
	[GroupNotificationID] [int] NOT NULL
)

BEGIN
   
	 
	-------------------------------------------------------------------------
	 INSERT INTO [dbo].[VOC_CTM_Rules]
           ([InstitutionID]
           ,[FacilityID]
           ,[GroupID]
           ,[RuleName]
           ,[Precedence]
           ,[FindingID]
           ,[TAT_OrderPlacedToComplete]
           ,[TAT_ProcCompleteToMsgSent]
           ,[TAT_MsgSentToMsgReceived]
           ,[TAT_OrderPlacedToMsgReceived]
           ,[CreatedOn]
           ,[UpdatedOn]
           ,[UpdatedBy]
           ,[Active])
     VALUES
           (@instituteId,
			@facilityId,
            @groupId,
            @ruleName,
			@precedence,
			@findingId,
			@tatOrderPlacedToComplete,
			@tatProcCompleteToMsgSent,
			@tatMsgSentToMsgReceived,
			@tatOrderPlacedToMsgReceived,
			@createdOn,
			null,
			@updatedBy,
			1
	    )
	-------------------------------------------------------------------------

	SELECT @ruleId = @@IDENTITY

	INSERT INTO @tblRuleParameters ( [RuleID], [ParameterID], [OperatorID], [Value], [MeasureUnitID]) (
	SELECT 
		@ruleId,
		ExternalInformation.Details.query('ParameterID').value('.', 'INTEGER') AS [ParameterID],
		ExternalInformation.Details.query('OperatorID').value('.', 'INTEGER') AS [OperatorID],
		ExternalInformation.Details.query('Value').value('.', 'VARCHAR(100)') AS [Value],
		ExternalInformation.Details.query('MeasureUnitID').value('.', 'INTEGER') AS [MeasureUnitID]
	FROM
		@ruleParameters.nodes('/DocumentElement/RuleParameters') AS ExternalInformation([Details]) )
	
	
	INSERT INTO [VOC_CTM_RuleParameters]
           ([RuleID]
           ,[ParameterID]
           ,[OperatorID]
           ,[Value]
           ,[MeasureUnitID])
	SELECT [RuleID],[ParameterID],[OperatorID],[Value],[MeasureUnitID]
	FROM @tblRuleParameters

	-------------------------------------------------------------------------
	
	INSERT INTO @tblRuleDeviceList ( [RuleID], [GroupNotificationID]) (
	SELECT 
		@ruleId,
		ExternalInformation.Details.query('GroupNotificationID').value('.', 'INTEGER') AS [GroupNotificationID]
	FROM
		@ruleDeviceList.nodes('/DocumentElement/RuleAssignDevicesList') AS ExternalInformation([Details]) )
	

	INSERT INTO [VOC_CTM_RuleNotificationDevices]
           (
			 [RuleID],
			 [GroupNotificationID],
			 [IsActive]
			)       
	SELECT [RuleID],[GroupNotificationID],1
	FROM @tblRuleDeviceList
	-------------------------------------------------------------------------


END
GO

PRINT 'Create Procedure [dbo].[VOC_CTM_addCriticalRule]'
GO


