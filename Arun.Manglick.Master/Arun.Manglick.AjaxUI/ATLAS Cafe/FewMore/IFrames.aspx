<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="IFrames.aspx.cs" Inherits="IFrames" Title="IFrames Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
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
                            <li>Single Update Panel</li>
                            <li>Single Update Panel with Trigger where the Button has kept outside the UpdatePanel</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Single Update Panel</legend>
                                    <br />
                                    <br />
                                    Last refresh
                                    <%=DateTime.Now.ToString() %><br />
                                    <br />
                                    <asp:Button ID="Button1" class="button" runat="server" Text="Refresh Panel" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Single Update Panel with Trigger</legend>
                                    <br />
                                    <br />
                                    Last refresh
                                    <%=DateTime.Now.ToString() %><br />
                                    <br />
                                </fieldset>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                        <asp:Button ID="Button2" class="button" runat="server" Text="Refresh Panel using Trigger Effect" />
                    </div>
                    <div class="DivClassFloat" style="width: 500px;">
                        <iframe id="Frame1" src="../../AJAXPostImplmentation/UpdatePanelsAndTriggers/SingleUpdatePanel.aspx"
                            width="500px" height="200px"></iframe>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
