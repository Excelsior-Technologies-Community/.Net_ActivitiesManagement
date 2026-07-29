CREATE OR ALTER PROCEDURE USP_State_GetAll
AS
BEGIN
    SELECT s.Id, s.CountryId, c.CountryName, s.StateName, s.ShortCode, s.StatusFlag
    FROM tbl_State_mst s
    LEFT JOIN tbl_Country_mst c ON c.ID = s.CountryId
    ORDER BY s.Id ASC
END
GO

CREATE OR ALTER PROCEDURE USP_Satate_GetById
    @Id INT
AS
BEGIN
    SELECT Id, CountryId, StateName, ShortCode, StatusFlag
    FROM tbl_State_mst WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_State_GetActiveList
AS
BEGIN
    SELECT Id, StateName FROM tbl_State_mst WHERE StatusFlag = 'A' ORDER BY StateName
END
GO

CREATE OR ALTER PROCEDURE USP_State_GetByCountryId
    @CountryId INT
AS
BEGIN
    SELECT Id, StateName FROM tbl_State_mst WHERE CountryId = @CountryId AND StatusFlag = 'A' ORDER BY StateName
END
GO

CREATE OR ALTER PROCEDURE USP_State_Insert
       @CountryId INT,
       @StateName NVARCHAR(200),
       @ShortCode NVARCHAR(50),
       @CreateUser INT
AS
BEGIN
    INSERT INTO tbl_State_mst (CountryId, StateName, ShortCode, StatusFlag, CreateUser, CreateDate)
    VALUES (@CountryId, @StateName, @ShortCode, 'A', @CreateUser, GETDATE())

    SELECT SCOPE_IDENTITY() AS NewId
END
GO

CREATE OR ALTER PROCEDURE USP_State_Update
    @Id INT,
    @CountryId INT,
    @StateName NVARCHAR(200),
    @ShortCode NVARCHAR(50),
    @UpdateUser INT
AS
BEGIN
    UPDATE tbl_State_mst
    SET CountryId = @CountryId, StateName = @StateName, ShortCode = @ShortCode,
    UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_State_ChangeStatus
     @Id INT,
     @StatusFlag NVARCHAR(1),
     @UpdateUser INT
AS
BEGIN
   UPDATE tbl_State_mst
   SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
   WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE USP_State_Delete
    @Id INT
AS
BEGIN
   DELETE FROM tbl_State_mst WHERE Id = @Id
END
GO