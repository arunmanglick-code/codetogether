set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
/*Notice: Formatted SQL is not the same as input*/

ALTER PROCEDURE [dbo].[VOC_VWRPT_getRDCSReportData]
@subscriberID INT,
@beginDate DATETIME,
@endDate DATETIME,
@findingsID INT,
@recipientType INT,
@recipientID INT,
@Msgstatus INT,
@agentGroupID INT = 0
AS
/******************************************************************************  
**  File: VOC_VWRPT_getRDCSReportData.sql  
**  Name: VOC_VWRPT_getRDCSReportData  
**  Desc: It gets the data for C&S report (get Radiology Data)  
--			@recipientType - 0 then Radiologist(Reporting Clinician , Specialist) 
--			@recipientType - 1 then Referring Physician(Ordering Physician)
--			@recipientType - 4 then Clinical Team
--			@recipientID  - -1 for All 
--			@Msgstatus Status - All - 2, Closed - 0, Open - 1  
**   
**  Called by:   VOC_VWRPT_CSReportData  
**             
**  Date: 16 APR 2007  
*******************************************************************************  
**  Change History  
*******************************************************************************  
**  Date:		Author:    Description:  
**  --------	--------   -------------------------------------------  
**  12.09.07	IAK		   Added Logic to get data related to Unit and Clinical Team
**						   Changed #tmp table names with proper names.	
**	17.09.07	IAK		   Change Where Clause, No need to check dept asign table data after getting filtered data.
**  05-06-2008	NDM			Performance improvement.    
**  05-07-2008	PKS			Performance improvement - Removed unwanted temp tables and also changed the approach to update counts. 
**  05-20-2008  Prerak     CR #252 Nickname and Specialty usage in Report data 
**  10.22.2008  PCS			Modified for Worklist Details – Non Arun recipient
**  11.05.2008  PKS			Modified to have Live Agent C&S Details  
**	11-20-2008	RG			Added agentGroupID parameter - Access reports to Agent admin
**	12-24-2008	IAK			TTP:278 - Read within compliance logic for embargo period
**	12-31-2008	IAK			TTP:278 - Read within compliance logic for embargo period
**	01-09-2009	IAK			TTP:278 - Read within compliance logic for embargo period
**	13-05-2009	Suhas		Rad DB Re-Design changes
*******************************************************************************/
BEGIN
	--SELECT DETAILS FOR SUBSCRIBER
	DECLARE @groupID INT
	DECLARE @timeZoneID INT
	DECLARE @groupName varchar(100)
	DECLARE @institutionName varchar(100)

	--UPDATE LOCAL VARIABLES
	--===============================================================================

	IF @agentGroupID > 0
	BEGIN
		SELECT 
			@institutionName = I.InstitutionName, 
			@groupID = G.GroupID, 
			@groupName = G.GroupName,
			@timeZoneID = G.TimeZoneID
		FROM Groups G		
		INNER JOIN Directories D
			ON G.DirectoryID = D.DirectoryID
		INNER JOIN Institutions I
			ON D.InstitutionID = I.InstitutionID
		WHERE G.GroupID = @agentGroupID
	END
	ELSE
	BEGIN
		SELECT 
			@institutionName = I.InstitutionName, 
			@groupID = G.GroupId, 
			@groupName = G.GroupName,
			@timeZoneID = G.TimeZoneID
		FROM Subscribers S
		INNER JOIN GRoups G
			ON S.GroupId = G.GroupId
		INNER JOIN Directories D
			ON G.DirectoryID = D.DirectoryID
		INNER JOIN Institutions I
			ON D.InstitutionID = I.InstitutionID
		WHERE SubscriberID = @subscriberID
	END

	--GET DATA FOR FILTERD DATA FOR ALL MESSAGES
	--===============================================================================
	/*
	SELECT * INTO #FilteredData 
	FROM
		(
			SELECT * 
			FROM
				(
					SELECT * 
					FROM VOC_MESSAGESS M
					JOIN ------					

				) T1

		) MsgData
	WHERE 
		WHERE MsgData.SRID <> 0 AND ---
	ORDER BY  MsgData.LastName, MsgData.SpecialistID, MsgData.FindingID
	*/
	SELECT * INTO #FilteredData FROM -- MsgData
		(	
			SELECT *, 
				CASE   
						WHEN DATEDIFF(MINUTE, CreatedOn, ReadOn) < emhrs  
									THEN 0  
						ELSE DATEDIFF(MINUTE, CreatedOn, ReadOn) - emhrs
				END totalMinutesToClose,  
				DATEDIFF(MINUTE, CreatedOn_UsersTime, ReadOn_UsersTime) ReadTimediff  
			FROM -- T1
					(
						SELECT   
							CASE @recipientType  
									WHEN 0 THEN M.SpecialistID  
									WHEN 4 THEN 
									(
										SELECT 
											CASE 
												WHEN MAX(DSA.DepartmentID) IS NULL THEN 0 
												ELSE MAX(DSA.DepartmentID)
												END 
										FROM VOC_DepartmentShiftAssignments DSA  
										WHERE DSA.ReferringPhysicianID  = M.RecipientID AND IsActive = 1 AND   
											CASE     
												WHEN DSA.EndDateTime IS NULL THEN 
															CASE WHEN (DSA.StartDateTime <= M.CreatedOn) THEN 1 ELSE 0 END     
												ELSE 
															CASE WHEN (DSA.StartDateTime <= M.CreatedOn AND DSA.EndDateTime >= M.CreatedOn) THEN 1 ELSE 0 END    
											END = 1
									)
									ELSE M.RecipientID  
							END SRID,   
							CASE @recipientType  
									WHEN 0 THEN SU.FirstName + ' ' + SU.LastName   
									WHEN 4 THEN dbo.fnVoc_vl_GetRecipient(
																(	SELECT 
																		CASE 
																				WHEN MAX(DSA.DepartmentID) IS NULL THEN 0 
																				ELSE MAX(DSA.DepartmentID)
																		END 
																	FROM VOC_DepartmentShiftAssignments DSA  
																	WHERE DSA.ReferringPhysicianID  = M.RecipientID AND IsActive = 1 AND   
																		CASE     
																				WHEN DSA.EndDateTime IS NULL THEN 
																							CASE WHEN (DSA.StartDateTime <= M.CreatedOn) THEN 1 ELSE 0 END     
																				ELSE 
																							CASE WHEN (DSA.StartDateTime <= M.CreatedOn AND DSA.EndDateTime >= M.CreatedOn) THEN 1 ELSE 0 END    
																		END = 1
																), 4, M.CreatedOn)
									ELSE CASE WHEN M.RecipientID = -1 THEN NSR.RecipientName
									ELSE dbo.fnVoc_GetRecipient(M.RecipientID,@recipientType)
									END
							END SName, 
							CASE @recipientType  
									WHEN 0 THEN	  SU.NickName  
									WHEN 1 THEN   dbo.fnVoc_vwrpt_GetNickName(M.RecipientID, @recipientType)
									ELSE ''
							END NickName,  
							CASE @recipientType  
									WHEN 0 THEN   S.Specialty 
									WHEN 1 THEN   dbo.fnVoc_vwrpt_GetSpecialty(M.RecipientID, @recipientType)
									ELSE ''
							END Specialty,   
  
							M.MessageID,
							SU.LastName, 
							S.SpecialistID,   
							CreatedOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, @timeZoneID),    
							ReadOn_UsersTime = dbo.fnVO_VD_getDateByUsersTime(M.ReadOn, @timeZoneID),   
							@timeZoneID TimeZoneID,  
							M.CreatedOn, 
							M.ReadOn,  
							F.FindingID, 
							F.FindingDescription,  
							F.EscalateEvery, 
							F.EndAfterMinutes,  
							F.ComplianceGoal, 
							F.Embargo, 
							F.EmbargoStartHour,  
							GrpName =@groupName,  
							InsName =@institutionName,  
							F.EmbargoEndHour, 
							F.EmbargoSpanWeekend,  
							dbo.fnVO_InEmbargoHours(
									dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, @timeZoneID), 
									dbo.fnVO_VD_getDateByUsersTime(M.ReadOn, @timeZoneID),   
									embargoSpanWeekend, embargoEndHour, embargoStartHour, embargo) emhrs,
							M.AgentCreatedOn,
							M.IsDocumented,
							WLMF.WorklistMessageID,
							AN.ActionID AS AgentActionID,  
							RecipientTypeID = 1  
						FROM	VOC_Messages AS M  
						JOIN	Specialists AS S ON S.SpecialistID = M.SPECIALISTID  
						JOIN  Subscribers AS SU ON SU.SubscriberID = S.SUBSCRIBERID  
						--JOIN  ReferringPhysicians AS RP ON RP.ReferringPhysicianID = M.REFERRINGPHYSICIANID  
						JOIN  Findings AS F ON F.FindingID = M.FINDINGID  
						LEFT OUTER JOIN VOC_ACC_WorkListMessageForward WLMF
							ON M.MessageID = WLMF.MessageID AND WLMF.MessageTypeId = 1
						LEFT OUTER JOIN Voc_ACC_AgentNotes AN ON WLMF.WorklistMessageID = AN.WorklistMessageID AND AN.ActionID IN (3,4,5)
						LEFT OUTER JOIN VOC_ACC_NonSystemRecipients NSR ON NSR.MessageID = M.MessageId AND NSR.MessageType = 1
						WHERE	CASE	WHEN @Msgstatus = 0 AND M.ReadOn IS NOT NULL THEN 1  
												WHEN @Msgstatus = 1 AND M.ReadOn IS NULL THEN 1  
												ELSE 1  
									END = 1  
									AND  F.GroupID = @groupID  
									AND  (dbo.fnVO_VD_getDateByUsersTime(M.CreatedOn, @timeZoneID) BETWEEN CONVERT(SMALLDATETIME, @beginDate) AND CONVERT(SMALLDATETIME, @endDate)  
									OR dbo.fnVO_VD_getDateByUsersTime(M.AgentCreatedOn, @timeZoneID) BETWEEN CONVERT(SMALLDATETIME, @beginDate) AND CONVERT(SMALLDATETIME, @endDate))
									AND	CASE	WHEN  @findingsID <> -1 AND  M.FindingID = @findingsID THEN 1  
														WHEN @findingsID = -1 THEN 1
														ELSE 0  
											END = 1  
									AND CASE  WHEN M.IsDocumented = 1 AND M.AgentCreatedOn IS NOT NULL THEN 1
														WHEN M.IsDocumented = 0 THEN 1
														ELSE 0
										END = 1
					)
					AS T1
					WHERE T1.SRID <> 0 AND 
						  CASE	WHEN @recipientID <> -1 AND T1.SRID = @recipientID THEN 1
									WHEN @recipientID = -1 THEN 1  
									ELSE  0		 
						  END = 1 


		) MsgData     
	WHERE MsgData.SRID <> 0 AND 
				CASE	WHEN @recipientID <> -1 AND MsgData.SRID = @recipientID THEN 1
							WHEN @recipientID = -1 THEN 1  
							ELSE  0		 
				END = 1   
	ORDER BY  MsgData.LastName, MsgData.SpecialistID, MsgData.FindingID

	--CREATE FINAL OUTPUT TABLE
	--===============================================================================
	CREATE TABLE #FinalOutput
	(
		SRID int,
		GrpName varchar(100),
		InsName varchar(100),  
		SName varchar(105),
		NumCloseMessages int DEFAULT(0),
		NumOpenMessages int DEFAULT(0),
		ReadBeforeFirst int DEFAULT(0),
		ReadBeforeEnd int DEFAULT(0),
		ReadWithinCompliance int DEFAULT(0),
		ReadAfterCompliance int DEFAULT(0),
		AverageTAT int DEFAULT(0),
		RecipientTypeID int,
		Specialty varchar (75),
		NickName varchar(50),
		NoClosedByAgent	int DEFAULT(0),
		NoSentByAgent	int DEFAULT(0),
		NoAgentAfterCompliance int DEFAULT(0),
		AgentAfterCompliancePercent int DEFAULT(0)	
	)

	--INSERT DISTINCT DATA TO FINAL OUTPUT TABLE
	--===============================================================================
	INSERT INTO #FinalOutput (SRID, GrpName, InsName, SName, RecipientTypeID,Specialty, NickName ) 
	SELECT Distinct SRID, GrpName, InsName, SName, RecipientTypeID = 0, Specialty, NickName
	FROM #FilteredData

	--===============================================
	--UPDATE FINAL OUTPUT DATA COLUMNS FOR THE COUNT
	--===============================================


	--OPEN MESSAGE COUNTS
	--===============================================
