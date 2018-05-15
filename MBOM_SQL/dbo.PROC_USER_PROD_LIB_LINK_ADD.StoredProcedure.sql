USE [MBOM_TJB]
GO
/****** Object:  StoredProcedure [dbo].[PROC_USER_PROD_LIB_LINK_ADD]    Script Date: 05/15/2018 15:22:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[PROC_USER_PROD_LIB_LINK_ADD]
(
@LIBID INT,
@IDS VARCHAR(2000),
@USERID INT,
@NAME VARCHAR(20),
@LOGIN VARCHAR(20)
)
AS
BEGIN
	DECLARE @ERROR INT = 0

	SELECT ID INTO #PRODIDS FROM SPLIT(@IDS, DEFAULT)
	LEFT JOIN [TN_USERPRODUCTLIBRARY_LINK] AS LINK ON LINK.CN_LIB_ID = @LIBID AND LINK.CN_PRODUCT_ID = ID
	WHERE LINK.CN_ID IS NULL

	INSERT INTO [DBO].[TN_USERPRODUCTLIBRARY_LINK]
           ([CN_LIB_ID]
           ,[CN_PRODUCT_ID]
           ,[CN_PRODUCT_CODE]
           ,[CN_PRODUCT_NAME]
           ,[CN_ITEM_ID]
           ,[CN_ITEM_CODE]
		   ,[CN_CREATE_BY]
		   ,[CN_CREATE_NAME]
		   ,[CN_CREATE_LOGIN]
		   )
	SELECT 
		@LIBID
		,PROD.CN_ID
		,PROD.CN_CODE
		,PROD.CN_NAME 
		,ITEM.CN_ID
		,ITEM.CN_ITEM_CODE
		,@USERID
		,@NAME
		,@LOGIN
	FROM TN_80_APP_0010_PRODUCT AS PROD
	INNER JOIN [DBO].[TN_80_APP_0000_ITEM] AS ITEM ON ITEM.CN_CODE = PROD.CN_CODE
	WHERE PROD.CN_ID IN ( SELECT ID FROM #PRODIDS )

	SET @ERROR = @ERROR + @@ERROR

	IF @ERROR = 0
	BEGIN

		SELECT '操作成功' AS MSG, CAST(1 AS BIT) AS SUCCESS
	END
	ELSE
	BEGIN

		SELECT '操作失败，请联系管理员' AS MSG, CAST(0 AS BIT) AS SUCCESS
	END;
END;
GO
