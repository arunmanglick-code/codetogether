<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ObjectToDataTable.aspx.cs" Inherits="Reflection_ObjectToDataTable"
    Title="Untitled Page" Theme="MyTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Reflection - Object to DataTable"></asp:Label>
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
                            <li>Using Reflection</li>
                            <li>Generate DataTable from an Object Structure using Reflection</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat" style="width: 200px">
                            <b>GridView using Refelection</b><br /><br />
                            <br />
                            <asp:GridView ID="ReflectionGrid" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateSelectButton="true" BackColor="White" BorderColor="#3366CC" BorderStyle="None"
                                BorderWidth="1px" Width="190px" HorizontalAlign="Center" CellPadding="4" PageSize="5">
                                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                <RowStyle BackColor="White" ForeColor="#003399" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
