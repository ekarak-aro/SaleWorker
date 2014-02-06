using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SaleWorker
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception objErr = Server.GetLastError().GetBaseException();
            string err = "<br><b>Error in: </b>" + Request.Url.ToString() +
                "<br><b>Error Message: </b>" + objErr.Message.ToString() +
                "<br><b>Stack Trace:</b><br>" + objErr.StackTrace.ToString();
            //Email sending method                      
            
            MailMessage message = new MailMessage("error.sale@pdgth.local", "ekarak.aro@pdgth.local");           
            message.IsBodyHtml = true;
            DateTime _dtNow = DateTime.Now;
            message.Subject = "Error (Do Not Reply Mail!!!!!) " + _dtNow.ToString();
            message.Body = err.ToString();

            SmtpClient smtp = new SmtpClient();
            smtp.Send(message);
            message.Dispose();
            smtp.Dispose();            

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
