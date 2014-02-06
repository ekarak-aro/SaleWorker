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
using System.Collections.Specialized;
using System.Text;

namespace SaleWorker.Admin
{
    public partial class AddUser : SessionCheck
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
                    cmd.CommandText = "select CODESLSP,CODESLSP as item from arsap where SWACTV = 1 order by CODESLSP";
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
            if (!checkBeforeSave())
            {
                return;
            }
            if (!checkUsernameDup())
            {
                return;
            }
            Page.Validate();
            if (Page.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into webpages_Membership(CreateDate, EditDate, CreateUser,EditUser,UserName,Password,Email, " +
                        "UserActiveDirectory,CodeSLSP,[Status],Title,Fname,Lname,Tel,Department) " +
                        "OUTPUT INSERTED.UserId " +
          "values(@CreateDate,@EditDate, @CreateUser,@EditUser,@UserName,@Password,@Email, " +
          "@UserActiveDirectory,@CodeSLSP,@Status,@Title,@Fname,@Lname,@Tel,@Department)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@EditDate", SqlDbType.DateTime).Value = DateTime.Now;
                        var _userEncrypt = new StringVarious();
                        Label _lbUser = this.Master.FindControl("lblName") as Label;
                        cmd.Parameters.Add("@CreateUser", SqlDbType.VarChar).Value = _lbUser.Text; ;
                        cmd.Parameters.Add("@EditUser", SqlDbType.VarChar).Value = _lbUser.Text; 
                        cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(tbUsername.Text);
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(tbPassword.Text);
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = tbEmail.Text;
                        cmd.Parameters.Add("@UserActiveDirectory", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(tbAD.Text);
                        if (ddlGroupUser.SelectedIndex == 0)
                        {
                            cmd.Parameters.Add("@CodeSLSP", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@CodeSLSP", SqlDbType.VarChar).Value = ddlSaleId.SelectedValue;
                        }
                        
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Active";
                        cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = tbTitle.Text;
                        cmd.Parameters.Add("@Fname", SqlDbType.VarChar).Value = tbFName.Text;
                        cmd.Parameters.Add("@Lname", SqlDbType.VarChar).Value = tbLName.Text;
                        cmd.Parameters.Add("@Tel", SqlDbType.VarChar).Value = tbTel.Text;
                        cmd.Parameters.Add("@Department", SqlDbType.Int).Value = ddlGroupUser.SelectedValue;

                        try
                        {
                            conn.Open();
                            Int32 newId = (Int32)cmd.ExecuteScalar();
                            foreach (ListItem item in cblRole.Items)
                            {
                                if (item.Selected)
                                {
                                    cmd.CommandText = "insert into webpages_UsersInRoles values(@userId,@roleId)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = newId;
                                    cmd.Parameters.Add("@roleId", SqlDbType.Int).Value = item.Value;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            MessageBox("Add data complete");

                            //This one call clear method to clear all control values
                            clear();

                        }
                        catch (Exception ex)
                        {
                            MessageBox("Please contact IT " + ex.Message.ToString());
                        }
                    }
                }
            }
        }

        private bool checkUsernameDup()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from webpages_Membership where UserName = @UserName and [status] = 'active' ";
                    cmd.CommandType = CommandType.Text;
                    var _userEncrypt = new StringVarious();
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = _userEncrypt.Encrypt(tbUsername.Text);
                    SqlDataReader dr;
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        MessageBox("username นี้มีอยู่แล้ว");
                        return false;
                    }
                    dr.Close();
                }

            }
            return true;
        }

        private  bool checkBeforeSave()
        {
            if (ddlGroupUser.SelectedIndex == 0)
            {
                MessageBox("กรุณาเลือก Department");
                return false;                
            }
            //else if (ddlSaleId.SelectedIndex == 0)
            //{
            //    MessageBox("กรุณาเลือก Sale Id");
            //    return false;
            //}
            return true;
        }

        void clear()
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
            cblRole.SelectedValue = "2";
        }


    }
}