ALTER PROCEDURE USP_ActivitiesMaster_GetAll
AS
BEGIN
    SELECT
        m.ID,
        m.Title,
        m.Amount,
        m.StatusFlag,
        m.InAppShow,
        ISNULL(STRING_AGG(d.Title, ', '), '') AS ActionListDisplay
    FROM tbl_Activities_mst m
    LEFT JOIN tbl_Activities_detail_mst d
        ON d.ActivityId = m.ID
    GROUP BY
        m.ID,
        m.Title,
        m.Amount,
        m.StatusFlag,
        m.InAppShow
    ORDER BY
        m.ID ASC;
END
