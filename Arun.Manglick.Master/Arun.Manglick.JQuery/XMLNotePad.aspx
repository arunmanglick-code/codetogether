<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XMLNotePad.aspx.cs" Inherits="XMLNotePad" EnableTheming="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>XML NotePad</title>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="XML NotePad"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
            </td>
        </tr>
    </table>
    <!-- Table 2 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td>
                <div id="divform">                    
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div id="DivNotePad" runat="server">
                        <a href='#' onclick='self.close();'>Close</a><br /><br />
                        <asp:TextBox ID="txtNotePad" EnableViewState="false" runat="server" CssClass="inputfield" BorderWidth="0" TextMode="MultiLine" Width="750px" Height="450px"></asp:TextBox>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
