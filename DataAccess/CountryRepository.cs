using ActivitiesManagement.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ActivitiesManagement.DataAccess
{
    public class CountryRepository
    {
        private readonly DbHelper _db;
        public CountryRepository(DbHelper db) { _db = db; }

        public List<Country> GetAll()
        {
            var list = new List<Country>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Country_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public Country GetById(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Country_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return Map(dr);
            }
            return null;
        }

        private Country Map(SqlDataReader dr)
        {
            return new Country
            {
                ID = Convert.ToInt64(dr["ID"]),
                CountryName = dr["CountryName"]?.ToString(),
                ShortCode = dr["ShortCode"]?.ToString(),
                IsIntrested = dr["IsIntrested"] != DBNull.Value && Convert.ToBoolean(dr["IsIntrested"]),
                IsPastRejection = dr["IsPastRejection"] != DBNull.Value && Convert.ToBoolean(dr["IsPastRejection"]),
                IsInquiry = dr["IsInquiry"] != DBNull.Value && Convert.ToBoolean(dr["IsInquiry"]),
                CountryFlagImage = dr["CountryFlagImage"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }

        public long Insert(Country model, long createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Country_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CountryName", model.CountryName ?? "");
            cmd.Parameters.AddWithValue("@ShortCode", model.ShortCode ?? "");
            cmd.Parameters.AddWithValue("@IsIntrested", model.IsIntrested);
            cmd.Parameters.AddWithValue("@IsPastRejection", model.IsPastRejection);
            cmd.Parameters.AddWithValue("@IsInquiry", model.IsInquiry);
            cmd.Parameters.AddWithValue("@CountryFlagImage", model.CountryFlagImage ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);
            con.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        public void Update(Country model, long updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Country_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", model.ID);
            cmd.Parameters.AddWithValue("@CountryName", model.CountryName ?? "");
            cmd.Parameters.AddWithValue("@ShortCode", model.ShortCode ?? "");
            cmd.Parameters.AddWithValue("@IsIntrested", model.IsIntrested);
            cmd.Parameters.AddWithValue("@IsPastRejection", model.IsPastRejection);
            cmd.Parameters.AddWithValue("@IsInquiry", model.IsInquiry);
            cmd.Parameters.AddWithValue("@CountryFlagImage", model.CountryFlagImage ?? "");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Country_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Country_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        public List<Country> GetActiveList()
        {
            var list = new List<Country>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("USP_Country_GetActiveList", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                list.Add(new Country
                {
                    ID = Convert.ToInt64(dr["ID"]),
                    CountryName = dr["CountryName"]?.ToString()
                });
            }

            return list;
        }
    }
}