
function OnlyInteger()
{
	var ratevalue='';
	ratevalue = document.Order.elements(event.srcElement.name).value;
	if (IsNumbervalue(ratevalue) == true)
		return true;
	else
		return false;
}

function IsNumbervalue(p)
{		
	if (!(event.keyCode >= '48' && event.keyCode <= '57'))
	{
		event.keyCode=0;
	}
}

function OnlyRate()
{
	var ratevalue='';
	ratevalue = document.Order.elements(event.srcElement.name).value;
	if (onlyRate(ratevalue) == true)
	{
		return true;
	}
	else
	{
		return false;
	}
}
function OrderLoad(usrtype)
{
	if(usrtype == 2)	// employee user 
		MM_swapImage('Image32','','/exposervicedesk/Images/tabs/order-r.gif',1);
	else if(usrtype == 3)				// online user
		MM_swapImage('Image42','','/exposervicedesk/Images/tabs/laborrequest-r.gif',1);
	
	SetCursor('txtBooth');
	
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
	}
	
}

function OrderAdjustmentLoad(usrtype)
{
	MM_swapImage('Image32','','/exposervicedesk/Images/tabs/order-r.gif',1);
	
	SetCursor('ddlOrderNo');
	
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
	hdnflg=document.getElementById('hdnflg');
	
	document.forms[0].hdnflg.value="1";
	document.forms[0].submit();
	return true;
}

function ShowBigImage(imgValue)
{
	ImageName = "/exposervicedesk/images/upload/" + imgValue
	FilePath = "/exposervicedesk/HeaderImage.aspx?img=" + ImageName;
	window.open (FilePath,"bigimgwindow","width=400,height=400,status=yes,toolbar=no,menubar=no,scrollbars=1");
	return false;
}

