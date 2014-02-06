using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SaleWorker.Error
{
    public partial class Error404 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            Response.AppendHeader("Refresh", "10; URL=" + this.ResolveUrl("~/Login.aspx"));
        }
    }
}