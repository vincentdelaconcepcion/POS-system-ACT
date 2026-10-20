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
-- Author:		DelaConcepcion, Vincent
-- Create date: 09/01/2026
-- Description:	A stored Procedure for searching inside the stock table
-- =============================================
CREATE PROCEDURE sp_Search
	 @SearchTerm VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        StockID,
        ProductName,
        Category,
        UnitPrice,
        Material,
        DateAdded
    FROM Stock
    WHERE
        @SearchTerm IS NULL
        OR @SearchTerm = ''
        OR ProductName LIKE '%' + @SearchTerm + '%'
        OR Category LIKE '%' + @SearchTerm + '%'
        OR Material LIKE '%' + @SearchTerm + '%'
        OR CAST(UnitPrice AS VARCHAR(20)) LIKE '%' + @SearchTerm + '%'
        OR CONVERT(VARCHAR(20), DateAdded, 101) LIKE '%' + @SearchTerm + '%';
END
GO
