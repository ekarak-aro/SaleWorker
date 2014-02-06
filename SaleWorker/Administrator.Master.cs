using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SaleWorker
{
    public partial class Administrator : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["LoginName"] = "Ekarak.aro";
            if (Session["LoginName"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                lblName.Text = "Welcome:: " + Convert.ToString(Session["LoginName"]);
            } 
        }
    }
}