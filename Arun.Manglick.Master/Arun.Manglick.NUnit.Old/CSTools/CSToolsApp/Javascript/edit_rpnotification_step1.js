// JScript File

function RefreshParent(query)
{
    //window.parent.document.frames("edit_rpnotification_step2").document.location.href = 'edit_rpnotification_step2.aspx?ReferringPhysicianID=' + query ;
    //window.parent.document.frames("edit_rpnotification_step3").document.location.href = 'edit_rpnotification_step3.aspx?ReferringPhysicianID=' + query ;
}

function Warning()
{
 alert('Warning: This device is associated with notification events. You must first reassign or delete notification events before you delete this device.');
}

function canCallAdd(btnConrol)
{
    if(validateDevices())
        return true;
       // __doPostBack(btnConrol.id, '');
    return false;
}
//Sets the flag textChanged to true if the text of any textbox is changed.
function UpdateProfile()
{
   document.getElementById(textChangedClientID).value = "true";
   document.getElementById(btnAddClientID).disabled =false;
}
/*This function is used to validate the Device address and the Carrier before adding the device*/
function validateDevices(ddlDevices,txtDeviceAddress,ddlCarrier,txtGateway,ddlFindings, ddlGroup, txtInitialPause)
{    
    var setfocus = "" ;
    var device = document.getElementById(ddlDevices).value;
    var grouplist = (document.getElementById(ddlGroup) == null) ? " " : document.getElementById(ddlGroup).value;
   if (device == null || device == -1)
   {
        alert("Device is incorrect.");
        return false;
   }
   if (grouplist == null || grouplist.length == 0)
   {
        alert("Group is not available.");
        return false;
   }
  
   if(device == "1" && document.getElementById(txtGateway) != null && (document.getElementById(txtGateway).value.indexOf('Enter') > -1 || trim(document.getElementById(txtGateway).value) == ""))
   {
        alert("Device address is incorrect.");
        return false;
   }
   var val = null;
   if (document.getElementById(txtDeviceAddress) != null)
        val = trim(document.getElementById(txtDeviceAddress).value);   
   var val1 = null;
   if (document.getElementById(ddlCarrier) != null)
        val1 = trim(document.getElementById(ddlCarrier).value);
   
   if(device == "11" || device == "12" || device == "13" || device == "14")
    {
        var deviceAdd = trim(document.getElementById(txtDeviceAddress).value);
        var lengthDeviceAdd = deviceAdd.length;
        if(lengthDeviceAdd != 10)
        {
            alert("Please enter valid Device Address.");
            document.getElementById(txtDeviceAddress).focus();
            return false;
        }
    }
   
   if ((val != null && (val == "Enter Cell # (numbers only)" || val == "Enter Email Address" || val == "Enter Fax # (numbers only)"  || val == "Enter Pager # (numbers only)" || val == "Enter Pager # + PIN (numbers only)" || val == "")) && (val1 != null && val1 == -1 ))
   {
        alert("Device information is incorrect.");
        return false;
   }
   if ((val == "Enter Cell # (numbers only)" || val == "Enter Email Address" || val == "Enter Fax # (numbers only)"  || val == "Enter Pager # (numbers only)" || val == "Enter Pager # + PIN (numbers only)" || val == "") && val != null)
   {        
        alert("Device address is incorrect.");
        return false;
   }
   else if (val1 == -1 && val1 != null)
   {
        alert("Carrier is incorrect.");
        return false;
   }
   var val2 = null;
   if (document.getElementById(txtInitialPause) != null)
        val2 = trim(document.getElementById(txtInitialPause).value);   
            
   
    if(device == "14")
    {
        if(val2 != null)
        {
            if(val2 == '')
            {
                alert("Initial pause time is incorrect");
                return false;
            }
            else
            {
                var regNumericValidate = /^(\d{0,5})(\.\d\d?)?$/;
                
                if(!regNumericValidate.test(val2))
                {
                   alert("Initial pause time is incorrect");
                   document.getElementById(txtInitialPause).focus();
                   return false;
                }
                
                if (val2.indexOf(".") == val2.lastIndexOf(".")) 
                {
                    if(val2 < 1 || val2 > 30.99)
                    {
                        alert("Initial pause time should be betweeen 1 to 30.99");
                        document.getElementById(txtInitialPause).focus();
                        return false;
                    }
                    if (val2.length > 5)
                    {
                        alert("Initial pause time is incorrect");
                        document.getElementById(txtInitialPause).focus();
                        return false;
                    }
                }
                else
                {
                    alert("Initial pause time is incorrect");
                    document.getElementById(txtInitialPause).focus();
                    return false;
                }
            }
        }
    }
}

