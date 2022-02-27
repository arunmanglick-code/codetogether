<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JQuerySimplePage.aspx.cs" Inherits="JQuerySimplePage" Title="JQuery Simple Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
<script type="text/javascript">

    function pageLoad() 
    {
        $(".inputfield").focus(hilite);
    }

    function hilite() 
    {
        $(this).css("background-color","red");
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
<Scripts>
    <asp:ScriptReference Path="~/JQuery/jquery-1.2.6.js" />
</Scripts>
</asp:ScriptManager>
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Your Text"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="validation-no-error">
                <asp:ValidationSummary ID="vlsStipulations" DisplayMode="List" CssClass="validation"
                    runat="server"></asp:ValidationSummary>
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
                    <!-- Features Div -->
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Enter Text</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                    <table>
                    <tr>
                    <td>
                        <asp:TextBox ID="TextBox1" CssClass="inputfield" runat="server"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:TextBox ID="TextBox2" CssClass="inputfield" runat="server"></asp:TextBox>
                    </td>
                    </tr>
                    </table>
                    
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
