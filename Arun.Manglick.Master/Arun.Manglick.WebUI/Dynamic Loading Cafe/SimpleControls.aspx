<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SimpleControls.aspx.cs" Inherits="SimpleControls" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Dynamically Adding Controls - Simple Controls"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" EnableViewState="false" Visible="true"></asp:Label>
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
                            <li>In case of 'Dynamic Control Loading', the controls must be generated on every postback (not just when the page is first loaded)</li>
                            <li>While regeneration, exactly the same order and with the same ID values must be maintained </li>
                            <li> Unlike control values, dynamically generated controls are not maintained in the viewstate of the page </li>
                            <li> However, values are maintained and will be reloaded after the controls have been created and added to the control tree.</li>
                        </ol>
                        
                        Note:
                        
                        <ol>
                            <li>The best event to use for Dynamic Loading is - Page_Load, instead of Init or Render.</li>
                            <li>In the Page_Load you'll have most success getting the process to work reliably, especially when wiring up event handlers.</li>
                        </ol>
                        
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
                                    <asp:PlaceHolder ID="PlaceHolderLabel" runat="server"></asp:PlaceHolder>
                                </td>
                            </tr>                            
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label4" runat="server" Text="Drop Down"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                   <asp:PlaceHolder ID="PlaceHolderDropDown" runat="server"></asp:PlaceHolder>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label3" runat="server" Text="Button"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:PlaceHolder ID="PlaceHolderButton" runat="server"></asp:PlaceHolder>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label1" runat="server" Text="TextBox"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:PlaceHolder ID="PlaceHolderTextBox" runat="server"></asp:PlaceHolder>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%" align="justify" colspan="2" class="labelcolumn">
                                    <asp:Label ID="Label2" ForeColor="Red" runat="server" Text="Run Below Test"></asp:Label>
                                </td>
                            </tr>                           
                            <tr>
                                <td style="width: 10%; text-align:center;" class="labelcolumn" colspan="2">
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
