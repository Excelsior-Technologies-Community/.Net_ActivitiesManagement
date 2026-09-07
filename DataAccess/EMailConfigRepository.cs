using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.Repositories
{
    public class EMailConfigRepository
    {
        private readonly string _connectionString;

        public EMailConfigRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<EMailConfig> GetAll()
        {
            var list = new List<EMailConfig>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_EMailConfig_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public EMailConfig GetById(int id)
        {
            EMailConfig item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_EMailConfig_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public int Insert(EMailConfig model, long createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_EMailConfig_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@EmailHost", model.EmailHost ?? "");
            cmd.Parameters.AddWithValue("@EmailPort", model.EmailPort ?? "");
            cmd.Parameters.AddWithValue("@EmailUserName", model.EmailUserName ?? "");
            cmd.Parameters.AddWithValue("@EmailPassword", model.EmailPassword ?? "");
            cmd.Parameters.AddWithValue("@EmailIsSSL", model.EmailIsSSL);
            cmd.Parameters.AddWithValue("@EmailType", model.EmailType ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);

            var outputParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (int)outputParam.Value;
        }

        public void Update(EMailConfig model, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_EMailConfig_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@EmailHost", model.EmailHost ?? "");
            cmd.Parameters.AddWithValue("@EmailPort", model.EmailPort ?? "");
            cmd.Parameters.AddWithValue("@EmailUserName", model.EmailUserName ?? "");
            cmd.Parameters.AddWithValue("@EmailPassword", model.EmailPassword ?? "");
            cmd.Parameters.AddWithValue("@EmailIsSSL", model.EmailIsSSL);
            cmd.Parameters.AddWithValue("@EmailType", model.EmailType ?? "");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(int id, string statusFlag, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_EMailConfig_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_EMailConfig_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        private static EMailConfig Map(SqlDataReader dr)
        {
            return new EMailConfig
            {
                Id = Convert.ToInt32(dr["ID"]),
                EmailHost = dr["EmailHost"]?.ToString(),
                EmailPort = dr["EmailPort"]?.ToString(),
                EmailUserName = dr["EmailUserName"]?.ToString(),
                EmailPassword = dr["EmailPassword"]?.ToString(),
                EmailIsSSL = dr["EmailIsSSL"] != DBNull.Value && Convert.ToBoolean(dr["EmailIsSSL"]),
                EmailType = dr["EmailType"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }
    }
}
