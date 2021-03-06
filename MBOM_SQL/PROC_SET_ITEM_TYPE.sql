alter PROCEDURE [dbo].[PROC_SET_ITEM_TYPE]
(
@ITEMCODE VARCHAR(32),
@TYPEID INT,
@USERID INT,
@NAME VARCHAR(30),
@LOGIN VARCHAR(30)
)
AS
BEGIN
	DECLARE @MSG VARCHAR(200) = '';
	DECLARE @SUCCESS BIT = 1;
	DECLARE @ERROR INT = 0

	IF LEN(RTRIM(@ITEMCODE)) = 0 OR @TYPEID IS NULL OR @TYPEID = 0
	BEGIN
		SET @MSG = '未能获取到参数，请联系管理员。PROC-0404';
		SELECT @MSG AS MSG,@SUCCESS AS SUCCESS
		RETURN
	END

	DECLARE @DATE DATETIME = GETDATE()
	DECLARE @TYPE_ID_PURCHASE INT = 2
	DECLARE @TYPE_ID_SELFMADE INT = 3

	DECLARE @ITEMID INT

	-- 获取要变更的当前物料的类型，如果是相同的，则不变化，不同则取消上条记录有效性，新增一条类型记录
  
	-- 如果TYPE和要变更的TYPEID一致，那么无需变更
	IF EXISTS ( 
		SELECT 1 FROM TN_80_APP_0000_ITEM AS ITEM 
		INNER JOIN TN_80_APP_0000_ITEM_HLINK AS ITEMHLINK ON ITEMHLINK.CN_ID = ITEM.CN_ID
			AND ITEMHLINK.CN_COMPONENT_CLASS_ID = 105
			AND ITEMHLINK.CN_COMPONENT_OBJECT_ID = @TYPEID
			AND (ITEMHLINK.CN_SYS_STATUS = '' OR ITEMHLINK.CN_SYS_STATUS = 'Y') 
		WHERE ITEM.CN_ITEM_CODE = @ITEMCODE
	)
	BEGIN
		SET @MSG = '物料已经是你要设置的采购或自制件类型，无需重复设置。PROC-0500';
		SELECT @MSG AS MSG,@SUCCESS AS SUCCESS
		RETURN
	END

	SELECT @ITEMID = CN_ID
	FROM TN_80_APP_0000_ITEM
	WHERE CN_ITEM_CODE = @ITEMCODE
	
	DECLARE @CAN_EXPAND BIT = 1
	IF @TYPEID = @TYPE_ID_PURCHASE
	BEGIN
		SET @CAN_EXPAND = 0
	END
	
	DECLARE @NEW_TYPE_NAME VARCHAR(32)
	
	SELECT @NEW_TYPE_NAME = CN_NAME
	FROM TN_50_DIC_0005_ITEMTYPE
	WHERE CN_ID = @TYPEID
	
	DECLARE @TYPE_ID_ORIGINAL INT
	
	SELECT @TYPE_ID_ORIGINAL = ITEMHLINK.CN_COMPONENT_OBJECT_ID FROM TN_80_APP_0000_ITEM AS ITEM 
	INNER JOIN TN_80_APP_0000_ITEM_HLINK AS ITEMHLINK ON ITEMHLINK.CN_ID = ITEM.CN_ID
		AND ITEMHLINK.CN_COMPONENT_CLASS_ID = 105
		AND ITEMHLINK.CN_COMPONENT_OBJECT_ID IN (@TYPE_ID_PURCHASE,@TYPE_ID_SELFMADE)
		AND (ITEMHLINK.CN_SYS_STATUS = '' OR ITEMHLINK.CN_SYS_STATUS = 'Y') 
	WHERE ITEM.CN_ITEM_CODE = @ITEMCODE
	 
	UPDATE TN_80_APP_0000_ITEM_HLINK
	SET 
	     CN_SYS_STATUS = 'N'
		,CN_DT_EXPIRY = @DATE
	WHERE
		CN_ID = @ITEMID
		AND CN_COMPONENT_CLASS_ID = 105
		AND CN_COMPONENT_OBJECT_ID = @TYPE_ID_ORIGINAL
 
	INSERT INTO [dbo].[TN_80_APP_0000_ITEM_HLINK]
        ([CN_ID]
        ,[CN_COMPONENT_CLASS_ID]
        ,[CN_COMPONENT_OBJECT_ID]
        ,[CN_ISBORROW]
        ,[CN_ORDER]
        ,[CN_F_QUANTITY]
        ,[CN_UNIT]
        ,[CN_DISPLAYNAME]
        ,[CN_CREATE_BY]
        ,[CN_CREATE_NAME]
        ,[CN_CREATE_LOGIN] ) 
	VALUES
		(@ITEMID
		,105
		,@TYPEID
		,0
		,0
		,1
		,''
		,@NEW_TYPE_NAME
		,@USERID
		,@NAME
		,@LOGIN )

	SET @ERROR = @ERROR + @@ERROR

	IF @ERROR <> 0
	BEGIN
		SET @MSG = '设置失败，请联系管理员'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		SET @MSG = '修改/设置类型成功'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		COMMIT TRANSACTION
	END
END;