--	UPDATE FNO
--	SET FNO.NumOpenMessages = T.NumOpenMessages
--	FROM #FinalOutput AS FNO
--	INNER JOIN
--	(
--		SELECT SRID, SName, Count(MessageId) as NumOpenMessages
--		FROM #FilteredData
--		WHERE ReadOn_UsersTime IS NULL  
--		GROUP BY SRID, SName	
--	) T
--	ON FNO.SRID = T.SRID AND FNO.SName = T.SName

	UPDATE FNO
	SET FNO.NumOpenMessages = 
		(
			SELECT Count(MessageId) as NumOpenMessages
			FROM #FilteredData T
			WHERE ReadOn_UsersTime IS NULL  AND
			FNO.SRID = T.SRID AND FNO.SName = T.SName
			GROUP BY SRID, SName	
		)
	FROM #FinalOutput AS FNO 



	--CLOSED MESSAGES
	--===============================================
	UPDATE FNO
	SET FNO.NumCloseMessages = T.NumCloseMessages
	FROM #FinalOutput AS FNO
	INNER JOIN
	(
		SELECT SRID, SName, COUNT(MessageID) NumCloseMessages
		FROM #FilteredData
		WHERE ReadOn_UsersTime IS NOT NULL  
		GROUP BY SRID, SName	
	) T
	ON FNO.SRID = T.SRID AND FNO.SName = T.SName

	--READ BEFORE FIRST ESC
	--===============================================
	UPDATE FNO
	SET FNO.ReadBeforeFirst = T.ReadBeforeFirst
	FROM #FinalOutput AS FNO
	INNER JOIN
	(
		SELECT SRID, SName, COUNT(MessageID) ReadBeforeFirst
		FROM #FilteredData
		WHERE ReadOn_UsersTime IS NOT NULL  
			AND DATEDIFF(MINUTE, CreatedOn_UsersTime, ReadOn_UsersTime) <= ESCALATEEVERY  
		GROUP BY SRID, SName 	
	) T
	ON FNO.SRID = T.SRID AND FNO.SName = T.SName


	--READ BEFORE END ESC
	--===============================================
	UPDATE FNO
	SET FNO.ReadBeforeEnd = T.ReadBeforeEnd
	FROM #FinalOutput AS FNO
	INNER JOIN
	(
		SELECT SRID, SName, COUNT(MessageID) ReadBeforeEnd
		FROM #FilteredData
		WHERE ReadOn_UsersTime IS NOT NULL  
			AND DATEDIFF(MINUTE, CreatedOn_UsersTime, ReadOn_UsersTime) <= ENDAFTERMINUTES  
		GROUP BY SRID, SName	
	) T
	ON FNO.SRID = T.SRID AND FNO.SName = T.SName

	--TAT TIME
	--===============================================
	UPDATE FNO
	SET FNO.AverageTAT = T.AverageTAT
	FROM #FinalOutput AS FNO
	INNER JOIN
	(
		SELECT SRID, SName, SUM(totalMinutesToClose) /  COUNT(MessageID) AverageTAT
		FROM #FilteredData
		WHERE ReadOn_UsersTime IS NOT NULL  
		GROUP BY SRID , SName	
	) T
	ON FNO.SRID = T.SRID AND FNO.SName = T.SName

	--PERCENTAGE OF READ WITHIN AND AFTER COMPLIANCE
	--===============================================
	UPDATE FNO
	SET FNO.ReadWithinCompliance = CAST((CONVERT(FLOAT(8), ISNULL(T.ReadWithinCompliance, 0)) / CONVERT(FLOAT(8), FNO.NumCloseMessages) * 100) AS INT), 
			FNO.ReadAfterCompliance = (100 - CAST((CONVERT(FLOAT(8), ISNULL(T.ReadWithinCompliance, 0)) / CONVERT(FLOAT(8), FNO.NumCloseMessages) * 100) AS INT))
	FROM #FinalOutput AS FNO
	LEFT OUTER JOIN
	(
		SELECT SRID, SName, CONVERT(FLOAT(8), (COUNT(MessageID))) AS ReadWithinCompliance
		FROM #FilteredData
		WHERE ReadOn_UsersTime IS NOT NULL
			AND dbo.fnVOC_MessageReadTimeExcludingEmbargo(Dbo.fnVO_VD_getDateByUsersTime(CreatedOn, @timeZoneID), 
											 Dbo.fnVO_VD_getDateByUsersTime(ReadOn, @timeZoneID), 
											 embargoSpanWeekend, embargo, embargoStartHour, embargoEndHour) <= COMPLIANCEGOAL
		Group By SRID, SName
	) T
	ON FNO.SRID = T.SRID AND FNO.SName = T.SName
	WHERE FNO.NumCloseMessages > 0

		--NO OF AGENT CLOSED MESSAGES
		--===============================================
		UPDATE FNO
		SET FNO.NoClosedByAgent = T.NoClosedByAgent
		FROM #FinalOutput AS FNO
		INNER JOIN
		(
			SELECT SRID, SName, Count(MessageId) as NoClosedByAgent
			FROM #FilteredData
			WHERE AgentCreatedOn IS NOT NULL AND ReadOn_UsersTime IS NOT NULL  
				AND (AgentActionID = 3 OR AgentActionID = 4)
			GROUP BY SRID, SName	
		) T
		ON FNO.SRID = T.SRID AND FNO.SName = T.SName

		--NO OF AGENT SENT Arun MESSAGES
		--===============================================
		UPDATE FNO
		SET FNO.NoSentByAgent = T.NoSentByAgent
		FROM #FinalOutput AS FNO
		INNER JOIN
		(
			SELECT SRID, SName, Count(MessageId) as NoSentByAgent
			FROM #FilteredData
			WHERE AgentCreatedOn IS NOT NULL AND AgentActionID = 5
			GROUP BY SRID, SName	
		) T
		ON FNO.SRID = T.SRID AND FNO.SName = T.SName

		--NO MESSAGES OUT OF COMPLIANCE SENT BY AGENT
		--===============================================
    --AGENT COMPLIANCE
    --===============================================
		UPDATE FNO
		SET FNO.NoAgentAfterCompliance = T.NoAgentAfterCompliance
		FROM #FinalOutput AS FNO
		INNER JOIN
		(
			SELECT SRID, SName, COUNT(MessageID) AS NoAgentAfterCompliance
			FROM #FilteredData
			WHERE AgentCreatedOn IS NOT NULL
					AND AgentActionID IS NOT NULL AND
          DATEDIFF(MINUTE, CreatedOn, AgentCreatedOn) >= COMPLIANCEGOAL
			Group By SRID, SName
		) T
		ON FNO.SRID = T.SRID AND FNO.SName = T.SName

