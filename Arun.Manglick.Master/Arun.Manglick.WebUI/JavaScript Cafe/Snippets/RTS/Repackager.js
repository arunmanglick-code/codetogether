function Validate()
{
	if (CheckBlank(frmRepackager.txtCode))
	{
		document.getElementById('lblErrorMessage').innerText="Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRepackager.txtName))
	{
		document.getElementById('lblErrorMessage').innerText="Name Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRepackager.txtAddress1))
	{
		document.getElementById('lblErrorMessage').innerText="Address Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRepackager.txtCity))
	{
		document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRepackager.txtZip))
	{
		document.getElementById('lblErrorMessage').innerText="Zip Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRepackager.txtPhone))
	{
		document.getElementById('lblErrorMessage').innerText="Phone Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRepackager.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckLength(frmRepackager.txtZip,5))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip code";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckMultiEmail(frmRepackager.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
