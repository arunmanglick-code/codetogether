<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JSStateOnVisibility.aspx.cs" Inherits="JSStateOnVisibility" Title="JavaScript Myth On Visibility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
<script type="text/javascript">
function MakeInvisible()
    {
        //debugger;
        obj1=document.getElementById('<%= lblVisible.ClientID %>');
        obj1.style.display='none';
        
        obj2=document.getElementById('<%= ddlVisible.ClientID %>');
        obj2.style.display='none';
        
        obj3=document.getElementById('<%= btnVisible.ClientID %>');
        obj3.style.display='none';
        
        obj4=document.getElementById('<%= txtVisible.ClientID %>');
        obj4.style.display='none';
    }
    
    
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="JSState Myth on Visibility"></asp:Label>
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
                    <div class="DivClassFeature" style="width:600px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>JavaScript State Myth on Visibility</li>
                            <li>Changes made at JS works regardless of the ViewState. i.e Control State is not dependent on Viewstate when changes are made at JS side</li> 
                            <li>However, the changes made at JS will be reset, once the postback occurs. i.e The controls will become visible again on 'SimplePostBack' operation</li>
                        </ol>
                        
                        Note: Here the 'Make Invisible' button is making controls invisble at JS and not making the postback<br />
                        Note: Here the EnableViewState = "True"
                        
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table border="1px";  width="25%" cellpadding="1" cellspacing="3">
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblVisible" runat="server" EnableViewState="true" Text="Check Label"></asp:Label>
                                </td>
                            </tr>                            
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label4" runat="server" Text="Drop Down"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:DropDownList ID="ddlVisible" EnableViewState="true" runat="server" class="inputfield">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label3" runat="server" Text="Button"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnVisible" runat="server" EnableViewState="true" CssClass="inputfield" Text="Check on Button" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label1" runat="server" Text="TextBox"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:TextBox ID="txtVisible" CssClass="inputfield" EnableViewState="true" runat="server" Text="Check on TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%" align="justify" colspan="2" class="labelcolumn">
                                    <asp:Label ID="Label2" ForeColor="Red" runat="server" Text="Run Below Test"></asp:Label>
                                </td>
                            </tr>                           
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnMakeInvisble" runat="server" EnableViewState="true" 
                                        CssClass="inputfield" Text="Make Invisible" OnClientClick="MakeInvisible(); return false;" />
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnSimplePostback" runat="server" EnableViewState="true" CssClass="inputfield" Text="Simple Postback"  />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
