USE [MBOM_TJB]
GO
/****** Object:  UserDefinedFunction [dbo].[FUNC_GET_ITEM_TREE]    Script Date: 05/15/2018 15:22:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FUNC_GET_ITEM_TREE]
(
@CODE NCHAR(32)
)
RETURNS @TABLE TABLE( CN_CODE VARCHAR(32) )
AS
BEGIN 

DECLARE @PLINK VARCHAR(50)
DECLARE @PID VARCHAR(50)
declare @tmp table (
	[level] int,
	parent_link varchar(200),
	link varchar(200),
	parentid varchar(200),
	id varchar(200),
	cn_itemid int,
	cn_item_code varchar(200),
	cn_code varchar(200),
	cn_name varchar(200),
	cn_bom_id int,
	cn_hlink_id int,
	quantity float,
	quantity_all float,
	cn_unit varchar(200),
	cn_isborrow int,
	is_assembly int,
	cn_order int
	)

insert INTO @tmp
select
	0 AS [LEVEL],
	CAST(NULL AS VARCHAR) AS PARENT_LINK,
	CAST(ITEM.CN_ID AS VARCHAR) AS LINK,
	CAST(NULL AS VARCHAR) AS PARENTID,
	CAST(ITEM.CN_ID AS VARCHAR) AS ID,
	ITEM.CN_ID AS CN_ITEMID,
	ITEM.CN_ITEM_CODE,
	ITEM.CN_CODE,
	ITEM.CN_NAME,
	ISNULL(BOMHLINK.CN_BOM_ID, 0) AS CN_BOM_ID,
	ISNULL(BOMHLINK.CN_HLINK_ID, 0) AS CN_HLINK_ID, 
	ISNULL(BOMHLINK.CN_F_QUANTITY, 1) AS QUANTITY,
	ISNULL(BOMHLINK.CN_F_QUANTITY, 1) AS QUANTITY_ALL,
	ISNULL(BOMHLINK.CN_UNIT, '') AS CN_UNIT,
	ISNULL(BOMHLINK.CN_ISBORROW, 0) AS CN_ISBORROW,
	ISNULL(BOMHLINK.CN_B_IS_ASSEMBLY, 0) AS IS_ASSEMBLY,
	ISNULL(BOMHLINK.CN_ORDER, 0) AS CN_ORDER
FROM TN_80_APP_0000_ITEM AS ITEM
INNER JOIN TN_80_APP_0025_BOM AS BOM ON ITEM.CN_CODE = BOM.CN_CODE
LEFT JOIN TN_80_APP_0025_BOM_HLINK AS BOMHLINK ON ITEM.CN_ID = BOMHLINK.CN_COMPONENT_OBJECT_ID
WHERE ITEM.CN_CODE = @CODE

SELECT @PLINK = LINK, @PID = ID FROM @tmp;

WITH X AS 
(
	SELECT
	1 AS [LEVEL],
	CAST(@PLINK AS VARCHAR) AS PARENT_LINK,
	CAST(@PLINK + ',' + CAST(ITEM.CN_ID AS VARCHAR) AS VARCHAR) AS LINK,

	CAST(@PID AS VARCHAR) AS PARENTID,
	CAST(@PID + CAST(ITEM.CN_ID AS VARCHAR) AS VARCHAR) AS ID,

	ITEM.CN_ID AS CN_ITEMID,

	ITEM.CN_ITEM_CODE,
	ITEM.CN_CODE,
	ITEM.CN_NAME,
	
	BOMHLINK.CN_BOM_ID,
	BOMHLINK.CN_HLINK_ID, 
	BOMHLINK.CN_F_QUANTITY AS QUANTITY,
	BOMHLINK.CN_F_QUANTITY AS QUANTITY_ALL,
	BOMHLINK.CN_UNIT,
	BOMHLINK.CN_ISBORROW,
	BOMHLINK.CN_B_IS_ASSEMBLY AS IS_ASSEMBLY,
	BOMHLINK.CN_ORDER

	FROM 
	TN_80_APP_0025_BOM AS BOM
	INNER JOIN TN_80_APP_0025_BOM_HLINK AS BOMHLINK ON BOM.CN_ID = BOMHLINK.CN_BOM_ID AND BOMHLINK.CN_STATUS_PBOM = 'Y'
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON BOMHLINK.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
	WHERE 
	BOM.CN_CODE = @CODE

	UNION ALL

	SELECT 
	X.[LEVEL] + 1 AS [LEVEL],
	CAST(ISNULL(X.PARENT_LINK+',','')+ CAST(X.[CN_ITEMID] AS VARCHAR) AS VARCHAR),
	CAST(CAST(X.[LINK] AS VARCHAR) +',' + CAST(ITEM.CN_ID AS VARCHAR) AS VARCHAR),

	CAST(ISNULL(X.PARENTID,'') + CAST(X.CN_ITEMID AS VARCHAR) AS VARCHAR),
	CAST(CAST(X.ID AS VARCHAR) + CAST(ITEM.CN_ID AS VARCHAR) AS VARCHAR),

	ITEM.CN_ID AS CN_ITEMID,

	ITEM.CN_ITEM_CODE,
	ITEM.CN_CODE,
	ITEM.CN_NAME,

	BOMHLINK.CN_BOM_ID,
	BOMHLINK.CN_HLINK_ID, 
	BOMHLINK.CN_F_QUANTITY,
	BOMHLINK.CN_F_QUANTITY * X.QUANTITY_ALL,
	BOMHLINK.CN_UNIT,
	BOMHLINK.CN_ISBORROW,
	BOMHLINK.CN_B_IS_ASSEMBLY,
	BOMHLINK.CN_ORDER
	FROM 
	TN_80_APP_0025_BOM AS BOM
	INNER JOIN TN_80_APP_0025_BOM_HLINK AS BOMHLINK ON BOM.CN_ID = BOMHLINK.CN_BOM_ID AND BOMHLINK.CN_STATUS_PBOM = 'Y'
	INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON BOMHLINK.CN_COMPONENT_OBJECT_ID = ITEM.CN_ID
	INNER JOIN X ON	BOM.CN_CODE = X.CN_CODE AND X.IS_ASSEMBLY = 1
)
insert into @TABLE 
select cn_code from (
select *  
FROM 
@tmp
UNION 
SELECT *
FROM X
) as p
OPTION (MAXRECURSION 32);

RETURN

END;
GO
