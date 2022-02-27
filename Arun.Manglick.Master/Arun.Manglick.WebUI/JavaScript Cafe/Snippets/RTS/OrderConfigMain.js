function Validate()
{
	if (frmOrderConfigMain.ddlExisting.selectedIndex==0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Existing Order for Editing";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}

function ValidateSearch()
{
	if (CheckBlank(frmOrderConfigMain.txtCriteria))
	{
		document.getElementById('lblErrorMessage').innerText="Search Criteria Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}