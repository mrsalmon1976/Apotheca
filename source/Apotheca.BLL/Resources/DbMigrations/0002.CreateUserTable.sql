IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = '{SCHEMA}' 
                 AND  TABLE_NAME = 'Users'))
BEGIN

	CREATE TABLE [{SCHEMA}].[Users](
		[Id] [uniqueidentifier] NOT NULL,
		[Email] [nvarchar](200) NOT NULL,
		[Password] [nvarchar](255) NOT NULL,
		[Salt] [nvarchar](100) NOT NULL,
		[FirstName] [nvarchar](100) NOT NULL,
		[Surname] [nvarchar](100) NOT NULL,
		[Role] [nvarchar](50) NOT NULL,
		[ApiKey] [nvarchar](100) NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [{SCHEMA}].[Users] ADD  CONSTRAINT [DF_Users_Id]  DEFAULT (newsequentialid()) FOR [Id]
END