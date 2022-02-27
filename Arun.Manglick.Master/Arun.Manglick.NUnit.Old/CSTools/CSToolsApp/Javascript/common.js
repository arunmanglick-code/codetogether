
// This function checks whether the entered key is alphabet only.
function isAlphabetKetStroke()
{
    if( ((event.keyCode >= 47) && (event.keyCode <= 64)) || 
        ((event.keyCode >= 33) && (event.keyCode <= 39)) || 
        ((event.keyCode >= 40) && (event.keyCode <= 46)) || 
        ((event.keyCode >= 91) && (event.keyCode <= 96)) || 
        ((event.keyCode >= 123) && (event.keyCode <= 126))) 
        event.returnValue = false;
}

/*
Check entered key is numeric or not
Called on onkeypress event only 
Use it instead of isNumericKeyStroke() function
*/
function isNumericKeyStrokes(hdnTextChangedControlID)
{ 
    var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
    
    //alert(event.keyCode);
    if (((keyCode >= 48) && (keyCode <= 57)))
    {
        if(hdnTextChangedControlID != null)
            document.getElementById(hdnTextChangedControlID).value = "true";  
        return;                                  
    }

    window.event.returnValue = null;     
}
/*
This function will return true if entered value is numeric or dot
Return true if condition satisfied
*/
function isNumericKeyStrokesOrDot(hdnTextChangedControlID)
{
   var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
    
   
    if (((keyCode >= 48) && (keyCode <= 57)) || keyCode == 46)
    {
        if(hdnTextChangedControlID != null)
            document.getElementById(hdnTextChangedControlID).value = "true";  
        return;                                  
    }

    window.event.returnValue = null;     
}



// This function checks whether the entered key is numeric or not
function isNumericKeyStroke()
    {
      var returnValue = false;
     var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;
     if ( ((keyCode >= 48) && (keyCode <= 57)) || // All numeric
               (keyCode ==  8) ||     // Backspace
               (keyCode == 13) ||
               (keyCode == 9) ||
               (keyCode == 23) ||  // Tab  
               (keyCode == 0) ||  // Tab     
               (keyCode == 46) || // delete key
               (keyCode == 16) || // shift key
               (keyCode == 17) || // ctrl key
               (keyCode == 35) || //End key
               (keyCode == 36) || //Home key
               (keyCode == 37) || // Left
               (keyCode == 39) || // right
               ((keyCode >= 96) && (keyCode <= 105)))     // Number pad numbers
             returnValue = true;

     if ( window.event.returnValue )
      window.event.returnValue = returnValue;

     return returnValue;
    }

// This function checks if the entered key is numeric or Dash
function isNumericKeyStrokeOrDash()
{
      var returnValue = false;
     var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;
     if ( ((keyCode >= 48) && (keyCode <= 57)) || // All numerics
               (keyCode ==  8) ||     // Backspace
               (keyCode == 13) ||
               (keyCode == 9) ||
               (keyCode == 23) ||  // Tab  
               (keyCode == 0) ||  // Tab     
               (keyCode == 46) || // delete key
               (keyCode == 16) || // shift key
               (keyCode == 17) || // ctrl key
               (keyCode == 35) || //End key
               (keyCode == 36) || //Home key
               (keyCode == 37) || // Left
               (keyCode == 39) || // right
               ((keyCode >= 96) && (keyCode <= 105)) || 
               (keyCode == 189) // dash
               )     // Number pad numbers
           {
            TextChanged();
             returnValue = true;
             }

     if ( window.event.returnValue )
      window.event.returnValue = returnValue;

     return returnValue;
}

// This function checks if the entered key is numeric or Decimal Point
function isNumericKeyStrokeOrDecimalpoint()
{
      var returnValue = false;
     var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;
     if ( ((keyCode >= 48) && (keyCode <= 57)) || // All numerics
               (keyCode ==  8) ||     // Backspace
               (keyCode == 13) ||
               (keyCode == 9) ||
               (keyCode == 23) ||  // Tab  
               (keyCode == 0) ||  // Tab     
               (keyCode == 46) || // delete key
               (keyCode == 16) || // shift key
               (keyCode == 17) || // ctrl key
               (keyCode == 35) || //End key
               (keyCode == 36) || //Home key
               (keyCode == 37) || // Left
               (keyCode == 39) || // right
               ((keyCode >= 96) && (keyCode <= 105)) || 
               (keyCode == 190) || // Decimal point
               (keyCode == 110) // Decimal point on Num pad
               )     // Number pad numbers
           {
             returnValue = true;
             }

     if ( window.event.returnValue )
      window.event.returnValue = returnValue;

     return returnValue;
}

