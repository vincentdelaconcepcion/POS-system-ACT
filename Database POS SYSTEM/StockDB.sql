CREATE DATABASE StockDB;
GO
 
USE StockDB;
GO
 
/* ============================================================
   TABLE
   ============================================================ */
 
CREATE TABLE Stock (
    StockID     INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Category    VARCHAR(50)  NOT NULL,
    UnitPrice   DECIMAL(10,2) NOT NULL,
    Material    VARCHAR(100) NULL,
    DateAdded   DATETIME NOT NULL DEFAULT GETDATE(),
 
    CONSTRAINT CK_Stock_Category CHECK (
        Category IN ('Garments', 'School Supplies', 'RTW', 'Footwear')
    )
);
GO

SELECT * FROM Stock


