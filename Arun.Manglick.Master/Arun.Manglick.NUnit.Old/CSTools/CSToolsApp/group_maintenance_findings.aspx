<%@ Page Language="c#" MasterPageFile="~/cs_tool.master" Theme="csTool" Inherits="Vocada.CSTools.group_maintenance_findings" CodeFile="group_maintenance_findings.aspx.cs" Title="CSTools: Findings and Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlFindings" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
    <script language="JavaScript" src="Javascript/gmf_notificationStep1.js" type="text/JavaScript"></script>
    
    <script language="JavaScript" type="text/JavaScript">
        var mapId = "group_maintenance_findings.aspx";
    </script>
    <table style="height:100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">        
       <tr class="ContentBg">
          <td valign="top" style="height: 50px">
             <input type="hidden" id="scrollPos" value="0" runat="server" />             
             <div style="overflow-y:Auto; height: 100%">
                    <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                        <tr>
                            <td  style="width:62%" class="Hd1">
                                <asp:Label ID="lblFindingsTitle" runat="server" Text="Findings and Notifications"></asp:Label>
                            </td>
                            <td style="width:38%" align="right" class="Hd1">
                                &nbsp;
                                <asp:LinkButton ID="lnkGroupMaintenance" runat="server" CssClass="AccountInfoText" Text="Group Maintenance" OnClick="lnkGroupMaintenance_Click" ></asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkGroupPreferences" runat="server" CssClass="AccountInfoText" Text="Group Preferences"  OnClick="lnkGroupPreferences_Click"></asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkAddFindings" runat="server" CssClass="AccountInfoText" Text="Add Finding" OnClick="lnkAddFindings_Click" ></asp:LinkButton>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table id="Table4" width="100%" border="0" cellpadding="=0" cellspacing="0">
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset class="fieldsetBlue">
                                    <legend class="">&nbsp;<b>Findings</b></legend>
                                    <table cellspacing="0" cellpadding="0" width="99%" border="0" align="center">
                                        <tr class="Row2">
                                            <td class="hd3">
                                                <br />
                                                <div id="GroupFindingsDiv" class="TDiv">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0" style="vertical-align:top;" align="center">
                                                        <tr>
                                                            <td>
                                                                <asp:DataGrid ID="grdFindings" runat="server" CssClass="GridHeader" AutoGenerateColumns="False"
                                                                    AllowSorting="True" Width="100%" OnItemCommand="grdFindings_ItemCommand">
                                                                    <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                    <HeaderStyle CssClass="THeader" VerticalAlign="Middle">
                                                                    </HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="FindingID" ReadOnly="True" Visible="false" HeaderText="Finding ID">
                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="FindingDescription" ReadOnly="True" HeaderText="Finding">
                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:TemplateColumn HeaderText="Active" HeaderStyle-Width="50px">
                                                                            <ItemTemplate>
                                                                            <asp:Image ID="imgActive" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn HeaderText="Default" HeaderStyle-Width="50px">
                                                                            <ItemTemplate>
                                                                            <asp:Image ID="imgDefault" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                                        </asp:TemplateColumn>
                                                                        <asp:HyperLinkColumn DataNavigateUrlField="FindingVoiceOverURL" DataNavigateUrlFormatString="{0}" 
                                                                            DataTextField="FindingVoiceOverURL" HeaderText="Finding Voiceover" DataTextFormatString="&lt;img border=0 src='./img/ic_play_msg.gif'&gt;"
                                                                            ItemStyle-Width="60px">
                                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        </asp:HyperLinkColumn>
                                                                        <asp:BoundColumn DataField="ComplianceGoal" HeaderText="Compliance Goal" DataFormatString="{0} mins">
                                                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EscalateEvery" HeaderText="Escalate Every" DataFormatString="{0} mins">
                                                                            <HeaderStyle HorizontalAlign="center" Width="60px"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EndAfterMinutes" HeaderText="End After" DataFormatString="{0} mins">
                                                                            <HeaderStyle HorizontalAlign="center" Width="60px"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="StartBackupAt" HeaderText="Start Backup At" DataFormatString="{0} Esc">
                                                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="ContinueToSendPrimary" HeaderText="Continue To Primary">
                                                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Embargo" HeaderText="Embargo?" DataFormatString="{0}">
                                                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EmbargoStartHour" HeaderText="Embargo Start">
                                                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EmbargoEndHour" HeaderText="Embargo End">
                                                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EmbargoSpanWeekend" HeaderText="Weekend Embargo" DataFormatString="{0}">
                                                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="RequireReadback" HeaderText="Require Readback">
                                                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:TemplateColumn HeaderText="Document Only">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgDocumented" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Wrap="true" HorizontalAlign="Center" ></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="AgentActionTypeName" HeaderText="Agent Action">
                                                                            <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Height="21px"></ItemStyle>
                                                                        </asp:BoundColumn>
                                                                        <asp:ButtonColumn Text="Edit" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Blue" HeaderText="Edit" CommandName="Edit"></asp:ButtonColumn>
								                                    </Columns>
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset class="fieldsetBlue">
                                    <legend class="">&nbsp;<b>Group Notifications</b></legend>
                                    <table width="99%" border="0" cellpadding="0" cellspacing="0" align="center">
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp;<b>Notification Devices and Events</b></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr class="Row2">
                                            <td class="hd3">
                                                <asp:UpdatePanel ID="upnlStep1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="top" align="center">
                                                            <div class="TDiv" id="GroupFindingsDevicesDiv" runat="server" style="width: 99.5%;
                                                                vertical-align: top" onscroll="document.getElementById(hiddenScrollPos).value=this.scrollTop;">
                                                                <asp:DataGrid ID="grdDevices" runat="server" CssClass="GridHeader" BorderStyle="None"
                                                                    DataKeyField="GroupID" AutoGenerateColumns="False" AllowSorting="True" Width="100%"
                                                                    OnItemDataBound="grdDevices_ItemDataBound" OnCancelCommand="grdDevices_CancelCommand"
                                                                    OnUpdateCommand="grdDevices_UpdateCommand" OnEditCommand="grdDevices_EditCommand"
                                                                    OnDeleteCommand="grdDevices_DeleteCommand">
                                                                    <SelectedItemStyle Font-Bold="True" ForeColor="Navy" BackColor="#EFCA98"></SelectedItemStyle>
                                                                    <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                    <HeaderStyle VerticalAlign="Middle" CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"
                                                                        Height="25px"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundColumn Visible="False" DataField="GroupDeviceID" ReadOnly="True" HeaderText="GroupDeviceID">
                                                                        </asp:BoundColumn>
                                                                        <asp:TemplateColumn HeaderText="Device ID" ItemStyle-Height="21px">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgrdDeviceName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="txtgrdDeviceName" runat="server" CssClass="AccountHeaderText" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'>
                                                                                </asp:TextBox>
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn HeaderText="Device Address / Number" ItemStyle-Height="21px">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgrdDeviceAddress" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="txtgrdDeviceAddress" runat="server" CssClass="AccountHeaderText"
                                                                                    Width="200px" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'>
                                                                                </asp:TextBox>
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="Carrier" ReadOnly="True" HeaderText="Carrier"></asp:BoundColumn>
                                                                        <asp:TemplateColumn HeaderText="Email Gateway Address" ItemStyle-Height="21px">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgrdGateway" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="txtgrdGateway" runat="server" CssClass="AccountHeaderText" Width="250px"
                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'>
                                                                                </asp:TextBox>
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateColumn>                                                                      
                                                                        <asp:ButtonColumn CommandName="Select" Text="Select" Visible="False"></asp:ButtonColumn>
                                                                        <asp:BoundColumn Visible="False" DataField="DeviceID" ReadOnly="True" HeaderText="MessageID">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn Visible="False" DataField="FindingID" ReadOnly="True" HeaderText="FindingID">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn Visible="False" DataField="GroupNotifyEventID" ReadOnly="True"
                                                                            HeaderText="NotifyEventID"></asp:BoundColumn>
                                                                         <asp:TemplateColumn HeaderText="Event" ItemStyle-Width="10%">
                                                                            <ItemStyle Height="23px" />
                                                                            <ItemTemplate>                                                                                
                                                                                <asp:Label ID="lblGridDeviceEvent" runat="server" EnableViewState="true"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:DropDownList ID="dlistGridEvents" runat="server" TabIndex="6" DataTextField="EventDescription"
                                                                                    DataValueField="GroupNotifyEventID" Width="115px">
                                                                                </asp:DropDownList>
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn HeaderText="Finding" ItemStyle-Width="12%">
                                                                            <ItemStyle Height="23px" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGridDeviceFinding" runat="server" EnableViewState="true"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:DropDownList ID="dlistGridFindings" runat="server"  Width="105px"
                                                                                    DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                </asp:DropDownList>
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateColumn> 
                                                                        <asp:BoundColumn Visible="False" DataField="GroupNotificationID" ReadOnly="True"
                                                                            HeaderText="GroupNotificationID"></asp:BoundColumn>
                                                                          <asp:EditCommandColumn ButtonType="LinkButton" ItemStyle-ForeColor="Blue" UpdateText="Update"
                                                                            HeaderText="Edit" CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
                                                                        <asp:TemplateColumn HeaderText="Delete" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton runat="server" ID="DeleteButton" ForeColor="Blue" CommandName="Delete"
                                                                                    Text="Delete" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </div>
                                                            <asp:Label ID="lblDeviceAlreadyExistsStep1" runat="server" ForeColor="Red" Style="position: relative"></asp:Label>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="5" width="100%">
                                                            <tr>
                                                                <td><asp:Label ID="lblDeviceType" runat="server" Text="Device Type"></asp:Label></td>
                                                                <td><asp:Label ID="lblNumAddress" runat="server" Visible="false" Text="Number / Address" ></asp:Label></td>
                                                                <td><asp:Label ID="lblCarrier" runat="server" Visible="false" Text="Carrier"></asp:Label></td>
                                                                <td><asp:Label ID="lblEmailGateway" runat="server" Visible="false" Text="Email Gateway"></asp:Label></td>
                                                                <td><asp:Label ID="lblEvents" runat="server" Visible="false" Text="Events"></asp:Label></td>
                                                                <td><asp:Label ID="lblFindings" runat="server" Visible="false" Text="Finding"></asp:Label></td>
                                                                <td colspan="2" style="width: 94%"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbDevices" TabIndex="18" runat="server" CssClass="AccountHeaderText"
                                                                        Width="220px" DataTextField="DeviceDescription" DataValueField="DeviceID" AutoPostBack="True"
                                                                        OnSelectedIndexChanged="cmbDevices_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDeviceAddress" TabIndex="19" runat="server" CssClass="AccountHeaderText"
                                                                        Width="175px" Visible="False" MaxLength="100">Enter Number or Email</asp:TextBox>
                                                                 </td>
                                                                 <td>                                                                
                                                                    <asp:DropDownList ID="cmbCarrier" TabIndex="20" runat="server" CssClass="AccountHeaderText"
                                                                        Visible="False" DataTextField="CarrierDescription" DataValueField="CarrierID"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="cmbCarrier_SelectedIndexChanged" Width="100px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGateway" TabIndex="21" runat="server" CssClass="AccountHeaderText"
                                                                        Width="220px" Visible="False" MaxLength="100"></asp:TextBox></td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbEvents" TabIndex="23" runat="server" CssClass="AccountHeaderText"
                                                                        Width="120px" DataTextField="EventDescription" DataValueField="GroupNotifyEventID" Visible="False">
                                                                     </asp:DropDownList>                                                            
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbFindings" TabIndex="24" runat="server" CssClass="AccountHeaderText"
                                                                        Width="80px" DataTextField="FindingDescription" DataValueField="FindingID"  Visible="False">
                                                                    </asp:DropDownList>         
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnShowHideDetails" Text="" CssClass="Frmbutton" Width="95" Visible="false"
                                                                              runat="server" TabIndex="58" OnClick="btnShowHideDetails_Click" CausesValidation="false">      
                                                                    </asp:Button>                                                                    
                                                                </td>
                                                                <td style="width: 93%">
                                                                     <asp:Button ID="btnAddDevice" TabIndex="22" runat="server" CssClass="Frmbutton" Width="62px"
                                                                        Visible="False" Text="Add" OnClick="btnAddDevice_Click" OnClientClick="return validateDevices()" CausesValidation="false">
                                                                     </asp:Button>                                                                                                                                
                                                                </td>
                                                            </tr>
                                                            </table>                           
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td>
                                                            <input type="hidden" id="hdnDeviceLabel" name="hdnDeviceLabel" runat="server" value="" />
                                                            <input type="hidden" id="hdnGatewayLabel" name="hdnGatewayLabel" runat="server" value="" />
                                                            <input type="hidden" id="hdnIsAddClicked" runat="server" name="hdnIsAddClicked" value="false" />                                                   
                                                            &nbsp;<asp:Label ForeColor="Green" Font-Size="Small" ID="lblNoRecordsStep1" runat="server"
                                                                Text="No Records available"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdDevices" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddDevice" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbDevices" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </fieldset>
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