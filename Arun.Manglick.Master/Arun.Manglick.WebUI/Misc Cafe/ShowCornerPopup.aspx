<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ShowCornerPopup.aspx.cs" Inherits="ShowCornerPopup" Title="Show Corner Popup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

<script type="text/javascript">

</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="JS State Myth on Changeability"></asp:Label>
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
                        <b>Varoius Features Used - AJAXified</b>
                        <ol>
                            <li>JavaScript State Myth on Change Ability</li>
                            <li>Changes made at JS works regardless of the ViewState. i.e Control State is not dependent on Viewstate when changes are made at JS side </li> 
                            <li>Mostly the changes made at JS will be reset, once the postback occurs. i.e The controls will set to their decalrative values again on 'SimplePostBack' operation </li>
                            <li>But, still the state for DropDown, TextBox & Hidden Field, does not reset, even after the postback occurs</li>
                            <li>i.e These three controls maintains the state which is changed in Javascript, at ServerSide as well</li>
                        </ol>
                        
                        Note: Here the 'Make Change' button is making controls disable at JS and not making the postback <br />
                        Note: Here the EnableViewState = "False", except for last two textboxes                        
                        
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table border="1px";  width="35%" cellpadding="1" cellspacing="3">
                                <tr>
                                    <td colspan="2">
                                        
                                    </td>
                                </tr>                                                         
                                <tr>
                                    <td style="width: 10%" class="labelcolumn">
                                        <asp:Button ID="btnMakeChange" runat="server" EnableViewState="false" 
                                            CssClass="inputfield" Text="Full PostBack" onclick="btnMakeChange_Click" />
                                    </td>
                                    <td style="width: 10%" class="labelcolumn">
                                        <asp:Button ID="btnSimplePostback" runat="server" EnableViewState="true" 
                                            CssClass="inputfield" Text="AJAX Postback" 
                                            onclick="btnSimplePostback_Click" />
                                    </td>
                                </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel><br />
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        <asp:Button ID="Button1" runat="server" EnableViewState="true" 
                                            CssClass="inputfield" Text="Simple Postback" 
                                            onclick="btnSimplePostback_Click" />
                    </div>
                </div>
            </td>
        </tr>
    </table>


</asp:Content>
