use master

DECLARE @Name nvarchar(1000);

DECLARE testdb_cursor CURSOR FOR
SELECT [name] from sys.server_principals where type_desc = 'SQL_LOGIN' and sid <> 0x01 and name like '%ADG54%' and name <> 'ADG54' and name <> 'adg_user'

OPEN testdb_cursor;

-- Perform the first fetch and store the value in a variable.
FETCH NEXT FROM testdb_cursor
INTO @Name;

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
	BEGIN

		-- Execute the query.
		exec ('Drop Login [' + @Name + ']');
		
		-- This is executed as long as the previous fetch succeeds.
		FETCH NEXT FROM testdb_cursor
		INTO @Name;
	   
	END

CLOSE testdb_cursor;
DEALLOCATE testdb_cursor;