function callCalender(e)
{
var cal1 = new calendar2(document.forms['frmBookOrder'].elements[e]);
cal1.year_scroll = true;
cal1.time_comp = false;
cal1.popup();
}
function Validate()
{
	if (CheckBlank(frmBookOrder.txtRequestedDate))
	{
		document.getElementById('lblErrorMessage').innerText="Requested Delivery Date Cannot be Blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (compareDates(Trim(frmBookOrder.txtDate.value),"MM/dd/yyyy",Trim(frmBookOrder.txtRequestedDate.value),"MM/dd/yyyy") == 1) //It check Req Date is >= Today
	{
		document.getElementById('lblErrorMessage').innerText="Requested Delivery Date should be today's date or greater.";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (CheckBlank(frmBookOrder.txtRentalDays))
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
if (frmBookOrder.ddlStandardOrder.selectedIndex==0)
{
do
{
	oElement = document.getElementById(name + i + "_txtQty");
	if (oElement == null)
		{x=1;}
	else
	{
		if (document.frmBookOrder.elements(name + i + "_txtQty").value > 0)
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
		if ((document.frmBookOrder.elements(name + i + "_txtQty").value > 0) ||(document.frmBookOrder.elements(name + i + "_txtQty").value =''))
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