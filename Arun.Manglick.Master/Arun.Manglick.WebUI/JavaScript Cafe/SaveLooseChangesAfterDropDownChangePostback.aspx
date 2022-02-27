<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SaveLooseChangesAfterDropDownChangePostback.aspx.cs" Inherits="SaveLooseChangesAfterDropDownChangePostback" Title="Save Loose Changes After DropDown Change Postback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server"> 
    <script src="../JS/Common.js" type="text/javascript"></script>
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
                    <div class="DivClassFeature" style="width:500px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>See how the 'Dirty' variable is used to maintain the Unsaved state of the page</li>
                            <li>No Hidden Variable is used.</li>                         
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
                                        Text="Prompt Loose Changes"></asp:Label>
                                </td>
                            </tr>                            
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="labelcolumn">
                                    <asp:Label ID="lblChange" runat="server" Text="Enter Text"></asp:Label>
                                </td>
                                <td style="width: 5%" class="inputcolumn">
                                    <asp:TextBox ID="txtChange" onchange="Javascript:HandleChangeEvent();" runat="server"></asp:TextBox>                                    
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                           </tr>  
                            <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="labelcolumn">
                                    <asp:Label ID="lblCheckPass" runat="server" Text="Pass"></asp:Label>
                                </td>
                                <td style="width: 5%" class="inputcolumn">
                                    <asp:CheckBox ID="chkCheckPass" onMouseDown="Javascript:HandleChangeEvent();" runat="server" />            
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                           </tr> 
                           <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="labelcolumn">
                                    <asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label>
                                </td>
                                <td style="width: 5%" class="inputcolumn">
                                    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" 
                                        onchange="Javascript:HandleChangeEvent();" 
                                        onselectedindexchanged="ddlCountry_SelectedIndexChanged">
                                        <asp:ListItem>aa</asp:ListItem>
                                        <asp:ListItem>bb</asp:ListItem>
                                        <asp:ListItem>cc</asp:ListItem>
                                    </asp:DropDownList>&nbsp;&nbsp;(Makes PostBack)
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    
                                </td>
                           </tr> 
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="inputcolumn">
                                    <asp:Button ID="btnClose" runat="server" Width="150px" class="button" Text="Close" OnClick="btnClose_Click" />
                                </td>
                                <td style="width: 5%" class="labelcolumn">
                                    <asp:Button ID="btnSave" runat="server" Width="150px" class="button" Text="Save" OnClientClick="Javascript:SaveEventCalled();alert('Saved Successfully');" OnClick="btnSave_Click" />
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                             </tr>                            
                        </table>    
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
