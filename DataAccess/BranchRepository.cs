using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.Repositories
{
    public class BranchRepository
    {
        private readonly string _connectionString;

        public BranchRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Branch> GetAll()
        {
            var list = new List<Branch>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Branch
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    BranchCode = dr["BranchCode"]?.ToString() ?? "",
                    BranchName = dr["BranchName"]?.ToString() ?? "",
                    IsHeadOffice = dr["IsHeadOffice"] != DBNull.Value && Convert.ToBoolean(dr["IsHeadOffice"]),
                    ContactPersonName = dr["ContactPersonName"]?.ToString(),
                    ContactNumber = dr["ContactNumber"]?.ToString(),
                    Email = dr["Email"]?.ToString(),
                    CompanyName = dr["CompanyName"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                });
            }
            return list;
        }

        public Branch? GetById(long id)
        {
            Branch? item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = new Branch
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    BranchCode = dr["BranchCode"]?.ToString() ?? "",
                    BranchName = dr["BranchName"]?.ToString() ?? "",
                    IsHeadOffice = dr["IsHeadOffice"] != DBNull.Value && Convert.ToBoolean(dr["IsHeadOffice"]),
                    ContactPersonName = dr["ContactPersonName"]?.ToString(),
                    ContactNumber = dr["ContactNumber"]?.ToString(),
                    Email = dr["Email"]?.ToString(),
                    Address = dr["Address"]?.ToString(),
                    CountryId = dr["CountryId"] == DBNull.Value ? null : Convert.ToInt64(dr["CountryId"]),
                    StateId = dr["StateId"] == DBNull.Value ? null : Convert.ToInt64(dr["StateId"]),
                    CityId = dr["CityId"] == DBNull.Value ? null : Convert.ToInt64(dr["CityId"]),
                    AreaId = dr["AreaId"] == DBNull.Value ? null : Convert.ToInt64(dr["AreaId"]),
                    Pincode = dr["Pincode"]?.ToString(),
                    CompanyId = dr["CompanyId"] == DBNull.Value ? null : Convert.ToInt64(dr["CompanyId"]),
                    Description = dr["Description"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                };
            }
            return item;
        }

        public long Insert(Branch m, long createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_Insert", con) { CommandType = CommandType.StoredProcedure };
            AddParams(cmd, m);
            cmd.Parameters.AddWithValue("@CreateUser", createUser);
            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            return (long)outParam.Value;
        }

        public void Update(Branch m, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", m.Id);
            AddParams(cmd, m);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        private static void AddParams(SqlCommand cmd, Branch m)
        {
            cmd.Parameters.AddWithValue("@BranchCode", m.BranchCode ?? "");
            cmd.Parameters.AddWithValue("@BranchName", m.BranchName ?? "");
            cmd.Parameters.AddWithValue("@IsHeadOffice", m.IsHeadOffice);
            cmd.Parameters.AddWithValue("@ContactPersonName", (object?)m.ContactPersonName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ContactNumber", (object?)m.ContactNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)m.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)m.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryId", (object?)m.CountryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StateId", (object?)m.StateId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CityId", (object?)m.CityId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AreaId", (object?)m.AreaId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Pincode", (object?)m.Pincode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CompanyId", (object?)m.CompanyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object?)m.Description ?? DBNull.Value);
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        // ---------- Sub-branch checklist ----------

        public List<SubBranchOption> GetSubBranchOptions(long? excludeId, List<long> selectedIds)
        {
            var list = new List<SubBranchOption>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_GetAllForSubBranchList", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ExcludeId", (object?)excludeId ?? DBNull.Value);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var id = Convert.ToInt64(dr["ID"]);
                list.Add(new SubBranchOption
                {
                    Id = id,
                    BranchName = dr["BranchName"]?.ToString() ?? "",
                    BranchCode = dr["BranchCode"]?.ToString() ?? "",
                    IsSelected = selectedIds.Contains(id)
                });
            }
            return list;
        }

        public List<long> GetAssignedSubBranchIds(long branchId)
        {
            var list = new List<long>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_GetAssignedSubBranchIds", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Convert.ToInt64(dr["SubBranchId"]));
            }
            return list;
        }

        public void SaveSubBranches(long branchId, List<long> subBranchIds)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Branch_SaveSubBranches", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@BranchId", branchId);
            cmd.Parameters.AddWithValue("@SubBranchIdsCsv", string.Join(",", subBranchIds));
            con.Open();
            cmd.ExecuteNonQuery();
        }

        // ---------- Dropdowns ----------

        public List<DropdownItem> GetCompanyDropdown()
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Company_GetAllActive", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropdownItem { Id = Convert.ToInt64(dr["ID"]), Name = dr["CompanyName"]?.ToString() });
            }
            return list;
        }

        public List<DropdownItem> GetCountryDropdown()
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Country_GetAllActive", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropdownItem { Id = Convert.ToInt64(dr["ID"]), Name = dr["CountryName"]?.ToString() });
            }
            return list;
        }

        public List<DropdownItem> GetStateDropdown(long countryId)
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_State_GetByCountryId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CountryId", countryId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropdownItem { Id = Convert.ToInt64(dr["Id"]), Name = dr["StateName"]?.ToString() });
            }
            return list;
        }

        public List<DropdownItem> GetCityDropdown(long stateId)
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_City_GetByStateId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@StateId", stateId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropdownItem { Id = Convert.ToInt64(dr["Id"]), Name = dr["CityName"]?.ToString() });
            }
            return list;
        }

        public List<DropdownItem> GetAreaDropdown(long cityId)
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Area_GetByCityId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CityId", cityId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropdownItem { Id = Convert.ToInt64(dr["Id"]), Name = dr["Area"]?.ToString() });
            }
            return list;
        }
    }
}