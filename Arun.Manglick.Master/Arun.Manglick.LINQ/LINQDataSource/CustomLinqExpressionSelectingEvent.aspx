<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomLinqExpressionSelectingEvent.aspx.cs" Inherits="CustomLinqExpressionSelectingEvent" Title="Custom LINQ Expression" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Custom LINQ Expression"></asp:Label></td>
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
                            <li>Using Custom LINQ Expression</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat">
                          
                        </div>
                        <div style="height:200px;">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True"  
                                DataSourceID="LinqDataSource1">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" />                                
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:BoundField DataField="ProductID" HeaderText="ProductID" ReadOnly="True" 
                                        SortExpression="ProductID" />
                                    <asp:BoundField DataField="ProductName" HeaderText="ProductName" 
                                        ReadOnly="True" SortExpression="ProductName" />
                                    <asp:BoundField DataField="QuantityPerUnit" HeaderText="QuantityPerUnit" 
                                        ReadOnly="True" SortExpression="QuantityPerUnit" />
                                    <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" ReadOnly="True" 
                                        SortExpression="UnitPrice" />
                                    <asp:BoundField DataField="CategoryID" HeaderText="CategoryID" ReadOnly="True" 
                                        SortExpression="CategoryID" />
                                    <asp:BoundField DataField="UnitsInStock" HeaderText="UnitsInStock" 
                                        ReadOnly="True" SortExpression="UnitsInStock" />
                                </Columns>
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:GridView>
                             <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
                                ContextTypeName="NorthwindDataContext" 
                                TableName="MyProducts"                                
                                EnableDelete="True" EnableInsert="True" 
                                EnableUpdate="True">                                
                            </asp:LinqDataSource>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
