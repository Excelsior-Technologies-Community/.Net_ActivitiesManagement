using Microsoft.Data.SqlClient;
using System.Data;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class ExamTypeRepository
    {
        private readonly DbHelper _db;

        public ExamTypeRepository(DbHelper db)
        {
            _db = db;
        }

        public List<ExamType> GetAll()
        {
            var list = new List<ExamType>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamType_GetAll", con) { CommandType = CommandType.StoredProcedure };
            con.Open();
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    list.Add(Map(dr));
                }
            }

            foreach (var item in list)
            {
                item.GradeTitles = GetDetailsByExamTypeId(item.Id);
            }

            return list;
        }

        public ExamType GetById(long id)
        {
            ExamType item = null;
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamType_GetById", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    item = Map(dr);
                }
            }

            if (item != null)
                item.GradeTitles = GetDetailsByExamTypeId(id);

            return item;
        }

        public long Insert(ExamType model, string createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamType_Insert", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsLead", model.IsLead ?? "N");
            cmd.Parameters.AddWithValue("@IsInquiry", model.IsInquiry ?? "N");
            cmd.Parameters.AddWithValue("@IsRegistration", model.IsRegistration ?? "N");
            cmd.Parameters.AddWithValue("@IsCoaching", model.IsCoaching ?? "N");
            cmd.Parameters.AddWithValue("@IsProcess", model.IsProcess ?? "N");
            cmd.Parameters.AddWithValue("@IsMock", model.IsMock ?? "N");
            cmd.Parameters.AddWithValue("@IsProfessional", model.IsProfessional ?? "N");
            cmd.Parameters.AddWithValue("@IsEnglishTest", model.IsEnglishTest ?? "N");
            cmd.Parameters.AddWithValue("@CreateUser", createUser ?? "");

            var outputParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (long)outputParam.Value;
        }

        public void Update(ExamType model, string updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamType_Update", con) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Id", model.Id);
            cmd.Parameters.AddWithValue("@Title", model.Title ?? "");
            cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsLead", model.IsLead ?? "N");
            cmd.Parameters.AddWithValue("@IsInquiry", model.IsInquiry ?? "N");
            cmd.Parameters.AddWithValue("@IsRegistration", model.IsRegistration ?? "N");
            cmd.Parameters.AddWithValue("@IsCoaching", model.IsCoaching ?? "N");
            cmd.Parameters.AddWithValue("@IsProcess", model.IsProcess ?? "N");
            cmd.Parameters.AddWithValue("@IsMock", model.IsMock ?? "N");
            cmd.Parameters.AddWithValue("@IsProfessional", model.IsProfessional ?? "N");
            cmd.Parameters.AddWithValue("@IsEnglishTest", model.IsEnglishTest ?? "N");
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ChangeStatus(long id, string statusFlag, string updateUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamType_ChangeStatus", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
            cmd.Parameters.AddWithValue("@UpdateUser", updateUser ?? "");
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamType_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public List<ExamTypeDetail> GetDetailsByExamTypeId(long examTypeId)
        {
            var list = new List<ExamTypeDetail>();
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamTypeDetail_GetByExamTypeId", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ExamTypeId", examTypeId);
            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ExamTypeDetail
                {
                    Id = Convert.ToInt64(dr["ID"]),
                    Title = dr["Title"]?.ToString(),
                    ExamTypeId = Convert.ToInt64(dr["ExamTypeId"]),
                    StatusFlag = dr["StatusFlag"]?.ToString()
                });
            }
            return list;
        }

        public long InsertDetail(long examTypeId, string title, long? createUser)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamTypeDetail_Insert", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ExamTypeId", examTypeId);
            cmd.Parameters.AddWithValue("@Title", title ?? "");
            cmd.Parameters.AddWithValue("@CreateUser", (object)createUser ?? DBNull.Value);

            var outputParam = new SqlParameter("@NewId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            con.Open();
            cmd.ExecuteNonQuery();

            return (long)outputParam.Value;
        }

        public void DeleteDetail(long id)
        {
            using var con = _db.GetConnection();
            using var cmd = new SqlCommand("usp_ExamTypeDetail_Delete", con) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        private static ExamType Map(SqlDataReader dr)
        {
            return new ExamType
            {
                Id = Convert.ToInt64(dr["ID"]),
                Title = dr["Title"]?.ToString(),
                Description = dr["Description"]?.ToString(),
                IsLead = dr["IsLead"]?.ToString(),
                IsInquiry = dr["IsInquiry"]?.ToString(),
                IsRegistration = dr["IsRegistration"]?.ToString(),
                IsCoaching = dr["IsCoaching"]?.ToString(),
                IsProcess = dr["IsProcess"]?.ToString(),
                IsMock = dr["IsMock"]?.ToString(),
                IsProfessional = dr["IsProfessional"]?.ToString(),
                IsEnglishTest = dr["IsEnglishTest"]?.ToString(),
                StatusFlag = dr["StatusFlag"]?.ToString()
            };
        }
    }
}

