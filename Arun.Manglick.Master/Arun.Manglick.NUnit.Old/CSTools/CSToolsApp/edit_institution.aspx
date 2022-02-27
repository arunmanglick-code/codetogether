<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="edit_institution.aspx.cs"
    Inherits="Vocada.CSTools.edit_institution" Theme="csTool" Title="CSTools: Edit Institution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlEditInstitution" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>

            <script language="JavaScript" src="Javascript/Institution.js" type="text/JavaScript"></script>

            <script language="javascript" type="text/JavaScript">
    var otherPostback =false;
    var mapId  = "edit_institution.aspx";
    function Navigate()
    {
        try
        {
            window.location.href = "institution_information.aspx";
        }
        catch(_error)
        {
            return;
        }
    }
    //Sets the flag textChanged to true if the text of any textbox is changed.
    function UpdateProfile()
    {
       document.getElementById('ctl00$cphMainSection$textChanged').value = "true";
    }  
        
    //Sets the flag textChanged to false when user clicks on Save button as it should not ask confirmation message even
    //if user clicks on Save button. 
    function ChangeFlag()
    {        
        document.getElementById('ctl00$cphMainSection$textChanged').value = "false";
    }
    //Sets the flag textChanged to true if the text is changed for any control.
    function TextChanged()
    {
       document.getElementById(hdnTextChangedClientID).value = "true";
       return true;
    }
    //Check for unit device address is blank or not
    function ValidateAddInstitutionForm()
    {
       if(document.getElementById(hdnTextChangedClientID).value == "true")
       {
        if(otherPostback == true)   
            return false;
       }                  
       return true;        
    }
    
            </script>

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                <tr>
                                    <td class="Hd1">
                                        Edit Institution
                                    </td>
                                    <td style="width: 38%;" align="right" class="Hd1">
                                        <asp:HyperLink ID="lnkInstitutionInfo" runat="server" CssClass="AccountInfoText"
                                            NavigateUrl="./institution_information.aspx">Back to Institution List</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" border="0" align="center" style="vertical-align: top; margin-top: -10px;"
                                cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="ContentBg" align="center">
                                        <fieldset class="fieldsetBlue" style="margin-left: 0px; margin-top: 0px;">
                                            <legend class="">Institution Information</legend>
                                            <fieldset class="innerFieldset" style="margin-left: 0px; width: 98%;">
                                                <legend class=""><b>Institution Info</b></legend>
                                                <table id="Table1" cellspacing="1" cellpadding="2" width="90%" border="0">
                                                    <tr>
                                                        <td width="20%">
                                                        </td>
                                                        <td width="30%">
                                                        </td>
                                                        <td width="3%">
                                                        </td>
                                                        <td width="20%">
                                                        </td>
                                                        <td width="27%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            InstitutionID:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtInstitutionID" runat="server" Text="1234" ReadOnly="true"
                                                                Columns="25"></asp:TextBox></td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td nowrap>
                                                            Institution Name*:</td>
                                                        <td>
                                                            <input type="hidden" id="hdnTextChanged" runat="server" name="textChanged" value="false"
                                                                style="display: none;" />
                                                            &nbsp;<asp:TextBox ID="txtInstitutionName" runat="server" Columns="40" MaxLength="50"></asp:TextBox>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Address1:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtAddress1" Columns="40" runat="server" MaxLength="100"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Address2:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtAddress2" Columns="40" runat="server" MaxLength="100"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            City:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtCity" Columns="40" runat="server" MaxLength="50"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            State:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtstate" Columns="4" runat="server" MaxLength="2"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Zip:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtZip" Columns="40" runat="server" MaxLength="10"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Main Number:</td>
                                                        <td>
                                                            &nbsp;(<asp:TextBox Width="35px" ID="txtMainNumber1" runat="server" MaxLength="3"></asp:TextBox>)
                                                            <asp:TextBox ID="txtMainNumber2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                ID="txtMainNumber3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <fieldset class="innerFieldset" style="margin-left: 0px; width: 98%;">
                                                <legend class="">&nbsp;<b>Contact Info</b></legend>
                                                <table id="Table3" cellspacing="1" cellpadding="2" width="90%" border="0">
                                                    <tr>
                                                        <td width="20%">
                                                        </td>
                                                        <td width="30%">
                                                        </td>
                                                        <td width="3%">
                                                        </td>
                                                        <td width="20%">
                                                        </td>
                                                        <td width="27%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" style="font-weight: bold">
                                                            Primary Contact Info</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Name*:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtPrimaryName" Columns="40" runat="server" MaxLength="50"></asp:TextBox></td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Title:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtPrimaryTitle" Columns="40" runat="server" MaxLength="50"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Phone:</td>
                                                        <td>
                                                            &nbsp;(<asp:TextBox Width="35px" ID="txtPrimaryPhone1" runat="server" MaxLength="3"></asp:TextBox>)
                                                            <asp:TextBox ID="txtPrimaryPhone2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                ID="txtPrimaryPhone3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Email:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox Columns="40" ID="txtPrimaryEmail" runat="server" MaxLength="100"></asp:TextBox>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" style="font-weight: bold">
                                                            Contact 1 Info</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Name:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtContact1Name" Columns="40" runat="server" MaxLength="50"></asp:TextBox></td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Title:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtContact1Title" Columns="40" runat="server" MaxLength="50"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Phone:</td>
                                                        <td>
                                                            &nbsp;(<asp:TextBox Width="35px" ID="txtContact1Phone1" runat="server" MaxLength="3"></asp:TextBox>)
                                                            <asp:TextBox ID="txtContact1Phone2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                ID="txtContact1Phone3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Email:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox Columns="40" ID="txtContact1email" runat="server" MaxLength="100"></asp:TextBox>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" style="font-weight: bold">
                                                            Contact 2 Info</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Name:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtContact2Name" Columns="40" runat="server" MaxLength="50"></asp:TextBox></td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Title:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtContact2Title" Columns="40" runat="server" MaxLength="50"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Phone:</td>
                                                        <td>
                                                            &nbsp;(<asp:TextBox Width="35px" ID="txtContact2Phone1" runat="server" MaxLength="3"></asp:TextBox>)
                                                            <asp:TextBox ID="txtContact2Phone2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                ID="txtContact2Phone3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Email:</td>
                                                        <td>
                                                            &nbsp;<asp:TextBox Columns="40" ID="txtContact2Email" runat="server" MaxLength="100"></asp:TextBox>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <fieldset class="innerFieldset" style="margin-left: 0px; width: 98%;">
                                                <legend class="">&nbsp;<b>Lab Info</b></legend>
                                                <table id="Table5" cellspacing="1" cellpadding="2" width="90%" border="0">
                                                    <tr>
                                                        <td width="20%">
                                                        </td>
                                                        <td width="30%">
                                                        </td>
                                                        <td width="3%">
                                                        </td>
                                                        <td width="20%">
                                                        </td>
                                                        <td width="27%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Lab 800 Number:</td>
                                                        <td>
                                                            &nbsp;(<asp:TextBox Width="35px" ID="txtLab800No1" runat="server" MaxLength="3"></asp:TextBox>)
                                                            <asp:TextBox ID="txtLab800No2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                ID="txtLab800No3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Nurse 800 Number:</td>
                                                        <td>
                                                            &nbsp;(<asp:TextBox Width="35px" ID="txtNurse800No1" runat="server" MaxLength="3"></asp:TextBox>)
                                                            <asp:TextBox ID="txtNurse800No2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                ID="txtNurse800No3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Time Zone:*</td>
                                                        <td>
                                                            &nbsp;<asp:DropDownList ID="drpTimeZone" runat="server" Width="145px" DataTextField="description"
                                                                DataValueField="TimeZoneID">
                                                            </asp:DropDownList>&nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Shift Nurse 800 Number:</td>
                                                        <td>
                                                            &nbsp;(<asp:TextBox Width="35px" ID="txtShiftNurce800No1" runat="server" MaxLength="3"></asp:TextBox>)
                                                            <asp:TextBox ID="txtShiftNurce800No2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                ID="txtShiftNurce800No3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <fieldset class="innerFieldset" style="margin-left: 0px; width: 98%;">
                                                <legend class="">Institution Preferences</legend>
                                                <br />
                                                <table id="Table2" cellspacing="1" cellpadding="2" width="90%" border="0">
                                                    <tr>
                                                        <td width="20%">
                                                        </td>
                                                        <td width="30%">
                                                        </td>
                                                        <td width="3%">
                                                        </td>
                                                        <td width="20%">
                                                        </td>
                                                        <td width="27%">
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td>
                                                            Require Callback Voice Over:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblCallbackVoiceOver" RepeatDirection="Horizontal" runat="server"
                                                                Visible="true">
                                                                <asp:ListItem Text="On" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Require Name Capture:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblNameCapture" RepeatDirection="Horizontal" runat="server"
                                                                Visible="true">
                                                                <asp:ListItem Text="On" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Require Readback for Unit of Measurement:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblReadback" RepeatDirection="Horizontal" runat="server"
                                                                Visible="true">
                                                                <asp:ListItem Text="On" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Require Name Capture Validation:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblNameCaptureValidation" RepeatDirection="Horizontal" runat="server"
                                                                Visible="true">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Show Exam Description:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblRequireExamDescription" RepeatDirection="Horizontal"
                                                                runat="server">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Require Acceptance for Outbound Call:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblOutboundCall" RepeatDirection="Horizontal" runat="server"
                                                                Visible="true">
                                                                <asp:ListItem Text="On" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Allow PIN for Message Retrieval:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblMessageRetrieveUsingPIN" RepeatDirection="Horizontal"
                                                                runat="server" Visible="true">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Batch Messages:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblBatchMessages" AutoPostBack="false" RepeatDirection="Horizontal"
                                                                runat="server" OnSelectedIndexChanged="rblRequireVoiceClips_SelectedIndexChanged">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                            Prompt for PIN for CT Message:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblPinForCT" RepeatDirection="Horizontal"
                                                                runat="server" Visible="true">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Enable Agent Team:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblEnableCallCenter" RepeatDirection="Horizontal" runat="server"
                                                                Visible="true">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Require ED Message:</td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblRequireVoiceClips" AutoPostBack="true" RepeatDirection="Horizontal"
                                                                runat="server" OnSelectedIndexChanged="rblRequireVoiceClips_SelectedIndexChanged">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                        <td> 
                                                            &nbsp;</td>
                                                       <asp:Panel ID="pnlTabName" runat="server" Visible="false">
                                                            <td>
                                                                <asp:Label ID="lblTabName" runat="server" Text="Tab Name on Veriphy Desktop:"></asp:Label></td>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblConnectED" RepeatDirection="Horizontal" runat="server"
                                                                    Visible="true">
                                                                    <asp:ListItem Text="ConnectED" Value="1" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="VeriphyED" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList></td>
                                                        </asp:Panel>
                                                    </tr>
                                                    <tr>
                                                       
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <p align="center">
                                                <asp:Button CssClass="Frmbutton" ID="btnEditInstitution" Text=" Save " runat="server"
                                                    OnClick="btnEditInstitution_Click" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="Frmbutton" OnClick="btnCancel_Click"
                                                    CausesValidation="False" />
                                            </p>
                                            <asp:ValidationSummary ID="vsmrInstitution" runat="server" ShowSummary="False" ShowMessageBox="True">
                                            </asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvInstitutionName" runat="server" Display="None"
                                                ErrorMessage="You Must Enter An Institution Name" ControlToValidate="txtInstitutionName"></asp:RequiredFieldValidator>
                                        </fieldset>
                                        <br />
                                        &nbsp;
                                    </td>
                                    <br />
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEditInstitution" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
