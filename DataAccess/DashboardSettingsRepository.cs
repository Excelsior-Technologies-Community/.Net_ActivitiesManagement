using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.Repositories
{
    public class DashboardSettingsRepository
    {
        private readonly string _connectionString;

        public DashboardSettingsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<DashboardSetting> GetAllForUser(long userId)
        {
            var list = new List<DashboardSetting>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_DashboardSettings_GetAllForUser", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CreateUser", userId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DashboardSetting
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    DashboardSettingsUserType = dr["DashboardSettingsUserType"]?.ToString(),
                    Title = dr["Title"]?.ToString() ?? "",
                    Type = dr["Type"]?.ToString() ?? "",
                    Action = dr["Action"]?.ToString(),
                    ActionType = dr["ActionType"]?.ToString(),
                    ActionValue = dr["ActionValue"]?.ToString()
                });
            }
            return list;
        }

        
        public void SaveAllForUser(long userId, List<DashboardSetting> settings)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();

            using (var deleteCmd = new SqlCommand("usp_DashboardSettings_DeleteAllForUser", con) { CommandType = CommandType.StoredProcedure })
            {
                deleteCmd.Parameters.AddWithValue("@CreateUser", userId);
                deleteCmd.ExecuteNonQuery();
            }

            foreach (var s in settings)
            {
                using var insertCmd = new SqlCommand("usp_DashboardSettings_Insert", con) { CommandType = CommandType.StoredProcedure };
                insertCmd.Parameters.AddWithValue("@DashboardSettingsUserType", (object?)s.DashboardSettingsUserType ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@Title", s.Title ?? "");
                insertCmd.Parameters.AddWithValue("@Type", s.Type ?? "");
                insertCmd.Parameters.AddWithValue("@Action", (object?)s.Action ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@ActionType", (object?)s.ActionType ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@ActionValue", (object?)s.ActionValue ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@CreateUser", userId);

                var outParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                insertCmd.Parameters.Add(outParam);

                insertCmd.ExecuteNonQuery();
            }
        }
    }
}