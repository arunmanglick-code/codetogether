<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UsingCheckBoxField.aspx.cs" Inherits="UsingButtonField"
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
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>                          
                            <li>Can be used to represent a Custom command or </li>
                            <li>Can be used to represent one of the standard edit commands (Edit, New, Delete,Select)</li>                            
                        </ol>
                    </div>
                    <br />
                    <br />
                    <div>
                        <div class="DivClassFloat">
                            <asp:Button ID="btnSubmit" Text="Search" runat="server"  />
                        </div>
                        <div style="height:300px;">
                             <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                AutoGenerateColumns="False" AllowSorting="True"  Width="80%" onrowcommand="GridView1_RowCommand" 
                                >
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099"  />
                                <Columns>                                   
                                    <asp:CheckBoxField DataField="Active" HeaderText="Active" />
                                    <asp:ButtonField CommandName="Select" Text="Select" />
                                    <asp:BoundField DataField="CourseId" SortExpression="CourseId" Visible="true"  HeaderText="CourseId" />
                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                    <asp:BoundField DataField="Institution" HeaderText="Institution" />
                                    <asp:BoundField DataField="Course" HeaderText="Course" />                                    
                                    
                                   
                                    
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
