using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;


namespace ActivitiesManagement.DataAccess
{
    public class ProgramRepository
    {
        private readonly string _connectionString;

        public ProgramRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Program> GetAll()
        {
            var list = new List<Program>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Program_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Program
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    ProgramCode = dr["ProgramCode"]?.ToString(),
                    Title = dr["Title"]?.ToString() ?? "",
                    ProgramTypeTitle = dr["ProgramTypeTitle"]?.ToString(),
                    InstituteTypeTitle = dr["InstituteTypeTitle"]?.ToString(),
                    InstituteTitle = dr["InstituteTitle"]?.ToString(),
                    ProgramDurationTitle = dr["ProgramDurationTitle"]?.ToString(),
                    ProgramCampus = dr["ProgramCampus"]?.ToString(),
                    OpenDate = dr["OpenDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["OpenDate"]),
                    CloseDate = dr["CloseDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["CloseDate"]),
                    MaxBacklogAllowed = dr["MaxBacklogAllowed"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                });
            }
            return list;
        }

        public Program? GetById(long id)
        {
            Program? item = null;
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Program_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                item = new Program
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    ProgramCode = dr["ProgramCode"]?.ToString(),
                    Title = dr["Title"]?.ToString() ?? "",
                    ProgramTypeId = dr["ProgramTypeId"] == DBNull.Value ? null : Convert.ToInt64(dr["ProgramTypeId"]),
                    CountryId = dr["CountryId"] == DBNull.Value ? null : Convert.ToInt64(dr["CountryId"]),
                    InstituteTypeId = dr["InstituteTypeId"] == DBNull.Value ? null : Convert.ToInt64(dr["InstituteTypeId"]),
                    InstituteId = dr["InstituteId"] == DBNull.Value ? null : Convert.ToInt64(dr["InstituteId"]),
                    ProgramLevelId = dr["ProgramLevelId"] == DBNull.Value ? null : Convert.ToInt64(dr["ProgramLevelId"]),
                    StreamId = dr["StreamId"] == DBNull.Value ? null : Convert.ToInt64(dr["StreamId"]),
                    SpecializationId = dr["SpecializationId"] == DBNull.Value ? null : Convert.ToInt64(dr["SpecializationId"]),
                    ProgramDurationId = dr["ProgramDurationId"] == DBNull.Value ? null : Convert.ToInt64(dr["ProgramDurationId"]),
                    ProgramCampus = dr["ProgramCampus"]?.ToString(),
                    OpenDate = dr["OpenDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["OpenDate"]),
                    CloseDate = dr["CloseDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["CloseDate"]),
                    QualificationLevelId = dr["QualificationLevelId"] == DBNull.Value ? null : Convert.ToInt64(dr["QualificationLevelId"]),
                    TypeOfGradeId = dr["TypeOfGradeId"] == DBNull.Value ? null : Convert.ToInt64(dr["TypeOfGradeId"]),
                    ScoresRequired = dr["ScoresRequired"]?.ToString(),
                    TotalAnnualFees = dr["TotalAnnualFees"] == DBNull.Value ? null : Convert.ToDecimal(dr["TotalAnnualFees"]),
                    MaxBacklogAllowed = dr["MaxBacklogAllowed"]?.ToString(),
                    IsIELTSRequired = dr["IsIELTSRequired"] != DBNull.Value && Convert.ToBoolean(dr["IsIELTSRequired"]),
                    IsTOEFLRequired = dr["IsTOEFLRequired"] != DBNull.Value && Convert.ToBoolean(dr["IsTOEFLRequired"]),
                    IsGRERequired = dr["IsGRERequired"] != DBNull.Value && Convert.ToBoolean(dr["IsGRERequired"]),
                    IsGMATRequired = dr["IsGMATRequired"] != DBNull.Value && Convert.ToBoolean(dr["IsGMATRequired"]),
                    IsPTERequired = dr["IsPTERequired"] != DBNull.Value && Convert.ToBoolean(dr["IsPTERequired"]),
                    IsOtherScoreRequired = dr["IsOtherScoreRequired"] != DBNull.Value && Convert.ToBoolean(dr["IsOtherScoreRequired"]),
                    IELTSListening = dr["IELTSListening"]?.ToString(),
                    IELTSReading = dr["IELTSReading"]?.ToString(),
                    IELTSWriting = dr["IELTSWriting"]?.ToString(),
                    IELTSSpeaking = dr["IELTSSpeaking"]?.ToString(),
                    IELTSOverAll = dr["IELTSOverAll"]?.ToString(),
                    IsIELTSRelax = dr["IsIELTSRelax"] != DBNull.Value && Convert.ToBoolean(dr["IsIELTSRelax"]),
                    IELTSModules = dr["IELTSModules"]?.ToString(),
                    IELTSScore = dr["IELTSScore"]?.ToString(),
                    PopulateInstituteAddress = dr["PopulateInstituteAddress"]?.ToString(),
                    ProgramURL = dr["ProgramURL"]?.ToString(),
                    Description = dr["Description"]?.ToString(),
                    Remarks = dr["Remarks"]?.ToString(),
                    StatusFlag = dr["StatusFlag"]?.ToString() ?? "A"
                };
            }
            return item;
        }

