<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SaleWorker.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <link href="Content/bootstrap.css" rel="stylesheet" />    
    <link href="Content/bootstrap-theme.min.css" rel="stylesheet" />    
    <link href="Styles/Site.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.0.min.js"></script>        
    <script src="Scripts/bootstrap.min.js"></script>
    <link runat="server" rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
    <link runat="server" rel="icon" href="favicon.ico" type="image/ico" />
</head>
<body>

    <div class="container">
        <form class="form-signin" role="form" runat="server" id="form">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <h2 class="form-signin-heading">Please sign in</h2>

            <asp:TextBox ID="tbUsername" runat="server" CssClass="form-control" placeholder="Username" required="required" autofocus OnTextChanged="tbUsername_TextChanged"></asp:TextBox>

            <!--<input type="text" class="form-control" placeholder="Email address" required autofocus>-->

            <asp:TextBox ID="tbPassword" runat="server" CssClass="form-control" placeholder="Password" required="required" TextMode="Password"></asp:TextBox>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control"></asp:DropDownList>
            <!--<input type="password" class="form-control" placeholder="Password" required>-->
            <asp:CheckBox ID="cbRemember" runat="server" Text="remember-me" CssClass="checkbox" />
            <!-- <label class="checkbox">
                    <input type="checkbox" value="remember-me">
                    Remember me
                </label> -->

            <asp:Button ID="btSignIn" runat="server" Text="Sign in" class="btn btn-lg btn-success btn-block" OnClick="onClick" />
            <!--<button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>-->

        </form>

    </div>
    <!-- /container -->

 
</body>
</html>

