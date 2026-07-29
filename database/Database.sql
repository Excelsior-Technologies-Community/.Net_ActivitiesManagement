-- ==========================================
-- CREATE DATABASE
-- ==========================================
CREATE DATABASE ActivitiesManagementDB;
GO

USE ActivitiesManagementDB;
GO

-- ==========================================
-- TABLES
-- ==========================================
CREATE TABLE [dbo].[tbl_Action_Type](
    [ID] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] [nvarchar](max) NULL,
    [Description] [nvarchar](max) NULL,
    [StatusFlag] [nvarchar](max) NULL,
    [CreateUser] [bigint] NULL,
    [UpdateUser] [bigint] NULL,
    [CreateDate] [datetime] NULL,
    [UpdateDate] [datetime] NULL
);
GO

CREATE TABLE [dbo].[tbl_Activities_mst](
    [ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ActivityId] [bigint] NULL,
    [Title] [nvarchar](max) NULL,
    [Amount] [nvarchar](max) NULL,
    [ActionTypeList] [nvarchar](max) NULL,
    [StatusFlag] [nvarchar](max) NULL,
    [CreateUser] [bigint] NULL,
    [UpdateUser] [bigint] NULL,
    [CreateDate] [datetime2](3) NULL,
    [UpdateDate] [datetime2](3) NULL,
    [InAppShow] [nvarchar](max) NULL
);
GO

CREATE TABLE [dbo].[tbl_Activities_detail_mst](
    [ID] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ActivityId] [bigint] NULL,
    [Title] [nvarchar](max) NULL,
    [ActionTypeId] [bigint] NULL,
    [ActionIsMarkAsStatusVal] [nvarchar](max) NULL,
    [ActionIsMarkAsStatusText] [nvarchar](max) NULL,
    [NewActionIsMarkAsStatusId] [nvarchar](max) NULL,
    [ActionIsMarkAsStatusId] [bigint] NULL,
    [ActionType] [nvarchar](max) NULL,
    [PageMasterId] [bigint] NULL,
    [PageMaster] [nvarchar](max) NULL,
    [StatusFlag] [nvarchar](max) NULL,
    [CreateUser] [nvarchar](max) NULL,
    [UpdateUser] [nvarchar](max) NULL,
    [CreateDate] [datetime2](3) NULL,
    [UpdateDate] [datetime2](3) NULL,
    [ActionTypeTitle] [nvarchar](max) NULL,
    [ActionMasterTitle] [nvarchar](max) NULL,
    [CollectionName] [nvarchar](max) NULL,
    [ModelName] [nvarchar](max) NULL,
    [ViewBagName] [nvarchar](max) NULL,
    [AjaxURL] [nvarchar](max) NULL,
    [ControlName] [nvarchar](max) NULL,
    [InAppShow] [nvarchar](max) NULL
);
GO

-- ==========================================
-- ACTION TYPE PROCEDURES
-- ==========================================
CREATE OR ALTER PROCEDURE USP_ActionType_GetAll
AS
BEGIN
    SELECT ID, Title, Description, StatusFlag, CreateDate, UpdateDate
    FROM tbl_Action_Type
    ORDER BY ID DESC
END
GO

CREATE OR ALTER PROCEDURE USP_ActionType_GetActiveList
AS
BEGIN
    SELECT ID, Title FROM tbl_Action_Type WHERE StatusFlag = 'Active' ORDER BY Title
END
GO

CREATE OR ALTER PROCEDURE USP_ActionType_GetById
    @ID BIGINT
AS
BEGIN
    SELECT ID, Title, Description, StatusFlag FROM tbl_Action_Type WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActionType_Insert
    @Title NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @CreateUser BIGINT
AS
BEGIN
    INSERT INTO tbl_Action_Type (Title, Description, StatusFlag, CreateUser, CreateDate)
    VALUES (@Title, @Description, 'Active', @CreateUser, GETDATE())

    SELECT SCOPE_IDENTITY() AS NewId
END
GO

CREATE OR ALTER PROCEDURE USP_ActionType_Update
    @ID BIGINT,
    @Title NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @UpdateUser BIGINT
