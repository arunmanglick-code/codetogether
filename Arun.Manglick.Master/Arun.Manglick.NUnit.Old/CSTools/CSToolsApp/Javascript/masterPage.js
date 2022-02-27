// JScript File
/******************************File History***************************
 * File Name        : MasterPage.js
 * Author           : Prerak Shah
 * Created Date     : 22-08-2007
 * Purpose          : Javascript used for the message center page.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 * 18-12-2007   IAK     Method added visibleNavigationButtons
 * 09-01-2008   IAK     Method added SaveUseProfileFormChanges - Use Profile Navigation alert added
 * 15-01-2009   GB      FR #152 - Clinical team assignment
 * ------------------------------------------------------------------- 
 */
function visibleNavigationButtons(value)
{
    var bValue;
    if(value == 'true')
        bValue = false;
    else
        bValue = true;
    document.getElementById(btnPreviousClientID).disabled = bValue;
    document.getElementById(btnNextClientID).disabled = bValue;
    document.getElementById(btnTopClientID).disabled = bValue;
    
    if(value = 'true')
    {
        document.getElementById(btnPreviousClientID).style.css = 'Frmbutton';
        document.getElementById(btnNextClientID).style.css = 'Frmbutton';
        document.getElementById(btnTopClientID).style.css = 'Frmbutton';
        
    }
    else
    {
        document.getElementById(btnPreviousClientID).style.css = 'FrmbuttonDisabled';
        document.getElementById(btnNextClientID).style.css = 'FrmbuttonDisabled';
        document.getElementById(btnTopClientID).style.css = 'FrmbuttonDisabled';
    
    }

} 

function DivSetVisible(state)
    {
        var DivRef = document.getElementById('dropmenu1');
        var IfrRef = document.getElementById('DivShim');
        if(state)
        {
            DivRef.style.display = "block";
            IfrRef.style.width = DivRef.offsetWidth;
            IfrRef.style.height = DivRef.offsetHeight;
            //IfrRef.style.top = DivRef.style.top;
            //IfrRef.style.left = DivRef.style.left;
            IfrRef.style.top = 90;
            IfrRef.style.left = 380;
            IfrRef.style.zIndex = DivRef.style.zIndex - 1;
            IfrRef.style.display = "block";
        }
        else
        {
            DivRef.style.display = "none";
            IfrRef.style.display = "none";
        }
    }

function DivSetVisible1(state)
    {
        var DivRef = document.getElementById('dropmenu2');
        var IfrRef = document.getElementById('DivShim1');
        if(state)
        {
            DivRef.style.display = "block";
            IfrRef.style.width = DivRef.offsetWidth;
            IfrRef.style.height = DivRef.offsetHeight;
            //IfrRef.style.top = DivRef.style.top;
            //IfrRef.style.left = DivRef.style.left;
            IfrRef.style.top = 88;
            IfrRef.style.left = 215;
            IfrRef.style.zIndex = DivRef.style.zIndex - 1;
            IfrRef.style.display = "block";
        }
        else
        {
            DivRef.style.display = "none";
            IfrRef.style.display = "none";
        }
    }
    
function DivSetVisible2(state)
    {
        var DivRef = document.getElementById('dropmenu3');
        var IfrRef = document.getElementById('DivShim2');
        if(state)
        {
            DivRef.style.display = "block";
            IfrRef.style.width = DivRef.offsetWidth;
            IfrRef.style.height = DivRef.offsetHeight;
            //IfrRef.style.top = DivRef.style.top;
            //IfrRef.style.left = DivRef.style.left;
            IfrRef.style.top = 88;
            IfrRef.style.left = 215;
            IfrRef.style.zIndex = DivRef.style.zIndex - 1;
            IfrRef.style.display = "block";
        }
        else
        {
            DivRef.style.display = "none";
            IfrRef.style.display = "none";
        }
    }
function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}



