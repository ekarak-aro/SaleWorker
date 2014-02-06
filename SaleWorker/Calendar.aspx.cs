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
    public partial class Calendar : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private String strConnStringAccpac = ConfigurationManager.ConnectionStrings["conAccpac"].ConnectionString;
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {           
            GetSchedule();
        }

        private void GetSchedule()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select convert(nvarchar(10),plandate,103) as dateevent,customername from WeeklyPlan left join" +
                            " webpages_Membership on saleid = CodeSLSP" +
                            " where recordstatus = 'Active'" +
                            " and plandate >= CAST(getdate() as date)" +
                            " and UserActiveDirectory = @user";
                    cmd.Parameters.Clear();                    
                    var _userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@user", SqlDbType.NVarChar).Value = _userEncrypt.Encrypt(Session["username"].ToString());
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {                        
                        dt.Load(dr);
                    }
                    dr.Close();
                }
            }

        }

        protected void CalendarWithEvent_DayRender(object sender, DayRenderEventArgs e)
        {

            foreach (DataRow row in dt.Rows)
            {
                if (row[0].ToString() == e.Day.Date.ToShortDateString())
                {
                    Literal l = new Literal();
                    l.Visible = true;
                    l.Text = "<br />";
                    e.Cell.Controls.Add(l);
                    Label lb = new Label();
                    lb.Visible = true;
                    lb.Text = row[1].ToString();
                    lb.ForeColor = System.Drawing.Color.Blue;
                    e.Cell.BackColor = System.Drawing.Color.Azure;
                    e.Cell.Controls.Add(lb);
                }
            }
        }

        protected void CalendarWithEvent_PreRender(object sender, EventArgs e)
        {
            //CalendarWithEvent.SelectedDates.Clear();
        }
    }
}