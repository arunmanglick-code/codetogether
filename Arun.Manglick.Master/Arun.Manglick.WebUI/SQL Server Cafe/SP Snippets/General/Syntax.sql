-- SYNTAXs
-----------------------------------------------------------------

-- CASE STATMENT
CASE <optional_input_value>
WHEN <expression_1> THEN <result_1>
WHEN <expression_N> THEN <result_N>
ELSE <end_result>
END
-----------------------------------------------------------------

-- IF STATMENT
IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES)> 20
	BEGIN
		SELECT TOP 20 *	FROM INFORMATION_SCHEMA.TABLES
	END
ELSE
BEGIN
	SELECT TOP 10 * FROM INFORMATION_SCHEMA.TABLES
END
-----------------------------------------------------------------

-- WHILE STATMENT
WHILE @CustomerID IS NOT NULL
BEGIN
	PRINT @CustomerID
	IF (@CustomerID)= 'CACTU'
		BEGIN
			CONTINUE
		END
	SELECT @CustomerID = MIN(CustomerId)
	FROM Customers
	WHERE CustomerID > @CustomerID
END
-----------------------------------------------------------------

-- RAISERROR STATMENT
RAISERROR (<message_id_OR_message_string>,<severity_level>,<message_state>,<arguments_N> )
WITH <LOG_or_NOWAIT_or_SETERROR>
-----------------------------------------------------------------