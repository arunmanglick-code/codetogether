<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomValidationDate.aspx.cs" Inherits="CustomValidation1" Title="Custom Validation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">


    <script src="../JS/Calendar.js" type="text/javascript"></script>
    <script src="../JS/Calendar_new.js" type="text/javascript"></script>
    <script src="../JS/Common.js" type="text/javascript"></script>

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
                <asp:Label ID="lblNoError" runat="server" EnableViewState="false" CssClass="validation-no-error"></asp:Label>
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
                    <div class="DivClassFeature" style="width: 600px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Usage of Custom Validator</li>
                            <li>Notice the use of 'ValidateEmptyText' property.</li>
                            <li>This property is required, otherwise CustomValidator bydefault, does not fire even
                                if the value is blank</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table border="0">
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblDateFrom" runat="server" Text="Date From"></asp:Label>
                                    <img src="../Images/calendar-icon.gif"
                                </td>
                                <td style="width: 2%" class="requiredcolumn">
                                    <asp:RegularExpressionValidator ID="revDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                        Text="<img src='../Images/error-icon.gif'/>" ErrorMessage="Date format is MM/DD/YYYY"
                                        ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"></asp:RegularExpressionValidator>
                                </td>
                                <td style="width: 57%" class="inputcolumn" valign="middle">
                                    <asp:TextBox ID="txtDateFrom" runat="server" ToolTip="MM/DD/YYY" CssClass="inputfield"></asp:TextBox>
                                    <img id="imgThroughDate" runat="server" alt="Select From Date" src="../Images/calendar-icon.gif"
                                        align='absmiddle' onmouseover="javascript:this.style.cursor='hand';" onclick="return showDateFromCalendar();" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblDateThrough" runat="server" Text="Date Through "></asp:Label>
                                </td>
                                <td>
                                
                                </td>
                                <td style="width: 59%" class="inputcolumn" valign="middle">
                                    <asp:TextBox ID="txtThroughDate" runat="server" ToolTip="MM/DD/YYY" CssClass="inputfield"></asp:TextBox>
                                    <img id="imgFromDate" runat="server" alt="Select From Date" src="../Images/calendar-icon.gif"
                                        align='absmiddle' onmouseover="javascript:this.style.cursor='hand';" onclick="return showDateThruCalendar();" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnCancel" runat="server" class="button" Text="Cancel"
                                        CausesValidation="False" />
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnFilter" runat="server" class="button" Text="Submit" /><br />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
