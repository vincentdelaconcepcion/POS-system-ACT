USE [StockDB];
GO

SET NOCOUNT ON;
GO

IF COL_LENGTH('dbo.Orders', 'Cashier') IS NULL
BEGIN
    ALTER TABLE dbo.Orders ADD Cashier NVARCHAR(100) NULL;
END
GO

IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Orders_PaymentMethod')
BEGIN
    ALTER TABLE dbo.Orders DROP CONSTRAINT CK_Orders_PaymentMethod;
END
GO

IF COL_LENGTH('dbo.Orders', 'PaymentMethod') IS NULL
BEGIN
    ALTER TABLE dbo.Orders
        ADD PaymentMethod NVARCHAR(50) NOT NULL
        CONSTRAINT DF_Orders_PaymentMethod DEFAULT N'Cash';
END
ELSE
BEGIN
    ALTER TABLE dbo.Orders ALTER COLUMN PaymentMethod NVARCHAR(50) NOT NULL;
END
GO

IF OBJECT_ID('dbo.PaymentMethods', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PaymentMethods
    (
        PaymentMethodID INT IDENTITY(1, 1) NOT NULL,
        Name NVARCHAR(50) NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_PaymentMethods_IsActive DEFAULT 1,
        CONSTRAINT PK_PaymentMethods PRIMARY KEY (PaymentMethodID),
        CONSTRAINT UQ_PaymentMethods_Name UNIQUE (Name)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.PaymentMethods WHERE Name = N'Cash')
BEGIN
    INSERT INTO dbo.PaymentMethods (Name) VALUES (N'Cash');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.PaymentMethods WHERE Name = N'Card')
BEGIN
    INSERT INTO dbo.PaymentMethods (Name) VALUES (N'Card');
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Orders_PaymentMethods'
      AND parent_object_id = OBJECT_ID('dbo.Orders')
)
BEGIN
    ALTER TABLE dbo.Orders
        ADD CONSTRAINT FK_Orders_PaymentMethods
        FOREIGN KEY (PaymentMethod) REFERENCES dbo.PaymentMethods(Name);
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Orders_OrderDate'
      AND object_id = OBJECT_ID('dbo.Orders')
)
BEGIN
    CREATE INDEX IX_Orders_OrderDate ON dbo.Orders(OrderDate);
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_OrderItems_OrderID'
      AND object_id = OBJECT_ID('dbo.OrderItems')
)
BEGIN
    CREATE INDEX IX_OrderItems_OrderID ON dbo.OrderItems(OrderID);
END
GO

IF OBJECT_ID('dbo.vw_SalesReport', 'V') IS NOT NULL
    DROP VIEW dbo.vw_SalesReport;
GO

CREATE VIEW dbo.vw_SalesReport
AS
SELECT
    o.OrderID,
    o.OrderDate,
    ISNULL(NULLIF(LTRIM(RTRIM(o.Cashier)), ''), 'Unassigned') AS Cashier,
    o.PaymentMethod,
    o.TotalAmount,
    ISNULL(SUM(oi.Quantity), 0) AS ItemCount
FROM dbo.Orders o
LEFT JOIN dbo.OrderItems oi ON oi.OrderID = o.OrderID
GROUP BY o.OrderID, o.OrderDate, o.Cashier, o.PaymentMethod, o.TotalAmount;
GO

IF OBJECT_ID('dbo.sp_GetSalesReport', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetSalesReport;
GO

CREATE PROCEDURE dbo.sp_GetSalesReport
    @DateFrom DATE = NULL,
    @DateTo DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @DateFrom = ISNULL(@DateFrom, CAST(GETDATE() AS DATE));
    SET @DateTo = ISNULL(@DateTo, @DateFrom);

    SELECT
        OrderID,
        OrderDate,
        Cashier,
        PaymentMethod,
        TotalAmount,
        ItemCount
    FROM dbo.vw_SalesReport
    WHERE OrderDate >= @DateFrom
      AND OrderDate < DATEADD(DAY, 1, @DateTo)
    ORDER BY OrderDate DESC, OrderID DESC;

    SELECT
        ISNULL(SUM(TotalAmount), 0) AS TotalSales,
        COUNT(*) AS TotalOrders,
        ISNULL(SUM(ItemCount), 0) AS ItemsSold
    FROM dbo.vw_SalesReport
    WHERE OrderDate >= @DateFrom
      AND OrderDate < DATEADD(DAY, 1, @DateTo);
END
GO

IF OBJECT_ID('dbo.sp_GetOrderDetails', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetOrderDetails;
GO

CREATE PROCEDURE dbo.sp_GetOrderDetails
    @OrderID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        o.OrderID,
        o.OrderDate,
        ISNULL(NULLIF(LTRIM(RTRIM(o.Cashier)), ''), 'Unassigned') AS Cashier,
        o.PaymentMethod,
        o.TotalAmount,
        oi.OrderItemID,
        oi.StockID,
        oi.ProductName,
        oi.Category,
        oi.UnitPrice,
        oi.Quantity,
        oi.Subtotal
    FROM dbo.Orders o
    LEFT JOIN dbo.OrderItems oi ON oi.OrderID = o.OrderID
    WHERE o.OrderID = @OrderID
    ORDER BY oi.OrderItemID;
END
GO

IF OBJECT_ID('dbo.sp_GetSalesByProduct', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetSalesByProduct;
GO

CREATE PROCEDURE dbo.sp_GetSalesByProduct
    @DateFrom DATE = NULL,
    @DateTo DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @DateFrom = ISNULL(@DateFrom, CAST(GETDATE() AS DATE));
    SET @DateTo = ISNULL(@DateTo, @DateFrom);

    SELECT
        oi.ProductName,
        oi.Category,
        SUM(oi.Quantity) AS ItemsSold,
        SUM(oi.Subtotal) AS SalesAmount
    FROM dbo.OrderItems oi
    INNER JOIN dbo.Orders o ON o.OrderID = oi.OrderID
    WHERE o.OrderDate >= @DateFrom
      AND o.OrderDate < DATEADD(DAY, 1, @DateTo)
    GROUP BY oi.ProductName, oi.Category
    ORDER BY SalesAmount DESC, oi.ProductName;
END
GO
