CREATE OR ALTER PROCEDURE USP_Area_GetAll
AS
BEGIN
    SELECT a.Id, a.CountryId, c.CountryName, a.StateId, s.StateName,
           a.CityId, ci.CityName, a.Area, a.Pincode, a.StatusFlag
    FROM tbl_Area_mst a
    LEFT JOIN tbl_Country_mst c ON c.ID = a.CountryId
    LEFT JOIN tbl_State_mst s ON s.Id = a.StateId
    LEFT JOIN tbl_City_mst ci ON ci.Id = a.CityId
    ORDER BY a.Id ASC
END
GO

CREATE OR ALTER PROCEDURE USP_Area_GetById
    @Id INT
AS
BEGIN
    SELECT Id, CountryId, StateId, CityId, Area, Pincode, StatusFlag
    FROM tbl_Area_mst WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_Area_Insert
    @CountryId INT,
    @StateId INT,
    @CityId INT,
    @Area NVARCHAR(200),
    @Pincode NVARCHAR(20),
    @CreateUser INT
AS
BEGIN
    INSERT INTO tbl_Area_mst (CountryId, StateId, CityId, Area, Pincode, StatusFlag, CreateUser, CreateDate)
    VALUES (@CountryId, @StateId, @CityId, @Area, @Pincode, 'A', @CreateUser, GETDATE())

    SELECT SCOPE_IDENTITY() AS NewId
END
GO

CREATE OR ALTER PROCEDURE USP_Area_Update
    @Id INT,
    @CountryId INT,
    @StateId INT,
    @CityId INT,
    @Area NVARCHAR(200),
    @Pincode NVARCHAR(20),
    @UpdateUser INT
AS
BEGIN
    UPDATE tbl_Area_mst
    SET CountryId = @CountryId, StateId = @StateId, CityId = @CityId,
        Area = @Area, Pincode = @Pincode,
        UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_Area_ChangeStatus
    @Id INT,
    @StatusFlag NVARCHAR(1),
    @UpdateUser INT
AS
BEGIN
    UPDATE tbl_Area_mst
    SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_Area_Delete
    @Id INT
AS
BEGIN
    DELETE FROM tbl_Area_mst WHERE Id = @Id
END
GO
