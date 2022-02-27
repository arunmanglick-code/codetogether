function Validate()
{
	if (frmSKUMain.ddlExisting.selectedIndex==0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Existing SKU for Editing";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}

function ValidateSearch()
{
	if (CheckBlank(frmSKUMain.txtCriteria))
	{
		document.getElementById('lblErrorMessage').innerText="Search Criteria Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}