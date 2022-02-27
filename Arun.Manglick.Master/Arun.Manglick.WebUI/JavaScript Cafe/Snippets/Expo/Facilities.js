function FacilityLoad()
{
	ValidateInActive();
}

//Function will used of validating all form field.
function Validate()
{
		if (CheckBlank(document.getElementById('txtFacilityname')))
		{
			alert("Facility Name Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Facility Name Cannot be Blank";
			document.getElementById('txtFacilityname').focus();
			return false;
		}
		else if(CheckBlank(document.getElementById('txtContactfirstname'))) 
		{
			alert("Contact First Name Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Contact First Name Cannot be Blank";
			document.getElementById('txtContactfirstname').focus();
			return false;
		}
		
		else if(CheckBlank(document.getElementById('txtContactlastname'))) 
		{
			alert("Contact Last Name Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Contact Last Name Cannot be Blank";
			document.getElementById('txtContactlastname').focus();
			return false;
		}			
		else if(CheckBlank(document.getElementById('txtAddress1'))) 
		{
			alert("Address1 Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Address1 Cannot be Blank";
			document.getElementById('txtAddress1').focus();
			return false;
		}
		else if (!CheckMaxLength(document.getElementById('txtAddress1'),300))
		{
			alert("Address1 Cannot be more than 300 characters");
			//document.getElementById('lblErrorMessage').innerText="Address1 Cannot be more than 300 characters";
			document.getElementById('txtAddress1').focus();
			return false;
		}
		
		else if (!CheckMaxLength(document.getElementById('txtAddress2'),300))
		{
			alert("Address2 Cannot be more than 300 characters");
			//document.getElementById('lblErrorMessage').innerText="Address2 Cannot be more than 300 characters";
			document.getElementById('txtAddress2').focus();
			return false;
		}
		else if(CheckBlank(document.getElementById('txtCity'))) 
		{
			alert("City Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="City Cannot be Blank";
			document.getElementById('txtCity').focus();
			return false;
		}
		
		else if (CheckBlank(document.getElementById('txtPostalCode')))
		{
			alert("Zip Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Zip Cannot be Blank";
			document.getElementById('txtPostalCode').focus();
			return false;
		}
		else if (!IsNumeric(document.getElementById('txtPostalCode').value))
		{
			alert("Zip should be Numeric");
			//document.getElementById('lblErrorMessage').innerText="Zip should be Numeric";
			document.getElementById('txtPostalCode').focus();
			return false;
		}
		else if (!CheckLength(document.getElementById('txtPostalCode'),5))
		{
			alert("Please Enter valid Zip");
			//document.getElementById('lblErrorMessage').innerText="Please Enter valid Zip";
			document.getElementById('txtPostalCode').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtPhone')))
		{
			alert("Phone Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Phone Cannot be Blank";
			document.getElementById('txtPhone').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtEmail')))
		{
			alert("Email Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Email Cannot be Blank";
			document.getElementById('txtEmail').focus();
			return false;
		}
		else if (!CheckEmail(document.getElementById('txtEmail')))
		{
			alert("Please Enter valid Email address");
			//document.getElementById('lblErrorMessage').innerText="Please Enter valid Email address";
			document.getElementById('txtEmail').focus();
			return false;
		}
		
		else if(!CheckBlank(document.getElementById('txtUrl')))
		{
			var TmpUrl = new String(document.getElementById('txtUrl').value);
			TmpUrl= TmpUrl.toUpperCase();
			TmpUrl = Trim(TmpUrl);
			if ((TmpUrl.indexOf("HTTP")==0))
			{
				alert("Don't Start URL With HTTP");
				//document.getElementById('lblErrorMessage').innerText="Don't Start URL With HTTP";
				document.getElementById('txtUrl').focus();
				return false;
			}
			if(!CheckUrl(document.getElementById('txtUrl')))
			{
				alert("Please Enter valid URL");
				//document.getElementById('lblErrorMessage').innerText="Please Enter valid URL";
				document.getElementById('txtUrl').focus();
				return false;
			}
		}
		return true;
}

//Function will open a popup window and shows the Big header logo image.
function winopen()
{
	var ImageName;
	var FilePath;
	ImageName = "/exposervicedesk/images/Logo/" + document.getElementById('lbtnHeaderImage').innerText;
	FilePath = "/exposervicedesk/HeaderImage.aspx?img=" + ImageName;
	window.open (FilePath,"mywindow","left=0,top=0,width=200,height=200,status=yes,toolbar=no,menubar=no,scrollbars=1");
	return false;
}


/*
function check_filename()
{
	with(document.Facilities)
	{
	splitted=new Array()
	file_with_path=flHeaderImage.value;

	splitted=file_with_path.split("\\");
	splitted.reverse();
	alert("file name="+splitted[0])

	var objRegExp  =  /(^[a-z_0-9 .A-Z]+$)/; 
	if(objRegExp.test(splitted[0]))
	alert("valid file name");
	return false;
	else
	alert("invalid file name");
	return false;
	}
	return true;
}
*/