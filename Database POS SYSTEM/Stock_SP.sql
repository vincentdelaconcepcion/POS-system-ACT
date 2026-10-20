-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		DelaConcepcion & Diosan
-- Create date: August 18 2026
-- Description:	adding new stock to the database
-- =============================================
CREATE PROCEDURE sp_Stock
	@ProductName VARCHAR(100),
    @Category    VARCHAR(50),
    @UnitPrice   DECIMAL(10,2),
    @Material    VARCHAR(100) = NULL,
    @DateAdded   DATETIME = NULL
AS
BEGIN
	 SET NOCOUNT ON;
 
    INSERT INTO Stock (ProductName, Category, UnitPrice, Material, DateAdded)
    VALUES (
        @ProductName,
        @Category,
        @UnitPrice,
        @Material,
        ISNULL(@DateAdded, GETDATE())
    );
 
    SELECT SCOPE_IDENTITY() AS NewStockID;
END
GO
