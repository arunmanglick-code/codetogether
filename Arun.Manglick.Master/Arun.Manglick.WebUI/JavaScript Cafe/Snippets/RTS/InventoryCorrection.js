function ValidateInvMain()
{
			var blnVal = false;
			if (document.getElementById('ddlRepackager').value =="0")
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
			}
			
}
		
		


	function ValidateInvCorr()
{
			
			document.getElementById('lblErrorMessage').innerText='';
			var name="dgInvCorr__ctl";
			
			var blnEmpty = true
			if (document.all["dgInvCorr"] !=null)
			{
				var rowItems = eval(dgInvCorr).rows;
				var rowCount = rowItems.length;
				
				for (i=2; i< rowCount-1; i++)
				{
				
					oElement = document.getElementById(name + i + "_txtOvrWIP");
					if (oElement != null)
					{
						if (oElement.value!= '')
						{
							
							blnEmpty=false;
							break;
						}
					}
					
					oElement = document.getElementById(name + i + "_txtOvrFinishGoods");
					if (oElement != null)
					{
						if (oElement.value!= '')
						{
							blnEmpty=false;
							break;
						}
					}
					
					oElement = document.getElementById(name + i + "_txtOvrScrap");
					if (oElement != null)
					{
						if (oElement.value != '')
						{
							blnEmpty=false;
							break;
						}
					}
						
					oElement = document.getElementById(name + i + "_txtOvrDisp");
					if (oElement != null)
					{
						if (oElement.value !='')
						{
							blnEmpty=false;
							break;
						}
					}
				}//End For
			}//End If
			if (blnEmpty == false)
			{
				return true;
			}
			else
			{
				document.getElementById('lblErrorMessage').innerText="Please, enter the value.";
				document.getElementById('lblErrorMessage').focus();
				return false;
			}
}//End Function
		
		