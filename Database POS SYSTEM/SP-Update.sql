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
-- Author:		Nino Gwapo
-- Create date: 9-8-2026
-- Description:	Edit a data
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateProduct]
	-- Add the parameters for the stored procedure here
	@PRODUCTNAME VARCHAR(50),
	@CATEGORY VARCHAR (50),
	@UNITPRICE DECIMAL (10,2),
	@MATERIAL VARCHAR (100),
	@DATE DATETIME,
	@STOCKID INT 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Stock
	SET ProductName = @PRODUCTNAME,
	Category = @CATEGORY,
	UnitPrice = @UNITPRICE,
	Material = @MATERIAL,
	DateAdded = @DATE
	WHERE StockID = @STOCKID

END
GO
