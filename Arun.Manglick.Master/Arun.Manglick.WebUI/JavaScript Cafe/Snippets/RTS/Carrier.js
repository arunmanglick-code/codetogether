
		function ValidateCarrier()
		{
			//Validation for Code
			    if (CheckBlank(document.frmCarrier.txtCode))	
			    {
					document.getElementById('lblErrorMessage').innerText="Code cannot be Blank.";
					document.getElementById('lblErrorMessage').focus();
					return false;				
				}
			//Validation for Name
				
			    if (CheckBlank(document.frmCarrier.txtName))	
			    {
					document.getElementById('lblErrorMessage').innerText="Name cannot be Blank.";
					document.getElementById('lblErrorMessage').focus();
					return false;				
				}
			//Validation for Zip Format
			    if (Trim(document.frmCarrier.txtZip.value) != '')	
			    {
					if (validateUSZip(Trim(document.frmCarrier.txtZip.value)) == false)
					{
						document.getElementById('lblErrorMessage').innerText="Please enter valid Zip code.";
						document.getElementById('lblErrorMessage').focus();
						return false;				
					}
				}
			//Validataion for Email Foramt
			    if (Trim(document.frmCarrier.txtEmail.value) != '')	
			    {
					if (CheckMultiEmail(document.frmCarrier.txtEmail) == false)
					{
						document.getElementById('lblErrorMessage').innerText="Please enter valid Email Address.";
						document.getElementById('lblErrorMessage').focus();
						return false;				
					}
				}
				return true;
		}

		
		
		