// This function checks if the entered key is Alpha-numeric
function isAlphaNumericKeyStroke()
{
    var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
    
    if ( ((keyCode >= 48) && (keyCode <= 57))  ||   //All numerics
         ((keyCode >= 65) && (keyCode <= 90))  ||   // All CAPS alphabets
         ((keyCode >= 97) && (keyCode <= 122)) ||   // All alphabets
         (keyCode == 45)                       ||   // dash
         (keyCode == 39)                       ||   // SINGLE QUOTE
         (keyCode == 32)                       ||   // space
         (keyCode == 13)                       ||   // enter key
         (keyCode == 46) ) // dot
   {        
        return;                                  
    }
    
    window.event.returnValue = null;     
}

// This function checks if the entered key is Alpha-numeric
//it also allows space, dash, underscore, single quote and dot
function isAlphaNumericKeyStrokeOrUnderscore()
{
    var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
   // alert(keyCode1)
    if ( ((keyCode >= 48) && (keyCode <= 57))  ||   //All numerics
         ((keyCode >= 65) && (keyCode <= 90))  ||   // All CAPS alphabets
         ((keyCode >= 97) && (keyCode <= 122)) ||   // All alphabets
         (keyCode == 45)                       ||   // dash
         (keyCode == 39)                       ||   // SINGLE QUOTE
         (keyCode == 32)                       ||   // space
         (keyCode == 13)                       ||   // enter key
         (keyCode == 46)                       ||   // dot
         (keyCode == 95))                           // underscore
   {        
        return ;                                  
    }
   
    window.event.returnValue = null;     
}



// This function will allow only space on the given location, to add Skytel Pager device address at 11 th position 
// there must be space only.
function isSpace()
    {
      var returnValue = false;
     var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

     if ((keyCode == 32) || // Space
              (keyCode ==  8) ||     // Backspace 
             (keyCode == 46) || // delete key
             (keyCode == 35) || //End key
               (keyCode == 36)  //Home key               
             )
             returnValue = true;

     if ( window.event.returnValue )
      window.event.returnValue = returnValue;

     return returnValue;
    }


//Message readback confirmation script and timer script to open the confirmation dialog.

var timerID = 0;
var tStart  = null;
//calls the Showpopup function to show the readback confirmation dialog.
function UpdateTimer(sURL) {
   if(timerID) {
      clearTimeout(timerID);
      clockID  = 0;
   }

   if(!tStart)
      tStart   = new Date();

   var   tDate = new Date();
   var   tDiff = tDate.getTime() - tStart.getTime();

   tDate.setTime(tDiff);
   timerID = setTimeout("UpdateTimer()", 3000);
   ShowPopup(sURL);
}

//timer start function. sets the timeout as 3 sec.
function Start(sURL) {
  tStart   = new Date();
  timerID  = setTimeout("UpdateTimer('" + sURL + "')", 3000);
   
}

//Stops the timer
function Stop() {
   if(timerID) {
      clearTimeout(timerID);
      timerID  = 0;
   }

   tStart = null;
}

//Returns the string by removing the spaces at the start and end. 
function trim(stringToTrim) 
{
	return stringToTrim.replace(/^\s+|\s+$/g,"");
}

