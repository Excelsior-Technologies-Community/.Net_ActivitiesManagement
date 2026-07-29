using ActivitiesManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ActivitiesManagement.DataAccess
{
    public class ActionTypeRepository
    {
        private readonly DbHelper _db;
        public ActionTypeRepository(DbHelper db) { _db = db; }

        public List<ActionType> GetAll()
        {
            var list = new List<ActionType>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActionType_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ActionType
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString(),
                    Description = dr["Description"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString(),
                    CreateDate = dr["CreateDate"] as DateTime?,
                    UpdateDate = dr["UpdateDate"] as DateTime?
                });
            }
            return list;
        }

        public List<ActionType> GetActiveList()
        {
            var list = new List<ActionType>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActionType_GetActiveList", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ActionType
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString()
                });
            }
            return list;
        }

        public ActionType GetById(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActionType_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new ActionType
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString(),
                    Description = dr["Description"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString()
                };
            }
            return null;
        }

        public long Insert(ActionType model, long createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActionType_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);
            con.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        public void Update(ActionType model, long updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActionType_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", model.ID);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActionType_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_ActionType_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}