GO
CREATE PROCEDURE [dbo].[Setting_GetList]
AS
BEGIN
    SELECT s.Id     AS Id,
		   s.Name   AS Name,
		   s.Value AS Value

	FROM dbo.Setting s WITH(NOLOCK)
	ORDER BY s.Id
END
GO

CREATE PROCEDURE [dbo].[Session_GetList]
    (
    @Guid nvarchar(36) = NULL, 
	@ModelName nvarchar(500) = NULL)
AS
BEGIN
    SELECT s.Id         AS Id,
		   s.Name       AS Name,
		   s.ModelName  AS ModelName,
		   s.Guid       AS Guid

	FROM dbo.Session s WITH(NOLOCK)
	WHERE (ISNULL(@Guid,'') = '' OR @Guid = s.Guid) AND
		  (ISNULL(@ModelName,'') = '' OR @ModelName = s.ModelName)
	ORDER BY s.Id
END
GO

ALTER PROCEDURE [dbo].[Session_Save]
     (
    @Guid uniqueidentifier,
	@Name nvarchar(200), 
	@AutoDeleteAfterXDays int,
	@ModelName nvarchar(500))
AS
BEGIN
    DECLARE @sessionId int = (SELECT TOP 1 s.Id FROM dbo.Session s WITH(NOLOCK)WHERE s.Guid = @Guid);

    IF ISNULL(@sessionId, 0) = 0
    BEGIN
        INSERT INTO dbo.Session
        (   Guid,
            Name,
            CreatedOnUtc,
            AutoDeleteAfterXDays,
            ModelName)
        VALUES
             (@Guid,                 -- Guid - uniqueidentifier
              @Name,                 -- Name - nvarchar(200)
              GETUTCDATE(),          -- CreatedOnUtc - datetime
              @AutoDeleteAfterXDays, -- AutoDeleteAfterXDays - int
              @ModelName             -- ModelName - nvarchar(500)
            );

        SET @sessionId = SCOPE_IDENTITY();
    END;

	SELECT @sessionId;
END
GO