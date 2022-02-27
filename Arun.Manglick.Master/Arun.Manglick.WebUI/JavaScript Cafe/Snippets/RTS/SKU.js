function Validate()
{
	if (CheckBlank(frmSKU.txtCode))
	{
		document.getElementById('lblErrorMessage').innerText="Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmSKU.txtDesc))
	{
		document.getElementById('lblErrorMessage').innerText="Description Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
