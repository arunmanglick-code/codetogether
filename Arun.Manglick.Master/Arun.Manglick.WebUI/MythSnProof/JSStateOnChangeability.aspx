<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JSStateOnChangeability.aspx.cs" Inherits="JSStateOnChangeability" Title="JS State Myth On Changeability" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

<script type="text/javascript">
function MakeChange()
    {
        //debugger;
        obj1=document.getElementById('<%= lblChange.ClientID %>');
        obj1.innerHTML='Changed';
        
        obj2=document.getElementById('<%= ddlChange.ClientID %>');
        obj2.selectedIndex = 1;
        
        obj3=document.getElementById('<%= btnChange.ClientID %>');
        obj3.value='Changed';
        
        obj4=document.getElementById('<%= txtChange1.ClientID %>');
        obj4.value='Changed';
        
        obj5=document.getElementById('<%= txtChange2.ClientID %>');
        obj5.value='Changed';
        
        obj6=document.getElementById('<%= txtChangePassword.ClientID %>');
        obj6.value='Changed';
        
        obj7=document.getElementById('<%= hdnField.ClientID %>');
        obj7.value = 'Changed';

        obj8 = document.getElementById('<%= chkBox.ClientID %>');
        obj8.checked = true;

        obj9 = document.getElementById('<%= RadioButton1.ClientID %>');
        obj9.checked = true;
            
    }
    
    function MakeChangeThruArray()
    {
        obj1=document.getElementById('<%= lblChange.ClientID %>');     
        obj2=document.getElementById('<%= ddlChange.ClientID %>');        
        obj3=document.getElementById('<%= btnChange.ClientID %>');
        obj4=document.getElementById('<%= txtChange1.ClientID %>');
        obj5=document.getElementById('<%= txtChange2.ClientID %>');
        obj6 = document.getElementById('<%= txtChangePassword.ClientID %>');
        obj7=document.getElementById('<%= hdnField.ClientID %>');
        
        var mycars = new Array();
        mycars[0] = obj1;
        mycars[1] = obj2;
        mycars[2] = obj3;
        mycars[3] = obj4; 
        mycars[4] = obj5;  
        mycars[5] = obj5;
        mycars[6] = obj6;
        mycars[7] = obj7;
        
        mycars[0].innerHTML ='Changed';
        mycars[1].selectedIndex = 1;
        
        for(i=2;i< mycars.length;i++)
        {
          mycars[i].value = 'Changed';
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
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>JavaScript State Myth on Change Ability</li>
                            <li>Changes made at JS works regardless of the ViewState. i.e Control State is not dependent on Viewstate when changes are made at JS side </li> 
                            <li>Mostly the changes made at JS will be reset, once the postback occurs. i.e The controls will set to their decalrative values again on 'SimplePostBack' operation </li>
                            <li>But, still the state for DropDown, TextBox, Hidden Field, CheckBox & RadioButton does not reset, even after the postback occurs</li>
                            <li>i.e These three controls maintains the state which is changed in Javascript, at ServerSide as well</li>
                        </ol>
                        
                        Note: Here the 'Change' button is changing control's  values and not making the postback <br />
                        Note: Here the EnableViewState = "False", except for last two textboxes                        
                        
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table border="1px";  width="35%" cellpadding="1" cellspacing="3">
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 20%" class="labelcolumn">
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
                                <td style="width: 10%" class="labelcolumn">
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
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label9" runat="server" Text="CheckBox"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:CheckBox ID="chkBox" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label10" runat="server" Text="Radio Button"></asp:Label>
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:RadioButton ID="RadioButton1" Text="Yes" GroupName="Toggle" runat="server" />
                                    <asp:RadioButton ID="RadioButton2" Text="No" GroupName="Toggle" Checked="true" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%" align="justify" colspan="2" class="labelcolumn">
                                    <asp:Label ID="Label2" ForeColor="Red" runat="server" Text="Run Below Test"></asp:Label>
                                </td>
                            </tr>                           
                            <tr>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnMakeChange" runat="server" EnableViewState="false" 
                                        CssClass="inputfield" Text="Change" 
                                        OnClientClick="MakeChange(); return false;" onclick="btnMakeChange_Click" />
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Button ID="btnSimplePostback" runat="server" EnableViewState="true" 
                                        CssClass="inputfield" Text="Simple Postback" 
                                        onclick="btnSimplePostback_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="Label8" runat="server" EnableViewState="true" Text="Label"></asp:Label>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
