CREATE TABLE [dbo].[tbl_Exam_Provider](
  [ID] [bigint] IDENTITY(1,1) NOT NULL,
  [ExamTypeID] [bigint] NULL,
  [Title] [nvarchar](max) NULL,
  [Website] [nvarchar](max) NULL,
  [Description] [nvarchar](max) NULL,
  [StatusFlag] [nvarchar](max) NULL,
  [CreateUser] [nvarchar](max) NULL,
  [UpdateUser] [nvarchar](max) NULL,
  [CreateDate] [datetime2](3) NULL,
  [UpdateDate] [datetime2](3) NULL)