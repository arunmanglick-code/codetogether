<%@ Page Language="C#" AutoEventWireup="true" CodeFile="advance_search.aspx.cs" Inherits="Vocada.CSTools.advance_search" EnableEventValidation="false" %>
<html>
<head id="Head1" runat="server">
<link href="App_Themes/cstool/cstool.css" rel="stylesheet" type="text/css" />
<script language="JavaScript" type="text/javascript" src="Javascript/CalendarPopup.js"></script>
<script language="JavaScript" type="text/javascript" src="Javascript/calendar.js"></script>
<script language="JavaScript" type="text/javascript" src="Javascript/common.js"></script>
<script language="JavaScript" type="text/javascript">document.write(getCalendarStyles());</script> 

<title>CSTools: Advance Search</title>
</head>
<body  style="background-color:White;" class="bodyIframe">
    <form id="frmAdvSearch" runat="server" method="post" >
     <asp:ScriptManager ID="atlS1" EnablePartialRendering="true" runat="server" ScriptMode="release">
        </asp:ScriptManager>
         <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
         <ContentTemplate>
         <script language="javascript" type="text/javascript">

    document.onclick = function(){	
                                try
                                {
			                        DivSetVisibleOnPage(false, 'iframeCalFromDate', 'ClaDiv1');
			                        DivSetVisibleOnPage(false, 'iframeCalToDate', 'ClaDiv2');
			                        DivSetVisibleOnPage(false, 'iframeCalSearchDOB', 'DivSearchDOB');
			                    }
			                    catch(e)
			                    {}
                             }
                             
    function DivSetVisibleOnPage(state, iframename, divname)
    {
    var DivRef = document.getElementById(divname);
    var IfrRef = document.getElementById(iframename);
    if(state)
    {
    DivRef.style.display = "block";
    IfrRef.style.width = DivRef.offsetWidth;
    IfrRef.style.height = DivRef.offsetHeight;
    IfrRef.style.top = DivRef.style.top;
    IfrRef.style.left = DivRef.style.left;
    IfrRef.style.zIndex = DivRef.style.zIndex - 1;

    IfrRef.style.display = "block";
    }
    else
    {
    IfrRef.style.display = "none";
    }
    }