AS
BEGIN
    UPDATE tbl_Action_Type
    SET Title = @Title, Description = @Description, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActionType_ChangeStatus
    @ID BIGINT,
    @StatusFlag NVARCHAR(20),
    @UpdateUser BIGINT
AS
BEGIN
    UPDATE tbl_Action_Type
    SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActionType_Delete
    @ID BIGINT
AS
BEGIN
    DELETE FROM tbl_Action_Type WHERE ID = @ID
END
GO

-- ==========================================
-- ACTIVITIES MASTER PROCEDURES
-- ==========================================
CREATE OR ALTER PROCEDURE USP_ActivitiesMaster_GetAll
AS
BEGIN
    SELECT m.ID, m.Title, m.Amount, m.StatusFlag, m.InAppShow,
           ISNULL(STRING_AGG(d.ActionIsMarkAsStatusVal, ', '), '') AS ActionListDisplay
    FROM tbl_Activities_mst m
    LEFT JOIN tbl_Activities_detail_mst d ON d.ActivityId = m.ID
    GROUP BY m.ID, m.Title, m.Amount, m.StatusFlag, m.InAppShow
    ORDER BY m.ID DESC
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesMaster_GetActiveList
AS
BEGIN
    SELECT ID, Title FROM tbl_Activities_mst WHERE StatusFlag = 'Active' ORDER BY Title
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesMaster_GetById
    @ID BIGINT
AS
BEGIN
    SELECT ID, ActivityId, Title, Amount, ActionTypeList, StatusFlag, InAppShow
    FROM tbl_Activities_mst WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesMaster_Insert
    @Title NVARCHAR(MAX),
    @Amount NVARCHAR(MAX),
    @ActionTypeList NVARCHAR(MAX),
    @InAppShow NVARCHAR(MAX),
    @CreateUser BIGINT
AS
BEGIN
    INSERT INTO tbl_Activities_mst
        (Title, Amount, ActionTypeList, StatusFlag, InAppShow, CreateUser, CreateDate)
    VALUES
        (@Title, @Amount, @ActionTypeList, 'Active', @InAppShow, @CreateUser, GETDATE())

    SELECT SCOPE_IDENTITY() AS NewId
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesMaster_Update
    @ID BIGINT,
    @Title NVARCHAR(MAX),
    @Amount NVARCHAR(MAX),
    @ActionTypeList NVARCHAR(MAX),
    @InAppShow NVARCHAR(MAX),
    @UpdateUser BIGINT
AS
BEGIN
    UPDATE tbl_Activities_mst
    SET Title = @Title, Amount = @Amount, ActionTypeList = @ActionTypeList,
        InAppShow = @InAppShow, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesMaster_ChangeStatus
    @ID BIGINT,
    @StatusFlag NVARCHAR(20),
    @UpdateUser BIGINT
AS
BEGIN
    UPDATE tbl_Activities_mst
    SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesMaster_Delete
    @ID BIGINT
AS
BEGIN
    DELETE FROM tbl_Activities_mst WHERE ID = @ID
END
GO

-- ==========================================
-- ACTIVITIES DETAIL MASTER PROCEDURES
-- ==========================================
CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_GetAll
AS
BEGIN
    SELECT d.ID, d.ActivityId, m.Title AS ActivityTitle, d.Title, d.ActionTypeId,
           at.Title AS ActionTypeTitle, d.ActionIsMarkAsStatusVal, d.ActionIsMarkAsStatusText,
           d.NewActionIsMarkAsStatusId, d.PageMaster, d.StatusFlag, d.CreateDate, d.UpdateDate
    FROM tbl_Activities_detail_mst d
    LEFT JOIN tbl_Activities_mst m ON m.ID = d.ActivityId
    LEFT JOIN tbl_Action_Type at ON at.ID = d.ActionTypeId
    ORDER BY d.ID DESC
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_GetById
    @ID BIGINT
