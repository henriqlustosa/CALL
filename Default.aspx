<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&family=Ubuntu:ital,wght@0,300;0,400;0,500;0,700;1,300;1,400;1,500;1,700&display=swap" rel="stylesheet">

    <title>Call HSPM</title>
    <!-- Bootstrap -->
    <%--    <link href="vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" type="text/css" />--%>
    <!-- Font Awesome -->
    <%--    <link href="vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />--%>
    <!-- NProgress -->
    <%--    <link href="vendors/nprogress/nprogress.css" rel="stylesheet" type="text/css" />--%>
    <!-- Animate.css -->
    <%--    <link href="vendors/animate.css/animate.min.css" rel="stylesheet" type="text/css" />--%>
    <!-- Custom Theme Style -->
    <%--    <link href="build/css/custom.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/login.css" rel="stylesheet" />
</head>
<body>
    <h2 class="titulo">SISTEMA CALL</h2>
    <div class="login">
        <div class="login-imagem">
            <img class="login-imagem--img" src="img/call.png" />
        </div>
        <div class="login-form">
            <h2 class="login-form--titulo">Login</h2>
            <form id="form1" runat="server">

                <asp:Login ID="Login1" runat="server"
                    DestinationPageUrl="~/administrativo/Principal.aspx" Width="100%">
                    <LayoutTemplate>
                        <div class="col-md-12 form-group">
                            <asp:TextBox ID="UserName" runat="server" class="form-control has-feedback-left login-form--textbox" placeholder="Usuário"></asp:TextBox>
                            <span class="fa fa-user form-control-feedback left" aria-hidden="true"></span>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server"
                                ControlToValidate="UserName" ErrorMessage="User Name is required."
                                ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-12 form-group">
                            <asp:TextBox ID="Password" runat="server" class="form-control has-feedback-left login-form--textbox" TextMode="Password" placeholder="Senha"></asp:TextBox>
                            <span class="fa fa-lock form-control-feedback left" aria-hidden="true"></span>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server"
                                ControlToValidate="Password" ErrorMessage="Password is required."
                                ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                        </div>
                        <div align="center" style="color: Red;">
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-12 col-md-offset-3">
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Entrar"
                                ValidationGroup="Login1" class="login-form--btn" />
                        </div>
                        <img src="img/Logo_HSPM_Pref.jpg" class="login-form--logo" />
                    </LayoutTemplate>
                </asp:Login>
            </form>
        </div>
    </div>
    <footer>
        <p>©2025 HSPM - Hospital do Servidor Público Municipal</p>
    </footer>
</body>
</html>
