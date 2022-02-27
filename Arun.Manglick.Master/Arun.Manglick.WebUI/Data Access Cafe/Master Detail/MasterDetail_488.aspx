<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MasterDetail_488.aspx.cs" Inherits="Data_Access_Cafe_Master_Detail_MasterDetail_488"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Master Detail"></asp:Label>
            </td>
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
                    <!-- Features Div -->
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Master Detail</li>
                            <li>Both Master & Child are GridView</li>
                            <li>Use 'ControlParameter' and 'SelectedDataKey("Id")' in the Child Grid</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat" style="width: 200px">
                            <b>Master - Grid View</b><br /><br />
                            <asp:GridView ID="MasterGrid" runat="server" AllowPaging="True" AllowSorting="True"
                                BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" Width="190px" HorizontalAlign="Center"
                                CellPadding="4" DataSourceID="srcMaster" PageSize="5" DataKeyNames="Id" AutoGenerateSelectButton="True">
                                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                <RowStyle BackColor="White" ForeColor="#003399" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="srcMaster" runat="server" ConnectionString="<%$ ConnectionStrings:MoviesConnectionString %>"
                                SelectCommand="SELECT [Id], [Name] FROM [MovieCategories]"></asp:SqlDataSource>
                        </div>
                        <br />
                        <div class="DivClassFloat" style="width: 200px">
                            <b>Child - Grid View</b><br /><br />
                            <asp:GridView ID="ChildGridView1" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" CellSpacing="2" DataSourceID="srChild" PageSize="5"
                                Width="190px" HorizontalAlign="Center">
                                <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                                <Columns>
                                    <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                                    <asp:BoundField DataField="Director" HeaderText="Director" SortExpression="Director" />
                                </Columns>
                                <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="srChild" runat="server" ConnectionString="<%$ ConnectionStrings:MoviesConnectionString %>"
                                SelectCommand="SELECT [Title], [Director] FROM [Movies] WHERE ([CategoryId] = @CategoryId)">
                                <SelectParameters>
                                    <asp:ControlParameter Name="CategoryId" Type="Int32" ControlID="MasterGrid" PropertyName="SelectedDataKey('Id')" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
            </td>
        </tr>
    </table>
</asp:Content>
