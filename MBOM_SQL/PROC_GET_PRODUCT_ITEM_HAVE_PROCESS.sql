ALTER PROCEDURE [dbo].[PROC_GET_PRODUCT_ITEM_HAVE_PROCESS]
(
@PROD_ITEMCODE NCHAR(30)
)
AS
BEGIN
	WITH X AS 
	(
		SELECT
		ITEM.CN_ID AS CN_ITEMID,
		ITEM.CN_ITEM_CODE
		FROM
		TN_80_APP_0030_PBOM_HLINK AS PBOMHLINK
		INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON PBOMHLINK.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
		WHERE 
		PBOMHLINK.CN_ITEMCODE_PARENT = @PROD_ITEMCODE

		UNION ALL

		SELECT
		ITEM.CN_ID AS CN_ITEMID,
		ITEM.CN_ITEM_CODE
		FROM 
		TN_80_APP_0030_PBOM_HLINK AS PBOMHLINK
		INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON PBOMHLINK.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
		INNER JOIN X ON X.CN_ITEM_CODE = PBOMHLINK.CN_ITEMCODE_PARENT
	)
	SELECT DISTINCT
	X.CN_ITEMID AS ITEMID,
	X.CN_ITEM_CODE AS ITEMCODE,
	P.CN_NAME AS NAME
	FROM X
	INNER JOIN TN_80_APP_0020_PROCESS AS P ON X.CN_ITEM_CODE = P.CN_ITEM_CODE
END;
GO