<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" Theme="csTool" AutoEventWireup="true" CodeFile="add_subscriber.aspx.cs" Inherits="Vocada.CSTools.add_subscriber" Title="CSTools: Add Subscriber" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!--script language="JavaScript" src="common.js" type="text/JavaScript"></script-->
    <script language="javascript" type="text/javascript" src="Javascript/userProfile.js"></script>
    <script language="javascript" type="text/JavaScript">
    var mapId = "add_subscriber.aspx";
    var isFocusOnButton  = false;
    var isFocusOnSaveButton  = false;
    /*Visible the speciliality info section only when role selected is specialist / lab tech */
    function ShowSpecialistInfo(roleCombo, canResetValues)
    {
        if(roleCombo.value == 1 || roleCombo.value == 4)
            document.getElementById(divSpecialistInfoClientID).style.display = "";
        else
            document.getElementById(divSpecialistInfoClientID).style.display = "none";
            
        if(canResetValues == true)
        {
            document.getElementById(txtAffilationClientID).value = "";
            document.getElementById(txtSpecialityClientID).value = "";
        }
    }
    /*If input key is numeric value then only reutn value else nulliphy effect*/
    function isNumericKey()
    {
        var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
      
        if ( ((keyCode >= 48) && (keyCode <= 57))) //||  All numerics
        {
            document.getElementById(txtChangedClientID).value = "true";  
            return;                                  
        }
        
        window.event.returnValue = null;     
    }
    
    /*If input key is alpha-numeric value then only reutn value else nulliphy effect*/
    function isAlphaNumericKey()
    {
        var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
        
        if((event.keyCode == 47 || (event.keyCode >= 58) && (event.keyCode <= 64)) || ((event.keyCode >= 33) && (event.keyCode <= 39)) || ((event.keyCode >= 40) && (event.keyCode <= 46)) || ((event.keyCode >= 91) && (event.keyCode <= 96)) || ((event.keyCode >= 123) && (event.keyCode <= 126))) 
          {
            event.returnValue = false;
            return false;
          }
      
            document.getElementById(txtChangedClientID).value = "true";  
            return;                                  
    }

    
    /*Cancel the enter key code when enter press on any control*/
    function comboClick()
    {
        var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
      
        if(keyCode == 13)//||  All numerics
        {
            //window.event.returnValue = 23;    
            window.event.keyCode = 23;    
            return false;
        }
        return true;
    }
   
    /*Cancel Enter event when enter click on*/
    function deactiveEnterAction()
    {
        if (window.event.keyCode == 13 && isFocusOnButton == false) 
        {
                window.event.keyCode = 23; 
                event.returnValue=false; 
                event.cancel = true;
        }
        else if(window.event.keyCode == 13 && isFocusOnButton && !isFocusOnSaveButton)
        {
            window.event.keyCode == 32; 
            event.returnValue=true; 
        }
    }
    
    function focusOnButton(value, saveButton)
    {
        isFocusOnButton = value;
        isFocusOnSaveButton = saveButton;
    }
    //Redirects user to the given URL as the Response.Redirect doesn't works sometime.
    function Navigate(url)
    {
        try
        {
            window.location.href = url;
        }
        catch(_error)
        {
            return;
        }
    }
    
    </script>
    <asp:UpdatePanel ID="upnlInstitutions" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <input type="hidden" id="txtChanged" runat="server" name="txtChanged" value="false" />
            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td valign="top" style="height: 50px">
                        <div style="overflow-y: Auto; height: 100%">
                            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                <tr>
                                    <td class="Hd1">
                                        Add Subscriber
                                    </td>
                                </tr>
                            </table>
                            <table width="98%" border="0" align="center" cellpadding="=0" cellspacing="0">
                                <tr>
                                    <td class="ContentBg">
                                        <fieldset class="fieldsetCBlue">
                                            <legend><b>Select Institution</b></legend>
                                            <table id="Table4" cellspacing="1" cellpadding="2" width="100%" border="0">
                                                <tr>
                                                    <td width="35%" style="white-space: nowrap;" align="center">
                                                        <asp:HiddenField ID="hdnOCDirectoryDataChanged" runat="server" Value="false" EnableViewState="true" />
                                                        <asp:HiddenField ID="hdnInstitutionVal" runat="server" Value="-1" EnableViewState="true" />
                                                        Institution Name:&nbsp;&nbsp;&nbsp;
                                                        <asp:DropDownList ID="cmbInstitutions" runat="server" AutoPostBack="true" Width="250px"
                                                            OnSelectedIndexChanged="cmbInstitutions_SelectedIndexChanged" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblInstName" runat="server" Visible="False"></asp:Label>
                                                        <asp:HiddenField ID="hdnIsSystemAdmin" runat="server" Value="1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <div id="divSubscriberInfo" runat="server">
                                <table width="98%" border="0" align="center" cellpadding="=0" cellspacing="0">
                                    <tr>
                                        <td class="ContentBg" align="center">
                                            <fieldset class="fieldsetCBlue">
                                                <legend>Subscriber Information</legend>
                                                <fieldset class="innerFieldset" style="margin-left: 0px;">
                                                    <legend class="innerLegend"><b>Account Info</b></legend>
                                                    <table id="Table1" align="center" cellspacing="1" cellpadding="2" width="80%" border="0">
                                                        <tr>
                                                            <td style="width: 15%; white-space: nowrap;">
                                                            </td>
                                                            <td style="width: 30%; white-space: nowrap;" align="left">
                                                            </td>
                                                            <td style="width: 10%; white-space: nowrap;" align="left">
                                                            </td>
                                                            <td style="width: 15%; white-space: nowrap;">
                                                            </td>
                                                            <td style="width: 30%; white-space: nowrap;" align="left">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Group*:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbGroupName" DataTextField="GroupName" DataValueField="GroupID"
                                                                    runat="server" Width="175px" TabIndex="2" OnSelectedIndexChanged="cmbGroupName_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="reqFieldVaildatorGroup" Display="none" runat="server"
                                                                    ErrorMessage="You must enter the group." ValidationGroup="SubscriberInfo" ControlToValidate="cmbGroupName"
                                                                    SetFocusOnError="true" InitialValue="-1"></asp:RequiredFieldValidator>&nbsp;</td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td>
                                                                Role*:</td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbRole" runat="server" Width="235px" TabIndex="3">
                                                                    <asp:ListItem Text="-- Select Role --" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>&nbsp;<asp:RequiredFieldValidator ID="reqFieldValidatorRole" runat="server"
                                                                    Display="none" ErrorMessage="You must select the role." ValidationGroup="SubscriberInfo"
                                                                    SetFocusOnError="true" ControlToValidate="cmbRole" InitialValue="-1"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                LoginID*:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtLoginId" runat="server" Columns="31" Font-Bold="False" TabIndex="4"
                                                                    MaxLength="10"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="reqFieldValidatorLoginID" runat="server" Display="none"
                                                                    SetFocusOnError="true" ErrorMessage="You must enter a login ID." ValidationGroup="SubscriberInfo"
                                                                    ControlToValidate="txtLoginId"></asp:RequiredFieldValidator></td>
                                                            <asp:RegularExpressionValidator ID="revLoginID" ValidationGroup="SubscriberInfo"
                                                                runat="server" ErrorMessage="Please enter valid Login ID." ControlToValidate="txtLoginId"
                                                                SetFocusOnError="true" Display="None" ValidationExpression="((\w)*(\d)*)"></asp:RegularExpressionValidator>
                                                                 <asp:CustomValidator ID="ctmValLogin" runat="server" ControlToValidate="txtLoginId"
                                                                    ClientValidationFunction="validateLoginID" ValidationGroup="SubscriberInfo"
                                                                    Display="None" ErrorMessage="Login ID must be 2-10 characters/digits."></asp:CustomValidator>
                                                                <td>
                                                                    &nbsp;</td>
                                                            <td style="white-space: nowrap;">
                                                                PIN*:</td>
                                                            <td style="width: 25%;">
                                                                <asp:TextBox ID="txtPin" Columns="10" runat="server" Font-Bold="False" MaxLength="10"
                                                                    TabIndex="5"></asp:TextBox>&nbsp;(Max 10 digits)
                                                                <asp:Button ID="btnGeneratePin" Text="Generate PIN" runat="server" CssClass="Frmbutton"
                                                                    TabIndex="6" Width="84px" OnClick="btnGeneratePin_Click" />
                                                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" Display="none" SetFocusOnError="true"
                                                                    ErrorMessage="You must enter a PIN." ValidationGroup="SubscriberInfo" ControlToValidate="txtPin"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="revalPassword" SetFocusOnError="true" ValidationGroup="SubscriberInfo"
                                                                    runat="server" ErrorMessage="Password must be 4-10 digits." ControlToValidate="txtPin"
                                                                    Display="None" ValidationExpression="\d{4,10}"></asp:RegularExpressionValidator>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <fieldset class="innerFieldset" style="margin-left: 0px;">
                                                    <legend class="innerLegend">&nbsp;<b>User Info</b></legend>
                                                    <table id="Table3" align="center" cellspacing="1" cellpadding="2" width="80%" border="0">
                                                        <tr>
                                                            <td style="width: 15%; white-space: nowrap;">
                                                            </td>
                                                            <td style="width: 30%; white-space: nowrap;" align="left">
                                                            </td>
                                                            <td style="width: 10%; white-space: nowrap;" align="left">
                                                            </td>
                                                            <td style="width: 15%; white-space: nowrap;">
                                                            </td>
                                                            <td style="width: 30%; white-space: nowrap;" align="left">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="white-space: nowrap;">
                                                                First Name*:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtFirstName" Columns="31" runat="server" Font-Bold="False" TabIndex="7"
                                                                    MaxLength="50" onKeypress="deactiveEnterAction();if(((event.keyCode >= 47) && (event.keyCode <= 64)) || ((event.keyCode >= 33) && (event.keyCode <= 38)) || ((event.keyCode >= 40) && (event.keyCode <= 45)) || ((event.keyCode >= 91) && (event.keyCode <= 96)) || ((event.keyCode >= 123) && (event.keyCode <= 126))) event.returnValue = false;"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="reqFieldValidatorFirstName" runat="server" Display="none"
                                                                    ErrorMessage="You must enter a first name." ValidationGroup="SubscriberInfo"
                                                                    SetFocusOnError="true" ControlToValidate="txtFirstName"></asp:RequiredFieldValidator></td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td style="white-space: nowrap;">
                                                                Email:</td>
                                                            <td>
                                                                <asp:TextBox Columns="42" ID="txtPrimaryEmail" runat="server" Font-Bold="False" TabIndex="10"
                                                                    MaxLength="75"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="regExpPrimaryEmail" runat="server" ErrorMessage="You must enter valid email id."
                                                                    SetFocusOnError="true" ControlToValidate="txtPrimaryEmail" ValidationGroup="SubscriberInfo"
                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="none"></asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="white-space: nowrap;">
                                                                Last Name*:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtLastName" Columns="31" runat="server" Font-Bold="False" TabIndex="8"
                                                                    MaxLength="50" onKeypress="deactiveEnterAction();if(((event.keyCode >= 47) && (event.keyCode <= 64)) || ((event.keyCode >= 33) && (event.keyCode <= 38)) || ((event.keyCode >= 40) && (event.keyCode <= 45)) || ((event.keyCode >= 91) && (event.keyCode <= 96)) || ((event.keyCode >= 123) && (event.keyCode <= 126))) event.returnValue = false;">
                                                                </asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="reqFieldValidatorLastName" runat="server"
                                                                    SetFocusOnError="true" ErrorMessage="You must enter a last name." Display="none"
                                                                    ValidationGroup="SubscriberInfo" ControlToValidate="txtLastName"></asp:RequiredFieldValidator></td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td style="white-space: nowrap;">
                                                                Office Phone:</td>
                                                            <td>
                                                                <asp:Label ID="Label4" Font-Bold="False" runat="server">(</asp:Label><asp:TextBox
                                                                    Width="35px" ID="txtPhone1" MaxLength="3" runat="server" Font-Bold="False" TabIndex="11"></asp:TextBox><asp:Label
                                                                        ID="Label5" Font-Bold="False" runat="server">)</asp:Label>&nbsp;
                                                                <asp:TextBox ID="txtPhone2" Width="35px" runat="server" Font-Bold="False" MaxLength="3"
                                                                    TabIndex="12"></asp:TextBox>&nbsp;
                                                                <asp:Label ID="Label6" Font-Bold="False" runat="server">&nbsp;-&nbsp;</asp:Label>
                                                                <asp:TextBox ID="txtPhone3" Columns="4" runat="server" Font-Bold="False" MaxLength="4"
                                                                    TabIndex="13"></asp:TextBox>&nbsp;
                                                                <asp:CustomValidator ID="ctmValPhone" runat="server" ControlToValidate="cmbGroupName"
                                                                    ClientValidationFunction="validatePhoneNumber" ValidationGroup="SubscriberInfo"
                                                                    Display="None" ErrorMessage="You must enter valid phone number."></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="white-space: nowrap;">
                                                                Nick Name:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtNickname" Columns="31" runat="server" Font-Bold="False" TabIndex="9"
                                                                    MaxLength="50" onKeypress="if(((event.keyCode >= 47) && (event.keyCode <= 64)) || ((event.keyCode >= 33) && (event.keyCode <= 38)) || ((event.keyCode >= 40) && (event.keyCode <= 45)) || ((event.keyCode >= 91) && (event.keyCode <= 96)) || ((event.keyCode >= 123) && (event.keyCode <= 126))) event.returnValue = false;"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td>
                                                                Fax:</td>
                                                            <td>
                                                                <asp:Label ID="Label1" Font-Bold="False" runat="server">(</asp:Label><asp:TextBox
                                                                    Width="35px" ID="txtFax1" MaxLength="3" TabIndex="14" runat="server" Font-Bold="False"></asp:TextBox><asp:Label
                                                                        ID="Label2" Font-Bold="False" runat="server">)</asp:Label>&nbsp;
                                                                <asp:TextBox ID="txtFax2" Width="35px" runat="server" Font-Bold="False" MaxLength="3"
                                                                    TabIndex="15"></asp:TextBox>&nbsp;
                                                                <asp:Label ID="Label3" Font-Bold="False" runat="server">&nbsp;-&nbsp;</asp:Label>
                                                                <asp:TextBox ID="txtFax3" Columns="4" runat="server" Font-Bold="False" MaxLength="4"
                                                                    TabIndex="16"></asp:TextBox>&nbsp;
                                                                <asp:CustomValidator ID="ctmValFax" runat="server" ControlToValidate="cmbGroupName"
                                                                    ClientValidationFunction="validateFaxNumber" ValidationGroup="SubscriberInfo"
                                                                    Display="None" ErrorMessage="You must enter valid fax number."></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                                <div id="divSpecialistInfo" style="height: 0px; display: none;" runat="server">
                                                    <table width="100%" border="0" align="center" cellpadding="=0" cellspacing="0">
                                                        <tr>
                                                            <td class="ContentBg" align="center">
                                                                <fieldset class="innerFieldset" style="margin-left: 0px;">
                                                                    <legend class="innerLegend">Specialist information </legend>
                                                                    <br />
                                                                    <table id="Table2" cellspacing="1" align="center" cellpadding="2" width="80%" border="0">
                                                                        <tr>
                                                                            <td style="width: 15%; white-space: nowrap;">
                                                                            </td>
                                                                            <td style="width: 30%; white-space: nowrap;" align="left">
                                                                            </td>
                                                                            <td style="width: 10%; white-space: nowrap;" align="left">
                                                                            </td>
                                                                            <td style="width: 15%; white-space: nowrap;">
                                                                            </td>
                                                                            <td style="width: 30%; white-space: nowrap;" align="left">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Speciality:</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSpeciality" runat="server" Columns="31" MaxLength="75" Font-Bold="False"></asp:TextBox></td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                Affiliation:</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtAffilation" runat="server" Columns="31" MaxLength="100" Font-Bold="False"></asp:TextBox>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <p align="center">
                                                    <asp:Button ID="btnSaveSubscriber" Text=" Save " runat="server" CssClass="Frmbutton"
                                                        OnClick="btnSaveSubscriber_Click" CausesValidation="true" ValidationGroup="SubscriberInfo" />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="Frmbutton" OnClick="btnCancel_Click"></asp:Button>
                                                </p>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:ValidationSummary ID="validationSummarySubscriber" runat="server" DisplayMode="BulletList"
                                ShowSummary="false" ShowMessageBox="true" ValidationGroup="SubscriberInfo" />
                        </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="cmbInstitutions" />
            <asp:AsyncPostBackTrigger ControlID="cmbGroupName"  />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
