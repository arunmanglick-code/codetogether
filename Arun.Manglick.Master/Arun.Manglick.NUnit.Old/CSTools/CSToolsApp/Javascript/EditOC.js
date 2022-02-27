// JScript File
var textChanged = false;
    function isNumericKeyStroke()
    {
     var returnValue = false;
     var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;
     document.getElementById(btnAddClientID).disabled =false;
     if ( ((keyCode >= 48) && (keyCode <= 57)) || // All numerics
               (keyCode ==  8) ||     // Backspace
               (keyCode == 13) ||
               (keyCode == 9) ||
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
    
    //Will open the popup window for Report when user clicks on the View Profile as PDF link.
    function SaveReport(url, name)
    {
      if ( window.parent.screen.width == 800  && window.parent.screen.height == 600 )
			{
				var refPopupWindow	= window.open(url, name, 'toolbar=no,location=no,status=no,menubar=no,scrollbars=Yes,resizable=yes,titlebar=no,top=30,height=560,left=30,width=750'); //800*600 resolution
					 
				if ((refPopupWindow) && (typeof(refPopupWindow) == 'object') && (refPopupWindow.focus))
				    refPopupWindow.focus();
				else
				    alert('Pop up is Blocked,Cannot open Window.');
					
			}
			else
			{	
				var refPopupWindow	= window.open(url, name, 'toolbar=no,location=no,status=no,menubar=no,scrollbars=Yes,resizable=yes,titlebar=no,top=30,height=635,left=30,right=50,width=840'); //1024  resolution
				if ((refPopupWindow) && (typeof(refPopupWindow) == 'object') && (refPopupWindow.focus))
			        refPopupWindow.focus();
			    else
			        alert('Pop up is Blocked,Cannot open Window.');
			}
        
    }
    
    //Sets the flag textChanged to true if the text of any textbox is changed.
    function UpdateProfile()
    {
       document.getElementById(textChangedClientID).value = "true";
       document.getElementById(btnAddClientID).disabled =false;
       textChanged = true;
    }
    
    //Sets the flag hdnCTChanged to true if the Clinical Team (CT) data is changed.
    function ctDataChanged()
    {       
        document.getElementById(hdnCTChangedClientID).value = "true";
        document.getElementById(btnAddClientID).disabled =false;
        return;
    }
    
    //Navuage to given url
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
    
    function onCancelClick(searchText, searchValue)
    {
         var url ="";         
         if(searchText != null && searchText == "True") 
         {
            if(searchValue != null)
            {
                url = "./directory_maintenance.aspx?StartingWith=" + searchValue;
                Navigate(url);
            } 
            else
            {
                url = "./directory_maintenance.aspx?StartingWith=";
                Navigate(url);
            }
         } 
         else
         {
            var error = false;
            try
            {
            if(txtLastNameClientID){}
            }
            catch(_error)
            {
                error =true;
                url = "./directory_maintenance.aspx?StartingWith=";
                Navigate(url);
            }
            if(error == false)
            {
                url = "./directory_maintenance.aspx?StartingWith=" + document.getElementById(txtLastNameClientID).value.substring(0, 1);
                Navigate(url);
            }
         }
         return false;
    }  
    
    
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
    
    //Sets the flag textChanged to false when user clicks on Save button as it should not ask confirmation message even
    //if user clicks on Save button. 
    function ChangeFlag()
    {        
        document.getElementById(textChangedClientID).value = "false";
    }   


var mapId  = "edit_rp.aspx";
