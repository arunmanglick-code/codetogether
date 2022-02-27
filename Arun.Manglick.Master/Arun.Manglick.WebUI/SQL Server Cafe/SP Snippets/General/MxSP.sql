-- Filename: LM_MigrateMDE6.sql
-- Implements a stored procedure to migrate MDE6 LM Data in the current database.
-- Assumes the LENDER has already been added to the LM_Lenders table.
--
-- Test:
/*
	deallocate csrStips
Declare	@return_value int;
Exec	@return_value = [dbo].[LM_MigrateMDE6]
		@sLenderShortName = N'WRLDOMNI',
		@sUsername = N'Test1';

*/

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_NULLS ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[LM_MigrateMde6]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
   DROP PROCEDURE [dbo].[LM_MigrateMde6]
GO

CREATE  PROCEDURE [dbo].[LM_MigrateMde6] 

/********************************************************
*
*	Procedure: [LM_MigrateMde6]
*	
* The script runs as a stored procedure in the database that is being migrated.
* The input data will come from the “current database”
* The output data will be put in the “current database”
*
* The input parameters for the script are: 'LenderId' and the 'Username'
* The output parameter will be: 1=success, 0=failure
* The migration will run inside a Try/Catch that does a commit/rollback based on whether an error occurs during the migration.
*
*   2007-10-03  JAV  Initial Creation
*   2008-01-10  GH   Adapt to use with refactored database.
*
* Filename: LM_MigrateMDE6.sql
* Implements a stored procedure to migrate MDE6 LM Data in the current database.
* Assumes the LENDER has already been added to the LM_Lenders table.
*
* Testing:

	deallocate csrStips;
Declare	@return_value int;
Exec	@return_value = [dbo].[LM_MigrateMDE6]
		@sLenderShortName = N'WRLDOMNI',
		@sUsername = N'Test1';
--
-- For Debug:
    Delete from LM_VERSION
    Delete from LM_LENDER
    Execute [dbo].[LM_MigrateMDE6] 'wrldomni', 'GARYH' 	

--
********************************************************/
-- Input:
  @sLenderShortName char(8),
  @sUsername varchar(50)
AS


SET NOCOUNT ON

BEGIN
	declare @nVersionID int
	declare @nNextID int
	declare @nVersionIdPrior int

	declare @dtStart datetime
	declare @dtEnd datetime
	declare @dtNow datetime

	declare @sShortName varchar(15)
	declare @sLongName varchar(40)
	declare @sNotes varchar(300)
	declare @sModifier varchar(30)
	declare @sDescription varchar(75)
	declare @sFinanceType char(1)
	declare @sStatus char(1)
	declare @nMaxPenalty int
	declare @nPenalty int
	declare @sPenaltySign char(1)

	declare @nOldID int
	declare @sOldStringID char(8)
	declare @nOldSequence int

	declare @sComment as varchar(1024)
	Set @sComment = ''
	
	declare @nItemId as int
	Set @nItemId = -1
	
	
	declare @sApplication varchar(50)
	Set @sApplication = 'LM_MigrateMde6'

	declare @sMsg as varchar(512)
	set @dtStart = getdate()

 Begin Transaction
	Begin Try

-- LM Lender
	-- Select * from LM_LENDER
	declare @nLenderId as integer	
	select  @nLenderId =LENDER_ID From LM_LENDER where short_name = @sLenderShortName
if @@rowcount <> 1
	begin
		Insert LM_LENDER ( SHORT_NAME ) VALUES ( @sLenderShortName )
		select  @nLenderId =LENDER_ID From LM_LENDER where short_name = @sLenderShortName
	end

	-- Get the previous high version (or 0 if none).
	Select @nVersionIdPrior= isnull(max(version_id),0) from LM_VERSION

-- LM_STIPULATION
Print 'LM_STIPULATION ' + cast( datediff(second,  @dtStart, Getdate()) as varchar)

	declare @sStipID char(8)
	declare @sStipDescription varchar(75)
	declare @sStipType char(1)


	declare csrStips cursor 
	for 
	select stipulation_id, stipulation_description, stipulation_type 
	from dbo.lender_stipulations
	where lender_id = @sLenderShortName
      and STIPULATION_TYPE <> 'M'
	order by stipulation_id

	open csrStips

	fetch next from csrStips
	into @sStipID, @sStipDescription, @sStipType

	while @@fetch_status = 0 
	begin

		Set @sStipID = Ltrim(Rtrim(@sStipID));
		
		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'STIPULATION', @sStipID, @nItemId, @sComment, @sApplication, 'N'
		exec @nNextID = LM_GetNextID 'LM_STIPULATION', 'stipulation_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_stipulation 
		            (version_id, stipulation_id, short_name, type, description) 
			values (@nVersionID, @nNextID, @sStipID, @sStipType, @sStipDescription)

		-- Get next Row
		fetch next from csrStips
		into @sStipID, @sStipDescription, @sStipType

	end

	close csrStips
	deallocate csrStips

---LM_MESSAGE-------------------------------------------------------
print 'LM_MESSAGE'

	--declare @sStipID char(8)
	--declare @sStipDescription varchar(75)
	--declare @sStipType char(1)

	declare csrStips cursor 
	for 
	select stipulation_id, stipulation_description, stipulation_type 
	from dbo.lender_stipulations
	where lender_id = @sLenderShortName
      and STIPULATION_TYPE = 'M'
	order by stipulation_id

	open csrStips

	fetch next from csrStips
	into @sStipID, @sStipDescription, @sStipType

	while @@fetch_status = 0 
	begin

		Set @sStipID = Ltrim(Rtrim(@sStipID));
		
		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'MESSAGE', @sStipID, @nItemId, @sComment, @sApplication, 'N'
		exec @nNextID = LM_GetNextID 'LM_MESSAGE', 'message_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into LM_MESSAGE (version_id, message_id, short_name, description) 
			values (@nVersionID, @nNextID, @sStipID, @sStipDescription)

		-- Get next Row
		fetch next from csrStips
		into @sStipID, @sStipDescription, @sStipType
	end

	close csrStips
	deallocate csrStips

-- LM_REFER
print 'LM_REFER'
	declare @sReferID char(8)
	declare @sReferDescription varchar(75)

	declare csrRefers cursor 
	for 
	select refer_reason_id, refer_reason_desc
	from dbo.lender_refer_reasons
	where lender_id = @sLenderShortName
	order by refer_reason_id


	open csrRefers

	fetch next from csrRefers
	into @sReferID, @sReferDescription

	while @@fetch_status = 0 
	begin

		set @dtNow = getdate()
		Set @sReferID = Ltrim(Rtrim(@sReferID));
		
		-- insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'REFER', @sReferID, @nItemId, @sComment, @sApplication, 'N'
		exec @nNextID = LM_GetNextID 'LM_REFER', 'refer_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_refer (version_id, refer_id, short_name, description) 
			values (@nVersionID, @nNextID, @sReferID, @sReferDescription)

		-- Get next Row
		fetch next from csrRefers
		into @sReferID, @sReferDescription
	end

	close csrRefers
	deallocate csrRefers
	
