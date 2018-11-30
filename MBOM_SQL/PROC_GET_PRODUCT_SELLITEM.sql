CREATE PROCEDURE [dbo].[PROC_GET_PRODUCT_SELLITEM]
(
@PROD_ITEMCODE VARCHAR(24),
@GUID_VER UNIQUEIDENTIFIER
)
AS
BEGIN
	DECLARE @DATE DATETIME = GETDATE()

	;WITH X AS 
	(
		SELECT
		ITEM.CN_ID AS ITEMID,
		ITEM.CN_ITEM_CODE AS ITEM_CODE,
		ITEM.CN_CODE AS CODE,
		ITEM.CN_NAME AS NAME,

		PBOMHLINK.CN_ISBORROW AS ISBORROW
		FROM 
		TN_80_APP_0030_PBOM_HLINK AS PBOMHLINK
		INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON PBOMHLINK.CN_ITEMCODE = ITEM.CN_ITEM_CODE
		WHERE 
		PBOMHLINK.CN_ITEMCODE_PARENT = @PROD_ITEMCODE
		AND PBOMHLINK.CN_DT_EFFECTIVE < @DATE
		AND PBOMHLINK.CN_DT_EXPIRY > @DATE

		UNION ALL

		SELECT
		ITEM.CN_ID AS CN_ITEMID,
		ITEM.CN_ITEM_CODE,
		ITEM.CN_CODE,
		ITEM.CN_NAME,

		PBOMHLINK.CN_ISBORROW
		FROM
		TN_80_APP_0030_PBOM_HLINK AS PBOMHLINK
		INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON PBOMHLINK.CN_ITEMCODE = ITEM.CN_ITEM_CODE
		INNER JOIN X ON X.ITEM_CODE = PBOMHLINK.CN_ITEMCODE_PARENT
		WHERE
		PBOMHLINK.CN_DT_EFFECTIVE < @DATE
		AND PBOMHLINK.CN_DT_EXPIRY > @DATE

	)
	SELECT 
	* 
	INTO #TMP
	FROM 
	(
	SELECT 
	CN_ID AS ITEMID
	,CN_ITEM_CODE AS ITEM_CODE
	,CN_CODE AS CODE
	,CN_NAME AS NAME
	,CAST(0 AS BIT) AS ISBORROW
	,NULL AS ISBORROWSTR
	FROM TN_80_APP_0000_ITEM WHERE CN_ITEM_CODE = @PROD_ITEMCODE
	UNION
	SELECT DISTINCT
	X.ITEMID,
	X.ITEM_CODE,
	X.CODE,
	X.NAME,
	X.ISBORROW,
	CASE X.ISBORROW WHEN 1 THEN 'Y' END AS ISBORROWSTR
	FROM X
	) AS T

	DECLARE @PRODID INT
	SELECT @PRODID = CN_ID
	FROM TN_80_APP_0010_PRODUCT
	WHERE CN_ITEM_CODE = @PROD_ITEMCODE

	SELECT 
	X.*
	,PRODHL.CN_CUSTOMERCODE AS CUS_CODE
	,PRODHL.CN_CUSTOMERNAME AS CUS_NAME
	,PRODHL.CN_CUSTOMERITEMCODE AS CUS_ITEMCODE
	,PRODHL.CN_CUSTOMERITEMNAME AS CUS_ITEMNAME
	,PRODHL.CN_SHIPPINGADDR AS CUS_SHIPPINGADDR
	,CONVERT(CHAR(20),PRODHL.CN_DT_CREATE,20) AS PDATE
	FROM #TMP AS X
	INNER JOIN TN_80_APP_0010_PRODUCT_HLINK AS PRODHL 
	ON X.ITEMID = PRODHL.CN_COMPONENT_OBJECT_ID
	AND PRODHL.CN_COMPONENT_CLASS_ID = 100
	AND PRODHL.CN_ID = @PRODID
	AND PRODHL.CN_GUID_VER = @GUID_VER
	ORDER BY ITEM_CODE
END;
GO

[PROC_GET_PRODUCT_SELLITEM] 'HGS7D3872000P1','958F8FF2-5E96-4D55-9B00-2C228C4B57CB'
