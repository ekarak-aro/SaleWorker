using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SaleWorker.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblName.Text = Convert.ToString(Session["username"]);
            //Session["LoginName"] = "Ekarak.aro";
            //if (Session["LoginName"] == null)
            //{
            //    Response.Redirect("Login.aspx");
            //}
            //else
            //{
            //    lblName.Text = "Welcome:: " + Convert.ToString(Session["LoginName"]);
            //} 
        }
    }
}