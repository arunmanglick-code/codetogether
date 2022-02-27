<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BrowserDependent.aspx.cs" Inherits="BrowserDependent" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
    <script src="../JS/BrowserDependency.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Your Text"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
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
                         <br />
                        <br />
                        <table class="Border" width="100%" cellpadding="1" cellspacing="3">
                            <!-- Row 1 -->
                            <tr>
                                <td colspan="4">
                                    <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                                        Text="Browser Dependent Script"></asp:Label><br /><br />
                                </td>
                            </tr>                            
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblScript1" runat="server" Text="Script 1"></asp:Label>
                                </td>
                                <td style="width: 15%" class="inputcolumn">
                                    <a href='#' onclick='CheckBrowser1();'>Check Browser</a>
                                </td>
                                <td style="width: 74%; text-align:left" class="requiredcolumn">
                                    Simple Script
                                </td>
                           </tr>  
                           <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblScript2" runat="server" Text="Script 2"></asp:Label>
                                </td>
                                <td style="width: 15%" class="inputcolumn">
                                    <a href='#' onclick='CheckBrowser2();'>Check Browser</a>
                                </td>
                                <td style="width: 74%; text-align:left" class="requiredcolumn">
                                    Complex Script
                                </td>
                           </tr>  
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