-- LM_UNDERWRITING_ACTION   (Should Be Hard Coded)
print 'LM_UNDERWRITING_ACTION'
	declare @nUAID int
	declare @sColumnName varchar(20)
	
	declare csrUAs cursor 
	for 
	select underwriting_action_id, underwriting_action_desc, column_name
	from dbo.lender_underwriting_actions
	order by underwriting_action_id


	open csrUAs

	fetch next from csrUAs
	into @nUAID, @sDescription, @sColumnName

	while @@fetch_status = 0 
	begin

		Set @nUAID = Ltrim(Rtrim(@nUAID));
		
		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'UNDERWRITING_ACTION', @nUAID, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_UNDERWRITING_ACTION', 'underwriting_action_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_underwriting_action (version_id, underwriting_action_id, short_name, description, column_name) 
			values (@nVersionID, @nNextID, @nUAID, @sDescription, @sColumnName)

		-- Get next Row
		fetch next from csrUAs
		into @nUAID, @sDescription, @sColumnName
	end

	close csrUAs
	deallocate csrUAs

-- LM_ADVERSE_ACTION
print 'LM_ADVERSE_ACTION'
	declare @sAAID char(8)
	declare @sAADescription varchar(75)

	declare csrAdverseActions cursor 
	for 
	select adverse_action_number, adverse_action_desc
	from dbo.lender_adverse_actions
	where lender_id = @sLenderShortName
	order by adverse_action_number

	open csrAdverseActions
	fetch next from csrAdverseActions
	into @sAAID, @sAADescription

	while @@fetch_status = 0 
	begin

		Set @sAAID = Ltrim(Rtrim(@sAAID));
		
		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'ADVERSE_ACTION', @sAAID, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_ADVERSE_ACTION', 'adverse_action_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_adverse_action (version_id, adverse_action_id, short_name, description) 
			values (@nVersionID, @nNextID, @sAAID, @sAADescription)

		-- Get next Row
		fetch next from csrAdverseActions
		into @sAAID, @sAADescription
	end

	close csrAdverseActions
	deallocate csrAdverseActions

-- LM_DEALER
print 'LM_DEALER'
	declare @sCity varchar(40)
	declare @sState char(2)
	declare @sZipCode char(10)
	

	declare csrDealers cursor 
	for 
	select d.dealer_id, dealer_name, city, state, zip_code, modifier
	from dbo.dealers as d, dbo.lender_dealers as ld
	where d.dealer_id = ld.dealer_id
	and ld.lender_id = @sLenderShortName
	order by d.dealer_id

	open csrDealers
	fetch next from csrDealers
	into @sShortName, @sLongName, @sCity, @sState, @sZipCode, @sModifier

	while @@fetch_status = 0 
	begin
		Set @sShortName = Ltrim(Rtrim(@sShortName));
		
		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'DEALER', @sShortName, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_DEALER', 'dealer_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_dealer (version_id, dealer_id, short_name, long_name, city, state, zip_code) 
			values (@nVersionID, @nNextID, @sShortName, @sLongName, @sCity, @sState, @sZipCode)

		-- Get next Row
		fetch next from csrDealers
		into @sShortName, @sLongName, @sCity, @sState, @sZipCode, @sModifier
	end

	close csrDealers
	deallocate csrDealers

-- LM_DEALER_GROUP
print 'LM_DEALER_GROUP'
	declare csrDealerGroups cursor 
	for 
	select dealer_group_name, dealer_desc, modifier, dealer_group_id
	from dbo.dealer_group 
	where lender_id = @sLenderShortName
	order by dealer_group_id

	open csrDealerGroups

	fetch next from csrDealerGroups
	into @sShortName, @sDescription, @sModifier, @nOldID

	while @@fetch_status = 0 
	begin

		Set @sShortName = Ltrim(Rtrim(@sShortName));

		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'DEALER_GROUP', @sShortName, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_DEALER_GROUP', 'dealer_group_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_dealer_group (version_id, dealer_group_id, short_name, long_name, description, old_dealer_group_id) 
			values (@nVersionID, @nNextID, @sShortName, '', @sDescription, @nOldID)

		-- Get next Row
		fetch next from csrDealerGroups
		into @sShortName, @sDescription, @sModifier, @nOldID
	end

	close csrDealerGroups
	deallocate csrDealerGroups

-- LM_DEALER_GROUP_DEALER
 print 'LM_DEALER_GROUP_DEALER'  
	declare @nDealerGroupId int
	declare @nDealerId int

	declare csrDealerGroupDealer cursor 
	for
	
	select dg.dealer_group_id, d.dealer_id, dg.short_name
		from lm_dealer_group as dg, lm_dealer as d, dbo.dealer_group_member as dgm
		where dg.old_dealer_group_id = dgm.dealer_group_id
		and dgm.dealer_id = d.short_name
		order by dg.dealer_group_id, d.dealer_id
 
	open csrDealerGroupDealer

	fetch next from csrDealerGroupDealer
	into @nDealerGroupId, @nDealerId, @sShortName

	while @@fetch_status = 0 
	begin
		select @nVersionID = max(version_id)
		 from lm_version
		 where item_type = 'DEALER_GROUP'
		  and item_name = @sShortName
		  and @nLenderId = Lender_id
		  and version_id > @nVersionIdPrior;

		insert into lm_dealer_group_dealer (version_id, dealer_group_id, dealer_id) 
			values (@nVersionID, @nDealerGroupId, @nDealerId)

		-- Get next Row
		fetch next from csrDealerGroupDealer
		into @nDealerGroupId, @nDealerId, @sShortName
	end

	close csrDealerGroupDealer
	deallocate csrDealerGroupDealer

-- LM_REGION
print 'LM_REGION'
	declare csrRegion cursor 
	for 
	select region_name, region_desc, modifier, region_id
	from dbo.region
	where lender_id = @sLenderShortName
	order by region_id

	open csrRegion

	fetch next from csrRegion
	into @sShortName, @sDescription, @sModifier, @nOldID

	while @@fetch_status = 0 
	begin
	
		Set @sShortName = Ltrim(Rtrim(@sShortName));

		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'REGION', @sShortName, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_REGION', 'region_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_region (version_id, region_id, short_name, long_name, description, old_region_id) 
			values (@nVersionID, @nNextID, @sShortName, '', @sDescription, @nOldID)

		-- Get next Row
		fetch next from csrRegion
		into @sShortName, @sDescription, @sModifier, @nOldID
	end

	close csrRegion
	deallocate csrRegion

-- LM_REGION_ZIP_CODE
  print 'LM_REGION_CODE' 
	declare @nRegionId int
	declare @cZipCode char(3)

	declare csrRegionZipCode cursor 
	for
	
	select r.region_id, rz.scf, r.short_name
		from  lm_region as r, dbo.region_zip as rz
		where r.old_region_id = rz.region_id
		and r.version_id > @nVersionIdPrior
		order by r.region_id, rz.scf
 
	open csrRegionZipCode

	fetch next from csrRegionZipCode
	into @nRegionId, @cZipCode, @sShortName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		from lm_version
		 where item_type = 'REGION'
		  and item_name = @sShortName;

		insert into lm_region_zip_code (version_id, region_id, zip_code_3) 
			values (@nVersionID, @nRegionId, @cZipCode)

		-- Get next Row
		fetch next from csrRegionZipCode
		into @nRegionId, @cZipCode, @sShortName
	end

	close csrRegionZipCode
	deallocate csrRegionZipCode

