CREATE FUNCTION FUNC_GETSTATUS
(
@TYPE VARCHAR(32),
@ORDER INT
)
RETURNS VARCHAR(32)
AS
BEGIN
	DECLARE @NAME VARCHAR(32)
	SELECT @NAME = CN_NAME FROM TN_50_DIC_0010_STATUS
	WHERE CN_TYPE = @TYPE AND CN_ORDER = @ORDER

	RETURN @NAME
END
GO