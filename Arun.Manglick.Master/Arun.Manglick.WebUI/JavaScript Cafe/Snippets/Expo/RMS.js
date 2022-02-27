function checktime(thetime) 
{
	var a,b,c,f,err=0;
	a=thetime.value;
	if (a.length != 5) err=1;
	b = a.substring(0, 2);
	c = a.substring(2, 3); 
	f = a.substring(3, 5); 
	if (/\D/g.test(b)) err=1; //not a number
	if (/\D/g.test(f)) err=1; 
	if (b<0 || b>23) err=1;
	if (f<0 || f>59) err=1;
	if (c != ':') err=1;
	if (err==1) 
	{
		return false
	}
		return true
}
		
		
function WidthChange()
{
	if (document.getElementById("txtWidth").value != '' && document.getElementById("txtLength").value != '')
	{
		document.getElementById("txttotal").value = (document.getElementById("txtWidth").value *  document.getElementById("txtLength").value);
	}
	else if (document.getElementById("txtWidth").value != '' && document.getElementById("txtLength").value == '')
	{
		document.getElementById("txttotal").value ='';
	}
	else
	{
		document.getElementById("txttotal").value ='';
	}
	return true;

}


function LenghtChange()
{
	if (document.getElementById("txtWidth").value != '' && document.getElementById("txtLength").value != '')
	{
		document.getElementById("txttotal").value = (document.getElementById("txtWidth").value *  document.getElementById("txtLength").value);
	}
	else if (document.getElementById("txtWidth").value != '' && document.getElementById("txtLength").value == '')
	{
		document.getElementById("txttotal").value ='';
	}
	else
	{
		document.getElementById("txttotal").value ='';
	}
	return true;
}

