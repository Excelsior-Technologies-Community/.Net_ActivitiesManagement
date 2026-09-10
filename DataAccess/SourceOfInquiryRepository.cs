using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.Repositories
{
    public class SourceOfInquiryRepository
    {
        private readonly string _connectionString;

        public SourceOfInquiryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SourceOfInquiry> GetAll()
        {
            var list = new List<SourceOfInquiry>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SourceOfInquiry_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public List<SourceOfInquiry> GetActiveList()
        {
            var list = new List<SourceOfInquiry>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SourceOfInquiry_GetActiveList", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new SourceOfInquiry
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString()
                });
            }
            return list;
        }

        public SourceOfInquiry GetById(long id)
        {
            SourceOfInquiry item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SourceOfInquiry_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public long Insert(SourceOfInquiry model, long createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SourceOfInquiry_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsLead", model.IsLead ? "Y" : "N");
            cmd.Parameters.AddWithValue("@IsInquiry", model.IsInquiry ? "Y" : "N");
            cmd.Parameters.AddWithValue("@IsRegistration", model.IsRegistration ? "Y" : "N");
            cmd.Parameters.AddWithValue("@IsCoaching", model.IsCoaching ? "Y" : "N");
            cmd.Parameters.AddWithValue("@IsProcess", model.IsProcess ? "Y" : "N");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);

            var outputParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (long)outputParam.Value;
        }

        public void Update(SourceOfInquiry model, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SourceOfInquiry_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsLead", model.IsLead ? "Y" : "N");
            cmd.Parameters.AddWithValue("@IsInquiry", model.IsInquiry ? "Y" : "N");
            cmd.Parameters.AddWithValue("@IsRegistration", model.IsRegistration ? "Y" : "N");
            cmd.Parameters.AddWithValue("@IsCoaching", model.IsCoaching ? "Y" : "N");
            cmd.Parameters.AddWithValue("@IsProcess", model.IsProcess ? "Y" : "N");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SourceOfInquiry_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SourceOfInquiry_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        private static SourceOfInquiry Map(SqlDataReader dr)
        {
            return new SourceOfInquiry
            {
                Id = Convert.ToInt64(dr["ID"]),
                Title = dr["Title"]?.ToString(),
                Description = dr["Description"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString(),
                IsLead = dr["IsLead"]?.ToString() == "Y",
                IsInquiry = dr["IsInquiry"]?.ToString() == "Y",
                IsRegistration = dr["IsRegistration"]?.ToString() == "Y",
                IsCoaching = dr["IsCoaching"]?.ToString() == "Y",
                IsProcess = dr["IsProcess"]?.ToString() == "Y"
            };
        }
    }
}