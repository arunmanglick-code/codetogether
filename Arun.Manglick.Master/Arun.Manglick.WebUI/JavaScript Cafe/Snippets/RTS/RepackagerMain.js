function Validate()
{
	if (frmRepackagerMain.ddlExisting.selectedIndex==0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Existing Repackager for Editing";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
function ValidateSearch()
{
	if (CheckBlank(frmRepackagerMain.txtCriteria))
	{
		document.getElementById('lblErrorMessage').innerText="Search Criteria Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
return true;
}