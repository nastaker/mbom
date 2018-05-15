USE [MBOM_TJB]
GO
/****** Object:  StoredProcedure [dbo].[PROC_GET_ITEM_SETINFO]    Script Date: 05/15/2018 15:22:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PROC_GET_ITEM_SETINFO]
(
@CODE NCHAR(24)
)
AS
BEGIN

	WITH X AS 
	(
		SELECT 
		M.CN_ID AS PARENT_ID,
		ITEM.CN_ID AS ITEMID,
		M.CN_CODE AS PARENT_CODE,
		M.CN_NAME AS PARENT_NAME,
		ITEM.CN_ITEM_CODE AS ITEM_CODE,
		ITEM.CN_CODE AS CODE,
		ITEM.CN_NAME AS NAME,

		BOM.CN_UNIT AS UNIT,
		BOM.CN_ISBORROW AS ISBORROW,
		BOM.CN_B_IS_ASSEMBLY AS B_IS_ASSEMBLY, 
		BOM.CN_ORDER AS [ORDER]

		FROM 
		TN_80_APP_0025_BOM AS M
		INNER JOIN TN_80_APP_0025_BOM_HLINK AS BOM ON M.CN_ID = BOM.CN_BOM_ID AND BOM.CN_STATUS_PBOM = 'Y'
		INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON BOM.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
		WHERE 
		M.CN_CODE = @CODE

		UNION ALL
		
		SELECT  
		X.[ITEMID] AS PARENT_ID,
		ITEM.CN_ID AS CN_ITEMID,
		M.CN_CODE AS PARENT_CODE,
		M.CN_NAME AS PARENT_NAME,
		ITEM.CN_ITEM_CODE,
		ITEM.CN_CODE,
		ITEM.CN_NAME,

		BOM.CN_UNIT,
		BOM.CN_ISBORROW,
		BOM.CN_B_IS_ASSEMBLY,
		BOM.CN_ORDER
		FROM 
		TN_80_APP_0025_BOM AS M
		INNER JOIN TN_80_APP_0025_BOM_HLINK AS BOM ON M.CN_ID = BOM.CN_BOM_ID AND BOM.CN_STATUS_PBOM = 'Y'
		INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON BOM.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID 
		INNER JOIN X ON	M.CN_CODE = X.CODE AND X.B_IS_ASSEMBLY = 1
	)
	SELECT 
	ROW_NUMBER () OVER (ORDER BY X.ITEMID) AS ROWNUMBER,
	X.*, 
	ITEMHL.CN_F_QUANTITY as F_QUANTITY,
	ITEMHL.CN_SHIPPINGADDR as SHIPPINGADDR,
	CASE WHEN ITEMHL.CN_ID IS NULL THEN 0 ELSE 1 END AS SALESET 
	INTO #TMP
	FROM X
	LEFT JOIN [TN_80_APP_0000_ITEM_HLINK] AS ITEMHL ON X.ITEMID = ITEMHL.CN_ID 
	AND ITEMHL.CN_COMPONENT_CLASS_ID = 105 
	AND ITEMHL.CN_COMPONENT_OBJECT_ID = 1
	AND ITEMHL.CN_ISDELETE = 0
	OPTION (MAXRECURSION 32);

	SELECT MAX(ROWNUMBER) AS ROWNUMBER
	INTO #DEDUPLICATION
	FROM #TMP
	GROUP BY ITEMID;
	
	SELECT 
	*
	FROM #TMP AS T 
	WHERE ROWNUMBER IN 
	(SELECT ROWNUMBER FROM #DEDUPLICATION)
	ORDER BY T.[ORDER];	
END;
GO
