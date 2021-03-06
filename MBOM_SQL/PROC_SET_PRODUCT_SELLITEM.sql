ALTER PROCEDURE [dbo].[PROC_SET_PRODUCT_SELLITEM]
(
@PROD_ITEMCODE VARCHAR(32),
@STR VARCHAR(2000),
@USERID INT,
@NAME NVARCHAR(30),
@LOGIN NVARCHAR(30)
)
AS
BEGIN
	-- 产品从 PDM初始化 -> 销售件设置完成
	DECLARE @CURR_STATUS VARCHAR(32) = DBO.FUNC_GETSTATUS('PRODUCT',1000)
	DECLARE @NEXT_STATUS VARCHAR(32) = DBO.FUNC_GETSTATUS('PRODUCT',2000)

	DECLARE @MSG VARCHAR(2000) = ''
	DECLARE @ERROR INT = 0
	
	DECLARE @PRODID INT
	DECLARE @DATE DATETIME = GETDATE()
	DECLARE @GUID_VER UNIQUEIDENTIFIER

	SELECT @PRODID = CN_ID
	FROM TN_80_APP_0010_PRODUCT
	WHERE CN_ITEM_CODE = @PROD_ITEMCODE

	--获取最新版本的产品属性表
	SELECT TOP 1 
		@GUID_VER = CN_GUID
	FROM TN_80_APP_0010_PRODUCT_VER
	WHERE CN_PRODUCT_ITEMCODE = @PROD_ITEMCODE AND CN_STATUS = 'Y' AND CN_IS_TOERP = 0
	ORDER BY CN_DT_CREATE DESC
	
	--拆分STR为程序可读取的数据
	SELECT
		DBO.SPLIT_SUB(ID,1,DEFAULT) AS T,--变更的类型，有三种：C添加，D删除，U更新
		CAST(DBO.SPLIT_SUB(ID,2,DEFAULT) AS INT) AS ITEMID,
		CAST(DBO.SPLIT_SUB(ID,3,DEFAULT) AS FLOAT) AS QUANTITY,
		DBO.SPLIT_SUB(ID,4,DEFAULT) AS ADDR,
		DBO.SPLIT_SUB(ID,5,DEFAULT) AS CUS_CODE,
		DBO.SPLIT_SUB(ID,6,DEFAULT) AS CUS_NAME,
		DBO.SPLIT_SUB(ID,7,DEFAULT) AS CUS_ID
	INTO #TMP_PRE
	FROM SPLIT(@STR, ';') 
	
	SELECT
		 TP.*
		,ITEM.CN_ITEM_CODE AS ITEMCODE
	INTO #TMP
	FROM #TMP_PRE AS TP
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON ITEM.CN_ID = TP.ITEMID
	
	BEGIN TRANSACTION
	-- 写日志
	DECLARE @LOGID INT
	INSERT INTO [TN_SYS_LOG]
           ([CN_ACTIONURL]
           ,[CN_USERIP]
           ,[CN_REQUESTTYPE]
           ,[CN_USERID]
           ,[CN_USERNAME]
           ,[CN_USERLOGIN]
           ,[CN_ISSUCCESS]
           ,[CN_DESC])
    SELECT 
			OBJECT_NAME(@@PROCID)
			,''
			,'存储过程'
			,@USERID
			,@NAME
			,@LOGIN
			,1
			,'设置销售件'
	SET @ERROR = @ERROR + @@ERROR
	
	SET @LOGID = SCOPE_IDENTITY()
	
	-- 在PRODUCTHLINK中删除ITEM的销售件属性
	UPDATE PRODH
	SET CN_DT_EXPIRY = SYSDATETIME()
	FROM TN_80_APP_0010_PRODUCT_HLINK AS PRODH
	INNER JOIN #TMP AS T ON T.ITEMID = PRODH.CN_COMPONENT_OBJECT_ID
	WHERE 
		PRODH.CN_COMPONENT_CLASS_ID = 100
	AND PRODH.CN_ID = @PRODID
	AND  T.T = 'D'
	SET @ERROR = @ERROR + @@ERROR
	
	-- 在PRODUCTHLINK中更新ITEM的销售件设置
	UPDATE PRODH
	SET
		 CN_CUSTOMERNAME = CUST.CN_NAME
		,CN_CUSTOMERCODE = CUST.CN_CODE
		,CN_CUSTOMERITEMNAME = T.CUS_NAME
		,CN_CUSTOMERITEMCODE = T.CUS_CODE
		,CN_SHIPPINGADDR = T.ADDR 
	FROM TN_80_APP_0010_PRODUCT_HLINK AS PRODH
	INNER JOIN #TMP AS T ON T.ITEMID = PRODH.CN_COMPONENT_OBJECT_ID
	INNER JOIN TN_APP_0090_CUSTOMER AS CUST ON CUST.CN_ID = T.CUS_ID
	WHERE 
		PRODH.CN_COMPONENT_CLASS_ID = 100
	AND PRODH.CN_ID = @PRODID
	AND T.T = 'U'
	SET @ERROR = @ERROR + @@ERROR
	
	-- 在PRODUCTHLINK中添加ITEM的销售件【详细】设置
	INSERT INTO TN_80_APP_0010_PRODUCT_HLINK
        ([CN_ID]
		,[CN_GUID_VER]
        ,[CN_COMPONENT_CLASS_ID]
        ,[CN_COMPONENT_OBJECT_ID]
        ,[CN_F_QUANTITY]
        ,[CN_DISPLAYNAME]
        ,[CN_CUSTOMERNAME]
		,[CN_CUSTOMERCODE]
		,[CN_CUSTOMERITEMNAME]
		,[CN_CUSTOMERITEMCODE]
		,[CN_SHIPPINGADDR]
        ,[CN_CREATE_BY]
        ,[CN_CREATE_NAME]
        ,[CN_CREATE_LOGIN]
        ,[CN_SYS_STATUS]
        )
    SELECT
		 @PRODID
		,@GUID_VER
		,100
		,ITEMID
		,QUANTITY
		,ITEMCODE
		,CUST.CN_NAME
		,CUST.CN_CODE
		,CUS_NAME
		,CUS_CODE
		,ADDR
		,@USERID
		,@NAME
		,@LOGIN
		,'Y'
	FROM #TMP AS T
	INNER JOIN TN_APP_0090_CUSTOMER AS CUST ON CUST.CN_ID = CUS_ID
	WHERE T.T = 'C'
	SET @ERROR = @ERROR + @@ERROR
	
	-- 添加系统日志详细信息
	INSERT INTO [TN_SYS_LOG_DETAIL]
           ([CN_LOG_ID]
           ,[CN_OPERATION_TABLE]
		   ,[CN_OPERATION_KEY]
		   ,[CN_OPERATION_VALUE]
           ,[CN_COLUMN_NAME]
           ,[CN_COLUMN_VALUE_BEFORE]
           ,[CN_COLUMN_VALUE_AFTER])
    SELECT
			@LOGID
			,'TN_80_APP_0010_PRODUCT'
			,'CN_CODE'
			,@PROD_ITEMCODE
			,'CN_SALE_SET'
			,@CURR_STATUS
			,@NEXT_STATUS
	FROM TN_80_APP_0010_PRODUCT
	WHERE CN_ITEM_CODE = @PROD_ITEMCODE
	SET @ERROR = @ERROR + @@ERROR
	
	UPDATE TN_80_APP_0010_PRODUCT 
	SET 
		CN_SALE_SET = '设置完成',
		CN_DT_SELL = @DATE,
		CN_USER_SELL = @NAME,
		CN_STATUS = @NEXT_STATUS
	WHERE CN_ITEM_CODE = @PROD_ITEMCODE
	AND CN_STATUS = @CURR_STATUS
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
