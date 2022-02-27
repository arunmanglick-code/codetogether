/********************************************************
function : chkSelection
usage	 : check facility and event is selected before employee user go ahead
**********************************************************/
function chkSelection()
{
	if (document.getElementById("ddlFacility").value == "")
	{
		alert("Please select a Facility");
		//document.getElementById('lblErrorMessage').innerText="Please select a Facility";
		document.getElementById('ddlFacility').focus();
		return false;
	}
	else if (document.getElementById("ddlEvent").value == "")
	{
		alert("Please select an Event");
		//document.getElementById('lblErrorMessage').innerText="Please select an Event";
		document.getElementById('ddlEvent').focus();
		return false;
	}
	return true;
}

/********************************************************
function : SetPage
usage	 : setting employeehome.aspx
**********************************************************/
function SetPage()
{
	MM_swapImage('Image1','','/exposervicedesk/Images/tabs/customerinfo-r.gif',1);
	
	obj1= document.getElementById('divAllCompany');
	obj2= document.getElementById('divFilterCompany');

	if (document.getElementById('chkFilter').checked == true)
	{
		obj1.runtimeStyle.display="none";
		obj2.runtimeStyle.display="block";
	}
	else
	{
		obj2.runtimeStyle.display="none";
		obj1.runtimeStyle.display="block";
	}
}

/********************************************************
function : ShowFilter
usage	 : 
**********************************************************/
function ShowFilter()
{
	if (document.getElementById('chkFilter').checked == true)
		__doPostBack('imgSearchAll','');
	else
		__doPostBack('imgSearchFilter','');
}
