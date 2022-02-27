<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RegEx.aspx.cs" Inherits="RegEx" Title="Regular Expression" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script src="../JS/WindowOpen.js" type="text/javascript"></script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Your Text"></asp:Label>
            </td>
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
                        <table border="1" width="45%" cellpadding="1" cellspacing="3">
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblValue" runat="server" Text="Value to Test"></asp:Label>
                                </td>
                                <td style="width: 2%">
                                    <asp:RegularExpressionValidator ID="revMoveRow" runat="server" ControlToValidate="txtValue"
                                        Text="<img src='../Images/error-icon.gif'/>" ErrorMessage="RegEx Fail" Display="Dynamic"></asp:RegularExpressionValidator>
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:TextBox ID="txtValue" Width="250px" CssClass="inputfield" runat="server" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn" colspan="2">
                                    <asp:Label ID="lblRegEx" runat="server" Text="Regular Expression"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:TextBox ID="txtRegEx" Width="250px" CssClass="inputfield" runat="server" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%" align="justify" colspan="3" class="labelcolumn">
                                    <asp:Label ID="Label2" ForeColor="Red" runat="server" Text="Run Below Test"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn" colspan="2">
                                    <asp:Button ID="btnSetRegEx" runat="server" EnableViewState="false" CausesValidation="false"
                                        CssClass="inputfield" Text="Set Regular Expression" OnClick="btnSetRegEx_Click" />
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnCheckRegEx" runat="server" EnableViewState="false" CssClass="inputfield"
                                        Text="Check RegEx" OnClick="btnCheckRegEx_Click" />
                                </td>
                            </tr>
                        </table>
                       <br /><br />
                       <a href="#" onclick="OpenSmallWindow('ExampleRegEx.aspx');" class="label">Reference RegEx</a>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
