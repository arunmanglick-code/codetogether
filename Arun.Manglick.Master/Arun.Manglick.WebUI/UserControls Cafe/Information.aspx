<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Information.aspx.cs" Inherits="TemplatePage" Title="Untitled Page" %>

<%@ Register src="UserControls/Information.ascx" tagname="Information" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Using Information User Control"></asp:Label>
            </td>
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
                            <li>Information UserControl</li>
                            <li>Requirement - How to access the controls of the User Control</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>                       
                        <table border="0" width="100%" cellpadding="1" cellspacing="3">                            
                            <!-- Row 1 -->
                            <tr>
                                <td colspan="3">                                    
                                    <uc1:Information ID="Information1" runat="server" />                                    
                                </td>
                            </tr>
                            <!-- Row 2 -->
                            <tr>
                                <td colspan="3">
                                    <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                                        Text="Using Infromation Control"></asp:Label>
                                </td>
                            </tr>
                             <!-- Row 3 -->
                            <tr>
                                <td colspan="3">                                    
                                       &nbsp;
                                </td>
                            </tr>
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblAssign" runat="server" Text="Assign Information"></asp:Label>
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Button ID="btnAssign"  class="button" runat="server" Text="Assign" OnClick="lnkAssign_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
