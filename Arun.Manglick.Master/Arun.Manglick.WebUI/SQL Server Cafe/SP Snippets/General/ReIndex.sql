
exec sp_dbcmptlevel 'DATABASE_NAME', 90;  -- Set the DATABASE_NAME to the target MDE database.

DECLARE @Database VARCHAR(255)
Set @Database = 'DATABASE_NAME'   -- Set the DATABASE_NAME to the target MDE database

DECLARE @Table VARCHAR(255)
DECLARE @cmd NVARCHAR(500)
DECLARE @fillfactor INT

SET @cmd = 'DECLARE TableCursor CURSOR FOR SELECT table_catalog + ''.'' + table_schema + ''.'' + table_name as tableName  
	    FROM ' + @Database + '.INFORMATION_SCHEMA.TABLES WHERE table_type = ''BASE TABLE'''  


EXEC (@cmd) 

OPEN TableCursor  

FETCH NEXT FROM TableCursor INTO @Table  
WHILE @@FETCH_STATUS = 0  
BEGIN  
DBCC DBREINDEX(@Table,' ',90)  
FETCH NEXT FROM TableCursor INTO @Table  
END  

CLOSE TableCursor  
DEALLOCATE TableCursor 
EXEC sp_updatestats 


