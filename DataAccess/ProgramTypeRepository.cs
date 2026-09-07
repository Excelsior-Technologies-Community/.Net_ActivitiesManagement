using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class ProgramTypeRepository
    {
        private readonly string _connectionString;

        public ProgramTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public List<ProgramType> GetAll()
        {
            var list = new List<ProgramType>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ProgramType_GetAll", con) { CommandType = CommandType.StoredProcedure };

            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public ProgramType? GetById(long id)
        {
            ProgramType? item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ProgramType_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public long Insert(ProgramType m, long createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ProgramType_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Title", m.Title ?? "");
            cmd.Parameters.AddWithValue("@ShortCode", (object?)m.ShortCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object?)m.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreateUser", createUser);

            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            con.Open();
            cmd.ExecuteNonQuery();
            return (long)outParam.Value;
        }

        public void Update(ProgramType m, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ProgramType_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", m.Id);
            cmd.Parameters.AddWithValue("@Title", m.Title ?? "");
            cmd.Parameters.AddWithValue("@ShortCode", (object?)m.ShortCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object?)m.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ProgramType_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ProgramType_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        private ProgramType Map(SqlDataReader dr)
        {
            return new ProgramType
            {
                Id = Convert.ToInt64(dr["ID"]),
                Title = dr["Title"]?.ToString() ?? "",
                ShortCode = dr["ShortCode"]?.ToString(),
                Description = dr["Description"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
            };
        }
    }
}
