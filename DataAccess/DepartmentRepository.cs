using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class DepartmentRepository
    {
        private readonly string _connectionString;

        public DepartmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Department> GetAll()
        {
            var list = new List<Department>();
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("usp_Department_GetAll", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(Map(dr));
                        }
                    }
                }
            }
            return list;
        }

        public Department GetById(int id)
        {
            Department item = null;
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("usp_Department_GetById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            item = Map(dr);
                        }
                    }
                }
            }
            return item;
        }

        public int Insert(Department model, int createUser)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("usp_Department_Insert", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
                    cmd.Parameters.AddWithValue("@ShortName", model.ShortName ?? "");
                    cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreateUser", createUser);

                    var outputParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outputParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    return (int)outputParam.Value;
                }
            }
        }

        public void Update(Department model, int updateUser)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("usp_Department_Update", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
                    cmd.Parameters.AddWithValue("@ShortName", model.ShortName ?? "");
                    cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ChangeStatus(int id, string statusFlag, int updateUser)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("usp_Department_ChangeStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
                    cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("usp_Department_Delete", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static Department Map(SqlDataReader dr)
        {
            return new Department
            {
                Id = Convert.ToInt32(dr["Id"]),
                Title = dr["Title"]?.ToString(),
                ShortName = dr["ShortName"]?.ToString(),
                Description = dr["Description"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }
    }
}