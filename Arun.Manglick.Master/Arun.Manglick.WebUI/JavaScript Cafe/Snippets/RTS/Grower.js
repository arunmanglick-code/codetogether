function Validate()
{
	if (CheckBlank(frmGrower.txtCode))
	{
		document.getElementById('lblErrorMessage').innerText="Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmGrower.txtDays))
	{
		document.getElementById('lblErrorMessage').innerText="Default Rental Days Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmGrower.txtName))
	{
		document.getElementById('lblErrorMessage').innerText="Name Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmGrower.txtAddress1))
	{
		document.getElementById('lblErrorMessage').innerText="Address Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmGrower.txtCity))
	{
		document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmGrower.txtZip))
	{
		document.getElementById('lblErrorMessage').innerText="Zip Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmGrower.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckLength(frmGrower.txtZip,5))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip code";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckMultiEmail(frmGrower.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}

