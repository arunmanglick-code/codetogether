var arrAll_Date = new Array();
var arrChk_Date = new Array();

function callCalender(e)
{
	var cal1 = new calendar2(document.forms['frmGlobalSetting'].elements[e]);
	cal1.year_scroll = true;
	cal1.time_comp = false;
	cal1.popup();
}
	
function ValidateGlobalSettings()
{
		document.getElementById('lblErrorMessage').innerText="";
		//Validation for Grower to Retailer Transit Days
		if (Trim(document.frmGlobalSetting.txtGTRTD.value) == '')	
		{
			document.getElementById('lblErrorMessage').innerText="Grower to Retailer Transit Days cannot be Blank.";
			document.getElementById('lblErrorMessage').focus();
			return false;				
		}
		//Validation for Breakdown to Pickup Time Allowance (days)
		if (Trim(document.frmGlobalSetting.txtBTPTA.value) == '')	
		{
			document.getElementById('lblErrorMessage').innerText="Breakdown to Pickup Time Allowance (days) cannot be Blank.";
			document.getElementById('lblErrorMessage').focus();
			return false;				
		}
		//Validation for Racks broken down to trigger pickup
		if (Trim(document.frmGlobalSetting.txtBBDTTP.value) == '')	
		{
			document.getElementById('lblErrorMessage').innerText="Racks broken down to trigger pickup cannot be Blank.";
			document.getElementById('lblErrorMessage').focus();
			return false;				
		}
		//Validation for Racks At Store Minimum
		if (Trim(document.frmGlobalSetting.txtRASM.value) == '')	
		{
			document.getElementById('lblErrorMessage').innerText="Racks At Store Minimum cannot be Blank.";
			document.getElementById('lblErrorMessage').focus();
			return false;				
		}
		//Validation for Breakdown Service Time Allowance (days)
		if (Trim(document.frmGlobalSetting.txtBSTA.value) == '')	
		{
			document.getElementById('lblErrorMessage').innerText="Store Unloading Time Allowance (days) cannot be Blank.";
			document.getElementById('lblErrorMessage').focus();
			return false;				
		}
		/* Validate Full / Half Service Dates of Region 1 */
		if (CheckForRegion1() == false) return false;
		if (CheckForRegion2() == false) return false;
		if (CheckForRegion3() == false) return false;
		if (CheckForRegion4() == false) return false;
		return true;
}

function CheckForRegion1()
{
	arrAll_Date[0]= 'txtFull_FromDate_Range1_Reg1';
	arrAll_Date[1]= 'txtFull_FromDate_Range2_Reg1';
	arrAll_Date[2]= 'txtFull_FromDate_Range3_Reg1';
	arrAll_Date[3]= 'txtFull_FromDate_Range4_Reg1';
	
	arrAll_Date[4]= 'txtFull_ToDate_Range1_Reg1';
	arrAll_Date[5]= 'txtFull_ToDate_Range2_Reg1';
	arrAll_Date[6]= 'txtFull_ToDate_Range3_Reg1';
	arrAll_Date[7]= 'txtFull_ToDate_Range4_Reg1';
	
	arrAll_Date[8] =  'txtHalf_FromDate_Range1_Reg1';
	arrAll_Date[9] =  'txtHalf_FromDate_Range2_Reg1';
	arrAll_Date[10]= 'txtHalf_FromDate_Range3_Reg1';
	arrAll_Date[11]= 'txtHalf_FromDate_Range4_Reg1';
	
	arrAll_Date[12] =  'txtHalf_ToDate_Range1_Reg1';
	arrAll_Date[13] =  'txtHalf_ToDate_Range2_Reg1';
	arrAll_Date[14] = 'txtHalf_ToDate_Range3_Reg1';
	arrAll_Date[15] = 'txtHalf_ToDate_Range4_Reg1';
	if (checkcommon2() == false) return false;
	return true;
}

