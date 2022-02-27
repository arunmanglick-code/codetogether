function ValidateSelect()
{	
	if (frmBDPUSummaryMain.ddlLoc.selectedIndex < 0)
	{
		document.getElementById('lblErrorMessage').innerText="Please Select Existing PoolPoint/Repackager for Editing";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;
}
function ShowWP()//ShowBDPUD
{
	var iBPID
		oElement = document.getElementById(event.srcElement.id);
		iBPID = oElement.getAttribute('BPID');
 		window.open('/EZRack/Transactions/ShowWorkOrder.aspx?BPid=' + iBPID + "&Action=WP"  ,'_blank','width=800,height= 800,status=no,resizable=yes,dependent=yes,alwaysRaised=yes');
 		//window.focus(); 
 		return false;
}
function ShowWO()//ShowBDDOC
	{
		var iBPID
		oElement = document.getElementById(event.srcElement.id);
		iBPID = oElement.getAttribute('BPID');
 		window.open('/EZRack/Transactions/ShowWorkOrder.aspx?BPid=' + iBPID + "&Action=WO",'_blank','width=800,height= 800,status=no,resizable=yes,dependent=yes,alwaysRaised=yes');
 		//window.focus(); 
 		return false;
	}
	
	
