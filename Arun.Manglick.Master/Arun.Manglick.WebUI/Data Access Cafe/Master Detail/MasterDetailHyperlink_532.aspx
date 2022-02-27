<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MasterDetailHyperlink_532.aspx.cs" Inherits="Data_Access_Cafe_Master_Detail_MasterDetail_488"
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
                            <li>Master - GridView</li>
                            <li>Child - DetailsView</li>
                            <li>Use 'QueryStringParameter' and 'QueryStringField' in the Child Grid</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat" style="width: 200px">
                            <b>Master - Grid View</b><br /><br />
                            <br />
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" BackColor="White" BorderColor="#336666"
                                BorderWidth="3px" CellPadding="4" DataKeyNames="Id" 
                                DataSourceID="srcMoviesMaster" GridLines="Horizontal" PageSize="5" 
                                Width="190px" HorizontalAlign="Center" BorderStyle="Double">
                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                <RowStyle BackColor="White" ForeColor="#333333" />
                                <Columns>
                                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataTextField="Name" DataNavigateUrlFormatString="MasterDetailHyperlink_532.aspx?Id={0}" />
                                </Columns>
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="srcMoviesMaster" runat="server" ConnectionString="<%$ ConnectionStrings:MoviesConnectionString %>"
                                SelectCommand="SELECT [Id], [Name] FROM [MovieCategories]"></asp:SqlDataSource>
                        </div>
                        <br />
                        <div class="DivClassFloat" style="width: 200px">
                            <b>Child - Grid View</b><br /><br />
                            <br />
                            <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="190px" HorizontalAlign="Center" BackColor="White"
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="srcMoviesDetail">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <EditRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:DetailsView>
                            <asp:SqlDataSource ID="srcMoviesDetail" runat="server" ConnectionString="<%$ ConnectionStrings:MoviesConnectionString %>"
                                SelectCommand="SELECT [Title], [Director], [CategoryId] FROM [Movies] WHERE ([CategoryId] = @CategoryId)">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="CategoryId" QueryStringField="Id" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
            </td>
        </tr>
    </table>
</asp:Content>