--    --TOTAL TIME COMPLIANCE
--    --===============================================
--		UPDATE FNO
--		SET FNO.NoAgentAfterCompliance = T.NoAgentAfterCompliance
--		FROM #FinalOutput AS FNO
--		INNER JOIN
--		(
--			SELECT SRID, SName, COUNT(MessageID) AS NoAgentAfterCompliance
--			FROM #FilteredData
--			WHERE AgentCreatedOn IS NOT NULL AND ReadOn_UsersTime IS NOT NULL 
--					AND AgentActionID IS NOT NULL AND
--					DATEDIFF(MINUTE, CreatedOn_UsersTime, ReadOn_UsersTime) - 
--					dbo.fnVO_CS_MsgReadCompliance(CreatedOn_UsersTime, ISNULL(ReadOn_UsersTime, Dbo.fnVO_VD_getDateByUsersTime(GETDATE(), @timeZoneID)),
--																					embargoSpanWeekend, embargoEndHour, embargoStartHour, Embargo) 
--					>= COMPLIANCEGOAL
--			Group By SRID, SName
--		) T
--		ON FNO.SRID = T.SRID AND FNO.SName = T.SName

		--PERCENTAGE MESSAGES OUT OF COMPLIANCE SENT BY AGENT
		--===================================================
		UPDATE FNO
		SET AgentAfterCompliancePercent = CAST(
												(CONVERT(FLOAT(8), ISNULL(FNO.NoAgentAfterCompliance, 0)) / 
												CONVERT(FLOAT(8), (FNO.NoSentByAgent + FNO.NoClosedByAgent)) * 100) AS INT
											)
		FROM #FinalOutput AS FNO
		WHERE (FNO.NoSentByAgent + FNO.NoClosedByAgent) > 0


	--SELECT FINAL OUTPUT DATA
	--===============================================================================
	IF @Msgstatus = 0
	BEGIN
			SELECT SRID, GrpName, InsName, SName, RecipientTypeID = 0, NumCloseMessages, 0 AS NumOpenMessages,  
				ReadBeforeFirst, ReadBeforeEnd, ReadWithinCompliance, ReadAfterCompliance, AverageTAT , Specialty, NickName,
				NoClosedByAgent, NoSentByAgent, NoAgentAfterCompliance, AgentAfterCompliancePercent
			FROM #FinalOutput
			WHERE NumCloseMessages > 0	
			ORDER BY SName
	END
	ELSE IF @Msgstatus = 1
	BEGIN
			SELECT SRID, GrpName, InsName, SName, RecipientTypeID = 0, 0 AS NumCloseMessages, NumOpenMessages,
					0 AS ReadBeforeFirst, 0 AS ReadBeforeEnd, 0 AS ReadWithinCompliance, 0 AS ReadAfterCompliance, 0 AS AverageTAT ,Specialty, NickName,
					NoClosedByAgent, NoSentByAgent, NoAgentAfterCompliance, AgentAfterCompliancePercent
			FROM #FinalOutput
			WHERE NumOpenMessages > 0
			ORDER BY SName
	END
	ELSE IF @Msgstatus = 2
	BEGIN
			SELECT * FROM #FinalOutput 
			WHERE SRID = 2969  --AM
			ORDER BY SName 
	END
	--DROP TEMP TABLES
	--===============================================================================
	DROP TABLE #FilteredData
	DROP TABLE #FinalOutput
END