function MM_nbGroup(event, currentTab, grpName) { //v6.0
   
  var i,img,nbArr,args=MM_nbGroup.arguments;
  if (event == "init" && args.length > 3) {
    if ((img = MM_findObj(args[3])) != null && !img.MM_init) {
      img.MM_init = true; img.MM_up = args[4]; img.MM_dn = img.src;
      if ((nbArr = document[grpName]) == null) nbArr = document[grpName] = new Array();
      nbArr[nbArr.length] = img;
      for (i=5; i < args.length-1; i+=2) if ((img = MM_findObj(args[i])) != null) {
        if (!img.MM_up) img.MM_up = img.src;
        img.src = img.MM_dn = args[i+1];
        nbArr[nbArr.length] = img;
    } setTabFocus(currentTab, args[3]);}
  } else if (event == "over") {
    document.MM_nbOver = nbArr = new Array();
    for (i=2; i < args.length-1; i+=3) if ((img = MM_findObj(args[i])) != null) {
      if (!img.MM_up) img.MM_up = img.src;
      img.src = (img.MM_dn && args[i+2]) ? args[i+2] : ((args[i+1])? args[i+1] : img.MM_up);
      nbArr[nbArr.length] = img;
    }
  } else if (event == "out" ) {
    for (i=0; i < document.MM_nbOver.length; i++) {
      img = document.MM_nbOver[i]; img.src = (img.MM_dn) ? img.MM_dn : img.MM_up; } 
  } else if (event == "down") {
    nbArr = document[grpName];
    if (nbArr)
      for (i=0; i < nbArr.length; i++) { img=nbArr[i]; img.src = img.MM_up; img.MM_dn = 0; }
    document[grpName] = nbArr = new Array();
    for (i=3; i < args.length-1; i+=2) if ((img = MM_findObj(args[i])) != null) {
      if (!img.MM_up) img.MM_up = img.src;
      img.src = img.MM_dn = (args[i+1])? args[i+1] : img.MM_up;
      nbArr[nbArr.length] = img;
  } }
}

function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}

//Open Help for respective page
function openHelp()
{
    window.parent.open("help/CS_Tools/cs tools.html", "CSTools");
}

function callHeaderHelp(TabName)
{         
    if(TabName != null && TabName !="" && TabName != 'undefined')
    {
        var endPoint = TabName.indexOf("?");
        var PageName = "";
        if(endPoint != -1)
        {
            PageName = TabName.substring(0,endPoint);
        }
        else
        {
            PageName = TabName;
        }
        CallHelp(PageName);
    }
}  

//Close Help window if open
function closeHelpWindow()
{
   
    var varHelp = document.getElementById("varHelp").value;   
    if (varHelp == "false")
    {
    CallHelp("DoNotOpen");
    CloseHelp();        
    }
}



//To set the hidden field value whenever user clicks on any tab.
function setTabChangeVar()
{
    if (document.getElementById("TabClick") != null)
        document.getElementById("TabClick").value = "true";
}


//set the focus on selected tab after menu initialization.
function setTabFocus(currentTab, tabImage)
{

    if (currentTab != null && currentTab != "")
    {
        switch (currentTab)
        {
            case "MsgCenter":
                if(tabImage.search(/MsgCenter/) != -1)
                    MM_nbGroup('down', 'MsgCenter', 'group1',tabImage,'img/t_msg_center_o.gif',1);
                break;
            case "GroupMonitor":
                if(tabImage.search(/GroupMonitor/) != -1)
                    MM_nbGroup('down','Group Monitor', 'group1',tabImage,'img/t_grp_monitor_o.gif',1);
                break;
            case "Search":
                if(tabImage.search(/Search/) != -1)
                    MM_nbGroup('down','Search', 'group1',tabImage,'img/t_search_o.gif',1);
                break;
             case "Tools":
                if(tabImage.search(/Tools/) != -1)
                    MM_nbGroup('down','Tools', 'group1',tabImage,'img/t_tools_o.gif',1);
                break;    
            case "Reports":
                if(tabImage.search(/Reports/) != -1)
                MM_nbGroup('down','Reports', 'group1',tabImage,'img/t_rep_o.gif',1);
                break;
           case "SystemAdmin":
                if(tabImage.search(/SystemAdmin/) != -1)
                MM_nbGroup('down','System Admin', 'group1',tabImage,'img/t_system_admin_o.gif',1);
                break;
           case "CallCenter":
                if(tabImage.search(/CallCenter/) != -1)
                MM_nbGroup('down','Call Center', 'group1',tabImage,'img/t_call_Center_o.gif',1);
                break;
         }
    }
} 
                
