IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = '{SCHEMA}' 
                 AND  TABLE_NAME = 'UserCategoryAssc'))
BEGIN

	CREATE TABLE [{SCHEMA}].[UserCategoryAssc](
		[Id] [int] IDENTITY (1,1) NOT NULL,
		UserId uniqueidentifier NOT NULL,
		CategoryId uniqueidentifier NOT NULL,
		CONSTRAINT [PK_UserCategoryAssc] PRIMARY KEY CLUSTERED 
		(
			[Id]
		) ON [PRIMARY]
	)

	ALTER TABLE [{SCHEMA}].[UserCategoryAssc] ADD CONSTRAINT [FK_UserCategoryAssc_UserId] FOREIGN KEY (UserId) REFERENCES [{SCHEMA}].[Users](Id)
	ALTER TABLE [{SCHEMA}].[UserCategoryAssc] ADD CONSTRAINT [FK_UserCategoryAssc_CategoryId] FOREIGN KEY (CategoryId) REFERENCES [{SCHEMA}].[Categories](Id)

END
