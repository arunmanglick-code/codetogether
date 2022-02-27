<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ExportGridView1.aspx.cs" Inherits="Data_Access_Cafe_Grid_View_VariousFeatures"
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
                                AutoGenerateColumns="false" AllowSorting="True">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <Columns>
                                    <asp:BoundField DataField="CourseId" SortExpression="CourseId" HeaderText="CourseId" />
                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                    <asp:BoundField DataField="Institution" HeaderText="Institution" />
                                    <asp:BoundField DataField="Course" HeaderText="Course" />
                                    <asp:TemplateField HeaderText="Average on Label">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Average") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
