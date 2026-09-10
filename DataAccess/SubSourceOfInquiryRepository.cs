using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.Repositories
{
    public class SubSourceOfInquiryRepository
    {
        private readonly string _connectionString;

        public SubSourceOfInquiryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SubSourceOfInquiry> GetAll()
        {
            var list = new List<SubSourceOfInquiry>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SubSourceOfInquiry_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr, includeParentTitle: true));
            }
            return list;
        }

        public SubSourceOfInquiry GetById(long id)
        {
            SubSourceOfInquiry item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SubSourceOfInquiry_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = Map(dr, includeParentTitle: false);
            }
            return item;
        }

        public long Insert(SubSourceOfInquiry model, long createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SubSourceOfInquiry_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@SourceOfInquiryId", model.SourceOfInquiryId);
            cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreateUser", createUser);

            var outputParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (long)outputParam.Value;
        }

        public void Update(SubSourceOfInquiry model, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SubSourceOfInquiry_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@SourceOfInquiryId", model.SourceOfInquiryId);
            cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SubSourceOfInquiry_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_SubSourceOfInquiry_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        private static SubSourceOfInquiry Map(SqlDataReader dr, bool includeParentTitle)
        {
            var item = new SubSourceOfInquiry
            {
                Id = Convert.ToInt64(dr["ID"]),
                Title = dr["Title"]?.ToString(),
                SourceOfInquiryId = dr["SourceOfInquiryId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["SourceOfInquiryId"]),
                Description = dr["Description"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };

            if (includeParentTitle)
            {
                item.SourceOfInquiryTitle = dr["SourceOfInquiryTitle"]?.ToString();
            }

            return item;
        }
    }
}
