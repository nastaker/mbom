ALTER VIEW [dbo].[VIEW_MATERIALBILLBOARDS]
AS
SELECT
	 ITEM.CN_CODE AS Code
	,ITEM.CN_ITEM_CODE AS ItemCode
	,ITEM.CN_NAME AS ItemName
	,ITEM.CN_IS_TOERP AS ErpStatus
	,ITEM.CN_DT_TOERP AS ErpDate
	,CAST(CASE WHEN SELL.CN_COMPONENT_OBJECT_ID IS NULL THEN 0 ELSE 1 END AS BIT) AS Sell
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 2 THEN 1 ELSE 0 END) AS BIT) AS Purchase
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 3 THEN 1 ELSE 0 END) AS BIT) AS SelfMade
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 4 THEN 1 ELSE 0 END) AS BIT) AS [Standard]
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 5 THEN 1 ELSE 0 END) AS BIT) AS RawMaterial
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 6 THEN 1 ELSE 0 END) AS BIT) AS Package
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 7 THEN 1 ELSE 0 END) AS BIT) AS Process
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 10 THEN 1 ELSE 0 END) AS BIT) AS DEOptional
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 11 THEN 1 ELSE 0 END) AS BIT) AS PBOMOptional
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 14 THEN 1 ELSE 0 END) AS BIT) AS MBOMOptional
	,CAST(MAX(CASE ITEMTYPE.CN_ID WHEN 15 THEN 1 ELSE 0 END) AS BIT) AS [Assembly]
FROM TN_80_APP_0000_ITEM AS ITEM
LEFT JOIN TN_80_APP_0000_ITEM_HLINK AS ITEMHLINK ON ITEM.CN_ID = ITEMHLINK.CN_ID AND ITEMHLINK.CN_COMPONENT_CLASS_ID = 105 AND ITEMHLINK.[CN_SYS_STATUS] <> 'N'
LEFT JOIN TN_50_DIC_0005_ITEMTYPE AS ITEMTYPE ON ITEMTYPE.CN_ID = ITEMHLINK.CN_COMPONENT_OBJECT_ID
LEFT JOIN TN_80_APP_0010_PRODUCT_HLINK AS SELL ON SELL.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
GROUP BY
	 ITEM.CN_CODE
	,ITEM.CN_ITEM_CODE
	,ITEM.CN_NAME
	,ITEM.CN_IS_TOERP
	,ITEM.CN_DT_TOERP
	,SELL.CN_COMPONENT_OBJECT_ID
GO