        public long Insert(Program m, long createUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Program_Insert", con) { CommandType = CommandType.StoredProcedure };
            AddParams(cmd, m);
            cmd.Parameters.AddWithValue("@CreateUser", createUser);
            var outParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);
            con.Open();
            cmd.ExecuteNonQuery();
            return (long)outParam.Value;
        }

        public void Update(Program m, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Program_Update", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", m.Id);
            AddParams(cmd, m);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        private static void AddParams(SqlCommand cmd, Program m)
        {
            cmd.Parameters.AddWithValue("@Title", m.Title ?? "");
            cmd.Parameters.AddWithValue("@ProgramTypeId", (object?)m.ProgramTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CountryId", (object?)m.CountryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InstituteTypeId", (object?)m.InstituteTypeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InstituteId", (object?)m.InstituteId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProgramLevelId", (object?)m.ProgramLevelId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StreamId", (object?)m.StreamId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SpecializationId", (object?)m.SpecializationId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProgramDurationId", (object?)m.ProgramDurationId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProgramCampus", (object?)m.ProgramCampus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OpenDate", (object?)m.OpenDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CloseDate", (object?)m.CloseDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@QualificationLevelId", (object?)m.QualificationLevelId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TypeOfGradeId", (object?)m.TypeOfGradeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ScoresRequired", (object?)m.ScoresRequired ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalAnnualFees", (object?)m.TotalAnnualFees ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaxBacklogAllowed", (object?)m.MaxBacklogAllowed ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsIELTSRequired", m.IsIELTSRequired);
            cmd.Parameters.AddWithValue("@IsTOEFLRequired", m.IsTOEFLRequired);
            cmd.Parameters.AddWithValue("@IsGRERequired", m.IsGRERequired);
            cmd.Parameters.AddWithValue("@IsGMATRequired", m.IsGMATRequired);
            cmd.Parameters.AddWithValue("@IsPTERequired", m.IsPTERequired);
            cmd.Parameters.AddWithValue("@IsOtherScoreRequired", m.IsOtherScoreRequired);
            cmd.Parameters.AddWithValue("@IELTSListening", (object?)m.IELTSListening ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IELTSReading", (object?)m.IELTSReading ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IELTSWriting", (object?)m.IELTSWriting ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IELTSSpeaking", (object?)m.IELTSSpeaking ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IELTSOverAll", (object?)m.IELTSOverAll ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsIELTSRelax", m.IsIELTSRelax);
            cmd.Parameters.AddWithValue("@IELTSModules", (object?)m.IELTSModules ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IELTSScore", (object?)m.IELTSScore ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PopulateInstituteAddress", (object?)m.PopulateInstituteAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProgramURL", (object?)m.ProgramURL ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object?)m.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Remarks", (object?)m.Remarks ?? DBNull.Value);
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Program_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Program_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        private List<DropdownItem> RunDropdown(string sp, string idCol, string nameCol, (string, object)? param = null)
        {
            var list = new List<DropdownItem>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sp, con) { CommandType = CommandType.StoredProcedure };
            if (param.HasValue) cmd.Parameters.AddWithValue(param.Value.Item1, param.Value.Item2);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DropdownItem { Id = Convert.ToInt64(dr[idCol]), Name = dr[nameCol]?.ToString() });
            }
            return list;
        }

        public List<DropdownItem> GetProgramTypeDropdown() => RunDropdown("usp_ProgramType_GetAllActive", "ID", "Title");
        public List<DropdownItem> GetCountryDropdown() => RunDropdown("usp_Country_GetAllActive", "ID", "CountryName");
        public List<DropdownItem> GetInstituteTypeDropdown() => RunDropdown("usp_InstituteType_GetAllActive", "Id", "Title");
        public List<DropdownItem> GetInstituteDropdown(long instituteTypeId) => RunDropdown("usp_Institute_GetByInstituteType", "ID", "InstituteName", ("@InstituteTypeId", instituteTypeId));
        public List<DropdownItem> GetStreamDropdown() => RunDropdown("usp_Stream_GetAllActive", "ID", "Title");
        public List<DropdownItem> GetSpecializationDropdown() => RunDropdown("usp_Specialization_GetAllActive", "ID", "Title");
        public List<DropdownItem> GetProgramDurationDropdown() => RunDropdown("usp_ProgramDuration_GetAllActive", "ID", "Title");
        public List<DropdownItem> GetGradeDropdown() => RunDropdown("usp_Grade_GetAllActive", "Id", "Title");
    }
}
