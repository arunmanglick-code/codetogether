
function VendorLoad(usrtype)
{
	if(usrtype == 0)	// admin user 
		MM_swapImage('Image3','','/exposervicedesk/Images/tabs/vendor-r.gif',1);
	else				// employee user
		MM_swapImage('Image33','','/exposervicedesk/Images/tabs/vendor-r.gif',1);
	
	SetCursor('txtCode')
}

function Validate()
{
	if (CheckBlank(document.getElementById('txtCode')))
	{
		alert("Vendor code Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Vendor code Cannot be Blank.";
		document.getElementById('txtCode').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtTitle'))) 
	{
		alert("Company Name Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Vendor title Cannot be Blank.";
		document.getElementById('txtTitle').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtName'))) 
	{
		alert("Vendor first name Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Vendor name Cannot be Blank.";
		document.getElementById('txtName').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtLastName'))) 
	{
		alert("Vendor last name Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Vendor title Cannot be Blank.";
		document.getElementById('txtLastName').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtAddress1'))) 
	{
		alert("Address1 Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Address1 Cannot be Blank";
		document.getElementById('txtAddress1').focus();
		return false;
	}
	else if (!CheckMaxLength(document.getElementById('txtAddress1'),300))
	{
		alert("Address1 Cannot be more than 300 characters");
		//document.getElementById('lblErrorMessage').innerText="Address1 Cannot be more than 300 characters";
		document.getElementById('txtAddress1').focus();
		return false;
	}
	else if (!CheckMaxLength(document.getElementById('txtAddress2'),300))
	{
		alert("Address2 Cannot be more than 300 characters");
		//document.getElementById('lblErrorMessage').innerText="Address2 Cannot be more than 300 characters";
		document.getElementById('txtAddress2').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtCity'))) 
	{
		alert("City Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
		document.getElementById('txtCity').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtPostalCode')))
	{
		alert("Zip Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Zip Cannot be Blank";
		document.getElementById('txtPostalCode').focus();
		return false;
	}
	else if (!IsNumeric(document.getElementById('txtPostalCode').value))
	{
		alert("Zip should be Numeric");
		//document.getElementById('lblErrorMessage').innerText="Zip should be Numeric";
		document.getElementById('txtPostalCode').focus();
		return false;
	}
	else if (!CheckLength(document.getElementById('txtPostalCode'),5))
	{
		alert("Please Enter valid Zip");
		//document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip";
		document.getElementById('txtPostalCode').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtPhone')))
	{
		alert("Phone Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Phone Cannot be Blank";
		document.getElementById('txtPhone').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtEMail')))
	{
		alert("Email Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank.";
		document.getElementById('txtEMail').focus();
		return false;
	}
	else if (!CheckEmail(document.getElementById('txtEmail')))
	{
		alert("Please Enter valid Email address");
		//document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
		document.getElementById('txtEmail').focus();
		return false;
	}
	else if (!CheckMaxLength(document.getElementById('txtShortDesc'),50))
	{
		alert("Short description cannot be more than 50 characters.");
		//document.getElementById('lblErrorMessage').innerText="Short description cannot be more than 50 characters.";
		document.getElementById('txtShortDesc').focus();
		return false;
	}			
	else if (!CheckMaxLength(document.getElementById('txtDesc'),300))
	{
		alert("Description cannot be more than 300 characters.");
		//document.getElementById('lblErrorMessage').innerText="Description cannot be more than 300 characters.";
		document.getElementById('txtDesc').focus();
		return false;
	}			
	return true;
}
