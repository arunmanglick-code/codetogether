
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VOC_CTM_Parameters]') AND type in (N'U'))
BEGIN

	IF NOT EXISTS (SELECT * FROM [VOC_CTM_Parameters] WHERE [ParameterName] = 'Procedure Code')
		BEGIN
			INSERT INTO [VOC_CTM_Parameters] (ParameterName,[Required])
			VALUES ('Procedure Code',1)
		END
	ELSE
		PRINT 'Procedure Code - ALREADY EXISTS'
	
	-------------------------------------------------------------------------------------
	IF NOT EXISTS (SELECT * FROM [VOC_CTM_Parameters] WHERE [ParameterName] = 'Procedure Description')
	BEGIN
		INSERT INTO [VOC_CTM_Parameters] (ParameterName,[Required])
		VALUES ('Procedure Description',1)
	END
	ELSE
	PRINT 'Procedure Description - ALREADY EXISTS'
	-------------------------------------------------------------------------------------
END
ELSE
 Print 'Table: [VOC_CTM_Parameters] Does Not Exists'
GO