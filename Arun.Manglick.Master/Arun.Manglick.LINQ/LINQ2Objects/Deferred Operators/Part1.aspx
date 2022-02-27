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
                            <asp:Button ID="btnQuery1" Text="Restriction - Where" CssClass="button" 
                                runat="server"    OnClick="btnQuery1_Click" Width="150px" /><br /><br />
                                
                            <asp:Button ID="btnQuery2" Text="Projection - Select" CssClass="button" 
                                runat="server" OnClick="btnQuery2_Click" Width="150px" /><br />
                            <asp:Button ID="btnQuery3" Text="Projection - SelectMany" CssClass="button" 
                                runat="server"  OnClick="btnQuery3_Click" Width="150px" /><br /><br />
                                
                            <asp:Button ID="btnQuery4" Text="Partitioning - Skip" CssClass="button" 
                                runat="server"  OnClick="btnQuery4_Click" Width="150px" /><br />
                            <asp:Button ID="btnQuery5" Text="Partitioning - SkipWhile" CssClass="button" 
                                runat="server"  OnClick="btnQuery5_Click" Width="150px" /><br />
                            <asp:Button ID="btnQuery6" Text="Partitioning - Take" CssClass="button" 
                                runat="server"  OnClick="btnQuery6_Click" Width="150px" /><br />                                
                            <asp:Button ID="btnQuery7" Text="Partitioning - TakeWhile" CssClass="button" 
                            runat="server"  OnClick="btnQuery7_Click" Width="150px" /><br /><br />
                            
                            <asp:Button ID="btnQuery8" Text="Concat" CssClass="button" 
                            runat="server"  OnClick="btnQuery8_Click" Width="150px" /><br /><br />
                            
                            <asp:Button ID="btnQuery9" Text="OrderBy" CssClass="button" 
                            runat="server"  OnClick="btnQuery9_Click" Width="150px" /><br />
                            <asp:Button ID="Button1" Text="ThenBy" CssClass="button" 
                            runat="server"  OnClick="btnQuery10_Click" Width="150px" /><br />
                             <asp:Button ID="Button2" Text="Reverse" CssClass="button" 
                            runat="server"  OnClick="btnQuery11_Click" Width="150px" /><br /><br />
                            
                            <asp:Button ID="Button11" Text="Distinct" CssClass="button" 
                            runat="server"  OnClick="Distinct_Click" Width="150px" /><br />
                            <asp:Button ID="Button12" Text="Except" CssClass="button" 
                            runat="server"  OnClick="Except_Click" Width="150px" /><br />
                            <asp:Button ID="Button13" Text="Intersect" CssClass="button" 
                            runat="server"  OnClick="Intersect_Click" Width="150px" /><br />
                             <asp:Button ID="Button14" Text="Union" CssClass="button" 
                            runat="server"  OnClick="Union_Click" Width="150px" /><br /><br />                            
                            
                            <asp:Button ID="Button10" Text="DefaultIfEmpty" CssClass="button" 
                            runat="server"  OnClick="btnQuery111_Click" Width="150px" /><br />
                            <asp:Button ID="Button3" Text="ElementAt" CssClass="button" 
                            runat="server"  OnClick="btnQuery12_Click" Width="150px" /><br />
                            <asp:Button ID="Button4" Text="First" CssClass="button" 
                            runat="server"  OnClick="btnQuery13_Click" Width="150px" /><br />
                             <asp:Button ID="Button5" Text="Last" CssClass="button" 
                            runat="server"  OnClick="btnQuery14_Click" Width="150px" /><br />
                             <asp:Button ID="Button6" Text="Single" CssClass="button" 
                            runat="server"  OnClick="btnQuery15_Click" Width="150px" /><br /><br />
                            
                            <asp:Button ID="Button7" Text="Empty" CssClass="button" 
                            runat="server"  OnClick="btnQuery16_Click" Width="150px" /><br />
                             <asp:Button ID="Button8" Text="Range" CssClass="button" 
                            runat="server"  OnClick="btnQuery17_Click" Width="150px" /><br />
                             <asp:Button ID="Button9" Text="Repeat" CssClass="button" 
                            runat="server"  OnClick="btnQuery18_Click" Width="150px" /><br /><br />
                            
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
