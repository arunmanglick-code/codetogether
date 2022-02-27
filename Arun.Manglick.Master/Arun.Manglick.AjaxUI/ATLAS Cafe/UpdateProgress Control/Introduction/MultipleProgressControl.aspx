<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MultipleProgressControl.aspx.cs" Inherits="MultipleProgressControl"
    Title="MultipleProgressControl Page" %>

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
                <asp:Label ID="lblHeader" runat="server" Text="'UpdateProgress' Control Introduction"></asp:Label>
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
                            <li>Multiple UpdateProgress Control</li>
                            <li>One UpdateProgress control on the page can show a progress message for all UpdatePanel controls on the page.</li>
                            <li>AssociatedUpdatePanelID - Enables to associate the UpdateProgress control with a single UpdatePanel control</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Multiple Update Panel with their own Progress Controls</legend>
                                    <br /><br />Last refresh<%=DateTime.Now.ToString() %><br /><br />
                                    <asp:Button ID="Button1" CssClass="button" runat="server" Text="Refresh Panel" OnClick="Button1_Click" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                Processing... <img src="../../../Images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <br />
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Multiple Update Panel with their own Progress Controls</legend>
                                    <br /><br />Last refresh<%=DateTime.Now.ToString() %><br /><br />
                                    <asp:Button ID="Button2" CssClass="button" runat="server" Text="Refresh Panel" OnClick="Button2_Click" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                            <ProgressTemplate>
                                Processing... <img src="../../../Images/ajax-loader.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
