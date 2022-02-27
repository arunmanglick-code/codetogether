function CheckBrowser1()
{
    if(navigator.appName == "Microsoft Internet Explorer")
    {
        alert('Microsoft Internet Explorer');
    }
    else
    {
        alert('Mozilla Firefox');
    }
}
// -------------------------------------------------------------------
function CheckBrowser2() 
{
    var agt=navigator.userAgent.toLowerCase();
    var browser;
    
    if (agt.indexOf("opera") != -1) browser = 'Opera';
    else if (agt.indexOf("staroffice") != -1) browser = 'Star Office';
    else if (agt.indexOf("webtv") != -1) browser = 'WebTV';
    else if (agt.indexOf("beonex") != -1) browser = 'Beonex';
    else if (agt.indexOf("chimera") != -1) browser = 'Chimera';
    else if (agt.indexOf("netpositive") != -1) browser = 'NetPositive';
    else if (agt.indexOf("phoenix") != -1) browser = 'Phoenix';
    else if (agt.indexOf("firefox") != -1) browser = 'Firefox';
    else if (agt.indexOf("safari") != -1) browser = 'Safari';
    else if (agt.indexOf("skipstone") != -1) browser = 'SkipStone';
    else if (agt.indexOf("msie") != -1) browser = 'Internet Explorer';
    else if (agt.indexOf("netscape") != -1) browser = 'Netscape';
    else if (agt.indexOf("mozilla/5.0") != -1) browser = 'Mozilla';    
    else if (agt.indexOf('\/') != -1) 
    {
        if (agt.substr(0,agt.indexOf('\/')) != 'mozilla') 
        {
            browser = navigator.userAgent.substr(0,agt.indexOf('\/'));
        }
        else 
            browser = 'Netscape';
    } 
    else if (agt.indexOf(' ') != -1)
        browser = navigator.userAgent.substr(0,agt.indexOf(' '));
    else 
        browser = navigator.userAgent;
    
    alert(browser);  
        
}

// -------------------------------------------------------------------