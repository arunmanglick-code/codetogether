function RelationLoad()
{
	SetCursor('ddlFacility');
	controlState(0,0)	
}

function Validate()
{
	var bOneSel;
	var objName="dgCategory";		
	var name=objName + "__ctl";
	var rowItems = eval(objName).rows;
	var rowCount = rowItems.length;
	for (i=2; i<= rowCount; i++)
	{
		oElement = document.getElementById(name + i + "_chkDefault");
		if(oElement.checked == true)
		{
			bOneSel=true;
			oElement = document.getElementById(name + i + "_ddlVendor");
			if (oElement.value=='')
			{
				alert('Must select Service Provider, if Category is a default for Facility');
				oElement.focus();
				return false;
			}
			oElement = document.getElementById(name + i + "_txtFacCom");
			if(oElement.value=='')
			{
				alert('Must specify commission value for Facility, if Category is a default for Facility');
				oElement.focus();
				return false;
			}
			else if (!IsNumber(oElement.value))
			{
				alert('Commission value for Facility should be Numeric');
				oElement.focus();
				return false;
			}
			oElement = document.getElementById(name + i + "_txtProCom");
			if(oElement.value=='')
			{
				alert('Must specify commission value for Service Provider, if Category is a default for Facility');
				oElement.focus();
				return false;
			}
			else if (!IsNumber(oElement.value))
			{
				alert('Commission value for Service Provider should be Numeric');
				oElement.focus();
				return false;
			}
			oElement = document.getElementById(name + i + "_txtTeaCom");
			if(oElement.value=='')
			{
				alert('Must specify commission value for Team Co, if Category is a default for Facility');
				oElement.focus();
				return false;
			}
			else if (!IsNumber(oElement.value))
			{
				alert('Commission value for Team Co should be Numeric');
				oElement.focus();
				return false;
			}
			oElement = document.getElementById(name + i + "_txtCatTax");
			if(oElement.value!='')
			{
				if(oElement.value>100)
				{
					alert('Tax value should be less than or equal to 100%');
					oElement.focus();
					return false;
				}
				if (!IsNumber(oElement.value))
				{
					alert('Tax value should be Numeric');
					oElement.focus();
					return false;
				}
			}
			var str=String(eval(document.getElementById(name + i + "_txtFacCom").value)+eval(document.getElementById(name + i + "_txtProCom").value)+eval(document.getElementById(name + i + "_txtTeaCom").value));			
			str=eval(str.substring(0,6));
//			if (100!=eval(document.getElementById(name + i + "_txtFacCom").value)+eval(document.getElementById(name + i + "_txtProCom").value)+eval(document.getElementById(name + i + "_txtTeaCom").value))
			if (100!=str)
			{
				alert('Commission value should be 100%');
				return false;
			}			
		}
	}
	if (bOneSel!=true)
	{
		alert('Must select atleast one Category with its Service Provider and commission values for Facility, Service Provider, Team Co');
		return false;
	}
	return true;
}

function changeStatus(objRow)
{
	//get id number from  dgCategory__ctl5_chkDefault 
	i=objRow.id.substring(15,objRow.id.length);
	i=i.substring(0,i.length-11);

	name="dgCategory__ctl";
	if(objRow.checked == false)
	{
		oElement = document.getElementById(name + i + "_ddlVendor");
		oElement.value =''

		oElement = document.getElementById(name + i + "_txtFacCom");
		oElement.value = ''

		oElement = document.getElementById(name + i + "_txtProCom");
		oElement.value =''

		oElement = document.getElementById(name + i + "_txtTeaCom");
		oElement.value =''

		oElement = document.getElementById(name + i + "_txtCatTax");
		oElement.value =''
	}
	controlState(i,i);
	changeBG(objRow,'Del');
	return true;
}

function controlState(st,end)
{
	var objName="dgCategory";		
	var name=objName + "__ctl";

	if(st==0 && end==0)
	{
		st = 2;
		end = eval(objName).rows;
		end = end.length;
	}
	
	for (i=st; i<= end; i++)
	{
		oElement = document.getElementById(name + i + "_chkDefault");
		if(oElement.checked == false)
			vis="none";
		else
		{
			vis="block";
			changeBG(oElement,'Del');
		}
			
		oElement = document.getElementById(name + i + "_ddlVendor");
		oElement.runtimeStyle.display=vis

		oElement = document.getElementById(name + i + "_txtFacCom");
		oElement.runtimeStyle.display=vis

		oElement = document.getElementById(name + i + "_txtProCom");
		oElement.runtimeStyle.display=vis

		oElement = document.getElementById(name + i + "_txtTeaCom");
		oElement.runtimeStyle.display=vis

		oElement = document.getElementById(name + i + "_txtCatTax");
		oElement.runtimeStyle.display=vis
	}
}