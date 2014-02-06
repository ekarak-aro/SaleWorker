using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;

namespace SaleWorker
{
    public partial class Home : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        //{
        //    if (!e.Day.IsOtherMonth)
        //    {
        //        //                            vSQL = string.Format(@"Select *
        //        //                                   From Createmeet
        //        //                                   Where Date = '{0}'", e.Day.Date);
        //        //                            dt = objSQL.GetDataTable(vSQL);
        //        string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("select meetingDate,custname from dbo.DailyMettingCustomer1 where custname is not null "))
        //            {
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    DataTable dt = new DataTable();
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.Connection = con;
        //                    sda.SelectCommand = cmd;
        //                    sda.Fill(dt);
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        e.Cell.Text = string.Format("{0} [{1}]", e.Day.Date.Day.ToString(), dt.Rows[1]["custname"].ToString());
        //                    }

        //                }
        //            }
        //        }

                
        //    }
           
                       
        //}

    }
}