/******************************File History***************************
 * File Name        : masterPage.js
 * Author           : RaviS
 * Created Date     : 13-04-07
 * Purpose          : Interface to add Subscriber Information.

 * *********************File Modification History*********************

 * * Date(dd-mm-yyyy) Developer Reason of Modification
**********************************************************************
 * 13-04-2007    RaviS     File Created
 * 04-01-2008    IAK       Added new function: validatePhoneNumber, validateFaxNumber and validatePhoneOrFax
 * 09-01-2008    IAK       Modified function: validatePhoneNumber, validateFaxNumber set focus on first textbox if invalid entry
 * 11-01-2008    IAK       Defect 2555
 * 13-11-2008    IAK       Defect #3593 validateLoginID() function added
 * 09-01-2009    GB        Changes made for FR #282     
*********************************************************************
 */
 
    var mapId = "user_profile.aspx";

    function RefreshParent()
    {
        window.parent.document.location.reload(true); 
    }

    function validateLoginID(source, arguments)
    {
        var len = document.getElementById(txtLoginIdClientID).value.length;
        if(len > 1 && len < 11)
        {
            arguments.IsValid = true;
            return true;
        }
        else
        {
            arguments.IsValid = false;
            document.getElementById(txtLoginIdClientID).focus();
            return false;
        }   
    }
    
    /* Custom function to validate Email*/
    function validatePhoneNumber(source, arguments)
    {
        var phone1 = document.getElementById(txtPhone1ClientID).value;
        var phone2 = document.getElementById(txtPhone2ClientID).value;
        var phone3 = document.getElementById(txtPhone3ClientID).value;
        if(validatePhoneOrFax(phone1, phone2, phone3))
        {
            
            arguments.IsValid = true;
            return true;
        }
        else
        {
            arguments.IsValid = false;
            document.getElementById(txtPhone1ClientID).focus();
            return false;
        }
         
    }
    
    /* Custom function to validate Fax*/
    function validateFaxNumber(source, arguments)
    {
        var phone1 = document.getElementById(txtFax1ClientID).value;
        var phone2 = document.getElementById(txtFax2ClientID).value;
        var phone3 = document.getElementById(txtFax3ClientID).value;
        if(validatePhoneOrFax(phone1, phone2, phone3))
        {
            arguments.IsValid = true;
            return true;
        }
        else
        {
            arguments.IsValid = false;
            document.getElementById(txtFax1ClientID).focus();
            return false;
        }
    }
    
    /* Validate phone number or fax for 3-3-4*/
    function validatePhoneOrFax(textbox1, textbox2, textbox3)
    {
        if(textbox1.length == 0 && textbox2.length == 0 && textbox3.length == 0)
        {
            return true;
        }
        else if(textbox1.length > 0 && textbox2.length > 0 && textbox3.length > 0)
        {
            if(textbox1.length != 3)
                return false;
            if(textbox2.length != 3)
                return false;
            if(textbox3.length != 4)
                return false;
            return true;    
        }    
        else
        {
            return false;
        }
    }
    
    /*Call Send Overdue Report button server side click */
    function SendReportButton()
    {
      document.getElementById("btnSendOverdueRpt").disabled == false;
    }
    
    /* This function will return true if input key in numberic else return false. This function used to accept numberic keys for textbox*/
    function isNumericKeyStroke()
    {
         var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

        if ( ((keyCode >= 48) && (keyCode <= 57))) //||  All numerics
        {
            document.getElementById(txtChangedClientID).value = "true";  
            return;                                  
        }
        
        window.event.returnValue = null;     
    }
    
    /* Sets the flag txtChanged to true if the text of any textbox is changed. */
    function UpdateProfile(txtChanged)
    {
        document.getElementById(txtChanged).value = "true";
    }
   
    function nullifyEnterKey()
    {
        var returnValue = false;
        var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;
        if (keyCode == 13)
        { 
          return false;
        }
        return true;
    }
    /* Action on Enter Key pressed in profile section call save routine */
    function OnEnterKeyPressed()
    {
       event.returnValue=false; 
       event.cancel = true;
    }   
    /* Enable the Generate pin button */
    function EnableGenerateButton(btnName)
    { 
       document.getElementById(btnName).disabled = false;
    }
    /* Disable the Generate pin button */
    function DisableGenerateButton(btnName)
    {
        document.getElementById(btnName).disabled = true;
        //document.getElementById(txtFirstNameClientID).focus();
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
    
    /* Change flag hdnOutstandingChanged to true in case of data changed in Outstanding Report data changed */
    function ChangeOutstandingData()
    {
       document.getElementById('hdnOutstandingChanged').value = "true";
    }
    
    /* Show message break line when given string find # */
    function showMessage(msg)
    {
        var temp = new Array();
        var errMsg; 
        temp = msg.split('#');
        for(var i=0; i<temp.length; i++)
        {
            if(i == 0)
            {
                errMsg = temp[i];           
            }
            else
            {
                errMsg += "\n" + temp[i];
            }
             
         }       
        
        alert(errMsg);
    }
    
    /* Call this function on form load and button Click of Update report setting to validate Outstanding Messages Reports */
    function ValidateMessageReportDetails()
    {      
        if(document.getElementById('cbEmailReports').checked || document.getElementById('cbFaxReports').checked)
        {
            WeekDaysDisabled(false);
        }
        else
        {
          WeekDaysDisabled(true);
        }
        if(document.getElementById('cbEmailReports').checked ==false && document.getElementById('cbFaxReports').checked == false)
        {
            WeekDaysChecked(false);
        }
    }  
  
  /* Call this function to on click of Outstanding Messages Reports check boxes- Enable/Disable the weekdays Checkboxes*/
  function ValidateMRDetails()
  {
        //txtValidateMessage: To do validation Outstanding report Section value =  1:valid  0:Invalid  
        var errorMessage = "";
         if(document.getElementById(cbEmailReportsClientID).checked == false && document.getElementById(cbFaxReportsClientID).checked == false)
        {
            document.getElementById(txtValidateMessageClientID).value =1;
        }    
        else if(document.getElementById(cbEmailReportsClientID).checked || document.getElementById(cbFaxReportsClientID).checked)
        {
            if(document.getElementById(cbMondayClientID).checked || 
               document.getElementById(cbTuesdayClientID).checked ||
               document.getElementById(cbWednesdayClientID).checked ||
               document.getElementById(cbThursdayClientID).checked ||
               document.getElementById(cbFridayClientID).checked ||
               document.getElementById(cbSaturdayClientID).checked ||
               document.getElementById(cbSundayClientID).checked)
            {
                document.getElementById(txtValidateMessageClientID).value =1;
            }
            else
            {
                document.getElementById(txtValidateMessageClientID).Value ="";
                isValid = false;
                errorMessage = "You must select atleast one week day.\r\n"
            }
            var isEmailSectionValid =true;
            var isFaxSectionValid =true;
            var faxLength = document.getElementById(txtFaxAreaCodeClientID).value.length  + document.getElementById(txtFaxPrefixClientID).value.length + document.getElementById(txtFaxNNNNClientID).value.length;
            if(faxLength != 10)
            {
              isFaxSectionValid =false;
            }
            if(document.getElementById(txtEmailClientID).value.length == 0)
            {   
                isEmailSectionValid = false;       
            }
            else
            {
                var email = document.getElementById(txtEmailClientID).value;
                var dotIndex = email.indexOf(".");
                var lastDotIndex = email.lastIndexOf(".");
                var atRateIndex = email.indexOf("@");
                if((dotIndex == -1) || (atRateIndex == -1) || (dotIndex == 0) || (atRateIndex == 0) || (dotIndex == email.length) || (atRateIndex == email.length) || (lastDotIndex < atRateIndex))
                    isEmailSectionValid = false;
                else 
                    {
                        isEmailSectionValid  = true;
                    }
            }
            if(document.getElementById(cbEmailReportsClientID).checked && document.getElementById(cbFaxReportsClientID).checked)
            {
                if(isEmailSectionValid ==  false && isFaxSectionValid == false)
                 errorMessage += "To send Outstanding Messages Reports by E-mail & Fax, you must have E-mail & Fax number into Profile section.\r\n";
                else if(isEmailSectionValid == false)
                     errorMessage += "To send Outstanding Messages Reports by E-mail, you must have E-mail into Profile section.\r\n";
               else   if(isFaxSectionValid == false)
                    errorMessage += "To send Outstanding Messages Reports by Fax, you must have Fax number into Profile section.\r\n";
            }
            else if(document.getElementById(cbEmailReportsClientID).checked)
            {
                 if(isEmailSectionValid == false)
                     errorMessage += "To send Outstanding Messages Reports by E-mail, you must have E-mail into Profile section.\r\n";
            }
            else if(document.getElementById(cbFaxReportsClientID).checked)
            {
                 if(isFaxSectionValid == false)
                    errorMessage += "To send Outstanding Messages Reports by Fax, you must have Fax number into Profile section.\r\n";
            }
            if(errorMessage.length != 0)
                alert(errorMessage);
        }
        else
        {
          WeekDaysDisabled(true); 
        }
        
       if(errorMessage.length == 0)
       { 
          document.getElementById(hdnProfileSavedClientID).value = "false";  
          document.getElementById(hdnSaveCalledClientID).value = "false";    
          return true;
       }
       else
       {
          return false;
       }
        
  }
    /* Enable/ Disable Week days of Outstanding Messages Reports depend on passed value*/
    function WeekDaysDisabled(bDisable)
    {
        document.getElementById(cbMondayClientID).disabled = bDisable;
        document.getElementById(cbTuesdayClientID).disabled = bDisable;
        document.getElementById(cbWednesdayClientID).disabled = bDisable;
        document.getElementById(cbThursdayClientID).disabled = bDisable;
        document.getElementById(cbFridayClientID).disabled = bDisable;
        document.getElementById(cbSaturdayClientID).disabled = bDisable;
        document.getElementById(cbSundayClientID).disabled = bDisable;
        document.getElementById(ddlReportTimeClientID).disabled = bDisable;
    }
    
    /* Check /unCheck week days of Outstanding Messages Reports depend on passed value*/
    function WeekDaysChecked(bChecked)
    {
        document.getElementById(cbMondayClientID).checked = bChecked;        
        document.getElementById(cbTuesdayClientID).checked = bChecked;        
        document.getElementById(cbWednesdayClientID).checked = bChecked;        
        document.getElementById(cbThursdayClientID).checked = bChecked;        
        document.getElementById(cbFridayClientID).checked = bChecked;        
        document.getElementById(cbSaturdayClientID).checked = bChecked;        
        document.getElementById(cbSundayClientID).checked = bChecked;      
    }


    
    /* Call this function on form load and button Click of Update report setting to validate Outstanding Messages Reports */
    function ValidateMessageReportDetails()
    {      
        if(document.getElementById(cbEmailReportsClientID).checked || document.getElementById(cbFaxReportsClientID).checked)
        {
            WeekDaysDisabled(false);
        }
        else
        {
          WeekDaysDisabled(true);
        }
        if(document.getElementById(cbEmailReportsClientID).checked ==false && document.getElementById(cbFaxReportsClientID).checked == false)
        {
            WeekDaysChecked(false);
        }
    }  
  
    /* Sets the flag txtChanged to false when user clicks on Save button as it should not ask confirmation message even
    if user clicks on Save button. */
    function ChangeFlag()
    {        
        try
        {
            if(txtChangedClientID)
            {       
            }
            document.getElementById(txtChangedClientID).value = "false";
        }
        catch(_error)
        {
            
        }
        
    }
    /* Set variable hdnSaveCalledClientID true, by which it will identify whether save button clicked or called by other routine */
    function Button_SaveCalled()
    {
        document.getElementById(hdnSaveCalledClientID).value = "true";    
        return true;  
    }
    
    /* Navigate to Message List page*/
    function cancelClick(navigationPage)
    {
            Navigate("./" + navigationPage);
            return false;
    }
    
    /* Called to disable the weekdays checkbox, if email or fax neither selected and wiseversa*/       
    function checkReports()
    {
        if(document.getElementById(cbEmailReportsClientID).checked || document.getElementById(cbFaxReportsClientID).checked)
            {
                document.getElementById(cbMondayClientID).disabled  = false;
                document.getElementById(cbTuesdayClientID).disabled  = false;
                document.getElementById(cbWednesdayClientID).disabled  = false;
                document.getElementById(cbThursdayClientID).disabled  = false;
                document.getElementById(cbFridayClientID).disabled  = false;
                document.getElementById(cbSaturdayClientID).disabled  = false;
                document.getElementById(cbSundayClientID).disabled  = false;
                document.getElementById(ddlReportTimeClientID).disabled = false;
                
            }
        else
            {
                document.getElementById(cbMondayClientID).disabled  = true;
                document.getElementById(cbTuesdayClientID).disabled  = true;
                document.getElementById(cbWednesdayClientID).disabled  = true;
                document.getElementById(cbThursdayClientID).disabled  = true;
                document.getElementById(cbFridayClientID).disabled  = true;
                document.getElementById(cbSaturdayClientID).disabled  = true;
                document.getElementById(cbSundayClientID).disabled  = true;
                document.getElementById(ddlReportTimeClientID).disabled = true;
            }
                document.getElementById(btnGeneratePasswordClientID).disabled = true;
    }
    
    /*Enable Affilation and Speciality textbox only if roles are lab tech or specialist*/
    function enableSpecialistInfo(roleComboClientID, txtAffilationClientID, txtSpecialityClientID, isComboChange)
    {
        ConfirmOKCancel();
        var roleCombo = document.getElementById(roleComboClientID);
        if(roleCombo.value == 1 || roleCombo.value == 4)
        {
            document.getElementById(txtAffilationClientID).disabled = false;
            document.getElementById(txtSpecialityClientID).disabled = false;
        }
        else
        {
            document.getElementById(txtAffilationClientID).disabled = true;
            document.getElementById(txtSpecialityClientID).disabled = true;
        }
            
        if(isComboChange == "true")
        {
            document.getElementById(txtAffilationClientID).value = "";
            document.getElementById(txtSpecialityClientID).value = "";
        }
    }
    
     /*Ask conformation before changing role to group admin*/
    function ConfirmOKCancel()
    {
      var roleComboVal = document.getElementById(ddlRoleClientID).value;
      var hdnRoleVal = document.getElementById(hdnUserRoleClientID).value;
      
      if((roleComboVal == "2" && hdnRoleVal != "2") ||  (roleComboVal == "8" && hdnRoleVal != "8"))
      {
          var where_to= confirm("Changing to Group Admin will remove all personal notifications and events but will not change group settings. Click OK to Proceed.");
          
          if (where_to== true)
          {
            document.getElementById(txtConfirmClientID).value = 1;
            //__doPostBack('btnConfirmClientID','');
          }
          else
          {
             document.getElementById(txtConfirmClientID).value = 0;
             document.getElementById(ddlRoleClientID).value = hdnRoleVal;
            //__doPostBack('btnConfirmClientID','');
          }
      }
      else
      {
        document.getElementById(txtConfirmClientID).value = 0;
      }
}

    //This function is used to set flag true on click of add button
    function AddRecordFromGrid()
    {
        try
        {            
            document.getElementById(hdnIsAddClickedClientID).value = 'true';
            return ChangeFlag();;            
            
        }
        catch(e){}
    }