-- LM_PRODUCT
print 'LM_PRODUCT'
	declare csrProduct cursor 
	for
	select product_id, product_name, product_desc, lease_or_purchase, status, modifier
	from dbo.product
	where lender_id = @sLenderShortName
	order by product_id
 
	open csrProduct

	fetch next from csrProduct
	into @nOldID, @sShortName, @sDescription, @sFinanceType, @sStatus, @sModifier

	while @@fetch_status = 0 
	begin

		Set @sShortName = Ltrim(Rtrim(@sShortName));

		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'PRODUCT', @sShortName, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_PRODUCT', 'product_id'
		Update LM_VERSION Set ITEM_ID = @nNextID
		 Where Version_id = @nVersionID


		insert into lm_product 
					(version_id, product_id, short_name, long_name, description, finance_type, status, old_product_id) 
			values (@nVersionID, @nNextID, @sShortName, '', @sDescription, @sFinanceType, @sStatus, @nOldID)

		-- Get next Row
		fetch next from csrProduct
		into @nOldID, @sShortName, @sDescription, @sFinanceType, @sStatus, @sModifier
	end

	close csrProduct
	deallocate csrProduct

    -- Change Purchase to Loan
	update lm_product
	set finance_type = 'F'
	where finance_type = 'P'

-- LM_PROGRAM
print 'LM_PROGRAM'
	declare @nProgramID int
	declare @nProgramVersion int
	declare @sProgramType char(1)
	
	create table #tempProgram
	(
		LENDER_ID char(8),
		PROGRAM_ID int,
		PROGRAM_VERSION int	
	)

	-- Most Recent Active Programs
	insert into #tempProgram (lender_id, program_id, program_version)
	select lender_id, program_id, program_version
	from dbo.program as p1
	where program_version in
	(
		select top(1) program_version
		from dbo.program as p2
		where p1.program_id = p2.program_id
		and lender_id = @sLenderShortName
		and status = 'A'
		order by program_id, program_version desc
	)
	and lender_id = @sLenderShortName


	-- Most Recent not in previous list
	insert into #tempProgram (lender_id, program_id, program_version)
	select lender_id, program_id, program_version
	from dbo.program as p1
	where program_version in
	(
		select top(1) program_version
		from dbo.program as p2
		where p1.program_id = p2.program_id
		and lender_id = @sLenderShortName
		order by program_id, program_version desc
	)
	and lender_id = @sLenderShortName
	and program_id not in
	(
		select program_id 
		from #tempProgram
	)

	declare csrProgram cursor 
	for
	select p.program_id, p.program_version, status, lease_or_purchase, program_name, program_description, 
           notes, program_type, modifier
	from dbo.program as p, #tempProgram as t
	where p.lender_id = t.lender_id
	and p.program_id = t.program_id
	and p.program_version = t.program_version
	order by p.program_id

	open csrProgram
	fetch next from csrProgram
	into @nProgramID, @nProgramVersion, @sStatus, @sFinanceType, @sLongName, @sDescription, @sNotes, @sProgramType, @sModifier

	while @@fetch_status = 0 
	begin

		-- insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'PROGRAM', @sLongName, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_PROGRAM', 'program_id'
		Update LM_VERSION
		 Set ITEM_ID = @nNextID
		 Where Version_id = @nVersionID

		insert into lm_program(version_id, program_id, short_name, long_name, description, notes, program_type, finance_type, status, old_program_id, old_program_version) 
			values (@nVersionID, @nNextID, '', @sLongName, @sDescription, @sNotes, @sProgramType, @sFinanceType, @sStatus, @nProgramID, @nProgramVersion)

		-- Get next Row
		fetch next from csrProgram
		into @nProgramID, @nProgramVersion, @sStatus, @sFinanceType, @sLongName, @sDescription, @sNotes, @sProgramType, @sModifier
	end

	close csrProgram
	deallocate csrProgram
	drop table #tempProgram

	-- Change Purchase to Loan
	update lm_program
	set finance_type = 'F'
	where finance_type = 'P'
	and version_id > @nVersionIdPrior

-- LM_PRODUCT_PROGRAM
 print 'LM_PRODUCT_PROGRAM'  
	declare @nProgramSequence int
	declare @nProductId int

	declare csrProductProgram cursor 
	for
	
	select p.product_id, ppgm.program_order, pgm.program_id, p.short_name
		from lm_product as p, lm_program as pgm, dbo.product_program as ppgm
		where p.old_product_id = ppgm.product_id
			and pgm.old_program_id = ppgm.program_id
			and pgm.old_program_version = ppgm.program_version
			and ppgm.lender_id = @sLenderShortName
			and p.version_id > @nVersionIdPrior
			and pgm.version_id > @nVersionIdPrior
		order by p.product_id, ppgm.program_order
 
	open csrProductProgram

	fetch next from csrProductProgram
	into @nProductId, @nProgramSequence, @nProgramId, @sShortName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		 from lm_version
		  where item_type = 'PRODUCT'
		   and item_name = @sShortName
		   and @nLenderId = Lender_id;


		insert into lm_product_program (version_id, product_id, program_sequence, program_id) 
			values (@nVersionID, @nProductId, @nProgramSequence, @nProgramId)

		-- Get next Row
		fetch next from csrProductProgram
		into @nProductId, @nProgramSequence, @nProgramId, @sShortName
	end

	close csrProductProgram
	deallocate csrProductProgram

-- LM_PROFILE
 print 'LM_PROFILE'  
	declare @sScorecard char(1)
	declare csrProfile cursor 
	for
	select profile_table_id, profile_name, profile_description, notes, status, modifier, scorecard_flag
		from dbo.profile_table
		where lender_id = @sLenderShortName
		order by profile_table_id
 
	open csrProfile

	fetch next from csrProfile
	into @nOldID, @sLongName, @sDescription, @sNotes, @sStatus, @sModifier, @sScorecard

	while @@fetch_status = 0 
	begin

		-- insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'PROFILE', @sLongName, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_PROFILE', 'profile_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_profile (version_id, profile_id, short_name, long_name, description, notes, status, scorecard, old_profile_id) 
			values (@nVersionID, @nNextID, '', @sLongName, @sDescription, @sNotes, @sStatus, @sScorecard, @nOldID)

		-- Get next Row
		fetch next from csrProfile
		into @nOldID, @sLongName, @sDescription, @sNotes, @sStatus, @sModifier, @sScorecard
	end

	close csrProfile
	deallocate csrProfile

-- LM_PROGRAM_PROFILE
print 'LM_PROGRAM_PROFILE'   
	declare @sCutoofTier char (1)
	declare @nDecisionSequence int
	declare @nProfileId int

	declare csrProgramProfile cursor 
	for
	
	select p.program_id, 
		CASE ppf.credit_basis
		WHEN 'A' THEN 1
		WHEN 'C' THEN 3
		WHEN 'AC' THEN 13
		END as decision_sequence, 

		ppf.profile_table_id, ppf.cutoff_tier, p.long_name
		from
			dbo.lm_program as p
		join
			dbo.program_profile as ppf
				on p.old_program_id = ppf.program_id
					and p.old_program_version = ppf.program_version
					and ppf.lender_id = @sLenderShortName
		left outer join
			dbo.lm_profile as pf
				on (ppf.profile_table_id = pf.old_profile_id)
					and ppf.lender_id = @sLenderShortName
		where pf.version_id > @nVersionIdPrior
		order by p.program_id, ppf.credit_basis
 
	open csrProgramProfile

	fetch next from csrProgramProfile
	into @nProgramId, @nProfileId, @nDecisionSequence, @sCutoofTier, @sLongName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		 from lm_version
		 where
				item_type = 'PROGRAM'
		   and	item_name = @sLongName
		   and	LENDER_ID = @nLenderId
		

