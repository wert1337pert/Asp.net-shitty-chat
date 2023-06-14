using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BootstrapThing
{
    public partial class Chatbox : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string constring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + HttpContext.Current.Server.MapPath("~") + @"App_Data\Main.mdf;Integrated Security=True";

            using (SqlConnection cnn = new SqlConnection(constring))
            {
                try
                {
                    HttpCookie Token = Request.Cookies["Authentication"];
                    if (Token == null)
                    {
                        Response.Redirect("/Default.aspx");
                    }
                }
                catch (Exception ex)
                {
                    HttpCookie Token = Request.Cookies["Authentication"];
                    if (Token != null)
                    {
                        Response.StatusCode = 500;
                        Response.Write("Unable to connect to the database => " + ex.Message);
                    }
                    else
                    {
                        Response.Redirect("/Default.aspx");
                    }
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Logout.ashx");
        }
    }
}