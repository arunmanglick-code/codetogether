
//-------------------------------------------------
// Script - To Check if the DTO is Dirty
//-------------------------------------------------
var isDirty = false;
//Function to set isDirty flag for the dirty DTO.
function HandleChangeEvent()
{
    isDirty = true;
}
//Function to set isDirty flag for the dirty DTO.
function SaveEventCalled()
{
    isDirty = false;
}
//Funtion to check if there is dirty DTO on page.
function CheckDirtyFlagBeforeRedirect()
{
    if(isDirty)
    {
        if(confirm('Data is unsaved. Do you want to continue?'))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

//// START - Code for Grid (Div) resize on resizing window
//function GetWindowHeight() 
//{
//    var myHeight = 0;
//    if( typeof( window.innerWidth ) == 'number' ){ 
//        //Non-IE
//        myHeight = window.innerHeight;
//    }
//    else {
//        if ( document.documentElement && ( document.documentElement.clientWidth ||    document.documentElement.clientHeight ) ){ 
//        //IE 6+ in 'standards compliant mode'
//        myHeight = document.documentElement.clientHeight;
//    }
//    else {
//        if ( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
//        { 
//            //IE 4 compatible
//            myHeight = document.body.clientHeight;
//        }
//    }
//    }
//    return myHeight;     
//}
//function SetPlaceHolderWrapperHeight() {
//    return wrapperHeight = GetWindowHeight() - 4;  //the height of the lower edge of the browser window
//}    
//// END - Code for Grid (Div) resize on resizing window

//// START - Code for the Grid View Selected row color change and method for Audit Trail
//var oldgridSelectedColor;
//var oldElement;

//function setMouseOverColor(element) 
//{
//    oldgridSelectedColor = element.style.backgroundColor;
//    element.style.backgroundColor='#CAD2D4';
//    if(oldElement != null)
//    {
//        setMouseOutColor(oldElement)
//    }
//    oldElement = element;
//}

//function setMouseOutColor(element) 
//{
//    element.style.backgroundColor=oldgridSelectedColor;
//    element.style.textDecoration='none';
//}

//function SetSelectedRow(rowId,hiddenCtrlId)
//{
//    var hiddenRowId = document.getElementById(hiddenCtrlId);
//    hiddenRowId.value = rowId - 1;
//}
//function ShowAuditTrail()
//{   
//    window.open('HierarrchichalAuditReport.aspx', '', 'fullscreen=no,menubar=no,scrollbars=yes,status=yes,titlebar=no,toolbar=no,resizable=yes,location=no');
//}
//// END - Code for the Grid View Selected row color change and method for Audit Trail

////=================================================================================
////	C H A N G E    H A N D L E R      F U N C T I O N S
////=================================================================================

////Set a central handler for all onchange events.
////If onChangeHandler is passed, it will be called back with
////the elemet that has changed.
////Modifed RPM 05/25/2004: added keypress handler for text inputs 
////and mousedown handler for checkboxes so we can set the dirty flag
////KLC 09/13/2004
////account for radio buttons, delete key, and space bar
//function setOnChangeHandlers(CurrentForm, onChangeHandler, onKeyPressHandler)
//{	   
//	onChangeCallback = onChangeHandler;
//	onKeyPressCallback = onKeyPressHandler;
//	
//	for (var i = 0; i < CurrentForm.elements.length; i++)
//	{
//		// Call onChangeCallback except for checkboxes to prevent two passes 
//		// because are handled on the mousedown event :
//		if (CurrentForm.elements[i].type != "checkbox")
//			CurrentForm.elements[i].onchange = new Function('valueChange(this)');
//		
//		//capture text box key strokes so we can set the dirty flag
//		if(CurrentForm.elements[i].type=="text" || CurrentForm.elements[i].type=="textarea")
//		{
//			CurrentForm.elements[i].onkeypress = new Function('textInputKeyPress(this)');
//		}		

//		//capture check box clicks so we can set the dirty flag
//		if(CurrentForm.elements[i].type=="checkbox" || CurrentForm.elements[i].type=="radio")
//		{
//			CurrentForm.elements[i].onmousedown = new Function('checkBoxMouseDown(this)');
//		}		
//	
//	}
//}

//function checkBoxMouseDown(changedElement)
//{
//	setDirty();
//	
//	//if an onChange callback function was passed to setOnChangeHandlers,
//	//call it!	
//	if(onChangeCallback)
//		return onChangeCallback(changedElement);

//	return true;
//}

//function textInputKeyPress(changedElement)
//{
//	setDirty();

//	//if an onChange callback function was passed to setOnChangeHandlers,
//	//call it!	
//	if(onKeyPressCallback)
//		onKeyPressCallback(changedElement);
//}

//function textInputKeyDown(changedElement)
//{
//	if(changedElement.onkeydown.keyCode == KEY_SPACEBAR || changedElement.onkeydown.keyCode == KEY_DELETE || changedElement.onkeydown.keyCode == KEY_BACKSPACE)
//	{
//		setDirty();

//		//if an onChange callback function was passed to setOnChangeHandlers,
//		//and it's a spacebar hitting a checkbox, call it!
//		if(changedElement.type == "checkbox" && changedElement.onkeydown.keyCode == KEY_SPACEBAR)	
//		{
//			if(onChangeCallback)
//				onChangeCallback(changedElement);
//		}
//	}
//}

//function setDirty()
//{
//	isDirty = true;
//	
//	if (frames.name=="contentArea")
//	{
//		parent.parent.frames.isDirty = true;
//	}
//}


//function clearDirty()
//{
//	isDirty = false;
//	
//	if (frames.name=="contentArea")
//	{
//		parent.parent.frames.isDirty = false;
//	}

//}

////Central onchange handler for all inputs.
//function valueChange(changedElement)
//{
//	setDirty();
//	
//	//if an onChange callback function was passed to setOnChangeHandlers,
//	//call it!	
//	if(onChangeCallback)
//		return onChangeCallback(changedElement);

//	return true;
//}



//Method to check the length of multiline text box
function CheckLength(Object,count,myEvent)
{
    if (Object.value.length < count)
    {
        return true;
    }
    else
    {
        if(navigator.appName == "Microsoft Internet Explorer")
        {
            if ((window.event.keyCode >= 37 && window.event.keyCode <= 40) || (window.event.keyCode==8) || (window.event.keyCode==46))
            {
                window.event.returnValue = true;
            }
            else
            {
                window.event.returnValue = false;
            }
        }
        else // Firefox
        {
            if ((myEvent.which>=37 && myEvent.which<=40) || (myEvent.which==8) || (myEvent.which==46))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}