<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SortingUsingIComparable.aspx.cs" Inherits="SortingUsingIComparable"
    Title="Sorting Using IComparable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Grid View - Sorting with IComparer"></asp:Label></td>
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
                            <li>Sorting GridView with IComparer</li>
                            <li>Neither of the DataSource(SqlDataSource, ObjectDataSource) is used.</li>
                            <li>Here- IComparer is used for Sorting, bcoz, source of the Gridview is List and not any DataTable.</li>
                            <li>GridView1.DataSource = List</li>
                            <li>GridView1.DataBind()</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <div>                                             
                        <div style="height:200px;">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                AutoGenerateColumns="false" AllowSorting="True" 
                                onsorting="GridView1_Sorting" onrowdatabound="GridView1_RowDataBound">
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
                        </div>
                        
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
