using System.Data;
using System.Reflection;
using ActivitiesManagement.Models;
using Microsoft.Data.SqlClient;

namespace ActivitiesManagement.DataAccess
{
    public class ActivityDetailRepository
    {
        private readonly DbHelper _db;
        public ActivityDetailRepository(DbHelper db) { _db = db; }

        public List<ActivityDetailMaster> GetAll()
        {
            var list = new List<ActivityDetailMaster>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ActivityDetailMaster
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    ActivityId = dr["ActivityId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ActivityId"]),
                    ActivityTitle = dr["ActivityTitle"]?.ToString(),
                    Title = dr["Title"]?.ToString(),
                    ActionTypeId = dr["ActionTypeId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ActionTypeId"]),
                    ActionTypeTitle = dr["ActionTypeTitle"]?.ToString(),
                    ActionIsMarkAsStatusVal = dr["ActionIsMarkAsStatusVal"]?.ToString(),
                    ActionIsMarkAsStatusText = dr["ActionIsMarkAsStatusText"]?.ToString(),
                    PageMaster = dr["PageMaster"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString()
                });
            }
            return list;
        }

        public ActivityDetailMaster GetById(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new ActivityDetailMaster
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    ActivityId = dr["ActivityId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ActivityId"]),
                    Title = dr["Title"]?.ToString(),
                    ActionTypeId = dr["ActionTypeId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ActionTypeId"]),
                    ActionIsMarkAsStatusVal = dr["ActionIsMarkAsStatusVal"]?.ToString(),
                    ActionIsMarkAsStatusText = dr["ActionIsMarkAsStatusText"]?.ToString(),
                    ActionIsMarkAsStatusId = dr["ActionIsMarkAsStatusId"] as long?,
                    NewActionIsMarkAsStatusId = dr["NewActionIsMarkAsStatusId"]?.ToString(),
                    PageMaster = dr["PageMaster"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString()
                };
            }
            return null;
        }

        public List<ActivityDetailRow> GetByActivityId(long activityId)
        {
            var list = new List<ActivityDetailRow>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_GetByActivityId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ActivityId", activityId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ActivityDetailRow
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString(),
                    ActionTypeId = dr["ActionTypeId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ActionTypeId"]),
                    ActionTypeTitle = dr["ActionTypeTitle"]?.ToString(),
                    MasterName = dr["ActionIsMarkAsStatusVal"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString(),
                    IsInAppVisible = dr["InAppShow"]?.ToString() == "true"
                });
            }
            return list;
        }

        public long Insert(ActivityDetailMaster model, string createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_Insert", con) { CommandType = CommandType.StoredProcedure };
            AddCommonParams(cmd, model);
            cmd.Parameters.AddWithValue("@CreateUser", createUser ?? "");
            con.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        public void Update(ActivityDetailMaster model, string updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", model.ID);
            AddCommonParams(cmd, model);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteByActivityId(long activityId)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_DeleteByActivityId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ActivityId", activityId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void InsertRow(long activityId, ActivityDetailRow row, string createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ActivityId", activityId);
            cmd.Parameters.AddWithValue("@Title", row.Title ?? "");
            cmd.Parameters.AddWithValue("@ActionTypeId", row.ActionTypeId);
            cmd.Parameters.AddWithValue("@ActionTypeTitle", row.ActionTypeTitle ?? "");
            cmd.Parameters.AddWithValue("@ActionIsMarkAsStatusVal", row.MasterName ?? "");
            cmd.Parameters.AddWithValue("@ActionIsMarkAsStatusText", "");
            cmd.Parameters.AddWithValue("@ActionIsMarkAsStatusId", DBNull.Value);
            cmd.Parameters.AddWithValue("@NewActionIsMarkAsStatusId", "");
            cmd.Parameters.AddWithValue("@PageMaster", "");
            cmd.Parameters.AddWithValue("@StatusFlag", string.IsNullOrEmpty(row.StatusFlag) ? "Active" : row.StatusFlag);
            cmd.Parameters.AddWithValue("@InAppShow", row.IsInAppVisible ? "true" : "false");
            cmd.Parameters.AddWithValue("@CreateUser", createUser ?? "");
            con.Open();
            cmd.ExecuteNonQuery();
        }
        private void AddCommonParams(SqlCommand cmd, ActivityDetailMaster model)
        {
            cmd.Parameters.AddWithValue("@ActivityId", model.ActivityId);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@ActionTypeId", model.ActionTypeId);
            cmd.Parameters.AddWithValue("@ActionTypeTitle", model.ActionTypeTitle ?? "");
            cmd.Parameters.AddWithValue("@ActionIsMarkAsStatusVal", model.ActionIsMarkAsStatusVal ?? "");
            cmd.Parameters.AddWithValue("@ActionIsMarkAsStatusText", model.ActionIsMarkAsStatusText ?? "");
            cmd.Parameters.AddWithValue("@ActionIsMarkAsStatusId", (object)model.ActionIsMarkAsStatusId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NewActionIsMarkAsStatusId", model.NewActionIsMarkAsStatusId ?? "");
            cmd.Parameters.AddWithValue("@PageMaster", model.PageMaster ?? "");
        }

        public void ChangeStatus(long id, string statusFlag, string updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesDetail_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public List<StatusOption> GetStatusOptionsForMaster(string masterKey)
        {
            var data = new Dictionary<string, List<StatusOption>>
            {
                ["ApplicationStatus"] = new() { new() { Id = "1", Text = "Submitted" }, new() { Id = "2", Text = "Under Review" }, new() { Id = "3", Text = "Approved" }, new() { Id = "4", Text = "Rejected" } },
                ["ApplicationType"] = new() { new() { Id = "1", Text = "Undergraduate" }, new() { Id = "2", Text = "Postgraduate" }, new() { Id = "3", Text = "Diploma" } },
                ["Bank"] = new() { new() { Id = "1", Text = "HDFC Bank" }, new() { Id = "2", Text = "ICICI Bank" }, new() { Id = "3", Text = "SBI" } },
                ["FileStatus"] = new() { new() { Id = "1", Text = "Open" }, new() { Id = "2", Text = "Closed" }, new() { Id = "3", Text = "Pending" } },
                ["Decision"] = new() { new() { Id = "1", Text = "Offer Made" }, new() { Id = "2", Text = "Waitlisted" }, new() { Id = "3", Text = "Declined" } },
                ["QueryStatus"] = new() { new() { Id = "1", Text = "Open" }, new() { Id = "2", Text = "Resolved" } }
            };

            return data.TryGetValue(masterKey, out var list)
                ? list
                : new List<StatusOption> { new() { Id = "0", Text = "No data source configured yet" } };
        }
    }
}