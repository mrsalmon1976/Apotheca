IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = '{SCHEMA}' 
                 AND  TABLE_NAME = 'AuditLogs'))
BEGIN

	CREATE TABLE [{SCHEMA}].[AuditLogs](
		[Id] [int] IDENTITY (1,1) NOT NULL,
		[Entity] [nvarchar](255) NOT NULL,
		[Key] [nvarchar](75) NOT NULL,
		[Action] [nvarchar](50) NOT NULL,
		[AuditDateTime] [datetime] NOT NULL,
		[UserId] uniqueidentifier NOT NULL,
		[EntityDetail] [nvarchar](max) NULL,
	 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [{SCHEMA}].[AuditLogs] ADD CONSTRAINT [FK_AuditLog_UserId] FOREIGN KEY (UserId) REFERENCES [{SCHEMA}].[Users](Id)
END