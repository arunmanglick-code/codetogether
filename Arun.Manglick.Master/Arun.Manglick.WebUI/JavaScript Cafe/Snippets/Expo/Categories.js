
function CategoryLoad()
{
	SetCursor('ddlAction');
	MM_swapImage('Image2','','/exposervicedesk/Images/tabs/category-r.gif',1);
	//FacitliyGridStatus();
}

function Validate()
{	
	document.getElementById('lblErrorMessage').innerTexts='';
	if (CheckBlank(document.getElementById('txtABRV')))
	{
		alert("Abbreviation Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Abbreviation Cannot be Blank";
		document.getElementById('txtABRV').focus();
		return false;
	}
	else if(CheckBlank(document.getElementById('txtNAME'))) 
	{
		alert("Name Cannot be Blank");
		//document.getElementById('lblErrorMessage').innerText="Name Cannot be Blank";
		document.getElementById('txtNAME').focus();
		return false;
	}
	else if (!CheckMaxLength(document.getElementById('txtDESC'),300))
	{
		alert("Description Cannot be more than 300 characters");
		//document.getElementById('lblErrorMessage').innerText="Description Cannot be more than 300 characters";
		document.getElementById('txtDESC').focus();
		return false;
	}
	return true;
	/*
	else if (!chkFacility())
	{
		alert('aa');
		return false;
	}
	*/
}

function DeleteRecord()
{
	var sResult
	sResult = confirm("Are you sure want to delete a selected node?");
	if (sResult == true)
		return true;
	else
		return false;
}

function FacitliyGridStatus()
{
	var obj;
	obj= document.getElementById('divGrid');

	if (document.getElementById('chkFacility').checked==true)
		obj.runtimeStyle.display="block";
	else
		obj.runtimeStyle.display="none";
	return true;
}

function chkFacility()
{
	var bonesel,name;

	if (document.getElementById('chkFacility').checked==true)
	{
		name="dgFacility__ctl"
		if (document.all['dgFacility'] !=null)
		{
			var rowItems = eval('dgFacility').rows;
			var rowCount = rowItems.length;
			bonesel = false
			for (i=2; i<= rowCount; i++)
			{
				oElement = document.getElementById(name + i + "_chkSelect");
				if(oElement.checked == true)
				{
					bonesel=true;
					break;
				}
			}
			if (bonesel == false)
			{
				alert("Please Select at least one facility to attache with category.");
				return false;
			}
		}
	}
	return true;
}	

