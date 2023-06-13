using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BootstrapThing.api
{
    /// <summary>
    /// Summary description for sendchat
    /// </summary>
    public class sendchat : IHttpHandler
    {
        string constring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dkoby\source\repos\BootstrapThing\BootstrapThing\App_Data\Main.mdf;Integrated Security=True";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            using (SqlConnection cnn = new SqlConnection(constring))
            {
                try
                {
                    HttpCookie Token = context.Request.Cookies["Authentication"];
                    if (Token != null)
                    {
                        cnn.Open();
                        string query1 = "SELECT * FROM Users WHERE AuthToken = @token";
                        using(SqlCommand user = new SqlCommand(query1, cnn))
                        {
                            user.Parameters.AddWithValue("@token", Token.Value);
                            SqlDataReader ReadUser = user.ExecuteReader();
                            if (ReadUser.HasRows)
                            {
                                string userid = "";
                                while (ReadUser.Read())
                                {
                                    userid = ReadUser["Id"].ToString();
                                }
                                ReadUser.Close();

                                string
                            }
                            else
                            {
                                context.Response.Write("{\"error\":\"Invalid authentication\"}");
                            }
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"error\":\"Not signed in\"}");
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("{\"error\":\"failed to connect to db\"}");
                }
            }

            
            
        }
        public static string StripHtmlTagsAndSpecialCharacters(string input)
        {
            string strippedHtml = Regex.Replace(input, "<.*?>", string.Empty);

            string pattern = "[^a-zA-Z0-9 ]";
            string strippedText = Regex.Replace(strippedHtml, pattern, string.Empty);

            string cleanedText = strippedText.Trim();

            return cleanedText;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}