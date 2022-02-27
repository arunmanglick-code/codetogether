
function ViewCart()
{
	FilePath = "/exposervicedesk/Transaction/Online/ViewCart.aspx?ordid=" + event.srcElement.nameProp;
	window.open (FilePath,"","left=0,top=0,width=740,height=500,status=no,toolbar=no,menubar=no,scrollbars=1");
	return false;
}

function CheckSelection()
{
	if (document.getElementById("ddlFacility").value == "")
	{
		alert("Select a Facility");
		//document.getElementById('lblErrorMessage').innerText="Select a Facility";
		document.getElementById('ddlFacility').focus();
		return false;
	}
	if (document.getElementById("ddlEvent").value == "")
	{
		alert("Select an Event");
		//document.getElementById('lblErrorMessage').innerText="Select an Event";
		document.getElementById('ddlEvent').focus();
		return false;
	}
	return true;
}