// Following function opens proper help window on click of F1
function CallHelp(mapId)
{
		var endPoint = mapId.indexOf("?");
        var PageName = "";
        var helpFilePath = 'Help/CS_Tools/cstools.html?page=';
        if(endPoint != -1)
        {
            PageName = mapId.substring(0,endPoint);
        }
        else
        {
            PageName = mapId;
        }
        
		try
		{
		    switch(PageName)
			{
			    case "message_center.aspx" : helpFilePath = helpFilePath + "7-message center.htm";
			                        break;
			    case "message_details.aspx" : helpFilePath = helpFilePath + "9-message details.htm";
			                          break;  
                case "mark_message_received.aspx" : helpFilePath = helpFilePath + "12-mark as received.htm";
			                        break;	
                case "message_forward.aspx" : helpFilePath = helpFilePath + "11forward message.htm";
                                    break;
                case "grammar.aspx" : helpFilePath = helpFilePath + "oc grammars.htm";
			                        break;
                case "test_result_definitions.aspx" : helpFilePath = helpFilePath + "lab test definition.htm";
			                        break;			                        			 
			    case "voiceover_utility.aspx" : helpFilePath = helpFilePath + "oc voice over utility.htm";			         
			                        break;			                          
			    case "assign_test.aspx" : helpFilePath = helpFilePath + "lab test setup.htm";			         
			                        break;
                case "device_notification_error.aspx" : helpFilePath = helpFilePath + "device notification error.htm";			         
			                        break;	
			    case "vuiErrorReport.aspx" : helpFilePath = helpFilePath + "vui errors.htm";			         
			                        break;	
			    case "institution_information.aspx" : helpFilePath = helpFilePath + "institute information.htm";			         
			                        break;	
			    case "add_institution.aspx" : helpFilePath = helpFilePath + "add institution.htm";			         
			                        break;	                      		                          
			    case "edit_institution.aspx" : helpFilePath = helpFilePath + "edit institution.htm";			         
			                        break;	   
			    case "role_task.aspx" : helpFilePath = helpFilePath + "manage roles and task.htm";			         
			                        break;	   
			    case "assign_tasks.aspx" : helpFilePath = helpFilePath + "assign tasks.htm";			         
			                        break;	   
			    case "add_directory.aspx" : helpFilePath = helpFilePath + "add-edit directory.htm";			         
			                        break;                                                                      
                case "add_nurse_directory.aspx" : helpFilePath = helpFilePath + "add-edit nurse directory.htm";			         
			                        break;
                case "group_monitor.aspx" : helpFilePath = helpFilePath + "group monitor.htm";			         
			                        break;	
			    case "group_maintenance.aspx" : helpFilePath = helpFilePath + "group maintenance.htm";			         
			                        break;	
			    case "group_preferences.aspx" : helpFilePath = helpFilePath + "group preferences.htm";	
			                        break;	
                case "group_maintenance_findings.aspx" : helpFilePath = helpFilePath + "findings and notifications.htm";	
			                        break;	
			    case "add_findings.aspx" : helpFilePath = helpFilePath + "add new findings.htm";	
			                        break;	
                case "add_group.aspx" : helpFilePath = helpFilePath + "add group.htm";	
			                        break;	
                case "add_subscriber.aspx" : helpFilePath = helpFilePath + "add new subscriber.htm";	
			                        break;
                case "user_profile.aspx" : helpFilePath = helpFilePath + "edit subscriber.htm";	
			                        break;	
			    case "user_management.aspx" : helpFilePath = helpFilePath + "Manage Roles and Task.htm";	
			                        break;	
			    case "add_OC.aspx" : helpFilePath = helpFilePath + "Add Ordering Clinician.htm";	
			                        break;  
			    case "edit_OC.aspx" : helpFilePath = helpFilePath + "16-edit ordering clinician details.htm";	
			                        break;
                case "add_CC_agent.aspx" : helpFilePath = helpFilePath + "3-add or edit call center agent.htm";	
                                    break;  
                case "add_callcenter.aspx" : helpFilePath = helpFilePath + "addedit_call_center.htm";	
			                        break; 
                case "callCenter_setup.aspx" : helpFilePath = helpFilePath + "call_center_edit_setup.htm";	
                break; 
                case "custom_notifications.aspx" : helpFilePath = helpFilePath + "custom_notification.htm";	
                break;                 
			                        		                        			                        		                        			                        
			} 
			
			//Closing previously opened help window if any.			
			try
			{
			  if(mapId != "DoNotOpen")
               {
                window.parent.frames[0].document.getElementById("varHelp").value = "false";
				CloseHelp();
			   }
			}
			catch(e)
			{}
			
			//Following line opens help window.
			if(window.dialogHeight)
			{
				//Parent window opening help is also an Pop Up
				popupWindow = window.parent.open(helpFilePath,"dp","toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=1,width=600,height=450,left=180,top=180");							
			}
			else
			{
				//Parent window opening help is a normal window									
					popupWindow = window.open(helpFilePath,"dp","toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=1,width=600,height=450,left=180,top=180");			
			}
		}
		catch(e)
		{}		
		
	}
	
	//Following function is used to close previously opened help window when page is unloaded
    function CloseHelp()
    {
	    try
        {
		    popupWindow.close();
		    popupWindow = null;
	    }
	    catch(e)
	    {}
    }
    function ValidateCharacters(Control, ControlName)
{   
    var increment;
    var ctrl = document.getElementById(Control);
    var s = ctrl.value;
    var bag = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ .'"; 
    for (increment = 0; increment < s.length; increment++)
    {   
        // Check that current character isn't whitespace.
        var c = s.charAt(increment);
        if (bag.indexOf(c) != -1) 
        {
            //Do nothing.
        }
        else
        {
            alert("Please Enter Valid" + " " + ControlName) + ".";
            ctrl.focus();
            return false;
        }
    }        
}

