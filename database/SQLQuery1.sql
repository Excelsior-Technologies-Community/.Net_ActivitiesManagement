-- Add this: delete all details for an activity (used on Update)
CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_DeleteByActivityId
    @ActivityId BIGINT
AS
BEGIN
    DELETE FROM tbl_Activities_detail_mst WHERE ActivityId = @ActivityId
END
GO

-- Replace your existing USP_ActivitiesMaster_GetAll with this
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

-- Get details for one activity (used when loading Edit page)
CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_GetByActivityId
    @ActivityId BIGINT
AS
BEGIN
    SELECT d.ID, d.ActivityId, d.Title, d.ActionTypeId, at.Title AS ActionTypeTitle,
           d.ActionIsMarkAsStatusVal, d.StatusFlag
    FROM tbl_Activities_detail_mst d
    LEFT JOIN tbl_Action_Type at ON at.ID = d.ActionTypeId
    WHERE d.ActivityId = @ActivityId
    ORDER BY d.ID
END
GO