SET NOCOUNT ON;
GO

IF DB_ID(N'LoginPOS') IS NULL
BEGIN
    CREATE DATABASE [LoginPOS];
END
GO

USE [LoginPOS];
GO

IF OBJECT_ID(N'dbo.[User]', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[User]
    (
        USERNAME VARCHAR(50) NOT NULL,
        PASSWORD VARCHAR(100) NOT NULL,
        Role NVARCHAR(40) NOT NULL
            CONSTRAINT DF_User_Role DEFAULT N'Cashier',
        CONSTRAINT PK_User PRIMARY KEY (USERNAME),
        CONSTRAINT CK_User_Role CHECK (Role IN (N'Cashier', N'Manager', N'Admin'))
    );
END
GO

IF COL_LENGTH(N'dbo.[User]', N'Role') IS NULL
BEGIN
    ALTER TABLE dbo.[User]
        ADD Role NVARCHAR(40) NOT NULL
            CONSTRAINT DF_User_Role DEFAULT N'Cashier';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_User_Role')
BEGIN
    ALTER TABLE dbo.[User]
        ADD CONSTRAINT CK_User_Role
        CHECK (Role IN (N'Cashier', N'Manager', N'Admin'));
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.[User] WHERE USERNAME = N'admin')
BEGIN
    INSERT INTO dbo.[User] (USERNAME, PASSWORD, Role)
    VALUES
    (
        N'admin',
        N'240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9',
        N'Admin'
    );
END
GO
IF OBJECT_ID(N'dbo.AdminRecovery', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AdminRecovery
    (
        Username         VARCHAR(50)   NOT NULL,
        SecurityQuestion NVARCHAR(200) NULL,
        AnswerHash       NVARCHAR(200) NULL,
        RecoveryCodeHash NVARCHAR(200) NULL,
        CONSTRAINT PK_AdminRecovery PRIMARY KEY (Username),
        CONSTRAINT FK_AdminRecovery_User FOREIGN KEY (Username)
            REFERENCES dbo.[User] (USERNAME) ON DELETE CASCADE
    );
END
GO



SELECT USERNAME, Role
FROM dbo.[User]
ORDER BY USERNAME;
GO