function DeleteItem()
{
	objName="dgCartItem";
	name=objName + "__ctl";
	
	if(document.all[objName] != null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		var flg=0;
				
		for (i=2; i<= rowCount; i++)
		{
			oElement = document.getElementById(name + i + "_chkDelete");
			
			if(oElement.checked)
			{
				flg=1;
				break;
			}
			else
			{
				flg=0;
			}
		}
				
		if(flg==0)
		{
			alert("Select atleast one item from the list");
			return false;
		}
		else
		{
			p=confirm('Are you sure want to delete the items ?')
			if(p==false)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
	else
	{
		alert("No Items Available");
		return false;
	}
}

function ShowLaborMsg(obj)
{
	if(obj.checked == true)
	{
		p=confirm('This item requires a Labor.' + '\n' + 'Want to continue adding this item?')
		if(p==false)
			return false;
		else
			return true;
	}
}

function CheckReturn()
{
	if(window.event.keyCode == 13)
	{
		SearchItem();
		return false;
	}
	return true;
}


function SearchItem()
{
	var i,j,k,l;
	search=document.getElementById('txtSearch');
	for (i=0;i<item.length;i+=3)
	{
		if(search.value.toUpperCase() == item[i+2].substring(0,search.value.length).toUpperCase())
		{	
			for (j=0;j<cat3.length;j+=3)
			{
				if(cat3[j+1] == item[i])		//item[i] is itmcatid		i.e. cat3
				{
					for (k=0;k<cat2.length;k+=3)
					{		
						if(cat2[k+1] == cat3[j])		//cat3[j] is cat2id		i.e. cat2
						{
							for (l=0;l<cat1.length;l+=2)
							{
								if(cat1[l] == cat2[k])		//cat2[k] is cat1id		i.e. cat1
								{
									lstcat1=document.getElementById('lstCat1');
									lstcat1[l/2].selected = true;
									lstCat1_onchange(cat2[k+1]);
									break;
								}
							}
							lstCat2_onchange(cat3[j+1]);
							break;
						}
					}
					lstCat3_onchange(item[i+1]);
					break;
				}																	
			}
			break;			
		}
	}
	return false;
}

function AddCart()
{
	objName="dgitems";
	name=objName + "__ctl";
		
	if(document.all[objName] != null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		var flg=0;
				
		for (i=2; i<= rowCount; i++)
		{
			oElement = document.getElementById(name + i + "_chkAdd");
			
			if(oElement.checked)
			{
				flg=1;
				break;
			}
			else
			{
				flg=0;
			}
		}
		if(flg==0)
		{
			alert("Select atleast one item from the list");
			return false;
		}
	}
	else
	{
		alert("No Items Available");
		return false;
	}
	return true;
}

function UpdateCart()
{
	objName="dgCartItem";
	name=objName + "__ctl"
	subtotal=0;
	appsumamt=0;
	
	if (document.all[objName] !=null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		
		if(rowCount >= 2)
		{
			for (i=2; i<= rowCount; i++)
			{
				var days;
				oElement = document.getElementById(name + i + "_txtdaysReq");
				if(oElement != null)
				{
					days=oElement.value;
					if(days == "")
					{
						days = 1;
						oElement.value = 1;
					}
					if(days == 0)
					{
						days = 1;
						oElement.value = 1;
					}
				}
				else
				{
					days=1;
				}
				
				oElement = document.getElementById(name + i + "_txtQty");
				qty=oElement.value;
				if(qty == "")
				{
					qty = 1;
					oElement.value = 1;
				}
				if(qty == 0)
				{
					qty = 1;
					oElement.value = 1;
				}
				
				
				var price;
				oElement = document.getElementById(name + i + "_rdoPrice");
				if(oElement != null)
				{
					if(oElement.checked == true)
					{
						oElement = document.getElementById(name + i + "_lbldis");
						dis=oElement.innerHTML;
						
						oElement = document.getElementById(name + i + "_txtPrice");
						price=oElement.value;
						
						if(dis != 0)
						{
							vdis = eval(price) *(eval(dis)/100);
							price = eval(price) - eval(vdis);
						}
					}
					else
					{
						oElement = document.getElementById(name + i + "_lbldis");
						dis=oElement.innerHTML;
						
						oElement = document.getElementById(name + i + "_txtlateprice");
						price=oElement.value;
						
						if(dis != 0)
						{
							vdis = eval(price) *(eval(dis)/100);
							price = eval(price) - eval(vdis);
						}
					}
				}
				else
				{
					if (document.getElementById(name + i + "_txtPrice") != null)
						price = document.getElementById(name + i + "_txtPrice").value;
					else if (document.getElementById(name + i + "_txtlateprice") != null)
						price = document.getElementById(name + i + "_txtlateprice").value;
				}
								
				var sqrftg = document.getElementById(name + i + "_txtSqrftg");				
				oElement = document.getElementById(name + i + "_txtSqrftg");
				if(oElement != null)
				{
					sqrftg = oElement.value;
					if(sqrftg == "")
					{
						sqrftg=1;
						oElement.value=1;
					}
					if(sqrftg == 0)
					{
						sqrftg=1;
						oElement.value=1;
					}
				}
				else
				{
					sqrftg=1;
				}
								
				oElement = document.getElementById(name + i + "_txtTotal");
				oElement.value=days * qty * price * sqrftg;
				
				totalval = oElement.value;
							
				oElement = document.getElementById(name + i + "_lblapptax");
				apptax = oElement.innerHTML;
				
				if(apptax != 0)
				{
					apptaxamt = (totalval * apptax) / 100;
					appsumamt = eval(appsumamt) + eval(apptaxamt);
					
					oElement = document.getElementById("txtApplicationtax");
					oElement.value=Math.round(appsumamt*100)/100;
				}
				else
				{
					oElement = document.getElementById("txtApplicationtax");
					oElement.value=0
				}
				
				subtotal = eval(subtotal) + eval(totalval);
				
				oElement = document.getElementById("txtOrdersubtotal");
				oElement.value=Math.round(subtotal*100)/100;
				
				oElement = document.getElementById("txtTotOrder");
				ftotal = eval(subtotal)+eval(appsumamt);
				oElement.value=Math.round(ftotal*100)/100;
			}
		}
		else
		{
				oElement = document.getElementById("txtOrdersubtotal");
				oElement.value=0;
				
				oElement = document.getElementById("txtApplicationtax");
				oElement.value=0;
				
				oElement = document.getElementById("txtTotOrder");
				oElement.value=0;
		}
		
		//only for order adjustment
		if (document.getElementById('txtDiff') != null)
		{
			oElement = document.getElementById('txtDiff')
			oElement.value = Math.round((document.getElementById('txtOriginalOrder').value - document.getElementById('txtTotOrder').value)*100)/100;
		}
	}
	else
		alert("No Items Available in Shopping Cart");

	return false;
}

function Calculatesubtotal()
{

	objName="dgCartItem";
	name=objName + "__ctl"
	subtotal=0;
	appsumamt=0;
	if (document.all[objName] !=null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		
		if(rowCount >= 2)
		{
			for (i=2; i<= rowCount; i++)
			{
				oElement = document.getElementById(name + i + "_txtdaysReq");
				if(oElement != null)
				{
					days=oElement.value;
					if(days == "")
					{
						days = 1;
						oElement.value=1;
					}
					if(days == 0)
					{
						days = 1;
						oElement.value=1;
					}
				}
				else
				{
					days=1;
				}
				
				oElement = document.getElementById(name + i + "_txtQty");
				qty=oElement.value;
				if(qty == "")
				{
					qty = 1;
					oElement.value=1;
				}
				if(qty == 0)
				{
					qty = 1;
					oElement.value=1;
				}				
								
				oElement = document.getElementById(name + i + "_rdoPrice");
				if(oElement.checked == true)
				{
					oElement = document.getElementById(name + i + "_lbldis");
					dis=oElement.innerHTML;
					
					oElement = document.getElementById(name + i + "_txtPrice");
					price=oElement.value;
					
					if(dis != 0)
					{
						vdis = eval(price) *(eval(dis)/100);
						price = eval(price) - eval(vdis);
					}
				}
				else
				{
					oElement = document.getElementById(name + i + "_lbldis");
					dis=oElement.innerHTML;
					
					oElement = document.getElementById(name + i + "_txtlateprice");
					price=oElement.value;
					
					if(dis != 0)
					{
						vdis = eval(price) *(eval(dis)/100);
						price = eval(price) - eval(vdis);
					}
				}
								
				oElement = document.getElementById(name + i + "_txtSqrftg");
				if(oElement != null)
				{
					sqrftg = oElement.value;
					if(sqrftg == "")
					{
						sqrftg=1;
						oElement.value=1;
					}
					if(sqrftg == 0)
					{
						sqrftg=1;
						oElement.value=1;
					}
				}
				else
				{
					sqrftg=1;
				}
								
				oElement = document.getElementById(name + i + "_txtTotal");
				oElement.value=days * qty * price * sqrftg;
				
				totalval = oElement.value;
							
				oElement = document.getElementById(name + i + "_lblapptax");
				apptax = oElement.innerHTML;
								
				if(apptax != 0)
				{
					apptaxamt = (totalval * apptax) / 100;
					appsumamt = eval(appsumamt) + eval(apptaxamt);
					
					oElement = document.getElementById("txtApplicationtax");
					oElement.value=Math.round(appsumamt*100)/100;
				}
				else
				{
					oElement = document.getElementById("txtApplicationtax");
					oElement.value=0
				}
								
				subtotal = eval(subtotal) + eval(totalval);
				
				oElement = document.getElementById("txtOrdersubtotal");
				oElement.value=Math.round(subtotal*100)/100;
				subchecktotal = Math.round(subtotal*100)/100;
				
				oElement = document.getElementById("txtTotOrder");
				ftotal = eval(subtotal)+eval(appsumamt);
				oElement.value=Math.round(ftotal*100)/100;
			}
			return true;
		}
		else
		{
				oElement = document.getElementById("txtOrdersubtotal");
				oElement.value=0;
				
				oElement = document.getElementById("txtApplicationtax");
				oElement.value=0;
				
				oElement = document.getElementById("txtTotOrder");
				oElement.value=0;
				
				return false;
		}
		
		return true;
	}
	else
	{
		alert("No Items Available in Shopping Cart");
		return false;
	}
}


//For Customer
function UpdateCartCustomer()
{
	objName="dgCartItem";
	name=objName + "__ctl"
	subtotal=0;
	appsumamt=0;
	
	if (document.all[objName] !=null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		
		if(rowCount >= 2)
		{
			for (i=2; i<= rowCount; i++)
			{
				oElement = document.getElementById(name + i + "_txtdaysReq");
				if(oElement != null)
				{
					days=oElement.value;
					if(days == "")
					{
						days = 1;
						oElement.value=1;
					}
					if(days == 0)
					{
						days = 1;
						oElement.value=1;
					}
				}
				else
				{
					days=1;
				}
				
				oElement = document.getElementById(name + i + "_txtQty");
				qty=oElement.value;
				if(qty == "")
				{
					qty = 1;
					oElement.value=1;
				}
				if(qty == 0)
				{
					qty = 1;
					oElement.value=1;
				}
				
				oElement = document.getElementById(name + i + "_lbldis");
				dis=oElement.innerHTML;
				
				oElement = document.getElementById(name + i + "_txtPrice");
				price=oElement.value;
				
				if(dis != 0)
				{
					vdis = eval(price) *(eval(dis)/100);
					price = eval(price) - eval(vdis);
				}
								
				oElement = document.getElementById(name + i + "_txtSqrftg");
				if(oElement != null)
				{
					sqrftg = oElement.value;
					if(sqrftg == "")
					{
						sqrftg=1;
						oElement.value=1;
					}
					if(sqrftg == 0)
					{
						sqrftg=1;
						oElement.value=1;
					}
				}
				else
				{
					sqrftg=1;
				}
								
				oElement = document.getElementById(name + i + "_txtTotal");
				oElement.value=days * qty * price * sqrftg;
				
				totalval = oElement.value;
							
				oElement = document.getElementById(name + i + "_lblapptax");
				apptax = oElement.innerHTML;
				
				if(apptax != 0)
				{
					apptaxamt = (totalval * apptax) / 100;
					appsumamt = eval(appsumamt) + eval(apptaxamt);
					
					oElement = document.getElementById("txtApplicationtax");
					oElement.value=Math.round(appsumamt*100)/100;
				}
				else
				{
					oElement = document.getElementById("txtApplicationtax");
					oElement.value=0
				}
				
				subtotal = eval(subtotal) + eval(totalval);
				
				oElement = document.getElementById("txtOrdersubtotal");
				oElement.value=Math.round(subtotal*100)/100;
				
				oElement = document.getElementById("txtTotOrder");
				ftotal = eval(subtotal)+eval(appsumamt);
				oElement.value=Math.round(ftotal*100)/100;
			}
		}
		else
		{
				oElement = document.getElementById("txtOrdersubtotal");
				oElement.value=0;
				
				oElement = document.getElementById("txtApplicationtax");
				oElement.value=0;
				
				oElement = document.getElementById("txtTotOrder");
				oElement.value=0;
		}
		
	}
	else
		alert("No Items Available in Shopping Cart");

	return false;

}

function CalculatesubtotalCustomer()
{
	if (CheckBlank(document.getElementById('txtBooth')))
	{
		alert("Enter Booth Number");
		//document.getElementById('lblErrorMessage').innerText="Enter Booth Number";
		document.getElementById('txtBooth').focus();
		return false;
	}		

	objName="dgCartItem";
	name=objName + "__ctl"
	subtotal=0;
	appsumamt=0;
	
	if (document.all[objName] !=null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		
		if(rowCount >= 2)
		{
			for (i=2; i<= rowCount; i++)
			{
				oElement = document.getElementById(name + i + "_txtdaysReq");
				if(oElement != null)
				{
					days=oElement.value;
					if(days == "")
					{
						days = 1;
						oElement.value=1;
					}
					if(days == 0)
					{
						days = 1;
						oElement.value=1;
					}
				}
				else
				{
					days=1;
				}
				
				oElement = document.getElementById(name + i + "_txtQty");
				qty=oElement.value;
				if(qty == "")
				{
					qty = 1;
					oElement.value=1;
				}
				if(qty == 0)
				{
					qty = 1;
					oElement.value=1;
				}
				
				oElement = document.getElementById(name + i + "_lbldis");
				dis=oElement.innerHTML;
				
				oElement = document.getElementById(name + i + "_txtPrice");
				price=oElement.value;
				
				if(dis != 0)
				{
					vdis = eval(price) *(eval(dis)/100);
					price = eval(price) - eval(vdis);
				}
								
				oElement = document.getElementById(name + i + "_txtSqrftg");
				if(oElement != null)
				{
					sqrftg = oElement.value;
					if(sqrftg == "")
					{
						sqrftg=1;
						oElement.value=1;
					}
					if(sqrftg == 0)
					{
						sqrftg=1;
						oElement.value=1;
					}
				}
				else
				{
					sqrftg=1;
				}
								
				oElement = document.getElementById(name + i + "_txtTotal");
				oElement.value=days * qty * price * sqrftg;
				
				totalval = oElement.value;
							
				oElement = document.getElementById(name + i + "_lblapptax");
				apptax = oElement.innerHTML;
								
				if(apptax != 0)
				{
					apptaxamt = (totalval * apptax) / 100;
					appsumamt = eval(appsumamt) + eval(apptaxamt);
					
					oElement = document.getElementById("txtApplicationtax");
					oElement.value=Math.round(appsumamt*100)/100;
				}
				else
				{
					oElement = document.getElementById("txtApplicationtax");
					oElement.value=0
				}
								
				subtotal = eval(subtotal) + eval(totalval);
				
				oElement = document.getElementById("txtOrdersubtotal");
				oElement.value=Math.round(subtotal*100)/100;
				subchecktotal = Math.round(subtotal*100)/100;
				
				oElement = document.getElementById("txtTotOrder");
				ftotal = eval(subtotal)+eval(appsumamt);
				oElement.value=Math.round(ftotal*100)/100;
			}
			return true;
		}
		else
		{
				oElement = document.getElementById("txtOrdersubtotal");
				oElement.value=0;
				
				oElement = document.getElementById("txtApplicationtax");
				oElement.value=0;
				
				oElement = document.getElementById("txtTotOrder");
				oElement.value=0;
				
				return false;
		}
		
		return true;
	}
	else
	{
		alert("No Items Available in Shopping Cart");
		return false;
	}
}

