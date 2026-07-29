CREATE OR ALTER PROCEDURE USP_State_GetById
    @Id INT
AS
BEGIN
    SELECT Id, CountryId, StateName, ShortCode, StatusFlag
    FROM tbl_State_mst WHERE Id = @Id
END
GO