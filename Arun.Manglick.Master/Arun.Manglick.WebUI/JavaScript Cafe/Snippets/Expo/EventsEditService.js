function EventsEditServiceLoad()
{
	SetCursor('ddlCategory');
}

function AddItems()
{
	var objName="dgItems";		
	var name=objName + "__ctl";
	var rowItems = eval(objName).rows;
	var rowCount = rowItems.length;

	for (i=2; i<=rowCount; i++)
	{
		oElement = document.getElementById(name + i + "_chkAdd");
		if(oElement!=null)
			oElement.checked = true;
	}
	return false;
}

function RemoveItems()
{
	var objName="dgItems";		
	var name=objName + "__ctl";
	var rowItems = eval(objName).rows;
	var rowCount = rowItems.length;

	for (i=2; i<=rowCount; i++)
	{
		oElement = document.getElementById(name + i + "_chkDel");
		if(oElement!=null)
			oElement.checked = true;
	}
	return false;
}

function Validate()
{
	var objName="dgItems";		
	var name=objName + "__ctl";
	var rowItems = eval(objName).rows;
	var rowCount = rowItems.length;

	for (i=3; i<=rowCount; i++)
	{
		oElement = document.getElementById(name + i + "_chkAdd");
		if (oElement)
		{
			if(oElement.checked == true)
			{		
				oElement = document.getElementById(name + i + "_txtPrice");
				if (!IsNumber(oElement.value))
				{
					alert('Early Price should be Numeric');
					oElement.focus();
					return false;
				}
				if (eval(oElement.value)>9999)
				{
					alert("Early Price Cannot be more than 9999");
					oElement.focus();
					return false;
				}
				
				oElement = document.getElementById(name + i + "_txtLPrice");
				if (!IsNumber(oElement.value))
				{
					alert('Regular Price should be Numeric');
					oElement.focus();
					return false;
				}
				if (eval(oElement.value)>9999)
				{
					alert("Regular Price Cannot be more than 9999");
					oElement.focus();
					return false;
				}
				
				/* dont uncomment - business rule changed
				oElement = document.getElementById(name + i + "_txtDiscount");
				if (!IsNumber(oElement.value))
				{
					alert('Discount value should be Numeric');
					oElement.focus();
					return false;
				}
				if (eval(oElement.value)>100)
				{
					alert("Discount Cannot be more than 100%");
					oElement.focus();
					return false;
				}
				*/
				else if (eval(document.getElementById(name + i + "_txtPrice").value) > eval(document.getElementById(name + i + "_txtLPrice").value))
				{
					alert("Early Price Cannot be greater than Regulare Price");
					document.getElementById(name + i + "_txtPrice").focus();
					return false;
				}
			}
		}
	}
	return true;
}