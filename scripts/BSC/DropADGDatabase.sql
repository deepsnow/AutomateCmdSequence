ALTER DATABASE ADG SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
DROP DATABASE ADG
GO
  
DECLARE @name varchar(max)	
DECLARE @dropscript varchar(max)	
DECLARE @LINEBREAK varchar(2)
SET @LINEBREAK = CHAR(13)+CHAR(10) 
DECLARE db_cursor CURSOR READ_ONLY FOR
(SELECT name
FROM sys.syslogins
WHERE (name LIKE 'ADG[0123456789]%[_]%'))

OPEN db_cursor

FETCH NEXT FROM db_cursor
INTO @name

WHILE @@FETCH_STATUS = 0
BEGIN

	PRINT @name
	SET @dropscript = 'IF EXISTS (SELECT * FROM sys.server_principals WHERE name = N''' + @name +   ''')' + @LINEBREAK
	SET @dropscript = @dropscript + 'DROP LOGIN [' + @name + ']' + @LINEBREAK

	PRINT @dropscript
	EXEC (@dropscript)
	
	FETCH NEXT FROM db_cursor
	INTO @name

END

CLOSE db_cursor
DEALLOCATE db_cursor

SELECT name
FROM sys.syslogins
WHERE (name LIKE 'ADG[0123456789]%[_]%')
 
 
 
GO