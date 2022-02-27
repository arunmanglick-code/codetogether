function Validate()
{
	if (frmBookOrderMain.ddlCustomer.selectedIndex==0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Grower for Order Entry";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