/*function ValidateCharactersForUnit(Control, ControlName)
{   
    var increment;
    var ctrl = document.getElementById(Control);
    var s = ctrl.value;
    var bag = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ -0123456789"; 
    for (increment = 0; increment < s.length; increment++)
    {   
        // Check that current character isn't whitespace.
        var c = s.charAt(increment);
        if (bag.indexOf(c) != -1) 
        {
            //Do nothing.
        }
        else
        {
            alert("Please Enter Valid" + " " + ControlName) + ".";
            ctrl.focus();
            return false;
        }
    }        
}*/
/*
// This function checks whether the entered key is not a wild character(allows only alphabets and . ')
function ValidateCharacters()
{
  var returnValue = true;
  var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

  if(((keyCode >= 47) && (keyCode <= 64)) ||
    ((keyCode >= 33) && (keyCode <= 38)) || 
    ((keyCode >= 40) && (keyCode <= 45)) ||
    ((keyCode >= 91) && (keyCode <= 96)) || 
    ((keyCode >= 123) && (keyCode <= 126)))
 {
    return false;
 }     

 if ( window.event.returnValue )
  window.event.returnValue = returnValue;

 return returnValue;
}
// This function checks whether the entered key is not a wild character(allows only alphabets, numbers and hyphen)
function ValidateCharactersForUnit()
{
  var returnValue = true;
  var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;
 
  if((keyCode == 46) ||(keyCode == 47) || 
    ((keyCode >= 33) && (keyCode <= 44)) ||    
    ((keyCode >= 58) && (keyCode <= 64)) || 
    ((keyCode >= 91) && (keyCode <= 96)) || 
    ((keyCode >= 123) && (keyCode <= 126)))
 {
    return false;
 }     

 if ( window.event.returnValue )
  window.event.returnValue = returnValue;

 return returnValue;
}
*/


/********************/

// Declaring valid date character, minimum year and maximum year
var dtCh= "/";
var minYear=1900;
var maxYear=2100;

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

function isValidDate(dtStr){
    var errorMessage ="";
	var daysInMonth = DaysArray(12)
	var pos1=dtStr.indexOf(dtCh)
	var pos2=dtStr.indexOf(dtCh,pos1+1)
	var strMonth=dtStr.substring(0,pos1)
	var strDay=dtStr.substring(pos1+1,pos2)
	var strYear=dtStr.substring(pos2+1)
	strYr=strYear
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	}
	month=parseInt(strMonth)
	day=parseInt(strDay)
	year=parseInt(strYr)
	if (pos1==-1 || pos2==-1){
		return " Format should be mm/dd/yyyy";
	}
	if (strMonth.length<1 || month<1 || month>12){
		//alert("Please enter a valid month")
		return "Please enter a valid month";
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		//alert("Please enter a valid day")
		return "Please enter a valid day";
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		//alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear)
		return "Please enter a valid 4 digit year between "+minYear+" and "+maxYear;
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		//alert("Please enter a valid date")
		return "Please enter a valid date";
	}
return "";
}

function ValidateDate(txtDate)
{
 	return isValidDate(txtDate.value);
}

/******************* Get Mouse Postion *********************************/

// Detect if the browser is IE or not.
// If it is not IE, we assume that the browser is NS.
var IE = document.all?true:false

// If NS -- that is, !IE -- then set up for mouse capture
if (!IE) document.captureEvents(Event.MOUSEMOVE)

// Set-up to use getMouseXY function onMouseMove
document.onmousemove = getMouseXY;

// Temporary variables to hold mouse x-y pos.s
var currentMouseXPosition = 0
var currentMouseYPosition = 0

// Main function to retrieve mouse x-y pos.s

function getMouseXY(e) {
    try
    {
      if (IE) { // grab the x-y pos.s if browser is IE
        currentMouseXPosition = event.clientX + document.body.scrollLeft
        currentMouseYPosition = event.clientY + document.body.scrollTop
      } else {  // grab the x-y pos.s if browser is NS
        currentMouseXPosition = e.pageX
        currentMouseYPosition = e.pageY
      }  
      // catch possible negative values in NS4
      if (currentMouseXPosition < 0){currentMouseXPosition = 0}
      if (currentMouseYPosition < 0){currentMouseYPosition = 0}  
      
      return true
     }
     catch(exp)
     {
        return false;
     }     
}
/******************* Get Mouse Postion *********************************/


/**********************************************************************/
// Functions for IE to get position of an object
function getPageOffsetLeft (el) {
	if (el != null)
	{
	    var ol=el.offsetLeft;
	    while ((el=el.offsetParent) != null) { ol += el.offsetLeft; }
	    return ol;
	}
	}
function getWindowOffsetLeft (el) {
	return getPageOffsetLeft(el) - document.body.scrollLeft;
	}	
