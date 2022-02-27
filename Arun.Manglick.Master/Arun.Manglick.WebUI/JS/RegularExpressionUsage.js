function RequiredValidation()
{
	//debugger;				
	if(document.getElementById('txtFileName').value =='')
	{
		window.alert('Please enter a valid filename without extension.');
		document.getElementById('txtFileName').focus();
		return false;
	}				
	else if(Trim(document.getElementById('txtFileName').value) == '')
	{
		// Here the Trim() is defined in "CustomFunction.js"
		window.alert('Filename cannot be blank.');
		document.getElementById('txtFileName').value="";
		document.getElementById('txtFileName').focus();
		return false;
	}
	else if(document.getElementById('txtFileName').value !='')
	{	
		var regexp=/[<*:?^">/\\|]/;
		str=document.getElementById('txtFileName').value;
		if(regexp.test(str))
		{
			window.alert('A filename cannot contain any of these characters (\ / : * ? " < > | .)');
			document.getElementById('txtFileName').value='';
			document.getElementById('txtFileName').focus();
			return false;
		}
		else
		{
			var regexp=/[.]/;
			str=document.getElementById('txtFileName').value;
			if(regexp.test(str))
			{
				window.alert('Please do not try to enter extension.');
				document.getElementById('txtFileName').value='';
				document.getElementById('txtFileName').focus();
				return false;
			}
			else
				{
				ShowProgress();
				return true;				
				}
		}
	}				
}