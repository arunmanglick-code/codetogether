<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NestedUpdatePanel.aspx.cs" Inherits="NestedUpdatePanel" Title="NestedUpdatePanel Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Nested Update Panel"></asp:Label>
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
                            <li>Nested Update Panel</li>
                            <li>Parent Update Panel makes all Child Panel update. This is not true vice-versa.</li>
                            <li>Child controls of nested UpdatePanel controls do not cause an update to the outer UpdatePanel control unless they are explicitly defined as triggers for the parent panel.</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Parent UpdatePanel</legend>
                                    <br /><br />Outer refresh <%=DateTime.Now.ToString() %><br /><br />
                                    <asp:Button ID="Button1" CssClass="button" runat="server" Text="Refresh Outer & Inner Panel" /><br /><br />
                                    <br />
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset>
                                                <legend>Nested UpdatePanel</legend>
                                                <br /><br />Inner refresh <%=DateTime.Now.ToString() %><br /><br />                                                
                                                <asp:Button ID="Button2" CssClass="button" runat="server" Text="Refresh Inner Panel" />
                                                <asp:Button ID="Button3" CssClass="button" runat="server" Text="Refresh Outer Panel forcely" OnClick="Button3_Click" />
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
