USE [MBOM_TJB]
GO
/****** Object:  StoredProcedure [dbo].[PROC_COMPOSITE_ITEM_UNLINK]    Script Date: 05/15/2018 15:22:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PROC_COMPOSITE_ITEM_UNLINK]
(
@CODE VARCHAR(32),
@ITEMID INT,
@BOMID INT,
@LINK VARCHAR(300)
)
AS
BEGIN
	DECLARE @MSG VARCHAR(200);
	DECLARE @SUCCESS BIT = 1;
	DECLARE @ERROR INT = 0;
	
	DECLARE @NOW DATETIME = SYSDATETIME()
	DECLARE @MBOM_VER_ID INT
	DECLARE @ITEMCODE VARCHAR(32)

	IF @ITEMID = 0 OR @BOMID = 0 OR @LINK IS NULL OR LEN(RTRIM(@LINK)) = 0
	BEGIN
		SET @MSG = '参数不完整，请联系管理员'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		RETURN
	END
	
	-- 获取当前产品的最新版本ID
	SELECT TOP 1 @MBOM_VER_ID = CN_ID 
	FROM TN_80_APP_0040_MBOM_VER 
	WHERE CN_CODE = @CODE AND CN_SYS_STATUS = 'Y' 
	ORDER BY CN_DT_CREATE DESC
	
	IF @MBOM_VER_ID IS NULL
	BEGIN
		SET @MSG = '获取产品版本信息失败，请联系管理员'
		RAISERROR(@MSG, 15, 1)
		RETURN 
	END
	
	IF NOT EXISTS (
	SELECT 1
	FROM TN_80_APP_0000_ITEM_HLINK AS ITEMHLINK 
	WHERE ITEMHLINK.CN_ID = @ITEMID 
	AND ITEMHLINK.CN_COMPONENT_CLASS_ID = 105 AND ITEMHLINK.CN_COMPONENT_OBJECT_ID = 13
	)
	BEGIN
		SET @MSG = '所选件非合件直接父级，请选择合件的根节点'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		RETURN
	END
	
	IF NOT EXISTS (
		SELECT 1 FROM
		TN_80_APP_0040_MBOM_HLINK
		WHERE CN_VER_ID = @MBOM_VER_ID
		AND CN_SYS_STATUS = 'Y'
		AND CN_ISLINKED = 1
		AND CN_TYPEID = 2
		AND CN_ITEMID = @ITEMID
	)
	BEGIN
		SET @MSG = '所选件没有被引用'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		RETURN
	END
	
	IF EXISTS ( SELECT 1 FROM TN_80_APP_0000_ITEM AS ITEM WHERE ITEM.CN_ID = @ITEMID AND CN_IS_TOERP <> 0 )
	BEGIN
		-- 已经产生业务
		SET @MSG = '已经发布给ERP系统，无法取消合件引用'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		RETURN
	END
	
	DECLARE @PARENTCODE VARCHAR(30);
	DECLARE @PARENTNAME VARCHAR(30);
	DECLARE @PARENTID INT = 0;
	-- 所选件上级列表
	SELECT ID AS ITEMID
	INTO #TMP_PARENTS
	FROM SPLIT(@LINK,DEFAULT)
	WHERE ID <> @ITEMID
	
	-- 查询所选合件父级是否有合件，不允许跨级取消引用
	SELECT @PARENTID = P.ITEMID
	FROM #TMP_PARENTS AS P
	INNER JOIN TN_80_APP_0000_ITEM_HLINK AS ITEMH ON 
	ITEMH.CN_COMPONENT_OBJECT_ID = 13 
	AND ITEMH.CN_COMPONENT_CLASS_ID = 105
	AND ITEMH.CN_ID = P.ITEMID

	SELECT @PARENTCODE = CN_ITEM_CODE, @PARENTNAME = CN_NAME FROM TN_80_APP_0000_ITEM WHERE CN_ID = @PARENTID
	IF @PARENTID != 0 
	BEGIN
		SET @MSG = '所选件父级存在合件['+@PARENTCODE+' '+@PARENTNAME+']，无法取消合件引用'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		RETURN
	END

	-- 查询所选合件父级是否有虚件，不允许跨级取消引用
	;WITH X AS
	(
	SELECT 
		BOMHLINK.CN_COMPONENT_OBJECT_ID AS ITEMID,
		ITEM.CN_ID AS PARENTID
	FROM TN_80_APP_0025_BOM_HLINK AS BOMHLINK
	INNER JOIN TN_80_APP_0025_BOM AS BOM ON BOM.CN_ID = BOMHLINK.CN_BOM_ID
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON ITEM.CN_CODE = BOM.CN_CODE
	WHERE CN_COMPONENT_OBJECT_ID = @ITEMID
	AND (CN_STATUS_PBOM = 'Y' OR (CN_STATUS_PBOM = '' AND CN_STATUS_MBOM <> 'N' ))
	AND BOMHLINK.CN_SYS_STATUS <> 'N'

	UNION ALL

	SELECT 
		BOMHLINK.CN_COMPONENT_OBJECT_ID AS ITEMID,
		ITEM.CN_ID AS PARENTID
	FROM TN_80_APP_0025_BOM_HLINK AS BOMHLINK
	INNER JOIN TN_80_APP_0025_BOM AS BOM ON BOM.CN_ID = BOMHLINK.CN_BOM_ID
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON ITEM.CN_CODE = BOM.CN_CODE
	INNER JOIN X ON X.PARENTID = BOMHLINK.CN_COMPONENT_OBJECT_ID 
	WHERE 
	(CN_STATUS_PBOM = 'Y' OR (CN_STATUS_PBOM = '' AND CN_STATUS_MBOM <> 'N' ))
	AND BOMHLINK.CN_SYS_STATUS <> 'N'
	)
	SELECT @PARENTID = X.PARENTID FROM X
	INNER JOIN TN_80_APP_0000_ITEM_HLINK AS ITEMH ON ITEMH.CN_ID = X.PARENTID 
	AND ITEMH.CN_COMPONENT_CLASS_ID = 105 AND ITEMH.CN_COMPONENT_OBJECT_ID = 12 AND CN_SYS_STATUS = 'Y'

	SELECT @PARENTCODE = CN_ITEM_CODE, @PARENTNAME = CN_NAME FROM TN_80_APP_0000_ITEM WHERE CN_ID = @PARENTID
	IF @PARENTID != 0 
	BEGIN
		SET @MSG = '所选件父级存在虚件['+@PARENTCODE+' '+@PARENTNAME+']，无法取消合件引用'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		RETURN
	END

	-- 获取选中合件代号
	SELECT @ITEMCODE = CN_CODE FROM TN_80_APP_0000_ITEM WHERE CN_ID = @ITEMID
	
	BEGIN TRANSACTION 
	
	DECLARE @BOMHLINKID INT 
	
	-- -------------------------- --
	-- 修改MBOM_HLINK表引用关系
	-- -------------------------- --
	
	-- 取消引用
	UPDATE M
	SET
		 CN_ISLINKED = 0
		,CN_DT_MODIFY = @NOW
		,CN_DESC = '用户取消合件引用，合件代号：' + @ITEMCODE
	FROM TN_80_APP_0040_MBOM_HLINK AS M
	WHERE CN_VER_ID = @MBOM_VER_ID
	AND CN_SYS_STATUS = 'Y'
	AND CN_TYPEID = 2
	AND CN_ISLINKED = 1
	AND CN_LINK = @LINK
	SET @ERROR = @ERROR + @@ERROR
	
	-- ------------------------- --
	-- 修改BOM_HLINK表引用关系
	-- ------------------------- --
	
	-- 取消BOM_HLINK引用
	UPDATE TN_80_APP_0025_BOM_HLINK
	SET
		 CN_STATUS_MBOM = 'N'
		,CN_DT_EX_MBOM = @NOW
		,CN_DESC = '用户取消合件引用，合件代号：' + @ITEMCODE
	WHERE CN_COMPONENT_OBJECT_ID = @ITEMID
	AND CN_BOM_ID = @BOMID
	AND CN_STATUS_MBOM = 'Y'
	SET @ERROR = @ERROR + @@ERROR

	IF @ERROR <> 0
	BEGIN
		SET @MSG = '取消合件引用过程中发生错误，请联系管理员'
		RAISERROR(@MSG, 15, 1)
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		SET @MSG = '已取消合件引用'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		COMMIT TRANSACTION
	END
END;
GO
