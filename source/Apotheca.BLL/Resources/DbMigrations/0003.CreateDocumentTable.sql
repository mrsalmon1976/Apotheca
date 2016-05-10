IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'apotheca' 
                 AND  TABLE_NAME = 'Documents'))
BEGIN

	CREATE TABLE [{SCHEMA}].[Documents](
		Id uniqueidentifier rowguidcol NOT NULL,
		Name nvarchar(255) NOT NULL,
		[Description] nvarchar(max) NULL,
		[FileContents] varbinary(MAX) NOT NULL DEFAULT (0x),
		Extension nvarchar(100) NOT NULL,
		MimeType nvarchar(255) NOT NULL,
		CreatedOn datetime NOT NULL,
		CreatedByUserId uniqueidentifier NOT NULL,
		CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		) ON [PRIMARY]
	)

	ALTER TABLE [{SCHEMA}].[Documents] ADD  CONSTRAINT [DF_Documents_Id] DEFAULT (NEWSEQUENTIALID()) FOR [Id]
END

IF NOT EXISTS (SELECT TOP 1 1 FROM sys.fulltext_catalogs WHERE name = 'DocumentCatalog')
BEGIN
    CREATE FULLTEXT CATALOG DocumentCatalog;

	CREATE FULLTEXT INDEX ON [{SCHEMA}].[Documents]
	( 
		Name Language 1033,
		[Description] Language 1033,
		[FileContents] TYPE COLUMN Extension Language 1033     
	) 
	KEY INDEX PK_Documents ON DocumentCatalog; 
 
END