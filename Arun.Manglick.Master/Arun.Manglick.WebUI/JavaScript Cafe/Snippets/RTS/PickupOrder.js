function Validate()
{
	var blnVal = false;
	if (compareDates(document.getElementById('txtFromDate').value,"MM/dd/yyyy",document.getElementById('txtToDate').value,"MM/dd/yyyy") == 1)
	{
		document.getElementById('lblErrorMessage').innerText="Invalid Date selection";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	if (document.getElementById('txtFromDate').value == "")
	{
		document.getElementById('lblErrorMessage').innerText="Please, select both the Date ranges";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	if	(document.getElementById('txtToDate').value == "")
	{
		document.getElementById('lblErrorMessage').innerText="Please, select both the Date ranges";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	if (!CheckSelected())
	{
		document.getElementById('lblErrorMessage').innerText="There should be atleast one Status Selected";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
}

function CheckSelected()
{
	var name = "chklStatus_";
	var x = 0;
	var i = 0;
	do{
		oElement = document.getElementById(name + i);
		if (oElement == null)
		{
			x = 1;
		}
		else
		{
			if (oElement.checked == true) 
			{
				return true;
			}
		}
		i++;
	}while(x<1);
	return false;
}

