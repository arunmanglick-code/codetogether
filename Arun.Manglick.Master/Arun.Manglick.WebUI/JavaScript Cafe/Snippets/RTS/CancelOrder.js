function ValidateCancel()
		{
			
			document.getElementById('lblErrorMessage').innerText='';
			var name="dgCancelOrder__ctl";
			var x=0;
			var curStatus;
			var SelStatus;
			var bonesel = false
			if (document.all["dgCancelOrder"] !=null)
			{
				var rowItems = eval(dgCancelOrder).rows;
				var rowCount = rowItems.length;
				
				for (i=2; i< rowCount-1; i++)
				{
				
					
					oElement = document.getElementById(name + i + "_chkCan");
					if (oElement.checked == true)
					{
						bonesel=true;
						break;		
					}	
				}//End For
			}//End If
			if (bonesel == false)
			{
				document.getElementById('lblErrorMessage').innerText="Please, select at least one record to cancel the order.";
				return false;
			}
			else
			{
				
				if (confirm("Are you sure to cancel orders?")==true)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			//return true;
		}//End Function
		
function NotBlankSearch()
		{
			if (CheckBlank(frmCancelOrderMain.txtCriteria) == true)
			{
				document.getElementById('lblErrorMessage').innerText="Search criteria can not be blank.";
				document.getElementById('lblErrorMessage').focus();
				
				return false;
			}
			return true;
		}