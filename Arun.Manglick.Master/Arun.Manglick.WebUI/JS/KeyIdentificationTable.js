
// set page-wide event listeners
// -----------------------------------
document.onkeydown = showDown;
document.onkeypress = showPress;
document.onkeyup = showUp;
// -----------------------------------


// array of table cell ids
// -----------------------------------
var tCells = ["downKey", "pressKey", "upKey", "downChar", "pressChar", 
"upChar", "keyTarget", "character", "downShift", "pressShift",
"upShift", "downAlt", "pressAlt", "upAlt", "downMeta", "pressMeta",
"upMeta", "downCtrl", "pressCtrl", "upCtrl"];
// -----------------------------------
  
  
// clear table cells for each key down event
// -----------------------------------
function clearCells() {
    for (var i = 0; i < tCells.length; i++) {
        document.getElementById(tCells[i]).innerHTML = "&mdash;";
    }
}
// -----------------------------------


// display target node's node name
function showTarget(event) 
{     
    var node;
    if (!event)  // For IE
    {
        // This is null in IE, but only when the event is not passed as a parameter at the time of calling. 
        // i.e Called as onkeydown='showTarget();' instead of onkeydown='showTarget(event);'
        
        event = window.event;
    }
    
    if(navigator.appName == "Microsoft Internet Explorer")
    {
        node = event.srcElement;
    }
    else
    {
       node = event.target;
    }
     
    if (node) 
    {
        document.getElementById("keyTarget").innerHTML =  node.nodeName;
    }
}
// -----------------------------------


var nbrEvent = 0;

// decipher key down codes
function showDown(event) 
{
    clearCells();
    if (!event) // For IE
    {
        // This is null in IE, but only when the event is not passed as a parameter at the time of calling. 
        // i.e Called as onkeydown='showDown();' instead of onkeydown='showDown(event);'
        
        event = window.event;
    }
    var text = "";
    document.getElementById("downKey").innerHTML = event.keyCode;
    if (event.charCode) 
    {
        document.getElementById("downChar").innerHTML = event.charCode;
    }
    document.getElementById("downShift").innerHTML = event.shiftKey? "true" : "false";
    document.getElementById("downCtrl").innerHTML = event.ctrlKey? "true" : "false";
    document.getElementById("downAlt").innerHTML =  event.altKey? "true" : "false";
    document.getElementById("downMeta").innerHTML = event.metaKey? "true" : "false";
}
  
// decipher key press codes
function showPress(event) 
{
    if (!event) // For IE
    {
        // This is null in IE, but only when the event is not passed as a parameter at the time of calling. 
        // i.e Called as onkeydown='showPress();' instead of onkeydown='showPress(event);'
        
        event = window.event;
    }
    var text = "";
    document.getElementById("pressKey").innerHTML = event.keyCode;
    if (event.charCode) 
    {
        document.getElementById("pressChar").innerHTML = event.charCode;
    }
    showTarget(event);
    var charCode = (event.charCode) ? event.charCode : event.keyCode;
    
    document.getElementById("character").innerHTML = String.fromCharCode(charCode);
    document.getElementById("pressShift").innerHTML = event.shiftKey? "true" : "false";
    document.getElementById("pressCtrl").innerHTML = event.ctrlKey? "true" : "false";
    document.getElementById("pressAlt").innerHTML = event.altKey? "true" : "false";
    document.getElementById("pressMeta").innerHTML =  event.metaKey? "true" : "false";
}
  
// decipher key up codes
function showUp(event) 
{
    if (!event) // For IE
    {
        event = window.event;
    }
    var text = "";
    document.getElementById("upKey").innerHTML = event.keyCode;
    if (event.charCode) 
    {
        document.getElementById("upChar").innerHTML = event.charCode;
    }
    document.getElementById("upShift").innerHTML = event.shiftKey? "true" : "false";
    document.getElementById("upCtrl").innerHTML = event.ctrlKey? "true" : "false";
    document.getElementById("upAlt").innerHTML = event.altKey? "true" : "false";
    document.getElementById("upMeta").innerHTML = event.metaKey? "true" : "false";
    return false;
} 


function displayObject(obj) {
  var s = typeof(obj) + "\n";
  for (var i in obj) {
    s = s + "obj[" + i + "] + " + obj[i] + "\n";
  }
  alert(s);
}
