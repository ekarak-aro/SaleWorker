using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SaleWorker.ObjectClass;

namespace SaleWorker
{
    public partial class DailyVisitImp : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private String strConnStringAccpac = ConfigurationManager.ConnectionStrings["conAccpac"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetItem();
            }
        }

        private void GetItem()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select distinct a.id,a.docno,a.plandate,a.saleid,a.salename,a.customerid,a.customername from weeklyplan a left join webpages_Membership b " +
                    " on a.saleid = b.CodeSLSP " +
                    " where recordstatus = 'active' and a.plandate <= convert(date,getdate()) and b.useractivedirectory = @user and a.plandate >= convert(date,DATEADD(m,-1,getdate())) ";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    var userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@user", SqlDbType.NVarChar).Value = userEncrypt.Encrypt(Session["username"].ToString());
                    try
                    {
                        conn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            gvitem.DataSource = dt;
                            gvitem.DataBind();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        protected void gvitem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvitem.PageIndex = e.NewPageIndex;
            GetItem();
        }

        protected void gvitem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int rowIndex = gvr.RowIndex;
                int documentID = Convert.ToInt32(gvitem.DataKeys[rowIndex].Value);
                Response.Redirect("~/DailyVisitAdd.aspx?WeeklyPlanId="+ documentID.ToString());
                //msgbx(documentID.ToString());
            }
        }

        private void msgbx(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
        }
    }
}