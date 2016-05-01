IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'{SCHEMA}' ) 
BEGIN
    EXEC('CREATE SCHEMA [{SCHEMA}] AUTHORIZATION [dbo]');
END