--print @nVersionID + ',' +  @nProgramId + ',' + @nProfileId + ',' + @nDecisionSequence + ',' + @sCutoofTier


		insert into lm_program_profile
			   (version_id, program_id, decision_sequence, profile_id, cutoff_tier) 
		values (@nVersionID, @nProgramId, @nProfileId, @nDecisionSequence, @sCutoofTier)

		-- Get next Row
		fetch next from csrProgramProfile
		into @nProgramId, @nProfileId, @nDecisionSequence, @sCutoofTier, @sLongName
	end

	close csrProgramProfile
	deallocate csrProgramProfile

-- LM_PROGRAM_DEALER_GROUP
print 'LM_PROGRAM_DEALER_GROUP'
	declare csrProgramDealerGroup cursor 
	for
	
	select p.program_id, dg.dealer_group_id, p.long_name
		from	lm_program as p,
				dbo.program_dealer_group as pdg,
				lm_dealer_group as dg
		where p.old_program_id = pdg.program_id
		and p.old_program_version = pdg.program_version
		and pdg.dealer_group_id = dg.old_dealer_group_id
		and pdg.lender_id = @sLenderShortName
		and p.version_id > @nVersionIdPrior

	open csrProgramDealerGroup

	fetch next from csrProgramDealerGroup
	into @nProgramId, @nDealerGroupId, @sLongName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
			from lm_version
			where item_type = 'PROGRAM'
			and item_name = @sLongName
			and @nLenderId = Lender_id;


		insert into lm_program_dealer_group (version_id, program_id, dealer_group_id) 
			values (@nVersionID, @nProgramId, @nDealerGroupId)

		-- Get next Row
		fetch next from csrProgramDealerGroup
		into @nProgramId, @nDealerGroupId, @sLongName
	end

	close csrProgramDealerGroup
	deallocate csrProgramDealerGroup

-- LM_PROGRAM_REGION
print 'LM_PROGRAM_REGION'
	declare csrProgramRegion cursor 
	for
	
	select p.program_id, r.region_id, p.long_name
		from lm_program as p,
			lm_region as r,
			dbo.program_region as pr
		where p.old_program_id = pr.program_id
		and p.old_program_version = pr.program_version
		and r.old_region_id = pr.region_id
		and pr.lender_id = @sLenderShortName
		and p.version_id > @nVersionIdPrior
		and r.version_id > @nVersionIdPrior

	open csrProgramRegion

	fetch next from csrProgramRegion
	into @nProgramId, @nRegionId, @sLongName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		  from lm_version
		  where item_type = 'PROGRAM'
		  and item_name = @sLongName
		  and @nLenderId = Lender_id;


		insert into lm_program_region (version_id, program_id, region_id) 
			values (@nVersionID, @nProgramId, @nRegionId)

		-- Get next Row
		fetch next from csrProgramRegion
		into @nProgramId, @nRegionId, @sLongName
	end

	close csrProgramRegion
	deallocate csrProgramRegion

-- LM_PROGRAM_TIER
print 'LM_PROGRAM_TIER'
	declare @sTier char(2)
	declare @sDenyTier char(2)
	declare @nFromScore int
	declare @nToScore int

	declare csrProgramTier cursor 
	for
	
	select p.program_id, pt.tier, pt.deny_tier, pt.tier_from_score, pt.tier_to_score, p.long_name
		from lm_program as p, dbo.PROGRAM_TIER_CAPS_AND_SERVICER as pt
		where p.old_program_id = pt.program_id
		and p.old_program_version = pt.program_version
		and  pt.lender_id = @sLenderShortName
		and p.version_id > @nVersionIdPrior 
		order by p.program_id, pt.tier


	open csrProgramTier

	fetch next from csrProgramTier
	into @nProgramId, @sTier, @sDenyTier, @nFromScore, @nToScore, @sLongName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
			from lm_version
			where item_type = 'PROGRAM'
			and item_name = @sLongName
			and @nLenderId = Lender_id
			and version_id > @nVersionIdPrior;


		insert into lm_program_tier (version_id, program_id, tier, deny_tier, from_score, to_score) 
			values (@nVersionID, @nProgramId, @sTier, @sDenyTier, @nFromScore, @nToScore)

		-- Get next Row
		fetch next from csrProgramTier
		into @nProgramId, @sTier, @sDenyTier, @nFromScore, @nToScore, @sLongName
	end

	close csrProgramTier
	deallocate csrProgramTier

-- LM_PROGRAM_TCTC
print 'LM_PROGRAM_TCTC'
	declare csrProgramTCTC cursor 
	for
	
	select p.program_id, pt.from_score, pt.to_score, p.long_name
		from lm_program as p, dbo.TOO_CLOSE_TO_CALL_SCORES as pt
		where p.old_program_id = pt.program_id
		and p.old_program_version = pt.program_version
		and  pt.lender_id = @sLenderShortName
		and p.version_id > @nVersionIdPrior
		order by p.program_id, pt.from_score, pt.to_score

	open csrProgramTCTC

	fetch next from csrProgramTCTC
	into @nProgramId, @nFromScore, @nToScore, @sLongName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
			from lm_version
			where item_type = 'PROGRAM'
			and item_name = @sLongName
			and @nLenderId = Lender_id
			and version_id > @nVersionIdPrior;


		insert into lm_program_tctc (version_id, program_id, from_score, to_score) 
			values (@nVersionID, @nProgramId, @nFromScore, @nToScore)

		-- Get next Row
		fetch next from csrProgramTCTC
		into @nProgramId, @nFromScore, @nToScore, @sLongName
	end

	close csrProgramTCTC
	deallocate csrProgramTCTC

