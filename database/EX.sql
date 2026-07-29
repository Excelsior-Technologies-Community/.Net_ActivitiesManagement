CREATE OR ALTER PROCEDURE USP_Country_GetActiveList
AS
BEGIN
    SELECT ID, CountryName FROM tbl_Country_mst WHERE StatusFlag = 'Active' ORDER BY CountryName
END
GO