<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="InformationSearch.aspx.cs" Inherits="TemplatePage" Title="Untitled Page" %>

<%@ Register Src="UserControls/InformationSearch.ascx" TagName="InformationSearch"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Using Information User Control"></asp:Label>
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
                            <li>InformationSearch UserControl</li>
                            <li>Requirement 1 - Raise an event from the User Control</li>
                            <li>Requirement 2 - Handle the raised event in the Containing Page</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table border="0" width="100%" cellpadding="1" cellspacing="3">
                            <!-- Row 1 -->
                            <tr>
                                <td colspan="3">
                                    <uc1:InformationSearch ID="InformationSearch1" runat="server" />
                                </td>
                            </tr>
                            <!-- Row 2 -->
                            <tr>
                                <td colspan="3">
                                    <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                                        Text="Using Infromation Control"></asp:Label>
                                </td>
                            </tr>
                            <!-- Row 3 -->
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblAssign" runat="server" Text="Searched Information"></asp:Label>
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:GridView ID="GridView1" runat="server" BorderColor="Maroon" BorderStyle="Solid"
                                        BorderWidth="1px" CellPadding="4" AutoGenerateColumns="False" ForeColor="#333333"
                                        GridLines="None">
                                        <FooterStyle BackColor="#990000" ForeColor="White" Font-Bold="True" />
                                        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                        <Columns>
                                            <asp:BoundField DataField="CourseId" HeaderText="CourseId" />
                                            <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                            <asp:BoundField DataField="Institution" HeaderText="Institution" />
                                            <asp:BoundField DataField="Course" HeaderText="Course" />
                                            <asp:TemplateField HeaderText="Average on Label">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Average") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle ForeColor="#333333" HorizontalAlign="Center" BackColor="#FFCC66" />
                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
