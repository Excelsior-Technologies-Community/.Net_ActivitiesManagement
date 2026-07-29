using ActivitiesManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ActivitiesManagement.DataAccess
{
    public class StateRepository
    {
        private readonly DbHelper _db;
        public StateRepository(DbHelper db) { _db = db; }

        public List<State> GetAll()
        {
            var list = new List<State>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_State_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new State
                {
                    ID = Convert.ToInt32(dr["Id"]),
                    CountryId = Convert.ToInt32(dr["CountryId"]),
                    CountryName = dr["CountryName"]?.ToString(),
                    StateName = dr["StateName"]?.ToString(),
                    ShortCode = dr["ShortCode"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                });
            }
            return list;
        }

        public State GetById(int id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_State_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new State
                {
                    ID = Convert.ToInt32(dr["Id"]),
                    CountryId = Convert.ToInt32(dr["CountryId"]),
                    StateName = dr["StateName"]?.ToString(),
                    ShortCode = dr["ShortCode"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                };
            }
            return null;
        }

        public List<State> GetByCountryId(int countryId)
        {
            var list = new List<State>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_State_GetByCountryId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CountryId", countryId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new State
                {
                    ID = Convert.ToInt32(dr["Id"]),
                    StateName = dr["StateName"]?.ToString()
                });
            }
            return list;
        }


        public int Insert(State model, int createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_State_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateName", model.StateName ?? "");
            cmd.Parameters.AddWithValue("@ShortCode", model.ShortCode ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);
            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(State model, int updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_State_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", model.ID);
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateName", model.StateName ?? "");
            cmd.Parameters.AddWithValue("@ShortCode", model.ShortCode ?? "");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(int id, string statusFlag, int updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_State_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_State_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        
    }
}