<%@ Page Language="c#" Inherits="Vocada.CSTools.messageDetails" CodeFile="message_details.aspx.cs"
    MasterPageFile="~/cs_tool.master" Title="CSTools: Message Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMessageList" runat="server">
        <contenttemplate>

            <script type="text/javascript" language="javascript" src="./Javascript/Common.js"></script>

            <script type="text/javascript" language="javascript">
            // This refreshes page after Notification Sent Message is shown to user and user clicks on ok button.
            var mapId =  "message_details.aspx";
            function RefreshPage(query, alertMessage)
            {
                if(alertMessage.length > 0)
                    alert(alertMessage);
                window.self.location.href='./message_details.aspx?' + query;
            }
            // This refreshes page after Notification Error Message is shown to user and user clicks on ok button.
            function ErrorRefreshPage(query)
            {
                alert('Error While Resend Notification. Try again later.');
                window.self.location.href='./message_details.aspx?MessageID=' + query +'&UpdateMessage=' ;
            }
            function goForward(navigationPage)
            {
                 Navigate("./" + navigationPage);
                 return false;
            }
             
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
            //This function handles validation for Add note section.
            function checkNote()
            {
                var errorMessage = "Please enter following: ";
                var errorMessage1 = "";
                var errorMessage2 = "";        
                if((document.getElementById(tbAuthorClientID).value == "") && (document.getElementById(tbNoteClientID).value == ""))
                {
                   errorMessage1 = "\n - Author \n - Note";        
                }
                else if(document.getElementById(tbAuthorClientID).value == "")
                {
                    errorMessage2 = "\n - Author";
                }
                else if(document.getElementById(tbNoteClientID).value == "" || trim(document.getElementById(tbNoteClientID).value)=='')
                {
                    errorMessage2 = "\n - Note";
                }
                if(errorMessage1 != "")
                {
                    alert(errorMessage + errorMessage1);
                    document.getElementById(tbAuthorClientID).focus();
                    return false;
                }
                else if(errorMessage2 != "")
                {
                  alert(errorMessage + errorMessage2);
                  document.getElementById(tbNoteClientID).focus();
                  return false;
                }
                return true;
            }
             
            var Result;
             //Open the popup window after some delay when the user clicks on speaker icon of message having readback open.
            function ShowPopup(sURL)
            {  
               Stop();    
               var url;
               var readbackID;
               var temp = new Array();
               temp = sURL.split('$');
               url = temp[0];
               readbackID = temp[1];
               var messageid = temp[2];
               var deptMsg = temp[3];
               var objArgument = new Object();					
               objArgument.ParentObject = window;
               objArgument.URL = "message_readback_confirmation.aspx?ReadBackID=" + readbackID + "&VoiceURL=" + url + "&MessageID=" + messageid + "&IsDeptMsg=" + deptMsg;	
               winProper = 'dialogHeight:142px;dialogLeft:300px;dialogTop:300px;dialogWidth:250px;center:yes;dialogHide:no;edge:sunken;resizable:no;scroll:yes;status:no;unadorned:yes;help:no;title=0';
               var result = window.showModalDialog('readback_popup.aspx',objArgument,winProper);			
               window.parent.location.href = 'message_details.aspx?MessageID=' + messageid;
               objArgument = null;
               
            }
            function UpdateProfile()
            {
                document.getElementById(textChangedClientID).value = "true";
            }
            var mapId = "message_details.aspx";
            var pagename  = "ctl00$ContentPlaceHolder1$lbtnRefresh";
             
            StartMsgDetail(pagename);
            var MsgDetailTimerID = 0;
            var MsgDetailTStart  = null;
            
            function UpdateTimerMsgDetail(sURL) 
            {
               if(MsgDetailTimerID) 
               {
                  clearTimeout(MsgDetailTimerID);
                  clockID  = 0;
               }

               if(!MsgDetailTStart)
                 MsgDetailTStart   = new Date();

               var   tDate = new Date();
               var   tDiff = tDate.getTime() - MsgDetailTStart.getTime();

               tDate.setTime(tDiff);
               MsgDetailTimerID = setTimeout("UpdateTimerMsgDetail()", 180000);
               __doPostBack(sURL, "");
               StopMsgDetail();
               StartMsgDetail(pagename);
            }

            //timer start function. sets the timeout as 60 sec.
            function StartMsgDetail(sURL) 
            {
               MsgDetailTStart   = new Date();
               MsgDetailTimerID  = setTimeout("UpdateTimerMsgDetail('" + sURL + "')", 180000);
            }

            //Stops the timer
            function StopMsgDetail() 
            {
               if(MsgDetailTimerID) 
               {
                  clearTimeout(MsgDetailTimerID);
                  MsgDetailTimerID  = 0;
               }
               MsgDetailTStart = null;
            }
            
            //Display the actual message popup
            function showActualMessagePopup(notificationHistoryID,messageType)
            {
                var objArgument = new Object();					
    	                
                objArgument.ParentObject = window;
                objArgument.URL =  "actual_message.aspx?NotificationHistoryID=" + notificationHistoryID +"&MessageType="+messageType;	
                {				
                    winProper = 'dialogHeight:380px;dialogLeft:200px;dialogTop:120px;dialogWidth:579px;center:yes;dialogHide:no;edge:sunken;resizable:no;scroll:yes;status:no;unadorned:no;help:no'	;    	    
                }								
                var result = window.showModalDialog('VCPopUP.aspx',objArgument,winProper);			
                objArgument = null;
            }
            
    </script>

            <table align="center" height="100%" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%; width: 100%; background-color: White">
                        <input type="hidden" id="textChanged" enableviewstate="true" runat="server" name="textChanged"
                                value="false" />
                            <table width="100%" border="0" cellpadding="=0" cellspacing="0" style="height: 1px">
                                <tr>
                                    <td class="Hd1" style="height: 19px">
                                        <asp:Label ID="lblMessageDetails" runat="server" CssClass="UserCenterTitle">Message Details</asp:Label>
                                    </td>
                                    <td align="right" class="Hd1" style="height: 19px;">
                                        <asp:Label ID="backtoMessage" runat="server" CssClass="UserCenterTitle">
                                            <asp:HyperLink ID="hlnkBackTo" runat="server" Text="Back To Messages" NavigateUrl="~/message_center.aspx"></asp:HyperLink>
                                            &nbsp;&nbsp;&nbsp;</asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="DivBg" valign="top" align="center">
                                        <div id="MessageDiv" class="TDiv" style="vertical-align: top; margin-top: 5px;">
                                            <asp:DataGrid CssClass="GridHeader" ID="dgMessage" runat="server" AllowSorting="True"
                                                UseAccessibleHeader="true" AutoGenerateColumns="False" Width="100%" Height="100%">
                                                <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="&#160;&#160;&#160;&#160;">
                                                        <ItemTemplate>
                                                            <asp:Image ID="Image" runat="server" ImageUrl="img/ic_ballon_green.gif"></asp:Image>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="MessageCreatedBySystem" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center"
                                                        HeaderText="Created"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="GroupName" HeaderText="Group"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="SpecialistDisplayName" HeaderText="Created By"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CreatedOn_UsersTime" HeaderText="Created On" DataFormatString="{0:MM/dd/yyyy <br /> hh:mm:sstt} ">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RecipientDisplayName" HeaderText="Recipients"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="BedNumber" HeaderText="Bed"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="FindingDescription" HeaderText="Finding"></asp:BoundColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="PatientVoiceURL" DataNavigateUrlFormatString="{0}"
                                                        DataTextField="PatientVoiceURL" HeaderText="Patient Name" DataTextFormatString="&lt;img border=0 src='./img/ic_play_msg.gif'&gt;">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:HyperLinkColumn>
                                                    <asp:BoundColumn DataField="Accession" HeaderText="Accession" ItemStyle-Width="60px"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MRN" HeaderText="MRN" ItemStyle-Width="60px"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="DOB" HeaderText="DOB" ItemStyle-Width="60px"></asp:BoundColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="ImpressionVoiceURL" DataNavigateUrlFormatString="{0}"
                                                        DataTextField="ImpressionVoiceURL" HeaderText="Message" DataTextFormatString="&lt;img border=0 src='./img/ic_play_msg.gif'&gt;">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:HyperLinkColumn>
                                                    <asp:BoundColumn HeaderText="Status"></asp:BoundColumn>
                                                    <asp:HyperLinkColumn Text="Mark Received" DataNavigateUrlField="MessageID" DataNavigateUrlFormatString="./mark_message_received.aspx?MessageID={0}">
                                                    </asp:HyperLinkColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="MessageReplyID" DataNavigateUrlFormatString="./mark_message_received.aspx?MessageReplyID={0}"
                                                        Text="Mark Received" Visible="False"></asp:HyperLinkColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="ReceivedByVoiceURL" DataNavigateUrlFormatString="{0}"
                                                        DataTextField="ReceivedByVoiceURL" HeaderText="Received By" DataTextFormatString="&lt;img border=0 src='./img/ic_play_msg.gif'&gt;">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:HyperLinkColumn>
                                                </Columns>
                                                <AlternatingItemStyle CssClass="AltRow" />
                                            </asp:DataGrid>
                                        </div>
                                        <br />
                                        <div id="LabTestDiv" runat="server">
                                            <table id="Table4" style="height: 23px; margin-left: 0px;" cellspacing="1" cellpadding="1"
                                                width="100%" border="0">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblLabTestResults" runat="server" CssClass="UserCenterTitle"><b>Lab Test Result</b></asp:Label>
                                                </tr>
                                            </table>
                                            <div id="TestResultDiv" class="TDiv" runat="server" style="vertical-align: top;">
                                                <asp:DataGrid CssClass="GridHeader" ID="dgTestResults" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" Width="100%" OnItemDataBound="dgTestResults_ItemDataBound">
                                                    <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="TestDescription" HeaderText="Test" ItemStyle-Width="25%">
                                                            <ItemStyle Height="21px" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TestResult" HeaderText="Result Level" ItemStyle-Width="25%">
                                                            <ItemStyle Height="21px" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="FindingDescription" HeaderText="Finding" ItemStyle-Width="25%">
                                                            <ItemStyle Height="21px" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="1st Instance" ItemStyle-Width="25%">
                                                            <ItemStyle Height="21px" HorizontalAlign="Center" Width="10%" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <AlternatingItemStyle CssClass="AltRow" />
                                                </asp:DataGrid>
                                            </div>
                                        </div>
                                        <br />
                                        <table id="Table5" cellspacing="5" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" CssClass="UserCenterTitle"><b>Activity Log</b></asp:Label></td>
                                            </tr>                                            
                                            <tr>
                                                <td align="left">
                                                    <div id="ActivityDiv" class="TDiv" style="vertical-align: top; width: 100%; height: 100%">
                                                        <asp:DataGrid ID="dgActivityLog" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            AllowSorting="True" CssClass="GridHeader" ItemStyle-Height="21px">
                                                            <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                            <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="&#160;&#160;&#160;&#160;">
                                                                    <ItemTemplate>
                                                                        <% if (strUserSettings != "YES")
                                                                           { %>
                                                                        <asp:Image ID="Image" runat="server" ImageUrl="img/ic_green_arrow.gif"></asp:Image>
                                                                        <% } %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="5%" />
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="CreatedOn_UsersTime" HeaderText="Date/Time" DataFormatString="{0:MM/dd/yyyy hh:mm:sstt} ">
                                                                    <ItemStyle Wrap="False" Width="10%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="EventDescription" HeaderText="Notification Event">
                                                                    <ItemStyle Wrap="True" Width="30%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ForUser" HeaderText="Recipient" ItemStyle-Width="10%"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="NotificationRecipient" HeaderText="Name" ItemStyle-Width="15%">
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="DeviceImage" HeaderText="Device" ItemStyle-Width="10%">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="NotificationRecipientAddress" HeaderText="Address" ItemStyle-Width="10%"
                                                                    ItemStyle-Wrap="true"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="NotificationStatusDescription" HeaderText="Status" ItemStyle-Width="10%"
                                                                    ItemStyle-Wrap="true"></asp:BoundColumn>
                                                                <asp:BoundColumn Visible="False" DataField="NotificationHistoryID" ReadOnly="True" HeaderText="NotificationHistoryID">
                                                                     </asp:BoundColumn>
                                                                 <asp:BoundColumn Visible="False" DataField="DeviceID" ReadOnly="True" HeaderText="DeviceID">
                                                                 </asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                        <table align="center" id="Table3" cellspacing="2" cellpadding="2" width="100%" border="0">
                                            <tr id="trNotification" runat="server" align="left">
                                                <td width="8%" style="white-space:nowrap;">
                                                    <asp:Label ID="Label2" runat="server" CssClass="AccountHeaderText">Resend Notification: </asp:Label>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlNotifications" runat="server" Width="350px">
                                                    </asp:DropDownList>&nbsp;
                                                    <asp:Button ID="btnResendNotification" TabIndex="30" runat="server" Width="59px"
                                                        CssClass="Frmbutton" Text="Go!" OnClick="btnResendNotification_Click" UseSubmitBehavior="False">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td width="8%" style="white-space:nowrap;">
                                                    <asp:Label ID="lblMessageForward" runat="server" CssClass="AccountHeaderText"
                                                        Width="153px">Forward to New Recipient:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnForward" runat="server" CssClass="Frmbutton" Text="Forward Message"/>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                        <table id="Table2" cellspacing="1" cellpadding="1" width="100%" style="margin-left: 0px;"
                                            border="0">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label3" runat="server" CssClass="UserCenterTitle"><b>Message Notes</b></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /></td>
                                            </tr>
                                        </table>
                                        <div id="divNotes" class="TDiv">
                                            <asp:DataGrid ID="dgMessageNotes" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ItemStyle-Height="21px" AllowSorting="True" CssClass="GridHeader">
                                                <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="&#160;&#160;&#160;&#160;">
                                                        <ItemTemplate>
                                                            <% if (strUserSettings != "YES")
                                                               { %>
                                                            <asp:Image ID="Image" runat="server" ImageUrl="img/ic_note.gif"></asp:Image>
                                                            <% } %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="3%" />
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="CreatedOn" HeaderText="Date/Time" DataFormatString="{0:MM/dd/yyyy hh:mm:sstt} ">
                                                        <ItemStyle Width="10%" Wrap="False" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Author" HeaderText="Author">
                                                        <ItemStyle Width="15%" Wrap="true" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Note" HeaderText="Note">
                                                        <ItemStyle Width="72%" Wrap="true" />
                                                    </asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid></div>
                                        </br>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="DivBg" valign="top" align="left">
                                        <div id="div1" class="TDiv" style="border: 0px; margin-left: 0px;">
                                            <table id="Table6" cellspacing="1" cellpadding="1" width="700" border="0">
                                                <tr valign="middle">
                                                    <td class="DivBg" valign="middle" align="left">
                                                        <asp:Label ID="Label4" runat="server" CssClass="AccountHeaderText" Width="70"><b>Add Note:</b></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbAuthor" TabIndex="30" runat="server" CssClass="AccountHeaderText"
                                                            MaxLength="25"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tbNote" TabIndex="31" runat="server" Width="528px" CssClass="AccountHeaderText"
                                                            TextMode="MultiLine" MaxLength="500" Rows="3"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;<asp:Button ID="btnAddNote" TabIndex="32" runat="server" CssClass="Frmbutton"
                                                            Text="Add" OnClick="btnAddNote_Click"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="DivBg" valign="middle">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lbtnRefresh" BackColor="White" OnClick="lbtnRefresh_Click"
                                            CausesValidation="true"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
