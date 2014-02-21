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
    public partial class DailyVisitAdd : SessionCheck
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
                        //if (dr.HasRows)
                        //{
                        //    DataTable dt = new DataTable();
                        //    dt.Load(dr);
                        //    tbWeeklyPlanNo.Text = dt.Rows[0][0].ToString();
                        //    tbPlanDate.Text = dt.Rows[0][1].ToString();
                        //    tbSaleId.Text = dt.Rows[0][2].ToString();
                        //    tbSaleName.Text = dt.Rows[0][3].ToString();
                        //    tbTypeCustomer.Text = dt.Rows[0][4].ToString();
                        //    tbCustId.Text = dt.Rows[0][5].ToString();
                        //    tbCustName.Text = dt.Rows[0][6].ToString();
                        //    tbDistrictId.Text = dt.Rows[0][7].ToString();
                        //    tbDistrictName.Text = dt.Rows[0][8].ToString();
                        //    tbProvinceId.Text = dt.Rows[0][9].ToString();
                        //    tbProvinceName.Text = dt.Rows[0][10].ToString();
                        //}

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
                            ViewState["weeklyplandocno"] = dr[0].ToString();
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

        protected void btSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "new_dailyVisit";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        Label _lbUser = this.Master.FindControl("lblName") as Label;
                        cmd.Parameters.Add("@usercreate", SqlDbType.NVarChar).Value = _lbUser.Text;
                        cmd.Parameters.Add("@useredit", SqlDbType.NVarChar).Value = _lbUser.Text;
                        cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = tbRemark.Text;
                        cmd.Parameters.Add("@weeklyplanDocNo", SqlDbType.NVarChar).Value = ViewState["weeklyplandocno"].ToString();                       
                        try
                        {
                            conn.Open();                            
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());
                            foreach (ListItem item in cblCustomer.Items)
                            {
                                if (item.Selected)
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "insert into ReactionCustomer(datecreate,dateedit,usercreate,useredit,value,item,idDailyvisit,Type)" +
                                    " values(getdate(),getdate(),@usercreateCus,@usereditCus,@valueCus,@itemCus,@idDailyvisitCus,@TypeCus)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@usercreateCus", SqlDbType.NVarChar).Value = _lbUser.Text;
                                    cmd.Parameters.Add("@usereditCus", SqlDbType.NVarChar).Value = _lbUser.Text;
                                    cmd.Parameters.Add("@valueCus", SqlDbType.NVarChar).Value = item.Value;
                                    cmd.Parameters.Add("@itemCus", SqlDbType.NVarChar).Value = item.Text;
                                    cmd.Parameters.Add("@idDailyvisitCus", SqlDbType.Int).Value = newId;
                                    cmd.Parameters.Add("@TypeCus", SqlDbType.NVarChar).Value = "Customer";                                    
                                    //cmd.ExecuteNonQuery();
                                }
                            }

                            foreach (ListItem item in cblOther.Items)
                            {
                                if (item.Selected)
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "insert into ReactionCustomer(datecreate,dateedit,usercreate,useredit,value,item,idDailyvisit,Type)" +
                                    " values(getdate(),getdate(),@usercreateOth,@usereditOth,@valueOth,@itemOth,@idDailyvisitOth,@TypeOth)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@usercreateOth", SqlDbType.NVarChar).Value = _lbUser.Text;
                                    cmd.Parameters.Add("@usereditOth", SqlDbType.NVarChar).Value = _lbUser.Text;
                                    cmd.Parameters.Add("@valueOth", SqlDbType.NVarChar).Value = item.Value;
                                    cmd.Parameters.Add("@itemOth", SqlDbType.NVarChar).Value = item.Text;
                                    cmd.Parameters.Add("@idDailyvisitOth", SqlDbType.Int).Value = newId;
                                    cmd.Parameters.Add("@TypeOth", SqlDbType.NVarChar).Value = "Other";
                                    //cmd.ExecuteNonQuery();
                                }
                            }
                            if (gvItem.Rows.Count!=0)
                            {
                                foreach (GridViewRow item in gvItem.Rows)
                                {
                                    cmd.CommandType = CommandType.Text;                                    
                                    cmd.CommandText = "insert into ReactionItem(datecreate,dateedit,usercreate,useredit,itemno,[desc],qty,stockunit,typeofpre,typeofpost,idDailyvisit)" +
                                        " values (getdate(),getdate(),@usercreateItm,@usereditItm,@itemno,@desc,@qty,@stockunit,@typeofpre,@typeofpost,@idDailyvisitItm) ";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@usercreateItm", SqlDbType.NVarChar).Value = _lbUser.Text;
                                    cmd.Parameters.Add("@usereditItm", SqlDbType.NVarChar).Value = _lbUser.Text;
                                    cmd.Parameters.Add("@itemno", SqlDbType.NVarChar).Value = item.Cells[0].Text;
                                    cmd.Parameters.Add("@desc", SqlDbType.NVarChar).Value = item.Cells[1].Text;
                                    cmd.Parameters.Add("@qty", SqlDbType.Decimal).Value = item.Cells[2].Text;
                                    cmd.Parameters.Add("@stockunit", SqlDbType.NVarChar).Value = item.Cells[3].Text;
                                    cmd.Parameters.Add("@typeofpre", SqlDbType.NVarChar).Value = item.Cells[4].Text;
                                    cmd.Parameters.Add("@typeofpost", SqlDbType.NVarChar).Value = item.Cells[5].Text;
                                    cmd.Parameters.Add("@idDailyvisitItm", SqlDbType.Int).Value = newId;                                    
                                    //cmd.ExecuteNonQuery();
                                }
                            }
                            ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Add data complete!');window.location='DailyVisitImp.aspx';</script>'");
                            //msgbx("Add data complete");
                            //Response.Redirect("~/DailyVisitImp.aspx");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

       
    }
}