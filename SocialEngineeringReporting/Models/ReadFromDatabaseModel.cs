using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Diagnostics;
using Antlr.Runtime.Misc;

namespace SocialEngineeringReporting.Models
{
    public class ReadFromDatabaseModel
    {
        public static List<Tuple<string, DateTime, string>> ReadFromDatabase()
        {
            List<Tuple<string, DateTime, string>> returnList = new List<Tuple<string, DateTime, string>>();
            Tuple<string, DateTime, string> result;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnectionString("DSlog");
                conn.Open();
                string sqlcmd = "SELECT Subject, Time, Result FROM SOCIALENGINEERINGREPORTING ORDER BY Time DESC";
                SqlCommand command = new SqlCommand(sqlcmd, conn);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        result = Tuple.Create(dataReader.GetSqlValue(0).ToString()
                            , DateTime.Parse(dataReader.GetSqlValue(1).ToString()),
                            (dataReader.GetSqlValue(2).ToString()));
                        returnList.Add(result);

                    }

                    return returnList;
                }

            }

        }
        // List<Tuple<標題,帳號,姓名,單位,結果>
        public static List<List<string>> ReadFromDatabaseAll(string sqlcmd)
        {
            
            List<List<string>> returnList = new List<List<string>>();
            List<string> result = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnectionString("DSlog");
                conn.Open();
                SqlCommand command = new SqlCommand(sqlcmd, conn);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            //Debug.WriteLine(dataReader.GetSqlValue(i).ToString());
                            result.Add(dataReader.GetSqlValue(i).ToString());
                        }

                        
                        returnList.Add(result.ToList());
                        
                        result.Clear();
                    }

                    return returnList;
                }

            }

        }

        public static List<string> GetMailAddress()
        {
            List<string> addr = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                
                conn.ConnectionString = GetConnectionString("DSlog");
                conn.Open();
                string sqlcmd = @"SELECT * FROM SOCIALENGINEERINGREPORTINGAUTH";
                SqlCommand command = new SqlCommand(sqlcmd, conn);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            addr.Add(dataReader.GetSqlValue(0).ToString());
                        }
                    }

                }
                return addr;
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