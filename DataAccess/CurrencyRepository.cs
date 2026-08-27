using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class CurrencyRepository
    {
        private readonly string _connectionString;

        public CurrencyRepository (IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Currency> GetAll()
        {
            var list = new List<Currency>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Currency_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public Currency GetById(int id)
        {
            Currency item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Currency_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr= cmd.ExecuteReader();
            if(dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public int Insert(Currency model, int createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Currency_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Description",(object) model.Description);
            cmd.Parameters.AddWithValue("@CreateUser", createUser);

            var outputParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (int)outputParam.Value;
        }
        public void Update(Currency model, int UpdateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Currency_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Description", (object)model.Description);
            cmd.Parameters.AddWithValue("@UpdateUser", UpdateUser);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(int id, string statusFlag, int UpdateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Currency_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", UpdateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Currency_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        public List<DropdownItem> GetCountryDropDown()
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Currency_GetAllActive", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                list.Add(new DropdownItem { Id = Convert.ToInt64(dr["ID"]), Name = dr["CountryName"]?.ToString() });
            }
            return list;
        }

        private static Currency Map(SqlDataReader dr)
        {
            return new Currency
            {
                Id = Convert.ToInt32(dr["ID"]),
                CountryId = Convert.ToInt32(dr["CountryId"]),
                CountryName = dr["CountyName"]?.ToString(),
                Title = dr["Title"]?.ToString(),
                Description = dr["Description"]?.ToString(),
                StatusFlag = dr["Statusflag"]?.ToString()
            };
        }
    }
}
