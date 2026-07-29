CREATE OR ALTER PROCEDURE USP_Country_GetAll
AS
BEGIN
    SELECT ID, CountryName, ShortCode, IsIntrested, IsPastRejection, IsInquiry,
           CountryFlagImage, StatusFlag, CreateDate, UpdateDate
    FROM tbl_Country_mst
    ORDER BY ID ASC
END
GO

CREATE OR ALTER PROCEDURE USP_Country_GetById
    @ID BIGINT
AS
BEGIN
    SELECT ID, CountryName, ShortCode, IsIntrested, IsPastRejection, IsInquiry,
           CountryFlagImage, StatusFlag
    FROM tbl_Country_mst WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_Country_Insert
    @CountryName NVARCHAR(MAX),
    @ShortCode NVARCHAR(MAX),
    @IsIntrested BIT,
    @IsPastRejection BIT,
    @IsInquiry BIT,
    @CountryFlagImage NVARCHAR(MAX),
    @CreateUser BIGINT
AS
BEGIN
    INSERT INTO tbl_Country_mst
        (CountryName, ShortCode, IsIntrested, IsPastRejection, IsInquiry, CountryFlagImage, StatusFlag, CreateUser, CreateDate)
    VALUES
        (@CountryName, @ShortCode, @IsIntrested, @IsPastRejection, @IsInquiry, @CountryFlagImage, 'Active', @CreateUser, GETDATE())

    SELECT SCOPE_IDENTITY() AS NewId
END
GO

CREATE OR ALTER PROCEDURE USP_Country_Update
    @ID BIGINT,
    @CountryName NVARCHAR(MAX),
    @ShortCode NVARCHAR(MAX),
    @IsIntrested BIT,
    @IsPastRejection BIT,
    @IsInquiry BIT,
    @CountryFlagImage NVARCHAR(MAX),
    @UpdateUser BIGINT
AS
BEGIN
    UPDATE tbl_Country_mst
    SET CountryName = @CountryName, ShortCode = @ShortCode, IsIntrested = @IsIntrested,
        IsPastRejection = @IsPastRejection, IsInquiry = @IsInquiry,
        CountryFlagImage = CASE WHEN @CountryFlagImage IS NULL OR @CountryFlagImage = '' THEN CountryFlagImage ELSE @CountryFlagImage END,
        UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_Country_ChangeStatus
    @ID BIGINT,
    @StatusFlag NVARCHAR(20),
    @UpdateUser BIGINT
AS
BEGIN
    UPDATE tbl_Country_mst
    SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_Country_Delete
    @ID BIGINT
AS
BEGIN
    DELETE FROM tbl_Country_mst WHERE ID = @ID
END
GO