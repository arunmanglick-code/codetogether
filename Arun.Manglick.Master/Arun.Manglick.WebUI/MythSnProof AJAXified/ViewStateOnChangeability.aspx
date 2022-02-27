<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewStateOnChangeability.aspx.cs" Inherits="ViewStateOnChangeability" Title="ViewState Myth On Changeability" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="ViewState Myth on Changeability"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Change="false"></asp:Label>
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
                            <li>ViewState Myth on Change Ability</li>
                            <li>Presently the ViewState is False/Disabled, except for one TextBox </li> 
                            <li>Only Label & Button Type, requires View State to be enabled. i.e It will default to its declarative value whenever Postback</li>
                            <li>The Textbox is not dependent on Viewstate. Irrespective of ViewState it maintains the changed values even after PostBack</li>
                            <li>Try the same after making EnableViewState="true" for the Label and Button control. Doing this will make the controls to maintain their changed state </li>
                        </ol>
                        
                        <ol>
                            <li>Note - If the Textbox control is of 'Password' mode then, neither the declarative value is displayed, nor the programmatically assigned values being shown. Only the values entered at runtime will be shown. And Above all the ViewState is not maintained for Textbos with Password mode</li>
                        </ol>
                        
                        Note: Here the EnableViewState = "False", except for one textbox.
                        
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table border="1px";  width="40%" cellpadding="1" cellspacing="3">
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblChange" runat="server" EnableViewState="false" Text="Initial"></asp:Label>
                                </td>
                            </tr>                            
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label4" runat="server" Text="Drop Down"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:DropDownList ID="ddlChange" EnableViewState="false" runat="server" class="inputfield">
                                        <asp:ListItem>Initial</asp:ListItem>
                                        <asp:ListItem>Changed</asp:ListItem>                                        
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:Label ID="Label3" runat="server" Text="Button"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnChange" runat="server" EnableViewState="false" CssClass="inputfield" Text="Initial" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label1" runat="server" Text="TextBox - ViewState Disabled"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:TextBox ID="txtChange1" CssClass="inputfield" EnableViewState="false" runat="server" Text="Initial"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label5" runat="server" Text="TextBox - ViewState Enabled"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:TextBox ID="txtChange2" CssClass="inputfield" EnableViewState="true" runat="server" Text="Initial"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label6" runat="server" Text="TextBox (Password) - ViewState Enabled"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:TextBox ID="txtChangePassword" CssClass="inputfield" EnableViewState="true" TextMode="Password" runat="server" Text="Initial"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label7" runat="server" Text="Hidden Field"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:HiddenField ID="hdnField" EnableViewState="false" runat="server" />
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
                                        CssClass="inputfield" Text="Make Change" onclick="btnMakeInvisble_Click" />
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnSimplePostback" runat="server" EnableViewState="true" 
                                        CssClass="inputfield" Text="Simple Postback" 
                                        onclick="btnSimplePostback_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
