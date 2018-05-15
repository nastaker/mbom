USE [MBOM_TJB]
GO
/****** Object:  StoredProcedure [dbo].[PROC_SET_SALELIST]    Script Date: 05/15/2018 15:22:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[PROC_SET_SALELIST]
(
@CODE VARCHAR(32),
@STR VARCHAR(2000),
@USERID INT,
@NAME NVARCHAR(30),
@LOGIN NVARCHAR(30)
)
AS
BEGIN
	DECLARE @MSG VARCHAR(2000) = ''
	DECLARE @ERROR INT = 0

	--拆分STR,以分号分段
	SELECT
		DBO.SPLIT_SUB(ID,1,DEFAULT) AS T,
		CAST(DBO.SPLIT_SUB(ID,2,DEFAULT) AS INT) AS ITEMID,
		CAST(DBO.SPLIT_SUB(ID,3,DEFAULT) AS FLOAT) AS QUANTITY,
		DBO.SPLIT_SUB(ID,4,DEFAULT) AS ADDR,
		DBO.SPLIT_SUB(ID,5,DEFAULT) AS CUSTOMERITEMCODE,
		DBO.SPLIT_SUB(ID,6,DEFAULT) AS CUSTOMERITEMNAME
	INTO #TMP
	FROM SPLIT(@STR, ';')  
	
	BEGIN TRANSACTION
	
	UPDATE ITEMH
	SET ITEMH.CN_ISDELETE = 1, ITEMH.CN_DT_EXPIRY = SYSDATETIME()
	FROM TN_80_APP_0000_ITEM_HLINK AS ITEMH
	INNER JOIN #TMP AS T ON T.ITEMID = ITEMH.CN_ID
	WHERE 
		ITEMH.CN_COMPONENT_CLASS_ID = 105 
	AND ITEMH.CN_COMPONENT_OBJECT_ID = 1
	AND  T.T = 'D'
	SET @ERROR = @ERROR + @@ERROR
	
	UPDATE ITEMH
	SET 
		 ITEMH.CN_F_QUANTITY = T.QUANTITY
		,ITEMH.CN_SHIPPINGADDR = T.ADDR
		,ITEMH.CN_CUSTOMERITEMCODE = T.CUSTOMERITEMCODE
		,ITEMH.CN_CUSTOMERITEMNAME = T.CUSTOMERITEMNAME
	FROM TN_80_APP_0000_ITEM_HLINK AS ITEMH
	INNER JOIN #TMP AS T ON T.ITEMID = ITEMH.CN_ID
	WHERE 
		ITEMH.CN_COMPONENT_CLASS_ID = 105 
	AND ITEMH.CN_COMPONENT_OBJECT_ID = 1
	AND T.T = 'U'
	SET @ERROR = @ERROR + @@ERROR
	
	INSERT INTO [MBOM_SERVER].[dbo].[TN_80_APP_0000_ITEM_HLINK]
           ([CN_ID]
           ,[CN_COMPONENT_CLASS_ID]
           ,[CN_COMPONENT_OBJECT_ID]
           ,[CN_ISDELETE]
           ,[CN_F_QUANTITY]
           ,[CN_DISPLAYNAME]
           ,[CN_CREATE_BY]
           ,[CN_CREATE_NAME]
           ,[CN_CREATE_LOGIN]
           ,[CN_SYS_STATUS]
           ,[CN_SHIPPINGADDR])
    SELECT
			ITEMID
		   ,105
		   ,1
		   ,0
		   ,QUANTITY
		   ,'销售件'
		   ,@USERID
		   ,@NAME
		   ,@LOGIN
		   ,'Y'
		   ,ADDR
	FROM #TMP
	WHERE T = 'C'
	SET @ERROR = @ERROR + @@ERROR
	 
	 
	UPDATE TN_80_APP_0010_PRODUCT 
	SET CN_SALE_SET = '设置完成'
	WHERE CN_CODE = @CODE
	SET @ERROR = @ERROR + @@ERROR
	
	IF @ERROR <> 0
	BEGIN
		SET @MSG = '执行过程中出现错误，请联系管理员。'
		SELECT @MSG AS MSG, CAST(1 AS BIT) AS SUCCESS
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		SET @MSG = '设置销售件成功。'
		SELECT @MSG AS MSG, CAST(1 AS BIT) AS SUCCESS
		COMMIT TRANSACTION
	END
END;
GO
