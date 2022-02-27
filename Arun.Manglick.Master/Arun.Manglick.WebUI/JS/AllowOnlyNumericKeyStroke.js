
function isNumericKeyStroke() 
{
    var keyCode = window.event.keyCode ? window.event.keyCode : window.event.charCode;
    if (((keyCode >= 48) && (keyCode <= 57))) // 1 to 9
    {
        return; // Allow
    }

    window.event.returnValue = null;
}