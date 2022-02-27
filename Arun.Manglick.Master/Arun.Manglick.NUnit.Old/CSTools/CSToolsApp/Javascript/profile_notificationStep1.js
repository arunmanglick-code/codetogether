

//This function will refresh Profile page
function RefreshParent(query)
{
// //window.parent.location.href='user_profile.aspx?SubscriberID=' + query ; 
//// window.parent.document.frames("notificationStep1").document.location.reload(true); 
// window.parent.document.frames("notificationStep2").document.location.href = 'profile_notificationStep2.aspx?SubscriberID=' + query ; 
// window.parent.document.frames("notificationStep3").document.location.href = 'profile_notificationStep3.aspx?SubscriberID=' + query ; 
// //window.parent.document.location.reload(true); 
}

//This function will display warning message
function Warning()
{
     alert('Warning: This device is associated with notification events. You must first reassign or delete notification events before you delete this device.');
}
function canCallAdd(btnConrol)
{
    if(validateDevices())
      return true;
    return false;
}
//This function is used to validate the Device address and the Carrier before adding the device
function validateDevices()
{
   var device = trim(document.getElementById(cmbDevicesClientID).value);
   if (device == null || device == -1)
   {
        alert("Device is incorrect.");
        return false;
   }
   var val = null;
   var lengthDeviceAdd=null;
   if (document.getElementById(txtDeviceAddressClientID) != null)
   {
        val = trim(document.getElementById(txtDeviceAddressClientID).value);
        lengthDeviceAdd = val.length;
   }
   var lengthGatway = null;    
   var val2 = null  
   if (document.getElementById(txtGatewayClientID) != null)
   {
        val2 = trim(document.getElementById(txtGatewayClientID).value);
        lengthGatway = val2.length;      
   }
   var val1 = null;
   if (document.getElementById(cmbCarrierClientID) != null)
        val1 = trim(document.getElementById(cmbCarrierClientID).value);
        
    if(device == "11" || device == "12" || device == "13" ||device == "14") // 11-14 Outbound devices
    {
        var deviceAdd = trim(document.getElementById(txtDeviceAddressClientID).value);
        var lengthDeviceAdd = deviceAdd.length;
        if(lengthDeviceAdd != 10 )
        {
            alert("Please enter valid Device Address.");
            document.getElementById(txtDeviceAddressClientID).focus();
            return false;
        }
    }
   
   if (val1 != null)
   {
        if(val1 == -1)
        {
            if(val != null)
            {
                if(val == "Enter Cell # (numbers only)" || val == "Enter Email Address" || val == "Enter Fax # (numbers only)"  || val == "Enter Pager # (numbers only)" || val == "Enter Pager # + PIN (numbers only)" || val == "-- Select Carrier" || val == "")
                {
                    alert("Device information is incorrect.");
                    return false;
                }
            }
            alert("Carrier is incorrect.");
            return false;
        }
   }
   if(val != null)
   {
       if (val == "Enter Cell # (numbers only)" || val == "Enter Email Address" || val == "Enter Fax # (numbers only)"  || val == "Enter Pager # (numbers only)" || val == "Enter Pager # + PIN (numbers only)" || val == "")
       {
            alert("Device address is incorrect.");
            return false;
       }
   }
   else
   {
        if (val1 == -1)
        {
           if(val1 != null)
           {
                alert("Carrier is incorrect.");
                return false;
           }
       }
   }
   
   return true;
}

// This function will remove the text that asks to Enter device in the Device Address textbox when the user clicks on it 
function RemoveDeviceLabel()
{
   if(document.getElementById(txtDeviceAddressClientID).value.indexOf('Enter') == 0)
   {
     document.getElementById(hidDeviceLabelClientID).value = document.getElementById(txtDeviceAddressClientID).value;
     document.getElementById(txtDeviceAddressClientID).value = "";
   }
}

// This function will remove the text that asks to Enter gateway in the Gateway textbox when the user clicks on it 
function RemoveGatewayLabel()
{
   if(document.getElementById(txtGatewayClientID).value.indexOf('Enter') == 0)
   {
     document.getElementById(hidGatewayLabelClientID).value = document.getElementById(txtGatewayClientID).value;
     document.getElementById(txtGatewayClientID).value = "";
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
//        return isNumericKey();                
//}


 /* This function will return true if input key in numberic else return false. This function used to accept numberic keys for textbox*/
function isNumericKey()
{
    var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

    if ( ((keyCode >= 48) && (keyCode <= 57))) //||  All numerics
    {
        return;                                  
    }
    
    window.event.returnValue = null;     
}

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
function isNumericKeyOrSpace(contolID)
{ 
    var control = txtDeviceAddressClientID;
    if(contolID != null)
    {
        control = contolID;
    }
    if(document.getElementById(control).value.length == 10)
    {
        return isSpace();        
    }  
    else
        return isNumericKeyStrokes();                
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
    
    