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
            if (tbSearchItem.Text =="")
            {
                msgbx("Please fill data for search");
                return;
            }
        }        

        private void msgbx(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
        }
    }
}