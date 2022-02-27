<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Sorting.aspx.cs" Inherits="Sorting"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Grid View - Various Features"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
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
                    <div class="DivClassFeature" style="width: 700px;">
                        <b>Varoius Features Used.</b>
                        <ol>                          
                            <li>Exporting GridView Data to Excel Format</li>
                            <li>Here the GridView does not contain any Controls, except the 'Label' control. Otherwise A run-time error occurs if the DataGrid contains any controls other than the LiteralControl</li>
                            <li>The Export feature produces an error - Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server</li>
                            <li>The soultion for this - Just override 'VerifyRenderingInServerForm(Control control)'</li>
                            <li>The overridden method would simply be empty</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <div>
                        <div class="DivClassFloat">
                            <asp:Button ID="btnSubmit" Text="Export to Excel" runat="server" OnClick="btnSubmit_Click" />
                        </div>
                        <div style="height:200px;">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" 
                                DataSourceID="MoveObjectDataSource">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Director" HeaderText="Director" 
                                        SortExpression="Director" />
                                    <asp:BoundField DataField="Language" HeaderText="Language" 
                                        SortExpression="Language" />
                                </Columns>
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="MoveObjectDataSource" runat="server" 
                                SelectMethod="GetEmptyDataTable" TypeName="Arun.Manglick.UI.MoveCollection">
                            </asp:ObjectDataSource>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