AS
BEGIN
    SELECT ID, ActivityId, Title, ActionTypeId, ActionIsMarkAsStatusVal, ActionIsMarkAsStatusText,
           NewActionIsMarkAsStatusId, ActionIsMarkAsStatusId, PageMasterId, PageMaster,
           CollectionName, ModelName, ViewBagName, AjaxURL, ControlName, StatusFlag, InAppShow
    FROM tbl_Activities_detail_mst WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_GetByActivityId
    @ActivityId BIGINT
AS
BEGIN
    SELECT d.ID, d.ActivityId, d.Title, d.ActionTypeId, at.Title AS ActionTypeTitle,
           d.ActionIsMarkAsStatusVal, d.StatusFlag, d.InAppShow
    FROM tbl_Activities_detail_mst d
    LEFT JOIN tbl_Action_Type at ON at.ID = d.ActionTypeId
    WHERE d.ActivityId = @ActivityId
    ORDER BY d.ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_Insert
    @ActivityId BIGINT,
    @Title NVARCHAR(MAX),
    @ActionTypeId BIGINT,
    @ActionTypeTitle NVARCHAR(MAX),
    @ActionIsMarkAsStatusVal NVARCHAR(MAX),
    @ActionIsMarkAsStatusText NVARCHAR(MAX),
    @ActionIsMarkAsStatusId BIGINT,
    @NewActionIsMarkAsStatusId NVARCHAR(MAX),
    @PageMaster NVARCHAR(MAX),
    @StatusFlag NVARCHAR(20),
    @InAppShow NVARCHAR(MAX),
    @CreateUser NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO tbl_Activities_detail_mst
        (ActivityId, Title, ActionTypeId, ActionTypeTitle, ActionIsMarkAsStatusVal,
         ActionIsMarkAsStatusText, ActionIsMarkAsStatusId, NewActionIsMarkAsStatusId,
         PageMaster, StatusFlag, InAppShow, CreateUser, CreateDate)
    VALUES
        (@ActivityId, @Title, @ActionTypeId, @ActionTypeTitle, @ActionIsMarkAsStatusVal,
         @ActionIsMarkAsStatusText, @ActionIsMarkAsStatusId, @NewActionIsMarkAsStatusId,
         @PageMaster, @StatusFlag, @InAppShow, @CreateUser, GETDATE())

    SELECT SCOPE_IDENTITY() AS NewId
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_Update
    @ID BIGINT,
    @ActivityId BIGINT,
    @Title NVARCHAR(MAX),
    @ActionTypeId BIGINT,
    @ActionTypeTitle NVARCHAR(MAX),
    @ActionIsMarkAsStatusVal NVARCHAR(MAX),
    @ActionIsMarkAsStatusText NVARCHAR(MAX),
    @ActionIsMarkAsStatusId BIGINT,
    @NewActionIsMarkAsStatusId NVARCHAR(MAX),
    @PageMaster NVARCHAR(MAX),
    @UpdateUser NVARCHAR(MAX)
AS
BEGIN
    UPDATE tbl_Activities_detail_mst
    SET ActivityId = @ActivityId, Title = @Title, ActionTypeId = @ActionTypeId,
        ActionTypeTitle = @ActionTypeTitle, ActionIsMarkAsStatusVal = @ActionIsMarkAsStatusVal,
        ActionIsMarkAsStatusText = @ActionIsMarkAsStatusText, ActionIsMarkAsStatusId = @ActionIsMarkAsStatusId,
        NewActionIsMarkAsStatusId = @NewActionIsMarkAsStatusId, PageMaster = @PageMaster,
        UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_ChangeStatus
    @ID BIGINT,
    @StatusFlag NVARCHAR(20),
    @UpdateUser NVARCHAR(MAX)
AS
BEGIN
    UPDATE tbl_Activities_detail_mst
    SET StatusFlag = @StatusFlag, UpdateUser = @UpdateUser, UpdateDate = GETDATE()
    WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_Delete
    @ID BIGINT
AS
BEGIN
    DELETE FROM tbl_Activities_detail_mst WHERE ID = @ID
END
GO

CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_DeleteByActivityId
    @ActivityId BIGINT
AS
BEGIN
    DELETE FROM tbl_Activities_detail_mst WHERE ActivityId = @ActivityId
END
GO