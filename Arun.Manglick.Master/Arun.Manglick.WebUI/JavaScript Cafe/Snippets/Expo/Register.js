/**********************************************************************************************
function : Navigate(obj)
usage    : for displaying div 1 to 3 for user registration wizard
inputs   : +1 or -1, navigate by one page in both the direction 
			Changes the header caption "Registering User ( Step 1 of 3)"
			"Enter Login Info/Enter Account Info" etc
			Changes the caption of button [btn2] also
**********************************************************************************************/
function Navigate(p)
{
	var obj,obj1,obj2,obj3;
	var objHeader1,objHeader2;
	var objBtn1,objBtn2;

	obj= document.getElementById('hpage');		//hidden textbox for page number
	if(p==1) 
	{
		if (!Validate(obj.value))
			return false;
	}

			
	objHeader1= document.getElementById('header1');		
	objHeader2= document.getElementById('header2');		
	objBtn1=document.getElementById('btn1');		
	objBtn2=document.getElementById('btn2');		
			
	obj.value=obj.value*1 + p ;
	if (obj.value==0) obj.value=1;
	if (obj.value==4) 
		Register.submit();
	else
	{
		obj1= document.getElementById('page1');
		obj2= document.getElementById('page2');
		obj3= document.getElementById('page3');
		
		if(obj.value==1)
		{
			objBtn1.runtimeStyle.visibility="hidden";
			obj1.runtimeStyle.display="block";
			obj2.runtimeStyle.display="none";
			obj3.runtimeStyle.display="none";
			objHeader2.innerText='Enter Login Information';
			objHeader1.innerText="Registering User ( Step 1 of 3)";
			//add user information						
			document.getElementById('UsrInfo').innerText="Please enter a username, password, retype password, secret question and its answer to start your secure registration process. To proceed to the next step in the secure registration process click the \"Next\" button at the bottom of the User Information form.";			
			document.getElementById('txtUserName').focus();
		}
		if (obj.value==2)
		{
			objBtn1.runtimeStyle.visibility="inherit";
			obj1.runtimeStyle.display="none";
			obj2.runtimeStyle.display="block";
			obj3.runtimeStyle.display="none";
			objHeader2.innerText="Enter Account Information";
			objHeader1.innerText="Registering User ( Step 2 of 3)";
			//add user information
			document.getElementById('UsrInfo').innerText="Please enter company name, first name, last name, address 1, address 2, city, state, zip, country, phone, fax, and email. To proceed to the next step in the secure registration process click the \"Next\" button at the bottom of the User Information form.";
			objBtn2.value="Next";
			
			var o;
			o=document.getElementById('username');
			o.innerText=document.getElementById('txtUserName').value;
			document.getElementById('txtCompany').focus();
		}		
		if (obj.value==3)
		{
			objBtn1.runtimeStyle.visibility="inherit";
			obj1.runtimeStyle.display="none";
			obj2.runtimeStyle.display="none";
			obj3.runtimeStyle.display="block";
			objHeader2.innerText="Enter Credit Card Information";
			objHeader1.innerText="Registering User ( Step 3 of 3)";
			//add user information
			//document.getElementById('UsrInfo').innerText="";
			document.getElementById('UsrInfo').innerText="Please your billing information so that we can verify your account so that we may complete the secure registration process. To process your account click the \"Submit\" button at the bottom of the User Information form.";
			document.getElementById('ddlCCType').focus();
			objBtn2.value="Submit";
		}		
	}
}

