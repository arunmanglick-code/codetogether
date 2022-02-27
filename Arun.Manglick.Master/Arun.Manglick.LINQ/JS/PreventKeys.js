//document.onkeydown = CheckKey;
//document.onkeydown = function(){return CheckKey();} // Will work in IE, bit not in Mozilla. Reason being, as required in Mozilla, the event parameter is not passed automatically. Hence use below one.
document.onkeydown = function(event){return CheckKey(event);} 

keys =new Array();

keys["f112"] = 'f1';
keys["f113"] = 'f2';
keys["f114"] = 'f3';
keys["f115"] = 'f4';
keys["f116"] = 'f5'; 
keys["f117"] = 'f6';
keys["f118"] = 'f7';
keys["f119"] = 'f8';
keys["f120"] = 'f9';
keys["f121"] = 'f10';
keys["f122"] = 'f11'; 
keys["f123"] = 'f12';
keys["f27"] = 'Escape';

var keyPres;

function CheckKey(myEvent)
{    
    var Mozilla = myEvent;
    if(!myEvent) // myEvent is null for IE
    {
        myEvent = window.event; 
    }
    
    return CancelEvent(myEvent,Mozilla);   
}

function CancelEvent(myEvent,Mozilla)
{
    if (keys["f" + myEvent.keyCode])     // Skip check for f1 to f12
    {
       return Cancel(myEvent,Mozilla);   
    }
    
    if(myEvent.ctrlKey)
    {        
        if(myEvent.keyCode == 80 || myEvent.keyCode == 79 || myEvent.keyCode == 78) //Disable Ctrl + P, N, O
        {
           return Cancel(myEvent,Mozilla);    
        }
    }
    
    if(myEvent.keyCode == 8) //Disable Ctrl + P, N, O
    {
       return Cancel(myEvent,Mozilla);    
    }
}

function Cancel(myEvent,Mozilla)
{
    if(!Mozilla)
    {
        myEvent.keyCode = 505;
        return false;  
    }
    else
    {
        return false; // Ffox
    }    
}
