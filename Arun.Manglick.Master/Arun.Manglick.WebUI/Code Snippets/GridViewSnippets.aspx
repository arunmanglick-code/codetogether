<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="GridViewSnippets.aspx.cs" Inherits="GridViewSnippets" Title="GridView Snippets Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
    <script src="../JS/Browser.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="GridView Snippets"></asp:Label></td>
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
                        <table border="0" width="100%" cellpadding="1" cellspacing="3">
                            <!-- Row 1 -->
                            <tr>
                                <td colspan="4">
                                    <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                                        Text="GridView Snippets"></asp:Label><br /><br />
                                </td>
                            </tr>
                           <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="lblScript1" runat="server" Text="Loop Thru Grid Columns/Cells"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad1" runat="server" OnClick="lnkNotePad1_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="lblScript2" runat="server" Text="Loop Thru All Controls In GridView"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad2" runat="server" OnClick="lnkNotePad2_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 49%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="lblScript3" runat="server" Text="Event Handlers for Controls Contained in Grid Veiw Row"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad3" runat="server" OnClick="lnkNotePad3_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label1" runat="server" Text="Sort and Also Customize the Sort Header"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkNotePad4_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>                                
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