/**********************************************************************************************
function : Validate(p)
usage    : for validating data in pages of user registration wizard
inputs   : page no, in which data entry needs to be validated
**********************************************************************************************/
function Validate(p)
{
	document.getElementById('lblErrorMessage').innerText="";
	if(p==1)	//validate login info
	{
		if (CheckBlank(document.getElementById('txtUserName')))
		{
			alert("Username Cannot be Blank");
			document.getElementById('txtUserName').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtPassword')))
		{
			alert("Password Cannot be Blank");
			document.getElementById('txtPassword').focus();
			return false;
		}
		else if (document.getElementById('txtPassword').value != document.getElementById('txtCPassword').value)
		{
			alert("Password Does not Match");
			document.getElementById('txtCPassword').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtAnswer')))
		{
			alert("Answer for Secret Question Cannot be Blank");
			document.getElementById('txtAnswer').focus();
			return false;
		}
		return true;		
	}
	else if(p==2)		//validate Account Info
	{
		if (CheckBlank(document.getElementById('txtCompany')))
		{
			alert("Company Cannot be Blank");
			document.getElementById('txtCompany').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtFName')))
		{
			alert("Firstname Cannot be Blank");
			document.getElementById('txtFName').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtLName')))
		{
			alert("Lastname Cannot be Blank");
			document.getElementById('txtLName').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtAdd1')))
		{
			alert("Address1 Cannot be Blank");
			document.getElementById('txtAdd1').focus();
			return false;
		}
		else if (!CheckMaxLength(document.getElementById('txtAdd1'),300))
		{
			alert("Address1 Cannot be more than 300 characters");			
			document.getElementById('txtAdd1').focus();
			return false;
		}
		else if (!CheckMaxLength(document.getElementById('txtAdd2'),300))
		{
			alert("Address2 Cannot be more than 300 characters");
			document.getElementById('txtAdd2').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtCity')))
		{
			alert("City Cannot be Blank");
			document.getElementById('txtCity').focus();
			return false;
		}
		else if (document.getElementById("ddlState").value == -1 && CheckBlank(document.getElementById('txtOther')))
		{
			alert("State Cannot be Blank");
			document.getElementById('txtOther').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtZip')))
		{
			alert("Zip Cannot be Blank");
			document.getElementById('txtZip').focus();
			return false;
		}
		else if (!CheckLength(document.getElementById('txtZip'),5))
		{
			alert("Please Enter valid Zip");
			document.getElementById('txtZip').focus();
			return false;
		}
		else if (!IsNumeric(document.getElementById('txtZip').value))
		{
			alert("Zip should be Numeric");
			document.getElementById('txtZip').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtPhone')))
		{
			alert("Phone Cannot be Blank");
			document.getElementById('txtPhone').focus();
			return false;
		}
		else if (CheckBlank(document.getElementById('txtEmail')))
		{
			alert("Email Cannot be Blank");
			document.getElementById('txtEmail').focus();
			return false;
		}
		else if (!CheckEmail(document.getElementById('txtEmail')))
		{
			alert("Please Enter valid Email address");
			document.getElementById('txtEmail').focus();
			return false;
		}		
		return true;
	}
	if(p==3)
	{
		if (document.getElementById("ddlCCType").value == -1)
		{
			alert("Select Credit Card");
			document.getElementById('ddlCCType').focus();
			return false;
		}
		else if(document.getElementById("txtCCNumber").value.length<15)
		{
			document.getElementById("txtCCNumber").focus();
			alert("Invalid Credit Card Number");
			return false;
		}
		else if (CheckBlank(document.getElementById('txtCCNumber')))
		{
			alert("Credit Card Number Cannot be Blank");
			document.getElementById('txtCCNumber').focus();
			return false;
		}
		else if (!IsNumeric(document.getElementById('txtCCNumber').value))
		{
			alert("Credit Card Number should be Numeric");
			document.getElementById('txtCCNumber').focus();
			return false;
		}
		else if(document.getElementById("txtCVVNumber").value.length<3)
		{
			document.getElementById("txtCVVNumber").focus();
			alert("Invalid CVV Number");
			return false;
		}
		else if (CheckBlank(document.getElementById('txtCVVNumber')))
		{
			alert("CVV Number Cannot be Blank");
			document.getElementById('txtCVVNumber').focus();
			return false;
		}
		else if (!IsNumeric(document.getElementById('txtCVVNumber').value))
		{
			alert("CVV Number should be Numeric");
			document.getElementById('txtCVVNumber').focus();
			return false;
		}
		return true;
	}
}

function SetPageCount()
{
	obj= document.getElementById('hpage');		//hidden textbox for page number
	obj.value=1; 
}

function ShowCVV()
{
	window.open("cvv.htm","cvv","height=400,width=500,status=yes,toolbar=no,menubar=no,scrollbars=1");
}

function ValidateState()
{
	var oElement
	oElement = document.getElementById("ddlState");
	if (oElement.value == -1)
	{
		oElement = document.getElementById("txtOther");
		oElement.disabled = false;
		return false
	}
	else
	{
		oElement = document.getElementById("txtOther");
		oElement.disabled = true;
		oElement.value='';
		return false;
	}
}
