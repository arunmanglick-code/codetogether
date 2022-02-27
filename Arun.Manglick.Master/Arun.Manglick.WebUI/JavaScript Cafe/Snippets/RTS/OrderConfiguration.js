function Validate()
{
	if (CheckBlank(frmOrderConfig.txtCode))
	{
		document.getElementById('lblErrorMessage').innerText="Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmOrderConfig.txtName))
	{
		document.getElementById('lblErrorMessage').innerText="Name Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
