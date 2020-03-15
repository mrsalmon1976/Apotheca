IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = '{SCHEMA}' 
                 AND  TABLE_NAME = 'DocumentCategoryAssc'))
BEGIN

	CREATE TABLE [{SCHEMA}].[DocumentCategoryAssc](
		[Id] [int] IDENTITY (1,1) NOT NULL,
		DocumentId uniqueidentifier NOT NULL,
		DocumentVersionNo int NOT NULL,
		CategoryId uniqueidentifier NOT NULL,
		CONSTRAINT [PK_DocumentCategoryAssc] PRIMARY KEY CLUSTERED 
		(
			[Id]
		) ON [PRIMARY]
	)

	ALTER TABLE [{SCHEMA}].[DocumentCategoryAssc] ADD CONSTRAINT [FK_DocumentCategoryAssc_DocumentId] FOREIGN KEY (DocumentId) REFERENCES [{SCHEMA}].[Documents](Id)
	ALTER TABLE [{SCHEMA}].[DocumentCategoryAssc] ADD CONSTRAINT [FK_DocumentCategoryAssc_CategoryId] FOREIGN KEY (CategoryId) REFERENCES [{SCHEMA}].[Categories](Id)

END
