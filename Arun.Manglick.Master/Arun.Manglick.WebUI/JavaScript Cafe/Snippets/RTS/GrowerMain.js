function Validate()
{
	if (frmGrowerMain.ddlExisting.selectedIndex==0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Existing Grower for Editing";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}

function ValidateSearch()
{
	if (CheckBlank(frmGrowerMain.txtCriteria))
	{
		document.getElementById('lblErrorMessage').innerText="Search Criteria Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
return true;
}