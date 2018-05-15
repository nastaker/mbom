USE [MBOM_TJB]
GO
/****** Object:  StoredProcedure [dbo].[PROC_ITEM_LINK]    Script Date: 05/15/2018 15:22:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PROC_ITEM_LINK]
(
@PID INT,
@PLINK VARCHAR(200),
@ITEMID INT,
@QUANTITY FLOAT,
@USERID INT,
@NAME NVARCHAR(30),
@LOGIN NVARCHAR(30)
)
AS
BEGIN
	DECLARE @MSG NVARCHAR(200) = '引用成功';
	DECLARE @SUCCESS BIT = 1;

	DECLARE @NOW DATETIME = SYSDATETIME()
	DECLARE @BOMID INT

	SELECT @BOMID = BOM.CN_ID
	FROM TN_80_APP_0025_BOM AS BOM
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON ITEM.CN_CODE = BOM.CN_CODE
	WHERE ITEM.CN_ID = @PID
	

	IF NOT EXISTS ( SELECT 1 FROM TN_80_APP_0000_ITEM WHERE CN_ID = @ITEMID AND CN_PDM_CLS_ID = 0 )
	BEGIN
		SET @MSG = '所引用件不存在，或不符合引用条件。如有问题请联系管理员。'
		RAISERROR( @MSG, 15, 1 )
		RETURN
	END

	--如果不存在BOMID，则说明非组件，需要自动添加一条组件记录
	IF @BOMID IS NULL
	BEGIN 
		INSERT INTO [dbo].[TN_80_APP_0025_BOM]
			([CN_CODE]
			,[CN_ITEM_CODE]
			,[CN_NAME]
			,[CN_TYPE]
			,[CN_DESC]
			,[CN_CREATE_BY]
			,[CN_CREATE_NAME]
			,[CN_CREATE_LOGIN]
			,[CN_SYS_STATUS]
			,[CN_DT_CREATE])
		SELECT 
			 ITEM.CN_CODE
			,ITEM.CN_ITEM_CODE
			,ITEM.CN_NAME
			,''
			,'自定义引用物料，此条作为父级之前不存在，由系统自行添加'
			,@USERID
			,@NAME
			,@LOGIN
			,'Y'
			,@NOW
		FROM TN_80_APP_0000_ITEM AS ITEM
		WHERE ITEM.CN_ID = @PID 

		SET @BOMID = @@IDENTITY

		UPDATE TN_80_APP_0025_BOM_HLINK SET CN_B_IS_ASSEMBLY = 1 WHERE CN_COMPONENT_OBJECT_ID = @PID
	END

	-- 添加引用
	INSERT INTO TN_80_APP_0025_BOM_HLINK 
		( CN_BOM_ID
		, CN_S_BOM_TYPE
		, CN_COMPONENT_CLASS_ID
		, CN_COMPONENT_OBJECT_ID
		, CN_B_IS_ASSEMBLY
		, CN_ORDER
		, CN_F_QUANTITY
		, CN_STATUS_MBOM
		, CN_CREATE_BY
		, CN_CREATE_LOGIN
		, CN_CREATE_NAME
		)
	SELECT 
		  @BOMID
		, '自定义物料'
		, 100
		, @ITEMID
		, 1
		, 0
		, @QUANTITY
		, 'Y'
		, @USERID
		, @LOGIN
		, @NAME

	SELECT @MSG AS MSG, @SUCCESS AS SUCCESS

END;
GO
