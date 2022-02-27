<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" Theme="csTool" AutoEventWireup="true"
    CodeFile="user_profile.aspx.cs" Inherits="Vocada.CSTools.user_profile" Title="CSTools: User Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMessageList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script language="javascript" type="text/javascript" src="./Javascript/Common.js"></script>

            <script language="javascript" type="text/javascript" src="Javascript/userProfile.js"></script>
            <script language="javascript" src="./Javascript/profile_notificationStep1.js" type="text/javascript"></script>
    
            <input type="hidden" id="scrollPos" value="0" runat="server" />
            <table align="center" width="98%" style="height: 100%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <input type="hidden" id="txtChanged" runat="server" name="txtChanged" value="false" />
                            <input type="hidden" id="hdnProfileSaved" runat="server" name="hdnProfileSaved" value="false" />
                            <input type="hidden" id="hdnSaveCalled" runat="server" name="hdnProfileFailedToSave"
                                value="false" />
                            <input type="hidden" id="hdnUserRole" runat="server" name="hdnUserRole"
                                value="" />
                            <input type="hidden" id="hdnOutstandingChanged" runat="server" name="hdnOutstandingChanged"
                                value="false" />
                            <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0"
                                valign="top">
                                <tr class="BottomBg">
                                    <td valign="top">
                                        <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                            <tr>
                                                <td width="100%" class="Hd1">
                                                    Profile</td>
                                            </tr>
                                        </table>
                                        <table style="vertical-align:top;" width="98%" align="center">
                                            <tr>
                                                <td>
                                                    <div id="divGroupAffiliation" runat="Server">
                                                        <asp:UpdatePanel ID="upnlGroupAffiliation" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <fieldset class="fieldsetCBlue">
                                                                    <legend >&nbsp;<b>Group Affiliation:
                                                                        <asp:Label ID="lblGroupName" runat="server"></asp:Label></b></legend>
                                                                    <table width="95%" border="0" align="center" cellpadding="2" cellspacing="1">
                                                                        <tr>
                                                                            <td colspan="2" height="5">
                                                                            </td>
                                                                            <td width="11%">
                                                                            </td>
                                                                            <td width="15%">
                                                                            </td>
                                                                            <td width="43%">
                                                                                <asp:Label ID="lblUpdateMessage" runat="server" Font-Bold="True"></asp:Label></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="9%">
                                                                                First Name*:</td>
                                                                            <td width="22%">
                                                                                <asp:TextBox ID="txtFirstName" TabIndex="1" runat="server" Columns="35" onKeypress="if(((event.keyCode >= 47) && (event.keyCode <= 64)) || ((event.keyCode >= 33) && (event.keyCode <= 38)) || ((event.keyCode >= 40) && (event.keyCode <= 45)) || ((event.keyCode >= 91) && (event.keyCode <= 96)) || ((event.keyCode >= 123) && (event.keyCode <= 126))) event.returnValue = false;"></asp:TextBox>
                                                                            </td>
                                                                            <td width="11%">
                                                                                &nbsp;</td>
                                                                            <td width="15%">
                                                                                Login ID*:</td>
                                                                            <td width="43%">
                                                                                <asp:TextBox ID="txtLoginID" TabIndex="11" runat="server" Columns="10" MaxLength="10"></asp:TextBox>&nbsp;(10
                                                                                characters/digits only)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="9%">
                                                                                Last Name*:</td>
                                                                            <td width="22%">
                                                                                <asp:TextBox ID="txtLastName" TabIndex="2" runat="server" Columns="35" onKeypress="if(((event.keyCode >= 47) && (event.keyCode <= 64)) || ((event.keyCode >= 33) && (event.keyCode <= 38)) || ((event.keyCode >= 40) && (event.keyCode <= 45)) || ((event.keyCode >= 91) && (event.keyCode <= 96)) || ((event.keyCode >= 123) && (event.keyCode <= 126))) event.returnValue = false;"></asp:TextBox>
                                                                            </td>
                                                                            <td width="11%">
                                                                                &nbsp;</td>
                                                                            <td width="15%">
                                                                                PIN*:</td>
                                                                            <td width="43%">
                                                                                <asp:TextBox ID="txtPassword" TabIndex="12" runat="server" Columns="10" MaxLength="10"></asp:TextBox>&nbsp;(Max 10 digits)
                                                                                <asp:Button ID="btnGeneratePassword" runat="server" CssClass="Frmbutton" Width="84px"
                                                                                    CausesValidation="False" Text="Generate PIN" OnClick="btnGeneratePassword_Click"
                                                                                    UseSubmitBehavior="false" TabIndex="13" onKeyDown="if(event.keyCode==13) return false;">
                                                                                </asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="9%">
                                                                                Nick Name:
                                                                            </td>
                                                                            <td width="22%">
                                                                                <asp:TextBox ID="txtNickname" TabIndex="3" runat="server" Columns="35" onKeypress="if(((event.keyCode >= 47) && (event.keyCode <= 64)) || ((event.keyCode >= 33) && (event.keyCode <= 38)) || ((event.keyCode >= 40) && (event.keyCode <= 45)) || ((event.keyCode >= 91) && (event.keyCode <= 96)) || ((event.keyCode >= 123) && (event.keyCode <= 126))) event.returnValue = false;"></asp:TextBox>
                                                                            </td>
                                                                            <td width="11%">
                                                                                &nbsp;</td>
                                                                            <td nowrap width="15%">
                                                                                Subscriber Role:</td>
                                                                            <td width="43%">
                                                                                <asp:DropDownList ID="ddlRole" TabIndex="14" runat="server" DataTextField="RoleDescription"
                                                                                    DataValueField="RoleID" 
                                                                                    Width="197px" onKeyDown="if(event.keyCode==13) return false;">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td nowrap width="9%" style="height: 28px">
                                                                                Active:</td>
                                                                            <td width="22%" style="height: 28px">
                                                                                <asp:CheckBox ID="cbActive" runat="server" TabIndex="4" onKeyDown="if(event.keyCode==13) return false;">
                                                                                </asp:CheckBox>
                                                                            </td>
                                                                            <td width="11%" style="height: 28px">
                                                                                &nbsp;</td>
                                                                            <td width="15%" style="height: 28px">
                                                                                Specialty:</td>
                                                                            <td width="43%" style="height: 28px">
                                                                                <asp:TextBox ID="txtSpecialty" TabIndex="15" runat="server" Columns="35" onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="9%">
                                                                                Office Phone:</td>
                                                                            <td width="25%" style="height: 28px">
                                                                                (<asp:TextBox ID="txtPrimaryPhoneAreaCode" TabIndex="5" runat="server" Columns="4"
                                                                                    MaxLength="3" onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>)
                                                                                <asp:TextBox ID="txtPrimaryPhonePrefix" TabIndex="6" runat="server" Columns="4" MaxLength="3"
                                                                                    onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>
                                                                                -
                                                                                <asp:TextBox ID="txtPrimaryPhoneNNNN" TabIndex="7" runat="server" Columns="6" MaxLength="4"
                                                                                    onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>
                                                                            </td>
                                                                            <td width="11%">
                                                                                &nbsp;</td>
                                                                            <td width="15%">
                                                                                Affiliation:</td>
                                                                            <td width="40%">
                                                                                <asp:TextBox ID="txtAffiliation" TabIndex="16" runat="server" Columns="35" onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="9%">
                                                                                Fax:
                                                                            </td>
                                                                            <td width="22%" style="height: 28px">
                                                                                (<asp:TextBox ID="txtFaxAreaCode" TabIndex="8" runat="server" Columns="4" MaxLength="3"
                                                                                    onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>)
                                                                                <asp:TextBox ID="txtFaxPrefix" TabIndex="9" runat="server" Columns="4" MaxLength="3"
                                                                                    onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>
                                                                                -
                                                                                <asp:TextBox ID="txtFaxNNNN" TabIndex="10" runat="server" Columns="6" MaxLength="4"
                                                                                    onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>
                                                                            </td>
                                                                            <td width="11%">
                                                                                &nbsp;</td>
                                                                            <td width="15%">
                                                                                Email:</td>
                                                                            <td width="43%">
                                                                                <asp:TextBox ID="txtEmail" TabIndex="17" runat="server" Columns="35" onKeyDown="if(event.keyCode==13) return false;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" align="right">
                                                                                <asp:Button ID="btnUpdateProfile" ValidationGroup="groupProfile" CausesValidation="true"
                                                                                    UseSubmitBehavior="true" TabIndex="18" runat="server" CssClass="Frmbutton" Text="Update Profile"
                                                                                    OnClick="btnUpdateProfile_Click"></asp:Button>
                                                                                &nbsp;&nbsp;
                                                                                <asp:Button ID="btnCancelProfileChanges" TabIndex="19" runat="server" CssClass="Frmbutton"
                                                                                    CausesValidation="False" Text="Cancel"></asp:Button>
                                                                                <asp:Button ID="btnDefault" Visible="true" TabIndex="20" runat="server" CausesValidation="False"
                                                                                    Width="0px" Height="0px" Text=""></asp:Button>
                                                                                <input type="hidden" runat="server" id="txtConfirm" /><asp:Button runat="server"
                                                                                    ID="btnConfirm" Width="0px" Height="0px" OnClick="btnConfirm_Click"></asp:Button>
                                                                            </td>
                                                                            <td align="right">
                                                                                Last Updated:
                                                                                <asp:Label ID="lblLastUpdated" runat="server"></asp:Label>
                                                                                <asp:ValidationSummary ID="vsmrGroupProfile" runat="server" ValidationGroup="groupProfile"
                                                                                    ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
                                                                                <asp:ValidationSummary ID="vsmrValidateForm" runat="server" ValidationGroup="messageValidate"
                                                                                    ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
                                                                                <asp:ValidationSummary ID="vsmrConfig" runat="server" ValidationGroup="groupConfig"
                                                                                    ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
                                                                                <asp:RequiredFieldValidator ID="rfvalFirstName" SetFocusOnError="true" ValidationGroup="groupProfile"
                                                                                    runat="server" ErrorMessage="You must enter a First Name." ControlToValidate="txtFirstName"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                                <asp:RequiredFieldValidator ID="rfvalLastName" SetFocusOnError="true" ValidationGroup="groupProfile"
                                                                                    runat="server" ErrorMessage="You must enter a Last Name" ControlToValidate="txtLastName"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                                <asp:RequiredFieldValidator ID="rfvalLoginID" SetFocusOnError="true" ValidationGroup="groupProfile"
                                                                                    runat="server" ErrorMessage="You must enter a Login ID." ControlToValidate="txtLoginID"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                                <asp:CustomValidator ID="ctmValLogin" runat="server" ControlToValidate="txtLoginID"
                                                                                    ClientValidationFunction="validateLoginID" ValidationGroup="groupProfile"
                                                                                    Display="None" ErrorMessage="Login ID must be 2-10 characters/digits."></asp:CustomValidator>
                                                                                <asp:RegularExpressionValidator ID="revalEmail" ValidationGroup="groupProfile" runat="server"
                                                                                    ErrorMessage="Please enter valid E-mail ID." ControlToValidate="txtEmail" Display="None"
                                                                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="groupProfile"
                                                                                    runat="server" ErrorMessage="Please enter valid Login ID." ControlToValidate="txtLoginID"
                                                                                    Display="None" ValidationExpression="(\w)*(\d)*"></asp:RegularExpressionValidator>
                                                                                <asp:RegularExpressionValidator ID="revalPassword" SetFocusOnError="true" ValidationGroup="groupProfile"
                                                                                    runat="server" ErrorMessage="Password must be 4-10 digits." ControlToValidate="txtPassword"
                                                                                    Display="None" ValidationExpression="\d{4,10}"></asp:RegularExpressionValidator>
                                                                                <asp:RequiredFieldValidator SetFocusOnError="true" ValidationGroup="groupProfile"
                                                                                    ID="rfvalPIN" runat="server" ErrorMessage="You must enter a PIN" ControlToValidate="txtPassword"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnUpdateProfile" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnCancelProfileChanges" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnGeneratePassword" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="txtEmail" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <br>
                                                  
                                                    <fieldset class="fieldsetCBlue">
                                                        <legend id="legendNotifation" runat="server" >Configuration Settings</legend>
                                                        <div id="divNotification" runat="server">
                                                            <table width="99%" border="0" align="center" cellpadding="3" cellspacing="1">
                                                                <tr>
                                                                    <td height="10">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td >
                                                                        <b>Step 1: Notification Devices and Events </b>
                                                                    </td>
                                                                </tr>
                                                                <tr class="Row2">
                                                                    <td class="hd3">
                                                                        
                                                                        
                                                                        <asp:UpdatePanel ID="upnlStep1" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                        <table width="100%" border="0" align="center" cellpadding="3" cellspacing="3">
                                                                            <tr>
                                                                                <td valign="top" align="center">
                                                                                <input type="hidden" id="hdnIsAddClicked" runat="server" name="hdnIsAddClicked" value="false" />
                                                                                   <div class="TDiv" id="ProfileDevicesDiv" runat="server" style="width: 99%;vertical-align:top" onscroll="document.getElementById(hiddenScrollPos).value=this.scrollTop;">
                                                                                        <asp:DataGrid ID="grdDevices" runat="server" CssClass="GridHeader" BorderStyle="None"
                                                                                            DataKeyField="SubscriberID" AutoGenerateColumns="False" AllowSorting="True" Width="100%"
                                                                                            ItemStyle-Height="25px" HeaderStyle-Height="25px"
                                                                                            OnCancelCommand="grdDevices_CancelCommand" OnEditCommand="grdDevices_EditCommand" 
                                                                                            OnUpdateCommand="grdDevices_UpdateCommand" OnDeleteCommand="grdDevices_DeleteCommand"
                                                                                            OnItemDataBound="grdDevices_ItemDataBound">
                                                                                            <SelectedItemStyle Font-Bold="True" ForeColor="Navy" BackColor="#EFCA98"></SelectedItemStyle>
                                                                                            <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                            <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                                                            <Columns>
                                                                                                <asp:BoundColumn Visible="False" DataField="SubscriberDeviceID" ReadOnly="True" HeaderText="SubscriberDeviceID">
                                                                                                </asp:BoundColumn>
                                                                                                <asp:BoundColumn Visible="False" DataField="SubscriberNotificationID" ReadOnly="True" HeaderText="SubscriberNotificationID">
                                                                                                </asp:BoundColumn>
                                                                                                <asp:TemplateColumn HeaderText="Device ID">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDeviceName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'>
                                                                                                        </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txtDeviceName" runat="server" Width="95%" MaxLength="50" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'>
                                                                                                        </asp:TextBox>
                                                                                                    </EditItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn HeaderText="Device Address / Number">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDeviceAddress" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'>
                                                                                                        </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txtDeviceAddress" runat="server" MaxLength="100" Width="95%" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'>
                                                                                                        </asp:TextBox>
                                                                                                    </EditItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:BoundColumn DataField="Carrier" ReadOnly="True" HeaderText="Carrier"></asp:BoundColumn>
                                                                                                <asp:TemplateColumn HeaderText="Email Gateway Address">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblGateway" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'>
                                                                                                        </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txtGateway" runat="server" MaxLength="100" Width="95%" Text='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'>
                                                                                                        </asp:TextBox>
                                                                                                    </EditItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                
                                                                                                <asp:TemplateColumn HeaderText="Event" ItemStyle-Width="10%">
                                                                                                    <ItemStyle Height="23px" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblGridDeviceEvent" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.EventDescription") %>'
                                                                                                            Text='<%# DataBinder.Eval(Container, "DataItem.EventDescription") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:DropDownList ID="dlistGridEvents" runat="server" DataTextField="EventDescription"
                                                                                                            DataValueField="SubscriberNotifyEventID" Width="150px">
                                                                                                        </asp:DropDownList>
                                                                                                    </EditItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn HeaderText="Finding" ItemStyle-Width="12%">
                                                                                                    <ItemStyle Height="23px" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblGridDeviceFinding" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.FindingDescription") %>'
                                                                                                            Text='<%# DataBinder.Eval(Container, "DataItem.FindingDescription") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:DropDownList ID="dlistGridFindings" runat="server"  Width="95px"
                                                                                                            DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                                        </asp:DropDownList>
                                                                                                    </EditItemTemplate>
                                                                                                </asp:TemplateColumn> 
                                                                                                
                                                                                                <asp:EditCommandColumn ItemStyle-ForeColor="Blue" UpdateText="Update" HeaderText="Edit"
                                                                                                    CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
                                                                                                <asp:ButtonColumn HeaderText="Delete" Text="Delete" ItemStyle-ForeColor="Blue" CommandName="Delete">
                                                                                                </asp:ButtonColumn>
                                                                                                <asp:ButtonColumn CommandName="Select" Text="Select" Visible="False"></asp:ButtonColumn>
                                                                                                <asp:BoundColumn Visible="False" DataField="DeviceID" ReadOnly="True" HeaderText="MessageID">
                                                                                                </asp:BoundColumn>
                                                                                                <asp:BoundColumn Visible="False" DataField="SubscriberNotifyEventID" ReadOnly="True" HeaderText="SubscriberNotifyEventID">
                                                                                                </asp:BoundColumn>
                                                                                                <asp:BoundColumn Visible="False" DataField="FindingID" ReadOnly="True" HeaderText="FindingID">
                                                                                                </asp:BoundColumn>
                                                                                            </Columns>
                                                                                        </asp:DataGrid>
                                                                                   </div>&nbsp;
                                                                                    <asp:Label ID="lblDeviceAlreadyExists" runat="server" Style="position: relative"
                                                                                                ForeColor="Red"></asp:Label>
                                                                                        
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" style="margin-left: 0px;">
                                                                                    <table width="99%" border="0" align="center" cellpadding="1" cellspacing="1">
                                                                                        <tr>
                                                                                            <td colspan="7" valign="bottom" class="Step" style="padding: 0 0 0 8px;">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 3%">
                                                                                                <input type="hidden" id="hidDeviceLabel" name="hidDeviceLabel" runat="server" value="" />
                                                                                                <input type="hidden" id="hidGatewayLabel" name="hidGatewayLabel" runat="server" value="" />
                                                                                                <asp:DropDownList ID="cmbDevices" runat="server" AutoPostBack="True" Width="170px"
                                                                                                    DataTextField="DeviceDescription" DataValueField="DeviceID" OnSelectedIndexChanged="cmbDevices_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td style="width: 1%">
                                                                                                <asp:TextBox ID="txtDeviceAddress" TabIndex="21" runat="server"
                                                                                                    Visible="False" Columns="35" MaxLength="100">Enter Number or Email</asp:TextBox>
                                                                                            </td>
                                                                                            <td style="width: 1%">
                                                                                                <asp:DropDownList ID="cmbCarrier" TabIndex="22" runat="server" Visible="False" Width="100px" AutoPostBack="True"
                                                                                                    DataTextField="CarrierDescription" DataValueField="CarrierID" OnSelectedIndexChanged="cmbCarrier_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td style="width: 1%">
                                                                                                <asp:TextBox ID="txtGateway" TabIndex="23" runat="server"
                                                                                                    Visible="False" Columns="35" MaxLength="100"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="width: 1%">
                                                                                                <asp:DropDownList ID="cmbEvent" TabIndex="24" runat="server" Visible="False" Width="130px"
                                                                                                    DataTextField="EventDescription" DataValueField="SubscriberNotifyEventID"> 
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td style="width: 1%">
                                                                                                <asp:DropDownList ID="cmbFinding" TabIndex="25" runat="server" Visible="False"
                                                                                                    Width="100px" DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td style="width: 91%">
                                                                                                <asp:Button ID="btnShowHideDetails" Text="Assign Event" CssClass="Frmbutton" Width="95" Visible="false"
                                                                                                    runat="server" TabIndex="26" OnClick="btnShowHideDetails_Click"> </asp:Button>
                                                                                                &#160;
                                                                                                <asp:Button CssClass="Frmbutton" ID="btnAddDevice" TabIndex="27" runat="server" Visible="False"
                                                                                                    Text="Add" OnClick="btnAddDevice_Click" OnClientClick="return canCallAdd(this)">
                                                                                                </asp:Button>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="center">
                                                                                <td align="center">
                                                                                    &nbsp;<asp:Label ForeColor="Green" Font-Size="Small" ID="lblNoRecordsStep1" runat="server" Text="No Records available"></asp:Label>
                                                                                </td>
                                                                             </tr>
                                                                        </table>
                                                                       </ContentTemplate>
                                                                       <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="grdDevices" />
                                                                       </Triggers>
                                                                       </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <b>Step 2 (optional): After-Hours Notifications</b></td>
                                                                </tr>
                                                                <tr class="Row2">
                                                                    <td class="hd3">
                                                                        
                                                                        <asp:UpdatePanel ID="upnlStep3" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                         <table width="100%" border="0" align="center" cellpadding="3" cellspacing="3">
                                                                            <tr>
                                                                                <td colspan = "5" valign="top" align="center">
                                                                                    
                                                                                  <div class="TDiv" id="ProfileAfterHrsDiv" runat="server" style=" width: 99%;">
                                                                                    <asp:DataGrid ID="grdAfterHoursNotifications" runat="server" CssClass="GridHeader" BorderStyle="None"
                                                                                          AutoGenerateColumns="False" AllowSorting="True" Width="100%" ItemStyle-Height="25px" HeaderStyle-Height="25px"
                                                                                          OnDeleteCommand="grdAfterHoursNotifications_DeleteCommand" OnItemDataBound="grdAfterHoursNotifications_ItemDataBound">
                                                                                          <SelectedItemStyle Font-Bold="True" ForeColor="Navy" BackColor="#EFCA98"></SelectedItemStyle>
                                                                                          <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                          <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True">
                                                                                          </HeaderStyle>                                         
                                                                                        <Columns>
                                                                                            <asp:BoundColumn Visible="False" DataField="SubscriberAfterHoursNotificationID" HeaderText="SubscriberAfterHoursNotificationID">
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="DeviceName" HeaderText="Device"></asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="FindingDescription" HeaderText="Finding"></asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="StartHour" HeaderText="Start"></asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="EndHour" HeaderText="End"></asp:BoundColumn>
                                                                                            <asp:ButtonColumn Text="Delete" ItemStyle-ForeColor ="Blue" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
                                                                                            <asp:ButtonColumn CommandName="Select" Text="Select" Visible="False"></asp:ButtonColumn>                                                    
                                                                                        </Columns>
                                                                                        <AlternatingItemStyle CssClass="AltRow" />
                                                                                    </asp:DataGrid>
                                                                                </div>
                                                                                               
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="5">&nbsp;</td>
                                                                            </tr>                
                                                                            <tr align="left" style="width:99%;" >
                                                                                <td style="padding:0 0 0 6px;width:7%;">
                                                                                    <asp:DropDownList ID="cmbAHDevice" TabIndex="29" runat="server" 
                                                                                        Width="170px" DataTextField="DeviceName" DataValueField="SubscriberDeviceID">
                                                                                    </asp:DropDownList>&nbsp;</td>
                                                                                <td style="width:5%;">
                                                                                    <asp:DropDownList ID="cmbAHFindings" TabIndex="30" runat="server" 
                                                                                        Width="170px" DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                    </asp:DropDownList>&nbsp;</td>
                                                                                <td style="width:0%;">
                                                                                    <asp:DropDownList ID="cmbAHStartHour" TabIndex="31" runat="server" 
                                                                                        Width="90px">
                                                                                        <asp:ListItem Value="1">1am</asp:ListItem>
                                                                                        <asp:ListItem Value="2">2am</asp:ListItem>
                                                                                        <asp:ListItem Value="3">3am</asp:ListItem>
                                                                                        <asp:ListItem Value="4">4am</asp:ListItem>
                                                                                        <asp:ListItem Value="5">5am</asp:ListItem>
                                                                                        <asp:ListItem Value="6">6am</asp:ListItem>
                                                                                        <asp:ListItem Value="7">7am</asp:ListItem>
                                                                                        <asp:ListItem Value="8">8am</asp:ListItem>
                                                                                        <asp:ListItem Value="9">9am</asp:ListItem>
                                                                                        <asp:ListItem Value="10">10am</asp:ListItem>
                                                                                        <asp:ListItem Value="11">11am</asp:ListItem>
                                                                                        <asp:ListItem Value="12" Selected="True">12noon</asp:ListItem>
                                                                                        <asp:ListItem Value="13">1pm</asp:ListItem>
                                                                                        <asp:ListItem Value="14">2pm</asp:ListItem>
                                                                                        <asp:ListItem Value="15">3pm</asp:ListItem>
                                                                                        <asp:ListItem Value="16">4pm</asp:ListItem>
                                                                                        <asp:ListItem Value="17">5pm</asp:ListItem>
                                                                                        <asp:ListItem Value="18">6pm</asp:ListItem>
                                                                                        <asp:ListItem Value="19">7pm</asp:ListItem>
                                                                                        <asp:ListItem Value="20">8pm</asp:ListItem>
                                                                                        <asp:ListItem Value="21">9pm</asp:ListItem>
                                                                                        <asp:ListItem Value="22">10pm</asp:ListItem>
                                                                                        <asp:ListItem Value="23">11pm</asp:ListItem>
                                                                                        <asp:ListItem Value="0">12midnight</asp:ListItem>
                                                                                    </asp:DropDownList>&nbsp;</td>
                                                                                <td style="width:0%;">
                                                                                    <asp:DropDownList ID="cmbAHEndHour" TabIndex="32" runat="server" 
                                                                                        Width="90px" DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                        <asp:ListItem Value="1">1am</asp:ListItem>
                                                                                        <asp:ListItem Value="2">2am</asp:ListItem>
                                                                                        <asp:ListItem Value="3">3am</asp:ListItem>
                                                                                        <asp:ListItem Value="4">4am</asp:ListItem>
                                                                                        <asp:ListItem Value="5">5am</asp:ListItem>
                                                                                        <asp:ListItem Value="6">6am</asp:ListItem>
                                                                                        <asp:ListItem Value="7">7am</asp:ListItem>
                                                                                        <asp:ListItem Value="8">8am</asp:ListItem>
                                                                                        <asp:ListItem Value="9">9am</asp:ListItem>
                                                                                        <asp:ListItem Value="10">10am</asp:ListItem>
                                                                                        <asp:ListItem Value="11">11am</asp:ListItem>
                                                                                        <asp:ListItem Value="12" Selected="True">12noon</asp:ListItem>
                                                                                        <asp:ListItem Value="13">1pm</asp:ListItem>
                                                                                        <asp:ListItem Value="14">2pm</asp:ListItem>
                                                                                        <asp:ListItem Value="15">3pm</asp:ListItem>
                                                                                        <asp:ListItem Value="16">4pm</asp:ListItem>
                                                                                        <asp:ListItem Value="17">5pm</asp:ListItem>
                                                                                        <asp:ListItem Value="18">6pm</asp:ListItem>
                                                                                        <asp:ListItem Value="19">7pm</asp:ListItem>
                                                                                        <asp:ListItem Value="20">8pm</asp:ListItem>
                                                                                        <asp:ListItem Value="21">9pm</asp:ListItem>
                                                                                        <asp:ListItem Value="22">10pm</asp:ListItem>
                                                                                        <asp:ListItem Value="23">11pm</asp:ListItem>
                                                                                        <asp:ListItem Value="0">12midnight</asp:ListItem>
                                                                                    </asp:DropDownList>&nbsp;</td>
                                                                                <td style="width:88%;">
                                                                                    <asp:Button CssClass="Frmbutton" ID="btnAddAH" TabIndex="33" runat="server"
                                                                                        Width="72px" Text="Add" OnClick="btnAddAH_Click"></asp:Button></td>
                                                                             </tr>
                                                                             <tr align ="center">
                                                                                <td colspan= "5" align="center">&nbsp;
                                                                                    <asp:Label ForeColor="Green" Font-Size="Small" ID="lblNoRecordsStep3" runat="server" Text="No Records available"></asp:Label>
                                                                                 </td>
                                                                             </tr>
                                                                         </table>
                                                                        
                                                                       </ContentTemplate>
                                                                       <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="btnAddAH" />
                                                                            <asp:AsyncPostBackTrigger ControlID="grdAfterHoursNotifications" />
                                                                       </Triggers>
                                                                       </asp:UpdatePanel> 
                                                                        
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divOutstandingmessagesRpt" runat="server">
                                                            <asp:UpdatePanel ID="upnlOutMessages" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="99%" border="0" align="center" cellpadding="3" cellspacing="1">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="Row2">
                                                                            <td>
                                                                                <b>Outstanding Messages Reports:</b></td>
                                                                        </tr>
                                                                        <tr class="Row2">
                                                                            <td align="center">
                                                                            <div class="TDiv" id="divReportBy" runat="server" style=" width: 98%;">
                                                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                    <tbody class="TBody">
                                                                                        <tr class="AltRow">
                                                                                            <td width="20%">
                                                                                                Send Reports By:</td>
                                                                                            <td width="61%" align="left">
                                                                                                &nbsp;<asp:CheckBox ID="cbEmailReports" TabIndex="34" runat="server" Text="Email"></asp:CheckBox>
                                                                                                <asp:CheckBox ID="cbFaxReports" TabIndex="35" runat="server" Text="Fax"></asp:CheckBox>&nbsp;
                                                                                            </td>
                                                                                            <td width="19%" align="right">
                                                                                                <asp:Button ID="btnSendOverdueRpt" UseSubmitBehavior="false" TabIndex="36" runat="server"
                                                                                                    CssClass="Frmbutton" Width="164px" Text="Send Overdue Report Now" OnClick="btnSendOverdueRpt_Click">
                                                                                                </asp:Button>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="20%">
                                                                                                Send Reports On:</td>
                                                                                            <td colspan="2">&nbsp;
                                                                                                <asp:CheckBox ID="cbMonday" TabIndex="37" runat="server" Text="Monday"></asp:CheckBox><asp:CheckBox
                                                                                                    ID="cbTuesday" TabIndex="38" runat="server" Text="Tuesday"></asp:CheckBox><asp:CheckBox
                                                                                                        ID="cbWednesday" TabIndex="39" runat="server" Text="Wednesday"></asp:CheckBox><asp:CheckBox
                                                                                                            ID="cbThursday" TabIndex="40" runat="server" Text="Thursday"></asp:CheckBox><asp:CheckBox
                                                                                                                ID="cbFriday" TabIndex="41" runat="server" Text="Friday"></asp:CheckBox><asp:CheckBox
                                                                                                                    ID="cbSaturday" TabIndex="42" runat="server" Text="Saturday"></asp:CheckBox><asp:CheckBox
                                                                                                                        ID="cbSunday" TabIndex="43" runat="server" Text="Sunday"></asp:CheckBox></td>
                                                                                        </tr>
                                                                                        <tr class="AltRow">
                                                                                            <td width="20%">
                                                                                                Send Reports At:</td>
                                                                                            <td colspan="2">&nbsp;
                                                                                                <asp:DropDownList ID="ddlReportTime" TabIndex="44" runat="server" Width="83px">
                                                                                                    <asp:ListItem Value="6">6:00am</asp:ListItem>
                                                                                                    <asp:ListItem Value="7">7:00am</asp:ListItem>
                                                                                                    <asp:ListItem Value="8" Selected="True">8:00am</asp:ListItem>
                                                                                                    <asp:ListItem Value="9">9:00am</asp:ListItem>
                                                                                                    <asp:ListItem Value="10">10:00am</asp:ListItem>
                                                                                                    <asp:ListItem Value="11">11:00am</asp:ListItem>
                                                                                                    <asp:ListItem Value="12">12:00pm</asp:ListItem>
                                                                                                    <asp:ListItem Value="13">1:00pm</asp:ListItem>
                                                                                                    <asp:ListItem Value="14">2:00pm</asp:ListItem>
                                                                                                    <asp:ListItem Value="15">3:00pm</asp:ListItem>
                                                                                                    <asp:ListItem Value="16">4:00pm</asp:ListItem>
                                                                                                    <asp:ListItem Value="17">5:00pm</asp:ListItem>
                                                                                                    <asp:ListItem Value="18">6:00pm</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="3" align="center">
                                                                                                &nbsp;
                                                                                                <asp:Button ID="btnUpdateRptSettings" TabIndex="45" runat="server" CssClass="Frmbutton"
                                                                                                    Text="Update Report Settings" OnClick="btnUpdateRptSettings_Click" UseSubmitBehavior="true">
                                                                                                </asp:Button>
                                                                                                &nbsp;&nbsp;
                                                                                                <asp:Button ID="btnCancelRptSettings" TabIndex="46" runat="server" CssClass="Frmbutton"
                                                                                                    CausesValidation="False" Text="Cancel"></asp:Button>
                                                                                                <asp:TextBox ID="txtValidateMessage" runat="server" Text="" Width="0px" />
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txtValidateMessage"
                                                                                                    ValidationGroup="messageValidate" runat="server" ErrorMessage="Please Select week Day"
                                                                                                    Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="AccountHeaderText NormalItemBackgroundColor" style="width: 122px; height: 1px;
                                                                                                display: none;" colspan="4">
                                                                                                <asp:RegularExpressionValidator ID="revalEmailReport" ValidationGroup="messageValidate"
                                                                                                    runat="server" ErrorMessage="Please enter valid E-mail ID." ControlToValidate="txtEmail"
                                                                                                    Display="None" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                                                <asp:Label ID="lblError" runat="server" ForeColor="Red" Width="501px"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnUpdateRptSettings" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelRptSettings" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSendOverdueRpt" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="txtChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div></br>
                                                        <div id="divMsgConfig" runat="server">
                                                            <asp:UpdatePanel ID="upnlMsgConfig" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="99%" border="0" align="center" cellpadding="3" cellspacing="1">
                                                                        <tr class="Row2">
                                                                            <td >
                                                                                <b>Message Center Configuration</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="Row2"  align="left">
                                                                            <td style="width:98%;margin-left:10px;" >
                                                                                Show all Closed messages since last
                                                                                <asp:TextBox ID="txtNoofDays" TabIndex="47" runat="server" Columns="4" MaxLength="3"></asp:TextBox>
                                                                                days
                                                                                <asp:Label ID="lblmessage" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="Row2">
                                                                            <td width="3%" >
                                                                                <asp:Button ID="btnaddUserConfig" ValidationGroup="groupConfig" CausesValidation="true"
                                                                                    UseSubmitBehavior="true" TabIndex="48" runat="server" CssClass="Frmbutton" OnClick="btnaddUserConfig_Click"
                                                                                    Width="72px" Text="Add"></asp:Button>
                                                                                <asp:RangeValidator ID="rvalDays" ValidationGroup="groupConfig" Type="Integer" ControlToValidate="txtNoofDays"
                                                                                    ErrorMessage="Show all Closed messages since last days should  be between 1 and 30." MaximumValue="30"
                                                                                    Display="none" MinimumValue="1" runat="server" SetFocusOnError="true" />
                                                                                <asp:RequiredFieldValidator ID="rfvalDays" ValidationGroup="groupConfig" runat="server"
                                                                                    ErrorMessage="Show all Closed messages since last days should not be left blank." ControlToValidate="txtNoofDays"
                                                                                    Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnaddUserConfig" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:RegularExpressionValidator ID="revPhoneAreaCode" SetFocusOnError="true" ValidationGroup="groupProfile"
                                runat="server" ErrorMessage="Please enter valid Phone area code." ControlToValidate="txtPrimaryPhoneAreaCode"
                                ValidationExpression="\d{3}" ForeColor="White"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="revPhonePrefix" SetFocusOnError="true" ValidationGroup="groupProfile"
                                runat="server" ErrorMessage="Please enter valid Phone prefix." ValidationExpression="\d{3}"
                                ControlToValidate="txtPrimaryPhonePrefix" ForeColor="White"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="revPhoneNNNN" SetFocusOnError="true" ValidationGroup="groupProfile"
                                runat="server" ErrorMessage="Please enter valid Phone extension." ValidationExpression="\d{4}"
                                ControlToValidate="txtPrimaryPhoneNNNN" ForeColor="White"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="revFaxAreaCode" SetFocusOnError="true" ValidationGroup="groupProfile"
                                runat="server" ErrorMessage="Please enter valid Fax area code." ControlToValidate="txtFaxAreaCode"
                                ValidationExpression="\d{3}" Width="241px" ForeColor="White"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="revFaxPrefix" SetFocusOnError="true" ValidationGroup="groupProfile"
                                runat="server" ErrorMessage="Please enter valid Fax prefix." ValidationExpression="\d{3}"
                                ControlToValidate="txtFaxPrefix" ForeColor="White"></asp:RegularExpressionValidator>&nbsp;
                            <asp:RegularExpressionValidator ID="revFaxNNNN" SetFocusOnError="true" ValidationGroup="groupProfile"
                                runat="server" ErrorMessage="Please enter valid Fax extension." ValidationExpression="\d{4}"
                                ControlToValidate="txtFaxNNNN" ForeColor="White"></asp:RegularExpressionValidator>&nbsp;
                      </div>
                    </td>
                </tr>
            </table>
              
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
