USE [MBOM_TJB]
GO
/****** Object:  StoredProcedure [dbo].[PROC_GET_PROCESS_ITEM]    Script Date: 05/15/2018 15:22:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PROC_GET_PROCESS_ITEM]
(
@CODE NCHAR(30)
)
AS
BEGIN
WITH X AS 
(
	SELECT
	ITEM.CN_ID AS CN_ITEMID,
	ITEM.CN_CODE,

	BOM.CN_B_IS_ASSEMBLY
	FROM 
	TN_80_APP_0025_BOM AS M
	INNER JOIN TN_80_APP_0025_BOM_HLINK AS BOM ON M.CN_ID = BOM.CN_BOM_ID AND BOM.CN_STATUS_PBOM = 'Y'
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON BOM.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
	WHERE 
	M.CN_CODE = @CODE

	UNION ALL

	SELECT
	ITEM.CN_ID AS CN_ITEMID,
	ITEM.CN_CODE,

	BOM.CN_B_IS_ASSEMBLY
	FROM 
	TN_80_APP_0025_BOM AS M
	INNER JOIN TN_80_APP_0025_BOM_HLINK AS BOM ON M.CN_ID = BOM.CN_BOM_ID AND BOM.CN_STATUS_PBOM = 'Y'
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON BOM.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
	INNER JOIN X ON M.CN_CODE = X.CN_CODE AND X.CN_B_IS_ASSEMBLY = 1
)
SELECT 
T.*,
P.CN_NAME
FROM 
(
SELECT DISTINCT
X.CN_ITEMID,
X.CN_CODE
FROM X
LEFT JOIN TN_80_APP_0000_ITEM_HLINK AS ITEMHL 
ON X.CN_ITEMID = ITEMHL.CN_ID
AND ITEMHL.CN_COMPONENT_CLASS_ID = 105
AND ITEMHL.CN_ISDELETE = 0
LEFT JOIN [TN_50_DIC_0005_ITEMTYPE] AS ITEMTYPE ON ITEMHL.CN_COMPONENT_OBJECT_ID = ITEMTYPE.CN_ID
) AS T
INNER JOIN TN_80_APP_0020_PROCESS AS P ON T.CN_CODE = P.CN_CODE

END;
GO
