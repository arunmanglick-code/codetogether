
/************************************************************
function : LTrim(strText)
usage    : for removing blank spaces from left of a string
inputs   : string.
output   : string without spaces on its left
e.g      : LTrim("  abc  ") = "abc  "
************************************************************/
function LTrim(strText)
{
	while (strText.substring(0,1) == ' ')
			strText = strText.substring(1, strText.length);
	return strText;
} 


/************************************************************
function : RTrim(strText)
usage    : for removing blank spaces from right of a string
inputs   : string.
output   : string without spaces on its right
e.g      : RTrim("  abc  ") = "  abc"
************************************************************/
function RTrim(strText)
{
	while (strText.substring(strText.length-1,strText.length) == ' ')
			strText = strText.substring(0, strText.length-1);
	return strText;
}
	

/************************************************************
function : Trim(strText)
usage    : for removing blank spaces from right and left of a string
inputs   : string.
output   : string without spaces on its right and left
e.g      : Trim("  abc  ") = "abc"
**************************************************************/
function Trim(strText)
{
	return RTrim(LTrim(strText));
}



/************************************************************
function : IsNumeric(strNum)
usage    : to determine whether given string is integer
inputs   : string.
output   : true if it is integer, false otherwise.
e.g      : "123" returns true, "abc44" returns false, "12.34" returns false, "-12" returns true
************************************************************/
function IsNumeric(strNum)
{
	
	if(strNum.indexOf(".")!=-1)
	{
		return false;
	}
	if(strNum.indexOf("e")!=-1)
	{
		return false;
	}
	if(strNum.indexOf("E")!=-1)
	{
		return false;
	}
	if(isNaN(strNum))
	{
		return false;
	}
	return true;
} 

/************************************************************
function : IsDigit(strNum)
usage    : to determine whether given string is integer
inputs   : string.
output   : true if it is integer, false otherwise.
e.g      : "123" returns true, "abc44" returns false, "12.34" returns false, "-12" returns false
************************************************************/
function IsDigit(strNum)
{
	
	if(strNum.indexOf(".")!=-1)
	{
		return false;
	}
	if(strNum.indexOf("-")!=-1)
	{
		return false;
	}
	if(strNum.indexOf("e")!=-1)
	{
		return false;
	}
	if(strNum.indexOf("E")!=-1)
	{
		return false;
	}
	if(isNaN(strNum))
	{
		return false;
	}
	return true;
} 


/************************************************************
function : IsDecimal(strNum)
usage    : to determine whether given string is decimal
inputs   : string.
output   : true if it is decimal, false otherwise.
e.g      : "123" returns true, "abc" returns false, "12.34" returns true,"-12.34" returns true
************************************************************/
function IsDecimal(strNum)
{
	if(strNum.indexOf("e")!=-1)
	{
		return false;
	}
	if(strNum.indexOf("E")!=-1)
	{
		return false;
	}
	if(isNaN(strNum))
	{
		return false;
	}
	return true;
} 




/**************************************************************
function : CheckPhone(p1,p2,p3)
usage    : To check phone number of the form ###-###-####
inputs   : three textbox fields used as input to phone number, str is message to be displayed
output   : returns true if phone number is valid; otherwise returns false
e.g      : 123-345-6789 is valid phone number
**************************************************************/
function CheckPhone(p1,p2,p3,str)
{
	if (p1.value=="" & p2.value=="" & p3.value=="")
	{
		return true;
	}
	if(p1.value.length!=3)
	{
		alert(str);
		p1.select();
		p1.focus();		
		return false;
	}	
	if(!IsDigit(p1.value))
	{
		alert(str);
		p1.select();
		p1.focus();
		return false;
	}
	if(p2.value.length!=3)
	{
		alert(str);
		p2.select();
		p2.focus();		
		return false;
	}	
	if(!IsDigit(p2.value))
	{
		alert(str);
		p2.select();
		p2.focus();
		return false;
	}
	if(p3.value.length!=4)
	{
		alert(str);
		p3.select();
		p3.focus();		
		return false;
	}
	if(!IsDigit(p3.value))
	{
		alert(str);
		p3.select();
		p3.focus();
		return false;
	}
	
	return true;
}


/************************************************************
function : CheckZip(p1,p2,str)
usage    : To check Zip number of the form #####-####
inputs   : two textbox fields used as input to zip number, str is message to be displayed
output   : returns true if zip number is valid; otherwise returns false
e.g      : 12334-6789 is valid Zip Number
************************************************************/

function CheckZip(p1,p2,str)
{
	if (p1.value=="" & p2.value=="")
	{
		return true;
	}
	if(p1.value.length!=5)
	{
		alert(str);
		p1.select();
		p1.focus();		
		return false;
	}
	if(!IsDigit(p1.value))
	{
		alert(str);
		p1.select();
		p1.focus();
		return false;
	}
	if(p2.value.length!=4)
	{
		alert(str);
		p2.select();
		p2.focus();		
		return false;
	}	
	if(!IsDigit(p2.value))
	{
		alert(str);
		p2.select();
		p2.focus();
		return false;
	}
	
	return true;
}


/************************************************************
function : CheckEmail(strMail)
usage    : To check Validity Of Email
inputs   : string containing mail address
output   : returns true if email is valid; otherwise returns false
e.g      : abc@xyz.com is valid Email Address.

************************************************************/

