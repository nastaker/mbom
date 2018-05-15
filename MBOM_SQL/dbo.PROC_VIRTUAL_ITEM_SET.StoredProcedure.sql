USE [MBOM_TJB]
GO
/****** Object:  StoredProcedure [dbo].[PROC_VIRTUAL_ITEM_SET]    Script Date: 05/15/2018 15:22:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[PROC_VIRTUAL_ITEM_SET]
(
	@CODE VARCHAR(32),
	@BOMID INT,
	@ITEMID INT,
	@SHOW BIT,
	@USERID INT,
	@NAME VARCHAR(20),
	@LOGIN VARCHAR(20)
)
AS
BEGIN
	-- 105 表示零件类型
	DECLARE @COMPONENT_CLASS_ID INT = 105
	-- 12 为 MBOM虚拟件
	DECLARE @COMPONENT_VIRTUAL_ID INT = 12
	-- 13 为 MBOM合件
	DECLARE @COMPONENT_COMBINE_ID INT = 13
	
	DECLARE @ITEMCODE VARCHAR(32)
	DECLARE @NOW DATETIME = SYSDATETIME()
	
	DECLARE @MBOM_VER_ID INT
	
	DECLARE @ERROR INT = 0
	DECLARE @SUCCESS BIT = 1
	DECLARE @MSG NVARCHAR(2000) = ''
	
	-- 获取版本信息
	SELECT TOP 1 @MBOM_VER_ID = CN_ID 
	FROM TN_80_APP_0040_MBOM_VER 
	WHERE CN_CODE = @CODE AND CN_SYS_STATUS = 'Y' AND CN_IS_TOERP <> 2
	ORDER BY CN_DT_CREATE DESC
	
	IF @MBOM_VER_ID IS NULL
	BEGIN
		SET @MSG = '版本信息丢失，无法继续操作。请联系管理员。' 
		RAISERROR('%s',15,1, @MSG)
		RETURN
	END

	IF NOT EXISTS (
		SELECT 1
		FROM 
		TN_80_APP_0000_ITEM AS ITEM
		INNER JOIN TN_80_APP_0025_BOM AS BOM ON ITEM.CN_CODE = BOM.CN_CODE
		WHERE
		ITEM.CN_ID = @ITEMID
	)
	BEGIN
		SET @MSG = '设置失败：选择的件没有子件，无法设置为虚件' 
		RAISERROR('%s',15,1, @MSG)
		RETURN
	END

	IF EXISTS (
		SELECT 1 FROM TN_80_APP_0040_MBOM_HLINK 
		WHERE 
		CN_VER_ID = @MBOM_VER_ID
		AND CN_SYS_STATUS = 'Y'
		AND CN_ITEMID = @ITEMID
		AND CN_TYPEID = 1
		)
	BEGIN
		SET @MSG = '设置失败：选择的件已经设置为虚件' 
		RAISERROR('%s',15,1, @MSG)
		RETURN
	END

	IF EXISTS (
		SELECT 1 FROM TN_80_APP_0040_MBOM_HLINK 
		WHERE 
		CN_VER_ID = @MBOM_VER_ID
		AND CN_SYS_STATUS = 'Y'
		AND CN_ITEMID = @ITEMID
		AND CN_TYPEID = 2
		)
	BEGIN
		SET @MSG = '设置失败：选择的件已经设置为合件' 
		RAISERROR('%s',15,1, @MSG)
		RETURN
	END
	
	SELECT @ITEMCODE = CN_ITEM_CODE 
	FROM TN_80_APP_0000_ITEM
	WHERE CN_ID = @ITEMID
	
	BEGIN TRANSACTION
	DECLARE @DATE DATETIME = GETDATE()

	INSERT INTO [DBO].[TN_80_APP_0000_ITEM_HLINK]
           ([CN_ID]
           ,[CN_ISDELETE]
           ,[CN_COMPONENT_CLASS_ID]
           ,[CN_COMPONENT_OBJECT_ID]
           ,[CN_COMPONENT_OBJECT_VER_ID]
           ,[CN_COMPONENT_OBJECT_VERSION]
           ,[CN_S_ATTACH_DATA]
           ,[CN_ITEMSTATE_TAGGER_DATA]
           ,[CN_ISFOLDER]
           ,[CN_ISBORROW]
           ,[CN_ORDER]
           ,[CN_NUMBER]
           ,[CN_F_QUANTITY]
           ,[CN_UNIT]
           ,[CN_B_IS_ASSEMBLY]
           ,[CN_DISPLAYNAME]
           ,[CN_S_FROM]
           ,[CN_DESC]
           ,[CN_CREATE_BY]
           ,[CN_CREATE_NAME]
           ,[CN_CREATE_LOGIN]
           ,[CN_SYS_STATUS]
           ,[CN_SYS_NOTE] ) 
	SELECT
		    @ITEMID			-- ID 
		   ,0				-- ISDELETE
		   ,@COMPONENT_CLASS_ID
		   ,@COMPONENT_VIRTUAL_ID
		   ,0				-- COMPONENT OBJECT VER ID
		   ,''				-- COMPONENT OBJECT VERSION
		   ,''				-- ATTACH DATA
		   ,0				-- ITEMSTATE TAGGER DATA
		   ,0				-- ISFOLDER
		   ,0				-- ISBORROW
		   ,0				-- ORDER
		   ,0				-- NUMBER
		   ,0				-- QUANTITY
		   ,''				-- UNIT
		   ,0				-- IS ASEEMBLY
		   ,'MBOM虚拟件'	-- DISPLAYNAME
		   ,''				-- FROM
		   ,NULL			-- DESC
		   ,@USERID			-- CREATE BY
		   ,@NAME			-- CREATE NAME
		   ,@LOGIN			-- CREATE LOGIN
		   ,'Y'				-- SYS STATUS
		   ,NULL			-- SYS NOTE
	-- MBOM_HLINK，将选中虚件复制一份出来
	INSERT INTO [TN_80_APP_0040_MBOM_HLINK]
       ([CN_BOM_HLINK_ID]
       ,[CN_VER_ID]
       ,[CN_PARENT_LINK]
       ,[CN_LINK]
       ,[CN_ITEMID]
       ,[CN_ISLINKED]
       ,[CN_ISSHOW]
       ,[CN_TYPEID]
       ,[CN_TYPENAME]
       ,[CN_SYS_STATUS]
       ,[CN_DESC]
       ,[CN_CREATE_BY]
       ,[CN_CREATE_NAME]
       ,[CN_CREATE_LOGIN])
	SELECT
		[CN_BOM_HLINK_ID]
       ,@MBOM_VER_ID
       ,[CN_PARENT_LINK]
       ,[CN_LINK]
       ,[CN_ITEMID]
       -- 才设置为虚件，默认不引用
       ,0
       -- 虚件，是否显示在列表
       ,@SHOW
		-- TYPEID 说明
		-- 0：普通件、1：虚件、2：烤漆合件、3：离散包、4：包装返厂
       ,1
       ,'虚件'
       ,'Y'
       ,'用户设置虚件'
       ,@USERID
       ,@NAME
       ,@LOGIN
	FROM TN_80_APP_0040_MBOM_HLINK AS MBOMHLINK 
	WHERE 
	CN_VER_ID = @MBOM_VER_ID
	AND MBOMHLINK.CN_SYS_STATUS = 'Y' 
	AND CN_ITEMID = @ITEMID
	
	-- 从_MBOM_中移除所有此件的链接。
	UPDATE TN_80_APP_0040_MBOM_HLINK
	SET 
		 CN_ISLINKED = 0
		,CN_DT_MODIFY = @NOW
	WHERE 
	CN_VER_ID = @MBOM_VER_ID
	AND CN_ISLINKED = 1
	AND CN_SYS_STATUS = 'Y'
	AND CN_ITEMID = @ITEMID
	SET @ERROR = @@ERROR + @ERROR

	IF @ERROR <> 0
	BEGIN
		SET @SUCCESS = 0
		SET @MSG = '设置虚件失败，请联系管理员'
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		SET @MSG = '设置虚件成功'
		COMMIT TRANSACTION
	END

	SELECT @MSG AS MSG, @SUCCESS AS SUCCESS

END;
GO
