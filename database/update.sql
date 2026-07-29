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