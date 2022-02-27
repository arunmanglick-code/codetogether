<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DSA.aspx.cs" Inherits="DSA" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Triple DES (3DES) Encryption"></asp:Label>
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
                            <li>DSA Encryption</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table border="0" width="100%" cellpadding="1" cellspacing="3">
                            <!-- Row 1 -->
                            <tr>
                                <td colspan="6">
                                    <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                                        Text="DSA Encryption"></asp:Label>
                                </td>
                            </tr>
                            <!-- Row 2 -->                            
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblToEncrypt" runat="server" Text="To Encrypt"></asp:Label>
                                </td>
                                <td style="width: 79%" class="inputcolumn">
                                    <asp:TextBox ID="txtToEncrypt" runat="server" CssClass="inputfield" Text="DSA"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSignData" runat="server" class="button" 
                                        Text="Sign Data" onclick="btnSignData_Click" />
                                </td>
                                <td>
                                </td>                                
                            </tr>
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblDigitalSignature" runat="server" Text="Digital Signature"></asp:Label>
                                </td>
                                <td style="width: 79%" class="inputcolumn">
                                    <asp:TextBox ID="txtDigitalSignature" runat="server" CssClass="inputfield"></asp:TextBox>
                                </td>
                            </tr>                          
                            
                            <!-- Row 4 -->
                            
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnVeriphyData" runat="server" class="button" Text="Veriphy Data" 
                                        onclick="btnVeriphyData_Click" />
                                </td>                                
                            </tr>
                            
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblFinal" runat="server" Text="Final"></asp:Label>
                                </td>
                                <td style="width: 79%" class="inputcolumn">
                                    <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnClearFields" runat="server" class="button" 
                                        Text="ClearFields" onclick="btnClearFields_Click" />
                                </td>                                
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
