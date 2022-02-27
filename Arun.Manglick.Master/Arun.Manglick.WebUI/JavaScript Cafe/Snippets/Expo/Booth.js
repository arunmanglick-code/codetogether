
function BoothLoad()
{
	countrecord();
	
	OA_Selected();

	CC_Selected();
	BA_Selected();
	CS_Selected();
	CK_Selected();
	WO_Selected();
	WT_Selected();

}

//Function is used when click on Add button in boothcheck page.
function validboothentry()
{
	if(document.getElementById('ddlEvent').value =="")
	{
		alert("Event should not be blank");
		return false;
	}
	if(CheckBlank(document.getElementById('txtbooth')))
	{
		alert("Booth Number Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Booth Number Cannot be Blank";
		document.getElementById('txtbooth').focus();
		return false;	
	}
	/*if(document.getElementById('lstcompanyid').value == "")
	{
		if(CheckBlank(document.getElementById('txtcompany')))
		{
		alert("Select Company from the list or enter company name in textbox");
		//document.getElementById('lblErrorMessage').innerText="Select Company from the list or enter company name in textbox";
		document.getElementById('txtcompany').focus();
		return false;			
		}
	}*/	
	/*if(CheckBlank(document.getElementById('txtContact')))
	{
		alert("Contact Person Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Contact Person Cannot be Blank";
		document.getElementById('txtContact').focus();
		return false;	
	}*/
	if(CheckBlank(document.getElementById('txtdesc')))
	{
		alert("Service Description Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Service Description Cannot be Blank";
		document.getElementById('txtdesc').focus();
		return false;		
	}
	if(CheckBlank(document.getElementById('txtqty')))
	{
		alert("Quantity Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Quantity Cannot be Blank";
		document.getElementById('txtqty').focus();
		return false;		
	}
	if(eval(document.getElementById('txtqty').value)>99)
	{
		alert("Quantity must be less than 100");
		//document.getElementById('lblErrorMessage').innerText="Quantity Cannot be Blank";
		document.getElementById('txtqty').focus();
		return false;
	}
	if (!IsNumber(document.getElementById('txtqty').value))
	{
		alert("Quantity Cannot be Alphanumeric Value");
		//document.getElementById('lblErrorMessage').innerText="Quantity Cannot be Alphanumeric Value";
		document.getElementById('txtqty').focus();
		return false;
	}
	if(CheckBlank(document.getElementById('txtprice')))
	{
		alert("Price Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Price Cannot be Blank";
		document.getElementById('txtprice').focus();
		return false;		
	}
	if(eval(document.getElementById('txtprice').value)>10000)
	{
		alert("Price must be less than 10000");
		//document.getElementById('lblErrorMessage').innerText="Price Cannot be Blank";
		document.getElementById('txtprice').focus();
		return false;		
	}
	if (!IsNumber(document.getElementById('txtprice').value))
	{
		alert("Price Cannot be Alphanumeric Value");
		//document.getElementById('lblErrorMessage').innerText="Price Cannot be Alphanumeric Value";
		document.getElementById('txtprice').focus();
		return false;
	}
	return true;
}


//Function is used for accepting only integer and float value.
function OnlyRate()
{
	var ratevalue='';
	ratevalue = document.form.elements(event.srcElement.name).value;
	if (onlyRate(ratevalue) == true)
	{
		return true;
	}
	else
	{
		return false;
	}
}

function OnlyInteger()
{
	var ratevalue='';
	ratevalue = document.form.elements(event.srcElement.name).value;
	if (IsNumbervalue(ratevalue) == true)
	{
		return true;
	}
	else
	{
		return false;
	}
}

function IsNumbervalue(p)
{		

	if (!(event.keyCode >= '49' && event.keyCode <= '57'))
	{
		event.keyCode=0;
	}
}


//Function is use in boothpayment page for validating the selection of the payment option
function Validate()
{
	var totalamt;
	
	if(document.getElementById('rdoMasterAccount').checked)
	{
		totalamt = 0;
		
		//alert("Master Account");
		if((document.getElementById('txtMasAccAmount').value != "") && (document.getElementById('txtMasAccAmount').value != 0))
		{
			totalamt = totalamt + eval(document.getElementById('txtMasAccAmount').value)
		}
		else
		{
			alert("Please Enter MasterAccount Amount");
			document.getElementById('txtMasAccAmount').focus();
			return false;	
		}
		
		document.getElementById('txtYourAmount').value = totalamt;
		return true;
	}
		
	if(document.getElementById('rdootheraccount').checked)
	{
		totalamt = 0;
		
		//alert("Other Account");
		if(document.getElementById('chkcard').checked==true)
		{
			if (CheckBlank(document.getElementById('txtPaymentNumber')))
			{
				alert("Card Number Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="Card Number Cannot be Blank";
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
			else if (CheckBlank(document.getElementById('txtPaymentName')))
			{
				alert("Card HolderName Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="Card HolderName Cannot be Blank";
				document.getElementById('txtPaymentName').focus();
				return false;
			}
			if((document.getElementById('txtCardAmount').value != "") && (document.getElementById('txtCardAmount').value != 0))
			{
				totalamt = totalamt + eval(document.getElementById('txtCardAmount').value)
			}
			else
			{
				alert("Please Enter Card Amount");
				document.getElementById('txtCardAmount').focus();
				return false;
			}
		}
		
		if(document.getElementById('chkcheque').checked==true)
		{
			if (CheckBlank(document.getElementById('txtNumber')))
			{
				document.getElementById('Message').innerText="Number Cannot be Blank";
				document.getElementById('txtNumber').focus();
				return false;
			}
			
			if((document.getElementById('txtChequeAmount').value != "") && (document.getElementById('txtChequeAmount').value != 0))
			{
				totalamt = totalamt + eval(document.getElementById('txtChequeAmount').value)
			}
			else
			{
				alert("Please Enter Check Amount");
				document.getElementById('txtChequeAmount').focus();
				return false;
			}
		}
		
		
		if(document.getElementById('chkwiretransfer').checked==true)
		{
			if (CheckBlank(document.getElementById('txtwireno')))
			{
				alert("Number Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="Number Cannot be Blank";
				document.getElementById('txtwireno').focus();
				return false;
			}
			
			if((document.getElementById('txtwireamt').value != "") && (document.getElementById('txtwireamt').value != 0))
			{
				totalamt = totalamt + eval(document.getElementById('txtwireamt').value)
			}
			else
			{
				alert("Please Enter Wire Amount");
				document.getElementById('txtwireamt').focus();
				return false;
			}
		}
		
		if(document.getElementById('chkwideoff').checked==true)
		{
			if (CheckBlank(document.getElementById('txtwideno')))
			{
				alert("Number Cannot be Blank");
				//document.getElementById('lblErrorMessage').innerText="Number Cannot be Blank";
				document.getElementById('txtwideno').focus();
				return false;
			}
		
			if((document.getElementById('txtwideamt').value != "") && (document.getElementById('txtwideamt').value != 0))
			{
				totalamt = totalamt + eval(document.getElementById('txtwideamt').value)
			}
			else
			{
				alert("Please Enter Wide Amount");
				document.getElementById('txtwideamt').focus();
				return false;
			}
		}		
		
		if(document.getElementById('chkCash').checked==true)
		{
			if((document.getElementById('txtCashAmount').value != "") && (document.getElementById('txtCashAmount').value != 0))
			{
				totalamt = totalamt + eval(document.getElementById('txtCashAmount').value)
			}
			else
			{
				alert("Please Enter Cash Amount");
				document.getElementById('txtCashAmount').focus();
				return false;	
			}
		}
	
		if(document.getElementById('chkBillingAccount').checked==true)
		{
			if((document.getElementById('txtbillingamount').value != "") && (document.getElementById('txtbillingamount').value != 0))
			{
				totalamt = totalamt + eval(document.getElementById('txtbillingamount').value)
			}
			else
			{
				alert("Please Enter BillingAccount Amount");
				document.getElementById('txtbillingamount').focus();
				return false;	
			}
		}
		
		if((document.getElementById('chkBillingAccount').checked == false)&&(document.getElementById('chkCash').checked == false)&&(document.getElementById('chkwideoff').checked == false)&&(document.getElementById('chkwiretransfer').checked == false)&&(document.getElementById('chkcheque').checked == false)&&(document.getElementById('chkcard').checked == false))
		{
				alert("Select Atleast one payment option");
				return false;
		}
		
		document.getElementById('txtYourAmount').value = totalamt;
		return true;
	}
	if((document.getElementById('rdoMasterAccount').checked==false)&&(document.getElementById('rdootheraccount').checked==false))
	{
			alert("Atleast Select One Payment Option");
			return false;
	}
	
	if((document.getElementById('txtYourAmount').value == "") || (document.getElementById('txtYourAmount').value <= 0))
	{
		alert("Enter Some Amount");
		return false;
	}
	return true;
}


//Count the records of second greed of boothcheck page
function countgridrecord()
{
	objName="dgboothcheck";
	name=objName + "__ctl"
	
	if(document.all[objName] !=null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		if(rowCount<=1)
		{
			alert("No Records Available For Update");
			return false;
		}
	}
	return true;
}

//Function check the amount value while pressing the Pay button.
function gotopayment(p)
{
	var n=p.id.substr( p.id.indexOf("__ctl")+5 , p.id.indexOf("_imgPay") - p.id.indexOf("__ctl") - 5 );
	if (document.getElementById('dgboothcheck__ctl' + n + '_txtamt').value =='')
	{
		alert('Enter due amount before proceeding to payment');
		return false;
	}
	return true;
}


//Count records of first grid for checking the records befor pressing the Submit Btton
function countrecordafteradd()
{
	objName="dgbooth";
	name=objName + "__ctl";
	
	if(document.all[objName] != null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		
		if(rowCount <= document.getElementById('hdncount').value)
		{
			alert("Add New Violation before Submit");
			return false;
		}
		else
		{
			return true;
		}
	}
}

//Count the records of first grid at the initial stage when page is load.
function countrecord()
{
	objName="dgbooth";
	name=objName + "__ctl";
	
	if(document.all[objName] != null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		
		if(document.getElementById('hdnfacility').value == "")
		{
			document.getElementById('hdnfacility').value=document.getElementById('ddlFacility').value;
		}
		
		
		if((document.getElementById('hdnfacility').value != "") && (document.getElementById('hdnfacility').value != document.getElementById('ddlFacility').value))
		{
			document.getElementById('hdncount').value ="";
			document.getElementById('hdnfacility').value = document.getElementById('ddlFacility').value;
		}
	
		if(document.getElementById('hdncount').value == "")
		{
			document.getElementById('hdncount').value=rowCount;
		}
	}
	return true;
}

//Function call at the time of Delete the booth check Entry.
function DeleteBoothCheck()
{
	var ans;
	ans = confirm("Are you sure want to delete the record ?");
	
	if(ans==true)
	{
		return true;
	}
	else
	{
		return false;
	}
}


function MA_Selected()
{
	document.getElementById('txtMANumber').disabled = false;
	document.getElementById('txtMasAccAmount').disabled = false;
	
	document.getElementById('chkcard').checked=false;
	CC_Selected();
	
	document.getElementById('chkBillingAccount').checked=false;
	BA_Selected();
	
	document.getElementById('chkCash').checked=false;
	CS_Selected();
	
	document.getElementById('chkcheque').checked=false;
	CK_Selected();
	
	document.getElementById('chkwideoff').checked=false;
	WO_Selected();
	
	document.getElementById('chkwiretransfer').checked=false;
	WT_Selected();

	Chk_Enable(true);
}

function OA_Selected()
{
	document.getElementById('txtMANumber').disabled = true;
	document.getElementById('txtMasAccAmount').disabled = true;
	
	Chk_Enable(false);
}

function CC_Selected()
{
	var enb=true;
	if(document.getElementById('chkcard').checked==true)
		enb=false;
		
	document.getElementById('ddlPaymentType').disabled = enb;
	document.getElementById('txtPaymentNumber').disabled = enb;
	document.getElementById('ddlMonth').disabled = enb;
	document.getElementById('ddlYear').disabled = enb;
	document.getElementById('txtPaymentCode').disabled = enb;
	document.getElementById('txtPaymentName').disabled = enb;
	document.getElementById('txtCardAmount').disabled = enb;

	if (document.getElementById("ddlPaymentType").disabled == false)
		SetCursor('ddlPaymentType');
}

function BA_Selected()
{
	var enb=true;
	if(document.getElementById('chkBillingAccount').checked==true)
		enb=false;

	document.getElementById('txtbillingamount').disabled = enb;
	if (document.getElementById("txtbillingamount").disabled == false)
		SetCursor('txtbillingamount');
	
}

function CS_Selected()
{
	var enb=true;
	if(document.getElementById('chkCash').checked==true)
		enb=false;

	document.getElementById('txtCashAmount').disabled = enb;
	if (document.getElementById("txtCashAmount").disabled == false)
		SetCursor('txtCashAmount');
}

function CK_Selected()
{
	var enb=true;
	if(document.getElementById('chkcheque').checked==true)
		enb=false;

	document.getElementById('txtNumber').disabled = enb;
	document.getElementById('txtChequeAmount').disabled = enb;

	if (document.getElementById("txtNumber").disabled == false)
		SetCursor('txtNumber');
}

function WO_Selected()
{
	var enb=true;
	if(document.getElementById('chkwideoff').checked==true)
		enb=false;

	document.getElementById('txtwideno').disabled = enb;
	document.getElementById('txtwideamt').disabled = enb;

	if (document.getElementById("txtwideno").disabled == false)
		SetCursor('txtwideno');
}

function WT_Selected()
{

	var enb=true;
	if(document.getElementById('chkwiretransfer').checked==true)
		enb=false;

	document.getElementById('txtwireno').disabled = enb;
	document.getElementById('txtwireamt').disabled = enb;

	if (document.getElementById("txtwireno").disabled == false)
		SetCursor('txtwireno');
}

function Chk_Enable(enb)
{
	document.getElementById('chkcard').disabled=enb;
	document.getElementById('chkBillingAccount').disabled=enb;
	document.getElementById('chkCash').disabled=enb;
	document.getElementById('chkcheque').disabled=enb;
	document.getElementById('chkwideoff').disabled=enb;
	document.getElementById('chkwiretransfer').disabled=enb;
}


function MaxLen()
{
	if(document.getElementById('txtdesc').value.length>1000)
	{
		alert("Description can not have more than 1000 characters.");
		document.getElementById('txtdesc').focus();
	}
}