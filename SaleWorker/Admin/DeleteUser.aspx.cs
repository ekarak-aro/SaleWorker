using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SaleWorker.ObjectClass;

namespace SaleWorker.Admin
{
    public partial class DeleteUser : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private String strConnStringAccpac = ConfigurationManager.ConnectionStrings["conAccpac"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (ViewState["searchUser"] != null)
                {
                    GetData();
                }
            }                            
        }
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        private void msgbx(string msg) 
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('"+ msg + "')", true);
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private void GetData()
        {
            ArrayList arr;
            if (ViewState["SelectedRecords"] != null)
                arr = (ArrayList)ViewState["SelectedRecords"];
            else
                arr = new ArrayList();            
            CheckBox chkAll = (CheckBox)gvUser.HeaderRow
                                .Cells[0].FindControl("chkAll");
            for (int i = 0; i < gvUser.Rows.Count; i++)
            {
                if (chkAll.Checked)
                {
                    if (!arr.Contains(gvUser.DataKeys[i].Value))
                    {
                        arr.Add(gvUser.DataKeys[i].Value);
                    }
                }
                else
                {
                    CheckBox chk = (CheckBox)gvUser.Rows[i]
                                       .Cells[0].FindControl("chk");
                    if (chk.Checked)
                    {
                        if (!arr.Contains(gvUser.DataKeys[i].Value))
                        {
                            arr.Add(gvUser.DataKeys[i].Value);
                        }
                    }
                    else
                    {
                        if (arr.Contains(gvUser.DataKeys[i].Value))
                        {
                            arr.Remove(gvUser.DataKeys[i].Value);
                        }
                    }
                }
            }
            ViewState["SelectedRecords"] = arr;
        }

        private void SetData()
        {
            int currentCount = 0;
            CheckBox chkAll = (CheckBox)gvUser.HeaderRow
                                    .Cells[0].FindControl("chkAll");
            chkAll.Checked = true;
            ArrayList arr = (ArrayList)ViewState["SelectedRecords"];
            for (int i = 0; i < gvUser.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)gvUser.Rows[i]
                                .Cells[0].FindControl("chk");
                if (chk != null)
                {
                    chk.Checked = arr.Contains(gvUser.DataKeys[i].Value);
                    if (!chk.Checked)
                        chkAll.Checked = false;
                    else
                        currentCount++;
                }
            }
            hfCount.Value = (arr.Count - currentCount).ToString();
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    
                    if (ddlType.SelectedValue == "1")
                    {
                        cmd.CommandText = "select * from webpages_Membership where ltrim(rtrim(fname)) like @search + '%' and status  = 'Active'";
                    }
                    else if (ddlType.SelectedValue == "2")
                    {
                        cmd.CommandText = "select * from webpages_Membership where ltrim(rtrim(email)) like @search + '%' and status = 'Active'";
                    }                                     
                    cmd.CommandType = CommandType.Text;
                    var _userEncrypt = new StringVarious();
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@search", SqlDbType.VarChar).Value = tbSearch.Text.Trim();                                     
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ViewState["searchUser"] = dt;
                        gvUser.DataSource = ViewState["searchUser"];
                        gvUser.DataBind();
                    }
                    else
                    {
                        gvUser.DataSource = null;
                        gvUser.DataBind();
                        ViewState.Remove("searchUser");
                        msgbx("Not found data");                        
                    }
                    dr.Close();
                }

            }
        }

        protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUser.PageIndex = e.NewPageIndex;
            gvUser.DataBind();
            SetData();
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            int count = 0;
            SetData();
            gvUser.AllowPaging = false;
            gvUser.DataSource = ViewState["searchUser"];
            gvUser.DataBind();
            ArrayList arr = (ArrayList)ViewState["SelectedRecords"];
            count = arr.Count;
            for (int i = 0; i < gvUser.Rows.Count; i++)
            {
                if (arr.Contains(gvUser.DataKeys[i].Value))
                {
                    DeleteRecord(gvUser.DataKeys[i].Value.ToString());
                    arr.Remove(gvUser.DataKeys[i].Value);
                }
            }
            ViewState["SelectedRecords"] = arr;
            hfCount.Value = "0";
            gvUser.AllowPaging = true;
            gvUser.DataSource = null;            
            //gvUser.DataSource = ViewState["searchUser"];
            gvUser.DataBind();
            ViewState.Remove("searchUser");
            //BindGrid();
            ShowMessage(count);
        }

        private void DeleteRecord(string userid)
        {

            string query = "update webpages_Membership set status = 'Deactive' ,editdate = getdate(),edituser = @edituser "  +
                            " where userid = @userid";
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Clear();
            Label _lbUser = this.Master.FindControl("lblName") as Label;
            cmd.Parameters.AddWithValue("@edituser", _lbUser.Text);
            cmd.Parameters.AddWithValue("@userid", userid);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();            
        }

        private void ShowMessage(int count)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("alert('");
            sb.Append(count.ToString());
            sb.Append(" records deleted.');");
            sb.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(),
                            "script", sb.ToString());
        }
    }
}