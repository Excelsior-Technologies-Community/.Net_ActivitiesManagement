CREATE TABLE [dbo].[tbl_Exam_Type](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ExamTypeId] [bigint] NULL,
	[Title] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[EType] [nvarchar](max) NULL,
	[IsLead] [nchar](1) NULL,
	[IsInquiry] [nchar](1) NULL,
	[IsRegistration] [nchar](1) NULL,
	[IsCoaching] [nchar](1) NULL,
	[IsProcess] [nchar](1) NULL,
	[IsMock] [nchar](1) NULL,
	[IsProfessional] [nchar](1) NULL,
	[IsEnglishTest] [nchar](1) NULL,
	[StatusFlag] [nvarchar](max) NULL,
	[CreateUser] [nvarchar](max) NULL,
	[UpdateUser] [nvarchar](max) NULL,
	[CreateDate] [datetime2](3) NULL,
	[UpdateDate] [datetime2](3) NULL
);
GO
 
CREATE TABLE [dbo].[tbl_Exam_Type_Detail](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[ExamTypeId] [bigint] NULL,
	[StatusFlag] [nvarchar](max) NULL,
	[CreateUser] [bigint] NULL,
	[UpdateUser] [bigint] NULL,
	[CreateDate] [datetime2](3) NULL,
	[UpdateDate] [datetime2](3) NULL
);
GO

CREATE PROCEDURE usp_ExamType_Insert
    @Title NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @IsLead NCHAR(1),
    @IsInquiry NCHAR(1),
    @IsRegistration NCHAR(1),
    @IsCoaching NCHAR(1),
    @IsProcess NCHAR(1),
    @IsMock NCHAR(1),
    @IsProfessional NCHAR(1),
    @IsEnglishTest NCHAR(1),
    @CreateUser NVARCHAR(MAX),
    @NewId BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_Exam_Type
        (Title, Description, IsLead, IsInquiry, IsRegistration, IsCoaching,
         IsProcess, IsMock, IsProfessional, IsEnglishTest, StatusFlag, CreateUser, CreateDate)
    VALUES
        (@Title, @Description, @IsLead, @IsInquiry, @IsRegistration, @IsCoaching,
         @IsProcess, @IsMock, @IsProfessional, @IsEnglishTest, 'A', @CreateUser, GETDATE());

    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE usp_ExamType_Update
    @Id BIGINT,
    @Title NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @IsLead NCHAR(1),
    @IsInquiry NCHAR(1),
    @IsRegistration NCHAR(1),
    @IsCoaching NCHAR(1),
    @IsProcess NCHAR(1),
    @IsMock NCHAR(1),
    @IsProfessional NCHAR(1),
    @IsEnglishTest NCHAR(1),
    @UpdateUser NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_Exam_Type SET
        Title = @Title, Description = @Description, IsLead = @IsLead, IsInquiry = @IsInquiry,
        IsRegistration = @IsRegistration, IsCoaching = @IsCoaching, IsProcess = @IsProcess,
        IsMock = @IsMock, IsProfessional = @IsProfessional, IsEnglishTest = @IsEnglishTest,
        UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @Id;
END
GO

CREATE PROCEDURE usp_ExamType_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, Title, Description, IsLead, IsInquiry, IsRegistration, IsCoaching,
           IsProcess, IsMock, IsProfessional, IsEnglishTest, StatusFlag
    FROM tbl_Exam_Type
    ORDER BY ID ASC;
END
GO

CREATE PROCEDURE usp_ExamType_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, Title, Description, IsLead, IsInquiry, IsRegistration, IsCoaching,
           IsProcess, IsMock, IsProfessional, IsEnglishTest, StatusFlag
    FROM tbl_Exam_Type
    WHERE ID = @Id;
END
GO

CREATE PROCEDURE usp_ExamType_ChangeStatus
    @Id BIGINT,
    @StatusFlag NVARCHAR(10),
    @UpdateUser NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_Exam_Type
    SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @Id;
END
GO

CREATE PROCEDURE usp_ExamType_Delete
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tbl_Exam_Type_Detail WHERE ExamTypeId = @Id;
    DELETE FROM tbl_Exam_Type WHERE ID = @Id;
END
GO

CREATE PROCEDURE usp_ExamTypeDetail_Insert
    @ExamTypeId BIGINT,
    @Title NVARCHAR(MAX),
    @CreateUser BIGINT = NULL,
    @NewId BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_Exam_Type_Detail (Title, ExamTypeId, StatusFlag, CreateUser, CreateDate)
    VALUES (@Title, @ExamTypeId, 'A', @CreateUser, GETDATE());

    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE usp_ExamTypeDetail_GetByExamTypeId
    @ExamTypeId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, Title, ExamTypeId, StatusFlag
    FROM tbl_Exam_Type_Detail
    WHERE ExamTypeId = @ExamTypeId
      AND (StatusFlag IS NULL OR StatusFlag <> 'D')
    ORDER BY ID ASC;
END
GO

CREATE PROCEDURE usp_ExamTypeDetail_Delete
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tbl_Exam_Type_Detail WHERE ID = @Id;
END
GO



