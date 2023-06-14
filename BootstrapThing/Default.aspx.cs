using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BootstrapThing
{
    public partial class Default : System.Web.UI.Page
    {
        string constring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + HttpContext.Current.Server.MapPath("~") + @"App_Data\Main.mdf;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            

            using (SqlConnection cnn = new SqlConnection(constring))
            {
                try
                {
                    HttpCookie Token = Request.Cookies["Authentication"];
                    if (Token != null)
                    {
                        Response.Redirect("/Chatbox.aspx");
                    }
                }
                catch (Exception ex)
                {
                    HttpCookie Token = Request.Cookies["Authentication"];

                    if (Token == null)
                    {
                        Response.StatusCode = 500;
                        Response.Write("Unable to connect to the database => " + ex.Message);
                    }
                    else
                    {
                        Response.Redirect("/Chatbox.aspx");
                    }
                }
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Users WHERE Username = @user";

            string username = StripHtmlTagsAndSpecialCharacters(txtUsername.Text);
            string pass = txtPassword.Text;
            try
            {
                using (SqlConnection cnn = new SqlConnection(constring))
                {
                    cnn.Open();

                    using (SqlCommand user = new SqlCommand(query, cnn))
                    {
                        user.Parameters.AddWithValue("@user", txtUsername.Text);
                        SqlDataReader reader = user.ExecuteReader();

                        if (reader.HasRows)
                        {
                            string passdb = "";
                            string authid = "";
                            if (reader.Read())
                            {
                                passdb = reader["Password"].ToString();
                                if (passdb == ComputeMD5Hash(pass))
                                {
                                    authid = reader["AuthToken"].ToString();

                                    HttpCookie authcookie = new HttpCookie("Authentication", authid);
                                    Response.Cookies.Add(authcookie);

                                    Response.Redirect("/Chatbox.aspx");
                                }
                                else
                                {
                                    lblError.Visible = true;
                                    lblError.Text = "Invalid password.";
                                }
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "Username couldn't be found";
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                HttpCookie Token = Request.Cookies["Authentication"];

                if (Token == null)
                {
                    Response.StatusCode = 500;
                    Response.Write("Unable to connect to the database => " + ex.Message);
                }
                else
                {
                    Response.Redirect("/Chatbox.aspx");
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
        public static string ComputeMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}