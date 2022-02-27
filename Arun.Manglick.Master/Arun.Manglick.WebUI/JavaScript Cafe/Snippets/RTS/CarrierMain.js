		//Function to Validate Data on Load Click
		function Validate()
		{
			if (frmCarrierMain.ddlExisting.selectedIndex==0)
			{
				document.getElementById('lblErrorMessage').innerText="Please Select Existing Carrier for Editing";
				document.getElementById('lblErrorMessage').focus();
				return false;
			}
			return true;
		}
		function ValidateSearch()
		{
			if (CheckBlank(frmCarrierMain.txtCriteria)== true)
			{
				document.getElementById('lblErrorMessage').innerText="Search criteria can not be blank.";
				document.getElementById('txtCriteria').focus();
				return false;
			}
			return true;
		}
		

