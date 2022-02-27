<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="vuiErrorReport.aspx.cs"
    Inherits="Vocada.CSTools.vuiErrorReport" Title="CSTools: VUI Errors" EnableEventValidation="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:UpdatePanel ID="upnlVUI" runat="server" UpdateMode="Conditional">
 <ContentTemplate>
    <script language="JavaScript" type="text/javascript" src="./Javascript/CalendarPopup.js"></script>

    <script language="JavaScript" type="text/javascript" src="./Javascript/calendar.js"></script>

    <script language="JavaScript" type="text/javascript">document.write(getCalendarStyles());</script>

    <script language="javascript" type="text/javascript" src="Javascript/Common.js"></script>

    <script language="JavaScript" type="text/JavaScript">
        var mapId = "vuiErrorReport.aspx";

        var calVUILogDate = new CalendarPopup("divErrorLogDate");
        CP_setControlAdjustments(00, -15);
        document.write(getCalendarStyles());
        calVUILogDate.setYearSelectStartOffset(50);
        calVUILogDate.showYearNavigation();
        calVUILogDate.showNavigationDropdowns();
        
        /* View Trace Div on mouse over of Detail column*/
        function viewTrace(imgControl, traceDesc, lastItem)
        { 
            if(traceDesc.length > 0)
            {
                var ol = imgControl.offsetParent; //offsetLeft;
                document.getElementById(divTraceClientID).style.display = '';
                document.getElementById(divTraceClientID).style.textAlign = 'left';
                document.getElementById(lblTraceClientID).innerText = traceDesc;
                document.getElementById(divTraceClientID).style.pixelRight = currentMouseXPosition - ol.offsetLeft; // AnchorPosition_getPageOffsetLeft(imgControl) + 10;
                if(lastItem == 'true')
                    document.getElementById(divTraceClientID).style.pixelTop = AnchorPosition_getPageOffsetTop(imgControl)- 120 - document.getElementById (divLogGridClientID).scrollTop;
                else
                    document.getElementById(divTraceClientID).style.pixelTop = AnchorPosition_getPageOffsetTop(imgControl)- 90 - document.getElementById (divLogGridClientID).scrollTop;
                    
                //document.getElementById(divTraceClientID).style.pixelBottom = AnchorPosition_getPageOffsetTop(imgControl) - document.getElementById (divLogGridClientID).scrollTop;
//                alert(AnchorPosition_getPageOffsetTop(imgControl)- 90 - document.getElementById (divLogGridClientID).scrollTop);
//                alert(document.getElementById (divLogGridClientID).scrollTop);
            }
        }
        
        /* Hide Trace Div on mouse out of Detail column*/
        function hideTrace()
        {
            document.getElementById(divTraceClientID).style.display = 'none';
            document.getElementById(lblTraceClientID).innerText = "";
        }
        
        /*Enable Go Button if valid input provided*/
        function EnableVUIErrorGoButton()
        {
            var errorMessage = getErrorMessage();
            if(errorMessage.length > 0)
            {
                document.getElementById(btnGOClientID).disabled = true;
            }
            else
            {
                document.getElementById(btnGOClientID).disabled = false;
            }
        }
        
        /* Validate and if error return message */
        function Validate()
        {
            var errorMessage = getErrorMessage();
            var currentDate = new Date();
            currentDate = currentDate.setHours(0,0,0,0);
            if(Date.parse(document.getElementById(txtStartDateClientID).value) > Date.parse(document.getElementById(txtEndDateClientID).value))
            {
                errorMessage += "Start date should be less than end date.\r\n";    
            }
            if(Date.parse(document.getElementById(txtStartDateClientID).value) > currentDate && Date.parse(document.getElementById(txtEndDateClientID).value) > currentDate)
            {
                errorMessage += "Start and end date should be less than current date.\r\n";    
            }
            else if(Date.parse(document.getElementById(txtStartDateClientID).value) > currentDate)
            {
                errorMessage += "Start date should be less than current date.\r\n";    
            }
            else if(Date.parse(document.getElementById(txtEndDateClientID).value)> currentDate)
            {
                errorMessage += "End date should be less than current date.\r\n";    
            }
            
            if(errorMessage.length > 0)
            {
                alert(errorMessage);
                return false;
            }
            return true;
        }
        
        /*Get error message from selection group*/
        function getErrorMessage()
        {
            var errorMsg ="";
            var isdateError = "";
            var startDateMsg = 0;
            var endDateMsg = 0;
            
            var hdnIsSystemAdmin = document.getElementById(hdnIsSystemAdminClientID).value ;
            if (hdnIsSystemAdmin == "1")
            {
                  if(document.getElementById(cmbInstitutionClientID).value == "0")
                    errorMsg = "Please select institution.\r\n"
            }
            if(document.getElementById(cmbGroupClientID).value == "0")
                errorMsg += "Please select group.\r\n"
            if(document.getElementById(cmbSubscriberClientID).value == "0")
                errorMsg += "Please select subscriber.\r\n"
                
                isdateError = ValidateDate(document.getElementById(txtStartDateClientID));
                if(isdateError.length > 0)
                 {
                    errorMsg += "Start Date: " + isdateError + "\r\n";
                    startDateMsg = 1;    
                 }
                 
                isdateError = ValidateDate(document.getElementById(txtEndDateClientID));
                if(isdateError.length > 0)
                 {
                    errorMsg += "End Date: " + isdateError + "\r\n";
                    endDateMsg = 1;
                 }
               
//               if(startDateMsg == 0  && endDateMsg == 0)
//               {
//                    if(Date.parse(document.getElementById(txtStartDateClientID).value) > Date.parse(document.getElementById(txtEndDateClientID).value))
//                        {
//                            errorMsg += "Start date should be less than end date.\r\n";    
//                        }
//               }
                return errorMsg;
               
              
        }
    </script>

    
        <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
            <tr height="100%" style="background-color: White">
            <div style="overflow-y: Auto; height: 100%">
                <td class="DivBg" valign="top">
                    <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                        <tr>
                            <td class="Hd1">
                                VUI Errors
                            </td>
                        </tr>
                    </table>
                    
                    <table align ="center" cellspacing="0" cellpadding="0" width="98%" border="0" >
                        <tr align="center">
                            <td colspan="5" >
                                <fieldset class="fieldsetCBlue" style="margin-left:0px;">
                                    <legend class="legend">Select</legend>
                                    <table border="0" width="80%">
                                        <tr>
                                            <td style="width:10%;white-space:nowrap; ">
                                                &nbsp;&nbsp;Institution Name:</td>
                                            <td style="width:25%;white-space:nowrap;">
                                                <asp:DropDownList runat="server" ID="cmbInstitution" Width="195" AutoPostBack="true" OnSelectedIndexChanged = "cmbInstitution_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                                <asp:Label ID="lblInstName" runat="server" Visible="False" ></asp:Label>
                                                <asp:HiddenField ID = "hdnIsSystemAdmin" runat="server" Value="1" /> 
                                            </td>
                                            <td style="width:5%;white-space:nowrap;" >
                                                &nbsp;&nbsp;Group:</td>
                                            <td style="width:25%;white-space:nowrap;">
                                                <asp:DropDownList runat="server" ID="cmbGroup" Width="195" AutoPostBack="true" OnSelectedIndexChanged = "cmbGroup_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width:5%;white-space:nowrap;">
                                                &nbsp;&nbsp;Subscriber:</td>
                                            <td style="width:25%;white-space:nowrap;">
                                                <asp:DropDownList runat="server" ID="cmbSubscriber" Width="195">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space:nowrap;">
                                                &nbsp;&nbsp;Start Date:</td>
                                            <td style="white-space:nowrap;">
                                                <asp:TextBox ID="txtStartDate" TabIndex="2" runat="server" Columns="30" MaxLength="75"></asp:TextBox>
                                                &nbsp;<a href="#" runat="server" name="anchFromDate" id="anchFromDate" style="height: 22px;
                                                    vertical-align: middle;"><% if (strUserSettings == "YES")
                                                                                { %>Calendar<% }
                                                                                               else
                                                                                               { %><img src="img/ic_cal.gif" width="17" height="15" border="0" /><% } %></a>
                                            </td>
                                            <td style="white-space:nowrap;">
                                                &nbsp;&nbsp;End Date:</td>
                                            <td style="white-space:nowrap;">
                                                <asp:TextBox ID="txtEndDate" TabIndex="16" runat="server" Columns="30" MaxLength="75"></asp:TextBox>
                                                &nbsp;<a href="#" runat="server" name="anchToDate" id="anchToDate" style="height: 22px;
                                                    vertical-align: middle;"><% if (strUserSettings == "YES")
                                                                                { %>Calendar<% }
                                                                                               else
                                                                                               { %><img src="img/ic_cal.gif" alt="" width="17" height="15" border="0" /><% } %></a>
                                            </td>
                                            <td style="width:20%" colspan="2">
                                                 &nbsp;&nbsp;<asp:Button Text="GO" ID="btnGO" runat="server" OnClientClick="return Validate();"
                                                  CssClass="Frmbutton" OnClick="btnGO_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                       
                        <tr>
                            <td>
                                <div id="divErrorLogDate" style="position: absolute; z-index: 100%;" class="Calander">
                                </div>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                        
                    <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                        <tr align="center">
                            <td>
                            <asp:UpdatePanel ID="upnlLogGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                  <div  id="divTrace" runat="server"  style="position: absolute; z-index: 100%; background-color:#FDF5E6;display:none;border-color:Black;border-width:1px;border-style:inset;" >
                                                                <asp:Label ID = "lblTrace" runat="server" Text ="" ></asp:Label>
                                                            </div>
                                        <fieldset class="fieldsetCBlue" style="margin-left:0px;">
                                          <legend class="legend">VUI Errors </legend>
                                        </BR>
                                        <div id="divLogGrid" runat ="Server" align="center" class="TDiv" style="width: 100%;">
                                            <asp:DataGrid runat="server" ID="grdErrorLog" CssClass="GridHeader" Width="100%"
                                                AutoGenerateColumns="False" BorderStyle="Solid" BorderColor="LightGray" AlternatingItemStyle-CssClass="Row3"
                                                OnItemDataBound="grdErrorLog_ItemDataBound" ItemStyle-Height="25" HeaderStyle-Height="25" AllowSorting="true"
                                                OnSortCommand = "grdErrorLog_SortCommand"
                                                EnableViewState="true">
                                                <HeaderStyle CssClass="THeader" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="0px">
                                                </HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="SubscriberName" HeaderStyle-HorizontalAlign ="left" HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="left" HeaderText= "Subscriber Name"  SortExpression="SubscriberName" ></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="LogDateTime" HeaderStyle-HorizontalAlign ="Left" HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="left" HeaderText= "Date/Time"  SortExpression="LogDateTime" ></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ShortLogDescription"  HeaderText= "Error Log" HeaderStyle-Width="65%" ItemStyle-Width="65%" ItemStyle-Wrap="true" ></asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText= "Details" HeaderStyle-HorizontalAlign ="Center" HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                        <asp:HyperLink id="anchDetailss"  Enabled="false" runat="server" Text ="" ></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                          
                                        </div>
                                        </fieldset>  
                                         </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnGO" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                        <td align="center">
                         <asp:Label ID ="lblNoRecordFound" runat ="server" Font-Size=  "Small" ForeColor = "Green" Visible="true" Style="position: relative; text-align:center;"></asp:Label>
                         </td>
                        </tr>
                    </table>
                </td>
            </tr>
             </div>
        </table>
   
     </ContentTemplate>
      <Triggers>
        <asp:AsyncPostBackTrigger ControlID = "cmbGroup" />
        <asp:AsyncPostBackTrigger ControlID="cmbInstitution"  />
      </Triggers>
   </asp:UpdatePanel>
</asp:Content>
