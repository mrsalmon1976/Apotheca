IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = '{SCHEMA}' 
                 AND  TABLE_NAME = 'Categories'))
BEGIN

	CREATE TABLE [{SCHEMA}].[Categories](
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [varchar](100) NOT NULL,
		[Description] [varchar](255) NULL,
		[CreatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [{SCHEMA}].[Categories] ADD  CONSTRAINT [DF_Categories_Id]  DEFAULT (newsequentialid()) FOR [Id]
END