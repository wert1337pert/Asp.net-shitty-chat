using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace BootstrapThing
{
    public partial class Signup : System.Web.UI.Page
    {
        string constring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dkoby\source\repos\BootstrapThing\BootstrapThing\App_Data\Main.mdf;Integrated Security=True";

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
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Users (Username, Password, AuthToken, JoinDate, Email) VALUES (@user, @pass, @auth, @join, @mail)";
            string countQuery = "SELECT COUNT(*) FROM Users WHERE Username = @user OR Email = @mail";

            try
            {
                string user = StripHtmlTagsAndSpecialCharacters(txtUsername.Text);
                string pass = txtPassword.Text;
                string email = txtEmail.Text;

                if (pass.Length >= 8 && user.Length >= 5 && user.Length <= 25 && email.Contains("@") && email.Contains(".com"))
                {
                    using (SqlConnection cnn = new SqlConnection(constring))
                    {
                        cnn.Open();

                        using (SqlCommand countCmd = new SqlCommand(countQuery, cnn))
                        {
                            countCmd.Parameters.AddWithValue("@user", user);
                            countCmd.Parameters.AddWithValue("@mail", email);

                            int count = (int)countCmd.ExecuteScalar();

                            if (count > 0)
                            {
                                txtUsername.Text = txtUsername.Text;
                                lblError.Visible = true;
                                lblError.Text = "Your email or username is already in use by another account.";
                                return;
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand(query, cnn))
                        {
                            cmd.Parameters.AddWithValue("@user", user);
                            cmd.Parameters.AddWithValue("@pass", ComputeMD5Hash(pass));
                            cmd.Parameters.AddWithValue("@auth", RandomStringGenerator.GenerateRandomString(100));
                            cmd.Parameters.AddWithValue("@join", DateTime.Now.ToString("MM/dd/yyyy"));
                            cmd.Parameters.AddWithValue("@mail", email);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    HttpCookie goofcookie = new HttpCookie("AccMade", "yes");
                    Response.Cookies.Add(goofcookie);

                    Response.StatusCode = 302;
                    Response.Redirect("/Default.aspx");
                }
                else
                {
                    txtPassword.Text = txtPassword.Text;
                    txtUsername.Text = txtUsername.Text;
                    txtEmail.Text = txtEmail.Text;
                    lblError.Visible = true;

                    if (pass.Length < 8) 
                    {
                        lblError.Text = "Make your password at least 8 characters long.";
                    }
                    if (user.Length < 5)
                    {
                        lblError.Text = "Please enter at least 5 characters for your username.";
                    }
                    else if (user.Length > 25)
                    {
                        lblError.Text = "The maximum character limit is 25 for your username.";
                    }
                    if (!email.Contains("@") || !email.Contains(".com"))
                    {
                        lblError.Text = "Please enter a valid email.";
                    }
                }
            }
            catch (Exception ex)
            {
                HttpCookie Token = Request.Cookies["Authentication"];

                if (Token == null)
                {
                    HttpCookie goof = Request.Cookies["AccMade"];
                    if(goof != null)
                    {
                        Response.Redirect("/Default.aspx");
                    }
                    else
                    {
                        Response.StatusCode = 500;
                        Response.Write("Unable to connect to the database => " + ex.Message);
                    }
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
    public static class RandomStringGenerator
    {
        private static readonly Random random = new Random();

        public static string GenerateRandomString(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(allowedChars.Length);
                char randomChar = allowedChars[randomIndex];
                stringBuilder.Append(randomChar);
            }

            return stringBuilder.ToString();
        }
    }
}