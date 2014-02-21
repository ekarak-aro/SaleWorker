using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.DirectoryServices;
using SaleWorker.ObjectClass;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace SaleWorker
{
    public partial class Login : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            if (!IsPostBack)
            {
                GetRole();
                HttpCookie cookie = Request.Cookies["userinfo"];
                if (!(cookie == null))
                {
                    tbUsername.Text = cookie["username"];
                    TextBox tb = ((TextBox)(form.FindControl("tbPassword")));
                    tb.Attributes["Value"] = cookie["password"];
                    //tbPassword.Text = cookie["password"];
                    cbRemember.Checked = true;
                }
            }

        }




        protected void onClick(object sender, EventArgs e)
        {
            if (ddlRole.SelectedIndex == 0)
            {
                MessageBox("Please Select Role");
                return;
            }
            string path = "LDAP://pdgth.com/CN=Users,DC=pdgth,DC=com";
            if (AuthenticateUser(path, tbUsername.Text, tbPassword.Text))
            {
                if (cbRemember.Checked == true)
                {
                    HttpCookie cookie = new HttpCookie("userinfo");
                    cookie["username"] = tbUsername.Text;
                    cookie["password"] = tbPassword.Text;
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(cookie);
                }
                if (CheckRoleUser())
                {
                    if (ddlRole.SelectedValue == "1")
                    {
                        //Admin
                        Session["username"] = tbUsername.Text;
                        Response.Redirect("Admin/AddUser.aspx");
                    }
                    else if (ddlRole.SelectedValue == "2")
                    {
                        //User                        
                        Session["username"] = tbUsername.Text;
                        Response.Redirect("~/AddLoneItem.aspx");
                    }
                    else if (ddlRole.SelectedValue == "3")
                    {
                        //ReportSaleUser
                        Session["username"] = tbUsername.Text;
                        Response.Redirect("ReportUser/ReportLoanitem.aspx");
                    }
                    else if (ddlRole.SelectedValue == "4")
                    {
                        //Manager
                        Session["username"] = tbUsername.Text;
                        Response.Redirect("http://www.google.com");
                    }                                       
                }
                else
                {
                    MessageBox("you not have Role position,please contact IT !!");
                }

            }
            else
            {
                MessageBox("username or password is invalid!!");
            }

        }

        private bool CheckRoleUser()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = " select ur.roleid as roleid ,ro.rolename as rolename,ltrim(rtrim(ms.codeslsp)) as codeslsp from dbo.webpages_Membership ms left join dbo.webpages_UsersInRoles ur on " +
                        " ms.userid = ur.userid" +
                        " left join dbo.webpages_Roles ro on ur.roleid = ro.RoleId" +
                        " where ms.UserActiveDirectory = @username and ur.roleid = @roleid";
                    cmd.CommandType = CommandType.Text;
                    var _userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = _userEncrypt.Encrypt(tbUsername.Text);
                    cmd.Parameters.Add("@roleid", SqlDbType.Int).Value = ddlRole.SelectedValue;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        if (dt.Rows.Count > 0)
                        {
                            Session["SaleId"] = dt.Rows[0][2].ToString();                          
                            dr.Close();
                        }                        
                        return true;
                    }
                }
            }
            return false;
        }

        private void GetRole()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select RoleId,RoleName from dbo.webpages_Roles";
                    cmd.CommandType = CommandType.Text;                  
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        if (dt.Rows.Count > 0)
                        {
                            ddlRole.DataSource = dt;
                            ddlRole.DataTextField = "RoleName";
                            ddlRole.DataValueField = "RoleId";
                            ddlRole.DataBind();

                            ddlRole.Items.Insert(0, "กรุณาเลือก");
                            ddlRole.SelectedIndex = 0;
                           
                            dr.Close();
                        }
                    }
                }
            }
        }

        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        private bool AuthenticateUser(string path, string user, string password)
        {
            var de = new DirectoryEntry(path, user, password, AuthenticationTypes.Secure);
            try
            {
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        

        protected void tbUsername_TextChanged(object sender, EventArgs e)
        {
            if (tbUsername.Text.Trim().Length == 0)
            {
                GetRole();
            }
            else
            {

            }
        }

    }
}