function getPageOffsetTop (el) {
	if (el != null)
	{
	    var ot=el.offsetTop;
	    while((el=el.offsetParent) != null) { ot += el.offsetTop; }
	    return ot;
	}
	}
function getWindowOffsetTop (el) {
	return getPageOffsetTop(el) - document.body.scrollTop;
	}
	
var resizeResetCounter = 0;
//Set height of grid as per page resolution to fit to scrren	
function setHeightOfGrid(controlID, adjustmentValue, maxLength)
{   
    resizeResetCounter++;
    //Some time calling this function on 'onresize' it goes in loop so used this resizeResetCounter variable to conrol it
    if(resizeResetCounter >1000)
    {
        resizeResetCounter = 0;
    }
    
    if(document.getElementById(controlID) == null)
        return 0;
    
    var calculatedHeight = document.body.offsetHeight - adjustmentValue - getPageOffsetTop(document.getElementById(controlID));
    var controlHeight = document.getElementById(controlID).offsetHeight + 2;
   
    if(calculatedHeight < 50)
    {
        if(controlHeight > 25)
            calculatedHeight = 50;
        else
            calculatedHeight = 25;
    }
        
    if(controlHeight < 1)
        controlHeight = 25;
       
    if(maxLength != null && maxLength < controlHeight)//(maxLength < calculatedHeight || maxLength < controlHeight))
       return maxLength;
    
    if(calculatedHeight > controlHeight)
        return controlHeight;
    else
        return calculatedHeight;
}

/*************************************************************************/

// Check Maximun length for textbox if length > given length then consider first 'length' char only
function CheckMaxLength(controlId, length)
{
    var text = document.getElementById(controlId).value;
    //if(text.length > 255)
    //{
    //  document.getElementById(controlId).value = text.substring(0, length);
    //}
    var text = document.getElementById(controlId).value;
    var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;
    if ( (keyCode ==  8) || // Backspace
       (keyCode == 9) ||
       (keyCode == 23) ||  // Tab  
       (keyCode == 0) ||  // Tab     
       (keyCode == 46) || // delete key
       (keyCode == 16) || // shift key
       (keyCode == 17) || // ctrl key
       (keyCode == 35) || //End key
       (keyCode == 36) || //Home key
       (keyCode == 37) || // Left
       (keyCode == 39))    // right
    {
        return true;
    }
    else if(keyCode == 13) // Enter key
    {
        if(text.length >= length)
            return false;
        else
            return true;
    }
    else if(keyCode == 86) // Ctrl + V : Paste
    {
        var evtobj=window.event? event : e
        if (evtobj.ctrlKey)
            if(text.length >= length)
            {
                document.getElementById(controlId).value = text.substring(0, length);
                return false;
            }
            else
                return true;
    }
    else
    {  
        if(text.length >= length)
        {
            //document.getElementById(controlId).value = text.substring(0, length);
            return false;
        } 
    }
} 

function PageLoading(isNotProcessing, fixTableName, processingTableName, imageWidth, isChildForm) // function for displaying loding icon
{
    if(isChildForm == null)
    {
        if (isNotProcessing ==1)
        {
            try
            {
                document.getElementById(fixTableName).style.visibility='visible';
                document.getElementById(processingTableName).style.visibility='hidden';
                document.getElementById(processingTableName).style.display='none';
                document.getElementById(fixTableName).style.display='inline';
             }
           
             catch(e){}
         }
         else
         {
            try
            {
                document.getElementById(processingTableName).style.visibility='visible';
                document.getElementById(fixTableName).style.visibility='hidden';
                document.getElementById(fixTableName).style.display='none';
                document.getElementById(processingTableName).style.display='block';
                document.getElementById(processingTableName).style.width=imageWidth;
                return true;
             }
           
             catch(e){}
         }
     }
     else
     {
        if (isNotProcessing ==1)
        {
            try
            {
                window.parent.document.getElementById(fixTableName).style.visibility='visible';
                window.parent.document.getElementById(processingTableName).style.visibility='hidden';
                window.parent.document.getElementById(processingTableName).style.display='none';
                window.parent.document.getElementById(fixTableName).style.display='inline';
             }
           
             catch(e){}
         }
         else
         {
            try
            {
                window.parent.document.getElementById(processingTableName).style.visibility='visible';
                window.parent.document.getElementById(fixTableName).style.visibility='hidden';
                window.parent.document.getElementById(fixTableName).style.display='none';
                window.parent.document.getElementById(processingTableName).style.display='block';
                window.parent.document.getElementById(processingTableName).style.width=imageWidth;
                return true;
             }
           
             catch(e){}
         }
     }
}