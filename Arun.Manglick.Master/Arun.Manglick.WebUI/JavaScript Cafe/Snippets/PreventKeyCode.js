function checkKeyCode(evt)
{

var evt = (evt) ? evt : ((event) ? event : null);
var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
if(event.keyCode==116)
{
evt.keyCode=0;
return false
}
else if(event.keyCode==8)
{
evt.keyCode=0;
return false
}

}
document.onkeydown=checkKeyCode;

----------------------


function checkKeyCode(e)
{
var key, node;
if (e)
{
key = e.which;
node = e.target;
}
else
{
key = event.keyCode;
node = event.srcElement;
}
if(key == 8 || key == 116)
{
if (e)
{
e.which = 0;
e.returnValue = false;
}
else
{
event.keyCode=0;
event.returnValue = false;
}
return false
}
return true;
}
document.onkeydown = checkKeyCode;