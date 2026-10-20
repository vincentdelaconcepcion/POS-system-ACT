USE [StockDB];
GO

SET NOCOUNT ON;
GO

IF COL_LENGTH('dbo.Orders', 'Cashier') IS NULL
BEGIN
    ALTER TABLE dbo.Orders ADD Cashier NVARCHAR(100) NULL;
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
    o.TotalAmount,
    ISNULL(SUM(oi.Quantity), 0) AS ItemCount
FROM dbo.Orders o
LEFT JOIN dbo.OrderItems oi ON oi.OrderID = o.OrderID
GROUP BY o.OrderID, o.OrderDate, o.Cashier, o.TotalAmount;
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
