CREATE OR ALTER PROCEDURE USP_ActivitiesDetail_GetAll
AS
BEGIN
    SELECT d.ID, d.ActivityId, m.Title AS ActivityTitle, d.Title, d.ActionTypeId,
           at.Title AS ActionTypeTitle, d.ActionIsMarkAsStatusVal, d.ActionIsMarkAsStatusText,
           d.NewActionIsMarkAsStatusId, d.PageMaster, d.StatusFlag, d.CreateDate, d.UpdateDate
    FROM tbl_Activities_detail_mst d
    LEFT JOIN tbl_Activities_mst m ON m.ID = d.ActivityId
    LEFT JOIN tbl_Action_Type at ON at.ID = d.ActionTypeId
    ORDER BY d.ID ASC
END
GO