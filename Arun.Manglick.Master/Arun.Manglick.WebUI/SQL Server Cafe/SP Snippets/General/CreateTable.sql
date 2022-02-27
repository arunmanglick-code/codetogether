/****** Object:  Table [dbo].[VOC_CTM_Rules]    Script Date: 04/30/2009 12:29:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/******************************************************************************
**		File: Create_VOC_CTM_Rules.sql
**		Name: Create_VOC_CTM_Rules
**		Desc: Creating table to store the Critical Rule
**
**		Auth: Arun M
**		Date: 11-May-2009
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			-------------------------------------------
*******************************************************************************/


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VOC_CTM_Rules]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[VOC_CTM_Rules](
		[RuleID] [int] IDENTITY(1,1) NOT NULL,
		[InstitutionID] [int] NOT NULL,
		[FacilityID] [int] NOT NULL,
		[GroupID] [int] NOT NULL,
		[RuleName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Precedence] [int] NOT NULL,
		[FindingID] [int] NOT NULL,
		[TAT_OrderPlacedToComplete] [int] NULL,
		[TAT_ProcCompleteToMsgSent] [int] NULL,
		[TAT_OrderPlacedToMsgSent] [int] NULL,
		[TAT_OrderPlacedToMsgReceived] [int] NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NULL,
		[UpdatedBy] [int] NOT NULL,
		[Active] [bit] NOT NULL CONSTRAINT [DF_VOC_CTM_Rules_Active]  DEFAULT ((1)),
		CONSTRAINT [PK_VOC_CTM_Rules] PRIMARY KEY CLUSTERED 
		(
			[RuleID] ASC
		)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],


	) ON [PRIMARY]

--	ALTER TABLE [dbo].[VOC_CTM_Rules]  WITH CHECK ADD  CONSTRAINT [FK_VOC_CTM_Rules_Findings] FOREIGN KEY([FindingID])
--	REFERENCES [dbo].[Findings] ([FindingID])

--	ALTER TABLE [dbo].[VOC_CTM_Rules]  WITH CHECK ADD  CONSTRAINT [FK_VOC_CTM_Rules_Groups] FOREIGN KEY([GroupID])
--	REFERENCES [dbo].[Groups] ([GroupID])

--	ALTER TABLE [dbo].[VOC_CTM_Rules]  WITH CHECK ADD  CONSTRAINT [FK_VOC_CTM_Rules_Institutions] FOREIGN KEY([InstitutionID])
--	REFERENCES [dbo].[Institutions] ([InstitutionID])

--	ALTER TABLE [dbo].[VOC_CTM_Rules]  WITH CHECK ADD  CONSTRAINT [FK_VOC_CTM_Rules_Voc_Facility] FOREIGN KEY([FacilityID])
--	REFERENCES [dbo].[Voc_Facility] ([FacilityID])

--	ALTER TABLE [dbo].[VOC_CTM_Rules]  WITH CHECK ADD  CONSTRAINT [FK_VOC_CTM_Rules_VOC_Users] FOREIGN KEY([UpdatedBy])
--	REFERENCES [dbo].[VOC_Users] ([VOCUserID])


END
ELSE
 Print 'Table: [VOC_CTM_Rules] Already Exists'

GO
SET ANSI_PADDING OFF


