
function OnlyRate()
{
	var ratevalue='';
	ratevalue = document.ShowAudit.elements(event.srcElement.name).value;
	if (onlyRate(ratevalue) == true)
	{
		return true;
	}
	else
	{
		return false;
	}
}
function chkValue(p)
{
	if(document.getElementById(p).value == '')
		return false;
	else
		return true;
}
function Validfield()
{
	if(document.getElementById('ddlFacility').value == "")
	{
		alert("Facility Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Facility Cannot be Blank";
		document.getElementById('ddlFacility').focus();
		return false;
	}
	
	if(document.getElementById('ddlEvent').value == "")
	{
		alert("Event Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Event Cannot be Blank";
		document.getElementById('ddlEvent').focus();
		return false;
	}
	
	objName="dgorder";
	name=objName + "__ctl"
	
	if(document.all[objName] !=null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		if(rowCount<=1)
		{
			alert("No Order Found For Selected Event");
			return false;
		}
	}
}