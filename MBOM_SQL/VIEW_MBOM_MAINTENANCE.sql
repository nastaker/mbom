ALTER VIEW [dbo].[VIEW_MBOM_MAINTENANCE]
AS
SELECT
	-- PROJECT 项目信息
	 PROJECT.CN_ID AS PROJECT_ID
	,PROJECT.CN_CODE AS CODE
	,PROJECT.CN_NAME AS PROJECT_NAME
	-- PRODUCT 产品信息
	,PRODUCT.CN_CODE AS PRODUCT_CODE
	,PRODUCT.CN_NAME AS PRODUCT_NAME
	,PRODUCT.CN_ITEM_CODE AS PRODUCT_ITEM_CODE
	,PRODUCT.CN_STATUS AS PRODUCT_STATUS
	,CASE PRODUCT.CN_STATUS WHEN '转批中' THEN '新' ELSE 
		CASE PRODUCT.CN_STATUS WHEN '可发布' THEN '新' ELSE '改' END
	 END AS TECH_STATUS
	,PRODUCT.CN_CHECK_STATUS AS CHECK_STATUS
	,PRODUCT.CN_SALE_SET AS SALE_SET
	,PRODUCT.CN_DESC AS [DESC]
	-- PBOM 版本信息
	,PBOM_VER.CN_VER AS PBOMVER
	,CAST(PBOM_VER.CN_GUID AS VARCHAR(48)) AS PBOMVER_GUID
	,PBOM_VER.CN_DT_VER AS DT_PBOMVER
	,PBOM_VER.CN_CREATE_NAME AS PBOM_CREATE_NAME
	-- MBOM
	-- MBOMVER
	,MBOM_VER.CN_VER AS MBOMVER
	,MBOM_VER.CN_DT_CREATE AS DT_MBOMVER
	,MBOM_VER.CN_CREATE_NAME AS MBOM_CREATE_NAME
	,MBOM_VER.CN_IS_TOERP AS MBOMVER_IS_TOERP
	-- PROJECT 项目创建者信息
	,PROJECT.CN_OWNER_ID AS OWNER_ID
	,PROJECT.CN_OWNER_NAME AS OWNER_NAME
	,PRODUCT.CN_MARK AS MARK
FROM         
dbo.TN_80_APP_0005_PROJECT AS PROJECT 
INNER JOIN dbo.TN_80_APP_0005_PROJECT_HLINK AS PRJ_HLINK ON PROJECT.CN_ID = PRJ_HLINK.CN_ID 
INNER JOIN dbo.TN_80_APP_0010_PRODUCT AS PRODUCT ON PRJ_HLINK.CN_COMPONENT_OBJECT_ID = PRODUCT.CN_ID AND PRJ_HLINK.CN_COMPONENT_CLASS_ID = 300
INNER JOIN dbo.TN_80_APP_0030_PBOM_VER AS PBOM_VER ON PRODUCT.CN_CODE = PBOM_VER.CN_CODE AND PBOM_VER.CN_SYS_STATUS = 'Y'
LEFT JOIN DBO.TN_80_APP_0040_MBOM_VER AS MBOM_VER ON MBOM_VER.CN_GUID_PBOM = PBOM_VER.CN_GUID and MBOM_VER.CN_STATUS = 'Y'
WHERE [PRODUCT].CN_IS_TOERP = 0
AND PRODUCT.CN_STATUS NOT IN ('待发起预转批','销售件已设置')
GO


select * from [VIEW_MBOM_MAINTENANCE]
 WHERE PRODUCT_ITEM_CODE = 'HGS7A768000P1'

 SELECT * FROM TN_80_APP_0040_MBOM_VER
 WHERE CN_ITEM_CODE = 'HGS7A768000P1'
  