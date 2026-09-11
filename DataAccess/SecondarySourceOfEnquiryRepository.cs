using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ActivitiesManagement.Models;

namespace ActivitiesManagement.DataAccess
{
    public class SecondarySourceOfEnquiryRepository : ISecondarySourceOfEnquiryRepository
    {
        private readonly string _connectionString;

        public SecondarySourceOfEnquiryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SecondarySourceOfEnquiry> GetAll()
        {
            var list = new List<SecondarySourceOfEnquiry>();

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("usp_SecondarySourceOfEnquiry_GetAll", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapReader(reader));
                    }
                }
            }

            return list;
        }

        public SecondarySourceOfEnquiry GetById(long id)
        {
            SecondarySourceOfEnquiry model = null;

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("usp_SecondarySourceOfEnquiry_GetById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        model = MapReader(reader);
                    }
                }
            }

            return model;
        }

        public long Insert(SecondarySourceOfEnquiry model, long createUser)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("usp_SecondarySourceOfEnquiry_Insert", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreateUser", createUser);

                con.Open();
                var result = cmd.ExecuteScalar();
                return Convert.ToInt64(result);
            }
        }

        public void Update(SecondarySourceOfEnquiry model, long updateUser)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("usp_SecondarySourceOfEnquiry_Update", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", model.Id);
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ChangeStatus(long id, string statusFlag, long updateUser)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("usp_SecondarySourceOfEnquiry_ChangeStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@StatusFlag", statusFlag);
                cmd.Parameters.AddWithValue("@UpdateUser", updateUser);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(long id)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("usp_SecondarySourceOfEnquiry_Delete", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private SecondarySourceOfEnquiry MapReader(SqlDataReader reader)
        {
            return new SecondarySourceOfEnquiry
            {
                Id = Convert.ToInt64(reader["ID"]),
                Title = reader["Title"]?.ToString(),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                StatusFlag = reader["StatusFlag"]?.ToString(),
                CreateUser = reader["CreateUser"] == DBNull.Value ? null : (long?)Convert.ToInt64(reader["CreateUser"]),
                UpdateUser = reader["UpdateUser"] == DBNull.Value ? null : (long?)Convert.ToInt64(reader["UpdateUser"]),
                CreateDate = reader["CreateDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["CreateDate"]),
                UpdateDate = reader["UpdateDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["UpdateDate"])
            };
        }
    }
}