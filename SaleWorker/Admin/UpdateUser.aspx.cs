using SaleWorker.ObjectClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SaleWorker.Admin
{
    public partial class UpdateUser : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private String strConnStringAccpac = ConfigurationManager.ConnectionStrings["conAccpac"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CallGroupUser();
                CallSaleId();
            }

        }

        private void CallSaleId()
        {
            using (SqlConnection conn = new SqlConnection(strConnStringAccpac))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select CODESLSP, CODESLSP as item from arsap where SWACTV = 1 order by CODESLSP";
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ddlSaleId.DataSource = dt;
                        ddlSaleId.DataTextField = "CODESLSP";
                        ddlSaleId.DataValueField = "item";
                        ddlSaleId.DataBind();

                        ddlSaleId.Items.Insert(0, "กรุณาเลือก");
                        ddlSaleId.SelectedIndex = 0;
                    }
                    dr.Close();
                }

            }
        }

        private void CallGroupUser()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select depid,depname from webpages_Department where depstatus = 'Active' order by depname";
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ddlGroupUser.DataSource = dt;
                        ddlGroupUser.DataTextField = "depname";
                        ddlGroupUser.DataValueField = "depid";
                        ddlGroupUser.DataBind();

                        ddlGroupUser.Items.Insert(0, "กรุณาเลือก");
                        ddlGroupUser.SelectedIndex = 0;
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

        protected void onClick(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue == "1")
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select * from webpages_Membership where username = @UserName";
                        cmd.CommandType = CommandType.Text;
                        var _userEncrypt = new StringVarious();
                        cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(tbSearch.Text);
                        SqlDataReader dr;
                        conn.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            lbUsername.Text = _userEncrypt.Decrypt(dt.Rows[0][5].ToString());
                            tbEmail.Text = dt.Rows[0][9].ToString();
                            lbUserActiveDirectory.Text = _userEncrypt.Decrypt(dt.Rows[0][10].ToString());
                            if (dt.Rows[0][11].ToString() == DBNull.Value.ToString())
                            {
                                ddlSaleId.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlSaleId.SelectedValue = dt.Rows[0][11].ToString();
                            }                            
                            ddlGroupUser.SelectedValue = dt.Rows[0][17].ToString();
                            tbTitle.Text = dt.Rows[0][13].ToString();
                            tbFName.Text = dt.Rows[0][14].ToString();
                            tbLName.Text = dt.Rows[0][15].ToString();
                            tbTel.Text = dt.Rows[0][16].ToString();
                            ViewState["id"] = dt.Rows[0][0].ToString();                           
                            foreach (ListItem item in cblRole.Items)
                            {
                                item.Selected = false;
                            }
                            getRole();
                        }
                        else
                        {
                            MessageBox("Not found data");
                            return;
                        }
                        dr.Close();
                    }

                }
            }
            else if (ddlType.SelectedValue == "2")
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select * from webpages_Membership where username = @UserAD";
                        cmd.CommandType = CommandType.Text;
                        var _userEncrypt = new StringVarious();
                        cmd.Parameters.Add("@UserAD", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(tbSearch.Text);
                        SqlDataReader dr;
                        conn.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            lbUsername.Text = _userEncrypt.Decrypt(dt.Rows[0][5].ToString());
                            tbEmail.Text = dt.Rows[0][9].ToString();
                            lbUserActiveDirectory.Text = _userEncrypt.Decrypt(dt.Rows[0][10].ToString());
                            ddlSaleId.SelectedValue = dt.Rows[0][11].ToString();
                            ddlGroupUser.SelectedValue = dt.Rows[0][17].ToString();
                            tbTitle.Text = dt.Rows[0][13].ToString();
                            tbFName.Text = dt.Rows[0][14].ToString();
                            tbLName.Text = dt.Rows[0][15].ToString();
                            tbTel.Text = dt.Rows[0][16].ToString();
                            ViewState["id"] = dt.Rows[0][0].ToString();                            
                            foreach (ListItem item in cblRole.Items)
                            {
                                item.Selected = false;
                            }
                            getRole();
                        }
                        else
                        {
                            MessageBox("Not found data");
                            return;
                        }
                        dr.Close();
                    }
                }
            }
        }

        private void getRole()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select a.roleid,b.rolename from webpages_UsersInRoles a inner join webpages_Roles b " +
                        " on a.RoleId = b.RoleId " +
                        " where a.userid = @userid ";
                    cmd.CommandType = CommandType.Text;
                    var _userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Convert.ToInt32(ViewState["id"].ToString());
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (cblRole.Items[j].Value == dt.Rows[i][0].ToString())
                                {
                                    cblRole.Items[j].Selected = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox("Not found data");
                        return;
                    }
                    dr.Close();
                }
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            //check page valid
            if (tbPassword.Text.Length == 0)
            {
                MessageBox("Please fill password");
                return;
            }
            else if (tbConfirmPassword.Text.Length == 0)
            {
                MessageBox("Please confirm password");
                return;
            }
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }
            //update membership
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    string _cmdText;
                    _cmdText = "update webpages_Membership set editdate = @editdate , edituser = @edituser,password = @password," +
                        " passwordchangeddate=@passwordchangeddate,codeslsp=@codeslsp,title=@title,fname=@fname,lname=@lname,tel=@tel," +
                        " department=@department ,email=@email where ";
                    if (ddlType.SelectedValue == "1")
                    {
                        _cmdText = _cmdText + " username=@username";
                    }
                    else if (ddlType.SelectedValue == "2")
                    {
                        _cmdText = _cmdText + " useractivedirectory=@username";
                    }
                    cmd.CommandText = _cmdText;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@editdate", SqlDbType.DateTime).Value = DateTime.Now;
                    Label _lbUser = this.Master.FindControl("lblName") as Label;
                    cmd.Parameters.Add("@edituser", SqlDbType.VarChar).Value = _lbUser.Text;
                    var _userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(tbPassword.Text);
                    cmd.Parameters.Add("@passwordchangeddate", SqlDbType.DateTime).Value = DateTime.Now;
                    if (ddlSaleId.SelectedIndex == 0)
                    {
                        cmd.Parameters.Add("@codeslsp", SqlDbType.VarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("@codeslsp", SqlDbType.VarChar).Value = ddlSaleId.SelectedValue;
                    }
                    cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = tbTitle.Text;
                    cmd.Parameters.Add("@fname", SqlDbType.VarChar).Value = tbFName.Text;
                    cmd.Parameters.Add("@lname", SqlDbType.VarChar).Value = tbLName.Text;
                    cmd.Parameters.Add("@tel", SqlDbType.VarChar).Value = tbTel.Text;
                    cmd.Parameters.Add("@department", SqlDbType.VarChar).Value = ddlGroupUser.SelectedValue;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = tbEmail.Text;
                    if (ddlType.SelectedValue == "1")
                    {
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(lbUsername.Text);

                    }
                    else if (ddlType.SelectedValue == "1")
                    {
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(lbUserActiveDirectory.Text);
                    }
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // delete role
                        cmd.CommandText = "delete from webpages_UsersInRoles where userid = @userid";                       
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Convert.ToInt32(ViewState["id"].ToString());                       
                        cmd.ExecuteNonQuery();

                        //insert role
                        foreach (ListItem item in cblRole.Items)
                        {
                            if (item.Selected)
                            {
                                cmd.CommandText = "insert into webpages_UsersInRoles values(@userId,@roleId)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = Convert.ToInt32(ViewState["id"].ToString());
                                cmd.Parameters.Add("@roleId", SqlDbType.Int).Value = item.Value;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        MessageBox("Add data complete");

                        //This one call clear method to clear all control values
                        clear();
                        cblRole.ClearSelection();

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                    }

                }
            }
        }

        private void clear()
        {
            //This loop takes all controls from the form1 - make sure your form name as form1 otherwise change it as per your form name
            foreach (Control c in form1.Controls)
            {
                //Clear all textbox values
                if (c is TextBox)
                    ((TextBox)c).Text = "";

                //clear all check boxes
                if (c is CheckBox)
                    ((CheckBox)c).Checked = false;                

                //Clear all radio buttons
                if (c is RadioButton)
                    ((RadioButton)c).Checked = false;

                //Clear all radio buttons
                if (c is DropDownList)
                    ((DropDownList)c).SelectedIndex = 0;
            }
            
        }
    }
}