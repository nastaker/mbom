CREATE FUNCTION GETPRODUCTVER
(
@PROD_ITEMCODE VARCHAR(32)
)
RETURNS UNIQUEIDENTIFIER
AS
BEGIN
	DECLARE @DATE DATETIME = GETDATE()
	DECLARE @GUID_VER UNIQUEIDENTIFIER
	SELECT TOP 1 @GUID_VER = CN_GUID
	FROM TN_80_APP_0010_PRODUCT_VER
	WHERE
		CN_PRODUCT_ITEMCODE = @PROD_ITEMCODE 
		AND CN_STATUS = 'Y'
		ORDER BY CN_DT_CREATE DESC
	RETURN @GUID_VER
END