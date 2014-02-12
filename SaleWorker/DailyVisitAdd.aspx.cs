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
    public partial class DailyVisitAdd : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private String strConnStringAccpac = ConfigurationManager.ConnectionStrings["conAccpac"].ConnectionString;
        DataTable dtItem;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckWeeklyplanId();
            }
            if (this.ViewState["Item"] == null)
            {
                dtItem = new DataTable();
                dtItem.Columns.Add(new DataColumn("ITEMNO", typeof(string)));
                dtItem.Columns.Add(new DataColumn("desc", typeof(string)));
                dtItem.Columns.Add(new DataColumn("unit", typeof(string)));
                dtItem.Columns.Add(new DataColumn("stockunit", typeof(string)));
                dtItem.Columns.Add(new DataColumn("TypPresentItem", typeof(string)));
                dtItem.Columns.Add(new DataColumn("ReactionCus", typeof(string)));
                this.ViewState["Item"] = dtItem;
            }
            else
            {
                dtItem = (DataTable)this.ViewState["Item"];
            }
        }

        private void CheckWeeklyplanId()
        {
            if (Request.QueryString["WeeklyPlanId"] != null)
            {
                string id = Request.QueryString["WeeklyPlanId"];
                //msgbx(id);
                ImportData(id);
            }
            else
            {
                Response.Redirect("~/DailyVisitImp.aspx");
            }
        }

        private void ImportData(string _id)
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select docno,convert(nvarchar(10),plandate,103),saleid,salename,case isnewcust when 1 then 'Exists' when 2 then 'New' else 'Unknown' end TypeCust," +
                    " customerid,customername,districtid,districtname,provinceid,provincename" +
                    " from weeklyplan " +
                    " where recordstatus = 'active' and  id= @id ";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    var userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(_id);
                    try
                    {
                        conn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            tbWeeklyPlanNo.Text = dr[0].ToString();
                            tbPlanDate.Text = dr[1].ToString();
                            tbSaleId.Text = dr[2].ToString();
                            tbSaleName.Text = dr[3].ToString();
                            tbTypeCustomer.Text = dr[4].ToString();
                            tbCustId.Text = dr[5].ToString();
                            tbCustName.Text = dr[6].ToString();
                            tbDistrictId.Text = dr[7].ToString();
                            tbDistrictName.Text = dr[8].ToString();
                            tbProvinceId.Text = dr[9].ToString();
                            tbProvinceName.Text = dr[10].ToString();
                        }
                        dr.Close();
                        //if (dr.HasRows)
                        //{
                        //    DataTable dt = new DataTable();
                        //    dt.Load(dr);
                        //    //gvitem.DataSource = dt;
                        //    //gvitem.DataBind();
                        //}

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        protected void btSearchItem_Click(object sender, EventArgs e)
        {
            if (tbSearchItem.Text == "")
            {
                msgbx("Please fill data for search");
                return;
            }
            Page.Validate();
            if (Page.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(strConnStringAccpac))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        if (ddlItemSearchType.SelectedValue == "0")
                        {
                            cmd.CommandText = "select ITEMNO,[DESC],STOCKUNIT from icitem where INACTIVE = 0 and itemno like '%'+ @search + '%'";
                        }
                        else
                        {
                            cmd.CommandText = "select ITEMNO,[DESC],STOCKUNIT from icitem where INACTIVE = 0 and [DESC] like '%'+ @search + '%'";
                        }
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar).Value = tbSearchItem.Text;
                        try
                        {
                            conn.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                DataTable dt = new DataTable();
                                dt.Load(dr);
                                ViewState["SearchItem"] = dt;
                                gvSearchItem.DataSource = ViewState["SearchItem"];
                                gvSearchItem.DataBind();
                            }
                            else
                            {
                                msgbx("ไม่พบข้อมูล");
                                return;
                            }
                            dr.Close();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        private void msgbx(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
        }

        protected void gvSearchItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSearchItem.PageIndex = e.NewPageIndex;
            gvSearchItem.DataSource = ViewState["SearchItem"];
            gvSearchItem.DataBind();
        }

        protected void gvSearchItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int rowIndex = gvr.RowIndex;
                tbItemCode.Text = gvSearchItem.Rows[rowIndex].Cells[0].Text;
                lbItemDesc.Text = gvSearchItem.Rows[rowIndex].Cells[1].Text;
                lbUom.Text = gvSearchItem.Rows[rowIndex].Cells[2].Text;
            }
        }

        protected void btAddItem_Click(object sender, EventArgs e)
        {
            if (ddlTypeItem.SelectedValue == "0")
            {
                msgbx("กรุณาเลือกประเภทนำเสนอ");
                return;
            }
            else if (ddlReactionCus.SelectedValue == "0")
            {
                msgbx("กรุณาเลือกผลตอบรับ");
                return;
            }
            foreach (GridViewRow item in gvItem.Rows)
            {
                if (tbItemCode.Text.Trim() == item.Cells[0].Text.Trim())
                {
                    msgbx("รายการนี้มีการบันทึกแล้ว");
                    return;
                }
            }
            DataRow dr = dtItem.NewRow();
            dr["ITEMNO"] = tbItemCode.Text.Trim();
            dr["desc"] = lbItemDesc.Text.Trim();
            dr["unit"] = tbUnit.Text.Trim();
            dr["stockunit"] = lbUom.Text.Trim();
            dr["TypPresentItem"] = ddlTypeItem.SelectedItem.ToString().Trim();
            dr["ReactionCus"] = ddlReactionCus.SelectedItem.ToString().Trim();
            dtItem.Rows.Add(dr);

            gvItem.DataSource = dtItem;
            gvItem.DataBind();
            tbItemCode.Text = string.Empty;
            lbItemDesc.Text = "Item desc";
            tbUnit.Text = string.Empty;
            lbUom.Text = string.Empty;
            ddlTypeItem.SelectedIndex = 0;
            ddlReactionCus.SelectedIndex = 0;
        }

        protected void gvItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int rowIndex = gvr.RowIndex;               
                DataTable _dt = (DataTable)ViewState["Item"];
                var rowsToDelete = _dt.AsEnumerable()
                       .Where(r => r.Field<string>("Itemno") == gvItem.Rows[rowIndex].Cells[0].Text)
                       .ToList();
                foreach (var row in rowsToDelete.ToList())
                {
                    row.Delete();
                }                       
                ViewState["Item"] = _dt;
                gvItem.DataSource = _dt;                
                gvItem.DataBind();
            }
        }

        protected void gvItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

       
    }
}