
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

-- Alter Column
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[VOC_CTM_Rules]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	IF EXISTS (SELECT * FROM information_schema.columns WHERE table_name='VOC_CTM_Rules' AND column_name='RuleName')
		BEGIN
		ALTER TABLE [VOC_CTM_Rules] ALTER COLUMN [RuleName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
		PRINT '[VOC_CTM_Rules] TABLE ALETERED SUCCESSFULLY'
		END
	ELSE
		PRINT '[RuleName] COLUMN DOES NOT EXISTS'
ELSE
		PRINT '[VOC_CTM_Rules] TABLE DOES NOT EXISTS'

GO

-- Rename Column
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[VOC_CTM_Rules]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	IF EXISTS (SELECT * FROM information_schema.columns WHERE table_name='VOC_CTM_Rules' AND column_name='TAT_OrderPlacedToMsgSent')
		BEGIN
		EXEC sp_rename 'VOC_CTM_Rules.TAT_OrderPlacedToMsgSent', 'TAT_MsgSentToMsgReceived', 'COLUMN';
		PRINT 'TAT_OrderPlacedToMsgSent Column Renamed to TAT_MsgSentToMsgReceived SUCCESSFULLY'
		END
	ELSE
		PRINT 'TAT_OrderPlacedToMsgSent COLUMN DOES NOT EXISTS'
ELSE
		PRINT '[VOC_CTM_Rules] TABLE DOES NOT EXISTS'

GO

-- Add Column
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Devices]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	IF NOT EXISTS (SELECT * FROM information_schema.columns WHERE table_name='Devices' AND column_name='RecipientCode')
		ALTER TABLE Devices
		ADD RecipientCode VARCHAR(16) NULL 
GO

-- Add Constraint
ALTER TABLE [dbo].[GroupNotifyEvents]  WITH CHECK ADD  CONSTRAINT [FK_GroupNotifyEvents_GroupNotificationModules] FOREIGN KEY([GroupNotificationModuleID])
REFERENCES [dbo].[GroupNotificationModules] ([GroupNotificationModuleID])
GO
ALTER TABLE [dbo].[GroupNotifyEvents] CHECK CONSTRAINT [FK_GroupNotifyEvents_GroupNotificationModules]
GO