﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SaleWorker.ObjectClass;

namespace SaleWorker.ReportUser
{
    public partial class ReportDailyVisit : SessionCheck
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private String strConnStringAccpac = ConfigurationManager.ConnectionStrings["conAccpac"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CallSale();
            }
        }

        private bool CheckRoleUser()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select roleid from webpages_UsersInRoles where userid = (select userid from webpages_Membership where username = @user )" +
                        " and roleid in(2,3)";
                    cmd.Parameters.Clear();
                    StringVarious str = new StringVarious();
                    cmd.Parameters.Add("@user", SqlDbType.NVarChar).Value = str.Encrypt(Session["username"].ToString());
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    int i = 0;
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            i++;
                        }
                        if (i > 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void CallSale()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    if (CheckRoleUser())
                    {
                        cmd.CommandText = "select ltrim(rtrim(codeslsp)) as CODESLSP ,NAMEEMPL from [172.16.20.22].[PD_DAT].dbo.arsap where swactv = 1 and " +
                            " codeslsp collate thai_bin in (select codeslsp from webpages_Membership where department = (select department from webpages_Membership where username = @user ))";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        StringVarious str = new StringVarious();
                        cmd.Parameters.Add("@user", SqlDbType.NVarChar).Value = str.Encrypt(Session["username"].ToString());
                    }
                    else
                    {
                        cmd.CommandText = "select ltrim(rtrim(CODESLSP)) as CODESLSP,NAMEEMPL from [172.16.20.22].[PD_DAT].dbo.arsap where SWACTV = 1 order by CODESLSP";
                        cmd.CommandType = CommandType.Text;
                    }


                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ddlSale.DataSource = dt;
                        ddlSale.DataTextField = "NAMEEMPL";
                        ddlSale.DataValueField = "CODESLSP";
                        ddlSale.DataBind();

                        //if (Session["SaleId"].ToString() != null)
                        //{
                        //    ddlSale.SelectedValue = Session["SaleId"].ToString();                            
                        //}
                        //else
                        //{
                        //    ddlSale.Items.Insert(0, "กรุณาเลือก");
                        //    ddlSale.SelectedIndex = 0;
                        //}

                        ddlSale.Items.Insert(0, "กรุณาเลือก");
                        ddlSale.SelectedIndex = 0;

                    }
                    dr.Close();
                }

            }
        }

        private void msgbx(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
        }

        protected void btSearchData_Click(object sender, EventArgs e)
        {
            if (tbDateFrom.Text.Length != 10 || tbDateTo.Text.Length != 10)
            {
                msgbx("รูปแบบวันที่ไม่ถูกต้อง(dd/MM/yyy)");
                return;
            }
            if (ddlSale.SelectedIndex == 0)
            {
                if (!string.IsNullOrEmpty((string)Session["SaleId"]))
                {
                    msgbx("กรุณาเลือกพนักงาน");
                    return;
                }
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
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    string _strText;
                    _strText = " select * " +
                           " from v_daily" +
                           " where ( actualdate between convert(date,@datefrom,103) and convert(date,@dateto,103))";

                    if (ddlSale.SelectedIndex == 0)
                    {
                        _strText = _strText + " order by actualdate desc";
                    }
                    else
                    {
                        _strText = _strText + " and saleid = @saleID order by actualdate desc";
                    }
                    cmd.CommandText = _strText;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@dateFrom", SqlDbType.NVarChar).Value = tbDateFrom.Text;
                    cmd.Parameters.Add("@dateTo", SqlDbType.NVarChar).Value = tbDateTo.Text;
                    if (ddlSale.SelectedIndex != 0)
                    {
                        cmd.Parameters.Add("@saleID", SqlDbType.NVarChar).Value = ddlSale.SelectedValue;
                    }
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ViewState["tableSearchitem"] = dt;
                        gvItem.DataSource = dt;
                        gvItem.DataBind();
                        reportViewer();
                    }
                    else
                    {
                        gvItem.DataSource = null;
                        gvItem.DataBind();
                        ViewState.Remove("tableSearchitem");
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.Refresh();
                        msgbx("Nodata found");
                    }
                    dr.Close();

                }

            }
        }

        private void reportViewer()
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/DailyVisit/DailyVisit.rdlc");
            DataSet ds = new DataSet();
            ds.Tables.Add((DataTable)ViewState["tableSearchitem"]);
            ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);
            ReportViewer1.AsyncRendering = false;
            ReportViewer1.SizeToReportContent = false;
            ReportViewer1.LocalReport.Refresh();

        }

        protected void gvItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvItem.PageIndex = e.NewPageIndex;
            gvItem.DataSource = ViewState["tableSearchitem"];
            gvItem.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }


        protected void btExcel_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "GridviewExport.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvItem.AllowPaging = false;
            gvItem.DataSource = ViewState["tableSearchitem"];
            gvItem.DataBind();
            //Change the Header Row back to white color
            gvItem.HeaderRow.Style.Add("background-color", "#FFFFFF");
            //Applying stlye to gridview header cells
            for (int i = 0; i < gvItem.HeaderRow.Cells.Count; i++)
            {
                gvItem.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
            }
            int j = 1;
            foreach (GridViewRow gvrow in gvItem.Rows)
            {
                gvrow.BackColor = System.Drawing.Color.White;
                if (j <= gvItem.Rows.Count)
                {
                    if (j % 2 != 0)
                    {
                        for (int k = 0; k < gvrow.Cells.Count; k++)
                        {
                            gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
                        }
                    }
                }
                j++;
            }
            gvItem.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

        }
    }
}