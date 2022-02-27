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
}