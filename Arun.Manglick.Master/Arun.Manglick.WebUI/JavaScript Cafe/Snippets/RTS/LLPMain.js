function Validate()
{
	if (frmLLPMain.ddlExisting.selectedIndex==0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Existing LLP for Editing";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}

function ValidateSearch()
{
	if (CheckBlank(frmLLPMain.txtCriteria))
	{
		document.getElementById('lblErrorMessage').innerText="Search Criteria Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}