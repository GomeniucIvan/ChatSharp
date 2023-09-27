GO
CREATE PROCEDURE [dbo].[Setting_GetList]
AS
BEGIN
    SELECT s.Id,
		   s.Name,
		   s.Value

	FROM dbo.Setting s WITH(NOLOCK)
	ORDER BY s.Id
END