//Displays calender on desired position 
  var cal1xx = new CalendarPopup("ClaDiv1");
  CP_setControlAdjustments(0,80);
  document.write(getCalendarStyles()); 
  cal1xx.setYearSelectStartOffset(50);
  cal1xx.showYearNavigation();
  cal1xx.showNavigationDropdowns();
  
  var cal2xx = new CalendarPopup("ClaDiv2"); 
  CP_setControlAdjustments(0,80);
  document.write(getCalendarStyles());
  cal2xx.setYearSelectStartOffset(50);
  cal2xx.showYearNavigation();
  cal2xx.showNavigationDropdowns();
  
  var cal3xx = new CalendarPopup("DivSearchDOB"); 
  CP_setControlAdjustments(100,80);
  document.write(getCalendarStyles());
  cal3xx.setYearSelectStartOffset(50);
  cal3xx.showYearNavigation();
  cal3xx.showNavigationDropdowns();
  
  // Validates whether the entered From and To date values are valid or not. 
    function validation()
    {	
        var errorMessage1 = "";
        var focusOn = "";  
         var today = new Date;
        var group = document.getElementById(cmbGroupClientID);
        if (group.value == "0")
        {
            errorMessage1 = "- Please select Group \n" ;
            focusOn = cmbGroupClientID;
        }
        var dtDOB =  document.getElementById(txtDOBClientID).value;
	    if(dtDOB.length > 0)
	    {
	        var err2 = isValidDate(dtDOB);
	        if (err2.length > 0)
	        {
	       	    errorMessage1 += "- Please enter valid DOB \n" ;
                if (focusOn == "")
                  focusOn = txtDOBClientID;
            }
            else
            {
                var dob1 = new Date(dtDOB);
                if (dob1 > today)
                {
                   errorMessage1 += "- DOB must be less then today's date \n" ;
                    if (focusOn == "")
                      focusOn = txtDOBClientID; 
                }
            }
        }	
        var dtFrom = document.getElementById(txtFromDateClientID).value;
        if (dtFrom.length <=0)
        {
            errorMessage1 += "- Please enter From Date \n" ;
            
            if (focusOn == "")
              focusOn = txtFromDateClientID;
        }
        else 
	    {    
	        var err1 = isValidDate(dtFrom);
	        if (err1.length > 0)
	        {
		        errorMessage1 += "- Please enter valid From Date \n" ;
                if (focusOn == "")
                  focusOn = txtFromDateClientID;
            } 
            else
            {
                var dtFrom1 = new Date(dtFrom);
	            if (dtFrom1 > today)
	            {
		            errorMessage1 += "- From Date must be less then today's date \n" ;
                    if (focusOn == "")
                      focusOn = txtFromDateClientID;
                } 
            }
	    }
	    var dtTo =  document.getElementById(txtToDateClientID).value;
	    if (dtTo.length <=0)
        {
            errorMessage1 += "- Please enter To Date \n" ;
            
            if (focusOn == "")
              focusOn = txtToDateClientID;
        }
        else 
	    {   
	        var err = isValidDate(dtTo);
	        if  (err.length > 0)
	        {
		        errorMessage1 += "- Please enter valid To Date \n" ;
                if (focusOn == "")
                  focusOn = txtToDateClientID;
            }
            else 
            {
                var dtTo1 = new Date(dtTo);
	            if (dtTo1 > today)
	            {
		            errorMessage1 += "- To Date must be less then today's date \n" ;
                    if (focusOn == "")
                      focusOn = txtToDateClientID;
                } 
            }
	    }
	    if (dtFrom.length > 0 && dtTo.length > 0)
	    {
	        var dtfromDate = new Date( document.getElementById(txtFromDateClientID).value);
	        var dttoDate = new Date( document.getElementById(txtToDateClientID).value);
	        if (dtfromDate > dttoDate)
	        {
		        errorMessage1 += "- From Date should be less than or equal to To Date.";
		         if (focusOn == "")
                    focusOn = txtFromDateClientID;
	        }
	    }
	    if (errorMessage1 != "")
	    {
	        alert(errorMessage1);
	        document.getElementById(focusOn).focus();
	        return false;
	    }
	    selectSearch();
        return true;			
    }
    function ControlVisibility()
    {
      if (document.getElementById(cmbGroupClientID) != null && (document.getElementById(cmbGroupClientID).value != "0" && document.getElementById(ddlistShowMessagesClientID).value != "0"))
      {
           if(document.getElementById(cmbOCClientID).value != "-1")
           {
             document.getElementById(cmbNurseClientID).disabled=true;
             document.getElementById(cmbUnitsClientID).disabled=true;
           }
           else if (document.getElementById(cmbNurseClientID).value != "-1")
           {
             document.getElementById(cmbOCClientID).disabled=true;
             document.getElementById(cmbUnitsClientID).disabled=true;
           }
           else if (document.getElementById(cmbUnitsClientID).value != "-1")
           {
             document.getElementById(cmbOCClientID).disabled=true;
             document.getElementById(cmbNurseClientID).disabled=true;
           }
           else
           {
             document.getElementById(cmbOCClientID).disabled=false;
             document.getElementById(cmbNurseClientID).disabled=false;
             document.getElementById(cmbUnitsClientID).disabled=false;
           }
       }
    }
    function ControlsSetting()
    {
       if (document.getElementById(cmbGroupClientID).value != "0")
       {
            if (document.getElementById(ddlistShowMessagesClientID).value == "0")
            {
                document.getElementById(cmbNurseClientID).disabled=true;//.style.visibility = "hidden";
                document.getElementById(cmbUnitsClientID).disabled=true;
                document.getElementById(lblNurseClientID).disabled=true;
                document.getElementById(lblUnitClientID).disabled=true;
            }
            else
            {
                 document.getElementById(cmbNurseClientID).disabled=false;//.style.visibility = "visible";
                document.getElementById(cmbUnitsClientID).disabled=false;
                document.getElementById(lblNurseClientID).disabled=false;
                document.getElementById(lblUnitClientID).disabled=false;
            }
       }
    }
    function doHourGlass()
    {
       document.body.style.cursor = 'wait'; 

    }
    
    function selectSearch()
    {
        __doPostBack(btnSelectClientID ,'btnSelect_Click'); 
        //__doPostBack(document.getElementById(btnSelectClientID) ,'btnSelect_Click'); 
        setTimeout('window.close()', 5000);
        //window.close();
    }
