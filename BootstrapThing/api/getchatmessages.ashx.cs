using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.SessionState;
using System.Web.Helpers;

namespace BootstrapThing.api
{
    /// <summary>
    /// Summary description for getchatmessages
    /// </summary>
    public class getchatmessages : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string constring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dkoby\source\repos\BootstrapThing\BootstrapThing\App_Data\Main.mdf;Integrated Security=True";
            List<object> messages = new List<object>();

            using (SqlConnection cnn = new SqlConnection(constring))
            {
                try
                {
                    dynamic NiceArray = new { };
                    HttpCookie Token = context.Request.Cookies["Authentication"];
                    if (Token != null)
                    {
                        cnn.Open();
                        string query1 = "SELECT * FROM Users WHERE AuthToken = @token";
                        using (SqlCommand user = new SqlCommand(query1, cnn))
                        {
                            user.Parameters.AddWithValue("@token", Token.Value);
                            SqlDataReader reader = user.ExecuteReader();

                            if (reader.HasRows)
                            {
                                string ClientUsername = "";
                                while (reader.Read())
                                {
                                    ClientUsername = reader["Username"].ToString();
                                }

                                reader.Close();

                                string query2 = "SELECT TOP 75 m.*, u.* FROM Chats m JOIN Users u ON m.Userid = u.Id ORDER BY m.Id DESC";
                                using (SqlCommand message = new SqlCommand(query2, cnn))
                                {
                                    SqlDataReader readMessages = message.ExecuteReader();
                                    List<object> messagesr = new List<object>();

                                    while (readMessages.Read())
                                    {
                                        string username = readMessages["Username"].ToString();
                                        string content = readMessages["Content"].ToString();
                                        bool ismod = Convert.ToBoolean(readMessages["Admin"]);

                                        var messagely = new
                                        {
                                            Username = username,
                                            Content = content,
                                            IsMod = ismod
                                        };

                                        messagesr.Add(messagely);
                                    }

                                    NiceArray = new
                                    {
                                        Total = messagesr.Count,
                                        Username = ClientUsername,
                                        Messages = messagesr.ToArray()
                                    };
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"Errors\":\"Invalid token\"}");
                                context.Response.End();
                            }
                        }
                    }
                    else
                    {
                        NiceArray = new
                        {
                            Total = 1,
                            Username = "Guest",
                            Messages = new[]
                            {
                                new
                                {
                                    Username = "System",
                                    Content = "Please go to the signup page.",
                                    IsMod = true
                                }
                            }
                        };
                    }

                    context.Response.Write(JsonConvert.SerializeObject(NiceArray));
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write("{\"Errors\":\"Unable to connect to the database => " + ex.Message + "\"}");
                }
            }
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