function CheckForRegion2()
{
	arrAll_Date[0]= 'txtFull_FromDate_Range1_Reg2';
	arrAll_Date[1]= 'txtFull_FromDate_Range2_Reg2';
	arrAll_Date[2]= 'txtFull_FromDate_Range3_Reg2';
	arrAll_Date[3]= 'txtFull_FromDate_Range4_Reg2';
	
	arrAll_Date[4]= 'txtFull_ToDate_Range1_Reg2';
	arrAll_Date[5]= 'txtFull_ToDate_Range2_Reg2';
	arrAll_Date[6]= 'txtFull_ToDate_Range3_Reg2';
	arrAll_Date[7]= 'txtFull_ToDate_Range4_Reg2';
	
	arrAll_Date[8] =  'txtHalf_FromDate_Range1_Reg2';
	arrAll_Date[9] =  'txtHalf_FromDate_Range2_Reg2';
	arrAll_Date[10]= 'txtHalf_FromDate_Range3_Reg2';
	arrAll_Date[11]= 'txtHalf_FromDate_Range4_Reg2';
	
	arrAll_Date[12] =  'txtHalf_ToDate_Range1_Reg2';
	arrAll_Date[13] =  'txtHalf_ToDate_Range2_Reg2';
	arrAll_Date[14] = 'txtHalf_ToDate_Range3_Reg2';
	arrAll_Date[15] = 'txtHalf_ToDate_Range4_Reg2';
	if (checkcommon2() == false) return false;
	return true;
}

function CheckForRegion3()
{
	arrAll_Date[0]= 'txtFull_FromDate_Range1_Reg3';
	arrAll_Date[1]= 'txtFull_FromDate_Range2_Reg3';
	arrAll_Date[2]= 'txtFull_FromDate_Range3_Reg3';
	arrAll_Date[3]= 'txtFull_FromDate_Range4_Reg3';
	arrAll_Date[4]= 'txtFull_ToDate_Range1_Reg3';
	arrAll_Date[5]= 'txtFull_ToDate_Range2_Reg3';
	arrAll_Date[6]= 'txtFull_ToDate_Range3_Reg3';
	arrAll_Date[7]= 'txtFull_ToDate_Range4_Reg3';
	arrAll_Date[8] =  'txtHalf_FromDate_Range1_Reg3';
	arrAll_Date[9] =  'txtHalf_FromDate_Range2_Reg3';
	arrAll_Date[10]= 'txtHalf_FromDate_Range3_Reg3';
	arrAll_Date[11]= 'txtHalf_FromDate_Range4_Reg3';
	arrAll_Date[12] =  'txtHalf_ToDate_Range1_Reg3';
	arrAll_Date[13] =  'txtHalf_ToDate_Range2_Reg3';
	arrAll_Date[14] = 'txtHalf_ToDate_Range3_Reg3';
	arrAll_Date[15] = 'txtHalf_ToDate_Range4_Reg3';
	if (checkcommon2() == false) return false;
	return true;
}


function CheckForRegion4()
{
	arrAll_Date[0]= 'txtFull_FromDate_Range1_Reg4';
	arrAll_Date[1]= 'txtFull_FromDate_Range2_Reg4';
	arrAll_Date[2]= 'txtFull_FromDate_Range3_Reg4';
	arrAll_Date[3]= 'txtFull_FromDate_Range4_Reg4';
	arrAll_Date[4]= 'txtFull_ToDate_Range1_Reg4';
	arrAll_Date[5]= 'txtFull_ToDate_Range2_Reg4';
	arrAll_Date[6]= 'txtFull_ToDate_Range3_Reg4';
	arrAll_Date[7]= 'txtFull_ToDate_Range4_Reg4';
	arrAll_Date[8] =  'txtHalf_FromDate_Range1_Reg4';
	arrAll_Date[9] =  'txtHalf_FromDate_Range2_Reg4';
	arrAll_Date[10]= 'txtHalf_FromDate_Range3_Reg4';
	arrAll_Date[11]= 'txtHalf_FromDate_Range4_Reg4';
	arrAll_Date[12] =  'txtHalf_ToDate_Range1_Reg4';
	arrAll_Date[13] =  'txtHalf_ToDate_Range2_Reg4';
	arrAll_Date[14] = 'txtHalf_ToDate_Range3_Reg4';
	arrAll_Date[15] = 'txtHalf_ToDate_Range4_Reg4';
	if (checkcommon2() == false) return false;
	return true;
}

