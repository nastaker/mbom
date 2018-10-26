ALTER PROCEDURE [dbo].[PROC_VIRTUAL_ITEM_LINK]
(
@PROD_ITEMCODE VARCHAR(32),
@ITEMCODE_PARENT VARCHAR(32),
@GUID UNIQUEIDENTIFIER,
@USERID INT,
@NAME VARCHAR(20),
@LOGIN VARCHAR(20)
)
AS
BEGIN
	DECLARE @MSG VARCHAR(2000);
	DECLARE @SUCCESS BIT = 1;
	DECLARE @ERROR INT = 0;
	
	DECLARE @DATE DATETIME = GETDATE()
	-- 获取版本
	DECLARE @GUID_VER UNIQUEIDENTIFIER
	DECLARE @DTEF DATE, @DTEX DATE
	SELECT 
	  @GUID_VER = CN_GUID
	, @DTEF = CN_DT_EFFECTIVE
	, @DTEX = CN_DT_EXPIRY 
	FROM dbo.GETMBOMVER(@PROD_ITEMCODE) 
	IF @GUID_VER IS NULL
	BEGIN
		SET @MSG = '版本信息丢失，无法继续操作。请联系管理员。' 
		RAISERROR('%s',15,1, @MSG)
		RETURN
	END

	SELECT @PROD_ITEMCODE = CN_ITEM_CODE
	FROM TN_80_APP_0000_ITEM 
	WHERE CN_CODE = @PROD_ITEMCODE
	
	-- 检查引用件是否为虚件
	IF NOT EXISTS (
		SELECT 1 FROM TN_80_APP_0040_MBOM_VER_HLINK
		WHERE
			CN_GUID_VER = @GUID_VER
			AND CN_GUID = @GUID
			AND CN_TYPE = 'V'
			AND CN_STATUS = '_V'
	)
	BEGIN
		SET @SUCCESS = 0
		SET @MSG = '引用失败：所选件非虚件，或已经被引用'
		RAISERROR(@MSG, 15, 1)
		RETURN
	END 
	
	--获取物料ITEMCODE
	DECLARE @ITEMCODE VARCHAR(32)
	SELECT 
	@ITEMCODE = CN_ITEMCODE
	FROM TN_80_APP_0040_MBOM_HLINK
	WHERE CN_GUID = @GUID

	-- 获取本级到引用父级的所有父级数量之积
	-- 这里不能带MBOMHLINK的日期条件，虚件已经被无效了，有GUID即可
	DECLARE @MULTIPLEVALUE FLOAT
	;WITH X AS
	(
		SELECT
			 MBOMHLINK.CN_GUID
			,MBOMHLINK.CN_ITEMCODE
			,MBOMHLINK.CN_ITEMCODE_PARENT
			,MBOMHLINK.CN_QUANTITY AS QUANTITY
		FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		WHERE
			CN_GUID = @GUID
			AND CN_DT_EFFECTIVE_PBOM < @DATE
			AND CN_DT_EXPIRY_PBOM > @DATE
		
		UNION ALL
		
		SELECT
			 MBOMHLINK.CN_GUID
			,MBOMHLINK.CN_ITEMCODE
			,MBOMHLINK.CN_ITEMCODE_PARENT
			,X.QUANTITY * MBOMHLINK.CN_QUANTITY AS QUANTITY
		FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		INNER JOIN X ON X.CN_ITEMCODE_PARENT = MBOMHLINK.CN_ITEMCODE
		WHERE 
			CN_DT_EFFECTIVE_PBOM < @DATE
			AND CN_DT_EXPIRY_PBOM > @DATE
	)
	SELECT @MULTIPLEVALUE = QUANTITY
	FROM X
	WHERE CN_ITEMCODE_PARENT = @ITEMCODE_PARENT
	
	IF @MULTIPLEVALUE IS NULL
	BEGIN
		SET @MULTIPLEVALUE = 1
	END
	
	BEGIN TRANSACTION 
	
	-- 获取被引用虚件第一级物料
	SELECT
		 NEWID() AS CN_GUID
		,MBOM_FIRST.[CN_GUID_PBOM]
		,MBOM_FIRST.[CN_ITEMCODE]
		,MBOM_FIRST.[CN_QUANTITY] * @MULTIPLEVALUE AS CN_QUANTITY
		,MBOM_FIRST.[CN_DISPLAYNAME]
		,CASE WHEN  MBOM_FIRST.[CN_ORDER] < 100 THEN (MBOMHLINK.CN_ID * 100) +  MBOM_FIRST.[CN_ORDER] ELSE MBOM_FIRST.CN_ORDER END AS CN_ORDER
		,MBOM_FIRST.[CN_UNIT]
		,MBOM_FIRST.[CN_ISBORROW]
	INTO #TMP_VIRTUAL_FIRST
	FROM
		TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		INNER JOIN TN_80_APP_0040_MBOM_HLINK AS MBOM_FIRST ON MBOM_FIRST.CN_ITEMCODE_PARENT = MBOMHLINK.CN_ITEMCODE
			AND MBOM_FIRST.CN_DT_EFFECTIVE_PBOM < @DATE
			AND MBOM_FIRST.CN_DT_EXPIRY_PBOM > @DATE
			AND MBOM_FIRST.CN_DT_EFFECTIVE < @DATE
			AND MBOM_FIRST.CN_DT_EXPIRY > @DATE
	WHERE MBOMHLINK.CN_GUID = @GUID
	SET @ERROR = @ERROR + @@ERROR;
	
	-- 添加对第一级子级的引用
	INSERT INTO TN_80_APP_0040_MBOM_HLINK
	(
	   [CN_GUID]
      ,[CN_GUID_LINK]
      ,[CN_GUID_EF]
      ,[CN_ITEMCODE_PARENT]
      ,[CN_ITEMCODE]
      ,[CN_QUANTITY]
      ,[CN_DISPLAYNAME]
      ,[CN_ORDER]
      ,[CN_UNIT]
      ,[CN_ISBORROW]
      ,[CN_DT_EFFECTIVE]
      ,[CN_CREATE_BY]
      ,[CN_CREATE_NAME]
      ,[CN_CREATE_LOGIN]
    )
	SELECT 
	   CN_GUID
      ,@GUID
      ,@GUID_VER
      ,@ITEMCODE_PARENT
      ,[CN_ITEMCODE]
      ,[CN_QUANTITY]
      ,[CN_DISPLAYNAME]
      ,[CN_ORDER]
      ,[CN_UNIT]
      ,[CN_ISBORROW]
      ,@DTEF
      ,@USERID
      ,@NAME
      ,@LOGIN
	FROM #TMP_VIRTUAL_FIRST
	SET @ERROR = @ERROR + @@ERROR;

	-- 添加第一级子级的虚件属性
	INSERT INTO [dbo].[TN_80_APP_0040_MBOM_VER_HLINK]
		([CN_GUID_VER]
		,[CN_GUID]
		,[CN_STATUS]
		,[CN_TYPE]
		,[CN_DT_CREATE]
		,[CN_DESC]
		,[CN_CREATE_BY]
		,[CN_CREATE_NAME]
		,[CN_CREATE_LOGIN])
     SELECT
		 @GUID_VER
		,CN_GUID
		,'Y'
		,'V'
		,@DATE
		,@NAME + ' 引用['+@ITEMCODE+']虚件下级'
		,@USERID
		,@NAME
		,@LOGIN
	FROM #TMP_VIRTUAL_FIRST
	SET @ERROR = @ERROR + @@ERROR
	
	-- 更新虚件引用状态为【已引用】
	UPDATE MBOMVER
	SET 
		CN_STATUS = '_VL'
	FROM TN_80_APP_0040_MBOM_VER_HLINK AS MBOMVER
	WHERE 
		CN_GUID_VER = @GUID_VER
		AND CN_GUID = @GUID
	SET @ERROR = @ERROR + @@ERROR;
	
	IF @ERROR <> 0
	BEGIN
		SET @MSG = '引用虚件失败，请联系管理员'
		RAISERROR(@MSG, 15, 1)

		ROLLBACK TRANSACTION
		RETURN;
	END
	ELSE
	BEGIN
		COMMIT TRANSACTION
		SET @MSG = '引用虚件成功'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
	END
END;  