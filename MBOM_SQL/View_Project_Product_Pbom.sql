ALTER VIEW [dbo].[View_Project_Product_Pbom]
AS
SELECT
	PRODUCT.CN_CODE AS PRODUCT_CODE
	,PRODUCT.CN_ITEM_CODE AS PRODUCT_ITEM_CODE
	,PRODUCT.CN_NAME AS PRODUCT_NAME
	,PRODUCT.CN_STATUS AS PRODUCT_STATUS
	,PRODUCT.CN_CHECK_STATUS AS CHECK_STATUS
	,PRODUCT.CN_SALE_SET AS SALE_SET
	,PRODUCT.CN_DT_PDM AS DT_PDM
	,PRODUCT.CN_USER_SELL AS USER_SELL
	,PRODUCT.CN_DT_SELL AS DT_SELL
	,PRODUCT.CN_USER_PRE AS USER_PRE
	,PRODUCT.CN_DT_PRE AS DT_PRE
	,PRODUCT.CN_USER_MAINTAIN AS USER_MAINTAIN
	,PRODUCT.CN_DT_MAINTAIN AS DT_MAINTAIN
	,PRODUCT.CN_USER_MBOM AS USER_MBOM
	,PRODUCT.CN_DT_MBOM AS DT_MBOM
	,PRODUCT.CN_DT_DATA_INTE AS DT_DATA_INTE
	,PRODUCT.CN_DT_DATA_MDM AS DT_DATA_MDM
	,PRODUCT.CN_DT_DATA_ERP AS DT_DATA_ERP
	,PROJECT.CN_ID AS PROJECT_ID
	,PROJECT.CN_CODE AS CODE
	,PROJECT.CN_NAME AS PROJECT_NAME
	,PROJECT.CN_OWNER_ID AS OWNER_ID
	,PROJECT.CN_OWNER_NAME AS OWNER_NAME 
	,PROJECT.CN_GROUPCODE AS GROUPCODE
	,PBOM_VER.CN_VER AS PBOMVER
	,PBOM_VER.CN_DT_VER AS PBOMVER_DT
	,PBOM_VER.CN_CREATE_NAME AS PBOMVER_CREATE_NAME
	,PROD_VER.CN_NAME AS PRODVER_NAME
	,PROD_VER.CN_IS_TOERP AS PRODVER_STATUS
FROM
dbo.TN_80_APP_0010_PRODUCT AS PRODUCT
LEFT JOIN dbo.TN_80_APP_0005_PROJECT_HLINK AS PRJ_HLINK ON PRJ_HLINK.CN_COMPONENT_OBJECT_ID = PRODUCT.CN_ID AND PRJ_HLINK.CN_COMPONENT_CLASS_ID = 300
LEFT JOIN dbo.TN_80_APP_0005_PROJECT AS PROJECT ON PROJECT.CN_ID = PRJ_HLINK.CN_ID
LEFT JOIN dbo.TN_80_APP_0030_PBOM_VER AS PBOM_VER ON PRODUCT.CN_CODE = PBOM_VER.CN_CODE AND PBOM_VER.CN_SYS_STATUS = 'Y'
LEFT JOIN DBO.TN_80_APP_0010_PRODUCT_VER AS PROD_VER ON PRODUCT.CN_CODE = PROD_VER.CN_PRODUCT_CODE AND PROD_VER.CN_STATUS = 'Y'
GO 