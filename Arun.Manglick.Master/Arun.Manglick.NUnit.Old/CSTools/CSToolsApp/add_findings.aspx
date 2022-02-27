<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" Theme="csTool" AutoEventWireup="true"
    CodeFile="add_findings.aspx.cs" Inherits="Vocada.CSTools.add_findings" Title="CSTools: Add Findings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel ID="UpdatePanelAddFinding" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
            <script language="JavaScript" src="Javascript/Institution.js" type="text/JavaScript"></script>
            <script language="JavaScript" src="Javascript/finding.js" type="text/JavaScript"></script>
            <table style="height: 100%" align="center" width="98%" border="0" cellpadding="0"
                cellspacing="0">
                <tr class="ContentBg">
                    <td class="DivBg" valign="top" style="height: 50px">
                        <div style="overflow-y: auto; height: 100%">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="Hd1">
                                        <input type="hidden" enableviewstate="true" id="hdnTextChanged" runat="server" name="textChanged"
                                            value="false" style="display: none;" />
                                        <asp:Label ID="lblPageName" runat="server"></asp:Label></td>
                                    <td style="width: 15%" align="right" class="Hd1">
                                        <asp:HyperLink ID="hlinkGroupMonitor" runat="server" CssClass="AccountInfoText" NavigateUrl="./group_monitor.aspx">Back to Group Monitor</asp:HyperLink>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table width="98%" border="0" cellpadding="0" cellspacing="0" align="Center">
                                <tr>
                                    <td class="ContentBg" valign="top">
                                        <fieldset class="fieldsetCBlue">
                                            <legend class="legend">Select</legend>
                                            <table width="40%" align="Center">
                                                <tr>
                                                    <td valign="middle" nowrap align="right">
                                                        Institution Name:&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                    <td align="left">
                                                        <asp:HiddenField ID="hdnInstitutionVal" runat="server" Value="false" />
                                                        <asp:DropDownList ID="cmbInstitution" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitution_SelectedIndexChanged"
                                                            DataTextField="InstitutionName" DataValueField="InstitutionID" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtInstitution" runat="server" Width="250" Visible="false" ReadOnly="true" TabIndex="1"/>
                                                        <asp:Label ID="lblInstName" runat="server" Visible="False"></asp:Label>
                                                        <asp:HiddenField ID="hdnIsSystemAdmin" runat="server" Value="1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        
                                        <fieldset class="fieldsetCBlue" >
                                            <legend class="">Finding Information</legend>
                                            <table align="center" width="90%" border="0" cellpadding="2" cellspacing="1">
                                                <tr>
                                                    <td style="width: 20%">
                                                    </td>
                                                    <td style="width: 40%">
                                                    </td>
                                                    <td style="width: 20%">
                                                    </td>
                                                    <td style="width: 23%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Group*:
                                                    </td>
                                                    <td style="white-space: nowrap">
                                                        &nbsp;<asp:DropDownList ID="cmbGroup" runat="server" DataTextField="GroupName" DataValueField="GroupID"
                                                            Width="250" TabIndex="2"/><asp:TextBox ID="txtGroupName" runat="server" Width="250" Visible="false" ReadOnly="true" TabIndex="2" AutoPostBack="false"/>
                                                        <asp:TextBox ID="txtGroupCheck" runat="server" Text="" Width="0" EnableViewState="true" />
                                                    </td>
                                                    <td style="white-space: nowrap">
                                                        Finding Description*:</td>
                                                    <td>
                                                        &nbsp;<asp:TextBox Width="200" ID="txtDescription" runat="server" MaxLength="50"  TabIndex="3"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Priority*:</td>
                                                    <td>
                                                        &nbsp;<asp:TextBox Columns="25" ID="txtPriority" runat="server" MaxLength="2" Width="200"  TabIndex="4"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Finding Order:</td>
                                                    <td>
                                                        &nbsp;<asp:TextBox Columns="25" ID="txtFindingOrder" runat="server" MaxLength="4" Width="200"  TabIndex="5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Active:</td>
                                                    <td>
                                                        <asp:CheckBox ID="chkActive" runat="server"  TabIndex="6"/>
                                                    </td>
                                                    <td>
                                                        Default:</td>
                                                    <td>
                                                        <input type="checkbox" id="chkDefault" name="chkDefault" runat="server" tabindex="7" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap">
                                                        Finding Voiceover:</td>
                                                    <td valign="middle">
                                                        &nbsp;<asp:HyperLink ID="hlinkPlay" Height="16px" Width="12px" runat="server" ImageUrl="./img/ic_play_msg.gif"
                                                            Visible="false" Style="vertical-align: middle; padding-right: 3"></asp:HyperLink><asp:FileUpload
                                                                ID="flupdVoiceOver" runat="server" Width="250" CssClass="frmButton" Style="height: 18px" TabIndex="8"/></td>
                                                    <td>
                                                        Require Readback:</td>
                                                    <td>
                                                        <asp:CheckBox ID="chkRequireReadback" runat="server" TabIndex="9"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                                
                                        <fieldset class="fieldsetCBlue" >
                                            <legend class="">Escalation Options</legend>
                                            <table align="center" width="90%" border="0" cellpadding="2" cellspacing="1">
                                                <tr>
                                                    <td style="width: 20%">
                                                    </td>
                                                    <td style="width: 40%">
                                                    </td>
                                                    <td style="width: 20%">
                                                    </td>
                                                    <td style="width: 23%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Compliance Goal:</td>
                                                    <td>
                                                        &nbsp;<asp:TextBox Columns="25" ID="txtComplianceGoal" runat="server" MaxLength="6" TabIndex="10"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Escalate Every (Minutes):</td>
                                                    <td>
                                                        &nbsp;<asp:TextBox Columns="25" ID="txtEscalateEvery" runat="server" MaxLength="4" TabIndex="11"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Start backup at:</td>
                                                    <td>
                                                        &nbsp;<asp:DropDownList ID="cmbStartBackup" runat="server" Width="150" TabIndex="12" AutoPostBack="false">
                                                            <asp:ListItem Text="1st Escalation" Value="1" Selected="true"></asp:ListItem>
                                                            <asp:ListItem Text="2nd Escalation" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3rd Escalation" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="End Escalation" Value="4"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left">
                                                        End After Minutes:</td>
                                                    <td>
                                                        &nbsp;<asp:TextBox Columns="25" ID="txtEndAfter" runat="server" MaxLength="4" TabIndex="13"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Continue to Primary:</td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkContinue" runat="server" TabIndex="14"/></td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Embargo:</td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkEmbargo" runat="server"  TabIndex="15"/>
                                                    </td>
                                                    <td>
                                                        Weekend Embargo:</td>
                                                    <td>
                                                        <asp:CheckBox ID="chkEmbWeekend" runat="server"  TabIndex="16"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Embargo Start Hour:</td>
                                                    <td>
                                                        &nbsp;<asp:TextBox Columns="25" ID="txtEmbargoStart" runat="server" MaxLength="2" Width="200" TabIndex="17"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Embargo End Hour:</td>
                                                    <td>
                                                        &nbsp;<asp:TextBox Columns="25" ID="txtEmbargoEnd" runat="server" MaxLength="2" Width="200" TabIndex="18"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                                
                                        <fieldset class="fieldsetCBlue" >
                                            <legend class="">Integration Options</legend>
                                            <table align="center" width="90%" border="0" cellpadding="2" cellspacing="1">
                                                <tr>
                                                    <td style="width: 20%">
                                                    </td>
                                                    <td style="width: 40%">
                                                    </td>
                                                    <td style="width: 20%">
                                                    </td>
                                                    <td style="width: 23%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Document Only:</td>
                                                    <td>
                                                        &nbsp;<asp:CheckBox ID="chkDocumentedOnly" runat="server" TabIndex="19"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Connect Live:</td>
                                                    <td><asp:RadioButtonList CssClass="radiobutton" id="rbConnectLive"  runat="server" TabIndex="20" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="None" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Connect Live" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Deliver Live" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Deliver Through Veriphy" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList> 
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        
                                        <fieldset class="fieldsetCBlue">
                                            <legend class="legend">Assign Notification Event to Device</legend>
                                             <table align="center" width="90%" border="0" cellpadding="2" cellspacing="1" style="margin-top:10px;">
                                                <tr>
                                                    <td valign="top" nowrap  style="width: 20%">
                                                        Notification Event:</td>
                                                    <td align="left" style="width: 40%">
                                                        <asp:DropDownList ID="cmbNotificationEvent" runat="server" Width="180"
                                                            DataTextField="EventDescription" DataValueField="RPNotifyEventID" TabIndex="21" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 20%">&nbsp;</td>
                                                    <td style="width: 40%">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap  style="width: 20%">
                                                        Device:</td>
                                                    <td align="left" style="width: 40%">
                                                        <asp:DropDownList ID="cmbDevice" runat="server" Width="180"
                                                            DataTextField="DeviceDescription" DataValueField="DeviceID" TabIndex="22" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 20%">&nbsp;</td>
                                                    <td style="width: 40%">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <table  align="center" width="90%" border="0" cellpadding="2" cellspacing="1" style="margin-top:5px;">
                                            <tr>
                                                <td align="Center">
                                                    <br />
                                                    <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="Frmbutton" TabIndex="23"  OnClick="btnSave_Click"></asp:Button>&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                                        CssClass="Frmbutton" OnClick="btnCancel_Click" TabIndex="24"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
    <!--<asp:LinkButton runat="server" ID="lnkSubmitUpload" BackColor="White" OnClick="lnkSubmitUpload_Click"></asp:LinkButton> -->
</asp:Content>
