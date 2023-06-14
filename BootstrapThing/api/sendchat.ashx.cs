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
        string constring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + HttpContext.Current.Server.MapPath("~") + @"App_Data\Main.mdf;Integrated Security=True";
        public void ProcessRequest(HttpContext context)
        {
           context.Response.ContentType = "application/json";
            using (SqlConnection cnn = new SqlConnection(constring))
            {
                try
                {
                    if(context.Request.Params["message"] != null && StripHtmlTagsAndSpecialCharacters(context.Request.Params["message"]) != "")
                    {
                        string text = StripHtmlTagsAndSpecialCharacters(context.Request.Params["message"]);
                        HttpCookie Token = context.Request.Cookies["Authentication"];
                        if (Token != null)
                        {
                            cnn.Open();
                            string query1 = "SELECT * FROM Users WHERE AuthToken = @token";
                            using (SqlCommand user = new SqlCommand(query1, cnn))
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

                                    string query2 = "INSERT INTO Chats (Content, Userid, PostedDate) VALUES (@content, @userid, @date)";
                                    using (SqlCommand chatcreate = new SqlCommand(query2, cnn))
                                    {
                                        chatcreate.Parameters.AddWithValue("@content", text);
                                        chatcreate.Parameters.AddWithValue("@userid", userid);
                                        chatcreate.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                        chatcreate.ExecuteNonQuery();

                                        context.Response.Write("{\"error\":\"none\"}");
                                    }
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
                    else
                    {
                        context.Response.Write("{\"error\":\"lolol\"}");
                    }
                    
                }
                catch (Exception ex)
                {
                    context.Response.Write("{\"error\":\"failed to connect to db => "+ex.Message+"\"}");
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