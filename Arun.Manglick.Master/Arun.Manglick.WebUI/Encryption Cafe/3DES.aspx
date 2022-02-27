<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="3DES.aspx.cs" Inherits="TemplatePage" Title="Untitled Page" %>

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
                            <li>3DES Encryption</li>
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
                                        Text="3DES Encryption"></asp:Label>
                                </td>
                            </tr>
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblKey" runat="server" Text="Key"></asp:Label>
                                </td>
                                <td style="width: 79%" class="inputcolumn">
                                    <asp:TextBox ID="txtKey" runat="server" CssClass="inputfield" Text="ABCDEFGH" ></asp:TextBox>
                                </td>                                
                            </tr>
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnPrivateKey" runat="server" class="button" 
                                        Text="Get PrivateKey" onclick="btnPrivateKey_Click" />
                                </td>
                                <td>
                                </td>                                
                            </tr>
                            <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblPrivateKey" runat="server" Text="Private Key"></asp:Label>
                                </td>
                                <td style="width: 79%" class="inputcolumn">
                                    <asp:TextBox ID="txtPrivateKey" runat="server" CssClass="inputfield"></asp:TextBox>
                                </td>
                            </tr>
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblPrivateIV" runat="server" Text="Private IV"></asp:Label>
                                </td>
                                <td style="width: 79%" class="inputcolumn">
                                    <asp:TextBox ID="txtPrivateIV" runat="server" CssClass="inputfield"></asp:TextBox>
                                </td>
                            </tr>
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblToEncrypt" runat="server" Text="To Encrypt"></asp:Label>
                                </td>
                                <td style="width: 79%" class="inputcolumn">
                                    <asp:TextBox ID="txtToEncrypt" runat="server" CssClass="inputfield" Text="Triple DES"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnEncrypt" runat="server" class="button" Text="Encrypt" 
                                        onclick="btnEncrypt_Click" />
                                </td>                                
                            </tr>
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblToDecrypt" runat="server" Text="To Decrypt"></asp:Label>
                                </td>
                                <td style="width: 79%" class="inputcolumn">
                                    <asp:TextBox ID="txtToDecrypt" runat="server" CssClass="inputfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnDecrypt" runat="server" class="button" Text="Decrypt" 
                                        onclick="btnDecrypt_Click" />
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
                                    <asp:TextBox ID="txtFinal" runat="server" CssClass="inputfield"></asp:TextBox>
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
