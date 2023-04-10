using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Serilog.Sinks.File;
using System.Diagnostics;

namespace AkaProje
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File(Server.MapPath("~/App_Data/logs.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            string stackEntryDelimiter = " at ";
            string topStackEntry = String.Empty;
            string stackTrace = exception.StackTrace;
            try
            {
                int nextStackEntry = stackTrace.IndexOf(stackEntryDelimiter, stackEntryDelimiter.Length);
                if (nextStackEntry > 0)
                {
                    topStackEntry = stackTrace.Substring(0, nextStackEntry);
                }
                else
                {
                    topStackEntry = stackTrace;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            //Log the exception using Serilog
            Log.Error(exception, "Error in {RequestUrl} User: {UserName} Error Message: {ErrorMessage} Line: {TopStackEntry}",
                Request.Url.ToString(), Session["kullaniciadi"], exception.Message, topStackEntry);

            Server.ClearError();
            Response.Redirect("http://localhost:49743/CustomError.aspx");
        }
    

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}