-- LM_RULE
 print 'LM_RULE'  
	declare @nSummaryMultiplierColimn int
    declare @sRuleLongName varchar(50)
	declare @sRuleScript varchar(max)

	declare csrRule cursor 
	for
	
	select r.rule_id, r.rule_name, r.rule_description, rs.rule_script, r.var_multiplier 
	from dbo.[rule] as r, dbo.[rule_script] as rs
	where r.rule_id = rs.rule_id
	order by r.rule_id
 
	open csrRule
	fetch next from csrRule
	into @sShortName, @sRuleLongName, @sDescription, @sRuleScript, @nSummaryMultiplierColimn

	while @@fetch_status = 0 
	begin

		Set @sShortName = Ltrim(Rtrim(@sShortName));

		-- Insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'RULE', @sShortName, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_RULE', 'rule_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID

		insert into lm_rule (version_id, rule_id, short_name, long_name, description, script, summary_multiplier_column) 
			values (@nVersionID, @nNextID, @sShortName, @sRuleLongName, @sDescription, @sRuleScript, @nSummaryMultiplierColimn)

		-- Get next Row
		fetch next from csrRule
		into @sShortName, @sRuleLongName, @sDescription, @sRuleScript, @nSummaryMultiplierColimn
	end

	close csrRule
	deallocate csrRule
    
	-- Set Summary Rule
	update lm_rule
	set type = 'S'
	where isnull(summary_multiplier_column, -1) > 0

	-- Set Scorecard
	update lm_rule
	set scorecard = 'Y'
	where short_name in
	(
		'SC_7-150', 'SC_AMF2', 'SC_CRRPT', 'SC_HASCR', 'SC_IN30+', 'SC_INSTL', 
		'SC_MJDRG', 'SC_MRCT', 'SC_MRDQ2', 'SC_NETRV', 'SC_PCTD2', 'SC_SATIS', 
		'SC_THNTK', 'SC_TL', 'SC_TL12M', 'SC_TL60', 'SC_TL90', 'SC_WRST',
		'SC2ADV', 'SC2AMF', 'SC2BANK', 'SC2INQ5M', 'SC2LSOLD', 'SC2MRDLQ', 'SC2MXDLQ', 
		'SC2NETIN', 'SC2NETRV', 'SC2NOOBJ', 'SC2PCTND', 'SC2PIN', 'SC2RTOLD', 'SC2TL90', 'SC2VAGE'
	)

	-- Set Underwriting Action Rules
	update lm_rule
	set underwriting_action = 'Y'
	where short_name in
	(
		'AMTFIN2', 'BOTHAUT2', 'CSMXAUT2', 'TL2'
	)

-- LM_RULE_VARIABLE

	declare @nRuleId int
	declare @nRuleSequence int
	declare @sDataType char(1)
	declare @sName varchar(60)
	declare @sLabel varchar(60)
	declare @sOperator char(2)
	declare @sSummaryType char(1)
	declare @sDropDownSelectable char(1)

	declare csrRuleVariable cursor 
	for
	
	select r.rule_id, rv.var_sequence, rv.var_name, rv.var_datatype, 
		rv.label, rv.compare_operator, rv.summary_operator, rv.selectable, r.short_name
	from lm_rule as r, dbo.rule_variables as rv
	where r.short_name = rv.rule_id
	and r.version_id > @nVersionIdPrior

	open csrRuleVariable
	fetch next from csrRuleVariable
	into @nRuleId, @nRuleSequence, @sName, @sDataType, @sLabel, @sOperator, @sSummaryType, @sDropDownSelectable, @sShortName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		from lm_version
		 where item_type = 'RULE'
		  and item_name = @sShortName
		  and @nLenderId = Lender_id
		  and version_id > @nVersionIdPrior;


		insert into lm_rule_variable (version_id, rule_id, variable_sequence, [name], data_type, label, operator, summary_type, drop_down_selectable) 
			values (@nVersionID, @nRuleId, @nRuleSequence, @sName, @sDataType, @sLabel, @sOperator, @sSummaryType, @sDropDownSelectable)

		-- Get next Row
		fetch next from csrRuleVariable
		into @nRuleId, @nRuleSequence, @sName, @sDataType, @sLabel, @sOperator, @sSummaryType, @sDropDownSelectable, @sShortName
	end

	close csrRuleVariable
	deallocate csrRuleVariable

-- LM_RULE_DROPDOWN_LIST
print 'LM_RULE_DROPDOWN_LIST'
	declare @sRuleVaribaleName varchar(60)
	declare @sValue varchar(10)

	declare csrRuleDDList cursor 
	for
	
	select var_name, field_name
	from dbo.rule_dropdown_list
	order by var_name, field_name
 
	open csrRuleDDList
	fetch next from csrRuleDDList
	into @sRuleVaribaleName, @sValue

	-- Insert into LM_VERSION
	exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'RULE_DROPDOWN_LIST', '', @nItemId, @sComment, @sApplication, 'N' 

	while @@fetch_status = 0 
	begin

		insert into lm_rule_dropdown_list (version_id, rule_variable_name, [value]) 
			values (@nVersionID, @sRuleVaribaleName, @sValue)

		-- Get next Row
		fetch next from csrRuleDDList
		into @sRuleVaribaleName, @sValue
	end

	close csrRuleDDList
	deallocate csrRuleDDList
	
-- LM_TOOL
 print 'LM_TOOL'  
	declare @sName1 varchar(10)
	declare @sName2 varchar(10)
	declare @sName3 varchar(10)
	declare @sName4 varchar(10)
	declare @sName5 varchar(10)
	declare @sName6 varchar(10)
	declare @sName7 varchar(10)
	declare @sName8 varchar(10)
	declare @sName9 varchar(10)
	declare @sName10 varchar(10)
	declare @sProprietary char(1)

	declare csrTool cursor 
	for
	
	select lt.tool_id, lt.tool_description, lt.notes, lt.status, r.rule_id, r.proprietary, lt.maximum_penalty, plus_or_minus, 
		lt.name_1, lt.name_2, lt.name_3, lt.name_4, lt.name_5, 
		lt.name_6, lt.name_7, lt.name_8, lt.name_9, lt.name_10, lt.modifier
	from dbo.lender_tools as lt
	left outer join lm_rule as r
		on lt.rule_id = r.short_name
	where lt.lender_id = @sLenderShortName
		-- XXX and r.Version_id > @nVersionIdPrior
	order by lt.tool_id

	open csrTool

	fetch next from csrTool
	into @sShortName, @sDescription, @sNotes, @sStatus, @nRuleID, @sProprietary, @nMaxPenalty, @sPenaltySign, 
			@sName1, @sName2, @sName3, @sName4, @sName5, @sName6, @sName7, @sName8, @sName9, @sName10, @sModifier

	while @@fetch_status = 0 
	begin

		Set @sShortName = Ltrim(Rtrim(@sShortName));

		-- insert into LM_VERSION
		exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'TOOL', @sShortName, @nItemId, @sComment, @sApplication, 'N' 
		exec @nNextID = LM_GetNextID 'LM_TOOL', 'tool_id'
		Update LM_VERSION Set ITEM_ID = @nNextID Where Version_id = @nVersionID


		insert into lm_tool (version_id, tool_id, short_name, long_name, description, status, rule_id, proprietary, penalty_maximum, penalty_sign, 
                      name_1, name_2, name_3, name_4, name_5, name_6, name_7, name_8, name_9, name_10) 
			values (@nVersionID, @nNextID, @sShortName, '', @sDescription, @sStatus, @nRuleID, @sProprietary, @nMaxPenalty, @sPenaltySign,
					@sName2, @sName3, @sName4, @sName5, @sName6, @sName7, @sName8, @sName9, @sName10, @sModifier)

		-- Get next Row
		fetch next from csrTool
		into @sShortName, @sDescription, @sNotes, @sStatus, @nRuleID, @sProprietary, @nMaxPenalty, @sPenaltySign, 
			@sName1, @sName2, @sName3, @sName4, @sName5, @sName6, @sName7, @sName8, @sName9, @sName10, @sModifier
	end

	close csrTool
	deallocate csrTool


