USE StockDB;
GO

IF COL_LENGTH('dbo.Stock', 'ImageData') IS NULL
BEGIN
    ALTER TABLE dbo.Stock ADD ImageData VARBINARY(MAX) NULL;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Stock
    @ProductName VARCHAR(100),
    @Category VARCHAR(50),
    @UnitPrice DECIMAL(10,2),
    @Material NVARCHAR(100) = NULL,
    @DateAdded DATETIME = NULL,
    @Quantity INT = 0,
    @ImageData VARBINARY(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Stock (ProductName, Category, UnitPrice, Material, DateAdded, Quantity, ImageData)
    VALUES (@ProductName, @Category, @UnitPrice, @Material, ISNULL(@DateAdded, GETDATE()), ISNULL(@Quantity, 0), @ImageData);

    SELECT SCOPE_IDENTITY() AS NewStockID;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Search
    @SearchTerm VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT StockID, ProductName, Category, UnitPrice, Material, DateAdded, Quantity, ImageData
    FROM Stock
    WHERE IsArchived = 0
      AND (@SearchTerm IS NULL
        OR @SearchTerm = ''
        OR ProductName LIKE '%' + @SearchTerm + '%'
        OR Category LIKE '%' + @SearchTerm + '%'
        OR Material LIKE '%' + @SearchTerm + '%'
        OR CAST(UnitPrice AS VARCHAR(20)) LIKE '%' + @SearchTerm + '%'
        OR CONVERT(VARCHAR(20), DateAdded, 101) LIKE '%' + @SearchTerm + '%');
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetArchived
AS
BEGIN
    SET NOCOUNT ON;

    SELECT StockID, ProductName, Category, UnitPrice, Material, DateAdded, Quantity, ImageData
    FROM dbo.Stock
    WHERE IsArchived = 1;
END
GO
