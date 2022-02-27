<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Trial.aspx.cs" Inherits="HandleLoadEvent" Title="SingleUpdatePanel Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
<script type="text/javascript">

function ShowError()
{
    var lblLanguage=document.getElementById('<%= lblError.ClientID %>');
    lblLanguage.innerHTML = "Error Visible";
    return false;
}

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
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
                <asp:Label ID="lblError" runat="server" CssClass="validation-error"></asp:Label>
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
                                    <asp:Button ID="Button1" CssClass="button" OnClientClick="return ShowError();" runat="server" Text="Button" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Button ID="Button2" CssClass="button" OnClientClick="return ShowError();" runat="server" Text="Button" />                      
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
