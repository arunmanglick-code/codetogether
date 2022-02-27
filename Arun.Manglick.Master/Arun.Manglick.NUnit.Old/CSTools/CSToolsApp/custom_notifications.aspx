<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="custom_notifications.aspx.cs"
    Inherits="Vocada.CSTools.custom_notifications" Title="CSTools: Custom Notifications"
    ValidateRequest="false" SmartNavigation="true" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlNotificationTemplateList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>          

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td valign="top">
                        <div style="overflow-y: Auto; height: 100%;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                            <tr>
                                                <td class="Hd1">
                                                    Custom Notifications
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" align="center" style="vertical-align: top; margin-top: 0px;"
                                            cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <table width="96%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="Hd2" align="center" style="vertical-align: top;">
                                                                <fieldset class="fieldsetCBlue">
                                                                    <legend class=""><b>Select</b></legend>
                                                                    <asp:UpdatePanel ID="upnlSelect" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="70%" border="0" cellspacing="0" cellpadding="0" align="center">
                                                                                <tr>
                                                                                    <td valign="middle" style="white-space: nowrap; width: 10%;" align="left">
                                                                                        Institution:</td>
                                                                                    <td align="left" style="white-space: nowrap; width: 35%;">
                                                                                        <asp:DropDownList ID="cmbInstitution" runat="server" AutoPostBack="true" DataTextField="InstitutionName"
                                                                                            DataValueField="InstitutionID" Width="200px" OnSelectedIndexChanged="cmbInstitution_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td valign="middle" style="white-space: nowrap; width: 7%;" align="left">
                                                                                        Group:</td>
                                                                                    <td align="left">
                                                                                        <asp:DropDownList ID="cmbGroup" runat="server" AutoPostBack="true" DataTextField="GroupName"
                                                                                            DataValueField="GroupID" Width="200px" OnSelectedIndexChanged="cmbGroup_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="cmbInstitution" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <table id="tblCustomNotifications" width="96%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="Hd2" align="center" style="vertical-align: top;">
                                                                <fieldset class="fieldsetCBlue" style="margin-left: 0px; width: 100%; margin-bottom: 0px;
                                                                    vertical-align: top;">
                                                                    <legend class="">Custom Notification List</legend>
                                                                    <div id="CustomNotificationDiv" runat="server" class="TDiv" style="margin-bottom: 5px;
                                                                        margin-top: 5px; margin-left: 0px; margin-right: 0px; height: 30px; width: 99%;">
                                                                        <asp:UpdatePanel ID="upnlCustomNotifications" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:DataGrid ID="grdCustomNotifications" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                    TabIndex="1" DataKeyField="NotificationTemplateID" CellPadding="0" CssClass="GridHeader"
                                                                                    ItemStyle-HorizontalAlign="left" ItemStyle-Height="25px" HorizontalAlign="left"
                                                                                    Width="100%" BorderWidth="1px" OnEditCommand="grdCustomNotifications_EditCommand"
                                                                                    OnDeleteCommand="grdCustomNotifications_DeleteCommand" OnItemCreated="grdCustomNotifications_OnItemCreated"
                                                                                    OnSortCommand="grdCustomNotifications_SortCommand">
                                                                                    <HeaderStyle CssClass="THeader" Font-Bold="True" HorizontalAlign="left" VerticalAlign="Middle"
                                                                                        Height="25px" />
                                                                                    <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                    <Columns>
                                                                                        <asp:BoundColumn Visible="False" DataField="NotificationTemplateID" ReadOnly="True"
                                                                                            HeaderText="NotificationTemplateID"></asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="DeviceDescription" ReadOnly="True" HeaderText="Device"
                                                                                            SortExpression="DeviceDescription" ItemStyle-Width="15%" HeaderStyle-Width="15%">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="DeviceID" ReadOnly="True" HeaderText="DeviceID">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="Recipient" ReadOnly="True" HeaderText="Recipient" SortExpression="Recipient"
                                                                                            ItemStyle-Width="15%" HeaderStyle-Width="15%"></asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="RecipientID" ReadOnly="True" HeaderText="RecipientID">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="EventDescription" ReadOnly="True" HeaderText="Event"
                                                                                            SortExpression="EventDescription" ItemStyle-Width="15%" HeaderStyle-Width="15%">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="EventID" ReadOnly="True" HeaderText="EventID">
                                                                                        </asp:BoundColumn> 
                                                                                        
                                                                                        <asp:BoundColumn DataField="MsgSendTypeDesc" ReadOnly="True" HeaderText="Msg Type"
                                                                                            SortExpression="MsgSendTypeDesc" ItemStyle-Width="10%" HeaderStyle-Width="10%">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="MsgSendType" ReadOnly="True" HeaderText="MsgSendType">
                                                                                        </asp:BoundColumn>
                                                                                                                                                                               
                                                                                        <asp:BoundColumn DataField="SubjectFullText" ReadOnly="True" HeaderText="Subject"
                                                                                            ItemStyle-Width="45%" HeaderStyle-Width="45%"></asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="SubjectText" ReadOnly="True" HeaderText="SubjectText">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="BodyText" ReadOnly="True" HeaderText="BodyText">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="FaxTemplateURL" ReadOnly="True" HeaderText="FaxTemplateURL">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="Escalation" ReadOnly="True" HeaderText="Escalation">
                                                                                        </asp:BoundColumn>
                                                                                        <asp:TemplateColumn HeaderText="Edit">
                                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnkButEdit" Text="Edit" runat="server" CommandName="Edit"></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:TemplateColumn HeaderText="Delete">
                                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnkButDelete" Text="Delete" runat="server" CommandName="Delete"
                                                                                                    OnClientClick="return ConformBeforeDelete();"></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:BoundColumn Visible="False" DataField="OtherMsgTypeTemplateExists" ReadOnly="True" HeaderText="OtherMsgTypeTemplateExists">
                                                                                        </asp:BoundColumn>
                                                                                    </Columns>
                                                                                </asp:DataGrid>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="grdCustomNotifications" EventName="DataBinding" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </div>                                                                  
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td colspan="2" valign="top">
                                                    <table width="100%" border="0">
                                                        <tr>
                                                            <td valign="top" style="width: 66%">
                                                                <asp:UpdatePanel ID="upnlTemplates" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table id="tblAddEditNotificationTemplate" align="center" width="94%" border="0"
                                                                            cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td class="Hd2" align="center">
                                                                                    <fieldset class="fieldsetCBlue" style="width: 100%; height: 99%; margin-left: 0px;">
                                                                                        <legend class="">Add/Edit Notification Template</legend>
                                                                                        <table align="center" width="96%" border="0" cellpadding="2" cellspacing="1">
                                                                                            <tr>
                                                                                                <td nowrap style="width: 15%">
                                                                                                    <asp:Label ID="lblSelectRecipientHeader" runat="server" Text="Recipient:" EnableViewState="false"></asp:Label>
                                                                                                </td>
                                                                                                <td nowrap>
                                                                                                    <asp:DropDownList ID="cmbRecipientTypes" runat="server" AutoPostBack="true" TabIndex="2"
                                                                                                        OnSelectedIndexChanged="cmbRecipientTypes_SelectedIndexChanged" Width="150px">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td nowrap style="width: 15%">
                                                                                                    <asp:Label ID="lblSelectDeviceHeader" runat="server" Text="Device:" EnableViewState="false"></asp:Label>
                                                                                                </td>
                                                                                                <td nowrap>
                                                                                                    <asp:DropDownList ID="cmbDevices" runat="server" AutoPostBack="true" TabIndex="3"
                                                                                                        OnSelectedIndexChanged="cmbDevices_SelectedIndexChanged" Width="150px">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td nowrap style="width: 15%">
                                                                                                    <asp:Label ID="lblSelectEventHeader" runat="server" Text="Event:" EnableViewState="false"></asp:Label>
                                                                                                </td>
                                                                                                <td nowrap>
                                                                                                    <asp:DropDownList ID="cmbEvents" runat="server" AutoPostBack="true" TabIndex="4"
                                                                                                        Width="150px" CausesValidation="false" 
                                                                                                        OnSelectedIndexChanged="cmbEvents_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td nowrap style="width: 15%">
                                                                                                    <asp:Label ID="lblMegType" runat="server" Text="Message Type:" EnableViewState="false"></asp:Label>
                                                                                                </td>
                                                                                                <td nowrap>
                                                                                                    <asp:DropDownList ID="cmbMsgType" runat="server" AutoPostBack="false" TabIndex="5"
                                                                                                        Width="150px" CausesValidation="false" >
                                                                                                        <asp:ListItem Text="<Select>" Value="-1" Selected="true" ></asp:ListItem>
                                                                                                        <asp:ListItem Text="Original" Value="1"  ></asp:ListItem>
                                                                                                        <asp:ListItem Text="Forwarded" Value="2"  ></asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <div id="divGeneralTemplate" runat="server" visible="true">
                                                                                                        <table align="left" width="97%" border="0" cellpadding="2" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td nowrap style="width: 15%">
                                                                                                                    <asp:Label ID="lblSubjectHeader" runat="server" Text="Subject:" EnableViewState="false"></asp:Label>
                                                                                                                </td>
                                                                                                                <td nowrap>
                                                                                                                    <table align="left" width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                        <tr>
                                                                                                                            <td align="left" style="width: 70%">
                                                                                                                                <asp:TextBox ID="txtSubject" TabIndex="6" runat="server" Columns="70" MaxLength="60"
                                                                                                                                    CausesValidation="false" ></asp:TextBox>
                                                                                                                            </td>
                                                                                                                            <td nowrap valign="middle" align="left">
                                                                                                                                &nbsp;&nbsp;<asp:Label ID="lblSubjectTextMaxLength" runat="server" Text="Max 60 Chars"
                                                                                                                                    EnableViewState="false"></asp:Label>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td nowrap style="width: 15%" valign="top">
                                                                                                                    <asp:Label ID="lblBodyHeader" runat="server" Text="Body:" EnableViewState="false"></asp:Label>
                                                                                                                </td>
                                                                                                                <td nowrap>
                                                                                                                    <table align="left" width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                        <tr>
                                                                                                                            <td align="left" style="width: 70%">
                                                                                                                                <asp:TextBox ID="txtBody" TabIndex="7" runat="server" Columns="70" Rows="5" MaxLength="250"
                                                                                                                                    TextMode="MultiLine" CausesValidation="false" ></asp:TextBox>
                                                                                                                            </td>
                                                                                                                            <td nowrap valign="middle" align="left">
                                                                                                                                &nbsp;&nbsp;<asp:Label ID="lblBodyTextMaxLength" runat="server" Text="Max 250 Chars"></asp:Label>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td colspan="2">
                                                                                                                    &nbsp;
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td nowrap style="width: 15%" valign="top">
                                                                                                                    &nbsp;
                                                                                                                </td>
                                                                                                                <td nowrap valign="top" align="center">
                                                                                                                    <table align="center" width="85%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <asp:Button ID="btnReset" TabIndex="9" runat="server" Text="Reset" CssClass="Frmbutton"
                                                                                                                                    Height="20px" Width="80px" OnClick="btnReset_Click" CausesValidation="false" />&nbsp;&nbsp;&nbsp;
                                                                                                                                <asp:Button ID="btnCancel" TabIndex="10" runat="server" Text="Cancel" CssClass="Frmbutton"
                                                                                                                                    Height="20px" Width="80px" OnClick="btnCancel_Click" CausesValidation="false" />&nbsp;&nbsp;&nbsp;
                                                                                                                                <asp:Button ID="btnSaveGeneralTemplate" TabIndex="8" runat="server" Text="Save" CssClass="Frmbutton"
                                                                                                                                    Height="20px" Width="80px" OnClick="btnSaveGeneralTemplate_Click"  />&nbsp;&nbsp;&nbsp;    
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                 </td> 
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                    <div id="divFaxTemplate" runat="server" visible="true">
                                                                                                        <table align="left" width="100%" border="0" cellpadding="2" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td nowrap style="width: 15%">
                                                                                                                    <asp:Label ID="lbl" runat="server" Text="Fax Template:" EnableViewState="false"></asp:Label>
                                                                                                                </td>
                                                                                                                <td nowrap align="left">
                                                                                                                    <asp:HyperLink ID="lnkFaxUrl" Height="22px" Width="12px" runat="server" ImageUrl="./img/ic_details.gif"
                                                                                                                        Visible="true" Style="vertical-align: middle; padding-right: 3" TabIndex="6"
                                                                                                                        Target="_blank"></asp:HyperLink><asp:FileUpload ID="flupdCTFaxTemplate" runat="server"
                                                                                                                            Width="250" CssClass="frmButton" Style="height: 18px" TabIndex="7" />
                                                                                                                    <asp:LinkButton ID="lbtnUseDefault" runat="server" Text="Use Default" Style="vertical-align: middle;
                                                                                                                        padding-left: 3" TabIndex="7" OnClick="lbtnUseDefault_Click" ></asp:LinkButton>
                                                                                                                </td>
                                                                                                                <td style="width: 20%">
                                                                                                                    &nbsp;</td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td colspan="3">
                                                                                                                    &nbsp;
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td nowrap style="width: 15%" valign="top">
                                                                                                                    &nbsp;
                                                                                                                </td>
                                                                                                                <td nowrap valign="top" align="center">
                                                                                                                    <table width="60%" border="0" cellpadding="0" cellspacing="0" align="left">
                                                                                                                        <tr>
                                                                                                                            <td align="center">
                                                                                                                               <asp:Button ID="btnResetForFax" TabIndex="10" runat="server" Text="Cancel" CssClass="Frmbutton"
                                                                                                                                 Height="20px" Width="80px" OnClick="btnCancel_Click" CausesValidation="false" />&nbsp;&nbsp;&nbsp;
                                                                                                                               <asp:Button ID="btnSaveFaxTemplate" TabIndex="9" runat="server" Text="Save" CssClass="Frmbutton"
                                                                                                                                    Height="20px" Width="80px" OnClick="btnSaveFaxTemplate_Click"/>&nbsp;&nbsp;&nbsp;  
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                                <td style="width: 20%">
                                                                                                                    &nbsp;</td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <asp:Label ID="lblDeviceNotes" runat="server" Text=""></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </fieldset>
                                                                                </td>
                                                                            </tr>
                                                                        </table>                                                               
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnSaveGeneralTemplate" EventName="Click" />
                                                                        <asp:PostBackTrigger ControlID="btnSaveFaxTemplate" />
                                                                        <asp:AsyncPostBackTrigger ControlID="cmbDevices" EventName="SelectedIndexChanged" />
                                                                        <asp:AsyncPostBackTrigger ControlID="cmbEvents" EventName="SelectedIndexChanged" />
                                                                        <asp:AsyncPostBackTrigger ControlID="cmbRecipientTypes" EventName="SelectedIndexChanged" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <table id="tblVariablesToIncludeList" align="left" width="96%" border="0" cellpadding="0"
                                                                    cellspacing="0">
                                                                    <tr>
                                                                        <td class="Hd2" align="center">
                                                                            <fieldset class="fieldsetCBlue" style="margin-left: 0px;">
                                                                                <legend class="">Variables To Include</legend>
                                                                                <div id="divVariablesToInclude" class="TDiv" style="vertical-align: top; margin-bottom: 5px;
                                                                                    margin-top: 5px; margin-left: 0px; margin-right: 0px; height: 285px; width: 99%;">
                                                                                    <asp:DataGrid ID="grdNotificationTemplateFields" runat="server" AllowSorting="false"
                                                                                        AutoGenerateColumns="False" DataKeyField="Code" CellPadding="0"
                                                                                        CssClass="GridHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Height="25px"
                                                                                        HorizontalAlign="left" Height="100%" Width="99%" BorderWidth="1px">
                                                                                        <HeaderStyle CssClass="THeader" Font-Bold="True" HorizontalAlign="center" VerticalAlign="Middle" />
                                                                                        <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                        <Columns>
                                                                                            <asp:TemplateColumn HeaderText="Field Name">
                                                                                                <ItemStyle HorizontalAlign="left" />
                                                                                                <HeaderStyle HorizontalAlign="left" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lbtnFieldName" Text='<%# DataBinder.Eval(Container, "DataItem.FieldName") %>'
                                                                                                        runat="server" OnClientClick="return insertAtCursor(this)"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="Code">
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lbtnCode" Text='<%# DataBinder.Eval(Container, "DataItem.Code") %>'
                                                                                                        runat="server" OnClientClick="return insertAtCursor(this)"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateColumn>
                                                                                        </Columns>
                                                                                    </asp:DataGrid>
                                                                                </div>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr valign="top" style="height: 100%">
                                                <td>
                                                    <asp:UpdatePanel ID="upnlHidenData" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <input type="hidden" id="hdnDataChanged" runat="server" name="hdnDataChanged" value="false" />                                                        
                                                            <input type="hidden" id="hdnTextboxID" runat="server" name="hdnTextboxID" enableviewstate="true" />
                                                            <input type="hidden" id="hdnOldMsgType" runat="server" name="hdnOldMsgType" enableviewstate="true" />
                                                            <input type="hidden" id="hdnOtherMsgTypeExists" runat="server" name="hdnOtherMsgTypeExists" enableviewstate="true" />
                                                            <input type="hidden" id="hdnOverWrite" runat="server" name="hdnOverWrite" value="false" enableviewstate="true" />
                                                            <input type="hidden" id="hdnIsEdit" runat="server" name="hdnIsEdit" value="false" enableviewstate="true" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">  
                var mapId = "custom_notifications.aspx";     
                
                // This function will ask for conformation before deletion record
                function ConformBeforeDelete()
                {
                    if(someDataChanged())
                    {
                        if(confirm("You are deleting the custom notification.\nThe system will now use the default notification."))
                        {
                            return true;
                        }
                    }
                    return false;    
                 }                
             
                
                //Sets the flag textChanged to true if the text of dept name textbox is changed.
                function dataChanged()
                {   
                    try
                    {
                        document.getElementById(hdnDataChangedClientID).value = "true";
                    }
                    catch(e)
                    {}
                   return true;
                }
                
                //Validate UI Form - Becuase Validators are not firing due 
                //to smart navigation attribute into header
                function ValidateUIForm(isFaxTemplateUI)                
                {                    
                   var errMsg= "";
                   var focusObj = null; 
                   try
                   {
                        var oldMsgType = document.getElementById(hdnOldMsgTypeClientID).value; 
                        var isEdit = document.getElementById(hdnIsEditClientID).value; 

                        //Validation for Event
                        var objEventDDL = document.getElementById(cmbEventsClientID);
                        if (objEventDDL)
                        {
                            if(objEventDDL.value == "-1")
                            {
                                errMsg += "- You must select an Event \n";                                
                                focusObj = objEventDDL;
                            }
                        }
                        var objMsgSendTypetDDL = document.getElementById(cmbMsgTypeClientID);
                        if (objMsgSendTypetDDL)
                        {
                            if(objMsgSendTypetDDL.value == "-1")
                            {
                                errMsg += "- You must select a Message Type \n";    
                                if(focusObj == null)                            
                                    focusObj = objMsgSendTypetDDL;
                            }
                        }
                        
                        if(isFaxTemplateUI)
                        {
                            //Validation for file name extension
                          var objFileUpload = document.getElementById(flupdCTFaxTemplateClientID);                                                    
                          
                          if(objFileUpload)
                          {
                                var fileName = objFileUpload.value;
                                if(fileName.trim().length > 0) 
                                {
                                    var ext = fileName.substr(fileName.length - 4,4);
                                    if (ext != ".rtf")
                                    {
                                         errMsg += "- Please select valid Fax Template Filename";                          
                                         if(focusObj == null)
                                            focusObj = objFileUpload;
                                    }
                                }       
                          }                         
                        }
                        else
                        {
                            //Validation for Subject Text
                            var objDevice = document.getElementById(cmbDevicesClientID);
                            var objSubject = document.getElementById(txtSubjectClientID);
                            if (objDevice && objSubject)
                            {
                               var subjectText = objSubject.value.replace( new RegExp(" ","g"), "");
                               if(objDevice.value == 1 && subjectText.length ==0)
                               {
                                    errMsg += "- You must enter Subject \n";                          
                                    if(focusObj == null)
                                        focusObj = objSubject;
                               }                               
                            }
                           
                            //Validation for Body Text
                            var objBody = document.getElementById(txtBodyClientID);
                            if (objBody)
                            {
                               var bodyText = objBody.value.replace( new RegExp(" ","g"), "");
                               if(bodyText.length ==0)
                               {
                                    errMsg += "- You must enter Body";                          
                                    if(focusObj == null)
                                        focusObj = objBody;
                               }//Numeric Pager Validation
                               else if(objDevice.value == 5 && bodyText.length > 0)
                               {
                                    if(!isValidNumericPagerTemplateText(bodyText))
                                    {
                                        errMsg += "- Body is not valid for Pager-Numeric-Regular device";                          
                                        if(focusObj == null)
                                            focusObj = objBody;
                                    }
                               }
                            }
                        }//else
                        
                        if(errMsg.length != 0)
                        {
                            alert(errMsg);
                            focusObj.focus();
                            return false;
                        }  
                        else if (objMsgSendTypetDDL.value != oldMsgType && isEdit == "true")
                        {
                            if (document.getElementById(hdnOtherMsgTypeExistsClientID).value == "True")
                            {
                                if(confirm("A custom notification for this scenario already exists.\nDo you want to overwrite the current notification?"))
                                {
                                  document.getElementById(hdnOverWriteClientID).value = 'true';
                                  document.getElementById(hdnDataChangedClientID).value = 'false';
                                  return true;
                                }
                                else
                                { 
                                    document.getElementById(hdnOverWriteClientID).value = 'false';
                                    return false;
                                }
                            }
                        }                      
                        
                   }//try
                   catch(e)
                   {}
                   return true;
                }
                
                /* Alert user if data chnaged in edit section of lab test */
                function someDataChanged()
                {    
                    try
                    {               
                        if(document.getElementById(hdnDataChangedClientID).value == 'true')
                        {
                            if(confirm("Some data has been changed, do you want to continue?"))
                            {
                                document.getElementById(hdnDataChangedClientID).value = 'false';
                                return true;
                            }
                            return false;         
                         }
                     }
                    catch(e)
                    {}
                    return true;
                }
                
                //Check and Restrict user for Body Length        
                function checkBodyLength(objTextArea)
                { 
                   try
                    {
                        dataChanged();            
                        var selectedDeviceID = document.getElementById(cmbDevicesClientID).value;
                        var maxLength = 250;
                        if(selectedDeviceID != 1) //Other than Email device
                        {
                            var bodyText = objTextArea.value;                        
                            if(bodyText.length > maxLength)
                            {
                                objTextArea.value = objTextArea.value.substring(0,maxLength);
                            }
                        }
                     }
                    catch(e)
                    {}
                    return false;
                }
                //This Function inserts code at cursor position into text box while click on grid
                function insertAtCursor(objLinkbtn) 
                {
                   var objCode = objLinkbtn; 
                   // if user clicks on fields name then get object of code field.
                   if(objLinkbtn.id.indexOf("lbtnFieldName")>=0)
                   {
                        var objCodeID = objLinkbtn.id.replace("lbtnFieldName","lbtnCode");
                        objCode = document.getElementById(objCodeID);
                   }
                   
                   var txtID = document.getElementById(hdnTextboxIDClientID).value;
                   if (txtID != "")
                   {
                       var objTargetTxt = document.getElementById(txtID);
                       var  strCode = objCode.innerText;
                        //IE support
                        if (document.selection) 
                        {
                            objTargetTxt.focus();
                            sel = document.selection.createRange();
                            sel.text = strCode;
                        }
                        //MOZILLA/NETSCAPE support
                        else if (objTargetTxt.selectionStart || objTargetTxt.selectionStart == "0") 
                        {
                            var startPos = objTargetTxt.selectionStart;
                            var endPos = objTargetTxt.selectionEnd;
                            objTargetTxt.value = objTargetTxt.value.substring(0, startPos)
                            + strCode
                            + objTargetTxt.value.substring(endPos, objTargetTxt.value.length);
                         } 
                        else 
                        {
                            objTargetTxt.value += strCode;
                        }
                    }
                    return false;
                }
                //This function sets the text box id
                function getTextboxID(txtID)
                {
                    document.getElementById(hdnTextboxIDClientID).value = txtID;
                }
                
                //Function to validate valid numeric pager text
                function isValidNumericPagerTemplateText(templateText)
                {                            
                    var isValid = false;                    
                    //Replace [GP800NM] with blank
                    templateText = replaceAll(templateText,"[GP800NM]", "");
                    //Replace [L800NM] with blank
                    templateText = replaceAll(templateText,"[L800NM]", "");
                    //Replace [N800NM] with blank
                    templateText = replaceAll(templateText,"[N800NM]", "");
                    //Replace [OC800NM] with blank
                    templateText = replaceAll(templateText,"[OC800NM]", "");
                    //Replace [PC] with blank
                    templateText = replaceAll(templateText,"[PC]", "");
                    //Replace spaces
                    templateText = templateText.replace( new RegExp(" ","g"), "");                    
                     var re_Number = /\D+/ ;		          
                    //Check if templateText is not a number
                    isValid = (!re_Number.test(templateText));
                                        
                    return isValid;       
                }
                
                //Function to replace Text
                function replaceAll(targetText, toBeReplaced, byReplaced) 
                {
                    while ( targetText.indexOf(toBeReplaced) != -1)
                    {
                        targetText = targetText.replace(toBeReplaced,byReplaced);
                    }
                    return targetText;
                }
    </script>

</asp:Content>
