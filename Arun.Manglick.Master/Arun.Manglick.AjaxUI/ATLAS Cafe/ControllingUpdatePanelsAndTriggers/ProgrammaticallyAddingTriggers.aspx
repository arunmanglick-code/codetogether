<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProgrammaticallyAddingTriggers.aspx.cs" Inherits="ProgrammaticallyAddingTriggers"
    Title="ProgrammaticallyAddingTriggers Page" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

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
                <asp:Label ID="lblHeader" runat="server" Text="Programmatically Adding Triggers"></asp:Label>
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
                    <div class="DivClassFeature" style="width: 800px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Programmatically Adding Triggers using 'ScriptManager.RegisterAsyncPostBackControl (Button1) '.</li>
                            <li>You can specify triggers declaratively using the Triggers collection of an UpdatePanel control. Additionally, you can programmatically identify triggers and cause an UpdatePanel control to refresh in the server code of your page.</li>
                            
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Programmatically Adding Triggers</legend>
                                    <br /><br />Last refresh <%=DateTime.Now.ToString() %><br /><br />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:Button ID="Button1" runat="server" CssClass="button" Text="Programmatically Added Trigger Effect"
                            OnClick="Button1_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
