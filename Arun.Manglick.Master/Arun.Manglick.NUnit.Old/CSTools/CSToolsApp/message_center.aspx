<%@ Page Language="c#" CodeFile="message_center.aspx.cs" MasterPageFile="~/cs_tool.master"
    AutoEventWireup="false" Inherits="Vocada.CSTools.message_center" Title="CSTools: Message Center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="JavaScript" type="text/javascript" src="./Javascript/CalendarPopup.js"></script>

    <script language="JavaScript" type="text/javascript" src="./Javascript/calendar.js"></script>

    <script type="text/javascript" language="javascript" src="./Javascript/Common.js"></script>

    <script language="JavaScript" type="text/javascript">
    
    //Start("MessageList");    
var Result;
var mapId = "message_center.aspx";
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
   var messageID = temp[2]; 
   var deptMsg = temp[3]; 
   var groupId = temp[4]; 
   var objArgument = new Object();					
   objArgument.ParentObject = window;
   objArgument.URL = "message_readback_confirmation.aspx?ReadBackID=" + readbackID + "&VoiceURL=" + url + "&MessageID=" + messageID + "&IsDeptMsg=" + deptMsg + "&GroupID=" + groupId;		
   winProper = 'dialogHeight:143px;dialogLeft:320px;dialogTop:320px;dialogWidth:250px;center:yes;dialogHide:no;edge:sunken;resizable:yes;scroll:auto;status:no;unadorned:yes;help:no;title=0';
   var result = window.showModalDialog('readback_popup.aspx',objArgument,winProper);			
   window.parent.location.href = 'message_center.aspx';
   objArgument = null;   
}
function OpenAdvanceSearch(groupID, isLabGroup)
{
   //debugger
     var objArgument = new Object();
                  
     var curDateTime = new Date();
    
    objArgument.ParentObject = window;					
    if(groupID != null)
        objArgument.URL = "advance_search.aspx?groupID=" + groupID + "&isLabGroup=" + isLabGroup + "&CurDateTime="+curDateTime;						
    else
        objArgument.URL = "advance_search.aspx?&CurDateTime=" + curDateTime;											
	{				
		//winProper = 'dialogHeight:275px;dialogLeft:100px;dialogTop:120px;dialogWidth:800px;center:yes;dialogHide:no;edge:sunken;resizable:no;scroll:no;status:no;unadorned:no;help:no'	;    	    
		winProper = 'dialogHeight:400px;dialogWidth:800px;center:yes;dialogHide:no;edge:sunken;resizable:no;scroll:no;status:no;unadorned:no;help:no'	;    	    
	}				
	window.showModalDialog('VCPopUP.aspx',objArgument,winProper);
	var retrunVal = PageLoad('2');
	if(retrunVal)
	   __doPostBack(btnGoClientID ,'load'); 
}
StartMsgList("message_center.aspx");
    var MsgListTimerID = 0;
    var MsgListMsgListTStart  = null;
    //calls the Showpopup function to show the readback confirmation dialog.
    function UpdateTimerMsgList(sURL) {
       if(MsgListTimerID) {
          clearTimeout(MsgListTimerID);
          clockID  = 0;
       }

       if(!MsgListTStart)
          MsgListTStart   = new Date();

       var   tDate = new Date();
       var   tDiff = tDate.getTime() - MsgListTStart.getTime();

       tDate.setTime(tDiff);
       MsgListTimerID = setTimeout("UpdateTimerMsgList()", 180000);
       __doPostBack(sURL, "load");
       StopMsgList();
       StartMsgList("message_center.aspx");
    }

    //timer start function. sets the timeout as 60 sec.
    function StartMsgList(sURL) {
      MsgListTStart   = new Date();
      MsgListTimerID  = setTimeout("UpdateTimerMsgList('" + sURL + "')", 180000);
       
    }

    //Stops the timer
    function StopMsgList() 
    {
       if(MsgListTimerID) {
          clearTimeout(MsgListTimerID);
          MsgListTimerID  = 0;
       }

       MsgListTStart = null;
    }
    
    </script>

 <asp:UpdatePanel runat="server" ID="parentPanel">
        <ContentTemplate>    
            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%; background-color: White">
                            <table height="100%" align="center" valign="top" width="100%" border="0" cellpadding="0"
                                cellspacing="0">
                                <tr valign="top">
                                    <td valign="top">
                                        <table height="25px" width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                                            <tr valign="middle" style="height: 25px">
                                                <td width="40%" class="Hd1" valign="middle" style="height: 18px">
                                                    <asp:Label ID="labelOutgoing" runat="server" Height="20px"></asp:Label></td>
                                                <td class="Hd1" valign="middle" align="right" style="height: 18px;" nowrap>
                                                    <div id="divListMessage" runat="server"> 
                                                               <asp:DropDownList ID="ddlistShowMessages" runat="server" Width="80">
                                                                    <asp:ListItem Value="0" Selected="true">Radiology</asp:ListItem>
                                                                    <asp:ListItem Value="1">Lab</asp:ListItem>
                                                                </asp:DropDownList>&nbsp;                                                                                                                    
                                                        <asp:DropDownList ID="cmbMsgStatus" runat="server" Width="170">
                                                            <asp:ListItem Text="Open" Selected="true" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Closed" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Documented Message" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Embargoed" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Support Monitoring Report" Value="7"></asp:ListItem>
                                                        </asp:DropDownList>&nbsp;
                                                        <asp:Button ID="btnGo" UseSubmitBehavior="true" runat="server" OnClick="btnGo_Click" Text=" GO " CssClass="Frmbutton" />&nbsp;&nbsp;&nbsp;
                                                     </div>                                                    
                                                </td>
                                            </tr>
                                           </table>
                                            <table height="25px" width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                                            <tr valign="middle">
                                                <td class="Hd1" valign="middle" style="height: 18px; width:100%" >
                                                   <!-- <asp:Label ID="lblSearch" Text="OC=Stiphan Hokings RC=Sagar MRN=455543545 Units=ICU Finding=Red Accession=0347890 FromDate=12/1/2007 ToDate=12/24/2007" runat="Server"></asp:Label> -->
                                                    <img runat="server" ID="hlinkSearch" onclick="OpenAdvanceSearch()" src="img/ic_adv_srch.gif" width="124" height="22" alt="Advanced Search" style="cursor:hand;vertical-align:middle; " />
                                                    &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lbtnClear" runat="server" Text="Clear Search" style="cursor:hand; vertical-align:middle;" OnClick="lbtnClear_Click"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            </table>
                                    </td>
                                </tr>
                                <tr height="100%;" align="center">
                                            <td class="DivBg" valign="top" align="Center" >
                                                <!-- START grid for outgoing calls -->
                                                <asp:UpdatePanel ID = "upnlGridData" UpdateMode="conditional" runat="server"> 
                                                <ContentTemplate>
                                                <div id="MessageDiv" class="MessageTDiv" style="margin-left:0px;margin-right:0px; width:99%;">
                                                 
                                                    <asp:DataGrid ID="dgOutstandingCalls" OnSortCommand="dgOutstandingCalls_SortCommand" OnItemCreated = "dgOutstandingCalls_ItemCreated"
                                                        runat="server" CssClass="GridHeader" Width="100%" Height="100%" UseAccessibleHeader="True" EnableViewState="false" 
                                                        AutoGenerateColumns="False" AllowSorting="True" BorderStyle="Solid" BorderColor="LightGray">
                                                        <HeaderStyle CssClass="THeader" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                        </HeaderStyle>                                                        
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="&#160;&#160;&#160;&#160;" ItemStyle-Width="2%">
                                                                <ItemTemplate>
                                                                    Image
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn Visible="False" DataField="MessageID" ReadOnly="True" HeaderText="MessageID">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn Visible="False" ReadOnly="True" HeaderText="SpecialistID">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="GroupName" HeaderText="Group" ItemStyle-Width="7%" SortExpression="GroupName">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SpecialistDisplayName" SortExpression="SpecialistDisplayName"
                                                                ItemStyle-Width="8%" HeaderText="Created By"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CreatedOn_UsersTime" SortExpression="CreatedOn_UsersTime"
                                                                HeaderText="Created On" DataFormatString="{0:MM/dd/yyyy hh:mmtt} " ItemStyle-Width="11%">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="RecipientDisplayName" SortExpression="RecipientDisplayName"
                                                                HeaderText="Recipients <br> #PassCode" ItemStyle-Width="15%"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="BedNumber" SortExpression="BedNumber" HeaderText="Bed"
                                                                ItemStyle-Width="7%"></asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Patient </br> MRN/DOB" ItemStyle-Width="150px">
                                                                <ItemTemplate>
                                                                    Patient Name
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                            </asp:TemplateColumn>    
                                                            <asp:BoundColumn DataField="FindingDescription" SortExpression="FindingDescription"
                                                                ItemStyle-Width="5%" HeaderText="Finding"></asp:BoundColumn>
                                                            <asp:HyperLinkColumn DataNavigateUrlField="ImpressionVoiceURL" DataNavigateUrlFormatString="{0}"
                                                                ItemStyle-Width="4%" DataTextField="ImpressionVoiceURL" HeaderText="MSG" DataTextFormatString="&lt;img border=0 src='./img/ic_play_msg.gif'&gt;">
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:HyperLinkColumn>
                                                            <asp:BoundColumn HeaderText="Status" DataField="MessageStatus" SortExpression="MessageStatusDateTime" ItemStyle-Width="8%"></asp:BoundColumn>
                                                            <asp:HyperLinkColumn DataNavigateUrlField="MessageID" DataNavigateUrlFormatString="./message_details.aspx?MessageID={0}"
                                                                ItemStyle-Width="3%" DataTextField="PatientVoiceURL" HeaderText="Details" DataTextFormatString="&lt;img border=0 src='./img/ic_details.gif'&gt;">
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:HyperLinkColumn>
                                                            <asp:BoundColumn DataField="Note" HeaderText="Note" DataFormatString="{0:MM/dd/yyyy hh:mmtt}"
                                                                ItemStyle-Width="7%" SortExpression="Note"></asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="Stage" SortExpression="Stage" ItemStyle-Width="7%"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Failures" HeaderText="Failures" SortExpression="Failures"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Starts" HeaderText="Starts" SortExpression="Starts"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="False" DataField="BackupStartedAt" HeaderText="BackUp Started On"
                                                                DataFormatString="{0:MM/dd/yyyy hh:mmtt} " ItemStyle-Width="10%">
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                            Font-Underline="False" HorizontalAlign="Left"  />
                                                    </asp:DataGrid> 
                                                                                                    
                                                    </div> 
                                                    <br />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID ="dgOutstandingCalls" />
                                                    </Triggers>
                                                    </asp:UpdatePanel>
                                                    <div id="divNoMessageWarningLabel" runat="server">
                                                    <asp:Label ForeColor="Green" Font-Size="Small" ID="lblNoRecords" runat="server" Text="No messages are available"></asp:Label>
                                                    <br />
                                                    <br />
                                                    </div>
                                                    <asp:Button ID="btnPrevious" runat="server" Text="Previous Week" CssClass="Frmbutton" OnClick="btnPrevious_Click" Width="82" />&nbsp;
                                                    <asp:Button ID="btnNext" runat="server" Text="Next Week" CssClass="Frmbutton" OnClick="btnNext_Click" Width="82" />&nbsp;
                                                    <asp:Button ID="btnTop" runat="server" Text="Top" CssClass="Frmbutton" OnClick="btnTop_Click" Width="82" />
                                                                
                                               </td>
                                        </tr>
                               </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
