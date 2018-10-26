ALTER PROCEDURE [dbo].[PROC_COMPOSITE_ITEM_DROP]
(
	@PROD_ITEMCODE VARCHAR(32),
	@GUID UNIQUEIDENTIFIER,
	@USERID INT,
	@NAME VARCHAR(20),
	@LOGIN VARCHAR(20)
)
AS
BEGIN
	DECLARE @MSG VARCHAR(2000) = ''
	DECLARE @SUCCESS BIT = 1
	DECLARE @ERROR INT = 0
	
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
	
	-- 检查是否为合件
	IF NOT EXISTS (
		SELECT 1
		FROM TN_80_APP_0040_MBOM_VER_HLINK
		WHERE 
		CN_GUID_VER = @GUID_VER
		AND CN_GUID = @GUID
		AND CN_TYPE = 'C'
	)
	BEGIN
		SET @MSG = '所选件非合件'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		RETURN
	END

	BEGIN TRANSACTION
	-- 获取合件GUID_LINK，如果不为空，说明为烤漆合件，直接下级要特殊处理
	DECLARE @GUID_LINK UNIQUEIDENTIFIER
	DECLARE @ITEMCODE_PARENT VARCHAR(32)
	DECLARE @ITEMCODE VARCHAR(32)
	SELECT 
		 @GUID_LINK = CN_GUID_LINK
		,@ITEMCODE_PARENT = CN_ITEMCODE_PARENT
		,@ITEMCODE = CN_ITEMCODE
	FROM TN_80_APP_0040_MBOM_HLINK
	WHERE CN_GUID = @GUID
	AND CN_DT_EFFECTIVE_PBOM < @DATE
	AND CN_DT_EXPIRY_PBOM > @DATE
	AND CN_DT_EXPIRY > @DATE

	IF @GUID_LINK IS NOT NULL
	BEGIN
		-- 还原子级的原件，如果没有发布
		UPDATE MBOMHLINK
		SET CN_DT_EXPIRY = '2100-01-01',CN_DESC = ''
		FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		WHERE MBOMHLINK.CN_GUID = @GUID_LINK AND CN_IS_TOERP = 0
		SET @ERROR = @ERROR + @@ERROR
		-- 复制子级的原件，如果已经发布
		INSERT INTO [dbo].[TN_80_APP_0040_MBOM_HLINK]
           ([CN_GUID]
		   ,[CN_GUID_PBOM]
           ,[CN_GUID_LINK]
           ,[CN_GUID_EF]
           ,[CN_ITEMCODE_PARENT]
           ,[CN_ITEMCODE]
           ,[CN_QUANTITY]
           ,[CN_DISPLAYNAME]
           ,[CN_ORDER]
           ,[CN_UNIT]
           ,[CN_ISBORROW]
           ,[CN_ISMBOM]
		   ,[CN_IS_TOERP]
		   ,[CN_DT_TOERP]
           ,[CN_DT_CREATE]
           ,[CN_DT_EFFECTIVE_PBOM]
           ,[CN_DT_EXPIRY_PBOM]
           ,[CN_DT_EFFECTIVE]
           ,[CN_CREATE_BY]
           ,[CN_CREATE_NAME]
           ,[CN_CREATE_LOGIN])
		SELECT TOP 1
			NEWID()
           ,CN_GUID_PBOM
           ,CN_GUID_LINK
           ,CN_GUID_EF
           ,CN_ITEMCODE_PARENT
           ,CN_ITEMCODE
           ,CN_QUANTITY
           ,CN_DISPLAYNAME
           ,CN_ORDER
           ,CN_UNIT
           ,CN_ISBORROW
           ,CN_ISMBOM
		   ,CN_IS_TOERP
		   ,CN_DT_TOERP
           ,CN_DT_CREATE
           ,CN_DT_EFFECTIVE_PBOM
           ,CN_DT_EXPIRY_PBOM
           ,@DTEF
           ,[CN_CREATE_BY]
           ,[CN_CREATE_NAME]
           ,[CN_CREATE_LOGIN]
		FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		WHERE MBOMHLINK.CN_GUID = @GUID_LINK AND CN_IS_TOERP > 0
		SET @ERROR = @ERROR + @@ERROR
	END
	ELSE
	BEGIN
		-- 选中合件的下级有多个

		-- 将被引用到合件下的合件放回上级中，避免丢失引用
		UPDATE MBOMHLINK
		SET CN_ITEMCODE_PARENT = @ITEMCODE_PARENT
		FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		WHERE CN_ITEMCODE_PARENT = @ITEMCODE
		AND CN_GUID_LINK IS NULL
		AND CN_DT_EFFECTIVE_PBOM < @DATE
		AND CN_DT_EXPIRY_PBOM > @DATE
		AND MBOMHLINK.CN_DT_EXPIRY > @DATE 
		SET @ERROR = @ERROR + @@ERROR
		-- 获取这些下级的原件GUID
		SELECT DISTINCT CN_GUID_LINK AS CN_GUID
		INTO #CHILDS
		FROM
		TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		WHERE MBOMHLINK.CN_ITEMCODE_PARENT = @ITEMCODE
		-- 如果原件没有发布，则直接恢复
		UPDATE MBOMHLINK
		SET 
			 CN_DT_EXPIRY = '2100-01-01'
			,CN_DESC = ''
		FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		INNER JOIN #CHILDS ON #CHILDS.CN_GUID = MBOMHLINK.CN_GUID
		WHERE MBOMHLINK.CN_IS_TOERP = 0
		SET @ERROR = @ERROR + @@ERROR
		-- 如果原件已经发布，则复制子级的原件，重新添加
		INSERT INTO [dbo].[TN_80_APP_0040_MBOM_HLINK]
           ([CN_GUID]
           ,[CN_GUID_PBOM]
           ,[CN_GUID_LINK]
           ,[CN_GUID_EF]
           ,[CN_ITEMCODE_PARENT]
           ,[CN_ITEMCODE]
           ,[CN_QUANTITY]
           ,[CN_DISPLAYNAME]
           ,[CN_ORDER]
           ,[CN_UNIT]
           ,[CN_ISBORROW]
           ,[CN_ISMBOM]
		   ,[CN_IS_TOERP]
		   ,[CN_DT_TOERP]
           ,[CN_DT_CREATE]
           ,[CN_DT_EFFECTIVE_PBOM]
           ,[CN_DT_EXPIRY_PBOM]
           ,[CN_DT_EFFECTIVE]
           ,[CN_CREATE_BY]
           ,[CN_CREATE_NAME]
           ,[CN_CREATE_LOGIN])
		SELECT DISTINCT	
			NEWID()
           ,MBOMHLINK.CN_GUID_PBOM
           ,CN_GUID_LINK
           ,CN_GUID_EF
           ,CN_ITEMCODE_PARENT
           ,CN_ITEMCODE
           ,CN_QUANTITY
           ,CN_DISPLAYNAME
           ,CN_ORDER
           ,CN_UNIT
           ,CN_ISBORROW
           ,CN_ISMBOM
		   ,CN_IS_TOERP
		   ,CN_DT_TOERP
           ,CN_DT_CREATE
           ,CN_DT_EFFECTIVE_PBOM
           ,CN_DT_EXPIRY_PBOM
           ,@DTEF
           ,[CN_CREATE_BY]
           ,[CN_CREATE_NAME]
           ,[CN_CREATE_LOGIN]
		FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
		INNER JOIN #CHILDS ON #CHILDS.CN_GUID = MBOMHLINK.CN_GUID
		WHERE MBOMHLINK.CN_IS_TOERP > 0
		SET @ERROR = @ERROR + @@ERROR
	END
	
	-- 删除合件子级，没发布
	DELETE MBOMHLINK
	FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
	WHERE CN_ITEMCODE_PARENT = @ITEMCODE
	AND CN_IS_TOERP = 0
	SET @ERROR = @ERROR + @@ERROR
	-- 失效选中合件子级，已发布
	UPDATE MBOMHLINK
	SET 
		 CN_DT_EXPIRY = @DATE
		,CN_GUID_EX = @GUID_VER
		,CN_DESC = '[' + CONVERT(CHAR(19),@DATE,20) + ']' + @NAME + '删除合件 '+@ITEMCODE
	FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
	WHERE
		MBOMHLINK.CN_ITEMCODE_PARENT = @ITEMCODE
		AND MBOMHLINK.CN_DT_EFFECTIVE_PBOM < @DATE
		AND MBOMHLINK.CN_DT_EXPIRY_PBOM > @DATE
		AND MBOMHLINK.CN_DT_EFFECTIVE >= @DTEF
		AND MBOMHLINK.CN_DT_EXPIRY > @DATE 
		AND MBOMHLINK.CN_IS_TOERP > 0
	SET @ERROR = @ERROR + @@ERROR
	-- 删除选中件合件属性
	DELETE FROM TN_80_APP_0040_MBOM_VER_HLINK
	WHERE CN_GUID_VER = @GUID_VER
	AND CN_GUID = @GUID
	AND CN_TYPE = 'C'
	SET @ERROR = @ERROR + @@ERROR
	-- 删除选中合件，如果没有发布
	DELETE MBOMHLINK
	FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
	WHERE CN_GUID = @GUID AND CN_IS_TOERP = 0
	SET @ERROR = @ERROR + @@ERROR
	-- 无效选中合件，如果已被发布
	UPDATE MBOMHLINK
	SET 
		 CN_DT_EXPIRY = @DATE
		,CN_DESC = @NAME + ' 删除合件'
	FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
	WHERE CN_GUID = @GUID AND CN_IS_TOERP > 0
	SET @ERROR = @ERROR + @@ERROR
	
	IF @ERROR <> 0
	BEGIN
		SET @MSG = '删除合件失败，请联系管理员'
		RAISERROR(@MSG, 15, 1)
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		SET @MSG = '已成功删除合件。<p>合件的创建与删除可能会对BOM结构产生非预期结果，请仔细查证。</p>如有疑问请与管理员联系。'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		COMMIT TRANSACTION
	END
END;

 