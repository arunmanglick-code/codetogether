
function OpenFullWindow(url)
{
   window.open(url, '', 'fullscreen=yes,menubar=no,scrollbars=yes,status=yes,titlebar=no,toolbar=no,resizable=yes,location=no,top=0,left=0');
}

function OpenSmallWindow(url)
{    
    window.open(url, '', 'fullscreen=no,menubar=no,scrollbars=yes,status=yes,titlebar=no,toolbar=no,resizable=yes,location=no,top=0,left=0');
}

// -----------------------------------------------------
// Open full Window - Disabled Browser Functionality
// -----------------------------------------------------
function Shoot() 
{
    var str = "left=0,screenX=0,top=0,screenY=0";
    if (window.screen) {
        var ah = screen.availHeight - 30;
        var aw = screen.availWidth - 10;
        str += ",height=" + ah;
        str += ",innerHeight=" + ah;
        str += ",width=" + aw;
        str += ",innerWidth=" + aw;
        str += ",resizable,scrollbars=yes";
    }
    else {
        str += ",resizable"; // so the user can resize the window manually
    }
    window.opener = self;
    var win = window.open("Home.aspx", "full", str);
    window.opener.close();
    win.focus();
}
// -----------------------------------------------------      