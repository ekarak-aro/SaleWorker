using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SaleWorker
{
    public partial class DailyVisitAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckWeeklyplanId();
            }
        }

        private void CheckWeeklyplanId()
        {
            if (Request.QueryString["WeeklyPlanId"]!=null)
            {
                string id = Request.QueryString["WeeklyPlanId"];
                msgbx(id);
                //ImportData();
            }
            else
            {
                Response.Redirect("~/DailyVisitImp.aspx");
            }
        }

        private void ImportData()
        {
            throw new NotImplementedException();
        }
        private void msgbx(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
        }
    }
}