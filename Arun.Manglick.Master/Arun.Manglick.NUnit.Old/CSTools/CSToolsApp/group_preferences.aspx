<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/cs_tool.master" Theme="csTool"
    CodeFile="group_preferences.aspx.cs" Inherits="Vocada.CSTools.group_preferences"
    Title="CSTools: Group Preferences" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlGroupPreferences" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>

            <script language="JavaScript" type="text/JavaScript">
     var mapId = "group_preferences.aspx";

    function Navigate(instId)
    {
        try
        {
            window.location.href = "group_monitor.aspx?InstitutionID=" + instId;
        }
        catch(_error)
        {
            return;
        }
    }
    function isNumericKeyStroke()
    {
        var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
        //alert(event.keyCode);
        if (((keyCode >= 48) && (keyCode <= 57)))
        {
            document.getElementById(hdnTextChangedClientID).value = "true";  
            return;                                  
        }
    
        window.event.returnValue = null;     
    }
    
    //enable / disable controls
    function enabledControl()
    {
        var reqMRN;
        reqMRN = document.getElementById(rblRequireClientID + '_0');
        if (reqMRN.checked)
        {
            document.getElementById(lblAlphaNumaricMRNClientID).style.visibility = "visible";
            document.getElementById(rblAllowAlphaNumaricMRNClientID).style.visibility = "visible";
        }
        else    
        {
            document.getElementById(lblAlphaNumaricMRNClientID).style.visibility = "hidden";
            document.getElementById(rblAllowAlphaNumaricMRNClientID).style.visibility = "hidden";
        }
       
      
    }
     //Sets the flag textChanged to true if the text is changed for any control.
    function TextChanged()
    {
       document.getElementById(hdnTextChangedClientID).value = "true";
       return true;
    } 
    
    //validation of form
    function checkRequired()
    {   //debugger
        var errorMessage1 = "";
        var focusOn = "";  
        var activeDays = trim(document.getElementById(txtMsgActiveDaysClientID).value);
        if(activeDays.length == 0 || isNaN(activeDays) || (activeDays < 1 || activeDays >60)) 
        {
           errorMessage1 = " - Please enter the value of Keep Message Active For Days between 1 to 60 \n";
           focusOn=txtMsgActiveDaysClientID;
        }

        var overdue = trim(document.getElementById(txtOverdueThClientID).value);
        if(overdue.length > 0 && (isNaN(overdue) || (overdue < 0 || overdue >9999)))
        {
           errorMessage1 = " - Please enter the value of Overdue Threshold between 0 to 9999 \n";
           if (focusOn == "")
               focusOn=txtOverdueThClientID;
        }
       
        var group800No = document.getElementById(txtPagerTAP800No1ClientID).value + document.getElementById(txtPagerTAP800No2ClientID).value + document.getElementById(txtPagerTAP800No3ClientID).value;
        if (group800No.length > 0 && group800No.length != 10)
        {
          errorMessage1 = errorMessage1 + " - Please enter valid Pager TAP 800 Number \n";
          if (focusOn == "")
            focusOn=txtPagerTAP800No1ClientID;  
        }       

        if(errorMessage1 != "")
        {
            alert(errorMessage1);
            document.getElementById(focusOn).focus();
            return false;
        }
        
    return true;
    }
        function setVisibility()
        {
            document.getElementById(lblVuiMsgForwardClientID).style.visibility = "hidden";
            document.getElementById(rblVuiMsgForwardClientID).style.visibility = "hidden";
            document.getElementById(lblVuiMsgForwardClientID).style.display = "none";
            document.getElementById(rblVuiMsgForwardClientID).style.display = "none";
            
            document.getElementById(lblRequireReadbackForFWDLabMsgClientID).style.visibility = "hidden";
            document.getElementById(rblReadbackForFWDLabMsgClientID).style.visibility = "hidden";
            document.getElementById(lblRequireReadbackForFWDLabMsgClientID).style.display = "none";
            document.getElementById(rblReadbackForFWDLabMsgClientID).style.display = "none";
        }
            </script>

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0" valign="top">
                                <tr class="BottomBg">
                                    <td width="69%" class="Hd1">
                                        &nbsp;Group Preferences</td>
                                    <td width="16%" align="right" class="Hd1" style="white-space: nowrap;">
                                        <asp:HyperLink ID="hlinkFindings" runat="server" CssClass="AccountInfoText">
                                            Findings and Notifications</asp:HyperLink>
                                        &nbsp;&nbsp;<asp:HyperLink ID="hlinkGroupMaintenance" runat="server" CssClass="AccountInfoText"
                                            NavigateUrl="./group_maintenance.aspx">Group Maintenance</asp:HyperLink>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table width="98%" align="center" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" align="center">
                                        <fieldset class="fieldsetCBlue">
                                            <legend class="">Group Information</legend>
                                            <table border="0" width="40%" align="center">
                                                <tr>
                                                    <input type="hidden" id="hdnTextChanged" runat="server" name="textChanged" value="false"
                                                        style="display: none;" />
                                                    <td style="white-space: nowrap;">
                                                        Group Name:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtGroupName" runat="server" Width="250px" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td style="white-space: nowrap;">
                                                        Group ID:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtGroupID" runat="server" Width="50px" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <fieldset class="innerFieldset" style="margin-left: 0px;">
                                                <legend style="font-weight: bold;">&nbsp;Message Parameters</legend>
                                                <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                                                    <tr>
                                                        <td style="width: 1%">
                                                        </td>
                                                        <td width="21%">
                                                        </td>
                                                        <td width="34%">
                                                        </td>
                                                        <td width="15%">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Keep Message Active For Days:</td>
                                                        <td align="left">
                                                            &nbsp;&nbsp;<asp:TextBox ID="txtMsgActiveDays" MaxLength="2" Width="42px" runat="server" />
                                                        </td>
                                                        <td style="white-space: nowrap;" align="left">
                                                            Overdue Threshold:</td>
                                                        <td align="left">
                                                            &nbsp;&nbsp;<asp:TextBox ID="txtOverdueTh" MaxLength="4" Width="42px" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td style="white-space: nowrap;">
                                                            Require Accession:</td>
                                                        <td align="left">
                                                            <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="rblReqAccession">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="true"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="white-space: nowrap;" align="left">
                                                            Require RP Acceptance:</td>
                                                        <td align="left">
                                                            <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="rblReqRPAcceptance">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="true"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Close Primary and Backup message:</td>
                                                        <td align="left">
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblClosePBkupMsg"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="white-space: nowrap;">
                                                            Require Patient Initials:</td>
                                                        <td align="left">
                                                            <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="rblReqPatientInitials">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0" Selected="true"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Allow Download:</td>
                                                        <td align="left">
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblAllowDownload"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="Group" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="All" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            Use Cc as Backup:</td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblUseCcAsBackup"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            VUI Errors:</td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblVuiErrors"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                        <td style="white-space: nowrap;">
                                                            Require Name Capture:</td>
                                                        <td align="left">
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblReqNameCapture"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td style="white-space: nowrap;">
                                                            Require:</td>
                                                        <td align="left">
                                                            <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="rblRequire"
                                                                Width="150">
                                                                <asp:ListItem Text="MRN" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="DOB" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="None" Value="2"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td align="left" nowrap>
                                                            <asp:Label ID="lblAlphaNumaricMRN" runat="server" Text="Allow AlphaNumeric MRN:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblAllowAlphaNumaricMRN"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Directory Tab On Desktop:</td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblDirectoryTab"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                        <td>
                                                            Group Type:</td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblGroupType"
                                                                CssClass="radiobutton" Enabled="False">
                                                                <asp:ListItem Text="Radiology" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Lab" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                    </tr>
                                                    <!--tr>
                                                        <td>
                                                           </td>
                                                       <td visible = "false">
                                                            </td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblMsgForwardingAlert" visible = "false"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                        <td visible = "false">
                                                            </td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblForwardedMsgClosed"
                                                                CssClass="radiobutton" visible = "false">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                    </tr>-->
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td align="left">Include Patient Name in Pager and SMS Notifications:</td>
                                                        <td align="left">
                                                            <asp:RadioButtonList ID="rlstPagerSms" RepeatDirection="Horizontal" runat="server">
                                                                <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                        <td align="left">Include Patient Name in Email Notifications:</td>
                                                            <td align="left"><asp:RadioButtonList ID="rlstEmail" RepeatDirection="Horizontal" runat="server">
                                                                <asp:ListItem Text="Yes" Value="1" ></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0" ></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Allow Send To Agent:</td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rdlAllowSendToAgent"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                         <td style="white-space:nowrap;">
                                                            Directory Synchronization:</td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblEnableDirectorySync"
                                                                CssClass="radiobutton">
                                                            <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            Pager TAP 800 Number:</td>
                                                        <td>
                                                            &nbsp; (<asp:TextBox Width="35px" ID="txtPagerTAP800No1" runat="server" MaxLength="3"></asp:TextBox>)&nbsp;
                                                            <asp:TextBox ID="txtPagerTAP800No2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>
                                                            -
                                                            <asp:TextBox ID="txtPagerTAP800No3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblVuiMsgForward" runat="server" Text="Allow VUI Message Forwarding:"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblVuiMsgForward"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                    </tr>
                                                     <tr>    
                                                     <td>
                                                            &nbsp;</td>                                                   
                                                        <td>
                                                            <asp:Label ID="lblRequireReadbackForFWDLabMsg" runat="server" Text="Require Readback for Forwarded Message:"></asp:Label></td>
                                                        <td colspan="3">
                                                            <asp:RadioButtonList RepeatDirection="horizontal" runat="server" ID="rblReadbackForFWDLabMsg"
                                                                CssClass="radiobutton">
                                                                <asp:ListItem Text="On" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <br />
                                            <br />
                                            <p align="center">
                                                <asp:Button CssClass="Frmbutton" ID="btnSave" runat="server" Text="Update Group Preferences" onClick="btnSave_Click" />&nbsp;
                                            </p>
                                            <br />
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostbackTrigger ControlID="btnSave" EventName="Click"  />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
