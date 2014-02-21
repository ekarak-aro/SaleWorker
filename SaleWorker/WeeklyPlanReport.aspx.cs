using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace SaleWorker
{
    public partial class WeeklyPlanReport : SessionCheck
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
        private void CallSale()
        {
            using (SqlConnection conn = new SqlConnection(strConnStringAccpac))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select ltrim(rtrim(CODESLSP)) as CODESLSP,NAMEEMPL from arsap where SWACTV = 1 order by CODESLSP";
                    cmd.CommandType = CommandType.Text;
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

                        if (Session["SaleId"].ToString() != null)
                        {
                            ddlSale.SelectedValue = Session["SaleId"].ToString();
                            ddlSale.Enabled = false;
                        }
                        else
                        {
                            ddlSale.Items.Insert(0, "กรุณาเลือก");
                            ddlSale.SelectedIndex = 0;
                        }

                        //ddlSale.Items.Insert(0, "กรุณาเลือก");
                        //ddlSale.SelectedIndex = 0;

                    }
                    dr.Close();
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
        }

        protected void btSearchData_Click(object sender, EventArgs e)
        {
            if (tbDateFrom.Text.Length != 10 || tbDateTo.Text.Length != 10)
            {
                msgbx("รูปแบบวันที่ไม่ถูกต้อง(dd/MM/yyy)");
                return;
            }
            //var dateFrom = DateTime.Parse(tbDateFrom.Text);
            //var dateTo = DateTime.Parse(tbDateTo.Text);
            //System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            //int weekNumToday = cul.Calendar.GetWeekOfYear(
            //    DateTime.Now,
            //    System.Globalization.CalendarWeekRule.FirstFourDayWeek,
            //    DayOfWeek.Monday);
            //int weekNum = cul.Calendar.GetWeekOfYear(
            //    dateFrom,
            //    System.Globalization.CalendarWeekRule.FirstFourDayWeek,
            //    DayOfWeek.Monday);
            //var result = DateTime.Parse(tbDateFrom.Text).Year;
            //var yearToday = DateTime.Now.Year;
            //if (dateFrom <= DateTime.Now)
            //{
            //    msgbx("วันที่(Date From)ต้องไม่น้อยกว่าหรือเท่ากับวันปัจจุบัน");
            //    return;
            //}
            //if (dateFrom > dateTo)
            //{
            //    msgbx("วันที่(Date To)ต้องไม่น้อยกว่าวันที่(Date From)");
            //    return;
            //}

            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    string _strText;
                    _strText = " select docno,plandate,typecustomer, " +                          
                          " saleid,salename,CustomerId,CustomerName,DistrictName,ProvinceName,Remark"+
                           " from v_weeklyplan" +
                           " where ( PlanDate between convert(date,@datefrom,103) and convert(date,@dateto,103))";

                    if (ddlSale.SelectedIndex == 0)
                    {
                        _strText = _strText + " order by plandate desc";
                    }
                    else
                    {
                        _strText = _strText + " and saleid = @saleID order by plandate desc";
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
                        MessageBox("Nodata found");
                    }
                    dr.Close();

                }

            }
        }

        private void reportViewer()
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/WeeklyPlan/WeeklyPlan.rdlc");
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
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvItem.AllowPaging = false;
                gvItem.DataSource = ViewState["tableSearchitem"];
                gvItem.DataBind();
                //this.BindGrid();

                gvItem.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvItem.HeaderRow.Cells)
                {
                    cell.BackColor = gvItem.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvItem.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvItem.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvItem.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvItem.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

            }

        
        }


    }
}