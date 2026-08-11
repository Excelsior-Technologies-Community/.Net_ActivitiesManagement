-- =============================================
-- Insert Exam Provider
-- =============================================
CREATE PROCEDURE usp_ExamProvider_Insert
    @ExamTypeID BIGINT,
    @Title NVARCHAR(MAX),
    @Website NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @CreateUser NVARCHAR(MAX),
    @NewId BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_Exam_Provider
        (ExamTypeID, Title, Website, Description, StatusFlag, CreateUser, CreateDate)
    VALUES
        (@ExamTypeID, @Title, @Website, @Description, 'A', @CreateUser, GETDATE());

    SET @NewId = SCOPE_IDENTITY();
END
GO

-- =============================================
-- Update Exam Provider
-- =============================================
CREATE PROCEDURE usp_ExamProvider_Update
    @Id BIGINT,
    @ExamTypeID BIGINT,
    @Title NVARCHAR(MAX),
    @Website NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @UpdateUser NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_Exam_Provider SET
        ExamTypeID = @ExamTypeID,
        Title = @Title,
        Website = @Website,
        Description = @Description,
        UpdateUser = @UpdateUser,
        UpdateDate = GETDATE()
    WHERE ID = @Id;
END
GO

-- =============================================
-- Get All Exam Providers (joined with Exam Type title)
-- =============================================
CREATE PROCEDURE usp_ExamProvider_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        EP.ID,
        EP.ExamTypeID,
        ET.Title AS ExamTypeTitle,
        EP.Title,
        EP.Website,
        EP.Description,
        EP.StatusFlag
    FROM tbl_Exam_Provider EP
    LEFT JOIN tbl_Exam_Type ET ON ET.ID = EP.ExamTypeID
    ORDER BY EP.ID ASC;
END
GO

-- =============================================
-- Get Exam Provider By Id
-- =============================================
CREATE PROCEDURE usp_ExamProvider_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        EP.ID,
        EP.ExamTypeID,
        ET.Title AS ExamTypeTitle,
        EP.Title,
        EP.Website,
        EP.Description,
        EP.StatusFlag
    FROM tbl_Exam_Provider EP
    LEFT JOIN tbl_Exam_Type ET ON ET.ID = EP.ExamTypeID
    WHERE EP.ID = @Id;
END
GO

-- =============================================
-- Change Status
-- =============================================
CREATE PROCEDURE usp_ExamProvider_ChangeStatus
    @Id BIGINT,
    @StatusFlag NVARCHAR(10),
    @UpdateUser NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_Exam_Provider
    SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @Id;
END
GO

-- =============================================
-- Delete Exam Provider
-- =============================================
CREATE PROCEDURE usp_ExamProvider_Delete
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tbl_Exam_Provider WHERE ID = @Id;
END
GO

-- =============================================
-- Dropdown: Get all active Exam Types (for the Add/Edit form)
-- =============================================
CREATE PROCEDURE usp_ExamType_GetAllActive
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, Title
    FROM tbl_Exam_Type
    WHERE StatusFlag = 'A'
    ORDER BY Title ASC;
END
GO