-- LM_PROFILE_TOOL
print 'LM_PROFILE_TOOL'
	declare @nToolSequence int
	declare @nToolId int

	declare csrProfileTool cursor 
	for
	
	select p.profile_id, t.tool_id, pt.tool_sequence, p.long_name
	from dbo.profile_tools as pt, lm_profile as p, lm_tool as t
	where pt.profile_table_id = p.old_profile_id
	and pt.tool_id = t.short_name
	and pt.lender_id = @sLenderShortName
	--xxxand p.version_id > @nVersionIdPrior
	--xxxand t.version_id > @nVersionIdPrior
	order by p.profile_id, pt.tool_sequence 

	open csrProfileTool

	fetch next from csrProfileTool
	into @nProfileId, @nToolId, @nToolSequence, @sLongName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		 from lm_version
		  where item_type = 'PROFILE'
		   and item_name = @sLongName
		   and @nLenderId = Lender_id;


		insert into lm_profile_tool (version_id, profile_id, tool_id, tool_sequence) 
			values (@nVersionID, @nProfileId, @nToolId, @nToolSequence)

		-- Get next Row
		fetch next from csrProfileTool
		into @nProfileId, @nToolId, @nToolSequence, @sLongName
	end

	close csrProfileTool
	deallocate csrProfileTool

-- LM_TOOL_ROW
print 'LM_TOOL_ROW'
	declare @sInVar1 varchar(10)
	declare @sInVar2 varchar(10)
	declare @sInVar3 varchar(10)
	declare @sInVar4 varchar(10)
	declare @sInVar5 varchar(10)
	declare @sInVar6 varchar(10)
	declare @sInVar7 varchar(10)
	declare @sInVar8 varchar(10)
	declare @sInVar9 varchar(10)
	declare @sInVar10 varchar(10)

	declare @sAction char(1)
	declare @sCalculationType char(1)
	declare @sDenialCode char(1)
	declare @nGoto int
	declare @sReferType char(1)
	declare @nAAId int


	declare csrToolRow cursor 
	for
	
	select tool_id, row_sequence, in_var_1, in_var_2, in_var_3, in_var_4, in_var_5, in_var_6, in_var_7, in_var_8, in_var_9, in_var_10,
	action, penalty, plus_or_minus, maximum_penalty, calculation_operator, denial_code, goto_profile_table_id, refer, adverse_action_id
	from
	 dbo.tool_rows as tr
	left outer join lm_adverse_action as aa
		on tr.adverse_action_number = aa.short_name
	where
	 lender_id = @sLenderShortName
	order by
	 tool_id, row_sequence
 
	open csrToolRow

	fetch next from csrToolRow
	into @sOldStringID, @nOldSequence, @sInVar1, @sInVar2, @sInVar3, @sInVar4, @sInVar5, @sInVar6, @sInVar7, @sInVar8, @sInVar9, @sInVar10,
           @sAction, @nPenalty, @sPenaltySign, @nMaxPenalty, @sCalculationType, @sDenialCode, @nGoto, @sReferType, @nAAId

	while @@fetch_status = 0 
	begin
		-- Get Version for Tool
		select @nVersionID = max(version_id)
		   from lm_version
		  where item_type = 'TOOL'
		  and item_name = @sOldStringID
		  and @nLenderId = Lender_id
		  --XXX and version_id > @nVersionIdPrior;


		exec @nNextID = LM_GetNextID 'LM_TOOL_ROW', 'tool_row_id'

		insert into lm_tool_row (version_id, tool_row_id, in_var_1, in_var_2, in_var_3, in_var_4, in_var_5, in_var_6, in_var_7, in_var_8,in_var_9, in_var_10,
                                   action, penalty, penalty_sign, penalty_maximum, calculation_type, denial_code, goto_profile_id, refer_type, 
                                    adverse_action_id, old_tool_id, old_tool_row_sequence) 
			values (@nVersionID, @nNextID, @sInVar1, @sInVar2, @sInVar3, @sInVar4, @sInVar5, @sInVar6, @sInVar7, @sInVar8, @sInVar9, @sInVar10,
                      @sAction, @nPenalty, @sPenaltySign, @nMaxPenalty, @sCalculationType, @sDenialCode, @nGoto, @sReferType, @nAAId, @sOldStringID, @nOldSequence)

		-- Get next Row
		fetch next from csrToolRow
		into @sOldStringID, @nOldSequence, @sInVar1, @sInVar2, @sInVar3, @sInVar4, @sInVar5, @sInVar6, @sInVar7, @sInVar8, @sInVar9, @sInVar10,
           @sAction, @nPenalty, @sPenaltySign,  @nMaxPenalty, @sCalculationType, @sDenialCode, @nGoto, @sReferType, @nAAId
	end

	close csrToolRow
	deallocate csrToolRow

	-- Change Refer Type N to Null
	update LM_TOOL_ROW
	set refer_type = NULL
	where refer_type = 'N'


-- LM_TOOL_TOOL_ROW
 print 'LM_TOOL_TOOL_ROW'  
	declare @nToolRowId int
	declare @nToolRowSequence int

	declare csrToolToolRow cursor 
	for
	
	select tool_id, tool_row_id, old_tool_row_sequence, short_name
	from lm_tool as t, lm_tool_row as tr
	where t.short_name = tr.old_tool_id
	and t.version_id > @nVersionIdPrior
	and tr.version_id > @nVersionIdPrior
 
	open csrToolToolRow
	fetch next from csrToolToolRow
	into @nToolID, @nToolRowId, @nToolRowSequence, @sShortName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		 from lm_version 
		 where item_type = 'TOOL' 
		 and item_name = @sShortName
		 and @nLenderId = Lender_id;


		insert into lm_tool_tool_row (version_id, tool_id, tool_row_id, tool_row_sequence) 
			values (@nVersionID, @nToolID, @nToolRowId, @nToolRowSequence)

		-- Get next Row
		fetch next from csrToolToolRow
		into @nToolID, @nToolRowId, @nToolRowSequence, @sShortName
	end

	close csrToolToolRow
	deallocate csrToolToolRow

-- LM_TOOL_ROW_STIPULATION
 print 'LM_TOOL_ROW_STIPULATION' 
    declare @nStipulationId int

	declare csrToolRowStipulation cursor 
	for
	
	select tool_row_id, s.stipulation_id, old_tool_id
	from
	 dbo.tool_row_stipulations as trs,
	 dbo.lm_tool_row as tr,
	 dbo.lm_stipulation as s
	where trs.tool_id = tr.old_tool_id
	and trs.row_sequence = tr.old_tool_row_sequence
	and trs.stipulation_id = s.short_name
	and trs.lender_id = @sLenderShortName
	and tr.version_id > @nVersionIdPrior
	and s.version_id > @nVersionIdPrior

	open csrToolRowStipulation

	fetch next from csrToolRowStipulation
	into @nToolRowId, @nStipulationId, @sShortName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		from lm_version 
		where item_type = 'TOOL' 
		and item_name = @sShortName
		and @nLenderId = Lender_id
		and version_id > @nVersionIdPrior;


		insert into lm_tool_row_stipulation (version_id, tool_row_id, stipulation_id) 
			values (@nVersionID, @nToolRowId, @nStipulationId)

		-- Get next Row
		fetch next from csrToolRowStipulation
		into @nToolRowId, @nStipulationId, @sShortName
	end

	close csrToolRowStipulation
	deallocate csrToolRowStipulation

