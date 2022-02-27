function callCalender(e)
{
var cal1 = new calendar2(document.forms['frmEditOrder'].elements[e]);
cal1.year_scroll = true;
cal1.time_comp = false;
cal1.popup();
}
function ValidateSplit()
{
/*	if ((frmEditOrder.txtStatus.value=='A') || (frmEditOrder.txtStatus.value=='T') || (frmEditOrder.txtStatus.value=='C'))
	{
		document.getElementById('lblErrorMessage').innerText="You cannot Split the Order after Assigning";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else */ if (confirm("Are you sure you want to split this Order?")==false)
	{
		return false;
	}
	return true;
}

function Validate()
{
	if (CheckBlank(frmEditOrder.txtRequestedDate))
	{
		document.getElementById('lblErrorMessage').innerText="Requested Delivery Date Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (compareDates(Trim(frmEditOrder.txtDate.value),"MM/dd/yyyy",Trim(frmEditOrder.txtRequestedDate.value),"MM/dd/yyyy") == 1)
	{
		document.getElementById('lblErrorMessage').innerText="Requested Delivery Date should be Greater than Today's Date.";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmEditOrder.txtRentalDays))
	{
		document.getElementById('lblErrorMessage').innerText="Rental Days Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (!CheckOrder())
	{
		document.getElementById('lblErrorMessage').innerText="Please Select either Standard Order or Custom Order";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}

function CheckOrder()
{
var name="dgSku__ctl";
var x=0;
var i=2;
if (frmEditOrder.ddlStandardOrder.selectedIndex==0)
{
do
{
	oElement = document.getElementById(name + i + "_txtQty");
	if (oElement == null)
		{x=1;}
	else
	{
		if (document.frmEditOrder.elements(name + i + "_txtQty").value > 0)
		{
			return true;
		}
	}
	i++;
}
while(x<1);
return false;
}
else
{
do
{
	oElement = document.getElementById(name + i + "_txtQty");
	if (oElement == null)
		{x=1;}
	else
	{
		if ((document.frmEditOrder.elements(name + i + "_txtQty").value > 0) ||(document.frmEditOrder.elements(name + i + "_txtQty").value =''))
		{
			return false;
		}
	}
	i++;
}
while(x<1);
return true;
}
}