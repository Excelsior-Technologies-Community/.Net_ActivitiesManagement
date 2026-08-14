using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class InstituteTypeRepository
    {
        private readonly string _connectionString;

        public InstituteTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<InstituteType> GetAll()
        {
            var list = new List<InstituteType>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_InstituteType_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public InstituteType GetById(int id)
        {
            InstituteType item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_InstituteType_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public int Insert(InstituteType model,int createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Institute_Insert", con);

            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@ShortCode", model.ShortCode ?? "");
            cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", createUser);

            var outputParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (int)outputParam.Value;
        }

        public void Update(InstituteType model, int updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_InstituteType_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@ShortCode", model.ShortCode ?? "");
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(int id, string statusFlag, int updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_InstituteType_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_InstituteType_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }


      private static InstituteType Map(SqlDataReader dr)
        {
            return new InstituteType
            {
                Id = Convert.ToInt32(dr["ID"]),
                Title = dr["Title"]?.ToString(),
                ShortCode = dr["ShortCode"]?.ToString(),
                Description = dr["Description"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }
    }
}
