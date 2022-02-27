function ItemLoad()
{
	MM_swapImage('Image4','','/exposervicedesk/Images/tabs/item-r.gif',1);
	
	var oElement
	oElement = document.getElementById("lstCat1");
	if (oElement.disabled == true)
		SetCursor('txtManufactureId');
	else
		SetCursor('lstCat1');
		
	if (document.forms[0].hdnflg.value == "2")
	{
		//page is reloaded so display selected categories
		lstcat1=document.getElementById('lstCat1')
		for(i=0; i<lstcat1.length; i++)
		{
			if (lstcat1[i].value == selCat1)
				lstcat1[i].selected = true;
		}
		lstCat1_onchange(selCat2);
		lstCat2_onchange(selCat3);

		if (document.getElementById("txtManufactureId").disabled == false)
			SetCursor('txtManufactureId');
	}
}

function lstCat1_onchange(p)
{
	lstcat1=document.getElementById('lstCat1');
	lstcat2=document.getElementById('lstCat2');
	
	lstcat2.length=(0);
	document.getElementById('lstCat3').length=(0);
	for (i=0;i<cat2.length;i+=3)
	{	
		if(lstcat1.value == cat2[i])
		{
			lstcat2.options[lstcat2.length]=new Option(cat2[i+2],cat2[i+1]);
			if(cat2[i+1] == p) 
				lstcat2[lstcat2.length-1].selected = true;
		}
	}
	return true;
}

function lstCat2_onchange(p)
{
	lstcat2=document.getElementById('lstCat2');
	lstcat3=document.getElementById('lstCat3');
	
	lstcat3.length=(0);
	for (i=0;i<cat3.length;i+=3)
	{
		if(lstcat2.value == cat3[i])
		{
			lstcat3.options[lstcat3.length]=new Option(cat3[i+2],cat3[i+1]);
			if(cat3[i+1] == p) 
				lstcat3[lstcat3.length-1].selected = true;
		}
	}
	return true;
}

function lstCat3_onchange(p)
{
	document.forms[0].hdnflg.value="1";
	document.forms[0].submit();
	return true;
}

function OnlyRate()
{
	var ratevalue='';
	ratevalue = document.Items.elements(event.srcElement.name).value;
	//alert(event.keyCode);
	if (onlyRate(ratevalue) == true)
	{
		return true;
	}
	else
	{
		return false;
	}
}

function Validate()
{
	if (document.getElementById('lstCat1').value=="")
	{
		alert("Specify Category before saving Item");
		document.getElementById('lstCat1').focus();
		return false;
	}
	if (document.getElementById('lstCat2').value=="")
	{
		alert("Specify SubCategory 1 before saving Item");
		document.getElementById('lstCat2').focus();
		return false;
	}
	if (document.getElementById('lstCat3').value=="")
	{
		alert("Specify SubCategory 2 before saving Item");
		document.getElementById('lstCat3').focus();
		return false;
	}
	
	if (CheckBlank(document.getElementById('txtPartNo')))
	{
		alert("Part # Cannot be Blank");
		document.getElementById('txtPartNo').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtName'))) 
	{
		alert("Name Cannot be Blank");
		document.getElementById('txtName').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtShortDesc'))) 
	{
		alert("Short Description Cannot be Blank");
		document.getElementById('txtShortDesc').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtPrice')))
	{
		alert("Early Price Cannot be Blank");
		document.getElementById('txtPrice').focus();
		return false;
	}
	else if (!IsNumber(document.getElementById('txtPrice').value))
	{
		alert("Early Price Cannot be Alphanumeric Value");
		document.getElementById('txtPrice').focus();
		return false;
	}
	else if (eval(document.getElementById('txtPrice').value)>9999)
	{
		alert("Early Price Cannot be more than 9999");
		document.getElementById('txtPrice').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtLateprice')))
	{
		alert("Regular Price Cannot be Blank");
		document.getElementById('txtLateprice').focus();
		return false;
	}
	else if (!IsNumber(document.getElementById('txtLateprice').value))
	{
		alert("Regular Price Cannot be Alphanumeric Value");
		document.getElementById('txtLateprice').focus();
		return false;
	}
	else if (eval(document.getElementById('txtLateprice').value)>9999)
	{
		alert("Regular Price Cannot be more than 9999");
		document.getElementById('txtLateprice').focus();
		return false;
	}
	//dont uncomment the following - discount is hidden
	/*else if (document.getElementById('txtDiscount').value!='')
	{
		if (!IsNumber(document.getElementById('txtDiscount').value))
		{
			alert("Discount Cannot be Alphanumeric Value");
			document.getElementById('txtDiscount').focus();
			return false;
		}
		else if(eval(document.getElementById('txtDiscount').value)>100)
		{
			alert("Discount Cannot be more than 100%");
			document.getElementById('txtDiscount').focus();
			return false;
		}
	}*/
	else if (eval(document.getElementById('txtPrice').value) > eval(document.getElementById('txtLateprice').value))
	{
		alert("Early Price Cannot be greater than Regulare Price");
		document.getElementById('txtPrice').focus();
		return false;
	}
	else if (!CheckMaxLength(document.getElementById('txtFullDesc'),300))
	{
		alert("Full Description Cannot be more than 300 characters");
		document.getElementById('txtFullDesc').focus();
		return false;
	}
	return true;
}			
		
function winopen(val)
{

	var ImageName;
	var FilePath;
	ImageName = "/exposervicedesk/images/upload/" + document.getElementById(val).innerText;
	FilePath = "/exposervicedesk/HeaderImage.aspx?img=" + ImageName;
	//window.open (FilePath,"mywindow","width=400,height=400,status=yes,toolbar=no,menubar=no,scrollbars=1");
	window.open (FilePath,"mywindow","left=0,top=0,width=400,height=400,status=yes,toolbar=no,menubar=no,resizable=1,scrollbars=1");
	return false;
}	
	
