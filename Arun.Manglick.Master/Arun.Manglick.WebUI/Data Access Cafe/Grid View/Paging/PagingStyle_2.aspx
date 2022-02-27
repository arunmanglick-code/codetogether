<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PagingStyle_2.aspx.cs" Inherits="Data_Access_Cafe_Grid_View_VariousFeatures"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="GridView Paging - PagerTemplate"></asp:Label></td>
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
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>                          
                            <li>Paging usiing PageTemplate</li>
                            <li>Two LinkButtons for Prev & Next</li>
                            <li>Menu control for Numeric Page Numbers</li>
                            <li>Notice the use of 'BottomPagerRow' in Code - GridView1_DataBound</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <div>
                        <div class="DivClassFloat">
                            <asp:TextBox ID="txtTitle" runat="server" />
                            <asp:Button ID="btnSubmit" Text="Search" runat="server" OnClick="btnSubmit_Click" />
                        </div>
                        <div style="height:200px;">
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan"
                                BorderWidth="1px" CellPadding="2" DataKeyNames="Id" DataSourceID="srcMovies"
                                ForeColor="Black" GridLines="None" PageSize="5" AutoGenerateEditButton="True" 
                                OnRowCreated="GridView1_RowCreated" ondatabound="GridView1_DataBound">
                                <FooterStyle BackColor="Tan" />
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True"
                                        SortExpression="Id">
                                        <HeaderStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                                    <asp:BoundField DataField="Director" HeaderText="Director" SortExpression="Director" />
                                    <asp:BoundField DataField="DateReleased" HeaderText="DateReleased" SortExpression="DateReleased" />
                                    <asp:TemplateField HeaderText="Template" SortExpression="Title">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="True" />
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                <PagerTemplate>
                                    <table>
                                        <tr>
                                            <td><asp:LinkButton ID="lnkPrev" Text="Prev" ToolTip="Previous Page" CommandName="Page" CommandArgument="Prev" runat="server" /></td>
                                            <td>
                                                <asp:Menu ID="mnuPage" Orientation="Horizontal" runat="server" 
                                                    onmenuitemclick="mnuPage_MenuItemClick">
                                                </asp:Menu>
                                            </td>
                                            <td><asp:LinkButton ID="lnkNext" Text="Next" ToolTip="Next Page" CommandName="Page" CommandArgument="Next" runat="server" /></td>
                                        </tr>
                                    </table>                                    
                                </PagerTemplate>
                                <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                                    HorizontalAlign="Right" />
                                <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="PaleGoldenrod" />                                
                            </asp:GridView>
                            <asp:SqlDataSource ID="srcMovies" runat="server" ConnectionString="<%$ ConnectionStrings:MoviesConnectionString %>"
                                SelectCommand="SELECT [Id], [Title], [Director], [DateReleased] FROM [Movies]"
                                DeleteCommand="DELETE FROM [Movies] WHERE [Id] = @Id" InsertCommand="INSERT INTO [Movies] ([Title], [Director], [DateReleased]) VALUES (@Title, @Director, @DateReleased)"
                                UpdateCommand="UPDATE [Movies] SET [Title] = @Title, [Director] = @Director, [DateReleased] = @DateReleased WHERE [Id] = @Id">
                                <DeleteParameters>
                                    <asp:Parameter Name="Id" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="Title" Type="String" />
                                    <asp:Parameter Name="Director" Type="String" />
                                    <asp:Parameter Name="DateReleased" Type="DateTime" />
                                    <asp:Parameter Name="Id" Type="Int32" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="Title" Type="String" />
                                    <asp:Parameter Name="Director" Type="String" />
                                    <asp:Parameter Name="DateReleased" Type="DateTime" />
                                </InsertParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