// Common Function
function checkCommon1(d1,d2)
{
	document.getElementById(d1).style.color = 'black';
	document.getElementById(d2).style.color = 'black';
	if (isDate(Trim(document.getElementById(d1).value),"MM/dd/yyyy") == false)
	{
		document.getElementById("lblErrorMessage").innerText="Invalid Date Format.";
		document.getElementById('lblErrorMessage').focus();
		document.getElementById(d1).style.color = 'red';
		return false;				
	}	
	else if (isDate(Trim(document.getElementById(d2).value),"MM/dd/yyyy") == false)
	{
		document.getElementById('lblErrorMessage').innerText="Invalid Date Format.";
		document.getElementById('lblErrorMessage').focus();
		document.getElementById(d2).style.color = 'red';
		return false;
	}	
	else if (compareDates(Trim(document.getElementById(d1).value),"MM/dd/yyyy",Trim(document.getElementById(d2).value),"MM/dd/yyyy") == 1)
	{
		document.getElementById('lblErrorMessage').innerText="To Date should be Greater than From Date.";
		document.getElementById('lblErrorMessage').focus();
		document.getElementById(d1).style.color = 'red';
		document.getElementById(d2).style.color = 'red';
		return false;
	}
	return true;		
}		

function checkDate(d1)
{

	document.getElementById(d1).style.color = 'black';
	if (isDate(Trim(document.getElementById(d1).value),"MM/dd/yyyy") == false)
	{
		document.getElementById("lblErrorMessage").innerText="Invalid Date Format.";
		document.getElementById('lblErrorMessage').focus();
		document.getElementById(d1).style.color = 'red';
		return false;				
	}
	var vdatevalue 
	var vdate = new Array();
	vdatevalue  = Trim(document.getElementById(d1).value);
	vdate = vdatevalue .split("/");
	//alert(vdate[2]);
	if (compareDates(Trim("01/01/1753"),"MM/dd/yyyy",Trim(document.getElementById(d1).value),"MM/dd/yyyy") == 1 || compareDates(Trim(document.getElementById(d1).value),"MM/dd/yyyy",Trim("12/31/9999"),"MM/dd/yyyy") == 1)
	{
		document.getElementById('lblErrorMessage').innerText="Date must be between 01/01/1753 and 12/31/9999.";
		document.getElementById('lblErrorMessage').focus();
		return false;
	}
	return true;		
}