function ValidateLabor()
{
	if (CheckBlank(document.getElementById('txtDate')))
	{
		alert("Date Cannot be Blank.");
		document.getElementById('txtDate').focus();
		return false;
	}
	else if (isDate(document.getElementById('txtDate').value,'M/d/yyyy') == false)			
	{
		alert("Date should be in MM/DD/YYYY Format.");
		document.getElementById('txtDate').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtTimeIn'))) 
	{
		alert("TimeIn Cannot be Blank.");
		document.getElementById('txtTimeIn').focus();
		return false;
	}
	else if (checktime(document.getElementById('txtTimeIn')) == false)			
	{
		alert("Time should be in HH:MM Format.");
		document.getElementById('txtTimeIn').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtTimeOut')))
	{
		alert("TimeOut Cannot be Blank.");
		document.getElementById('txtTimeOut').focus();
		return false;
	}
	else if (checktime(document.getElementById('txtTimeOut')) == false)			
	{
		alert("Time should be in HH:MM Format.");
		document.getElementById('txtTimeOut').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtOff'))) 
	{
		alert("Off Cannot be Blank.");
		document.getElementById('txtOff').focus();
		return false;
	}
	else if (!IsNumber(document.getElementById('txtOff').value))
	{
		alert('Invalid value for Off');
		document.getElementById('txtOff').focus();
		return false;
	}
	else if (eval(document.getElementById('txtOff').value)>24)
	{
		alert('Invalid value for Off');
		document.getElementById('txtOff').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtST')))
	{
		alert("Straight Time Cannot be Blank.");
		document.getElementById('txtST').focus();
		return false;
	}
	else if (!IsNumber(document.getElementById('txtST').value))
	{
		alert('Invalid value for Straight Time');
		document.getElementById('txtST').focus();
		return false;
	}
	else if (eval(document.getElementById('txtST').value)>999)
	{
		alert('Invalid value for Straight Time');
		document.getElementById('txtST').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtOT')))
	{
		alert("Over Time Cannot be Blank.");
		document.getElementById('txtOT').focus();
		return false;
	}
	else if (!IsNumber(document.getElementById('txtOT').value))
	{
		alert('Invalid value for Over Time');
		document.getElementById('txtOT').focus();
		return false;
	}
	else if (eval(document.getElementById('txtOT').value)>999)
	{
		alert('Invalid value for Over Time');
		document.getElementById('txtOT').focus();
		return false;
	}
	if (document.getElementById('txtOff').value == "0" && document.getElementById('txtST').value =="0" &&
		document.getElementById('txtOT').value =="0")
	{
		alert("Invalid Labor Record");
		return false;
	}
	return true;
}


function ValidateMaterial()
{
	if (CheckBlank(document.getElementById('txtDays')))
	{
		alert("Days Cannot be Blank.");
		document.getElementById('txtDays').focus();
		return false;
	}
	else if (!IsDigit(document.getElementById('txtDays').value))
	{
		alert('Invalid value for Days');
		document.getElementById('txtDays').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtSQFT'))) 
	{
		alert("SQ.FT Cannot be Blank.");
		document.getElementById('txtSQFT').focus();
		return false;
	}
	else if (!IsDigit(document.getElementById('txtSQFT').value))
	{
		alert('Invalid value for SQ.FT');
		document.getElementById('txtSQFT').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtQty'))) 
	{
		alert("Quantity Cannot be Blank.");
		document.getElementById('txtQty').focus();
		return false;
	}
	else if (!IsDigit(document.getElementById('txtQty').value))
	{
		alert('Invalid value for Qunatity');
		document.getElementById('txtQty').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtUnitPrice')))
	{
		alert("Unit Price Cannot be Blank.");
		document.getElementById('txtUnitPrice').focus();
		return false;
	}
	else if (!IsNumber(document.getElementById('txtUnitPrice').value))
	{
		alert('Invalid value for Unit Price');
		document.getElementById('txtUnitPrice').focus();
		return false;
	}
	else if (eval(document.getElementById('txtUnitPrice').value)>999)
	{
		alert('Invalid value for Unit Price');
		document.getElementById('txtUnitPrice').focus();
		return false;
	}
	if (document.getElementById('txtDays').value =="0" && document.getElementById('txtSQFT').value =="0" &&
		document.getElementById('txtQty').value =="0" && document.getElementById('txtUnitPrice').value =="0")
	{
		alert("Invalid Material Record");
		return false;
	}
	return true;
}


function OnlyRate()
{
	var ratevalue='';
	ratevalue = document.RMS.elements(event.srcElement.name).value;
	
	if (onlyRate(ratevalue) == true)
	{
		return true;
	}
	else
	{
		return false;
	}
}


//CheckMaxLength
function Validate()
{
	if (document.getElementById('ddlOrderNo').value==0)
		return false;
		
	//Validation for Item Grid
	document.getElementById('lblErrorMessage').innerText='';
	var name="dgOrder__ctl";
	
	if (document.all["dgOrder"] !=null)
	{
		var rowItems = eval(dgOrder).rows;
		var rowCount = rowItems.length;

		for (i=2; i<= rowCount; i++)
		{
			oElement = document.getElementById(name + i + "_ddlLaborType");
			if (oElement != null)
			{
				if (document.RMS.elements(name + i + "_ddlLaborType").value == 0)
				{
					alert("Please select Labor Type.");
					document.getElementById(name + i + "_ddlLaborType").focus();
					return false;
				}
			}
		}
	}		
		
	//Validation for Booth Grid
	objName="dgBooth";
	name=objName + "__ctl"
	
	if (document.all[objName] !=null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		
		for (i=2; i<= rowCount; i++)
		{
			for (j=1; j<11; j++)
			{	
				oElement = document.getElementById(name + i + "_txtCol" + j);
				if(!CheckSpecialChar(oElement))
				{
					alert("Indicate Power Location on the grid provided and using appropriate symbols");
					oElement.focus();
					return false;	
				}
			}	
		}
	}
		
	//txtBooth4
	if(CheckBlank(document.getElementById('txtBooth1'))) 
	{
		alert("Booth Cannot be Blank.");
		document.getElementById('txtBooth1').focus();
		return false;
	}
	if (!CheckSpecialChar(document.getElementById('txtBooth1')))
	{
		alert("Invalide value for Booth Number");
		document.getElementById('txtBooth1').focus();
		return false;	
	}
	if (CheckBlank(document.getElementById('txtBooth2')))
	{
		alert("Booth Cannot be Blank.");
		document.getElementById('txtBooth2').focus();
		return false;
	}			
	if (!CheckSpecialChar(document.getElementById('txtbooth2')))
	{
		alert("Invalide value for Booth Number");
		document.getElementById('txtbooth2').focus();
		return false;	
	}
	if (CheckBlank(document.getElementById('txtBooth3')))
	{
		alert("Booth Cannot be Blank.");
		document.getElementById('txtBooth3').focus();
		return false;
	}		
	if (!CheckSpecialChar(document.getElementById('txtBooth3')))
	{
		alert("Invalide value for Booth Number");
		document.getElementById('txtBooth3').focus();
		return false;	
	}
	if (CheckBlank(document.getElementById('txtBooth4')))
	{
		alert("Booth Cannot be Blank.");
		document.getElementById('txtBooth4').focus();
		return false;
	}							
	if (!CheckSpecialChar(document.getElementById('txtBooth4')))
	{
		alert("Invalide value for Booth Number");
		document.getElementById('txtBooth4').focus();
		return false;	
	}
	if (CheckBlank(document.getElementById('txtWidth')))
	{
		alert("Width Cannot be Blank.");
		document.getElementById('txtWidth').focus();
		return false;
	}
	if (!IsDigit(document.getElementById('txtWidth').value))
	{
		alert('Booth Size should be Decimal value');
		document.getElementById('txtWidth').focus();
		return false;
	}
	if (document.getElementById('txtWidth').value == "0")
	{
		alert('Invalid value for Booth Size');
		document.getElementById('txtWidth').focus();
		return false;
	}
	if (CheckBlank(document.getElementById('txtLength')))
	{
		alert("Length Cannot be Blank.");
		document.getElementById('txtLength').focus();
		return false;
	}
	if (!IsDigit(document.getElementById('txtLength').value))
	{
		alert('Booth Size should be Decimal value');
		document.getElementById('txtLength').focus();
		return false;
	}
	if (document.getElementById('txtLength').value =="0")
	{
		alert('Invalid value for Booth Size');
		document.getElementById('txtLength').focus();
		return false;
	}

	document.getElementById('txttotal').value = document.getElementById('txtLength').value * document.getElementById('txtWidth').value;
	if (!(CheckBlank(document.getElementById('txtNotes'))))
	{
		if (CheckMaxLength(document.getElementById('txtNotes'),1000) == false)
		{
		alert("Notes should be up to 1000 Character.");
		document.getElementById('txtNotes').focus();
		return false;
		}
	}
	return true;		
}




function DeleteConform()
{
	var bresult
	bresult = confirm("Are you sure to delete this record.")
	return bresult;

}
