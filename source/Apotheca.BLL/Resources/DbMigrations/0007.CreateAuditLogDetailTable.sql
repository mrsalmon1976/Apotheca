IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = '{SCHEMA}' 
                 AND  TABLE_NAME = 'AuditLogDetails'))
BEGIN

	CREATE TABLE [{SCHEMA}].[AuditLogDetails](
		[Id] [int] IDENTITY (1,1) NOT NULL,
		[AuditLogId] [int] NOT NULL,
		[Column] [varchar](255) NOT NULL,
		[FromValue] [varchar](max) NOT NULL,
		[ToValue] [varchar](max) NOT NULL,
	 CONSTRAINT [PK_AuditLogDetail] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [{SCHEMA}].[AuditLogDetails] ADD CONSTRAINT [FK_AuditLogDetail_AuditLogId] FOREIGN KEY (AuditLogId) REFERENCES [{SCHEMA}].[AuditLogs](Id)
END