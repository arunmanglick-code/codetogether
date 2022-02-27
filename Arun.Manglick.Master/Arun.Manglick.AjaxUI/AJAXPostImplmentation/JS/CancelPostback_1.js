// JScript File

var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_initializeRequest(InitializeRequest);
prm.add_endRequest(EndRequest);        
function InitializeRequest(sender, args)
{
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm.get_isInAsyncPostBack())
    {
        args.set_cancel(true);
        ActivateAlertDiv('visible','Previous request is still in Progress');
    }
}

function EndRequest(sender, args)
{
   ActivateAlertDiv('hidden','Previous request is still in Progress');
}

function ActivateAlertDiv(visString,msg)
{            
     var adiv = $get('AlertDiv');
     var aspan = $get('AlertMessage');
     adiv.style.visibility = visString;
     aspan.innerHTML = msg;
}

if(typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();