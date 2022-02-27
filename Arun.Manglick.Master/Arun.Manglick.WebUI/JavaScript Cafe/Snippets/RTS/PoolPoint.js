function Validate()
{
	if (CheckBlank(frmPoolPoint.txtCode))
	{
		document.getElementById('lblErrorMessage').innerText="Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmPoolPoint.txtName))
	{
		document.getElementById('lblErrorMessage').innerText="Name Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmPoolPoint.txtAddress1))
	{
		document.getElementById('lblErrorMessage').innerText="Address Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmPoolPoint.txtCity))
	{
		document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmPoolPoint.txtZip))
	{
		document.getElementById('lblErrorMessage').innerText="Zip Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmPoolPoint.txtPhone))
	{
		document.getElementById('lblErrorMessage').innerText="Phone Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmPoolPoint.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckLength(frmPoolPoint.txtZip,5))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip code";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckMultiEmail(frmPoolPoint.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
