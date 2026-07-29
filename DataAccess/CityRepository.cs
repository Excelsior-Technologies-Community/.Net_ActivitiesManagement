using ActivitiesManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ActivitiesManagement.DataAccess
{
    public class CityRepository
    {
        private readonly DbHelper _db;
        public CityRepository(DbHelper db) { _db = db; }

        public List<City> GetAll()
        {
            var list = new List<City>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_City_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new City
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryId = Convert.ToInt32(dr["CountryId"]),
                    CountryName = dr["CountryName"]?.ToString(),
                    StateId = Convert.ToInt32(dr["StateId"]),
                    StateName = dr["StateName"]?.ToString(),
                    CityName = dr["CityName"]?.ToString(),
                    ShortCode = dr["ShortCode"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                });
            }
            return list;
        }

        public City GetById(int id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_City_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new City
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CountryId = Convert.ToInt32(dr["CountryId"]),
                    StateId = Convert.ToInt32(dr["StateId"]),
                    CityName = dr["CityName"]?.ToString(),
                    ShortCode = dr["ShortCode"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                };
            }
            return null;
        }

        public List<City> GetByStateId(int stateId)
        {
            var list = new List<City>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_City_GetByStateId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@StateId", stateId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new City
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CityName = dr["CityName"]?.ToString()
                });
            }
            return list;
        }

        public int Insert(City model, int createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_City_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateId", model.StateId);
            cmd.Parameters.AddWithValue("@CityName", model.CityName ?? "");
            cmd.Parameters.AddWithValue("@ShortCode", model.ShortCode ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);
            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(City model, int updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_City_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateId", model.StateId);
            cmd.Parameters.AddWithValue("@CityName", model.CityName ?? "");
            cmd.Parameters.AddWithValue("@ShortCode", model.ShortCode ?? "");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(int id, string statusFlag, int updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_City_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_City_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