</script>
        <table width="100% height:95%;"  align="center" style="margin-left: 0px; height:100%;background-color:White; margin-bottom : 100%" border="0"
            cellpadding="0" cellspacing="0">
            <tr>
                <td style="vertical-align:top;" >
                   
                    <fieldset class="fieldsetBlue" style=";width:98%; ">
                        <legend class="">Advanced Search</legend><br />
                        <fieldset class="innerFieldset" style="width:96%;">
                            <legend class=""><b>Select</b></legend>
                             <table width="60%" align="center" border="0" cellpadding="2" cellspacing="1" class="input">
                                <tr>
                                 <td>
                                    <asp:DropDownList ID="ddlistShowMessages" AutoPostBack="true" runat="server" Width="80" OnSelectedIndexChanged="ddlistShowMessages_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Selected="true">Radiology</asp:ListItem>
                                        <asp:ListItem Value="1">Lab</asp:ListItem>
                                       <%-- <asp:ListItem Value="2">Both</asp:ListItem> --%>
                                    </asp:DropDownList>                                                                                                                    
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="upnlGroup" runat="server" UpdateMode="Conditional">
                                   <ContentTemplate>
                                    <asp:DropDownList ID="cmbGroup" runat="server" DataValueField="GroupID" DataTextField="GroupName"
                                       Width="200" OnSelectedIndexChanged="cmbGroup_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers >
                                        <asp:AsyncPostBackTrigger ControlID="ddlistShowMessages" />
                                    </Triggers>
                                  </asp:UpdatePanel>   
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbMsgStatus" runat="server" Width="150" >
                                        <asp:ListItem Text="Open" Selected="true" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Closed" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Documented Message" Value="2"></asp:ListItem>
                                        <%--<asp:ListItem Text="Embargoed" Value="3"></asp:ListItem>--%> 
                                        <asp:ListItem Text="All" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                               </tr>
                             </table> 
                        </fieldset>    <br /> <br />
                         <asp:UpdatePanel ID="upnlControls" runat="server" UpdateMode="Conditional">
                        <ContentTemplate> 
                         <fieldset class="innerFieldset" style="width:96%;" >
                            <legend class=""><b>Search Criteria</b></legend> 
                        <table width="98%" align="center" border="0" cellpadding="2" cellspacing="1" class="input">
                            <tr>
                                <td style="width:15%"></td>
                                <td style="width:35%"></td>
                                <td style="width:15%"></td>
                                <td ></td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap">Ordering Clinician:</td>
                                <td>
                                    <asp:DropDownList ID="cmbOC" runat="server" Width="200px" DataTextField="OCName" DataValueField="ReferringPhysicianID"  >
                                    </asp:DropDownList>
                                </td>
                                <td style="white-space: nowrap">Reporting Clinician:</td>
                                <td>
                                    <asp:DropDownList ID="cmbRC" runat="server" Width="200px" >
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap"><asp:Label ID="lblNurse" Text="Nurse:" runat="server"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="cmbNurse" runat="server" Width="200" DataTextField="NurseName" DataValueField="NurseID">
                                    </asp:DropDownList>
                                </td>
                                <td style="white-space: nowrap"><asp:Label ID="lblUnit" Text="Units:" runat="server"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="cmbUnits" runat="server" Width="200px" DataTextField="UnitName" DataValueField="UnitID">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>MRN:</td>
                                <td>
                                    <asp:TextBox ID="txtMRN" runat="server" Width="150px" > </asp:TextBox></td>
                                <td>Finding:</td>
                                <td>
                                    <asp:DropDownList ID="cmbFinding" runat="server" Width="150px" DataTextField="FindingDescription" DataValueField="FindingID">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>DOB:</td>
                                <td align="left"><table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <asp:TextBox ID="txtDOB" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                                <a href="#" runat="server" name="anchSearchDOB" id="anchSearchDOB" style="height: 22px;vertical-align: middle;"><img src="img/ic_cal.gif" alt="" width="17" height="15" border="0" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 196px">
                                                <iframe id="iframeCalSearchDOB" src="blankHTMLPage.htm" scrolling="no" frameborder="0" style="position: absolute;
                                                    top: 0px; left: 0px; display: none;"></iframe>
                                                <div id="DivSearchDOB" style="position: absolute; z-index: 1000;" class="Calander">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>Accession:</td>
                                <td>
                                    <asp:TextBox ID="txtAccession" runat="server" Width="150px" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>From:</td>
                                <td ><table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr style="height: 23px" align="left">
                                            <td align="left" valign="middle">
                                                <asp:TextBox ID="txtFromDate" runat="server" Width="100px"></asp:TextBox>
                                                <a href="#" runat="server" name="anchFromDate" id="anchFromDate" style="height: 22px; vertical-align: middle;"><img src="img/ic_cal.gif" width="17" height="15" border="0" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <iframe id="iframeCalFromDate" src="blankHTMLPage.htm" scrolling="no" frameborder="0" style="position: absolute;
                                                    top: 0px; left: 0px; display: none;"></iframe>
                                                <div id="ClaDiv1" style="position: absolute; z-index: 1000;" class="Calander">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td >To:</td>
                                <td style="height: 23px">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr style="height: 23px" align="left">
                                            <td align="left" valign="middle">
                                                <asp:TextBox ID="txtToDate" runat="server" Width="100px" ></asp:TextBox>
                                                <a href="#" runat="server" name="anchToDate" id="anchToDate" style="height: 22px;vertical-align: middle;"><img src="img/ic_cal.gif" alt="" width="17" height="15" border="0" /></a>
                                             </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <iframe id="iframeCalToDate" src="blankHTMLPage.htm" scrolling="no" frameborder="0" style="position: absolute;
                                                    top: 0px; left: 0px; display: none;"></iframe>
                                                <div id="ClaDiv2" style="position: absolute; z-index: 1000;" class="Calander">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        
                    </fieldset>
                    <br /><br />
                    <div align="center">
                        <asp:Button CssClass="Frmbutton" ID="btnSelect" Text=" Go " UseSubmitBehavior="true"
                            OnClientClick="return validation();" runat="server" />&nbsp;
                        <asp:Button  ID="butClose" CssClass="Frmbutton" Text="Close" runat="server" UseSubmitBehavior="false"
                                     OnClientClick = "Javascript:window.close();" />
                   </div><br />
                   </ContentTemplate> 
                            <Triggers >
                               <asp:AsyncPostBackTrigger ControlID="cmbGroup" />
                              
                            </Triggers>
                        </asp:UpdatePanel> 
                   </fieldset>
                  
                </td>
            </tr>
        </table>
        </ContentTemplate> 
          </asp:UpdatePanel> 
 </form>
</body>
</html>
