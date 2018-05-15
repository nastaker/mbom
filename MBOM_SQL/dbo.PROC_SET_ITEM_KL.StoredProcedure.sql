USE [MBOM_TJB]
GO
/****** Object:  StoredProcedure [dbo].[PROC_SET_ITEM_KL]    Script Date: 05/15/2018 15:22:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PROC_SET_ITEM_KL]
(
@BOMHLINKIDS VARCHAR(200),
@PROCESSHLINKID INT,
@USERID INT,
@NAME VARCHAR(20),
@LOGIN VARCHAR(20)
)
AS
BEGIN
	DECLARE @MSG VARCHAR(200)
	DECLARE @SUCCESS BIT = 1
	DECLARE @ERROR INT = 0

	IF @PROCESSHLINKID = 0 OR LEN(@BOMHLINKIDS) = 0
	BEGIN
		SET @MSG = '参数传递错误，请联系管理员'
		RAISERROR(@MSG, 15, 1)
		RETURN
	END

	SELECT CAST(ID AS INT) AS ID
	INTO
	#tmpids
	FROM SPLIT(@BOMHLINKIDS,default)

	IF EXISTS ( SELECT 1 FROM [dbo].[TN_80_APP_0025_BOM_HLINK_KL] WHERE CN_IS_TOERP = 1 AND CN_BOM_HLINK_ID IN ( SELECT ID FROM #tmpids ) )
	BEGIN
		DELETE FROM [dbo].[TN_80_APP_0025_BOM_HLINK_KL] WHERE CN_IS_TOERP = 1 AND CN_BOM_HLINK_ID IN ( SELECT ID FROM #tmpids )
	END

	BEGIN TRANSACTION
	
	INSERT INTO [dbo].[TN_80_APP_0025_BOM_HLINK_KL]
	(
		CN_BOM_HLINK_ID,
		CN_PROCESS_HLINK_ID,
		CN_GX_CODE,
		CN_GX_NAME,
		CN_CREATE_BY,
		CN_CREATE_NAME
	)
	SELECT
		ID,
		PVH.CN_HLINK_ID,
		PVH.CN_GX_CODE,
		PVH.CN_GX_NAME,
		@USERID,
		@NAME
	FROM
	#tmpids
	INNER JOIN TN_80_APP_0020_PROCESS_VER_HLINK AS PVH ON PVH.CN_HLINK_ID = @PROCESSHLINKID

	SELECT @ERROR = @@ERROR + @ERROR

	IF @ERROR <> 0
	BEGIN
		SET @MSG = '设置扣料信息失败，请联系管理员'
		RAISERROR(@MSG, 15, 1)
		ROLLBACK TRANSACTION
	END
	ELSE
	BEGIN
		SET @MSG = '设置扣料信息完成'
		SELECT @MSG AS MSG, @SUCCESS AS SUCCESS
		COMMIT TRANSACTION
	END

END;
GO
