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
    public partial class WeeklyPlanAdd : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private String strConnStringAccpac = ConfigurationManager.ConnectionStrings["conAccpac"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CallProvince();
                CallDistrict();
            }
        }

        private void CallDistrict()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select ltrim(rtrim(district)) as district,id from district where recordstatus = 'active' order by district";
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);

                        ddlDistrict.DataSource = dt;
                        ViewState["district"] = dt;
                        ddlDistrict.DataTextField = "district";
                        ddlDistrict.DataValueField = "id";
                        ddlDistrict.DataBind();

                        ddlDistrict.Items.Insert(0, "กรุณาเลือก");
                        ddlDistrict.SelectedIndex = 0;


                        ddlNewDistrict.DataSource = dt;
                        ddlNewDistrict.DataTextField = "district";
                        ddlNewDistrict.DataValueField = "id";
                        ddlNewDistrict.DataBind();

                        ddlNewDistrict.Items.Insert(0, "กรุณาเลือก");
                        ddlNewDistrict.SelectedIndex = 0;


                    }
                    dr.Close();
                }
            }
        }


        private void CallProvince()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select ltrim(rtrim(province)) as province,id from province where recordstatus = 'active' order by province";
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);

                        ddlProvince.DataSource = dt;
                        ViewState["province"] = dt;
                        ddlProvince.DataTextField = "province";
                        ddlProvince.DataValueField = "id";
                        ddlProvince.DataBind();

                        ddlProvince.Items.Insert(0, "กรุณาเลือก");
                        ddlProvince.SelectedIndex = 0;


                        ddlNewProvince.DataSource = dt;
                        ddlNewProvince.DataTextField = "province";
                        ddlNewProvince.DataValueField = "id";
                        ddlNewProvince.DataBind();

                        ddlNewProvince.Items.Insert(0, "กรุณาเลือก");
                        ddlNewProvince.SelectedIndex = 0;


                    }
                    dr.Close();
                }
            }
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProvince.SelectedIndex != 0)
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select ltrim(rtrim(district)) as district,id from district where recordstatus = 'active' and provinceid = @provinceid order by district";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@provinceid", SqlDbType.Int).Value = Convert.ToInt32(ddlProvince.SelectedValue);
                        SqlDataReader dr;
                        conn.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            ddlDistrict.DataSource = dt;
                            ddlDistrict.DataTextField = "district";
                            ddlDistrict.DataValueField = "id";
                            ddlDistrict.DataBind();

                            ddlDistrict.Items.Insert(0, "กรุณาเลือก");
                            ddlDistrict.SelectedIndex = 0;

                        }
                        else
                        {
                            ddlDistrict.DataSource = ViewState["district"];
                            ddlDistrict.DataTextField = "district";
                            ddlDistrict.DataValueField = "id";
                            ddlDistrict.DataBind();

                            ddlDistrict.Items.Insert(0, "กรุณาเลือก");
                            ddlDistrict.SelectedIndex = 0;
                        }
                        dr.Close();
                    }
                }
            }

        }

        protected void ddlNewProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNewProvince.SelectedIndex != 0)
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select ltrim(rtrim(district)) as district,id from district where recordstatus = 'active' and provinceid = @provinceid order by district";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@provinceid", SqlDbType.Int).Value = Convert.ToInt32(ddlNewProvince.SelectedValue);
                        SqlDataReader dr;
                        conn.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            ddlNewDistrict.DataSource = dt;
                            ddlNewDistrict.DataTextField = "district";
                            ddlNewDistrict.DataValueField = "id";
                            ddlNewDistrict.DataBind();

                            ddlNewDistrict.Items.Insert(0, "กรุณาเลือก");
                            ddlNewDistrict.SelectedIndex = 0;

                        }
                        else
                        {
                            ddlNewDistrict.DataSource = ViewState["district"];
                            ddlNewDistrict.DataTextField = "district";
                            ddlNewDistrict.DataValueField = "id";
                            ddlNewDistrict.DataBind();

                            ddlNewDistrict.Items.Insert(0, "กรุณาเลือก");
                            ddlNewDistrict.SelectedIndex = 0;
                        }
                        dr.Close();
                    }
                }
            }
        }
        private void msgbx(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
        }
        protected void btNewCust_Click(object sender, EventArgs e)
        {
            if (tbNewCustName.Text != null && ddlNewProvince.SelectedIndex != 0 && ddlNewDistrict.SelectedIndex != 0)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(strConnString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "sp_new_customer";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("@customerName", SqlDbType.NVarChar).Value = tbNewCustName.Text;
                            cmd.Parameters.Add("@districtid", SqlDbType.Int).Value = Convert.ToInt32(ddlNewDistrict.SelectedValue);
                            cmd.Parameters.Add("@province", SqlDbType.Int).Value = Convert.ToInt32(ddlNewProvince.SelectedValue);
                            Label _lbUser = this.Master.FindControl("lblName") as Label;
                            cmd.Parameters.Add("@UserCreate", SqlDbType.NVarChar).Value = _lbUser.Text;
                            //                            cmd.Parameters.Add("@retId", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;                          
                            conn.Open();
                            //cmd.ExecuteNonQuery();
                            var Identity = cmd.ExecuteScalar();
                            int Id = Convert.ToInt32(Identity);
                            //int Id = Convert.ToInt32(cmd.Parameters["@retId"].Value);
                            //int Id = Convert.ToInt32(cmd.ExecuteScalar());                            
                            AssignDataNewCust(Id);
                            msgbx("add new customer complete");
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgbx(ex.ToString());
                }

            }
        }

        private void AssignDataNewCust(int _id)
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from new_Customer where id = @id ";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = _id;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        tbCustId.Text = dt.Rows[0][1].ToString();
                        lbCustomer.Text = dt.Rows[0][2].ToString();
                        ddlDistrict.SelectedValue = dt.Rows[0][3].ToString();
                        ddlProvince.SelectedValue = dt.Rows[0][4].ToString();
                        lbCustomer.ForeColor = System.Drawing.Color.Blue;


                        tbCustId.Enabled = false;
                        ddlProvince.Enabled = false;
                        ddlDistrict.Enabled = false;
                        

                        tbNewCustName.Text = null;
                        ddlNewDistrict.SelectedIndex = 0;
                        ddlNewProvince.SelectedIndex = 0;

                        //ddlNewDistrict.DataSource = dt;
                        //ddlNewDistrict.DataTextField = "district";
                        //ddlNewDistrict.DataValueField = "id";
                        //ddlNewDistrict.DataBind();

                        //ddlNewDistrict.Items.Insert(0, "กรุณาเลือก");
                        //ddlNewDistrict.SelectedIndex = 0;

                    }
                    dr.Close();
                }
            }
        }        

        protected void btAdd_Click(object sender, EventArgs e)
        {
            if (tbDate.Text.Length != 10)
            {
                msgbx("รูปแบบวันที่ไม่ถูกต้อง(dd/MM/yyy)");
                return;
            }
            var fillTime = DateTime.Parse(tbDate.Text);
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int weekNumToday = cul.Calendar.GetWeekOfYear(
                DateTime.Now,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
            int weekNum = cul.Calendar.GetWeekOfYear(
                fillTime,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
            var result = DateTime.Parse(tbDate.Text).Year;
            var yearToday = DateTime.Now.Year;
            if (fillTime <= DateTime.Now)
            {
                msgbx("วันที่ต้องไม่น้อยกว่าวันปัจจุบัน");
                return;
            }
            if (result == yearToday)
            {
                if (weekNum <= weekNumToday)
                {
                    msgbx("วันที่ที่บันทึกต้องไม่อยู่ในสัปดาห์เดียวกัน");
                    return;
                }
            }
            

            Page.Validate();
            if (Page.IsValid)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(strConnString))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "sp_insert_weekly_plan";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            Label _lbUser = this.Master.FindControl("lblName") as Label;
                            cmd.Parameters.Add("@UserCreate", SqlDbType.NVarChar).Value = _lbUser.Text;
                            cmd.Parameters.Add("@plandate", SqlDbType.NVarChar).Value = tbDate.Text;
                            cmd.Parameters.Add("@saleid", SqlDbType.NVarChar).Value = GetSaleId(_lbUser.Text);
                            cmd.Parameters.Add("@salename", SqlDbType.NVarChar).Value = GetSaleName(_lbUser.Text);
                            if (ddlTypeCust.SelectedValue == "1")
                            {
                                cmd.Parameters.Add("@isnewcust", SqlDbType.NChar).Value = "1";
                                cmd.Parameters.Add("@DistrictId", SqlDbType.Int).Value = DBNull.Value;
                                cmd.Parameters.Add("@DistrictName", SqlDbType.NVarChar).Value = ViewState["codestte"].ToString();
                                cmd.Parameters.Add("@ProvinceId", SqlDbType.Int).Value = DBNull.Value;
                                cmd.Parameters.Add("@ProvinceName", SqlDbType.NVarChar).Value = ViewState["namecity"].ToString();
                            }
                            else
                            {
                                cmd.Parameters.Add("@isnewcust", SqlDbType.NChar).Value = "2";
                                cmd.Parameters.Add("@DistrictId", SqlDbType.Int).Value = Convert.ToInt32(ddlDistrict.SelectedValue);
                                cmd.Parameters.Add("@DistrictName", SqlDbType.NVarChar).Value = ddlDistrict.SelectedItem.ToString();
                                cmd.Parameters.Add("@ProvinceId", SqlDbType.Int).Value = Convert.ToInt32(ddlProvince.SelectedValue);
                                cmd.Parameters.Add("@ProvinceName", SqlDbType.NVarChar).Value = ddlProvince.SelectedItem.ToString();
                            }
                            cmd.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = tbCustId.Text;
                            cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = lbCustomer.Text;

                            cmd.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = tbRemark.Text;
                            cmd.Parameters.Add("@Isapprove", SqlDbType.NVarChar).Value = "NO";
                            conn.Open();
                            cmd.ExecuteNonQuery();                           
                            msgbx("add data complete");
                            ClearData();
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgbx(ex.ToString());
                }
            }
        }

        private void ClearData()
        {
            tbDate.Text = null;
            tbCustId.Text = null;
            lbCustomer.Text = "Customer Desc";
            ddlDistrict.SelectedIndex = 0;
            ddlProvince.SelectedIndex = 0;
            tbRemark.Text = null;
            ddlTypeCust.SelectedValue = "2";
        }

        private object GetSaleName(string p)
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select top 1 title+fname+' '+lname as salename from webpages_Membership where status = 'Active' and useractivedirectory = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    var _userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = _userEncrypt.Encrypt(p);
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ViewState["salename"] = dt.Rows[0][0].ToString();
                    }
                    dr.Close();
                    return ViewState["salename"].ToString();
                }
            }
        }

        private object GetSaleId(string p)
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select top 1 codeslsp from webpages_Membership where status = 'Active' and useractivedirectory = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    var _userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = _userEncrypt.Encrypt(p);
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ViewState["codeslsp"] = dt.Rows[0][0].ToString();

                    }
                    dr.Close();
                    return ViewState["codeslsp"].ToString();
                }
            }
        }

        protected void gvCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomer.PageIndex = e.NewPageIndex;
            //gvCustomer.SelectedIndex = -1;
            gvCustomer.DataSource = ViewState["tableSearcust"];
            gvCustomer.DataBind();  
        }

        protected void gvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int rowIndex = gvr.RowIndex;
                tbCustId.Text = gvCustomer.Rows[rowIndex].Cells[0].Text;
                //tbCusCode.Enabled = false;
                lbCustomer.ForeColor = System.Drawing.Color.Black;
                lbCustomer.Text = gvCustomer.Rows[rowIndex].Cells[1].Text;
                ViewState["namecity"] = gvCustomer.Rows[rowIndex].Cells[2].Text;
                ViewState["codestte"] = gvCustomer.Rows[rowIndex].Cells[3].Text;
            }
        }

        protected void btSearchCustGV_Click(object sender, EventArgs e)
        {
            if (tbSearchCust.Text.Length == 0)
            {
                msgbx("Please fill data");
                return;
            }
            using (SqlConnection conn = new SqlConnection(strConnStringAccpac))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    if (ddlTypeSearchCust.SelectedValue == "CustomerCode")
                    {
                        cmd.CommandText = "select IDCUST,ltrim(rtrim(NAMECUST)) as namecust,namecity,codestte  from arcus where  swactv = 1  and IDCUST like '%' + @search + '%'";
                    }
                    else if (ddlTypeSearchCust.SelectedValue == "CustomerDesc")
                    {
                        cmd.CommandText = "select IDCUST,ltrim(rtrim(NAMECUST)) as namecust,namecity,codestte  from arcus where  swactv = 1  and NAMECUST like '%' + @search + '%'"; ;

                    }

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@search", SqlDbType.NVarChar).Value = tbSearchCust.Text;
                    DataSet ds = new DataSet();
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ViewState["tableSearcust"] = dt;
                        gvCustomer.DataSource = ViewState["tableSearcust"];
                        gvCustomer.DataBind();
                    }
                    else
                    {
                        //BingEmpyGridViewWithHeader(gvCustomer, ds, "No data found");
                        msgbx("ไม่พบลูกค้า");
                        //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Not found data" + "');", true);                   
                        gvCustomer.DataSource = null;
                        gvCustomer.DataBind();
                    }
                    dr.Close();
                }
            }
        }

        


    }
}