-- LM_TOOL_ROW_REFER
print 'LM_TOOL_ROW_REFER'  
    declare @nReferId int

	declare csrToolRowRefer cursor 
	for
	
	select tool_row_id, refer_id, old_tool_id
	from 
		dbo.tool_row_refer_reasons as trrr,
		lm_tool_row as tr,
		lm_refer as r
	where trrr.tool_id = tr.old_tool_id
	and trrr.row_sequence = tr.old_tool_row_sequence
	and trrr.refer_reason_id = r.short_name
	and trrr.lender_id = @sLenderShortName
	and tr.version_id > @nVersionIdPrior
	and r.version_id > @nVersionIdPrior

	open csrToolRowRefer
	fetch next from csrToolRowRefer
	into @nToolRowId, @nReferId, @sShortName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		 from lm_version
		  where item_type = 'TOOL'
		   and item_name = @sShortName
		   and @nLenderId = Lender_id
		   and version_id > @nVersionIdPrior;


		insert into lm_tool_row_refer (version_id, tool_row_id, refer_id) 
			values (@nVersionID, @nToolRowId, @nReferId)

		-- Get next Row
		fetch next from csrToolRowRefer
		into @nToolRowId, @nReferId, @sShortName
	end

	close csrToolRowRefer
	deallocate csrToolRowRefer

-- LM_TOOL_ROW_UNDERWRITING_ACTION
 print 'LM_TOOL_ROW_UNDERWRITING_ACTION' 
    --declare @nReferId int

	declare csrToolRowUA cursor 
	for
	
	select tr.tool_row_id, ua.underwriting_action_id, old_tool_id
	from
	 dbo.tool_row_underwriting_actions as trua,
	 dbo.lm_tool_row as tr,
	 dbo.lm_underwriting_action as ua
	where trua.tool_id = tr.old_tool_id
	and trua.row_sequence = tr.old_tool_row_sequence
	and trua.underwriting_action_id = ua.short_name
	and  trua.lender_id = @sLenderShortName
	and tr.version_id > @nVersionIdPrior
	and ua.version_id > @nVersionIdPrior

	open csrToolRowUA

	fetch next from csrToolRowUA
	into @nToolRowId, @nUAId, @sShortName

	while @@fetch_status = 0 
	begin

		select @nVersionID = max(version_id)
		 from lm_version where item_type = 'TOOL'
		  and item_name = @sShortName
		  and @nLenderId = Lender_id;


		insert into lm_tool_row_underwriting_action (version_id, tool_row_id, underwriting_action_id) 
			values (@nVersionID, @nToolRowId, @nUAId)

		-- Get next Row
		fetch next from csrToolRowUA
		into @nToolRowId, @nUAId, @sShortName
	end

	close csrToolRowUA
	deallocate csrToolRowUA

-- LM_CR_STEP_INSTRUCTION

	declare @nPersonSequence int
	declare @nStep int
	declare @sPull1 char(1)
	declare @sPull2 char(1)
	declare @sPull3 char(1)
	declare @sMerge1 char(1)
	declare @sMerge2 char(1)
	declare @sMerge3 char(1)
	declare @nNextStepNoCreditReport int
	declare @nNextStepNoHitCreditReport int
	declare @nNextStepThinCreditReport int
	declare @nNextStepCreditScore int
	declare @nNextStepWeakCredit int
	declare @nNextStepDerogatoryCredit int
	declare @nNextStepAmountFinanced int
	declare @nNextStepAlert int
	declare @nNextStepOther int

	-- Insert into LM_VERSION
	exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'CR_STEP_INSTRUCTION', '', @nItemId, @sComment, @sApplication, 'N' 

	-- Applicant - Person 1
	declare csrCRStepInstruction cursor 
	for
	select 1 as person_sequence, step_no, 
			isnull(CASE cr_per1_ch1 WHEN 'M' THEN 'Y' END, 'N'), isnull(CASE cr_per1_ch2 WHEN 'M' THEN 'Y' END, 'N'), isnull(CASE cr_per1_ch3 WHEN 'M' THEN 'Y' END, 'N'), 
			isnull(jmd_per1_ch1, 'N'), isnull(jmd_per1_ch2, 'N'), isnull(jmd_per1_ch3, 'N'),
			null, no_hit, thin_report, credit_score, weak_credit, derog_credit, amt_financed, alert, other
	from dbo.lender_instructions
	where lender_id = @sLenderShortName
    and appl_type = 'I'
	order by person_sequence, step_no
 
	open csrCRStepInstruction

	fetch next from csrCRStepInstruction
	into @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
		@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
		@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther

	while @@fetch_status = 0 
	begin

		insert into lm_cr_step_instruction (version_id, person_sequence, step, pull_1, pull_2, pull_3, merge_1, merge_2, merge_3, 
					next_step_no_credit_report, next_step_no_hit_credit_report, next_step_thin_credit_report, next_step_credit_report_score, 
					next_step_weak_credit_report, next_step_derogatory_credit, next_step_amount_financed, next_step_alert, next_step_other)
			values(@nVersionID, @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
					@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
						@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther)

		-- Get next Row
		fetch next from csrCRStepInstruction
		into @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
			@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
			@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther
	end

	close csrCRStepInstruction
	deallocate csrCRStepInstruction

	-- Joint Applicant - Person 2
	declare csrCRStepInstruction cursor 
	for
	select 2 as person_sequence, step_no, 
			isnull(CASE cr_per2_ch1 WHEN 'M' THEN 'Y' END, 'N'), isnull(CASE cr_per2_ch2 WHEN 'M' THEN 'Y' END, 'N'), isnull(CASE cr_per2_ch3 WHEN 'M' THEN 'Y' END, 'N'), 
			isnull(jmd_per2_ch1, 'N'), isnull(jmd_per2_ch2, 'N'), isnull(jmd_per2_ch3, 'N'),
			null, null, null, null, null, null, null, null, null
	from dbo.lender_instructions
	where lender_id = @sLenderShortName
    and appl_type = 'J'
	order by person_sequence, step_no
 

	open csrCRStepInstruction
	fetch next from csrCRStepInstruction
	into @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
		@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
		@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther

	while @@fetch_status = 0 
	begin

		insert into lm_cr_step_instruction (version_id, person_sequence, step, pull_1, pull_2, pull_3, merge_1, merge_2, merge_3, 
					next_step_no_credit_report, next_step_no_hit_credit_report, next_step_thin_credit_report, next_step_credit_report_score, 
					next_step_weak_credit_report, next_step_derogatory_credit, next_step_amount_financed, next_step_alert, next_step_other)
			values(@nVersionID, @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
					@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
						@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther)

		-- Get next Row
		fetch next from csrCRStepInstruction
		into @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
			@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
			@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther
	end

	close csrCRStepInstruction
	deallocate csrCRStepInstruction

	-- CoMaker - Person 3
	declare csrCRStepInstruction cursor 
	for
	select 3 as person_sequence, step_no, 
			isnull(CASE cr_per3_ch1 WHEN 'M' THEN 'Y' END, 'N'), isnull(CASE cr_per3_ch2 WHEN 'M' THEN 'Y' END, 'N'), isnull(CASE cr_per3_ch3 WHEN 'M' THEN 'Y' END, 'N'), 
			isnull(jmd_per3_ch1, 'N'), isnull(jmd_per3_ch2, 'N'), isnull(jmd_per3_ch3, 'N'),
			null, no_hit, thin_report, credit_score, weak_credit, derog_credit, amt_financed, alert, other
	from dbo.lender_instructions
	where lender_id = @sLenderShortName
    and appl_type = 'C'
	order by person_sequence, step_no
 
	open csrCRStepInstruction

	fetch next from csrCRStepInstruction
	into @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
		@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
		@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther

	while @@fetch_status = 0 
	begin

		insert into lm_cr_step_instruction (version_id, person_sequence, step, pull_1, pull_2, pull_3, merge_1, merge_2, merge_3, 
					next_step_no_credit_report, next_step_no_hit_credit_report, next_step_thin_credit_report, next_step_credit_report_score, 
					next_step_weak_credit_report, next_step_derogatory_credit, next_step_amount_financed, next_step_alert, next_step_other)
			values(@nVersionID, @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
					@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
						@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther)

		-- Get next Row
		fetch next from csrCRStepInstruction
		into @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
			@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
			@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther
	end

	close csrCRStepInstruction
	deallocate csrCRStepInstruction

	-- Joint CoMaker - Person 4
	declare csrCRStepInstruction cursor 
	for
	select 4 as person_sequence, step_no, 
			isnull(CASE cr_per4_ch1 WHEN 'M' THEN 'Y' END, 'N'), isnull(CASE cr_per4_ch2 WHEN 'M' THEN 'Y' END, 'N'), isnull(CASE cr_per4_ch3 WHEN 'M' THEN 'Y' END, 'N'), 
			isnull(jmd_per4_ch1, 'N'), isnull(jmd_per4_ch2, 'N'), isnull(jmd_per4_ch3, 'N'),
			null, null, null, null, null, null, null, null, null
	from dbo.lender_instructions
	where lender_id = @sLenderShortName
    and appl_type = 'CJ'
	order by person_sequence, step_no
 
	open csrCRStepInstruction

	fetch next from csrCRStepInstruction
	into @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
		@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
		@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther

	while @@fetch_status = 0 
	begin

		insert into lm_cr_step_instruction (version_id, person_sequence, step, pull_1, pull_2, pull_3, merge_1, merge_2, merge_3, 
					next_step_no_credit_report, next_step_no_hit_credit_report, next_step_thin_credit_report, next_step_credit_report_score, 
					next_step_weak_credit_report, next_step_derogatory_credit, next_step_amount_financed, next_step_alert, next_step_other)
			values(@nVersionID, @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
					@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
						@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther)

		-- Get next Row
		fetch next from csrCRStepInstruction
		into @nPersonSequence, @nStep, @sPull1, @sPull2, @sPull3, @sMerge1, @sMerge2, @sMerge3,
			@nNextStepNoCreditReport, @nNextStepNoHitCreditReport, @nNextStepThinCreditReport, @nNextStepCreditScore, 
			@nNextStepWeakCredit, @nNextStepDerogatoryCredit, @nNextStepAmountFinanced, @nNextStepAlert, @nNextStepOther
	end 

	close csrCRStepInstruction
	deallocate csrCRStepInstruction

