<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Temp.aspx.cs" Inherits="Temp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="App_Themes/MyTheme/stylecal.css" rel="stylesheet" type="text/css" />

    <script src="JS/Calendar_new.js" type="text/javascript"></script>
    <script src="JS/Calendar.js" type="text/javascript"></script>
    <script src="JS/calendar-en.js" type="text/javascript"></script>
    <script type="text/javascript">
        //Method to check the length of the text box
        function CheckLength(ctrl)
        {
            
            if(ctrl.value.length < 5)
            {
                return true;
            }
            return false;
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="DivClassFloat">
                            <asp:TextBox ID="txtTitle" runat="server" />
                             <img alt="Select From Date" src="images/calendar-icon.gif" onmouseover="javascript:this.style.cursor='hand';" onclick="return showCalendar('txtTitle','dd/mm/y');" />
                            <asp:Button ID="btnSubmit" Text="Search" runat="server" OnClick="btnSubmit_Click" />
                        </div>
                        <div style="height:200px;">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:GridView>
                            <br /><br />
                            
                            <textarea id="htxtDescription" runat="server" cols="25" rows="3" onkeypress="Javascript:return CheckLength(this)"></textarea>
                        </div>
    </div>
    </form>
</body>
</html>
