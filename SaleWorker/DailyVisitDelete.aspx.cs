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

namespace SaleWorker
{
    public partial class DailyVisitDelete :  SessionCheck
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (ViewState["searchActual"] != null)
                {
                    GetData();
                }
            }
        }

        private void GetData()
        {
            ArrayList arr;
            if (ViewState["SelectedRecords"] != null)
                arr = (ArrayList)ViewState["SelectedRecords"];
            else
                arr = new ArrayList();
            CheckBox chkAll = (CheckBox)gvActual.HeaderRow
                                .Cells[0].FindControl("chkAll");
            for (int i = 0; i < gvActual.Rows.Count; i++)
            {
                if (chkAll.Checked)
                {
                    if (!arr.Contains(gvActual.DataKeys[i].Value))
                    {
                        arr.Add(gvActual.DataKeys[i].Value);
                    }
                }
                else
                {
                    CheckBox chk = (CheckBox)gvActual.Rows[i]
                                       .Cells[0].FindControl("chk");
                    if (chk.Checked)
                    {
                        if (!arr.Contains(gvActual.DataKeys[i].Value))
                        {
                            arr.Add(gvActual.DataKeys[i].Value);
                        }
                    }
                    else
                    {
                        if (arr.Contains(gvActual.DataKeys[i].Value))
                        {
                            arr.Remove(gvActual.DataKeys[i].Value);
                        }
                    }
                }
            }
            ViewState["SelectedRecords"] = arr;
        }

        private void msgbx(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            if (tbDateFrom.Text.Length != 10 || tbDateTo.Text.Length != 10)
            {
                msgbx("รูปแบบวันที่ไม่ถูกต้อง(dd/MM/yyy)");
                return;
            }
            var dateFrom = DateTime.Parse(tbDateFrom.Text);
            var dateTo = DateTime.Parse(tbDateTo.Text);
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int weekNumToday = cul.Calendar.GetWeekOfYear(
                DateTime.Now,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
            int weekNum = cul.Calendar.GetWeekOfYear(
                dateFrom,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
            var result = DateTime.Parse(tbDateFrom.Text).Year;
            var yearToday = DateTime.Now.Year;
            //if (dateFrom <= DateTime.Now)
            //{
            //    msgbx("วันที่(Date From)ต้องไม่น้อยกว่าหรือเท่ากับวันปัจจุบัน");
            //    return;
            //}
            if (dateFrom > dateTo)
            {
                msgbx("วันที่(Date To)ต้องไม่น้อยกว่าวันที่(Date From)");
                return;
            }
            Page.Validate();
            if (Page.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = " select a.id,a.docno,a.PlanDate,a.SaleId,a.SaleName,a.CustomerId,a.CustomerName from DailyVisit a left join webpages_Membership b" +
                            " on a.saleid = b.codeslsp " +
                            " where ( PlanDate between convert(date,@datefrom,103) and convert(date,@dateto,103)) " +
                            " and RecordStatus = 'Active' and useractivedirectory=@user";
                        cmd.CommandType = CommandType.Text;
                        var _userEncrypt = new StringVarious();
                        cmd.Parameters.Clear();
                        Label _lbUser = this.Master.FindControl("lblName") as Label;
                        cmd.Parameters.Add("@user", SqlDbType.NVarChar).Value = _userEncrypt.Encrypt(_lbUser.Text);
                        cmd.Parameters.Add("@datefrom", SqlDbType.VarChar).Value = tbDateFrom.Text;
                        cmd.Parameters.Add("@dateto", SqlDbType.VarChar).Value = tbDateTo.Text;
                        SqlDataReader dr;
                        conn.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            ViewState["searchActual"] = dt;
                            gvActual.DataSource = ViewState["searchActual"];
                            gvActual.DataBind();
                        }
                        else
                        {
                            gvActual.DataSource = null;
                            gvActual.DataBind();
                            ViewState.Remove("searchActual");
                            msgbx("Not found data");                           
                        }
                        dr.Close();
                    }

                }
            }
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            int count = 0;
            SetData();
            gvActual.AllowPaging = false;
            gvActual.DataSource = ViewState["searchActual"];
            gvActual.DataBind();
            ArrayList arr = (ArrayList)ViewState["SelectedRecords"];
            count = arr.Count;
            for (int i = 0; i < gvActual.Rows.Count; i++)
            {
                if (arr.Contains(gvActual.DataKeys[i].Value))
                {
                    DeleteRecord(gvActual.DataKeys[i].Value.ToString());
                    arr.Remove(gvActual.DataKeys[i].Value);
                }
            }
            ViewState["SelectedRecords"] = arr;
            hfCount.Value = "0";
            gvActual.AllowPaging = true;
            gvActual.DataSource = null;
            //gvUser.DataSource = ViewState["searchUser"];
            gvActual.DataBind();
            ViewState.Remove("searchActual");
            //BindGrid();
            ShowMessage(count);
        }

        private void SetData()
        {
            int currentCount = 0;
            CheckBox chkAll = (CheckBox)gvActual.HeaderRow
                                    .Cells[0].FindControl("chkAll");
            chkAll.Checked = true;
            ArrayList arr = (ArrayList)ViewState["SelectedRecords"];
            for (int i = 0; i < gvActual.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)gvActual.Rows[i]
                                .Cells[0].FindControl("chk");
                if (chk != null)
                {
                    chk.Checked = arr.Contains(gvActual.DataKeys[i].Value);
                    if (!chk.Checked)
                        chkAll.Checked = false;
                    else
                        currentCount++;
                }
            }
            hfCount.Value = (arr.Count - currentCount).ToString();
        }
        private void DeleteRecord(string id)
        {
            string query = "update DailyVisit set recordstatus = 'Inactive' ,dateedit = getdate(),useredit = @edituser " +
                           " where id = @id";
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Clear();
            Label _lbUser = this.Master.FindControl("lblName") as Label;
            cmd.Parameters.AddWithValue("@edituser", _lbUser.Text);
            cmd.Parameters.AddWithValue("@id", id);
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

        protected void gvActual_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //gvActual.DataSource = ViewState["searchActual"];
            //gvActual.DataBind();
            if (e.CommandName.Equals("detail"))
            {
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int rowIndex = gvr.RowIndex;
                int id = Convert.ToInt32(gvActual.DataKeys[rowIndex].Value);                
                string query = "select value,item,Type from  ReactionCustomer " +
                           " where IdDailyVisit = @id ";
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", rowIndex);
                con.Open();
                cblCustomer.ClearSelection();
                cblOther.ClearSelection();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        ListItem item = new ListItem();
                        item.Text = sdr["item"].ToString();
                        item.Value = sdr["value"].ToString();
                        if (sdr["Type"].ToString()=="Customer")
                        {
                            foreach (ListItem i in cblCustomer.Items)
                            {
                                if (item.Value == i.Value)
                                {
                                    i.Selected = true;
                                }
                            }
                        }
                        else if (sdr["Type"].ToString() == "Other")
                        {
                            foreach (ListItem i in cblOther.Items)
                            {
                                if (item.Value == i.Value)
                                {
                                    i.Selected = true;
                                }
                            }
                        }
                       
                        //item.Selected = Convert.ToBoolean(sdr["IsSelected"]);
                        //chkHobbies.Items.Add(item);
                    }
                }

                query = "select itemno,[desc],qty,stockunit,TypeOfPre as TypPresentItem,TypeOfPost as ReactionCus from  ReactionItem " +
                           " where IdDailyVisit = @id ";
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;               
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);                   
                    gvItem.DataSource = dt;
                    gvItem.DataBind();
                }
                else
                {
                    gvItem.DataSource = null;
                    gvItem.DataBind();                   
                }
                con.Close();
              

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#modalDetail').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                               "ModalScript", sb.ToString(), false);
            }
        }
       
        
    }
}