-- Update Columns
print '  Update Columns...'
	update lm_cr_step_instruction
	set next_step_no_hit_credit_report = null
	where next_step_no_hit_credit_report = 0
	and version_id > @nVersionIdPrior;

	update lm_cr_step_instruction
	set next_step_thin_credit_report = null
	where next_step_thin_credit_report = 0
	and version_id > @nVersionIdPrior;

	update lm_cr_step_instruction
	set next_step_credit_report_score = null
	where next_step_credit_report_score = 0
	and version_id > @nVersionIdPrior;

	update lm_cr_step_instruction
	set next_step_weak_credit_report = null
	where next_step_weak_credit_report = 0
	and version_id > @nVersionIdPrior;

	update lm_cr_step_instruction
	set next_step_derogatory_credit = null
	where next_step_derogatory_credit = 0
	and version_id > @nVersionIdPrior;

	update lm_cr_step_instruction
	set next_step_amount_financed = null
	where next_step_amount_financed = 0
	and version_id > @nVersionIdPrior;

	update lm_cr_step_instruction
	set next_step_alert = null
	where next_step_alert = 0
	and version_id > @nVersionIdPrior;

	update lm_cr_step_instruction
	set next_step_other = null
	where next_step_other = 0
	and version_id > @nVersionIdPrior;

-- LM_CR_ZIP_TABLE
print 'LM_CR_ZIP_TABLE'

	-- insert into LM_VERSION
	exec @nVersionID = LM_InsertLMVersion @nLenderId, @sUsername,  'CR_ZIP_TABLE', '', @nItemId, @sComment, @sApplication, 'N' 

	declare @nZipCode as int
	declare @sZipCode3 as varchar(3)
	
	declare csrZipTable cursor 
	for
	 select zip_code 

		from dbo.zip_table
		where lender_id = @sLenderShortName
		order by zip_code
 
	open csrZipTable
	fetch next from csrZipTable

	while @@fetch_status = 0 
	begin
	
		fetch next from csrZipTable
		into @nZipCode
	
		Set @sZipCode3=  case 
			when  Len(@nZipCode) = 1 then '00' + cast(@nZipCode as varchar(3))
			when  Len(@nZipCode) = 2 then '0' + cast(@nZipCode as varchar(3))
			else cast(@nZipCode as varchar(3))
			end
			
			declare @cb_choice_1 char(2);
			declare @cb_choice_2 char(2);
			declare @cb_choice_3 char(2);
			
			Select
			  @cb_choice_1 = cb_choice_1,
			  @cb_choice_2 = cb_choice_2,
			  @cb_choice_3 = cb_choice_3
			From dbo.ZIP_TABLE
			Where 
					lender_id = @sLenderShortName
				and zip_code = @nZipCode

	
	declare @nCount int
	select @nCount=count(*) from LM_CR_ZIP_TABLE where @sZipCode3 = zip_code_3

	if @nCount = 0
	begin
		insert into LM_CR_ZIP_TABLE
				(version_id, zip_code_3, credit_bureau_1, credit_bureau_2, credit_bureau_3)
		 Values ( @nVersionID, @sZipCode3, @cb_choice_1, @cb_choice_2, @cb_choice_3)
	end

	fetch next from csrZipTable
	end  -- cursor
	
	close csrZipTable
	deallocate csrZipTable

----------------------
print '  Apply Name...'
	-- Name the version with the current date-time.
	declare @versionName varchar(128)
	set @dtEnd = getdate()
	Set @versionName = 'Migrated LM Version '+ replace(@dtEnd,':','_')

	update LM_VERSION
	Set VERSION_NAME = @versionName
		where @nVersionID = version_id
  
	Commit Transaction
	End Try

Begin Catch
	Rollback Transaction
	 DBCC CHECKIDENT (LM_VERSION, RESEED, 1)
	Select @sMsg = 'LM_MigrateMDE6 Failed: ' + Error_Message() + ' at line: ' + cast( ERROR_LINE() as varchar);
	raiserror (@sMsg, 15, 1);
	return 0; -- Failed

End Catch
	Set @sMsg = 'Migration Completed. Elapsed time: ' + cast( datediff(second,  @dtStart, @dtEnd) as varchar) + ' (seconds)';
	print @sMsg
return 1	-- Success
END -- End of Stored Procedure.


------- Trailer ----------
Go













