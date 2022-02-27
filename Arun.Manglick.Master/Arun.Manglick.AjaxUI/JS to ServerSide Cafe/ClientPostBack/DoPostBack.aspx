<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DoPostBack.aspx.cs" Inherits="DoPostBack" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
    <script src="../JS/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
    function Show()
    {
        var status = window.confirm('Do you want to continue?');
        
        if(status)
        {
            var btn = document.getElementById('<%=btnSimple.ClientID %>');
            btn.click();
        }
        else
        {
            return false;
        }
    }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Which Control Made PostBack"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="true"></asp:Label>
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
                    <div class="DivClassFeature" style="width:600px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>See the use of calling Server Side 'Button Click' event handler code from Client Side</li>
                            <li>To Test, toggle the Checkbox selection</li>
                            <li>Though here we are firing the Click event of Button from Client side, it still requires this binding as, OnClick="btnSimple_Click"</li>
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
                                        Text="DoPostBack"></asp:Label>
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
                                    <asp:TextBox ID="txtChange" runat="server"></asp:TextBox>                                    
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
                                    <asp:CheckBox ID="chkCheckPass" AutoPostBack="false" runat="server" onclick="Show();"/>            
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                           </tr> 
                           <!-- Row 3 -->                           
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>                                
                                <td style="width: 5%" class="labelcolumn">
                                    <asp:Button ID="btnSimple" runat="server" Width="150px" class="button" Text="Simple Postback" OnClick="btnSimple_Click" />
                                </td>
                                <td style="width: 2%" class="inputcolumn">
                                    <asp:Button ID="btnReset" runat="server" Width="150px" class="button" Text="Reset" OnClick="btnReset_Click" />
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
