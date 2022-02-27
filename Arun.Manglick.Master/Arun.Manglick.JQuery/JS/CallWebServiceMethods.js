
// This function calls the Web service method without passing the callback function. 
function GetNoReturn()
{
    Arun.Manglick.UI.ComplexWebService.GetServerTime();
    alert("This method does not return a value.");    
}


// This function calls the Web service method and passes the event callback function.  
function GetTime()
{
    Arun.Manglick.UI.ComplexWebService.GetServerTime(SucceededCallback);    
}


// This function calls the Web service method passing simple type parameters and the callback function  
function Add(a,  b)
{
    Arun.Manglick.UI.ComplexWebService.Add(a, b,SucceededCallback);
}

// This function calls the Web service method passing simple type parameters and the callback function  
function AddSumObject(a,  b)
{
    var objSum = new Arun.Manglick.UI.SumObject();
    objSum.Number1 = a;
    objSum.Number2 = b;
            
    Arun.Manglick.UI.ComplexWebService.Sum(objSum,SucceededCallback);
}

// This function calls the Web service method that returns an XmlDocument type.  
function GetXmlDocument() 
{
    Arun.Manglick.UI.ComplexWebService.GetXmlDocument(SucceededCallbackWithContext, FailedCallback,"XmlDocument") // "XmlDocument" will be used as 'userConext' in callback method
}

// This function calls a Web service method that uses GET to make the Web request.
function MakeGetRequest() 
{
    Arun.Manglick.UI.ComplexWebService.EchoStringAndDate(new Date("1/1/2007"), " Happy",SucceededCallback, FailedCallback, "HappyNewYear"); // "HappyNewYear" will be used as 'userConext' in callback method
}



// This is the callback function invoked if the Web service succeeded.
// It accepts the result object, the user context, and the calling method name as parameters.
function SucceededCallbackWithContext(result, userContext, methodName)
{
    var output;
    var readResult;
    
    var RsltElem = document.getElementById("ResultId");
    
    if (userContext == "XmlDocument")
	{	
	    if (document.all) // Firefox
	        readResult = result.documentElement.firstChild.text;
		else		   
		   readResult =  result.documentElement.firstChild.textContent;
		
	     RsltElem.innerHTML = "XmlDocument content: " + readResult;
	}
    
}

// This is the callback function invoked if the Web service  succeeded.
// It accepts the result object as a parameter.
function SucceededCallback(result, eventArgs)
{
    // Page element to display feedback.
    var RsltElem = document.getElementById("ResultId");
    RsltElem.innerHTML = result;
}


// This is the callback function invoked if the Web service failed.
// It accepts the error object as a parameter.
function FailedCallback(error)
{
    // Display the error.    
    var RsltElem = document.getElementById("ResultId");
    RsltElem.innerHTML = "Service Error: " + error.get_message();
}

if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();