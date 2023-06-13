using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootstrapThing
{
    /// <summary>
    /// Summary description for Logout
    /// </summary>
    public class Logout : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpCookie token = context.Request.Cookies["Authentication"];
            if(token != null)
            {
                token.Expires = DateTime.Now.AddDays(-1);
                context.Response.Cookies.Add(token);
            }
            context.Response.Redirect("/Default.aspx");
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