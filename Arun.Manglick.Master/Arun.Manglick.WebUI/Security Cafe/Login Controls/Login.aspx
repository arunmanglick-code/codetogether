<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Security_Cafe_Login_Controls_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <asp:Login
        id="Login1"
        InstructionText="Please log in before
            accessing the premium section of our Website."
        TitleText="Log In"
        TextLayout="TextOnTop"
        LoginButtonText="Log In"
        DisplayRememberMe="false"
        CssClass="login"
        TitleTextStyle-CssClass="login_title"
        InstructionTextStyle-CssClass="login_instructions"
        LoginButtonStyle-CssClass="login_button"
        Runat="server" />

    </div>
    </form>
</body>
</html>
