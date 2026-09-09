using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class CompanyRepository
    {
        private readonly string _connectionString;

        public CompanyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Company> GetAll()
        {
            var list = new List<Company>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Company_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public Company GetById(long id)
        {
            Company item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Company_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public long Insert(Company model, string createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Company_Insert", con) { CommandType = CommandType.StoredProcedure };
            AddCommonParams(cmd, model);
            cmd.Parameters.AddWithValue("@CreateUser", createUser ?? "");

            var outputParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (long)outputParam.Value;
        }

        public void Update(Company model, string updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Company_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", model.Id);
            AddCommonParams(cmd, model);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, string updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Company_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Company_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

       

        public List<LookupItem> GetCountryList()
        {
            return RunLookup("usp_Company_GetCountryList", null, null);
        }

        public List<LookupItem> GetStateListByCountry(long countryId)
        {
            return RunLookup("usp_Company_GetStateListByCountry", "@CountryId", countryId);
        }

        public List<LookupItem> GetCityListByState(long stateId)
        {
            return RunLookup("usp_Company_GetCityListByState", "@StateId", stateId);
        }

        public List<LookupItem> GetAreaListByCity(long cityId)
        {
            return RunLookup("usp_Company_GetAreaListByCity", "@CityId", cityId);
        }

        private List<LookupItem> RunLookup(string procName, string paramName, long? paramValue)
        {
            var list = new List<LookupItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(procName, con) { CommandType = CommandType.StoredProcedure };
            if (paramName != null)
                cmd.Parameters.AddWithValue(paramName, paramValue);

            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new LookupItem
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString()
                });
            }
            return list;
        }

        private static void AddCommonParams(SqlCommand cmd, Company model)
        {
            cmd.Parameters.AddWithValue("@RegistrationDate", (object)model.RegistrationDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName ?? "");
            cmd.Parameters.AddWithValue("@ContactPerson1Name", model.ContactPerson1Name ?? "");
            cmd.Parameters.AddWithValue("@ContactPerson1Mobile", model.ContactPerson1Mobile ?? "");
            cmd.Parameters.AddWithValue("@ContactPerson2Name", (object)model.ContactPerson2Name ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ContactPerson2Mobile", (object)model.ContactPerson2Mobile ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", model.Email ?? "");
            cmd.Parameters.AddWithValue("@Website", (object)model.Website ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CompanyDescription", (object)model.CompanyDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CompanyMapLink", (object)model.CompanyMapLink ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CompanyLogo", (object)model.CompanyLogo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryId", (object)model.CountryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryName", (object)model.CountryName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StateId", (object)model.StateId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StateName", (object)model.StateName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CityId", (object)model.CityId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CityName", (object)model.CityName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AreaId", (object)model.AreaId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Area", (object)model.Area ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Pincode", (object)model.Pincode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object)model.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TermAndConditions", (object)model.TermAndConditions ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GSTNumber", (object)model.GSTNumber ?? DBNull.Value);
        }

        private static Company Map(SqlDataReader dr)
        {
            return new Company
            {
                Id = Convert.ToInt64(dr["ID"]),
                RegistrationDate = dr["RegistrationDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["RegistrationDate"]),
                CompanyName = dr["CompanyName"]?.ToString(),
                ContactPerson1Name = dr["ContactPerson1Name"]?.ToString(),
                ContactPerson1Mobile = dr["ContactPerson1Mobile"]?.ToString(),
                ContactPerson2Name = dr["ContactPerson2Name"]?.ToString(),
                ContactPerson2Mobile = dr["ContactPerson2Mobile"]?.ToString(),
                Email = dr["Email"]?.ToString(),
                Website = dr["Website"]?.ToString(),
                CompanyDescription = dr["CompanyDescription"]?.ToString(),
                CompanyMapLink = dr["CompanyMapLink"]?.ToString(),
                CompanyLogo = dr["CompanyLogo"]?.ToString(),
                CountryId = dr["CountryId"] == DBNull.Value ? null : Convert.ToInt64(dr["CountryId"]),
                CountryName = dr["CountryName"]?.ToString(),
                StateId = dr["StateId"] == DBNull.Value ? null : Convert.ToInt64(dr["StateId"]),
                StateName = dr["StateName"]?.ToString(),
                CityId = dr["CityId"] == DBNull.Value ? null : Convert.ToInt64(dr["CityId"]),
                CityName = dr["CityName"]?.ToString(),
                AreaId = dr["AreaId"] == DBNull.Value ? null : Convert.ToInt64(dr["AreaId"]),
                Area = dr["Area"]?.ToString(),
                Pincode = dr["Pincode"]?.ToString(),
                Address = dr["Address"]?.ToString(),
                TermAndConditions = dr["TermAndConditions"]?.ToString(),
                GSTNumber = dr["GSTNumber"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }
    }
}
