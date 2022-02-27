<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MultipleUpdatePanel.aspx.cs" Inherits="MultipleUpdatePanel" Title="MultipleUpdatePanel Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

<script type="text/javascript">


</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    
    
    
    </asp:ScriptManager>
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Multiple Update Panel"></asp:Label>
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
                            <li>Multiple Update Panel</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Multiple Update Panel</legend>
                                    <br /><br />Last refresh <%=DateTime.Now.ToString() %><br /><br />
                                    <asp:Button ID="Button1" CssClass="button" runat="server" Text="Refresh Panel 1" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Multiple Update Panel</legend>
                                    <br /><br />Last refresh <%=DateTime.Now.ToString() %><br /><br />
                                    <asp:Button ID="Button2" CssClass="button" runat="server" Text="Refresh Panel 2" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
