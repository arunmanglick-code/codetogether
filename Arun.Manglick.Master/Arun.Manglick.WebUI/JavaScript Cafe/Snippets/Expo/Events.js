
function EventLoad()
{
	if(document.getElementById("ddlFacility").disabled == false )
		document.getElementById("ddlFacility").focus();
	else
		document.getElementById("txtShowNumber").focus();

	controlState(0,0);

	return true;
}

function Validate()
{
	if (CheckBlank(document.getElementById('txtShowNumber')))
	{
		alert("Show Number Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Show Number Cannot be Blank.";
		document.getElementById('txtShowNumber').focus();
		return false;
	}
	else if (CheckBlank(document.getElementById('txtName')))
	{
		alert("Name Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Name Cannot be Blank.";
		document.getElementById('txtName').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtEmail'))) 
	{
		alert("Email Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank.";
		document.getElementById('txtEmail').focus();
		return false;
	}
	else if (!CheckEmail(document.getElementById('txtEmail')))
	{
		alert("Please Enter valid Email address");
		//document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
		document.getElementById('txtEmail').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtStartDate'))) 
	{
		alert("Start Date Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="Start Date Cannot be Blank.";
		return false;
	}
	else if (CheckBlank(document.getElementById('txtEndDate')))
	{
		alert("End Date Cannot be Blank.");
		//document.getElementById('lblErrorMessage').innerText="End Date Cannot be Blank.";
		return false;
	}
	else if (compareDates(Trim(document.getElementById('txtStartDate').value),"MM/dd/yyyy",Trim(document.getElementById('txtEndDate').value),"MM/dd/yyyy") == 1)
	{
		alert("End Date should not be less than Start Date.");
		//document.getElementById('lblErrorMessage').innerText="End Date should not be less than Start Date.";
		return false;
	}
	else if (!chkCategory())
	{
		return false;
	}
	return true;
}

//Check for Selected Category
function chkCategory()
{
	var bonesel,name;
	name="dgCategory__ctl"
	if (document.all['dgCategory'] !=null)
	{
		var rowItems = eval('dgCategory').rows;
		var rowCount = rowItems.length;
		bonesel = false
		for (i=2; i<= rowCount; i++)
		{
			oElement = document.getElementById(name + i + "_chkCategory");
			if(oElement.checked == true)
			{
				bonesel=true;
				break;
			}
		}
		if (bonesel == false)
		{
			alert("Please Select at least one Category to attache with Event.");
			return false;
		}
	}
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
		oElement = document.getElementById(name + i + "_chkCategory");
		changeBG(oElement,'Del');
	}
}
