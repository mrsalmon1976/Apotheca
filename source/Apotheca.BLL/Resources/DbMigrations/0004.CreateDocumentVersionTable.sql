IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = '{SCHEMA}' 
                 AND  TABLE_NAME = 'DocumentVersions'))
BEGIN

	CREATE TABLE [{SCHEMA}].[DocumentVersions](
		Id uniqueidentifier rowguidcol NOT NULL,
		VersionNo int NOT NULL,
		[FileName] nvarchar(255) NOT NULL,
		[Description] nvarchar(max) NULL,
		[FileContents] varbinary(MAX) NOT NULL DEFAULT (0x),
		Extension nvarchar(100) NOT NULL,
		MimeType nvarchar(255) NOT NULL,
		CreatedOn datetime NOT NULL,
		CreatedByUserId uniqueidentifier NOT NULL,
		CONSTRAINT [PK_DocumentVersions] PRIMARY KEY CLUSTERED 
		(
			[Id], [VersionNo] 
		) ON [PRIMARY]
	)

END
