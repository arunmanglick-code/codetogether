    /***********************************************
    * Disable Text Selection script- Â© Dynamic Drive DHTML code library (www.dynamicdrive.com)
    * This notice MUST stay intact for legal use
    * Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
    ***********************************************/

    function disableSelection(target)
    {
        if (typeof target.onselectstart!="undefined") //IE route
	        target.onselectstart=function(){return false}
        else if (typeof target.style.MozUserSelect!="undefined") //Firefox route
	        target.style.MozUserSelect="none"
        else //All other route (ie: Opera)
	        target.onmousedown=function(){return false}
        target.style.cursor = "default"
    }

    //Sample usages
    disableSelection(document.body) //Disable text selection on entire body
    //disableSelection(document.getElementById("mydiv")) //Disable text selection on element with id="mydiv"
