		
function ValidateInvAdjust()
{
			
		document.getElementById('lblErrorMessage').innerText='';
		var name="dgInvAdj__ctl";
		
		var blnEmpty = true
		if (document.all["dgInvAdj"] !=null)
		{
			var rowItems = eval(dgInvAdj).rows;
			var rowCount = rowItems.length;
			
			for (i=2; i< rowCount-1; i++)
			{
			
				oElement = document.getElementById(name + i + "_txtAdjFinishGoods");
				if (oElement != null)
				{
					if (oElement.value != '' && oElement.value != '0')
					{
						blnEmpty=false;
						break;
					}
				}
				
				oElement = document.getElementById(name + i + "_txtAdjScrap");
				if (oElement != null)
				{
					if (oElement.value != '' && oElement.value != '0')
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
			document.getElementById('lblErrorMessage').innerText="Please enter value.";
			document.getElementById('lblErrorMessage').focus();
			return false;
		}
		
}//End Function
		
		