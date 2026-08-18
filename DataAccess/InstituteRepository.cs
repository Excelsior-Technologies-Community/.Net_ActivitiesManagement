using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.Repositories
{
    public class InstituteRepository
    {
        private readonly string _connectionString;

        public InstituteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Institute> GetAll()
        {
            var list = new List<Institute>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Institute_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public Institute GetById(long id)
        {
            Institute item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Institute_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = new Institute
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    InstituteTypeId = dr["InstituteTypeId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["InstituteTypeId"]),
                    InstituteTypeTitle = dr["InstituteTypeTitle"]?.ToString(),
                    InstituteName = dr["InstituteName"]?.ToString(),
                    ContactNumber = dr["ContactNumber"]?.ToString(),
                    Email = dr["Email"]?.ToString(),
                    Website = dr["Website"]?.ToString(),
                    Institutecode = dr["Institutecode"]?.ToString(),
                    Pincode = dr["Pincode"]?.ToString(),
                    InstituteLogo = dr["InstituteLogo"]?.ToString(),
                    Address = dr["Address"]?.ToString(),
                    Remarks = dr["Remarks"]?.ToString(),
                    CountryId = dr["CountryId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["CountryId"]),
                    StateId = dr["StateId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["StateId"]),
                    CityId = dr["CityId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["CityId"]),
                    AreaId = dr["AreaId"] == DBNull.Value ? null : Convert.ToInt64(dr["AreaId"]),
                    StatusFlag = dr["StatusFlag"]?.ToString()
                };
            }
            return item;
        }

        public long Insert(Institute model, string createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Institute_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@InstituteTypeId", model.InstituteTypeId);
            cmd.Parameters.AddWithValue("@InstituteName", model.InstituteName ?? "");
            cmd.Parameters.AddWithValue("@ContactNumber", (object)model.ContactNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Website", (object)model.Website ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Institutecode", (object)model.Institutecode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Pincode", (object)model.Pincode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InstituteLogo", (object)model.InstituteLogo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", model.Address ?? "");
            cmd.Parameters.AddWithValue("@Remarks", (object)model.Remarks ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateId", model.StateId);
            cmd.Parameters.AddWithValue("@CityId", model.CityId);
            cmd.Parameters.AddWithValue("@AreaId", (object)model.AreaId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreateUser", createUser ?? "");

            var outputParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (long)outputParam.Value;
        }

        public void Update(Institute model, string updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Institute_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@InstituteTypeId", model.InstituteTypeId);
            cmd.Parameters.AddWithValue("@InstituteName", model.InstituteName ?? "");
            cmd.Parameters.AddWithValue("@ContactNumber", (object)model.ContactNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Website", (object)model.Website ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Institutecode", (object)model.Institutecode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Pincode", (object)model.Pincode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InstituteLogo", (object)model.InstituteLogo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", model.Address ?? "");
            cmd.Parameters.AddWithValue("@Remarks", (object)model.Remarks ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateId", model.StateId);
            cmd.Parameters.AddWithValue("@CityId", model.CityId);
            cmd.Parameters.AddWithValue("@AreaId", (object)model.AreaId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, string updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Institute_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Institute_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.BeginExecuteNonQuery();
        }

        public List<DropdownItem> GetInstituteTypeDropdown()
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_InstituteType_GetAllActive", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropdownItem { Id = Convert.ToInt64(dr["Id"]), Name = dr["Title"]?.ToString() });
            }
            return list;
        }

        public List<DropdownItem> GetCityDropdown(long stateId)
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_City_GetByStateId", con) { CommandType = CommandType.S
        }


    }
}

