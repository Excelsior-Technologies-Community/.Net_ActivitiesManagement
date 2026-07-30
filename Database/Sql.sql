CREATE OR ALTER PROCEDURE USP_City_GetAll
AS
BEGIN
    SELECT ci.Id, ci.CountryId, c.CountryName, ci.StateId, s.StateName,
           ci.CityName, ci.ShortCode, ci.StatusFlag
    FROM tbl_City_mst ci
    LEFT JOIN tbl_Country_mst c ON c.ID = ci.CountryId
    LEFT JOIN tbl_State_mst s ON s.Id = ci.StateId
    ORDER BY ci.Id ASC
END
GO

CREATE OR ALTER PROCEDURE USP_City_GetById
    @Id INT
AS
BEGIN
    SELECT Id, CountryId, StateId, CityName, ShortCode, StatusFlag
    FROM tbl_City_mst WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_City_GetActiveList
AS
BEGIN
    SELECT Id, CityName FROM tbl_City_mst WHERE StatusFlag = 'A' ORDER BY CityName
END
GO

CREATE OR ALTER PROCEDURE USP_City_GetByStateId
    @StateId INT
AS
BEGIN
    SELECT Id, CityName FROM tbl_City_mst WHERE StateId = @StateId AND StatusFlag = 'A' ORDER BY CityName
END
GO

CREATE OR ALTER PROCEDURE USP_City_Insert
    @CountryId INT,
    @StateId INT,
    @CityName NVARCHAR(200),
    @ShortCode NVARCHAR(50),
    @CreateUser INT
AS
BEGIN
    INSERT INTO tbl_City_mst (CountryId, StateId, CityName, ShortCode, StatusFlag, CreateUser, CreateDate)
    VALUES (@CountryId, @StateId, @CityName, @ShortCode, 'A', @CreateUser, GETDATE())

    SELECT SCOPE_IDENTITY() AS NewId
END
GO

CREATE OR ALTER PROCEDURE USP_City_Update
    @Id INT,
    @CountryId INT,
    @StateId INT,
    @CityName NVARCHAR(200),
    @ShortCode NVARCHAR(50),
    @UpdateUser INT
AS
BEGIN
    UPDATE tbl_City_mst
    SET CountryId = @CountryId, StateId = @StateId, CityName = @CityName, ShortCode = @ShortCode,
        UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_City_ChangeStatus
    @Id INT,
    @StatusFlag NVARCHAR(1),
    @UpdateUser INT
AS
BEGIN
    UPDATE tbl_City_mst
    SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_City_Delete
    @Id INT
AS
BEGIN
    DELETE FROM tbl_City_mst WHERE Id = @Id
END
GO