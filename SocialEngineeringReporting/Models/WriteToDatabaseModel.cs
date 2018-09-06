using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Diagnostics;


namespace SocialEngineeringReporting.Models
{
    public class WriteToDatabaseModel
    {

        public static bool WriteToDatabase(ReportingModel reporting)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnectionString("DSlog");
                conn.Open();
                string sqlcmd = @"INSERT INTO SOCIALENGINEERINGREPORTING (Time,Account,Subject,Name,Department,Result) VALUES 
                    (GETDATE(),@Account,@Subject,@Name,@Department,'尚未確認')";

                using (SqlCommand command = new SqlCommand(sqlcmd, conn))
                {
                    command.Parameters.AddWithValue("@Account", reporting.Account);
                    command.Parameters.AddWithValue("@Subject", reporting.Subject);
                    command.Parameters.AddWithValue("@Name", reporting.Name);
                    command.Parameters.AddWithValue("@Department", reporting.Department);
                    command.ExecuteNonQuery();
                }
                return true;
            }

        }

        public static void UpdateReporting(string Result,string Subject,string Name,string Id)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnectionString("DSlog");
                conn.Open();
                string sqlcmd =
                    "UPDATE SOCIALENGINEERINGREPORTING SET Result = @Result, ReplyTime = GetDate(),ReplyName=@ReplyName WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(sqlcmd, conn))
                {
                    command.Parameters.AddWithValue("@Result", Result);
                    command.Parameters.AddWithValue("@Subject", Subject);
                    command.Parameters.AddWithValue("@ReplyName", Name);
                    command.Parameters.AddWithValue("@id", Id);
                 
                    command.ExecuteNonQuery();
                }
                
            }
        }

        private static string GetConnectionString(string database)
        {
            SqlConnectionStringBuilder connString = new SqlConnectionStringBuilder
            {
                DataSource = "192.168.171.46",
                UserID = "cdcintranet",
                Password = "5n+bwM7s",
                InitialCatalog = database
            };
            return connString.ToString();
        }

        
    }
}