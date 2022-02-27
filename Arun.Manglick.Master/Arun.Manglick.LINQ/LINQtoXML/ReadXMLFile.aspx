<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ReadXMLFile.aspx.cs" Inherits="ReadXMLFile" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server"><a href="../Simple/Part1.aspx">../Simple/Part1.aspx</a>
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="LINQ To XML"></asp:Label></td>
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
                            <li>LINQ to XML  - To perform LINQ queries over XML that either retrieved from the file-system</li>
                            <li>Als0 notice the use of C# ?? null coalescing operator</li>
                            <li>'Read Remote RSS Feed' button will not work without Internet connection</li>
                        </ol>
                        
                        <ol>
                            <li>Notice - In Code, 'ReadRemoteRSS()' perform a further sub-query on the result to filter the latest posts </li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat">
                            <asp:Button ID="btnQuery1" Text="Read XML File" CssClass="button" 
                                runat="server"    OnClick="btnQuery1_Click" Width="150px" /><br />
                            <asp:Button ID="btnQuery2" Text="Read Hierarchichal XML File" CssClass="button" 
                                runat="server" OnClick="btnQuery2_Click" Width="150px" /><br />
                            <asp:Button ID="btnQuery3" Text="Read Remote RSS Feed" CssClass="button" 
                                runat="server"  OnClick="btnQuery3_Click" Width="150px" /><br />                            
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
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
