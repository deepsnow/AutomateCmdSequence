use master

DECLARE @Name nvarchar(1000);

DECLARE testdb_cursor CURSOR FOR
SELECT 'ALTER DATABASE' + '[' + NAME + ']' + ' SET SINGLE_USER WITH ROLLBACK IMMEDIATE DROP DATABASE ' + '[' + NAME + ']' FROM sys.sysdatabases where name like 'CASE_%'

OPEN testdb_cursor;

-- Perform the first fetch and store the value in a variable.
FETCH NEXT FROM testdb_cursor
INTO @Name;

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
	BEGIN

		-- Concatenate and display the current values in the variables.
		exec sp_executesql @Name;

		-- This is executed as long as the previous fetch succeeds.
		FETCH NEXT FROM testdb_cursor
		INTO @Name;
	   
	END

CLOSE testdb_cursor;
DEALLOCATE testdb_cursor;