/* This function will remove the text that asks to Enter device in the Device Address textbox when the user clicks on it */
    function RemoveDeviceLabel(txtDeviceAddress,hidDeviceLabel)
    {
       if(document.getElementById(txtDeviceAddress).value.indexOf('Enter') == 0)
       {
         document.getElementById(hidDeviceLabel).value = document.getElementById(txtDeviceAddress).value;
         document.getElementById(txtDeviceAddress).value = "";
       }
    }

    /* This function will remove the text that asks to Enter gateway in the Gateway textbox when the user clicks on it */
    function RemoveGatewayLabel(txtGateway,hidGatewayLabel)
    {
       if(document.getElementById(txtGateway).value.indexOf('Enter') == 0)
       {
         document.getElementById(hidGatewayLabel).value = document.getElementById(txtGateway).value;
         document.getElementById(txtGateway).value = "";
       }
    }

    /* This function will remove the text that asks to Enter device in the Initial Pause textbox when the user clicks on it */
    function RemoveInitialPauseLabel(txtInitialPause, hidInitPauseLabel)
    {
       if(document.getElementById(txtInitialPause).value.indexOf('Value') == 0)
       {
         document.getElementById(hidInitPauseLabel).value = document.getElementById(txtInitialPause).value;
         document.getElementById(txtInitialPause).value = "1";
       }
    }
///* For the device type Skytel / Pager USA it should allow space at 10 th index*/
//function isNumericKeyOrSpace()
//{
//    if(document.getElementById(txtDeviceAddressClientID).value.length == 10)
//    {
//        return isSpace();        
//    }  
//    else
//        return isNumericKeyStroke();                
//}

 //Check entered key is numeric or not
function isNumericKeyStrokes()
{ 
    var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
    
    //alert(event.keyCode);
    if (((keyCode >= 48) && (keyCode <= 57)))
    {
       return;                                  
    }
    window.event.returnValue = null;     
}

/* For the device type Skytel / Pager USA it should allow space at 10 th index*/
    function isNumericKeyOrSpace(txtDeviceAddress)
    {
        if(document.getElementById(txtDeviceAddress).value.length == 10)
        {
            return isSpace();        
        }  
        else
        {
            document.getElementById(btnAddClientID).disabled =false;
            return isNumericKeyStrokes(textChangedClientID);                
        }
    } 
    
/*For pager devices which will be accepting space at any position */
function PagerValidationWithSpace(controlID)
{   
    if(document.getElementById(controlID).value.length > 0 && (!IsTextContainsSpace(document.getElementById(controlID).value)))
    {
      if(isSpace())
        return true;
      else
        return isNumericKeyStrokes();        
    }
    else
    {            
        return isNumericKeyStrokes();
    }
    
}

/*Function to know the number of spaces entered for the pager device */
function IsTextContainsSpace(controlText)
{   
    var count = 0;
    if(controlText.length > 0)
    {
        for(var i = 0; i < controlText.length; i++)
        {
            if(controlText.charAt(i)==" ")
             count++;            
        }
    }
    if(count >= 1)
        return true;
    else 
        return false;    
}
    
    
     /*To Get Confirmation before Delete.*/
     function ConformBeforeDelete()
     {
        if(confirm('Are you sure you want to delete this record?' ))
        {
            ChangeFlagforGrid();
            otherPostback =true;
            return true;
        }
        otherPostback =false;
        return false;          
     }
//Sets Postback Variable value to True
    function SetPostbackVarTrue()
    {
        otherPostback = true;
    }

    //Sets Postback Variable value to False
    function SetPostbackVarFalse()
    {
        otherPostback = false;
    }
    //Sets the flag textChanged to false when user clicks on Save button as it should not ask confirmation message even
    //if user clicks on Save button. 
    function ChangeFlagforGrid()
    {        
        var flag = document.getElementById(textChangedClientID).value
        if (flag == "true")
        {
            document.getElementById(textChangedClientID).value = "false";
            document.getElementById(hdnGridChangedClientID).value = "true";
        }
        return true;
    }
    
    //This function is used to set flag true on click of add button
    function AddRecordFromGrid()
    {
        try
        {            
            document.getElementById(hdnIsAddClickedClientID).value = 'true';
            return ChangeFlagforGrid();            
            
        }
        catch(e){}
    }                                       


    function chnangeGridFlag()
    {
            document.getElementById(textChangedClientID).value = "true";
            document.getElementById(hdnGridChangedClientID).value = "false";
           // return;
    }
