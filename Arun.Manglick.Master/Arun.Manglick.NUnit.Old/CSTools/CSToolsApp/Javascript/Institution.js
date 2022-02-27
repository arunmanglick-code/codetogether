// JScript File
/******************************File History***************************
 * File Name        : Institution.js
 * Author           : Prerak Shah
 * Created Date     : 17-Aug-2007
 * Purpose          : This Java script file contains functions for institution, Group and finding.
 *                  : 
 *                  :

 * *********************File Modification History*********************

 * Date(dd-mm-yyyy) Developer Reason of Modification
 *  10-12-2007 Prerak - Change FindingCheckRequired function for checking duplicate 
                        finding order and Fading Description
 *  10-12-2007 Prerak - Change FindingCheckRequired function for checking Required and 
                        duplicate Priority.
 *  17-12-2007 Prerak - Change FindingCheckRequried function for duplicate finding order, 
                        description and priority.   
 *  28-02-2008 IAK    - Finding- Embargo Validation Changed,   
 *  19-09-2008 IAK    - Moved FindingCheckRequired() function to finding.js   
 *  19-11-2008 Prerak - Validation added - 800Number and Phone Number does not accepts alphabets when copy pasted.                                       
 * ------------------------------------------------------------------- 
 
 */

function isNumericKeyStroke()
{
    var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
    //alert(event.keyCode);
    if ( ((keyCode >= 48) && (keyCode <= 57)) )//||  All numerics
            //((keyCode >= 96) && (keyCode <= 105)))     Number pad numbers
           {
                document.getElementById(hdnTextChangedClientID).value = "true";  
                return;                                  
            }
    
    window.event.returnValue = null;     
}

