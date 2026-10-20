USE LoginPOS;
GO

IF COL_LENGTH('dbo.[User]', 'Role') IS NULL
BEGIN
    ALTER TABLE dbo.[User]
        ADD [Role] NVARCHAR(20) NOT NULL
        CONSTRAINT DF_User_Role DEFAULT 'Cashier';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_User_Role')
BEGIN
    ALTER TABLE dbo.[User]
        ADD CONSTRAINT CK_User_Role
        CHECK ([Role] IN ('Cashier', 'Manager', 'Admin'));
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.[User] WHERE USERNAME = 'admin')
BEGIN
    INSERT INTO dbo.[User] (USERNAME, [PASSWORD], [Role])
    VALUES ('admin', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', 'Admin');
END
GO
