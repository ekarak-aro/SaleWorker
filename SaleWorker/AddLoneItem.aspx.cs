using SaleWorker.ObjectClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SaleWorker
{

    public partial class AddLoneItem : SessionCheck
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private String strConnStringAccpac = ConfigurationManager.ConnectionStrings["conAccpac"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            btSendData.Attributes.Add("onClick", "document.body.style.cursor = 'wait';");
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

                    }
                    dr.Close();
                }

            }
        }

        protected void Onclick(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(strConnStringAccpac))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select top 1 IDCUST,NAMECUST,AMTCRLIMT - AMTBALDUEH as credit   from arcus where IDCUST = @idCust and swactv = 1";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@idCust", SqlDbType.NVarChar).Value = tbCusCode.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        lbCustName.Text = dt.Rows[0][1].ToString();
                        lbCustName.ForeColor = System.Drawing.Color.Black;
                        lbCredit.Text = string.Format("{0:F2}", Convert.ToDecimal(dt.Rows[0][2].ToString()));
                        //lbCredit.Text = dt.Rows[0][2].ToString();
                        lbCredit.ForeColor = System.Drawing.Color.Blue;
                    }
                    else
                    {
                        lbCustName.Text = "Customer Desc Not Found";
                        lbCustName.ForeColor = System.Drawing.Color.Red;
                        tbCusCode.Text = "";
                        lbCredit.Text = "0";
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


        protected void gvItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["ChooseItem"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["ChooseItem"] = dt;
            gvItem.DataSource = ViewState["ChooseItem"];
            gvItem.DataBind();
            CalculatePrice();
        }



        protected void btSendData_Click(object sender, EventArgs e)
        {

            if (ViewState["ChooseItem"] == null)
            {
                //MessageBox("Please click calculate button");
                msgbx("Please click calculate button");
                return;
            }
            if (Convert.ToDecimal(lbCredit.Text) < Convert.ToDecimal(Label3.Text))
            {
                //MessageBox("Please check credit limit");
                msgbx("Please check credit limit");
                return;
            }
            if (ViewState["ChooseItem"] != null && Label4.Text == "0")
            {
                //MessageBox("Please click calculate button or fill UOM");
                msgbx("Please click calculate button or fill UOM");
                return;
            }

            SaveData();
        }

        private void SaveData()
        {
            DataTable dt = (DataTable)ViewState["ChooseItem"];
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Insert into Loan_Items (DateCreate,DateEdit,UserCreate,UserEdit,RecordStatus,SaleId,SaleName,CustomerId,CustomerName,TotalPrice,CreaditLimit)" +
                         " OUTPUT INSERTED.Id " +
                         " values(@DateCreate,@DateEdit,@UserCreate,@UserEdit,@RecordStatus,@SaleId,@SaleName,@CustomerId,@CustomerName,@TotalPrice,@CreaditLimit)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@DateCreate", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@DateEdit", SqlDbType.DateTime).Value = DateTime.Now;
                    Label _lbUser = this.Master.FindControl("lblName") as Label;
                    cmd.Parameters.Add("@UserCreate", SqlDbType.NVarChar).Value = _lbUser.Text;
                    cmd.Parameters.Add("@UserEdit", SqlDbType.NVarChar).Value = _lbUser.Text;
                    cmd.Parameters.Add("@RecordStatus", SqlDbType.NVarChar).Value = "Yes";
                    cmd.Parameters.Add("@SaleId", SqlDbType.NVarChar).Value = ddlSale.SelectedValue.ToString().Trim();
                    cmd.Parameters.Add("@SaleName", SqlDbType.NVarChar).Value = ddlSale.SelectedItem.ToString().Trim();
                    cmd.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = tbCusCode.Text;
                    cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = lbCustName.Text;
                    cmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(ViewState["SumTotalPrice"].ToString());
                    cmd.Parameters.Add("@CreaditLimit", SqlDbType.Decimal).Value = lbCredit.Text;
                    conn.Open();
                    try
                    {
                        Int32 newId = (Int32)cmd.ExecuteScalar();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            cmd.CommandText = "insert into Loan_Item_detail values(@DateCreate,@DateEdit,@UserCreate,@UserEdit,@RecordStatus, " +
                                " @IdLoanItem,@ItemCode,@ItemDesc,@Unit,@Uom,@UnitPrice,@TotalPrice,@location)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("@DateCreate", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@DateEdit", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@UserCreate", SqlDbType.NVarChar).Value = _lbUser.Text;
                            cmd.Parameters.Add("@UserEdit", SqlDbType.NVarChar).Value = _lbUser.Text;
                            cmd.Parameters.Add("@RecordStatus", SqlDbType.NVarChar).Value = "Yes";
                            cmd.Parameters.Add("@IdLoanItem", SqlDbType.Int).Value = newId;
                            cmd.Parameters.Add("@ItemCode", SqlDbType.NVarChar).Value = dt.Rows[i]["Col1"].ToString();
                            cmd.Parameters.Add("@ItemDesc", SqlDbType.NVarChar).Value = dt.Rows[i]["Col2"].ToString();
                            cmd.Parameters.Add("@Unit", SqlDbType.Int).Value = dt.Rows[i]["UOM"].ToString();
                            cmd.Parameters.Add("@Uom", SqlDbType.NVarChar).Value = dt.Rows[i]["Col3"].ToString();
                            cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = dt.Rows[i]["Col4"].ToString();
                            cmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = dt.Rows[i]["TotalPrice"].ToString();
                            cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = dt.Rows[i]["LOCATION"].ToString();
                            cmd.ExecuteNonQuery();
                        }


                        //Thread email = new Thread(delegate() 
                        //{
                        SendAlertMail();
                        //});
                        //email.IsBackground = true;
                        //email.Start();


                        //This one call clear method to clear all control values
                        clear();
                        //msgbx("Add data Complete");
                        MessageBox("Add data Complete");


                    }
                    catch (Exception ex)
                    {
                        MessageBox("Please contact IT " + ex.Message.ToString());
                    }

                }
            }
        }

        private string getHTML(GridView gv)
        {
            StringBuilder sb = new StringBuilder();
            //StringWriter textwriter = new StringWriter(sb);
            //HtmlTextWriter htmlwriter = new HtmlTextWriter(textwriter);
            //gv.RenderControl(htmlwriter);
            //htmlwriter.Flush();
            //textwriter.Flush();
            //htmlwriter.Dispose();
            //textwriter.Dispose();

            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    gv.RenderControl(hw);
                    return sb.ToString();
                }
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private string getEmail()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select email from webpages_Membership where status = 'Active' and useractivedirectory = @UserName ";
                    cmd.Parameters.Clear();
                    var _userEncrypt = new StringVarious();
                    Label _lbUser = this.Master.FindControl("lblName") as Label;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(_lbUser.Text);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        return dt.Rows[0][0].ToString().Trim();
                    }
                    dr.Close();
                }
            }
            return null;
        }

        private void SendAlertMail()
        {
            StringBuilder _body = new StringBuilder();
            _body.Append("<h2/>");
            _body.Append("<b>การทดสอบระบบแจ้งใบคำขอยืมสินค้าจากพนักงานขาย (Test) </b>");
            _body.Append("</h2>");
            _body.Append("<br/>");
            _body.Append("<b>ชื่อบริษัท : </b>");
            _body.Append(lbCustName.Text.Trim() + "(" + tbCusCode.Text.Trim() + ")");
            _body.Append("<br/>");
            _body.Append("<b>ชื่อผู้ขอยืม : </b>");
            _body.Append(ddlSale.SelectedItem.ToString() + "(" + ddlSale.SelectedValue.ToString() + ")");
            _body.Append("<br/>");
            _body.Append("<b>รวมเป็นเงินทั้งสิ้น : <b>");
            _body.Append(ViewState["SumTotalPrice"].ToString());
            _body.Append("<br/>");
            _body.Append(getHTML(gvItem));
            string fromEmail;
            ViewState["getEmail"] = getEmail();
            if (ViewState["getEmail"] != null)
            {
                fromEmail = ViewState["getEmail"].ToString();
            }
            else
            {
                fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
            }

            string toEmail = ConfigurationManager.AppSettings["EmailLoanItem"].ToString();
            MailMessage message = new MailMessage(fromEmail, toEmail);
            message.CC.Add(ConfigurationManager.AppSettings["EmailITPerson"].ToString());
            message.CC.Add(fromEmail);
            message.IsBodyHtml = true;
            DateTime _dtNow = DateTime.Now;
            message.Subject = "ทดสอบระบบ แจ้งรายการขอยืมสินค้า (Do Not Reply Mail!!!!!) " + _dtNow.ToString();
            message.Body = _body.ToString();

            SmtpClient smtp = new SmtpClient();
            smtp.Send(message);
            message.Dispose();
            smtp.Dispose();

        }

        private void clear()
        {
            //This loop takes all controls from the form1 - make sure your form name as form1 otherwise change it as per your form name
            //foreach (Control c in form1.Controls)
            //{
            //    //Clear all textbox values
            //    if (c is TextBox)
            //        ((TextBox)c).Text = "";

            //    //clear all check boxes
            //    //if (c is CheckBox)
            //    //    ((CheckBox)c).Checked = false;

            //    //Clear all radio buttons
            //    //if (c is RadioButton)
            //    //    ((RadioButton)c).Checked = false;

            //    //Clear all radio buttons
            //    if (c is DropDownList)
            //        ((DropDownList)c).SelectedIndex = 0;
            //}
            ddlSale.SelectedIndex = 0;
            lbCredit.Text = "0";
            Label3.Text = "0";
            Label4.Text = "0";
            tbCusCode.Text = "";
            ddlSale.SelectedValue = Session["SaleId"].ToString();
            lbCustName.Text = "Customer Des";
            gvItem.DataSource = null;
            gvItem.DataBind();
            ViewState["ChooseItem"] = null;
            //cblRole.SelectedValue = "2";
        }

        protected void btSearchItem_Click(object sender, EventArgs e)
        {
            if (tbItemSearch.Text.Length == 0)
            {
                msgbx("Please fill data");
                return;
            }
            using (SqlConnection conn = new SqlConnection(strConnStringAccpac))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    if (ddlSearchTypeItem.SelectedValue == "ItemCode")
                    {
                        cmd.CommandText = "select a.itemno,descrip,uom,price,b.LOCATION,(b.QTYONHAND-b.QTYSHNOCST)+(b.QTYRENOCST - b.QTYCOMMIT) as stockUnit from  Pricelist.dbo.T_Pricelist a left join iciloc b on " +
                            " a.ITEMNO collate thai_bin=b.ITEMNO collate thai_bin" +
                            " where LOCATION = @location " +
                            " and (QTYONHAND-QTYSHNOCST)+(QTYRENOCST - QTYCOMMIT) > 0 and a.itemno like  @search + '%'";
                    }
                    else if (ddlSearchTypeItem.SelectedValue == "ItemDesc")
                    {
                        cmd.CommandText = "select a.itemno,descrip,uom,price,b.LOCATION,(b.QTYONHAND-b.QTYSHNOCST)+(b.QTYRENOCST - b.QTYCOMMIT) as stockUnit from  Pricelist.dbo.T_Pricelist a left join iciloc b on " +
                            " a.ITEMNO collate thai_bin=b.ITEMNO collate thai_bin" +
                            " where LOCATION = @location " +
                            " and (QTYONHAND-QTYSHNOCST)+(QTYRENOCST - QTYCOMMIT) > 0 and a.descrip like  @search + '%'";

                    }

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = ddlLocation.SelectedValue;
                    cmd.Parameters.Add("@search", SqlDbType.NVarChar).Value = tbItemSearch.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ViewState["tableSearchitem"] = dt;
                        gvSearchItem.DataSource = ViewState["tableSearchitem"];
                        gvSearchItem.DataBind();
                    }
                    else
                    {
                        msgbx("ไม่พบของใน stock สินค้า");
                        //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Not found data" + "');", true);                   
                        gvSearchItem.DataSource = null;
                        gvSearchItem.DataBind();
                    }
                    dr.Close();
                }
            }
        }



        protected void btCloseItem_Click(object sender, EventArgs e)
        {
            ddlSearchTypeItem.SelectedValue = "ItemCode";
            tbItemSearch.Text = null;
            gvSearchItem.DataSource = null;
            gvSearchItem.DataBind();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script>$('#modalItem').modal('hide');</script>", false);
        }

        protected void gvSearchItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            int rowIndex = gvr.RowIndex;
            if (e.CommandName == "select")
            {
                if (ViewState["ChooseItem"] == null)
                {
                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    dt.Columns.Add(new DataColumn("Col1", typeof(string)));
                    dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                    dt.Columns.Add(new DataColumn("UOM", typeof(string)));
                    dt.Columns.Add(new DataColumn("Col3", typeof(string)));
                    dt.Columns.Add(new DataColumn("Col4", typeof(string)));
                    dt.Columns.Add(new DataColumn("LOCATION", typeof(string)));
                    dt.Columns.Add(new DataColumn("TotalPrice", typeof(int)));
                    dt.Columns.Add(new DataColumn("DeleteButton", typeof(string)));
                    dr = dt.NewRow();
                    dr["Col1"] = gvSearchItem.Rows[rowIndex].Cells[0].Text;
                    dr["Col2"] = gvSearchItem.Rows[rowIndex].Cells[1].Text;
                    dr["Col3"] = gvSearchItem.Rows[rowIndex].Cells[2].Text;
                    dr["Col4"] = gvSearchItem.Rows[rowIndex].Cells[3].Text;
                    dr["LOCATION"] = gvSearchItem.Rows[rowIndex].Cells[5].Text;
                    dr["DeleteButton"] = string.Empty;
                    dr["UOM"] = 0;
                    dr["TotalPrice"] = 0;
                    dt.Rows.Add(dr);
                    ViewState["ChooseItem"] = dt;
                }
                else
                {
                    DataTable dt = (DataTable)ViewState["ChooseItem"];
                    DataRow dr = null;
                    dr = dt.NewRow();
                    dr["Col1"] = gvSearchItem.Rows[rowIndex].Cells[0].Text;
                    dr["Col2"] = gvSearchItem.Rows[rowIndex].Cells[1].Text;
                    dr["Col3"] = gvSearchItem.Rows[rowIndex].Cells[2].Text;
                    dr["Col4"] = gvSearchItem.Rows[rowIndex].Cells[3].Text;
                    dr["LOCATION"] = gvSearchItem.Rows[rowIndex].Cells[5].Text;
                    dr["DeleteButton"] = string.Empty;
                    dr["UOM"] = 0;
                    dr["TotalPrice"] = 0;
                    dt.Rows.Add(dr);
                    ViewState["ChooseItem"] = dt;
                }
                gvItem.DataSource = ViewState["ChooseItem"];
                gvItem.DataBind();

            }
        }

        protected void gvSearchItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSearchItem.PageIndex = e.NewPageIndex;
            gvSearchItem.DataSource = ViewState["tableSearchitem"];
            gvSearchItem.DataBind();
        }

        protected void btCalculate_Click(object sender, EventArgs e)
        {
            CalculatePrice();
        }

        private void CalculatePrice()
        {

            DataTable dt = new DataTable();
            if (gvItem.HeaderRow != null)
            {
                dt.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt.Columns.Add(new DataColumn("UOM", typeof(string)));
                dt.Columns.Add(new DataColumn("Col3", typeof(string)));
                dt.Columns.Add(new DataColumn("Col4", typeof(string)));
                dt.Columns.Add(new DataColumn("LOCATION", typeof(string)));
                dt.Columns.Add(new DataColumn("TotalPrice", typeof(int)));
                dt.Columns.Add(new DataColumn("DeleteButton", typeof(string)));
            }
            foreach (GridViewRow row in gvItem.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "TotalPrice")
                    {
                        TextBox TextBoxUom = (TextBox)row.Cells[3].FindControl("tbGridViewUom");
                        int _uom = Convert.ToInt32(TextBoxUom.Text);
                        int _price = Convert.ToInt32(row.Cells[4].Text);
                        dr[i] = (_uom * _price);
                    }
                    else if (dt.Columns[i].ColumnName == "UOM")
                    {
                        TextBox TextBoxUom = (TextBox)row.Cells[3].FindControl("tbGridViewUom");
                        dr[i] = TextBoxUom.Text;
                    }
                    else
                    {
                        dr[i] = row.Cells[i].Text.Replace(" ", "");
                    }

                }
                dt.Rows.Add(dr);
            }

            var _sumTotalPrice = dt.AsEnumerable().Sum(x => x.Field<int>("TotalPrice"));
            ViewState["SumTotalPrice"] = _sumTotalPrice;
            Label3.Text = string.Format("{0:F2}", _sumTotalPrice.ToString());
            //Label3.Text = _sumTotalPrice.ToString();
            Label3.ForeColor = System.Drawing.Color.Blue;

            var _countData = dt.AsEnumerable().ToList().Count;
            ViewState["TotalRecordItem"] = _countData;
            Label4.Text = _countData.ToString();
            Label4.ForeColor = System.Drawing.Color.Blue;


            ViewState["ChooseItem"] = dt;
            gvItem.DataSource = ViewState["ChooseItem"];
            gvItem.DataBind();
        }


    }
}