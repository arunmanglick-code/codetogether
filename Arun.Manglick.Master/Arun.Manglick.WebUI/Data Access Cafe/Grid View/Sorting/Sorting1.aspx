<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sorting1.aspx.cs" Inherits="Sorting1"
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
                            <li>Sorting GridView Data</li>
                            <li>Requirement - No DataSource is used, instead programmatically BindGrid() is used.</li>
                            <li>Learnings - In the Sorting event, the e.SortDirection always comes 'Ascending'</li>
                            <li>If you set AllowPaging="true" or AllowSorting="true" on a GridView control without using a DataSourceControl DataSource (i.e. SqlDataSource, ObjectDataSource), you will run into the following errors:
                            <ul>
                                <li>The GridView 'GridViewID' fired event PageIndexChanging which wasn't handled.
                                </li>
                                <li>As a result of not setting the DataSourceID property of the GridView to a DataSourceControl DataSource, you have to add event handlers for sorting and paging.</li>
                                <li>gridView_PageIndexChanging</li>
                                <li>gridView_Sorting</li>
                            </ul>
                            </li>
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
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                AutoGenerateColumns="false" AllowSorting="True" EmptyDataText="No Records found"  
                                onsorting="GridView1_Sorting" onrowdatabound="GridView1_RowDataBound">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <Columns>
                                    <asp:BoundField DataField="CourseId" SortExpression="CourseId" HeaderText="CourseId" />
                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                    <asp:BoundField DataField="Institution" HeaderText="Institution" />
                                    <asp:BoundField DataField="Course" HeaderText="Course" />
                                    <asp:TemplateField HeaderText="Average on Label" SortExpression="Average">
                                        <HeaderTemplate>                                            
                                            <asp:Label ID="Label2" runat="server" Text="My Header"></asp:Label>
                                        </HeaderTemplate>
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
