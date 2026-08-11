using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class ExamCenterRepository
    {
        private readonly string _connectionString;

        public ExamCenterRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<ExamCenter> GetAll()
        {
            var list = new List<ExamCenter>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamCenter_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                list.Add(Map(dr));
            }
            return list;
        }

        public ExamCenter GetById(int id)
        {
            ExamCenter item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamCenter_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                item = Map(dr);
            }
            return item;
        }

        public int Insert(ExamCenter model,string createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamCenter_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@ExamTypeId", (object)model.ExamTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ExamProviderId", model.ExamTypeId);
            cmd.Parameters.AddWithValue("@EcamCenterName", model.ExamCenterName ?? "");
            cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MobileNo", (object)model.MobileNo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address",(object)model.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateId", model.StateId);
            cmd.Parameters.AddWithValue("@CityId",model.CityId);
            cmd.Parameters.AddWithValue("@AreaId",model.AreaId);
            cmd.Parameters.AddWithValue("@Pincode",(object)model.Pincode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreateUser", createUser ?? "");

            var outputParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (int)outputParam.Value;
        }

        public void Update(ExamCenter model, string updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamCenter_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@ExamTypeId", (object)model.ExamTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ExamProviderId", model.ExamTypeId);
            cmd.Parameters.AddWithValue("@EcamCenterName", model.ExamCenterName ?? "");
            cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MobileNo", (object)model.MobileNo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object)model.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            cmd.Parameters.AddWithValue("@StateId", model.StateId);
            cmd.Parameters.AddWithValue("@CityId", model.CityId);
            cmd.Parameters.AddWithValue("@AreaId", model.AreaId);
            cmd.Parameters.AddWithValue("@Pincode", (object)model.Pincode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(int id, string statusFlag, string updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Examcenter_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamCenter_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public List<DropDownItem> GetExamProviderDropDown()
        {
            var list = new List<DropDownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_ExamProvider_GetAllActiveDropDown", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropDownItem { Id = Convert.ToInt64(dr["ID"]), Name = dr["Title"]?.ToString() });
            }
            return list;
        }

        public List<DropDownItem> GetCountryDropDown()
        {
            var list = new List<DropDownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Country_GetAllActive", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                list.Add(new DropDownItem { Id = Convert.ToInt64(dr["ID"]), Name = dr["Title"]?.ToString() });
            }
            return list;
        }

        public List<DropDownItem> GetStateDropDown(long countryId)
        {
            var list = new List<DropDownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_State_GetByCountryId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CountryId", countryId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                list.Add(new DropDownItem { Id = Convert.ToInt64(dr["ID"]), Name = dr["Title"]?.ToString() });
            }
            return list;
        }

        public List<DropDownItem> GetCityDropDown(long stateId)
        {
            var list = new List<DropDownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_City_GetByStateId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@StateId", stateId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropDownItem { Id = Convert.ToInt64(dr["ID"]), Name = dr["Title"]?.ToString() });
            }
            return list;
        }

        public List<DropDownItem> GetAreaDropDown(long cityId)
        {
            var list = new List<DropDownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Area_GetByCityId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("CityId", cityId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropDownItem { Id = Convert.ToInt64(dr["ID"]), Name = dr["Title"]?.ToString() });
            }
            return list;
        }

        public static ExamCenter Map(SqlDataReader dr)
        {
            return new ExamCenter
            {
                Id = Convert.ToInt32(dr["ID"]),
                ExamTypeId = dr["ExamTypeId"] == DBNull.Value ? null : Convert.ToInt64(dr["ExamTypeId"]),
                ExamProviderId = dr["ExamTypeId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ExamProvider"]),
                ExamProviderTitle = dr["ExamProviderTitle"]?.ToString(),
                ExamCenterName = dr["ExamCenterName"]?.ToString(),
                Email = dr["Email"]?.ToString(),
                MobileNo = dr["MobileNo"]?.ToString(),
                Address = dr["Address"]?.ToString(),
                CountryId = dr["CountryId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["CountryId"]),
                StateId = dr["StateId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["StateId"]),
                CityId = dr["CityId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["CityId"]),
                AreaId = dr["AreaId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["AreaId"]),
                Pincode = dr["Pincode"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }
    }
}
