<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="callCenter_setup.aspx.cs"
    Inherits="Vocada.CSTools.callCenter_setup" Title="CSTools: Agent Team Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlCCSetup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script type="text/javascript" language="javascript" src="./Javascript/Common.js"></script>

            <script language="javascript" src="./Javascript/LiveAgent_Notifications.js" type="text/javascript"></script>

            <script language="javascript" type="text/javascript">
               //document.getElementById("ReasonDiv").style.height=150;
                var mapId = "callCenter_setup.aspx";
                /* Navuage to given url */
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
                function checkRequired()
                {  
                    var regxEmail = /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/; 
                    var errorMessage1 = "";
                    var focusOn = "";  
                    
                      
                    var primaryEmail = document.getElementById(txtEmailClientID).value.replace(/^\s+|\s+$/g,"");
                    if (primaryEmail.length > 0)
                    {
                        if (regxEmail.test(primaryEmail)==false)
                        {   errorMessage1 = errorMessage1 + " - Please enter valid Email \n";
                            if (focusOn == "")
                                focusOn = txtEmailClientID;
                        }
                    }
                    if(document.getElementById(txtContactNameClientID).value.replace(/^\s+|\s+$/g,"") == "")
                    {
                       errorMessage1 = errorMessage1 + " - You Must Enter A Contact Name. \n";        
                       focusOn = txtContactNameClientID;
                    }
                    
                     if(document.getElementById(txtPrimaryPhone1ClientID).value.replace(/^\s+|\s+$/g,"") == "")
                    {
                       errorMessage1 = errorMessage1 +  " - You Must Enter A Primary Phone Area Code. \n";        
                       if (focusOn == "")    
                            focusOn = txtPrimaryPhone1ClientID;
                    }
                    
                    if(document.getElementById(txtPrimaryPhone2ClientID).value.replace(/^\s+|\s+$/g,"") == "")
                    {
                       errorMessage1 = errorMessage1 +  " - You Must Enter A Primary Phone Prefix. \n";        
                       if (focusOn == "")    
                            focusOn = txtPrimaryPhone2ClientID;
                    }
                    if(document.getElementById(txtPrimaryPhone3ClientID).value.replace(/^\s+|\s+$/g,"") == "")
                    {
                       errorMessage1 = errorMessage1 +  " - You Must Enter A Primary Phone Extension. \n";        
                       if (focusOn == "")    
                            focusOn = txtPrimaryPhone3ClientID;
                    }
                    
                    var pager = document.getElementById(txtPrimaryPager1ClientID).value + document.getElementById(txtPrimaryPager2ClientID).value + document.getElementById(txtPrimaryPager3ClientID).value
                    if (pager.length > 0 && pager.length != 10)
                    {
                        errorMessage1 = errorMessage1 + " - Please enter valid Pager Number. \n";
                        if (focusOn == "")
                            focusOn = txtPrimaryPager1ClientID;
                    }
                    
                    var fax = document.getElementById(txtPrimaryFax1ClientID).value + document.getElementById(txtPrimaryFax2ClientID).value + document.getElementById(txtPrimaryFax3ClientID).value
                    if (fax.length > 0 && fax.length != 10)
                    {
                        errorMessage1 = errorMessage1 + " - Please enter valid Fax Number. \n";
                        if (focusOn == "")
                            focusOn = txtPrimaryFax1ClientID;
                    }
                    
                    var alternateNum = document.getElementById(txtAlternatePhone1ClientID).value + document.getElementById(txtAlternatePhone2ClientID).value + document.getElementById(txtAlternatePhone3ClientID).value
                    if (alternateNum.length > 0 && alternateNum.length != 10)
                    {
                        errorMessage1 = errorMessage1 + " - Please enter valid Alternate Phone Number. \n";
                        if (focusOn == "")
                            focusOn = txtAlternatePhone1ClientID;
                    }
                    var lockedMsgTimeout = document.getElementById(txtLockedMsgTimeoutClientID).value 
                    if (lockedMsgTimeout < 2 || lockedMsgTimeout > 10)
                    {
                        errorMessage1 = errorMessage1 + " - Please enter Locked Message Timeout between 2-10. \n";
                        if (focusOn == "")
                            focusOn = txtLockedMsgTimeoutClientID;
                    }
                    
                    var autoLogout = document.getElementById(txtAutoLogoutClientID).value 
                    if (autoLogout < 10 || autoLogout > 30)
                    {
                        errorMessage1 = errorMessage1 + " - Please enter Auto Logout Time between 10-30. \n";
                        if (focusOn == "")
                            focusOn = txtAutoLogoutClientID;
                    }
                    
                    if(errorMessage1 != "")
                    {
                        alert(errorMessage1);
                        document.getElementById(focusOn).focus();
                        return false;
                    }
                return true;
                }
                
                  //Check whether other record has been edited, then ask for conformation.
                function confirmOnDataChange()
                {
                    if(document.getElementById(hdnTextChangedClientID).value =="true")
                    {
                        if(confirm("Some data has been changed, do you want to continue?"))
                        {
                            document.getElementById(hdnTextChangedClientID).value = "false";
                            return true;
                        }
                        else
                            return false;                    
                    }
                    return true;
                }
                 /* Check max length of the textbox, if exceed then dont take next input*/
                function setCallCenterReason(controlId, itemIndex)
                {
                    var text = document.getElementById(controlId).value;
                    document.getElementById(hdnGridCCReasonClientID).value = document.getElementById(controlId).value;
                    if (document.getElementById(hdnGridCCReasonClientID).value.trim().length <= 0)
                    {
                        alert('Please enter Agent Team Reason');
                        document.getElementById(controlId).focus();
                        return false;
                    }
                    else
                    {
                       if(itemIndex < 10)
                           __doPostBack('ctl00$ContentPlaceHolder1$grdReason$ctl0' + itemIndex + '$ctl00', '');
                       else
                           __doPostBack('ctl00$ContentPlaceHolder1$grdReason$ctl' + itemIndex + '$ctl00', '');
                        
                        return false;                           
                    }
                    return true;
                }   
                
               
                function onGridCancelClick(controlId, itemIndex)
                {
                    if(confirmOnDataChange())
                    {
                        document.getElementById(hdnTextChangedClientID).value = "false"; 
                        if(itemIndex < 10)
                            __doPostBack('ctl00$ContentPlaceHolder1$grdReason$ctl0' + itemIndex + '$ctl01', '');
                        else
                            __doPostBack('ctl00$ContentPlaceHolder1$grdReason$ctl' + itemIndex + '$ctl01', '');
                        return false;   
                    }
                    else
                    {
                        document.getElementById(controlId).focus();
                        return false;
                    }
                    return true;
                }
                function TextChanged()
                {
                   document.getElementById(hdnTextChangedClientID).value = "true";
                   return true;
                }
                
            
                
            </script>

            <table align="center" border="0" cellpadding="0" cellspacing="0" style="width: 100%;
                height: 99%">
                <tr class="ContentBg">
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                <tr>                                
                                    <td width="62%" class="Hd1">
                                        Agent Team Setup</td>
                                    <td align="right" nowrap width="0%" class="Hd1">
                                    <input type="hidden" id="scrollPos" value="0" runat="server" />  
                                        <asp:HyperLink ID="lnkLable" runat="server" CssClass="Link" NavigateUrl="./add_callcenter.aspx"> Back To Agent Team List</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;<img
                                            src="img/ic_add.gif" /><asp:HyperLink ID="lnkAddAgent" runat="server" CssClass="Link"> Add/Edit Agent</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                cellpadding="0" cellspacing="0">
                                <tr valign="top" style="width: 100%;" align="center">
                                    <td class="Hd2">
                                        <asp:UpdatePanel ID="upnlContact" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset class="fieldsetCBlue">
                                                    <legend class="">
                                                        <asp:Label ID="lblCCInfoLine" runat="server"></asp:Label></legend>
                                                    <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                                        cellpadding="0" cellspacing="0">
                                                        <tr valign="top" style="width: 100%;" align="center">
                                                            <td class="Hd2">
                                                                <fieldset class="fieldsetCBlue">
                                                                    <legend class="">Contact Info</legend>
                                                                    <table id="Table1" cellspacing="3" cellpadding="3" width="90%" border="0">
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
                                                                                Contact Name*:</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtContactName" runat="server" Columns="35" MaxLength="75" TabIndex="1"></asp:TextBox></td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                Alternate Contact:</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtAlternateContact" runat="server" Columns="35" MaxLength="75"
                                                                                    TabIndex="13"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Email:</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtEmail" runat="server" Columns="35" MaxLength="75" TabIndex="2"></asp:TextBox></td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                Alternate Phone:</td>
                                                                            <td>
                                                                                &nbsp;(<asp:TextBox Width="35px" ID="txtAlternatePhone1" runat="server" MaxLength="3"
                                                                                    TabIndex="14"></asp:TextBox>)
                                                                                <asp:TextBox ID="txtAlternatePhone2" Width="35px" runat="server" MaxLength="3" TabIndex="15"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                                    ID="txtAlternatePhone3" Width="55px" runat="server" MaxLength="4" TabIndex="16"></asp:TextBox>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Phone Number*:</td>
                                                                            <td>
                                                                                &nbsp;(<asp:TextBox Width="35px" ID="txtPrimaryPhone1" runat="server" MaxLength="3"
                                                                                    TabIndex="3"></asp:TextBox>)
                                                                                <asp:TextBox ID="txtPrimaryPhone2" Width="35px" runat="server" MaxLength="3" TabIndex="4"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                                    ID="txtPrimaryPhone3" Width="55px" runat="server" MaxLength="4" TabIndex="5"></asp:TextBox>&nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                Groups using Agent Team:</td>
                                                                            <td rowspan="4" align="left" valign="top">
                                                                                <div id="Div1" runat="server" style="overflow-y: Auto; height: 125px; vertical-align: top;
                                                                                    margin-bottom: 10px; margin-left: 0px; margin-right: 0px;" class="TDiv">
                                                                                    <asp:CheckBoxList ID="clstGroupList" runat="server" Width="250px" Height="1px" RepeatLayout="Flow"
                                                                                        TabIndex="17">
                                                                                    </asp:CheckBoxList>
                                                                                </div>
                                                                                <asp:Label ForeColor="Green" Font-Size="X-Small" ID="lblNoRecords" runat="server"
                                                                                    Text="No Groups available" Visible="false"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Fax Number:</td>
                                                                            <td>
                                                                                &nbsp;(<asp:TextBox Width="35px" ID="txtPrimaryFax1" runat="server" MaxLength="3"
                                                                                    TabIndex="6"></asp:TextBox>)
                                                                                <asp:TextBox ID="txtPrimaryFax2" Width="35px" runat="server" MaxLength="3" TabIndex="7"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                                    ID="txtPrimaryFax3" Width="55px" runat="server" MaxLength="4" TabIndex="8"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Pager Number:</td>
                                                                            <td>
                                                                                &nbsp;(<asp:TextBox Width="35px" ID="txtPrimaryPager1" runat="server" MaxLength="3"
                                                                                    TabIndex="9"></asp:TextBox>)
                                                                                <asp:TextBox ID="txtPrimaryPager2" Width="35px" runat="server" MaxLength="3" TabIndex="10"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
                                                                                    ID="txtPrimaryPager3" Width="55px" runat="server" MaxLength="4" TabIndex="11"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Active:</td>
                                                                            <td colspan="3">
                                                                                <asp:CheckBox ID="chkActive" runat="server" TabIndex="12"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                                        cellpadding="0" cellspacing="0">
                                                        <tr valign="top" style="width: 100%;" align="center">
                                                            <td class="Hd2">
                                                                <fieldset class="fieldsetCBlue">
                                                                    <legend class="">Preferences</legend>
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
                                                                        <tr>
                                                                            <td>
                                                                                Locked Message Timeout:</td>
                                                                            <td>
                                                                                Minutes:&nbsp;&nbsp;<asp:TextBox ID="txtLockedMsgTimeout" runat="server" Columns="5"
                                                                                    MaxLength="2" TabIndex="18"></asp:TextBox></td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                Auto Logout:</td>
                                                                            <td>
                                                                                Minutes:&nbsp;&nbsp;<asp:TextBox ID="txtAutoLogout" runat="server" Columns="5" MaxLength="2"
                                                                                    TabIndex="21"></asp:TextBox></td>
                                                                        </tr>                                                                       
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                                        cellpadding="0" cellspacing="0">
                                                        <tr valign="top" style="width: 100%;" align="center">
                                                            <td class="Hd2">
                                                                <fieldset class="fieldsetCBlue">
                                                                    <legend class="">Connect Live Notification Devices</legend>
                                                                    <asp:UpdatePanel ID="upnlStep1" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="100%" align="center" border="0" cellpadding="1" cellspacing="1">
                                                                                <tr>
                                                                                    <td style="width: 1%">
                                                                                        <br />
                                                                                        <asp:Label ID="lblDeviceType" runat="server" Text="Device Type"></asp:Label></td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:Label ID="lblNumAddress" runat="server" Visible="true" Text="Number / Address"></asp:Label></td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:Label ID="lblCarrier" runat="server" Visible="true" Text="Carrier"></asp:Label></td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:Label ID="lblEmailGateway" runat="server" Visible="true" Text="Email Gateway"></asp:Label></td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:Label ID="lblInitialPause" runat="server" Visible="true" Text="Initial Pause"></asp:Label></td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:Label ID="lblEvents" runat="server" Visible="true" Text="Events"></asp:Label></td>
                                                                                    <td style="width: 93%" colspan="4">
                                                                                        &nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 3%">
                                                                                        <asp:DropDownList ID="cmbDeviceType" AutoPostBack="true" runat="server" DataTextField="DeviceDescription"
                                                                                            DataValueField="DeviceID" TabIndex="24" Width="210px" OnSelectedIndexChanged="cmbDeviceType_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:TextBox ID="txtNumAddress" runat="server" MaxLength="100" Visible="true" TabIndex="26"
                                                                                            Width="95px"></asp:TextBox></td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:DropDownList ID="cmbCarrier" runat="server" TabIndex="27" AutoPostBack="true"
                                                                                            DataTextField="CarrierDescription" DataValueField="CarrierID" Visible="true"
                                                                                            Width="120px" OnSelectedIndexChanged="cmbCarrier_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:TextBox ID="txtEmailGateway" runat="server" MaxLength="100" Visible="true" TabIndex="28"
                                                                                            Width="115px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:TextBox ID="txtInitialPause" TabIndex="29" runat="server" Visible="true" Columns="23"
                                                                                            MaxLength="5">1</asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 1%">
                                                                                        <asp:DropDownList ID="cmbEvents" runat="server" TabIndex="30" Visible="true" Width="130px"
                                                                                            DataTextField="EventDescription" DataValueField="AgentNotifyEventID">
                                                                                        </asp:DropDownList></td>
                                                                                    <td style="width: 90%">
                                                                                        &nbsp;<asp:Button ID="btnShowHideDetails" Text="" CssClass="Frmbutton" Width="95" Visible="false"
                                                                                            runat="server" TabIndex="58" OnClick="btnShowHideDetails_Click" CausesValidation="false">  </asp:Button> &nbsp;
                                                                                            <asp:Button ID="btnAddDevice" Text="Add" CssClass="Frmbutton" Width="45" runat="server"
                                                                                            TabIndex="32" OnClick="btnAddDevice_Click" OnClientClick="TextChanged()" CausesValidation="false"/>    
                                                                                   </td>
                                                                                    <td>
                                                                                        <input type="hidden" id="hidInitPauseLabel" name="hidInitPauseLabel" runat="server"
                                                                                            value="" /><input type="hidden" id="hidDeviceLabel" name="hidDeviceLabel" runat="server"
                                                                                                value="" /></td>
                                                                                    <td>
                                                                                        <input type="hidden" id="hidGatewayLabel" name="hidGatewayLabel" runat="server" value="" /></td>
                                                                                </tr>
                                                                            </table>
                                                                            <table width="100%" align="center" border="0" cellpadding="2" cellspacing="2">
                                                                                <tr>
                                                                                    <td align="center" valign="top" style="width: 98%; height: 100%;">
                                                                                        <br />
                                                                                        <input type="hidden" id="hdnOldDeviceName" enableviewstate="true" runat="server"
                                                                                            name="textChanged" value="" />
                                                                                        <input type="hidden" id="hdnIsAddClicked" runat="server" name="hdnIsAddClicked" value="false" />                                                   
                                                                                            
                                                                                        <div id="OCDeviceDiv" runat="server" class="TDiv" onscroll="document.getElementById(hiddenScrollPos).value=this.scrollTop;" style="width: 99.5%;vertical-align: top" >
                                                                                            <asp:DataGrid ID="grdDevices" runat="server" CssClass="GridHeader" BorderStyle="None"
                                                                                                DataKeyField="RowID" AutoGenerateColumns="False" Width="100%" ItemStyle-Height="21px"
                                                                                                HeaderStyle-Height="25" OnDeleteCommand="grdDevices_DeleteCommand" OnEditCommand="grdDevices_EditCommand"
                                                                                                OnCancelCommand="grdDevices_CancelCommand" OnItemDataBound="grdDevices_ItemDataBound"
                                                                                                OnUpdateCommand="grdDevices_UpdateCommand" OnItemCreated="grdDevices_ItemCreated"
                                                                                                TabIndex="33">
                                                                                                <SelectedItemStyle Font-Bold="True" ForeColor="Navy" BackColor="#EFCA98"></SelectedItemStyle>
                                                                                                <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                                <HeaderStyle VerticalAlign="Top" CssClass="THeader" HorizontalAlign="Left" Font-Bold="True">
                                                                                                </HeaderStyle>
                                                                                                <Columns>
                                                                                                    <asp:TemplateColumn HeaderText="Device Type" ItemStyle-Width="12%">
                                                                                                        <ItemStyle Height="23px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGridDeviceType" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'
                                                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <EditItemTemplate>
                                                                                                            <asp:TextBox ID="txtGridDeviceType" runat="server" MaxLength="50" TabIndex="2" Width="95px"
                                                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>' ValidationGroup="updatePanelEditDevice">                                             
                                                                                                            </asp:TextBox>
                                                                                                            <asp:RequiredFieldValidator ID="rfvGridDeviceType" runat="server" Display="None"
                                                                                                                ErrorMessage="You Must Enter A Device Name" ControlToValidate="txtGridDeviceType"
                                                                                                                ValidationGroup="updatePanelEditDevice"></asp:RequiredFieldValidator>
                                                                                                        </EditItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:TemplateColumn HeaderText="Group" ItemStyle-Width="13%" Visible="false">
                                                                                                        <ItemStyle Height="23px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGridGroup" runat="server" EnableViewState="true"></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:TemplateColumn HeaderText="Number / Address" ItemStyle-Width="15%">
                                                                                                        <ItemStyle Height="23px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGridDeviceNumber" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'
                                                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <EditItemTemplate>
                                                                                                            <asp:TextBox ID="txtGridDeviceNumber" runat="server" Width="120px" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'
                                                                                                                ValidationGroup="updatePanelEditDevice" MaxLength="100"></asp:TextBox>
                                                                                                            <asp:RequiredFieldValidator ID="rfvGridDeviceNumber" runat="server" Display="None"
                                                                                                                ErrorMessage="You Must Enter A Device Address / Number" ControlToValidate="txtGridDeviceNumber"
                                                                                                                ValidationGroup="updatePanelEditDevice"></asp:RequiredFieldValidator>
                                                                                                        </EditItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:TemplateColumn HeaderText="Carrier" ItemStyle-Width="13%">
                                                                                                        <ItemStyle Height="23px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGridCarrier" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.Carrier") %>'
                                                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.Carrier") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:TemplateColumn HeaderText="Email Gateway" ItemStyle-Width="18%">
                                                                                                        <ItemStyle Height="23px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGridDeviceEmailGateway" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'
                                                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <EditItemTemplate>
                                                                                                            <asp:TextBox ID="txtGridEmailGateway" runat="server" Width="180px" MaxLength="100"
                                                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'></asp:TextBox>
                                                                                                        </EditItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:TemplateColumn HeaderText="Event" ItemStyle-Width="10%">
                                                                                                        <ItemStyle Height="23px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGridDeviceEvent" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.EventDescription") %>'
                                                                                                                Text='<%# DataBinder.Eval(Container, "DataItem.EventDescription") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <EditItemTemplate>
                                                                                                            <asp:DropDownList ID="dlistGridEvents" runat="server" TabIndex="6" DataTextField="EventDescription"
                                                                                                                DataValueField="AgentNotifyEventID" Width="130px">
                                                                                                            </asp:DropDownList>
                                                                                                        </EditItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:TemplateColumn HeaderText="Finding" ItemStyle-Width="12%" Visible="false">
                                                                                                        <ItemStyle Height="23px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGridDeviceFinding" runat="server" EnableViewState="true"></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:TemplateColumn HeaderText="Initial Pause" ItemStyle-Width="12%" Visible="false">
                                                                                                        <ItemStyle Height="23px" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGridInitialPause" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.InitialPause") %>'>
                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <EditItemTemplate>
                                                                                                            <asp:TextBox ID="txtGridInitialPause" runat="server" Width="120px" Text='<%# DataBinder.Eval(Container, "DataItem.InitialPause") %>'></asp:TextBox>
                                                                                                        </EditItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:EditCommandColumn HeaderText="Edit" UpdateText="Update" CancelText="Cancel"
                                                                                                        EditText="Edit" CausesValidation="false" ValidationGroup="updatePanelEditDevice"
                                                                                                        ItemStyle-Width="10%">
                                                                                                        <HeaderStyle Width="5%" />
                                                                                                        <ItemStyle Height="23px" />
                                                                                                    </asp:EditCommandColumn>
                                                                                                    <asp:TemplateColumn HeaderText="Delete" ItemStyle-Width="10%">
                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CausesValidation="false"
                                                                                                                CommandName="Delete" OnClientClick="return ConformBeforeDelete();"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateColumn>
                                                                                                    <asp:BoundColumn Visible="False" DataField="DeviceID" ReadOnly="True" HeaderText="MessageID">
                                                                                                    </asp:BoundColumn>
                                                                                                    <asp:BoundColumn Visible="False" DataField="GroupID" ReadOnly="True" HeaderText="GroupID">
                                                                                                    </asp:BoundColumn>
                                                                                                    <asp:BoundColumn Visible="False" DataField="FindingID" ReadOnly="True" HeaderText="FindingID">
                                                                                                    </asp:BoundColumn>
                                                                                                    <asp:BoundColumn Visible="False" DataField="AgentNotifyEventID" ReadOnly="True" HeaderText="Clinical TeamNotifyEventID">
                                                                                                    </asp:BoundColumn>
                                                                                                    <asp:BoundColumn Visible="False" DataField="RowID" ReadOnly="True" HeaderText="Clinical">
                                                                                                    </asp:BoundColumn>
                                                                                                    <asp:BoundColumn Visible="False" DataField="CallCenterID" ReadOnly="True" HeaderText="CallCenterID">
                                                                                                    </asp:BoundColumn>
                                                                                                    <asp:BoundColumn Visible="False" DataField="AgentDeviceID" ReadOnly="True" HeaderText="DeviceID">
                                                                                                    </asp:BoundColumn>
                                                                                                    <asp:BoundColumn Visible="False" DataField="AgentNotificationID" ReadOnly="True"
                                                                                                        HeaderText="NotificationID"></asp:BoundColumn>
                                                                                                </Columns>
                                                                                                <AlternatingItemStyle CssClass="AltRow" />
                                                                                            </asp:DataGrid>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="center">
                                                                                    <td align="center">
                                                                                        &nbsp;<asp:Label ForeColor="Green" Font-Size="X-Small" ID="lblNoRecordsStep1" runat="server"
                                                                                            Text="No Records available"></asp:Label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="grdDevices" />
                                                                            <asp:AsyncPostBackTrigger ControlID="cmbDeviceType" />
                                                                            <asp:AsyncPostBackTrigger ControlID="cmbCarrier" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                                        cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="Hd2" align="center" style="height: 30px">
                                                                <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="Frmbutton" OnClick="btnAdd_Click"
                                                                    TabIndex="34"></asp:Button>
                                                                &nbsp;&nbsp;
                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                                                    CssClass="Frmbutton" TabIndex="35"></asp:Button>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlHid" runat="server" UpdateMode="Always">
                                                                    <ContentTemplate>
                                                                        <asp:HiddenField ID="hdnTextChanged" runat="server" Value="false" EnableViewState="true" />
                                                                        <input type="hidden" id="hdnGridChanged" enableviewstate="true" runat="server" name="hdnGridChanged"
                                                                            value="false" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
