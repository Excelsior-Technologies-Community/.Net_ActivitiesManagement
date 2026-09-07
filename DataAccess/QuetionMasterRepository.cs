using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.Repositories
{
    public class QuestionMasterRepository
    {
        private readonly string _connectionString;

        public QuestionMasterRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<QuestionMaster> GetAll()
        {
            var list = new List<QuestionMaster>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Question_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public QuestionMaster GetById(long id)
        {
            QuestionMaster item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Question_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public long Insert(QuestionMaster model, long createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Question_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@AnsType", model.AnsType ?? "");
            cmd.Parameters.AddWithValue("@PageMasterId", (object)model.PageMasterId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreateUser", createUser);

            var outputParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (long)outputParam.Value;
        }

        public void Update(QuestionMaster model, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Question_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@AnsType", model.AnsType ?? "");
            cmd.Parameters.AddWithValue("@PageMasterId", (object)model.PageMasterId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Question_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Question_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public List<PageMasterLookup> GetPageMasterList()
        {
            var list = new List<PageMasterLookup>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_PageMaster_GetActiveList", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new PageMasterLookup
                {
                    Id = Convert.ToInt64(dr["Id"]),
                    Title = dr["Title"]?.ToString()
                });
            }
            return list;
        }

        private static QuestionMaster Map(SqlDataReader dr)
        {
            return new QuestionMaster
            {
                Id = Convert.ToInt64(dr["Id"]),
                Title = dr["Title"]?.ToString(),
                AnsType = dr["AnsType"]?.ToString(),
                PageMasterId = dr["PageMasterId"] == DBNull.Value ? null : Convert.ToInt64(dr["PageMasterId"]),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }
    }
}
