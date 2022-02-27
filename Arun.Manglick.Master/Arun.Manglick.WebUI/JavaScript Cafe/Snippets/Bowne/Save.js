//---------------------------------------------------------------------------
//	save.aspx
//---------------------------------------------------------------------------

function RequiredValidation()
{
	//debugger;
	if(document.getElementById('txtNewName').value=='')
	{
		window.alert('Please enter a valid filename without extension.');
		document.getElementById('txtNewName').focus();
		return false;
	}
	else if(document.getElementById('txtNewName').value !='')
	{
		str=document.getElementById('txtNewName').value;	
		var regexp=/[<*.:?^">/\\|]/;
		result=regexp.test(str);
		if (result)
		{
			window.alert('A filename cannot contain any of these characters (\ / : * ? " < > | .)');
			document.getElementById('txtNewName').focus();
			return false;
		}
		else
			ShowProgress();
	}				
	//return true;				
}
//---------------------------------------------------------------------------

function RequiredValidation1()
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
//---------------------------------------------------------------------------
			
function ShowProgress()
{
	winid1=window.open("/ProposingBowneLive/ProposingBowne/admin/Progress.htm",'Progress','height=200,width=300,top=134,left=312');
	winid1.focus();
}
//---------------------------------------------------------------------------

function ShowProgressComplete()
{
	//debugger;
	try
	{
		if(winid1 !=null)
		{
			//winid1.close();
			
			newwinid1=window.open("/ProposingBowneLive/ProposingBowne/admin/SaveComplete.htm",'Progress','height=300,width=400,top=134,left=312');
			newwinid1.focus();
		}
	}
	catch(ex)
	{
		return false;
	}
}	
//---------------------------------------------------------------------------
