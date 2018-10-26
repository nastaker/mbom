ALTER PROCEDURE [dbo].[PROC_MBOM_REMOVE]
(
@PROD_ITEMCODE VARCHAR(32),
@GUIDS VARCHAR(4000),
@USERID INT,
@NAME VARCHAR(30),
@LOGIN VARCHAR(30)
)
AS
BEGIN
	DECLARE @MSG NVARCHAR(200) = '已成功删除引用'
	DECLARE @SUCCESS BIT = 1
	DECLARE @DATE DATETIME = GETDATE()
	DECLARE @COUNT INT
	DECLARE @ROWCOUNT INT
	-- 获取版本
	DECLARE @GUID_VER UNIQUEIDENTIFIER
	DECLARE @DTEF DATE, @DTEX DATE
	SELECT 
	  @GUID_VER = CN_GUID
	, @DTEF = CN_DT_EFFECTIVE
	, @DTEX = CN_DT_EXPIRY 
	FROM dbo.GETMBOMVER(@PROD_ITEMCODE) 
	IF LEN(@PROD_ITEMCODE) > 0 AND @GUID_VER IS NULL
	BEGIN
		SET @MSG = '版本信息丢失，无法继续操作。请联系管理员。' 
		RAISERROR('%s',15,1, @MSG)
		RETURN
	END

	IF @DTEF IS NULL
	BEGIN
		SET @DTEF = GETDATE()
	END
	-- 获取所有GUID
	CREATE TABLE #TBL_GUID ( CN_GUID UNIQUEIDENTIFIER )
	
	INSERT INTO #TBL_GUID
	SELECT ID FROM SPLIT(@GUIDS, DEFAULT)
	
	SELECT @ROWCOUNT = COUNT(1) FROM #TBL_GUID
	
	BEGIN TRAN

	DELETE MBOMHLINK
	FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK
	INNER JOIN #TBL_GUID AS T ON T.CN_GUID = MBOMHLINK.CN_GUID
	AND CN_IS_TOERP = 0
	AND CN_ISMBOM = 1
	SET @COUNT = @COUNT + @@ROWCOUNT
	
	UPDATE MBOM
	SET 
		 CN_DT_EXPIRY = @DATE
		,CN_GUID_EX = @GUID_VER
		,CN_DESC = @NAME + ' 取消引用'
	FROM TN_80_APP_0040_MBOM_HLINK AS MBOM
	INNER JOIN #TBL_GUID AS T ON T.CN_GUID = MBOM.CN_GUID
	WHERE
	( CN_DT_EFFECTIVE_PBOM < @DATE
	AND CN_DT_EXPIRY_PBOM > @DATE
	AND CN_DT_EXPIRY > @DATE
	AND CN_IS_TOERP > 0
	) OR CN_ISMBOM = 0

	SET @COUNT = @COUNT + @@ROWCOUNT

	-- 删除类型
	DELETE MBOMHLINK
	FROM TN_80_APP_0040_MBOM_VER_HLINK  AS MBOMHLINK
	INNER JOIN #TBL_GUID ON #TBL_GUID.CN_GUID = MBOMHLINK.CN_GUID
	WHERE CN_GUID_VER = @GUID_VER 

	IF @COUNT <> @ROWCOUNT
	BEGIN
		SET @MSG = '删除引用失败，未能获取到要删除的引用，请刷新后重试。如仍有错误请联系管理员'
		SET @SUCCESS = 0
	END

	IF @SUCCESS = 0
	BEGIN
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		COMMIT TRANSACTION
	END
	SELECT @SUCCESS AS SUCCESS, @MSG AS MSG
END;
