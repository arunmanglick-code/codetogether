function Validate()
{
	if (frmRetailerMain.ddlExisting.selectedIndex==0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Existing Retailer for Editing";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}

function ValidateCreate()
{
	if (CheckBlank(frmRetailerMain.txtRetailChain))
	{
		document.getElementById('lblErrorMessage').innerText="Retail Chain cannot be blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
function ValidateSearch()
{
	if (CheckBlank(frmRetailerMain.txtCriteria))
	{
		document.getElementById('lblErrorMessage').innerText="Search Criteria Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
return true;
}