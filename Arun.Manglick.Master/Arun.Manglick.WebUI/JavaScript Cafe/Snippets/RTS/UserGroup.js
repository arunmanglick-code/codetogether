		//Function to Validate Data on Load Click
		function Validate()
		{
			
			if (frmUserGroupsMain.ddlGroupId.selectedIndex==0)
			{
				
				document.getElementById('lblErrorMessage').innerText="Please Select Existing User for Editing";
				document.getElementById('lblErrorMessage').focus();
				return false;
			}
			return true;
		}
		
		function NotBlankSearch()
		{
			if (CheckBlank(frmUserGroupsMain.txtCriteria)== true)
			{
				document.getElementById('lblErrorMessage').innerText="Search criteria can not be blank.";
				document.getElementById('lblErrorMessage').focus();
				//alert('here');
				return false;
			}
			return true;
		}
		
		// Validate User data
		function ValidateGroupField()
		{
			if (CheckBlank(frmUserGroups.txtUserGRId))
			{
				document.getElementById('lblErrorMessage').innerText="Group Id can not be blank.";
				document.getElementById('lblErrorMessage').focus();
				//alert('User Id');
				return false;
			}
			else if (CheckBlank(frmUserGroups.txtDescription))
			
			{	
				document.getElementById('lblErrorMessage').innerText="Description can not be blank.";
				document.getElementById('lblErrorMessage').focus();
				//alert('Name Id');
				return false;
			}
						
			return true;
		}
		
		
