ALTER PROC [dbo].[PROC_PRODUCT_TRANSFER_INITIATTE]
(
@PROD_ITEMCODE VARCHAR(200),
@USERID INT,
@NAME VARCHAR(32),
@LOGIN VARCHAR(32)
)
AS
BEGIN 
	-- 产品从 销售件设置完成 -> MBOM维护中
	DECLARE @CURR_STATUS VARCHAR(32) = DBO.FUNC_GETSTATUS('PRODUCT',2000)
	DECLARE @NEXT_STATUS VARCHAR(32) = DBO.FUNC_GETSTATUS('PRODUCT',3000)

	DECLARE @DATE DATETIME = GETDATE()
	DECLARE @MSG NVARCHAR(2000);
	DECLARE @SUCCESS BIT = 1;

	SELECT
		 ITEM.CN_CODE AS CODE
		,ITEM.CN_ITEM_CODE AS ITEMCODE
	INTO #TMP_TRANSFER 
	FROM SPLIT(@PROD_ITEMCODE,DEFAULT) AS T
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON ITEM.CN_ITEM_CODE = T.ID;

	SELECT CN_ID AS PROD_ID
	INTO #TMP_PRODIDS
	FROM TN_80_APP_0010_PRODUCT AS PROD
	INNER JOIN #TMP_TRANSFER AS TRA ON TRA.ITEMCODE = PROD.CN_ITEM_CODE

	IF NOT EXISTS (
		SELECT 1
		FROM #TMP_TRANSFER AS T
		INNER JOIN TN_80_APP_0010_PRODUCT AS PROD ON PROD.CN_CODE = T.CODE
		WHERE PROD.CN_STATUS = @CURR_STATUS
	)
	BEGIN
		SET @MSG = '选择的产品未设置销售件';
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
	END

	--获取最新版本的产品版本表
	SELECT CN_GUID
	INTO #GUID_VERS
	FROM TN_80_APP_0010_PRODUCT_VER
	INNER JOIN #TMP_TRANSFER ON ITEMCODE = CN_PRODUCT_ITEMCODE AND CN_STATUS = 'Y' AND CN_IS_TOERP = 0
	ORDER BY CN_DT_CREATE DESC

	;WITH X AS 
	(
		SELECT
		ITEM.CN_ID AS CN_ITEMID,
		ITEM.CN_ITEM_CODE
		FROM
		TN_80_APP_0030_PBOM_HLINK AS PBOMHLINK
		INNER JOIN #TMP_TRANSFER AS T ON T.ITEMCODE = PBOMHLINK.CN_ITEMCODE
		INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON PBOMHLINK.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
		WHERE
		PBOMHLINK.CN_DT_EFFECTIVE < @DATE
		AND PBOMHLINK.CN_DT_EXPIRY > @DATE

		UNION ALL

		SELECT
		ITEM.CN_ID AS CN_ITEMID,
		ITEM.CN_ITEM_CODE
		FROM 
		TN_80_APP_0030_PBOM_HLINK AS PBOMHLINK
		INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON PBOMHLINK.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
		INNER JOIN X ON X.CN_ITEM_CODE = PBOMHLINK.CN_ITEMCODE_PARENT
		WHERE
		PBOMHLINK.CN_DT_EFFECTIVE < @DATE
		AND PBOMHLINK.CN_DT_EXPIRY > @DATE
	)
	SELECT DISTINCT CN_ITEM_CODE
	INTO #TMP_ITEMID
	FROM X
	--查询出所有本产品借用子件的直属产品，若存在其他未发布产品不能转批
	SELECT DISTINCT ITEM.CN_PRODUCT_BASE
	INTO #TMP_RELATEDPROD
	FROM
	#TMP_ITEMID AS TMP INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON TMP.CN_ITEM_CODE = ITEM.CN_ITEM_CODE
	WHERE
	LEN(ITEM.CN_PRODUCT_BASE) > 0
	AND ITEM.CN_PRODUCT_BASE NOT IN ( SELECT CODE FROM #TMP_TRANSFER )
	AND CN_IS_TOERP = 0
	GROUP BY ITEM.CN_PRODUCT_BASE,ITEM.CN_IS_TOERP
	
	DECLARE @PRODS NVARCHAR(500)
	IF EXISTS ( SELECT 1 FROM #TMP_RELATEDPROD )
	BEGIN
		-- 不能转批
		SET @SUCCESS = 0
		SET @MSG = ISNULL(@MSG, '') + ' 该产品下有引用件：';

		SELECT 
		@MSG = ISNULL(@MSG, '') + '[' + RTRIM(PROD.CN_CODE) + ' ' + RTRIM(PROD.CN_NAME) + ']：' +
		CASE PROD.CN_SALE_SET WHEN '设置完成' THEN '' ELSE '销售件未设置' END + 
		'。<BR/>' 
		FROM  #TMP_RELATEDPROD BASEPROD
		INNER JOIN TN_80_APP_0010_PRODUCT PROD ON BASEPROD.CN_PRODUCT_BASE = PROD.CN_CODE
		
		
		SELECT 
			@PRODS = ISNULL(@PRODS, '') +
			'[' + RTRIM(BASEPROD.CN_PRODUCT_BASE) +']、'
		FROM 
		#TMP_RELATEDPROD BASEPROD
		IF @PRODS IS NOT NULL
		BEGIN
			SET @MSG = ISNULL(@MSG, '') +  ''+ SUBSTRING( @PRODS, 0, LEN(@PRODS) ) + ' 需要统一发布。';
		END
	END

	--可以直接转批
	IF @SUCCESS = 1
	BEGIN
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
				,'转批发起'
		SET @LOGID = SCOPE_IDENTITY()
	
	
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
				,'CN_STATUS'
				,@CURR_STATUS
				,@NEXT_STATUS
		FROM TN_80_APP_0010_PRODUCT
		INNER JOIN #TMP_TRANSFER ON ITEMCODE = CN_ITEM_CODE

		UPDATE PROD
		SET 
			--更新状态
			CN_STATUS = @NEXT_STATUS
			--更新节点日志
			,CN_DT_PRE = @DATE
			,CN_USER_PRE = @NAME
		FROM TN_80_APP_0010_PRODUCT AS PROD
		INNER JOIN #TMP_TRANSFER ON ITEMCODE = CN_ITEM_CODE
	
		UPDATE PRODH
		SET
		CN_IS_TOERP = 1
		FROM TN_80_APP_0010_PRODUCT_HLINK AS PRODH
		INNER JOIN #TMP_PRODIDS AS PRODIDS ON PRODIDS.PROD_ID = PRODH.CN_ID
		INNER JOIN #GUID_VERS AS G ON G.CN_GUID = PRODH.CN_GUID_VER
		WHERE
		PRODH.CN_IS_TOERP = 0
		AND PRODH.CN_COMPONENT_CLASS_ID = 100
	
		UPDATE PRODVER
		SET 
		CN_IS_TOERP = 1
		FROM TN_80_APP_0010_PRODUCT_VER AS PRODVER
		INNER JOIN #GUID_VERS AS G ON G.CN_GUID = PRODVER.CN_GUID


		SET @MSG = '已成功预转批，请等待预转批完成';
	END

	SELECT @MSG AS MSG, @SUCCESS AS SUCCESS

	-- 结束存储过程 --
END
