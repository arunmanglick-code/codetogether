<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UsingCommandField.aspx.cs" Inherits="UsingCommandField" EnableEventValidation="false"
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
                            <li>Paging Style</li>
                            <li>There are multiple styles as below</li>
                            <ul>
                            <li>NextPrev</li>
                            <li>Numeric</li>
                            <li>NextPreviousFirstLast</li>
                            <li>NumericFirstLast - Current</li>
                            </ul>
                            <li>Note the use of 'PageButtonCount' property</li>
                            <li>Change PagerSettings Attribute in Property Dialog to see the effect</li>
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
                                AutoGenerateColumns="False" AllowSorting="True"  Width="80%" 
                                onrowediting="GridView1_RowEditing" 
                                 onrowcancelingedit="GridView1_RowCancelingEdit" 
                                 onrowdeleting="GridView1_RowDeleting" onrowupdating="GridView1_RowUpdating">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099"  />
                                <Columns>                                   
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:BoundField DataField="CourseId" SortExpression="CourseId" HeaderText="CourseId" />
                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                    <asp:BoundField DataField="Institution" HeaderText="Institution" />
                                    <asp:BoundField DataField="Course" HeaderText="Course" />                                    
                                    
                                    <asp:CommandField ShowEditButton="True"  />
                                    <asp:CommandField ShowDeleteButton="True" />
                                    
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
