using ActivitiesManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ActivitiesManagement.DataAccess
{
    public class ActivityMasterRepository
    {
        private readonly DbHelper _db;
        public ActivityMasterRepository(DbHelper db) { _db = db; }

        public List<ActivityMaster> GetAll()
        {
            var list = new List<ActivityMaster>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesMaster_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(MapForList(dr));
            }
            return list;
        }

        public ActivityMaster GetById(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesMaster_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                var model = new ActivityMaster
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString(),
                    Amount = dr["Amount"]?.ToString(),
                    ActionTypeList = dr["ActionTypeList"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString(),
                    InAppShow = dr["InAppShow"]?.ToString()
                };
                model.SelectedActionTypeIds = (model.ActionTypeList ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt64(x.Trim()))
                    .ToList();
                return model;
            }
            return null;
        }

        private ActivityMaster MapForList(SqlDataReader dr)
        {
            return new ActivityMaster
            {
                Id = Convert.ToInt64(dr["ID"]),
                Title = dr["Title"]?.ToString(),
                Amount = dr["Amount"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString(),
                InAppShow = dr["InAppShow"]?.ToString(),
                ActionListDisplay = dr["ActionListDisplay"]?.ToString()
            };
        }


        public long Insert(ActivityMaster model, long createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesMaster_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Amount", model.Amount ?? "");
            cmd.Parameters.AddWithValue("@ActionTypeList", string.Join(",", model.SelectedActionTypeIds));
            cmd.Parameters.AddWithValue("@InAppShow", model.InAppShow ?? "false");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);
            con.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        public void Update(ActivityMaster model, long updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesMaster_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", model.Id);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Amount", model.Amount ?? "");
            cmd.Parameters.AddWithValue("@ActionTypeList", string.Join(",", model.SelectedActionTypeIds));
            cmd.Parameters.AddWithValue("@InAppShow", model.InAppShow ?? "false");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesMaster_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActivitiesMaster_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}