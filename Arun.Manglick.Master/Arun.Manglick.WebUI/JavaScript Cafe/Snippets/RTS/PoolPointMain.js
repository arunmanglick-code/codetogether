function Validate()
{
	if (frmPoolPointMain.ddlExisting.selectedIndex==0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Existing Pool Point for Editing";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
function ValidateSearch()
{
	if (CheckBlank(frmPoolPointMain.txtCriteria))
	{
		document.getElementById('lblErrorMessage').innerText="Search Criteria Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
return true;
}