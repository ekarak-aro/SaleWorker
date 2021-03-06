﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Data;
using System.Configuration;


namespace SaleWorker
{
    public class SessionCheck : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Session["username"] == null)
            {                
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}