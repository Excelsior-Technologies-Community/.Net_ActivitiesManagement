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
    SELECT ID, ActivityId, Title, Amount, ActionTypeList, StatusFlag, InAppShow, CreateDate, UpdateDate
    FROM tbl_Activities_mst
    ORDER BY ID DESC
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
    @CreateUser NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO tbl_Activities_detail_mst
        (ActivityId, Title, ActionTypeId, ActionTypeTitle, ActionIsMarkAsStatusVal,
         ActionIsMarkAsStatusText, ActionIsMarkAsStatusId, NewActionIsMarkAsStatusId,
         PageMaster, StatusFlag, CreateUser, CreateDate)
    VALUES
        (@ActivityId, @Title, @ActionTypeId, @ActionTypeTitle, @ActionIsMarkAsStatusVal,
         @ActionIsMarkAsStatusText, @ActionIsMarkAsStatusId, @NewActionIsMarkAsStatusId,
         @PageMaster, 'Active', @CreateUser, GETDATE())

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