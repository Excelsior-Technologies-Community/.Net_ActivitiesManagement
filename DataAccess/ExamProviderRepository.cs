using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class ExamProviderRepository
    {
        private readonly string _connectionString;

        public ExamProviderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<ExamProvider> GetAll()
        {
            var list = new List<ExamProvider>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamProvider_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public ExamProvider GetById(long id)
        {
            ExamProvider item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamProvider_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public long Insert(ExamProvider model,string createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamProvider_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@ExamTypeId", model.ExamTypeId);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@website", (object)model.Website ?? "");
            cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", createUser ?? "");

            var outputParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (long)outputParam.Value;
        }

        public void Update(ExamProvider model, string updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamProvider_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@ExamTypeId", model.ExamTypeId);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Website",(object)model.Website ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object)model.Description ??  DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");
            
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, string updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamProvider_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();

        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamProvider_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public List<ExamTypeLookup> GetExamTypeDropDown()
        {
            var list = new List<ExamTypeLookup>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamType_GetAllActive", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ExamTypeLookup
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString()
                });
            }
            return list;
        }

        private static ExamProvider Map(SqlDataReader dr)
        {
            return new ExamProvider
            {
                Id = Convert.ToInt64(dr["ID"]),
                ExamTypeId = dr["ExamTypeId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ExamTypeId"]),
                ExamTypeTitle = dr["ExamTypeTitle"]?.ToString(),
                Title = dr["Title"]?.ToString(),
                Website = dr["Website"]?.ToString(),
                Description = dr["Description"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }
    }
}
