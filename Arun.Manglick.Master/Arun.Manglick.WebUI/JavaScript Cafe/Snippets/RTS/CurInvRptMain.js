function Validate(e)
{
	var sStatus;
	sStatus='';
	if (!CheckSelected())
	{
		document.getElementById('lblErrorMessage').innerText="There should be atleast one Report type Selected";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
		for (var i=0; i<=4 ; i++ )
		{
		
			objDdl = document.getElementById('chklItems_' + i);
			if(objDdl.checked == true)
			{
				if (objDdl.parentElement.innerText == "Repackagers")
				{
					sStatus += "1" + ","
				}
				else if (objDdl.parentElement.innerText == "Pool Points")
				{
					sStatus += "2" + ","
				}
				else if (objDdl.parentElement.innerText == "Growers")
				{
					sStatus += "3" + ","
				}
				else if (objDdl.parentElement.innerText == "In-Transit")
				{
					sStatus += "4" + ","
				}
				else if (objDdl.parentElement.innerText == "Retailers")
				{
					sStatus += "5" + ","
				}
						//sStatus += objDdl.parentElement.innerText.charAt(0);
			}
		}
		
		window.open('ExcelReportForm.aspx?RName=CurrentInvDetail'+ "&SType="+ e +"&Type=CURRENT" + "&Status=" + sStatus ,'_blank','width=800,height= 800,status=no,resizable=yes,menubar = yes,dependent=yes,alwaysRaised=yes');
		return false;
}
function ValidateGo(e)
{
	var sStatus;
	var sLocType;
	sStatus='';
	sLocType='';
	
	if (!CheckSelected())
	{
		document.getElementById('lblErrorMessage').innerText="There should be atleast one Report type Selected";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	if (document.getElementById('txtDate').value == "")
	{
		document.getElementById('lblErrorMessage').innerText="Date cannot be blank";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	else if (document.getElementById('txtDate').value == document.getElementById('txtCurDate').value )
	{
		document.getElementById('lblErrorMessage').innerText="To view a snapshot of yesterday's inventory, select yesterday's date";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	for (var i=0; i<=4 ; i++ )
		{
		
			objDdl = document.getElementById('chklItems_' + i);
			if(objDdl.checked == true)
			{
				if (objDdl.parentElement.innerText == "Repackagers")
				{
					sLocType += "1" + ","
				}
				else if (objDdl.parentElement.innerText == "Pool Points")
				{
					sLocType += "2" + ","
				}
				else if (objDdl.parentElement.innerText == "Growers")
				{
					sLocType += "3" + ","
				}
				else if (objDdl.parentElement.innerText == "In-Transit")
				{
					sLocType += "4" + ","
				}
				else if (objDdl.parentElement.innerText == "Retailers")
				{
					sLocType += "5" + ","
				}
						//sStatus += objDdl.parentElement.innerText.charAt(0);
			}
		}
	
		sStatus = document.getElementById('txtDate').value;
		window.open('ExcelReportForm.aspx?RName=CurrentInvDetail'+ "&SType="+ e +"&Type=PAST" + "&Status=" + sStatus + "&LocType=" + sLocType ,'_blank','width=800,height= 800,status=no,resizable=yes,menubar = yes,dependent=yes,alwaysRaised=yes');
		return false;
}

function CheckSelected()
{
	var name = "chklItems_";
	var x = 0;
	var i = 0;
	do{
		oElement = document.getElementById(name + i);
		if (oElement == null)
		{
			x = 1;
		}
		else
		{
			if (oElement.checked == true) 
			{
				return true;
			}
		}
		i++;
	}while(x<1);
	return false;
}