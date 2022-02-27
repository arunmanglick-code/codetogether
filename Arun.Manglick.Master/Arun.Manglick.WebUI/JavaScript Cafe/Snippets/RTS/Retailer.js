function Validate()
{	
	if (CheckBlank(frmRetailer.txtCode))
	{
		document.getElementById('lblErrorMessage').innerText="Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (frmRetailer.ddlRetailChain.selectedIndex==-1)
	{
		document.getElementById('lblErrorMessage').innerText="No Retail Chain Avaiable, Please Enter Retail Chain";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (frmRetailer.ddlRepackager.selectedIndex==-1)
	{
		document.getElementById('lblErrorMessage').innerText="No Repackager Avaiable, Please Enter Repackager";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRetailer.txtStoreNumber))
	{
		document.getElementById('lblErrorMessage').innerText="Store Number Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRetailer.txtAddress1))
	{
		document.getElementById('lblErrorMessage').innerText="Address Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRetailer.txtCity))
	{
		document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmRetailer.txtZip))
	{
		document.getElementById('lblErrorMessage').innerText="Zip Code Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	/*else if (CheckBlank(frmRetailer.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}*/
	else if (!CheckLength(frmRetailer.txtZip,5))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip code";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckMultiEmail(frmRetailer.txtEmail))
	{
		document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
