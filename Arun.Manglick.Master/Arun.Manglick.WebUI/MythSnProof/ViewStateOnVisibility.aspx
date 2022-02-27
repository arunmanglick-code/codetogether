<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewStateOnVisibility.aspx.cs" Inherits="ViewStateOnVisibility" Title="ViewState Myth On Visibility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="ViewState Myth on Visibility"></asp:Label>
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
                            <li>ViewState Myth on Visibility</li>
                            <li>Presently the ViewState is False/Disabled </li> 
                            <li>All controls are dependent on Viewstate. Here as the Viewstate is disabled, the controls will become visible again on 'SimplePostBack' operation</li>
                            <li>Try the same after making EnableViewState="true" for the controls. Doing this will not make the controls visible on postback </li>

                        </ol>
                                                
                        Note: Here the EnableViewState = "False".
                        
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
                                    <asp:Label ID="lblVisible" runat="server" EnableViewState="false" Text="Check Label"></asp:Label>
                                </td>
                            </tr>                            
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label4" runat="server" Text="Drop Down"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:DropDownList ID="ddlVisible" EnableViewState="false" runat="server" class="inputfield">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label3" runat="server" Text="Button"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnVisible" runat="server" EnableViewState="false" CssClass="inputfield" Text="Check on Button" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label1" runat="server" Text="TextBox"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:TextBox ID="txtVisible" CssClass="inputfield" EnableViewState="false" runat="server" Text="Check on TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%" align="justify" colspan="2" class="labelcolumn">
                                    <asp:Label ID="Label2" ForeColor="Red" runat="server" Text="Run Below Test"></asp:Label>
                                </td>
                            </tr>                           
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnMakeInvisble" runat="server" EnableViewState="false" 
                                        CssClass="inputfield" Text="Make Invisible" onclick="btnMakeInvisble_Click" />
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnSimplePostback" runat="server" EnableViewState="false" CssClass="inputfield" Text="Simple Postback" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
