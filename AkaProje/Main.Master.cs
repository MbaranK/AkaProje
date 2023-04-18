using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkaProje
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["kullaniciadi"] == null)
                {                  
                    HttpCookie cookie = Request.Cookies["cerezler"];
                    if (cookie != null)
                    {
                        string cookieval = cookie.Value;
                        Session["kullaniciadi"] = cookieval;
                    }                        
                    else
                        Response.Redirect("http://localhost:49743/login.aspx");
                }
            }
            
        }
    }
}