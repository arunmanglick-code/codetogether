function Validate()
{
	if (CheckBlank(frmLLP.txtCode))
	{
		document.getElementById('lblErrorMessage').innerText="Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmLLP.txtName))
	{
		document.getElementById('lblErrorMessage').innerText="Name Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmLLP.txtAddress1))
	{
		document.getElementById('lblErrorMessage').innerText="Address Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmLLP.txtCity))
	{
		document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmLLP.txtZip))
	{
		document.getElementById('lblErrorMessage').innerText="Zip Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmLLP.txtPhone))
	{
		document.getElementById('lblErrorMessage').innerText="Phone Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmLLP.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckLength(frmLLP.txtZip,5))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip code";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckEmail(frmLLP.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
