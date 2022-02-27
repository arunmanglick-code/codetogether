// JScript File
/******************************File History***************************
 * File Name        : finding.js
 * Author           : Indrajeet K
 * Created Date     : 22-July-2007
 * Purpose          : This Java script file contains functions for add, edit findings.
 *                  : 
 *                  :

 * *********************File Modification History*********************
 * Date(dd-mm-yyyy) Developer Reason of Modification
 *  22-07-2008 IAK    - Created, moved the JS code from Institution.js and add_findings.aspx code in this file   
 *  12-12-2008 RG       Fixed #3177
 *  12-19-2008 GB     - Added functions enableDefault, enableConnectLive for fields Default and Connect Live as per TTP #244 and #231.                                                                                      
 * ------------------------------------------------------------------- */
 
    var otherPostback =false;
    var mapId = "add_findings.aspx";
    var isFocusOnButton  = false;
    var isFocusOnSaveButton  = false;
    
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
    // On Institution chnage check whether form data has been changed, if yes the ask for confirmation.
    function onComboChange()
    {
        if(confirmOnDataChange())
        {
            __doPostBack('ctl00$ContentPlaceHolder1$cmbInstitution','');
            document.getElementById(hdnInstitutionValClientID).value = document.getElementById(cmbInstitutionClientID).value ;
            return true;
        }
        else
        {
            document.getElementById(cmbInstitutionClientID).value = document.getElementById(hdnInstitutionValClientID).value;
            return false;
        }
    }
    function enableEmbargo(flg)
    {
        var embargo = document.getElementById(chkEmbargoClientID);
        var embargoweekend = document.getElementById(chkEmbWeekendClientID);
        var txtembstart = document.getElementById(txtEmbargoStartClientID);
        var txtembend = document.getElementById(txtEmbargoEndClientID);
        
        
        if (embargo.checked)
        {
            embargoweekend.disabled=false;
            txtembstart.disabled=false;
            txtembend.disabled=false;
        }
        else
        {
            embargoweekend.disabled=true;
            txtembstart.disabled=true;
            txtembend.disabled=true;
            txtembstart.value = "";
            txtembend.value = "";
            embargoweekend.checked = false;
        }
        if (flg == "true")
            document.getElementById(hdnTextChangedClientID).value = "true";
        else
            document.getElementById(hdnTextChangedClientID).value = "false";
    }
    
    function enableDefault()
    {     
        var chkActive = document.getElementById(chkActiveClientID);
        var chkDefault = document.getElementById(chkDefaultClientID);        
        var cmbInst = document.getElementById(cmbInstitutionClientID);        
        if (chkActive.checked && cmbInst.value != -1)
        {
            chkDefault.disabled=false;
        }
        else
        {
            chkDefault.checked = false;
            chkDefault.disabled = true;
        }
    }
    
    function enableConnectLive()
    {
        var documentonly = document.getElementById(chkDocumentedOnlyClientID);
        var connectlive = document.getElementById(rbConnectLiveClientID);
        var rbnoneid=connectlive.id+"_0";
        var rbnone = document.getElementById(rbnoneid);
        
        if (documentonly.checked)
        {
            rbnone.checked = true;
            connectlive.disabled=true;
        }
        else
        {
            connectlive.disabled=false;
        }
    }
    
    function deactiveEnterAction()
    {
        if (window.event.keyCode == 13 && isFocusOnButton == false) 
        {
                event.returnValue=false; 
                event.cancel = true;
        }
        else if(window.event.keyCode == 13 && isFocusOnButton && !isFocusOnSaveButton)
        {window.event.keyCode == 32; 
                //__doPostBack("btnCancel", "");
                event.returnValue=true; 
                //event.cancel = true;
        }
    }
    
    function focusOnButton(value, saveButton)
    {
        isFocusOnButton = value;
        isFocusOnSaveButton = saveButton;
    }
   
    function hideControl(isSysAdmin)
    {
        document.getElementById(cmbGroupClientID).style.visibility = "hidden";
        document.getElementById(cmbGroupClientID).style.display = "none";
        
        if (isSysAdmin == "True")
        {
          document.getElementById(cmbInstitutionClientID).style.visibility = "hidden";
          document.getElementById(cmbInstitutionClientID).style.display = "none";
        }
    }
  
    
   // Java scripts for Add/Edit Findings page
    
     function FindingCheckRequired(addfinding)
     {  
        
        var errorMessage = "";
        var focusOn = "";  
        var regx = /[a-zA-Z\s\.\'\-]/;    
        var group = document.getElementById(cmbGroupClientID);
              
        var fOrder = '' ;
        var fName = '';
        var fPriority = '';
        var allgrp =  document.getElementById(txtGroupCheckClientID).value;
        var groupValue;
        if (allgrp.length > 0 )
        {
            if(group != null)
            {
                if (allgrp.indexOf('#' + group.value + '#') != -1)
                {
                    groupValue = allgrp.substring(allgrp.indexOf('#' + group.value + '#'), allgrp.lastIndexOf('#' + group.value + '#'));
                    
                    fOrder = groupValue.substring(groupValue.indexOf('*'), groupValue.lastIndexOf('*') + 1);
                    fName = groupValue.substring(groupValue.indexOf('$'), groupValue.lastIndexOf('$') + 1);
                    fPriority = groupValue.substring(groupValue.indexOf('%'), groupValue.lastIndexOf('%') + 1);
                }
            }
        }

        if(group != null && group.value == "-1") 
        {
           errorMessage = " - Please select Group. \n";
           focusOn=cmbGroupClientID;
        }
        var desc = document.getElementById(txtDescriptionClientID).value;
        if(desc.length <= 0)
        {
           errorMessage = errorMessage + " - Please enter Findings Description. \n";
            if (focusOn == "")
            focusOn=txtDescriptionClientID;
        }
        else
        {
            if (regx.test(desc)==false || desc.trim().length <= 0 )
              { 
                errorMessage = errorMessage + " - Please enter valid Findings Description. \n";  
                if (focusOn == "")
                    focusOn = txtDescriptionClientID;
              }  
            if (fName.indexOf('$' + desc.toLowerCase() + '$') != -1)
             { 
                errorMessage = errorMessage + " - Findings Description already exist. \n";    
                if (focusOn == "")
                    focusOn = txtDescriptionClientID;
             }
        }
        var filenm = document.getElementById(flupdVoiceOverClientID).value ;
        if(filenm.length > 0) 
        {
            var ext = filenm.substr(filenm.length - 4,4);
            if (ext != ".wav")
            { errorMessage = errorMessage + " - Please select valid Finding VoiceOverURL Filename. \n";
                if (focusOn == "")
                focusOn=flupdVoiceOverClientID;
            }
        }
        var embstart = document.getElementById(txtEmbargoStartClientID).value ;
        var embend = document.getElementById(txtEmbargoEndClientID).value ;
        var embargo = document.getElementById(chkEmbargoClientID);
        
        if (embargo.checked)
        { 
        
            if(embstart.length == 0) 
            {
                    errorMessage = errorMessage + " - Please enter Embargo Start Hour. \n";
                    if (focusOn == "")
                    focusOn=txtEmbargoStartClientID; 
            }
            if(embend.length == 0)
            {
                    errorMessage = errorMessage + " - Please enter Embargo End Hour. \n";
                    if (focusOn == "")
                    focusOn=txtEmbargoEndClientID; 
            }
            if(embstart.length > 0) 
            {
                if (embstart < 0 || embstart > 23)
                { 
                    errorMessage = errorMessage + " - Please enter valid Embargo Start Hour in range from 00 through 23. \n";
                    if (focusOn == "")
                    focusOn=txtEmbargoStartClientID;
                }
            }
            if(embend.length > 0) 
            {
                if (embend < 0 || embend > 23)
                { errorMessage = errorMessage + " - Please enter valid Embargo End Hour in range from 00 through 23. \n";
                    if (focusOn == "")
                    focusOn=txtEmbargoEndClientID;
                }
            }
            if ((embstart.length > 0 && embend.length > 0) && (embstart == embend)) //&& (embstart != 0 && embend != 0)
            {
                    errorMessage = errorMessage + " - Please enter Embargo End Hour value other than Embargo Start Hour value.\n";
                    if (focusOn == "")
                    focusOn=txtEmbargoEndClientID; 
            }
        }
        
        var findingOrder = document.getElementById(txtFindingOrderClientID).value ;
        if (findingOrder.length > 0)
        {
            if (fOrder.indexOf('*' + findingOrder + '*') != -1)
             { 
                errorMessage = errorMessage + " - Finding Order already exist. \n";    
                if (focusOn == "")
                    focusOn = txtFindingOrderClientID;
             }
        }
        var priority = document.getElementById(txtPriorityClientID).value;
        if(priority.length <= 0)
        {
           errorMessage = errorMessage + " - Please enter Priority. \n";
            if (focusOn == "")
            focusOn=txtPriorityClientID;
        }
        else
        {
             if (fPriority.indexOf('%' + priority + '%') != -1)
             { 
                errorMessage = errorMessage + " - Priority already exist. \n";    
                if (focusOn == "")
                    focusOn = txtPriorityClientID;
             }
        }
        
        var eventID = document.getElementById(cmbNotificationEventClientID).value;
        var deviceID = document.getElementById(cmbDeviceClientID).value;
        
        if((eventID == 0 && deviceID > 0) || (eventID > 0 && deviceID == 0))
        {
            if(eventID > 0)
                {
                    errorMessage = errorMessage + " - Please select Device. \n"
                    if (focusOn == "")
                        focusOn = cmbDeviceClientID;
                }
            else if(deviceID > 0)
                {
                    errorMessage = errorMessage + " - Please select Notification Event. \n"
                    if (focusOn == "")
                        focusOn = cmbNotificationEventClientID;
                }
        }
        
        if(errorMessage != "")
        {
            alert(errorMessage);
            document.getElementById(focusOn).focus();
            return false;
        }
        //document.getElementById(hdnTextChangedClientID).value = "false";
        return true;
     }
     
     //Clear UI Controls
     function clearUIControls()
     {
        try
        {
            var objTxtCGoal = document.getElementById(txtComplianceGoalClientID);
            if(objTxtCGoal) objTxtCGoal.value = "";
            
            var objTxtDesc  = document.getElementById(txtDescriptionClientID);
            if(objTxtDesc) objTxtDesc.value = "";
            
            var objTxtEmbargoEnd = document.getElementById(txtEmbargoEndClientID);
            if(objTxtEmbargoEnd) objTxtEmbargoEnd.value = "";
            
            var objTxtEmbargoStart  = document.getElementById(txtEmbargoStartClientID);
            if(objTxtEmbargoStart) objTxtEmbargoStart.value = "";
            
            var objTxtEndAfter = document.getElementById(txtEndAfterClientID);
            if(objTxtEndAfter) objTxtEndAfter.value = "";
            
            var objTxtEscalateEvery  = document.getElementById(txtEscalateEveryClientID);
            if(objTxtEscalateEvery) objTxtEscalateEvery.value = "";
            
            var objTxtFindingOrder = document.getElementById(txtFindingOrderClientID);
            if(objTxtFindingOrder) objTxtFindingOrder.value = "";
            
            var objTxtGSL  = document.getElementById(txtGSLClientID);        
            if(objTxtGSL) objTxtGSL.value = "";
            
            var objTxtPriority = document.getElementById(txtPriorityClientID);
            if(objTxtPriority) objTxtPriority.value = "";
            
            var objChkActive  = document.getElementById(chkActiveClientID);
            if(objChkActive) objChkActive.checked = false;
            
            var objChkDocumentedOnly = document.getElementById(chkDocumentedOnlyClientID);
            if(objChkDocumentedOnly) objChkDocumentedOnly.checked = false;
            
            var objChkContinue  = document.getElementById(chkContinueClientID);
            if(objChkContinue) objChkContinue.checked = false;
            
            var objChkEmbargo = document.getElementById(chkEmbargoClientID);
            if(objChkEmbargo) objChkEmbargo.checked = false;
            
            var objChkEmbWeekend  = document.getElementById(chkEmbWeekendClientID);
            if(objChkEmbWeekend) objChkEmbWeekend.checked = false;
            
            var objChkRequireReadback = document.getElementById(chkRequireReadbackClientID);
            if(objChkRequireReadback) objChkRequireReadback.checked = false;
            
            var objCmbStartBackup  = document.getElementById(cmbStartBackupClientID);
            if(objCmbStartBackup) objCmbStartBackup.selectedIndex = 0;
                        
            var objCmbDevice  = document.getElementById(cmbDeviceClientID);
            if(objCmbDevice) objCmbDevice.selectedIndex = 0;
            
            var objCmbNotificationEvent = document.getElementById(cmbNotificationEventClientID);
            if(objCmbNotificationEvent) objCmbNotificationEvent.selectedIndex = 0;
                        
            var objChkSendToAgent  = document.getElementById(chkSendToAgentClientID);
            if(objChkSendToAgent) objChkSendToAgent.checked = false;
        }
        catch(e){}
        
     }

     
