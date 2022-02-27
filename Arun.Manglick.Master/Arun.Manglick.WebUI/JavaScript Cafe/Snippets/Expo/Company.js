//administrator role -  Company.aspx
function CompanyLoad(usrtype)
{
	if(usrtype == 0)	// admin user 
		MM_swapImage('Image7','','/exposervicedesk/Images/tabs/company-r.gif',1);
	
	SetCursor('txtCompanyCode');
}


//employee role - EmployeeDashboard.aspx
function EmployeeLoad()
{
	MM_swapImage('Image31','','/exposervicedesk/Images/tabs/company-r.gif',1);
	SetCursor('txtCompanyCode');
}

function Validate()
{
	if (CheckBlank(document.getElementById('txtCompanycode')))
	{
		alert("Contact Title Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Company Name Cannot be Blank";
		document.getElementById('txtCompanycode').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtCompanyname')))
	{
		alert("Company Name Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Contact Title Cannot be Blank";
		document.getElementById('txtCompanyname').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtContactFirstname')))
	{
		alert("Contact First Name Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Contact First Name Cannot be Blank";
		document.getElementById('txtContactFirstname').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtContactLastname')))
	{
		alert("Contact Last Name Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Contact Last Name Cannot be Blank";
		document.getElementById('txtContactLastname').focus();
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
	else if (CheckBlank(document.getElementById('txtEmail')))
	{
		alert("Email Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank";
		document.getElementById('txtEmail').focus();
		return false;
	}
	else if (!CheckEmail(document.getElementById('txtEmail')))
	{
		alert("Please Enter valid Email address");
		//document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
		document.getElementById('txtEmail').focus();
		return false;
	}
	return true;
}