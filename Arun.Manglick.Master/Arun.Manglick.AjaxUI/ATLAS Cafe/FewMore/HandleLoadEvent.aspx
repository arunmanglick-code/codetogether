<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HandleLoadEvent.aspx.cs" Inherits="HandleLoadEvent" Title="SingleUpdatePanel Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
<script type="text/javascript">

// This will fire only once and not again. Hence to overcome this, solution is to make this in AJAX
//window.onload = function(){HideMe();}  

function HideMe()
{
    var lblLanguage=document.getElementById('<%= Label1.ClientID %>');
    lblLanguage.style.display='none';
}

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
<script language="javascript" type="text/javascript">

var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_pageLoaded(pageLoaded);

function pageLoaded(sender, args) 
{
    var lblLanguage=document.getElementById('<%= Label1.ClientID %>');
    lblLanguage.style.display='none';
}
</script>
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Single Update Panel"></asp:Label>
            </td>
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
                            <li>Handle window.onLoad() in AJAX</li>
                            <li>Achvievment is to make the Label Invisible on window load</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width:500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br /><br />
                                    <asp:Button ID="Button1" CssClass="button" runat="server" Text="Button" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>                      
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