function CheckEmail(strMail)
{
	var strMessage;
	strMessage="Invalid E-Mail Address"
	if (strMail.value=="")
	{
		return true;
	}
	var intLen=strMail.value.length
	var blnFlag=0
	if (strMail.value.charAt(0)=="@" || strMail.value.charAt(0)==".")
	{
		alert(strMessage)
		strMail.select()
		strMail.focus()
		return false
	}
	if (strMail.value.charAt(intLen-1)=="@" || strMail.value.charAt(intLen-1)==".")
	{
		alert(strMessage)
		strMail.select()
		strMail.focus()
		return false
	}
	for (var i=0;i<intLen;i++)
	{
		if (strMail.value.charAt(i)=="@")
		{
			blnFlag=blnFlag+1
		}
	}
	if (blnFlag>=0 && blnFlag<1 || blnFlag>1)
	{
		alert(strMessage)
		strMail.select()
		strMail.focus()
		return false
	}
	strSplit=(strMail.value).split("@")
	intSptLen=strSplit[1].length
	var intCnt=0
	for(var j=0;j<intSptLen;j++)
	{
		if (strSplit[1].charAt(j)==".")
		{
			intCnt=intCnt+1
		}
	}
	if (intCnt<=0)
	{
		alert(strMessage)
		strMail.select()
		strMail.focus()
		return false
	}
	return true
}


/**************************************************************
function : CheckTaxID(p1,strMessage)
usage    : To check TaxID of the form ##-#######
inputs   : string containing taxid
output   : returns true if taxid is valid; otherwise returns false
e.g      : 12-3456789 is valid tax id
**************************************************************/
function CheckTaxID(p1,strMessage)
{
	var strMessage;
	if (p1.value=="")
	{
		return true;
	}
	if(p1.value.length!=10)
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
	if(!IsDigit(p1.value.substring(0,2)))
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
	if(p1.value.substring(2,3)!="-")
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
	if(!IsDigit(p1.value.substring(3)))
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
return true;
}

/**************************************************************
function : CheckSSN(p1,strMessage)
usage    : To check SSN number of the form ###-##-####
inputs   : string containing ssn number
output   : returns true if ssn number is valid; otherwise returns false
e.g      : 123-34-6789 is valid phone number
**************************************************************/
function CheckSSN(p1,strMessage)
{
	var strMessage;
	if (p1.value=="")
	{
		return true;
	}
	if(p1.value.length!=11)
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
	if(!IsDigit(p1.value.substring(0,3)))
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
	if(p1.value.substring(3,4)!="-")
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
	if(!IsDigit(p1.value.substring(4,6)))
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
	if(p1.value.substring(6,7)!="-")
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
	if(!IsDigit(p1.value.substring(7)))
	{
		alert(strMessage);
		p1.select();
		p1.focus();		
		return false;
	}
return true;
}

///this function is used to check if value in textbox is empty
function CheckBlank(p,strMessage)
{
	if(Trim(p.value)=="")
	{
		alert(strMessage);
		p.select();
		p.focus();
		return false;
	}
	return true;
}


/**************************************************************
function : isDate(strDate)
usage    : To check valid date of the form dd/mm/yyyy or dd-mm-yyyy
inputs   : string containing Date
output   : returns true if Date is valid; otherwise returns false
e.g      : 31/12/2005 or 31-12-2005 is valid Date
**************************************************************/
function isDate(dateStr) 
	{
			//debugger;
			var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
			var matchArray = dateStr.match(datePat); // is the format ok?

			if (matchArray == null) {
			alert("Please enter date in 'dd/mm/yyyy' format.");
			return false;
			}

			month = matchArray[3]; // p@rse date into variables
			day = matchArray[1];
			year = matchArray[5];

			if (month < 1 || month > 12) { // check month range
			alert("Month must be between 1 and 12.");
			return false;
			}

			if (day < 1 || day > 31) {
			alert("Day must be between 1 and 31.");
			return false;
			}

			if ((month==4 || month==6 || month==9 || month==11) && day==31) {
			alert("Month "+month+" doesn`t have 31 days!")
			return false;
			}

			if (month == 2) { // check for february 29th
			var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
			if (day > 29 || (day==29 && !isleap)) {
			alert("February " + year + " doesn`t have " + day + " days!");
			return false;
			}
			}
			return true; // date is valid
	}
	
	
/**************************************************************
function : isOnlyAlpha(str)
usage    : To check valid String containg only alphabets, space, tab, Enter but not numbers
inputs   : string containing Date
output   : returns true if valid; otherwise returns false
e.g      : arun is valid
**************************************************************/
function isOnlyAlpha(txtobject) 
	{
			//debugger;
			
			var obj=document.getElementById(txtobject);
			var str=obj.value;
			
			//var pat="/^[a-z]{1,2}$/";
			//var pat="/^[A-Za-z]\w{1,}[A-Za-z]$/";
			//var pat="E[a-z]{2}$";
			//var pat="/^\w{1,5}$/";
			
			var pat="[a-z]{1,50}";   // Best Works
			//var pat="\d{2}";   // Best Works
			//var pat="[a-z]{1,50}(\w*)[a-z]{1,50}";   // Works
			//var pat="\w*\d*";   // Works
			
			
			
			//var pat="/^\w*$/";
			
			var matching=str.match(pat);
			
			if (matching == null)
				{
				alert("Please enter String only");
				obj.focus();
				return false;
				}
			else
				return true;	
	}
			
	