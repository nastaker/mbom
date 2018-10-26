ALTER PROCEDURE [dbo].[PROC_VIRTUAL_ITEM_UNLINK]
(
@PROD_ITEMCODE VARCHAR(32),
@GUID UNIQUEIDENTIFIER,
@USERID INT,
@NAME VARCHAR(20),
@LOGIN VARCHAR(20)
)
AS
BEGIN
	DECLARE @MSG VARCHAR(2000)
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
	
	-- 判断选中件是否为虚件，不是虚件不予处理
	IF NOT EXISTS ( 
		SELECT 1
		FROM TN_80_APP_0040_MBOM_VER_HLINK
		WHERE
			CN_GUID_VER = @GUID_VER
			AND CN_GUID = @GUID
			AND CN_TYPE = 'V'
	)
	BEGIN
		SET @MSG = '选择的件不是虚件，无法取消引用'
		RAISERROR(@MSG, 15, 1)
		RETURN;
	END

	BEGIN TRANSACTION
	
	-- 删除第一级子级虚件属性
	DELETE FROM VERHLINK
	FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
	INNER JOIN TN_80_APP_0040_MBOM_VER_HLINK AS VERHLINK ON VERHLINK.CN_GUID = MBOMHLINK.CN_GUID
		AND VERHLINK.CN_GUID_VER = @GUID_VER
		AND VERHLINK.CN_TYPE = 'V'
	WHERE
		CN_GUID_LINK = @GUID
		AND CN_DT_EFFECTIVE_PBOM < @DATE
		AND CN_DT_EXPIRY_PBOM > @DATE
		AND CN_DT_EXPIRY > @DATE 
	SET @ERROR = @ERROR + @@ERROR
	-- 删除【未发布】虚件下级
	DELETE TN_80_APP_0040_MBOM_HLINK
	WHERE CN_GUID_LINK = @GUID
	AND CN_IS_TOERP = 0
	-- 失效【已发布】虚件下级
	UPDATE TN_80_APP_0040_MBOM_HLINK
	SET 
		CN_DT_EXPIRY = @DATE,
		CN_DESC = @NAME + ' 删除虚件引用'
	WHERE
		CN_GUID_LINK = @GUID
		AND CN_DT_EFFECTIVE_PBOM < @DATE
		AND CN_DT_EXPIRY_PBOM > @DATE
		AND CN_DT_EXPIRY > @DATE
		AND CN_IS_TOERP = 2
	SET @ERROR = @ERROR + @@ERROR
	
	-- 更新虚件引用状态为【未引用】
	UPDATE TN_80_APP_0040_MBOM_VER_HLINK
	SET 
		CN_STATUS = '_V',
		CN_DESC = @NAME + ' 删除虚件引用'
	WHERE
		CN_GUID_VER = @GUID_VER
		AND CN_GUID = @GUID
	SET @ERROR = @ERROR + @@ERROR
	
	
	IF @ERROR = 0
	BEGIN
		SET @MSG = '已经成功取消虚件的引用'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		COMMIT TRANSACTION
	END
	ELSE
	BEGIN
		SET @MSG = '取消虚件引用失败，请联系管理员'
		RAISERROR(@MSG, 15, 1)
		ROLLBACK TRANSACTION
	END

END;
