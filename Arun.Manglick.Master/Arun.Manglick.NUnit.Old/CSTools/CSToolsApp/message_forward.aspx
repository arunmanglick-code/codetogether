<%@ Page Language="C#" Inherits="Vocada.CSTools.MessageForward" AutoEventWireup="true"
    CodeFile="message_forward.aspx.cs" MasterPageFile="~/cs_tool.master" Title="CSTools: Message Forward" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
            // This function validates for the Clinician Name and Note textbox before forwarding the message.
            function Validate()
            {
               if(document.getElementById(ddlRefPhysicianClientID).selectedIndex < 0 && document.getElementById(dlistUnitsClientID) == null)
               {
                    alert('Please select Clinician/Clinical Team.');
                    return false;
               }
               
               if(document.getElementById(ddlRefPhysicianClientID).selectedIndex < 0 && document.getElementById(dlistUnitsClientID) != null)
               {
                    if(document.getElementById(dlistUnitsClientID).selectedIndex <= 0)
                    {
                        alert('Please select Clinician/Clinical Team and/or Unit.');
                        return false;
                    }
               }
               if(document.getElementById(txtNoteClientID).value.length <= 0 || trim(document.getElementById(txtNoteClientID).value)=='')
               {       
                    alert('Please enter Note.');
                    return false;
               }
            }
            
            function setReceipientValue()
            {
                 var selectedIndex = document.getElementById(ddlRefPhysicianClientID).selectedIndex
                document.getElementById(hdnReceipientIndexClientID).value = selectedIndex;
                var receipientName = document.getElementById(ddlRefPhysicianClientID).options[selectedIndex].text;
                if(receipientName.indexOf("<") != -1)
                    receipientName = receipientName.substring(0, receipientName.indexOf("<"));
                document.getElementById(hdnReceipientNameClientID).value = receipientName;
            }
            function UpdateProfile()
            {
                document.getElementById(textChangedClientID).value = "true";
            }
            
            function Navigate(msgID, IsdeptMessage, IslabMessage)
            {
                try
                {
                    window.location.href = "message_details.aspx?MessageID=" + msgID + "&IsDeptMsg=" + IsdeptMessage + "&IsLabMsg=" + IslabMessage;
                }
                catch(_error)
                {
                    return;
                }
            }
            var mapId = "message_forward.aspx";
    
    </script>

    <script language="javascript" type="text/javascript" src="Javascript/Common.js"></script>

    <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td class="DivBg" valign="top">
                <div style="overflow-y: Auto; height: 100%; background-color: White">
                
                    <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                        <tr>
                            <td class="Hd1">
                            <input type="hidden" id="textChanged" enableviewstate="true" runat="server" name="textChanged"
                                value="false" />
                                <asp:Label ID="lblForwardMessage" runat="server" CssClass="UserCenterTitle">&nbsp;&nbsp;Forward Message to New Recipient</asp:Label>
                            </td>
                            <td class="Hd1" align="right">
                                <asp:HyperLink ID="lnkBack" runat="server" >Back to Message</asp:HyperLink>&nbsp;&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblMessage" runat="server" CssClass="AccountHeaderText" Font-Bold="True"
                                    ForeColor="red"></asp:Label></td>
                        </tr>
                    </table>
                    <br />
                    <table width="75%" align="center" style="margin-left: 10px;" border="0" cellpadding="=0"
                        cellspacing="0">
                        <tr>
                            <td class="Hd2">
                                <fieldset class="fieldsetCBlue" style="width: auto">
                                    <legend class="">Message Forward</legend>
                                    <asp:UpdatePanel ID="upnlDataPanel" runat="server" UpdateMode="conditional">
                                        <ContentTemplate>
                                            <table id="tblAddNoteForwardMsg" cellspacing="1" cellpadding="1" align="center" width="100%"
                                                border="0">
                                                <input type="hidden" id="fwdSuccessID" runat="server" name="fwdSuccessID" value="" />
                                                <input type="hidden" id="hdnReceipientIndex" runat="server" name="hdnReceipientIndex" value="-1" />
                                                <input type="hidden" id="hdnReceipientName" runat="server" name="hdnReceipientName" value="" />
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td style="background-color: white;">
                                                        From:</td>
                                                    <td style="background-color: white;">
                                                        <asp:Label ID="lblFrom" runat="server" Font-Bold="True"></asp:Label></td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td>
                                                        Finding:</td>
                                                    <td>
                                                        <asp:Label ID="lblFinding" runat="server" Font-Bold="True"></asp:Label></td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td width="15%" style="height: 128px">
                                                    </td>
                                                    <td style="height: 128px" width="10%" valign="top">
                                                        <asp:Label ID="lblClinition" runat="server" Text="Clinician/Clinical Team:"></asp:Label>
                                                    </td>
                                                    <td style="height: 128px">
                                                        <asp:UpdatePanel ID="upnlReceipient" runat="server" UpdateMode="conditional">
                                                            <ContentTemplate>
                                                                &nbsp;<asp:ListBox ID="ddlRefPhysician" DataTextField="RecipientDisplayName" DataValueField="RowNumber"  EnableViewState="false" 
                                                                    runat="server" Width="98%" Height="98%"></asp:ListBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlRefPhysician" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td width="15%" style="height: 128px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td width="15%">
                                                    </td>
                                                    <td valign="top" width="10%">
                                                        <asp:Label ID="lblUnits" runat="server" Text="Unit:"></asp:Label></td>
                                                    <td style="white-space: nowrap;">
                                                        <table>
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:UpdatePanel ID="upnlUnits" runat="server" UpdateMode="conditional">
                                                                        <ContentTemplate>
                                                                            &nbsp;<asp:DropDownList ID="dlistUnits" runat="server" Width="188px" TabIndex="1"  
                                                                                DataValueField="UnitID" DataTextField="UnitName" AutoPostBack="true" OnSelectedIndexChanged="dlistUnits_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="dlistUnits" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    &nbsp;&nbsp;<asp:Label ID="lblRoomBed" runat="server" Text="Bed:"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:UpdatePanel ID="upnlRoomBeds" runat="server" UpdateMode="conditional">
                                                                        <ContentTemplate>
                                                                            &nbsp;<asp:DropDownList ID="dlistRoomBed" runat="server" Width="188px" TabIndex="1"  
                                                                                DataValueField="RoomBedID" DataTextField="RoomBedNumber">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="dlistRoomBed" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="15%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 70px">
                                                    </td>
                                                    <td style="height: 70px" valign="top">
                                                        <asp:Label ID="lblAddNote" runat="server">Add Note:</asp:Label></td>
                                                    <td style="height: 70px">
                                                        <asp:TextBox ID="txtNote" runat="server" Width="99%" Rows="5" Height="100%" MaxLength="500"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td width="15%" style="height: 70px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td>
                                                        <div id="LabTestDiv" runat="server">
                                                            <table id="Table4" style="height: 23px" cellspacing="1" cellpadding="1" width="100%"
                                                                border="0">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblLabTestResults" runat="server" CssClass="UserCenterTitle"><b>Lab Test Result</b></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                            <asp:UpdatePanel ID="upnlLabResult" runat="server" UpdateMode="conditional">
                                                                <ContentTemplate>
                                                                    <div id="ForwardTestReultDiv" class="TDiv" style="vertical-align: top; height: 110px;">
                                                                        <asp:DataGrid CssClass="GridHeader" ID="grdTestResults" runat="server" AllowSorting="True"  
                                                                            AutoGenerateColumns="False" Width="100%" Height="100%" OnItemDataBound="grdTestResults_ItemDataBound">
                                                                            <HeaderStyle VerticalAlign="Top" CssClass="THeader" HorizontalAlign="Left" Font-Bold="True">
                                                                            </HeaderStyle>
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="TestDescription" HeaderText="Test" ItemStyle-Height="20px"
                                                                                    ItemStyle-Width="25%"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ResultLevel" HeaderText="Result Level" ItemStyle-Height="20px"
                                                                                    ItemStyle-Width="25%"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="FindingDescription" HeaderText="Finding" ItemStyle-Height="20px"
                                                                                    ItemStyle-Width="25%"></asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="1st Instance" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="25%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="Image1" runat="server" Visible='<%# Eval("NewFinding") %>' BorderWidth="0"
                                                                                            ImageUrl="~/img/ic_tick.gif"></asp:Image>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="10%" />
                                                                                    <ItemStyle Height="20px" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <AlternatingItemStyle CssClass="AltRow" />
                                                                        </asp:DataGrid>
                                                                    </div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="grdTestResults" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="tblCloseMessage" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td width="12%" nowrap>
                                                        <asp:Label ID="lblCloseOrgMsg" runat="server" Text="Close Original Message:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkCloseMessage" runat="server" Style="position: relative" Text=""
                                                            TextAlign="Left" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <table id="Table1" cellspacing="1" cellpadding="1" align="center" width="100%" border="0">
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td align="center">
                                                <asp:UpdatePanel ID="uplnButtons" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnForward" runat="server" CssClass="Frmbutton" Text="Forward Message"
                                                            OnClientClick="return Validate()" OnClick="btnForward_Click" />
                                                        &nbsp; &nbsp;<asp:Button ID="btnCancel" CssClass="Frmbutton" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnForward" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