var errorMessageOnLeave = "Your changes will be lost.";
//Call function to save changes while navigating from one page to other, if form content changes
function SaveChanges(currentPage, currentTab, currentInnerTab)
{   
    //CloseHelp();
    var promtMessage= "";
    
    if(currentInnerTab == null) return false;
    if(currentTab == null) return false;
    switch(currentTab)
    {
      case "MsgCenter" :
             switch(currentInnerTab)
            {
              case "EditOC":
                 promtMessage = SaveProfileFormChanges();
              break;
              case "MessageDetails":
                 promtMessage = SaveProfileMessageDetails();
              break;
              case "MessageReceived":
                 promtMessage = SaveProfileMessageReceived();
              break;
              case "MessageForward":
                 promtMessage = SaveProfileMessageForward();
              break;
              
//              case "AssignedTask":
//                 promtMessage = SaveAssignedFormChanges();
//              break;
            }
       break;
       case "GroupMonitor" :
            switch(currentInnerTab)
            {
              case "GroupMaintenance":
                 promtMessage = SaveGroupFormChanges();
              break;
              case "GroupPreferences":
                 promtMessage = SaveGroupPreferencesFormChanges();
              break;
              case "UseProfile":
                 promtMessage = SaveUseProfileFormChanges();
              break;
            }
       break;
       case "Tools" :
            switch (currentInnerTab)
            {
                case "TestResult" :
                 promtMessage = SaveTestResultFromChanges();
                break;
                case "OC Grammars" :
                 promtMessage = SaveGrammarChanges();
                break;
                case "AddNurseDirectory" :
                 promtMessage = SaveNurseDirectoryChanges();
                break;
                case "AddOCDirectory" :
                 promtMessage = SaveOCDirectoryChanges();
                break;
                case "AddGroup":
                 promtMessage = SaveGroupFormChanges();
              break;
              case "AddFindings":
                 promtMessage = SaveFindingsFormChanges();
              break;
              case "AddSubscriber" :
                 promtMessage = SaveAddSubscriberChanges();
                break;
              case "AddOC" :
                 promtMessage = SaveAddOCChanges();
                break;
              case "Custom_Notifications" :
                promtMessage = SaveCustomNotificationsChanges();
                break;
                
            }
       break;
        case "SystemAdmin" :
            switch (currentInnerTab)
            {
                case "AddInstitution" :
                 promtMessage = SaveInstitutionFromChanges();
                break;
                case "EditInstitution" :
                 promtMessage = SaveInstitutionFromChanges();
                break;
                case "RolesandTasks" :
                 promtMessage = SaveRolesFromChanges();
                break;
                case "AssignTasks" :
                 promtMessage = SaveAssignTasksFromChanges();
                break;
                case "UserManagement" :
                 promtMessage = SaveUserInformationFromChanges();
                break;
            }
       break;
       case "CallCenter" :
            switch (currentInnerTab)
            {
                case "AddCallCenter" :
                 promtMessage = SaveCallCenterFormChanges();
                break;
                case "CallCenterSetup" :
                 promtMessage = SaveCallCenterSetupChanges();
                break;
                case "AddAgent" :
                 promtMessage = SaveAddAgentChanges();
                break;
            }
        break;
    }
    
    if(promtMessage.length > 0)
        return errorMessageOnLeave;

}

