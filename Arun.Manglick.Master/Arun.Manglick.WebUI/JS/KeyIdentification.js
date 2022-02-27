
//document.onkeydown = CheckKey;
//document.onkeypress = CheckKey;

document.onkeydown = function(event){CheckKey(event);}
//document.onkeypress = function(event){CheckKey(event);}

function CheckKey(myEvent)
{    
    //debugger;
    if(!myEvent) 
    {        
        myEvent = window.event;
    }
    
    if(myEvent.shiftKey)
    {
        alert('Shift Pressed');
    }
    else if(myEvent.ctrlKey)
    {
        alert('Ctrl Pressed');
    }
    else if(myEvent.altKey)
    {
       alert('Alt Pressed');
    }
    else
    {
       alert('Some Other Key Pressed with Key Code - ' + myEvent.keyCode);       
    }
}
// -------------------------------------------------------------------
// More explanatory
// -------------------------------------------------------------------
function CheckKeyProof(myEvent)
{
    var salt = 'In FFox :';
    if(!myEvent)
    {
       // This is null in IE, but only when the event is not passed as a parameter at the time of calling. 
       // i.e Called as onkeydown='CheckKey();' instead of onkeydown='CheckKey(event);'
        
        myEvent = window.event; // IE does not understand passed event as parameter
        var salt = 'In IExplorer :';
    }
    
    if(myEvent.shiftKey)
    {
        alert(salt + ' Shift Pressed');
    }
    else if(myEvent.ctrlKey)
    {
       alert(salt + ' Ctrl Pressed');
    }
    else if(myEvent.altKey)
    {
       alert(salt + ' Alt Pressed');
    }
    else
    {
        alert(salt + ' Some Other Key Pressed');
    }
}


// -------------------------------------------------------------------

