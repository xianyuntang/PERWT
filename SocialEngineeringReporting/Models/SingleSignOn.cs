using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Diagnostics;
using System.Data.SqlClient;
namespace SocialEngineeringReporting.Models
{
    public class SingleSignOn
    {


        public static Tuple<string, string, string> SSO(string ssokey)
        {
            var dirEntry = new DirectoryEntry("LDAPURL","ACCOUNT", "PASSWORD");
            var search = new DirectorySearcher(dirEntry);
            var filter = $"sSOSignKey={ssokey}";
            filter = Microsoft.Security.Application.Encoder.LdapFilterEncode(filter);
            search.Filter = filter;
            search.SearchScope = SearchScope.Subtree;
            try
            {
                var searchResult = search.FindOne();

                var user = searchResult.GetDirectoryEntry();
                var dNcount = searchResult.Path.Split(',').Length;
                var dept = searchResult.Path.Split(',')[dNcount - 4].Substring(3);
                var returnvalue =
                    Tuple.Create(user.Properties["sAMAccountName"].Value.ToString(),
                        user.Properties["cn"].Value.ToString(), dept);
                return returnvalue;
            }
            catch (Exception e)
            {
                var returnvalueF = Tuple.Create("LoginFailed", "LoginFailed", "LoginFailed");
                return returnvalueF;
            }






        }

        public static bool GetAuth(string account)
        {

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GetConnectionString("DSlog");
                conn.Open();
                using (SqlCommand command = new SqlCommand("SELECT top 1 username FROM SOCIALENGINEERINGREPORTINGAUTH WHERE username =@Account", conn))
                {
                    command.Parameters.AddWithValue("@Account", account);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public static string GetConnectionString(string database)
        {
            SqlConnectionStringBuilder connString = new SqlConnectionStringBuilder
            {
                DataSource = "DataSource",
                UserID = "UserID",
                Password = "Password",
                InitialCatalog = database
            };
            return connString.ToString();
        }
    }
}