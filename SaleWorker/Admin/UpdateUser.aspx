<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="UpdateUser.aspx.cs" Inherits="SaleWorker.Admin.UpdateUser" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" role="form" class="form-horizontal" id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="form-group">
            <div class="col-lg-3 col-md-3 col-sm-3 col-lg-offset-1 col-md-offset-1 col-sm-offset-1">
                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">Username</asp:ListItem>
                    <asp:ListItem Value="2">AD Username</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-sm-5 col-lg-4 col-md-5">
                <asp:TextBox ID="tbSearch" runat="server" class="form-control" placeholder="Search"></asp:TextBox>
            </div>
            <asp:Button ID="btSearch" runat="server" Text="Search" class="btn btn-primary" OnClick="onClick" />
        </div>
        <hr />
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Username : " class="col-sm-2 control-label"></asp:Label>
            <span class="label label-info">
                <asp:Label ID="lbUsername" runat="server" class="control-label"></asp:Label></span>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Password : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-sm-5 col-lg-4 col-md-5">
                <asp:TextBox ID="tbPassword" runat="server" class="form-control" placeholder="Password" TextMode="Password" ControlToCompare="tbPassword"></asp:TextBox>
                <asp:RegularExpressionValidator ID="valPassword" runat="server"
                    ControlToValidate="tbPassword"
                    ErrorMessage="Minimum password length is 6"
                    ValidationExpression=".{5}.*" Display="Dynamic" ForeColor="Red" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Confirm Password : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-sm-5 col-lg-4 col-md-5">
                <asp:TextBox ID="tbConfirmPassword" runat="server" class="form-control" placeholder="Confirm Password" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password mismatch" ControlToValidate="tbConfirmPassword" ControlToCompare="tbPassword" Display="Dynamic" ForeColor="Red"></asp:CompareValidator>
            </div>
        </div>
        <hr />
        <div class="row">
            <asp:Label ID="Label4" runat="server" Text="Title : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-sm-1 col-lg-1 col-md-1">
                <asp:TextBox ID="tbTitle" runat="server" class="form-control" placeholder="Title"></asp:TextBox>
            </div>
            <asp:Label ID="Label5" runat="server" Text="First Name : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-sm-2 col-lg-2 col-md-2">
                <asp:TextBox ID="tbFName" runat="server" class="form-control" placeholder="First Name"></asp:TextBox>
            </div>
            <asp:Label ID="Label6" runat="server" Text="Last Name : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-sm-3 col-lg-3 col-md-3">
                <asp:TextBox ID="tbLName" runat="server" class="form-control" placeholder="Last Name"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="row">
            <asp:Label ID="Label8" runat="server" Text="E-mail : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-sm-4 col-lg-4 col-md-4">
                <div class="input-group">
                    <span class="input-group-addon">@</span>
                    <asp:TextBox ID="tbEmail" runat="server" class="form-control" placeholder="E-mail" type="email"></asp:TextBox>
                </div>
            </div>
            <asp:Label ID="Label9" runat="server" Text="Telephone : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-sm-4 col-lg-4 col-md-4">
                <asp:TextBox ID="tbTel" runat="server" class="form-control" placeholder="Telephone"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="form-group">
            <asp:Label ID="Label7" runat="server" Text="User Active Directory : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-sm-4">
                <span class="label label-info">
                    <asp:Label ID="lbUserActiveDirectory" runat="server" class="control-label"></asp:Label></span>
            </div>
            <asp:Label ID="Label11" runat="server" Text="Sale Id : " class="col-sm-2 control-label "></asp:Label>
            <div class="col-sm-4">
                <asp:DropDownList ID="ddlSaleId" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label12" runat="server" Text="Department : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-lg-4 col-md-4 col-sm-4">
                <asp:DropDownList ID="ddlGroupUser" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <asp:Label ID="Label10" runat="server" Text="Role : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-lg-4 col-md-4 col-sm-4">
                <asp:CheckBoxList ID="cblRole" runat="server" CssClass="radio">
                    <asp:ListItem Value="1">Administrator </asp:ListItem>
                    <asp:ListItem Selected="True" Value="2">SaleUser</asp:ListItem>
                    <asp:ListItem Value="3">ReportSaleUser</asp:ListItem>
                    <asp:ListItem Value="4">Manager</asp:ListItem>
                </asp:CheckBoxList>
            </div>
        </div>
        <br />
        <div class="row">
            <asp:Button ID="btUpdate" runat="server" Text="Update User" class="btn btn-default btn-success col-md-offset-5" OnClick="btUpdate_Click" />
            <asp:Button ID="btCancel" runat="server" Text="Cancel" class="btn btn-default col-md-offset-1" />
        </div>

    </form>
    <script>
        $(document).ready(function () {           
            //$("#btCancel").click(function () {
            //    var password = $("#tbPassword").val;
            //    if (password == "") {
            //        alert("Please fill password");
            //    }               
            //});
        });    
    </script>
</asp:Content>
