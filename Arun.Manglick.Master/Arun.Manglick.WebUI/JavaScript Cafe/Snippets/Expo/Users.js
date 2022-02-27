
function UserLoad()
{
	MM_swapImage('Image8','','/exposervicedesk/Images/tabs/user-r.gif',1);
	
	if (document.getElementById("ddlFacility").disabled == false)
		SetCursor('ddlFacility');
	else if (document.getElementById("ddlUserType").disabled == false)
		SetCursor('ddlUserType');
		
}		

function Validate()
{
	if (CheckBlank(document.getElementById('txtCompanyName')))
	{
		alert("Company Name Cannot be Blank");
		document.getElementById('txtCompanyName').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtUserName'))) 
	{
		alert("User Name Cannot be Blank");
		document.getElementById('txtUserName').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtPassword')))
	{
		alert("Password Cannot be Blank");
		document.getElementById('txtPassword').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtFirstName'))) 
	{
		alert("First Name Cannot be Blank");
		document.getElementById('txtFirstName').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtLastName'))) 
	{
		alert("Last Name Cannot be Blank");
		document.getElementById('txtLastName').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtAddress1')))
	{
		alert("Address1 Cannot be Blank");
		document.getElementById('txtAddress1').focus();
		return false;
	}
	else if (!CheckMaxLength(document.getElementById('txtAddress1'),300))
	{
		alert("Address1 Cannot be more than 300 characters");
		document.getElementById('txtAddress1').focus();
		return false;
	}
	else if (!CheckMaxLength(document.getElementById('txtAddress2'),300))
	{
		alert("Address2 Cannot be more than 300 characters");
		document.getElementById('txtAddress2').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtCity')))
	{
		alert("City Cannot be Blank");
		document.getElementById('txtCity').focus();
		return false;
	}
	else if (document.getElementById("ddlState").value == -1 && CheckBlank(document.getElementById('txtState')))
	{
		alert("State Cannot be Blank");
		document.getElementById('txtState').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtPostalCode')))
	{
		alert("Zip Cannot be Blank");
		document.getElementById('txtPostalCode').focus();
		return false;
	}
	else if (!CheckLength(document.getElementById('txtPostalCode'),5))
	{
		alert("Please Enter valid Zip");
		document.getElementById('txtPostalCode').focus();
		return false;
	}
	else if (!IsNumeric(document.getElementById('txtPostalCode').value))
	{
		alert("Zip should be Numeric");
		document.getElementById('txtPostalCode').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtPhone')))
	{
		alert("Phone Cannot be Blank");
		document.getElementById('txtPhone').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtEmail')))
	{
		alert("Email Cannot be Blank");
		document.getElementById('txtEmail').focus();
		return false;
	}
	else if (!CheckEmail(document.getElementById('txtEmail')))
	{
		alert("Please Enter valid Email address");
		document.getElementById('txtEmail').focus();
		return false;
	}
	else 
	{
		var oElement
		oElement = document.getElementById("ddlUserType");
		if (oElement.value == 3)
		{
			if (document.getElementById("ddlCCType").value == -1)
			{
				alert("Select Credit Card");
				document.getElementById('ddlCCType').focus();
				return false;
			}
			if (CheckBlank(document.getElementById('txtCCNumber')))
			{
				alert("Credit Card Number Cannot be Blank");
				document.getElementById('txtCCNumber').focus();
				return false;
			}
			if (!IsNumeric(document.getElementById('txtCCNumber').value))
			{
				alert("Credit Card Number should be Numeric");
				document.getElementById('txtCCNumber').focus();
				return false;
			}
			if(document.getElementById("txtCCNumber").value.length<15)
			{
				document.getElementById("txtCCNumber").focus();
				alert("Invalid Credit Card Number");
				return false;
			}
			if (CheckBlank(document.getElementById('txtCVVNumber')))
			{
				alert("CVV Number Cannot be Blank");
				document.getElementById('txtCVVNumber').focus();
				return false;
			}
			if (!IsNumeric(document.getElementById('txtCVVNumber').value))
			{
				alert("CVV Number should be Numeric");
				document.getElementById('txtCVVNumber').focus();
				return false;
			}
			if(document.getElementById("txtCVVNumber").value.length<3)
			{
				document.getElementById("txtCVVNumber").focus();
				alert("Invalid CVV Number");
				return false;
			}
			if (CheckBlank(document.getElementById('txtAnswer')))
			{
				alert("Answer for Secrete Question Cannot be Blank");
				document.getElementById('txtAnswer').focus();
				return false;
			}
			return true;
		}
	}
	return true;
}

function ValidateState()
{
	var oElement
	oElement = document.getElementById("ddlState");
	if (oElement.value == -1)
	{
		oElement = document.getElementById("txtState");
		oElement.disabled = false;
		return false
	}
	else
	{
		oElement = document.getElementById("txtState");
		oElement.disabled = true;
		oElement.value='';
		return false;
	}
}
	
function CreditCard()
{
	var oElement,oEle
	oElement = document.getElementById("ddlUserType");
	oEle = document.getElementById("divCCard");
	if (oElement.value == 3)
	{
		oEle.runtimeStyle.display="block";
	}
	else
	{
		oEle.runtimeStyle.display="none";
	}
}

