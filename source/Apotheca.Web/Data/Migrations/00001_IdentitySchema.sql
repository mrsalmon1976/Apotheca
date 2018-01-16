CREATE SCHEMA [identity]
GO

CREATE TABLE [identity].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](100) NULL,
    [NormalizedUserName] [nvarchar](256) NOT NULL,
    [Email] [nvarchar](256) NULL,
    [NormalizedEmail] [nvarchar](256) NULL,
	[FullName] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](256) NULL,
	[SecurityStamp] [nvarchar](256) NULL,
	[Discriminator] [nvarchar](128) NULL,
    [EmailConfirmed] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] [nvarchar](256) NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetime NULL,
    [PhoneNumber] [nvarchar](100) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL
 CONSTRAINT [PK_Identity_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
  IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO

CREATE TABLE [identity].[Roles](
	[Id] uniqueidentifier NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
    [NormalizedName] [nvarchar](256) NULL,
    ConcurrencyStamp [nvarchar](256) NULL
CONSTRAINT [PK_Identity_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
        ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

CREATE TABLE [identity].[UserTokens](
	[UserId] uniqueidentifier NOT NULL,
	[LoginProvider] [nvarchar](256) NOT NULL,
    [Name] [nvarchar](256) NOT NULL,
    [Value] [nvarchar](256) NULL
CONSTRAINT [PK_Identity_UserTokens] PRIMARY KEY NONCLUSTERED 
(
	[UserId], [LoginProvider], [Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
        ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

ALTER TABLE [identity].[UserTokens]     
WITH CHECK ADD CONSTRAINT FK_Identity_UserTokens_UserId FOREIGN KEY (UserId)     
    REFERENCES [identity].[Users] (Id)     
    ON DELETE CASCADE;
GO
ALTER TABLE [identity].[UserTokens] CHECK CONSTRAINT FK_Identity_UserTokens_UserId
GO

CREATE TABLE [identity].[RoleClaims](
	[Id] uniqueidentifier NOT NULL,
	[ClaimType] [nvarchar](256) NOT NULL,
    [ClaimValue] [nvarchar](256) NULL,
    [RoleId] uniqueidentifier NULL
CONSTRAINT [PK_Identity_RoleClaims] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
        ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

ALTER TABLE [identity].[RoleClaims]     
WITH CHECK ADD CONSTRAINT FK_Identity_RoleClaims_RoleId FOREIGN KEY (RoleId)     
    REFERENCES [identity].[Roles] (Id)     
    ON DELETE CASCADE;
GO


CREATE TABLE [identity].[UserClaims](
	[Id] uniqueidentifier NOT NULL,
	[ClaimType] [nvarchar](256) NULL,
	[ClaimValue] [nvarchar](256) NULL,
	[UserId] uniqueidentifier NOT NULL,
 CONSTRAINT [PK_Identity_UserClaims] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
        ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO

ALTER TABLE [identity].[UserClaims]     
WITH CHECK ADD CONSTRAINT FK_Identity_UserClaims_UserId FOREIGN KEY (UserId)     
    REFERENCES [identity].[Users] (Id)     
    ON DELETE CASCADE;
GO
ALTER TABLE [identity].[UserClaims] CHECK CONSTRAINT FK_Identity_UserClaims_UserId
GO

CREATE TABLE [identity].[UserRoles](
	[UserId] uniqueidentifier NOT NULL,
	[RoleId] uniqueidentifier NOT NULL,
 CONSTRAINT [PK_Identity_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
       ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [identity].[UserRoles]     
WITH CHECK ADD CONSTRAINT FK_Identity_UserRoles_RoleId FOREIGN KEY (RoleId)     
    REFERENCES [identity].[Roles] (Id)     
    ON DELETE CASCADE;
GO
ALTER TABLE [identity].[UserRoles] CHECK CONSTRAINT FK_Identity_UserRoles_RoleId
GO

ALTER TABLE [identity].[UserRoles]     
WITH CHECK ADD CONSTRAINT FK_Identity_UserRoles_UserId FOREIGN KEY (UserId)     
    REFERENCES [identity].[Users] (Id)     
    ON DELETE CASCADE;
GO
ALTER TABLE [identity].[UserRoles] CHECK CONSTRAINT FK_Identity_UserRoles_UserId
GO

CREATE TABLE [identity].[UserLogins](
	[LoginProvider] [nvarchar](256) NOT NULL,
	[ProviderKey] [nvarchar](256) NOT NULL,
	[ProviderDisplayName] [nvarchar](256) NULL,
	[UserId] uniqueidentifier NOT NULL
 CONSTRAINT [PK_Identity_UserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
       ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [identity].[UserLogins]     
WITH CHECK ADD CONSTRAINT FK_Identity_UserLogins_UserId FOREIGN KEY (UserId)     
    REFERENCES [identity].[Users] (Id)     
    ON DELETE CASCADE;
GO
ALTER TABLE [identity].[UserLogins] CHECK CONSTRAINT FK_Identity_UserLogins_UserId
GO