function checkcommon2()
{
	var ictl;
	ictl=0;

	for (var icount = 0; icount < 4; icount++) 
	{
		document.getElementById(arrAll_Date[icount]).style.backgroundColor= 'white';
		document.getElementById(arrAll_Date[icount + 4]).style.backgroundColor= 'white';
		
		if (Trim(document.getElementById(arrAll_Date[icount]).value) != '')
		{
			if (checkDate(arrAll_Date[icount]) == true)
			{			
				if (Trim(document.getElementById(arrAll_Date[icount + 4]).value) != '')
				{
					if(checkCommon1(arrAll_Date[icount],arrAll_Date[icount+4]) == true)
					{
						arrChk_Date[ictl] = Trim(document.getElementById(arrAll_Date[icount]).value)+ ','+ Trim(document.getElementById(arrAll_Date[icount+4]).value) + ',' + arrAll_Date[icount] + ',' + arrAll_Date[icount + 4];
						ictl = ictl + 1;
					}// end if
					else
					return false
				}// End if
				else
				{
					document.getElementById('lblErrorMessage').innerText="Please enter date.";
					document.getElementById('lblErrorMessage').focus();
					document.getElementById(arrAll_Date[icount + 4]).style.backgroundColor= '#b8bf95';
					return false;				
				}
			}// End if
			else
			return false
		}// End if	
		else
		{
			if (Trim(document.getElementById(arrAll_Date[icount + 4]).value) != '')
			{
				if (checkDate(arrAll_Date[icount + 4]) == true)
				{
					document.getElementById('lblErrorMessage').innerText="Please enter date.";
					document.getElementById('lblErrorMessage').focus();
					document.getElementById(arrAll_Date[icount]).style.backgroundColor= '#b8bf95';
					return false;				
				}
				else
				return false
			}
		}
	}// End For
		
	for (var icount = 8; icount < 12; icount++) 
	{
		if (Trim(document.getElementById(arrAll_Date[icount]).value) != '')
		{
			if (checkDate(arrAll_Date[icount]) == true)
			{			
				if (Trim(document.getElementById(arrAll_Date[icount + 4]).value) != '')
				{
					if(checkCommon1(arrAll_Date[icount],arrAll_Date[icount+4]) == true)
					{
						arrChk_Date[ictl] = Trim(document.getElementById(arrAll_Date[icount]).value)+ ','+ Trim(document.getElementById(arrAll_Date[icount+4]).value) + ',' + arrAll_Date[icount] + ',' + arrAll_Date[icount + 4];
						ictl = ictl + 1;
					}// end if
					else
					return false
				}// end if
				else
				{
					document.getElementById('lblErrorMessage').innerText="Please enter date.";
					document.getElementById('lblErrorMessage').focus();
					document.getElementById(arrAll_Date[icount + 4]).style.backgroundColor= '#b8bf95';
					return false;				
				}
			} // end if
			else
			return false
		}// end if
		else
		{
			if (Trim(document.getElementById(arrAll_Date[icount + 4]).value) != '')
			{
				if (checkDate(arrAll_Date[icount + 4]) == true)
				{
					document.getElementById('lblErrorMessage').innerText="Please enter date.";
					document.getElementById('lblErrorMessage').focus();
					document.getElementById(arrAll_Date[icount]).style.backgroundColor= '#b8bf95';
					return false;				
				}
				else
				return false
			}
		}
	}// end for
	
	
	for (var icount = 0; icount < ictl; icount++) 
	{
		
		//compareTwoDates
		var strsep_date;
		var date1;
		var date2;
		var date3;
		var date4;
		var datename1;
		var datename2;
		var datename3;
		var datename4;
		
		strsep_date = arrChk_Date[icount].split(',');
		date1 = strsep_date[0];
		date2 = strsep_date[1];
		datename1 = strsep_date[2];
		datename2 = strsep_date[3];
		
		for (var irow = icount+1 ; irow < ictl; irow++)
		{
			strsep_date = arrChk_Date[irow].split(',');
			date3 = strsep_date[0];
			date4 = strsep_date[1];
			datename3 = strsep_date[2];
			datename4 = strsep_date[3];
			
			//alert('date1 '+ date1 + 'date2 '+ date2 + 'date3 ' + date3 + 'date4 ' + date4);
			if (compareTwoDates(date1,"MM/dd/yyyy",date2,"MM/dd/yyyy",date3,"MM/dd/yyyy",date4,"MM/dd/yyyy") == 1)
			{
				//message
				document.getElementById(datename1).style.color = 'red';
				document.getElementById(datename2).style.color = 'red';
				document.getElementById(datename3).style.color = 'red';
				document.getElementById(datename4).style.color = 'red';
				document.getElementById('lblErrorMessage').innerText="Dates should not be overlap.";
				document.getElementById('lblErrorMessage').focus();
				
				return false;
			}
			else
			{
				document.getElementById(datename1).style.color = 'black';
				document.getElementById(datename2).style.color = 'black';
				document.getElementById(datename3).style.color = 'black';
				document.getElementById(datename4).style.color = 'black';
				document.getElementById('lblErrorMessage').innerText='';
				document.getElementById('lblErrorMessage').focus();
			}
		}
	}
	return true;
}