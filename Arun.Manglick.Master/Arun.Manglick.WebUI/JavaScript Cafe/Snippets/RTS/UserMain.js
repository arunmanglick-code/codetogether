		//Function to Validate Data on Load Click
		function Validate()
		{
		
			if (frmUserMain.ddlUserId.selectedIndex==0)
			{
				document.getElementById('lblErrorMessage').innerText="Please Select Existing User for Editing";
				document.getElementById('lblErrorMessage').focus();
				return false;
			}
			return true;
		}
		
		function NotBlankSearch()
		{
			if (CheckBlank(frmUserMain.txtCriteria)== true)
			{
				document.getElementById('lblErrorMessage').innerText="Search criteria can not be blank.";
				document.getElementById('lblErrorMessage').focus();
				//alert('here');
				return false;
			}
			return true;
		}
		
		// Validate User data
		function ValidateUserField()
		{
			if (CheckBlank(frmUser.txtUserId))
			{
				document.getElementById('lblErrorMessage').innerText="User Id can not be blank.";
				document.getElementById('lblErrorMessage').focus();
				//alert('User Id');
				return false;
			}
			else if (CheckBlank(frmUser.txtUserName))
			
			{	
				document.getElementById('lblErrorMessage').innerText="Name can not be blank.";
				document.getElementById('lblErrorMessage').focus();
				//alert('Name Id');
				return false;
			}
			else if (CheckBlank(frmUser.txtPassword))
			{
				document.getElementById('lblErrorMessage').innerText="Password can not be blank.";
				document.getElementById('lblErrorMessage').focus();
				//alert('Pass Id');
				return false;
			}
			else if	(CheckBlank(frmUser.txtEmail)!= true)
			{
			
				if (CheckMultiEmail(frmUser.txtEmail) == false)
				{
					document.getElementById('lblErrorMessage').innerText="Please enter valid Email Address.";
					document.getElementById('lblErrorMessage').focus();
					return false;
				}
			}
			
			return true;
		}
		
		
				
		
function dispconfirm()
{
	var name=confirm("Are you sure you want to delete this User Id?")
	if (name==true)
	{
		return true;
	}
	else
	{
		return false;
	}
}

function CheckDelete()
{
	return false
}