//This function handles validation for required fields of Add Institution.
function checkRequired()
{  

var errorMessage = "Please enter following:";
var errorMessage1 = "";
var errorMessage2 = "";  
var regx = /[a-zA-Z\s\.\'\-]/;
var regxEmail = /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/; 
var reg800Nm = /\d{10}/; 
var focusOn = "";    
if(document.getElementById(txtInstitutionNameClientID).value.replace(/^\s+|\s+$/g,"") == "")
{
   errorMessage1 = " - Please enter Institution Name \n";        
   focusOn = txtInstitutionNameClientID;
}
if(document.getElementById(PrimaryNameClientID).value.replace(/^\s+|\s+$/g,"") == "")
{
   errorMessage1 = errorMessage1 + " - Please enter Primary Name \n";
    if (focusOn == "")
       focusOn = PrimaryNameClientID; 
}
if(document.getElementById(cmbTimeZoneClientID).value == "-1")
{
    errorMessage1 = errorMessage1 + " - Please select TimeZone \n";
     if (focusOn == "")
       focusOn = cmbTimeZoneClientID; 
}
if (regx.test(document.getElementById(txtInstitutionNameClientID).Value)==false)
{  
    errorMessage1 = errorMessage1 + " - Please enter valid Institution Name \n";
    if (focusOn == "")
       focusOn = txtInstitutionNameClientID; 
}
var mainNum = document.getElementById(txtMainNumber1ClientID).value + document.getElementById(txtMainNumber2ClientID).value + document.getElementById(txtMainNumber3ClientID).value
if ((mainNum.length > 0 && mainNum.length != 10)|| (mainNum.length > 0 && reg800Nm.test(mainNum)==false))
{
    errorMessage1 = errorMessage1 + " - Please enter valid Main Number \n";
    if (focusOn == "")
        focusOn = txtMainNumber1ClientID;
}
var primaryNum = document.getElementById(txtPrimaryPhone1ClientID).value + document.getElementById(txtPrimaryPhone2ClientID).value + document.getElementById(txtPrimaryPhone3ClientID).value
if ((primaryNum.length > 0 && primaryNum.length != 10) || (primaryNum.length > 0 && reg800Nm.test(primaryNum)==false))
{
    errorMessage1 = errorMessage1 + " - Please enter valid Primary Phone Number \n";
    if (focusOn == "")
        focusOn = txtPrimaryPhone1ClientID;
}
var primaryEmail = document.getElementById(txtPrimaryEmailClientID).value.replace(/^\s+|\s+$/g,"");
if (primaryEmail.length > 0)
{
    if (regxEmail.test(primaryEmail)==false)
    {   errorMessage1 = errorMessage1 + " - Please enter valid Primary Email \n";
        if (focusOn == "")
            focusOn = txtPrimaryEmailClientID;
    }
}
var contact1Num = document.getElementById(txtContact1Phone1ClientID).value + document.getElementById(txtContact1Phone2ClientID).value + document.getElementById(txtContact1Phone3ClientID).value
if ((contact1Num.length > 0 && contact1Num.length != 10) || (contact1Num.length > 0 && reg800Nm.test(contact1Num)==false))
{
    errorMessage1 = errorMessage1 + " - Please enter valid Contact1 Phone Number \n";
    if (focusOn == "")
        focusOn = txtContact1Phone1ClientID;
}
var contact1Email = document.getElementById(txtContact1emailClientID).value;
if (contact1Email.length > 0)
{
    if (regxEmail.test(contact1Email)==false)
    {   errorMessage1 = errorMessage1 + " - Please enter valid Contact1 Email \n";
        if (focusOn == "")
            focusOn = txtContact1emailClientID;
    }    
}
var contact2Num = document.getElementById(txtContact2Phone1ClientID).value + document.getElementById(txtContact2Phone2ClientID).value + document.getElementById(txtContact2Phone3ClientID).value
if ((contact2Num.length > 0 && contact2Num.length != 10)|| (contact2Num.length > 0 && reg800Nm.test(contact2Num)==false))
{
    errorMessage1 = errorMessage1 + " - Please enter valid Contact2 Phone Number \n";
    if (focusOn == "")
        focusOn = txtContact2Phone1ClientID;
}
var contact2Email = document.getElementById(txtContact2emailClientID).value;
if (contact2Email.length > 0)
{
    if (regxEmail.test(contact2Email)==false)
    {   errorMessage1 = errorMessage1 + " - Please enter valid Contact2 Email \n";
        if (focusOn == "")
          focusOn = txtContact2emailClientID; 
    }
}
var lab800Num = document.getElementById(txtLab800No1ClientID).value + document.getElementById(txtLab800No2ClientID).value + document.getElementById(txtLab800No3ClientID).value
if ((lab800Num.length > 0 && lab800Num.length != 10) || (lab800Num.length > 0 && reg800Nm.test(lab800Num)==false))
{
    errorMessage1 = errorMessage1 + " - Please enter valid Lab 800 Number \n";
    if (focusOn == "")
        focusOn = txtLab800No1ClientID;
}
var nurse800Num = document.getElementById(txtNurse800No1ClientID).value + document.getElementById(txtNurse800No2ClientID).value + document.getElementById(txtNurse800No3ClientID).value
if ((nurse800Num.length > 0 && nurse800Num.length != 10) || (nurse800Num.length > 0 && reg800Nm.test(nurse800Num)==false))
{
    errorMessage1 = errorMessage1 + " - Please enter valid Nurse 800 Number \n";
    if (focusOn == "")
        focusOn = txtNurse800No1ClientID;
}
var shiftNurse800Num = document.getElementById(txtShiftNurce800No1ClientID).value + document.getElementById(txtShiftNurce800No2ClientID).value + document.getElementById(txtShiftNurce800No3ClientID).value
if ((shiftNurse800Num.length > 0 && shiftNurse800Num.length != 10)|| (shiftNurse800Num.length > 0 && reg800Nm.test(shiftNurse800Num)==false))
{
    errorMessage1 = errorMessage1 + " - Please enter valid Shift Nurse 800 Number \n";
    if (focusOn == "")
        focusOn = txtShiftNurce800No1ClientID;
}

if(errorMessage1 != "")
{
    alert(errorMessage1);
    document.getElementById(focusOn).focus();
    return false;
}
return true;
}

 function enabledControl()
{
    
    var reqVoiceclips;
    reqVoiceclips = document.getElementById(rbRequireVoiceClipsClientID + '_0');
    if (reqVoiceclips.checked)
    {
        document.getElementById(lblTabNameClientID).style.visibility = "visible";
        document.getElementById(rbConnectEDClientID).style.visibility = "visible";
    }
    else
    {
        document.getElementById(lblTabNameClientID).style.visibility = "hidden";
        document.getElementById(rbConnectEDClientID).style.visibility = "hidden";
    }
}

    // For Add/Edit Group page Java scripts
 //This function handles validation for required fields of Add group.
 function groupCheckRequired(addgroup)
 {  
    var errorMessage1 = "";
    var focusOn = "";  
    var regx = /[a-zA-Z\s\.\'\-]/; 
    var reg800Nm = /\d{10}/;      
    if(document.getElementById(txtGroupNameClientID).value.trim() == "") 
    {
       errorMessage1 = " - Please enter Group Name \n";
       focusOn=txtGroupNameClientID;
    }
    if(document.getElementById(cmbDirNameClientID).value == "-1")
    {
       errorMessage1 = errorMessage1 + " - Please select Directory \n";
        if (focusOn == "")
        focusOn=cmbDirNameClientID;
    }
    if((document.getElementById(txtGroup800No1ClientID).value == "") && (document.getElementById(txtGroup800No2ClientID).value == "") && (document.getElementById(txtGroup800No3ClientID).value == ""))
    {
        errorMessage1 = errorMessage1 + " - Please enter Group 800 Number \n";
        if (focusOn == "")
        focusOn=txtGroup800No1ClientID;
    }
    else
    {
        var group800No = document.getElementById(txtGroup800No1ClientID).value + document.getElementById(txtGroup800No2ClientID).value + document.getElementById(txtGroup800No3ClientID).value;
        if ((group800No.length > 0 && group800No.length != 10) || reg800Nm.test(group800No)==false )
        {
          errorMessage1 = errorMessage1 + " - Please enter Valid Group 800 Number \n";
          if (focusOn == "")
            focusOn=txtGroup800No1ClientID;  
        }
    }
    
    if((document.getElementById(txtRP800No1ClientID).value == "") && (document.getElementById(txtRP800No2ClientID).value == "") && (document.getElementById(txtRP800No3ClientID).value == ""))
    {
        errorMessage1 = errorMessage1 + " - Please enter Ordering Clinician 800 Number \n";
        if (focusOn == "")
        focusOn=txtRP800No1ClientID;
    }
    else
    {
        var RP800No = document.getElementById(txtRP800No1ClientID).value + document.getElementById(txtRP800No2ClientID).value + document.getElementById(txtRP800No3ClientID).value;
        if ((RP800No.length > 0 && RP800No.length != 10) || reg800Nm.test(RP800No)==false )
        {
            errorMessage1 = errorMessage1 + " - Please enter Valid Ordering Clinician 800 Number \n";
            if (focusOn == "")
                focusOn=txtRP800No1ClientID; 
        }
    }
    if(document.getElementById(cmbPractieTypeClientID).value == "-1")
    {
        errorMessage1 = errorMessage1 + " - Please select Practice Type \n";
        if (focusOn == "")
        focusOn=cmbPractieTypeClientID;
    }
     if(document.getElementById(cmbTimeZoneClientID).value == "-1")
    {
        errorMessage1 = errorMessage1 + " - Please select TimeZone \n" ;
        if (focusOn == "")
        focusOn=cmbTimeZoneClientID
    }
    var primaryNum = document.getElementById(txtPhone1ClientID).value + document.getElementById(txtPhone2ClientID).value + document.getElementById(txtPhone3ClientID).value
    if ((primaryNum.length > 0 && primaryNum.length != 10)|| (primaryNum.length > 0 && reg800Nm.test(primaryNum)==false))
    {
        errorMessage1 = errorMessage1 + " - Please enter valid Phone Number \n";
        if (focusOn == "")
            focusOn = txtPhone1ClientID;
    }
    if (addgroup == "AddGroup")
    {
        if (document.getElementById(chkDefFindingClientID).checked == false && document.getElementById(chkOtherFindingsClientID).checked == false)
        {
            errorMessage1 = errorMessage1 + " - Please check Insert Default Findings or Configure Other Findings \n";
            if (focusOn == "")
                focusOn = chkDefFindingClientID;
        }
    }

    if(errorMessage1 != "")
    {
        alert(errorMessage1);
        document.getElementById(focusOn).focus();
        return false;
    }
    
    return true;
    
    
 }

//Check for unit device address is blank or not
    var otherPostback =false;

    function ValidateAddGroupForm()
    {
       if(document.getElementById(hdnTextChangedClientID).value == "true")
       {
        if(otherPostback == true)   
            return false;
       }                  
       return true;        
    }
 // Check for Group support Note    
 function checkNote()
 {
    var errorMessage = "Please enter following: ";
    var errorMessage1 = "";
    var errorMessage2 = "";        
    if((document.getElementById(txtAuthorClientID).value == "") && (document.getElementById(txtNoteClientID).value == ""))
    {
       errorMessage1 = "\n - Author \n - Note";        
    }
    else if(document.getElementById(txtAuthorClientID).value == "")
    {
        errorMessage1 = "\n - Author";
    }
    else if(document.getElementById(txtNoteClientID).value == "")
    {
        errorMessage2 = "\n - Note";
    }
    if(errorMessage1 != "")
    {
        alert(errorMessage + errorMessage1);
        document.getElementById(txtAuthorClientID).focus();
        return false;
    }
    else if(errorMessage2 != "")
    {
      alert(errorMessage + errorMessage2);
      document.getElementById(txtNoteClientID).focus();
      return false;
    }
    return true;
 }
 //Change text of Default finding
 function changeTextforDefaultFinding()
 {  
    var groupType;
    groupType = document.getElementById(rbGroupTypeClientID + '_0');
    if (groupType.checked)
       document.getElementById(lblDefaultClientID).innerText = "Insert Default Findings ( red, orange, yellow ):";
    else
       document.getElementById(lblDefaultClientID).innerText = "Insert Default Findings ( red, orange, yellow, pos, neg, other ):";
 }
   //Sets the flag textChanged to true if the text is changed for any control.
    function TextChanged()
    {
       document.getElementById(hdnTextChangedClientID).value = "true";
       return true;
    }   
    
   