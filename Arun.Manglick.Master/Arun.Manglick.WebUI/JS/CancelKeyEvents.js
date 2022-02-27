
document.onkeydown = CheckKey;
document.onkeypress = CheckKey;

//document.onkeydown = function(){CheckKey();}
//document.onkeypress = function(){CheckKey();}

// Cancel Print (Ctrl + P)
// --------------------------------------
function CheckKey(myEvent)
{    
    if(!myEvent) 
    {        
        myEvent = window.event; 
    }    
            
    if(myEvent.ctrlKey)
    {
       if(myEvent.keyCode == 80) 
       {
           if(navigator.appName == "Microsoft Internet Explorer")
           {
                myEvent.keyCode = 505;
               //myEvent.cancelBubble = true;  // Does not work
               //myEvent.returnValue = false;  // Does not work
                return false;
           }
           else
           {
              return false;
           }           
       }
    }
}
// --------------------------------------