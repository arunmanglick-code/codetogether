
function PaymentLoad()
{
	MM_swapImage('Image42','','/exposervicedesk/Images/tabs/laborrequest-r.gif',1);
}


function OnlyRate()
{
	var ratevalue='';
	ratevalue = document.Payment.elements(event.srcElement.name).value;
	if (onlyRate(ratevalue) == true)
	{
		return true;
	}
	else
	{
		return false;
	}
}

function Amountcalculate()
{
	var totalamt;

	if(document.getElementById('rdoMasterAccount').checked)
	{
		totalamt = 0;
	
		if(document.getElementById('txtMANumber').value == "")
		{
			alert("Please Enter MasterAccount Number");
			//document.getElementById('lblErrorMessage').innerText="Please Enter MasterAccount Number";
			document.getElementById('txtMANumber').focus();
			return false;	
		}
		if(document.getElementById('txtMasAccAmount').value != "")
		{
			if (!IsNumber(document.getElementById('txtMasAccAmount')))
			{
				alert("MasterAccount Amount Cannot be AlphaNumeric value");
				//document.getElementById('lblErrorMessage').innerText="MasterAccount Amount Cannot be AlphaNumeric value";
				document.getElementById('txtMasAccAmount').focus();
				return false;
			}
			totalamt = totalamt + eval(document.getElementById('txtMasAccAmount').value);
		}
		else
		{
			alert("Please Enter MasterAccount Amount");
			//document.getElementById('lblErrorMessage').innerText="Please Enter MasterAccount Amount";
			document.getElementById('txtMasAccAmount').focus();
			return false;	
		}	
		document.getElementById('txtYourAmount').value = totalamt;
		return true;
	}
		
	if(document.getElementById('rdootheraccount').checked)
	{
		totalamt = 0;
		
		//////////////////////////////////// CREDIT CARD //////////////////////////////////////
		if(document.getElementById('chkcard').checked==true)
		{
			if (CheckBlank(document.getElementById('txtPaymentNumber')))
			{
				alert("Card Number Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="Card Number Cannot be Blank";
				document.getElementById('txtPaymentNumber').focus();
				return false;
			}
			else if (!IsNumber(document.getElementById('txtPaymentNumber').value))
			{
				alert("Card Number Cannot be AlphaNumeric value");
				//document.getElementById('lblErrorMessage').innerText="Card Number Cannot be AlphaNumeric value";
				document.getElementById('txtPaymentNumber').focus();
				return false;
			}
			else if (CheckBlank(document.getElementById('txtPaymentCode')))
			{
				alert("Card Code Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="Card Code Cannot be Blank";
				document.getElementById('txtPaymentCode').focus();
				return false;
			}
			else if (!IsNumber(document.getElementById('txtPaymentCode').value))
			{
				alert("Card Code Cannot be AlphaNumeric value");
				//document.getElementById('lblErrorMessage').innerText="Card Code Cannot be AlphaNumeric value";
				document.getElementById('txtPaymentCode').focus();
				return false;
			}
			else if (CheckBlank(document.getElementById('txtPaymentName')))
			{
				alert("Card HolderName Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="Card HolderName Cannot be Blank";
				document.getElementById('txtPaymentName').focus();
				return false;
			}
			if(document.getElementById('txtCardAmount').value != "")
			{
				totalamt = totalamt + eval(document.getElementById('txtCardAmount').value);
			}
			else
			{
				alert("Please Enter Card Amount");
				//document.getElementById('lblErrorMessage').innerText="Please Enter Card Amount";
				document.getElementById('txtCardAmount').focus();
				return false;
			}
		}
		//////////////////////////////////// CHEQUE //////////////////////////////////////
		if(document.getElementById('chkcheque').checked==true)
		{
			if (CheckBlank(document.getElementById('txtNumber')))
			{
				alert("Check Number Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="Cheque Number Cannot be Blank";
				document.getElementById('txtNumber').focus();
				return false;
			}
			if (!IsNumber(document.getElementById('txtNumber').value))
			{
				alert("Check Number Cannot be AlphaNumeric value";);
				//document.getElementById('lblErrorMessage').innerText="Cheque Number Cannot be AlphaNumeric value";
				document.getElementById('txtNumber').focus();
				return false;
			}
			if(document.getElementById('txtChequeAmount').value != "")
			{
				totalamt = totalamt + eval(document.getElementById('txtChequeAmount').value);
			}
			else
			{
				alert("Please Enter Check Amount");
				//document.getElementById('lblErrorMessage').innerText="Please Enter Cheque Amount";
				document.getElementById('txtChequeAmount').focus();
				return false;
			}
		}
		//////////////////////////////////// CASH //////////////////////////////////////
		if(document.getElementById('chkCash').checked==true)
		{
			if(document.getElementById('txtCashAmount').value != "")
			{
				totalamt = totalamt + eval(document.getElementById('txtCashAmount').value);
			}
			else
			{
				alert("Please Enter Cash Amount");
				//document.getElementById('lblErrorMessage').innerText="Please Enter Cash Amount";
				document.getElementById('txtCashAmount').focus();
				return false;	
			}
		}
		//////////////////////////////////// BILLING AMOUNT //////////////////////////////////////
		if(document.getElementById('chkBillingAccount').checked==true)
		{
			if(document.getElementById('txtbillingamount').value != "")
			{
				totalamt = totalamt + eval(document.getElementById('txtbillingamount').value);
			}
			else
			{
				alert("Please Enter BillingAccount Amount");
				//document.getElementById('lblErrorMessage').innerText="Please Enter BillingAccount Amount";
				document.getElementById('txtbillingamount').focus();
				return false;	
			}
		}
		//////////////////////////////////// WIDE OFF //////////////////////////////////////
		if(document.getElementById('chkwideoff').checked==true)
		{
			if (CheckBlank(document.getElementById('txtwideno')))
			{
				alert("WO Number Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="WO Number Cannot be Blank";
				document.getElementById('txtwideno').focus();
				return false;
			}
			if (!IsNumber(document.getElementById('txtwideno').value))
			{
				alert("WO Number Cannot be AlphaNumeric value");
				//document.getElementById('lblErrorMessage').innerText="WO Number Cannot be AlphaNumeric value";
				document.getElementById('txtwideno').focus();
				return false;
			}
			if(document.getElementById('txtwideamt').value != "")
			{
				totalamt = totalamt + eval(document.getElementById('txtwideamt').value);
			}
			else
			{
				alert("Please Enter Wide Off Amount");
				//document.getElementById('lblErrorMessage').innerText="Please Enter Wide Off Amount";
				document.getElementById('txtwideamt').focus();
				return false;
			}
		}	
		//////////////////////////////////// WIDE TRANSFER //////////////////////////////////////
		if(document.getElementById('chkwiretransfer').checked==true)
		{
			if (CheckBlank(document.getElementById('txtwireno')))
			{
				alert("WT Number Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="WT Number Cannot be Blank";
				document.getElementById('txtwireno').focus();
				return false;
			}
			if (!IsNumber(document.getElementById('txtwireno').value))
			{
				alert("WT Number Cannot be AlphaNumeric value");
				//document.getElementById('lblErrorMessage').innerText="WT Number Cannot be AlphaNumeric value";
				document.getElementById('txtwireno').focus();
				return false;
			}
			if(document.getElementById('txtwireamt').value != "")
			{
				totalamt = totalamt + eval(document.getElementById('txtwireamt').value);
			}
			else
			{
				alert("Please Enter Wire Amount");
				//document.getElementById('lblErrorMessage').innerText="Please Enter Wire Amount";
				document.getElementById('txtwireamt').focus();
				return false;
			}
		}
		
		if((document.getElementById('chkBillingAccount').checked == false)&&(document.getElementById('chkCash').checked == false)&&(document.getElementById('chkwideoff').checked == false)&&(document.getElementById('chkwiretransfer').checked == false)&&(document.getElementById('chkcheque').checked == false)&&(document.getElementById('chkcard').checked == false))
		{
			alert("Select Atleast one payment option");
			//document.getElementById('lblErrorMessage').innerText="Select Atleast one payment option";
			return false;
		}
		
		document.getElementById('txtYourAmount').value = totalamt;
		return true;
	}
	
	if((document.getElementById('rdoMasterAccount').checked==false)&&(document.getElementById('rdootheraccount').checked==false))
	{
		alert("Select Atleast One Payment Method ( Master Account / Othere Account )");
		//document.getElementById('lblErrorMessage').innerText="Select Atleast One Payment Method ( Master Account / Othere Account )";
		return false;
	}
}

function Validate()
{
	var flg;
	flg=Amountcalculate();
	
	if(flg==false)
	{
		return false;
	}
	
	if(document.getElementById('optNew').checked)
	{
		if (CheckBlank(document.getElementById('txtCompanyName')))
		{
			alert("Company Name Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Company Name Cannot be Blank";
			document.getElementById('txtCompanyName').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtCardHolder')))
		{
			alert("Card Holder Name Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Card Holder Name Cannot be Blank";
			document.getElementById('txtCardHolder').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtAddress1')))
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
		else if (CheckBlank(document.getElementById('txtCity')))
		{
			alert("City Cannot be Blank";
			//document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
			document.getElementById('txtCity').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtZip')))
		{
			alert("Zip Code Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Zip Code Cannot be Blank";
			document.getElementById('txtZip').focus();
			return false;
		}
		else if (!CheckLength(document.getElementById('txtZip'),5))
		{
			alert("Please Enter valid Zip Code");
			//document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip Code";
			document.getElementById('txtZip').focus();
			return false;
		}
		else if (!IsNumeric(document.getElementById('txtZip').value))
		{
			alert("Zip Code should be Numeric");
			//document.getElementById('lblErrorMessage').innerText="Zip Code should be Numeric";
			document.getElementById('txtZip').focus();
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
	}
	return true;
}

function ShowPaymentTerms()
{
	window.open("/exposervicedesk/Transaction/Online/PaymentTerms.htm","PaymentTerms","height=300,width=500,status=yes,toolbar=no,menubar=no,scrollbars=1");
}

function CustomerPayment()
{
	if (CheckBlank(document.getElementById('txtPaymentNumber')))
	{
		alert("Card Number Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Card Number Cannot be Blank";
		document.getElementById('txtPaymentNumber').focus();
		return false;
	}
	else if (!IsNumber(document.getElementById('txtPaymentNumber').value))
	{
		alert("Card Number Cannot be AlphaNumeric value");
		//document.getElementById('lblErrorMessage').innerText="Card Number Cannot be AlphaNumeric value";
		document.getElementById('txtPaymentNumber').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtPaymentCode')))
	{
		alert("CVV Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="CVV Cannot be Blank";
		document.getElementById('txtPaymentCode').focus();
		return false;
	}
	else if (!IsNumber(document.getElementById('txtPaymentCode').value))
	{
		alert("CVV Cannot be AlphaNumeric value");
		//document.getElementById('lblErrorMessage').innerText="CVV Cannot be AlphaNumeric value";
		document.getElementById('txtPaymentCode').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtPaymentName')))
	{
		alert("Card HolderName Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Card HolderName Cannot be Blank";
		document.getElementById('txtPaymentName').focus();
		return false;
	}
	if (document.getElementById('optNew').checked == true)
	{
		if (CheckBlank(document.getElementById('txtCompanyName')))
		{
			alert("Company Name Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Company Name Cannot be Blank";
			document.getElementById('txtCompanyName').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtCardHolder')))
		{
			alert("Card HolderName Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Card HolderName Cannot be Blank";
			document.getElementById('txtCardHolder').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtAddress1')))
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
		else if (CheckBlank(document.getElementById('txtCity')))
		{
			alert("City Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
			document.getElementById('txtCity').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtZip')))
		{
			alert("Zip Code Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Zip Code Cannot be Blank";
			document.getElementById('txtZip').focus();
			return false;
		}
		else if (!CheckLength(document.getElementById('txtZip'),5))
		{
			alert("Please Enter valid Zip Code");
			//document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip Code";
			document.getElementById('txtZip').focus();
			return false;
		}
		else if (!IsNumeric(document.getElementById('txtZip').value))
		{
			alert("Zip Code should be Numeric");
			//document.getElementById('lblErrorMessage').innerText="Zip Code should be Numeric";
			document.getElementById('txtZip').focus();
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
	}
	if(document.getElementById('chkAgree').checked == false)
	{
		alert('Click on I Agree to accept Payment terms');
		return false;
	}
	return true;
}
