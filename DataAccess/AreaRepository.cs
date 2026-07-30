using ActivitiesManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ActivitiesManagement.DataAccess
{
    public class AreaRepository
    {
        private readonly DbHelper _db;
        public AreaRepository(DbHelper db) { _db = db; }

        public List<Area> GetAll()
        {
            var list = new List<Area>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Area_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Area
                {
                    Id = Convert.ToInt32(dr["ID"]),
                    CountryId = Convert.ToInt32(dr["CountryId"]),
                    CountryName = dr["CountryName"]?.ToString(),
                    StateId = Convert.ToInt32(dr["StateId"]),
                    StateName = dr["StateName"]?.ToString(),
                    CityId = Convert.ToInt32(dr["CityId"]),
                    CityName = dr["CityName"]?.ToString(),
                    AreaName = dr["Area"]?.ToString(),
                    Pincode = dr["Pincode"]?.ToString(),
                    StatusFlag = dr["StatusFlag"].ToString() ?? "A"
                });
            }
            return list;
        }

        public Area GetById(int id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Area_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                return new Area
                {
                    Id = Convert.ToInt32(dr["ID"]),
                    CountryId = Convert.ToInt32(dr["CountryId"]),
                    StateId = Convert.ToInt32(dr["StateId"]),
                    CityId = Convert.ToInt32(dr["CityId"]),
                    AreaName = dr["Area"]?.ToString(),
                    Pincode = dr["Pincode"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                };
            }
            return null; 
        }
        public int Insert(Area model, int createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Area_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateId", model.StateId);
            cmd.Parameters.AddWithValue("@CityId", model.CityId);
            cmd.Parameters.AddWithValue("@Area",model.AreaName ?? "");
            cmd.Parameters.AddWithValue("@Pincode", model.Pincode ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);
            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(Area model, int updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Area_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@CityId", model.CityId);
            cmd.Parameters.AddWithValue("@Area", model.AreaName ?? "");
            cmd.Parameters.AddWithValue("@Pincode", model.Pincode ?? "");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(int id, string StatusFlag, int UpdateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Area_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", StatusFlag);
            cmd.Parameters.AddWithValue("UpdateUser", UpdateUser);
            con.Open(); 
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Area_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