// Check changes on User Profile page
function SaveAddAgentChanges()
{
      try
      {  
          if(hdnTestDataChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(hdnTestDataChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}

// Check changes on User Profile page
function SaveCallCenterSetupChanges()
{
    try
    {
        if(hdnTextChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnTextChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}


// Check changes on User Profile page
function SaveCallCenterFormChanges()
{
    try
    {
        if(hdnCallCenterDataChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnCallCenterDataChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}

// Check changes on User Profile page
function SaveUserInformationFromChanges()
{
      try
      {  
          if(hdnTestDataChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(hdnTestDataChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}

// Check changes on User Profile page
function SaveUseProfileFormChanges()
{
      try
      {  
          if(txtChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(txtChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}

// Check changes on message Forward page
function SaveProfileMessageForward()
{
      try
      {  
          if(textChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(textChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}

// Check changes on message Received page
function SaveProfileMessageReceived()
{
      try
      {  
          if(textChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(textChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}

// Check changes on message Details page
function SaveProfileMessageDetails()
{
      try
      {  
          if(textChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(textChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}


// Check changes on Add/Edit Institution page
function SaveAddSubscriberChanges()
{
      try
      {  
          if(txtChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(txtChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}

// Check changes on Add/Edit Institution page
function SaveAddOCChanges()
{
      try
      {  
          if(textChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(textChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}

// Check changes on Custom Notifications page
function SaveCustomNotificationsChanges()
{
      try
      {  
          if(hdnDataChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(hdnDataChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}

// Check changes on Add/Edit Institution page
function SaveInstitutionFromChanges()
{
      try
      {  
          if(hdnTextChangedClientID) 
          {
          }
      }
      catch(_error)
      {
        return "";
      }
       
      if(document.getElementById(hdnTextChangedClientID).value == "true")
      {
         return errorMessageOnLeave;
      }
  
  return "";
}
//Check Profile page data change
function SaveProfileFormChanges()
{
 try
    {
        if(textChangedClientID && hdnCTChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(textChangedClientID).value == "true" || document.getElementById(hdnCTChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
    return "";
}

//Check for Test Defination page data change
function SaveTestResultFromChanges()
{
    try
    {
        if(hdnTestDataChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnTestDataChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
    return "";
}

//Check whether Grammar page data has been changed or not.
function SaveGrammarChanges()
{
    try
    {
        if(hdnGrammarDataChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnGrammarDataChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}

//Check whether Grammar page data has been changed or not.
function SaveNurseDirectoryChanges()
{
    try
    {
        if(hdnNurseDirectoryDataChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnNurseDirectoryDataChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}

//Check whether Grammar page data has been changed or not.
function SaveOCDirectoryChanges()
{
    try
    {
        if(hdnOCDirectoryDataChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnOCDirectoryDataChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}
//Check whether ManageRoles&Tasks page data has been changed or not.
function SaveRolesFromChanges()
{
    try
    {
        if(hdnRolesDataChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnRolesDataChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}
//Check whether AssignTasks page data has been changed or not.
function SaveAssignTasksFromChanges()
{
    try
    {
        if(hdnDataChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnDataChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}
//Check whether AssignTasks page data has been changed or not.
function SaveGroupFormChanges()
{
    
    try
    {
        if(hdnTextChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnTextChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}
//Check whether Findings page data has been changed or not.
function SaveFindingsFormChanges()
{
    
    try
    {
        if(hdnTextChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnTextChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}
//Check whether Findings page data has been changed or not.
function SaveGroupPreferencesFormChanges()
{
    
    try
    {
        if(hdnTextChangedClientID){}
    }
    catch(_error)
    {
     return "";
    }
    if(document.getElementById(hdnTextChangedClientID).value == "true")
    {
        return errorMessageOnLeave;
    }
   
    return "";
}


                    var dayarray=new Array("Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday")
                    var montharray=new Array("January","February","March","April","May","June","July","August","September","October","November","December")
                    function getthedate(){
                    var mydate=new Date()
                    var year=mydate.getYear()
                    if (year < 1000)
                    year+=1900
                    var day=mydate.getDay()
                    var month=mydate.getMonth()
                    var daym=mydate.getDate()
                    if (daym<10)
                    daym="0"+daym
                    var hours=mydate.getHours()
                    var minutes=mydate.getMinutes()
                    var seconds=mydate.getSeconds()
                    var dn="AM"
                    if (hours>=12)
                    dn="PM"
                    if (hours>12){
                    hours=hours-12
                    }
                    {
                     d = new Date();
                     Time24H = new Date();
                     Time24H.setTime(d.getTime() + (d.getTimezoneOffset()*60000) + 3600000);
                     InternetTime = Math.round((Time24H.getHours()*60+Time24H.getMinutes()) / 1.44);
                     if (InternetTime < 10) InternetTime = '00'+InternetTime;
                     else if (InternetTime < 100) InternetTime = '0'+InternetTime;
                    }
                    if (hours==0)
                    hours=12
                    if (minutes<=9)
                    minutes="0"+minutes
                    if (seconds<=9)
                    seconds="0"+seconds
                    //change font size here
                    var cdate=dayarray[day]+", "+montharray[month]+" "+daym+", "+year+" - "+hours+":"+minutes+":"+seconds+" "+dn
                    if (document.all)
                    document.all.clock.innerHTML=cdate
                    else if (document.getElementById)
                    document.getElementById("clock").innerHTML=cdate
                    else
                    document.write(cdate)
                    }
                    if (!document.all&&!document.getElementById)
                    getthedate()
                    function goforit(){
                    if (document.all||document.getElementById)
                    setInterval("getthedate()",1000)
                    }
                    window.onload=goforit
              

   // Navigate Finding
function NavigateFinding(page,id)
    {
        try
        {
            if (page == "Notifications") 
              window.location.href = "group_maintenance_findings.aspx?GroupID=" + id;
            else
             window.location.href = "group_monitor.aspx?InstitutionID=" + id;
        }
        catch(_error)
        {
            return;
        }
    }
