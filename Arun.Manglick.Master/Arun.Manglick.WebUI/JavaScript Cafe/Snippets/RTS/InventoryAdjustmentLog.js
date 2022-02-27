function ValidateInvMain()
{
			var blnVal = false;
			if (document.getElementById('ddlRepackager').value == "-1" && document.getElementById('ddlGrower').value == "-1" && document.getElementById('ddlRetailer').value == "-1")
			{
				document.getElementById('lblErrorMessage').innerText="Please, select value from any drop down.";
				document.getElementById('lblErrorMessage').focus();
				return false;
			}
			if (document.getElementById('txtFromDate').value == "")
			{
				document.getElementById('lblErrorMessage').innerText="Please, select both the Date ranges";
				document.getElementById('lblErrorMessage').focus();
				return false;
			}
			if	(document.getElementById('txtToDate').value == "")
			{
				document.getElementById('lblErrorMessage').innerText="Please, select both the Date ranges";
				document.getElementById('lblErrorMessage').focus();
				return false;
			}
			
			/*if (document.getElementById('ddlRepackager').value =="0")
			{
				if (document.getElementById('ddlGrower').value !="0")
				{
					blnVal = true;
				}
				if (document.getElementById('ddlRetailer').value !="0")
				{
					if (blnVal == true) 
					{
						document.getElementById('lblErrorMessage').innerText="Please, select only one value";
						document.getElementById('lblErrorMessage').focus();
						return false;
					}
					else 
					{return true;}
					
				}
				else
				{
					if (blnVal == true) 
					{return true;}
					else 
					{
						document.getElementById('lblErrorMessage').innerText="Please, select value from any drop down.";
						document.getElementById('lblErrorMessage').focus();
						return false;
					}
					
				}
			}
			else 
			{
				if (document.getElementById('ddlGrower').value !="0") 
				{
						document.getElementById('lblErrorMessage').innerText="Please, select only one value";
						document.getElementById('lblErrorMessage').focus();
						return false;
				}
				if (document.getElementById('ddlRetailer').value !="0") 
				{
						document.getElementById('lblErrorMessage').innerText="Please, select only one value";
						document.getElementById('lblErrorMessage').focus();
						return false;
				}
				else 
				{return true;}
			}*/
			
}
		
				
		