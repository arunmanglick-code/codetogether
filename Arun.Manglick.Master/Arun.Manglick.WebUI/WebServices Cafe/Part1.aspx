<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Part1.aspx.cs" Inherits="Part1" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="LINQ Queries"></asp:Label></td>
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
                            <li>Enter Text</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat">
                            <asp:Button ID="btnDataTableReturnType" Text="DataTable - Retrun Types" CssClass="button" 
                                runat="server" OnClick="btnDataTableReturnType_Click" Width="150px" /><br />
                             <asp:Button ID="Button1" Text="Converted DataTable - Retrun Types" CssClass="button" 
                                runat="server" OnClick="btnConvertedDataTableReturnType_Click" Width="150px" /><br />
                            <asp:Button ID="btnArrayListReturnType" Text="ArrayList - Retrun Types" CssClass="button" 
                                runat="server" OnClick="btnArrayListReturnType_Click" Width="150px" /><br />
                            <asp:Button ID="Button2" Text="Generic List - Retrun Types" CssClass="button" 
                                runat="server" OnClick="btnGenericListReturnType_Click" Width="150px" /><br />
                          <asp:Button ID="Button3" Text="Hash Table - Retrun Types" CssClass="button" 
                                runat="server" OnClick="btnHashTableReturnType_Click" Width="150px" /><br />
                           <br />
                        </div>
                        
                        <div style="height:200px;">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                AutoGenerateColumns="true" AllowSorting="True">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" />                                
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:GridView>
                        </div>
                        <div style="height:200px;">
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="inputfield">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
