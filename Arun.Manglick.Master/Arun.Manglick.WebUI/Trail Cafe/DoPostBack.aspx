<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DoPostBack.aspx.cs" Inherits="Trail_Cafe_Trial2" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" Runat="Server">
    <link href="../App_Themes/MyTheme/Style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/MyTheme/Style.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
    function Show()
    {
        debugger;
        var id = 5;
        
        var status = window.confirm('Do you');
        
        if(status)
        {
            __doPostBack("CheckBox1","");
        }
        else
        {
            return false;
        }
    }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" Runat="Server">

    <asp:TextBox ID="TextBox1" runat="server" onblur="Show();" 
        ontextchanged="TextBox1_TextChanged"></asp:TextBox><br />
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><br />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
    <asp:Button ID="Button1" runat="server" Text="DoPostBack 1" 
        onclick="Button1_Click" /><br />
    <asp:Button ID="Button2" runat="server" Text="DoPostBack 2" 
        onclick="Button2_Click" /><br />
        
    <asp:CheckBox ID="CheckBox1" runat="server" onclick="Show();" oncheckedchanged="CheckBox1_CheckedChanged" />

</asp:Content>

