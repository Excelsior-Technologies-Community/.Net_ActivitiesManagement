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