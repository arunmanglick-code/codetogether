
/************************************************************
function : CheckLength(Obj,ln)
usage    : for checking Total length
inputs   : object and length
output   : false if total length exceeds ln
e.g      : CheckLength(obj,ln)
************************************************************/
function CheckLength(Obj,ln)
{
	var strText=Obj.value;
	if (strText.length > ln)
	{
		return false;
	}
	else if(strText.length < ln)
	{
		return false;
	}
	return true;
} 


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

/************************************************************
function : CheckBlank(p)
usage    : To check NULL/EMPTY value in TextBox
inputs   : string
output   : returns true if string contains some value; otherwise returns false
e.g      : p = 'abc' is valid, p = '' is invalid.

************************************************************/
function CheckBlank(p)
{
	if(Trim(p.value)=="")
	{
		p.select();
		return true;
	}
	return false;
}

/************************************************************
function : DateCompare(p,s)
usage    : To check date first date with second 
inputs   : string
output   : returns true if Date is not Future; otherwise returns false
e.g      : p > s is invalid, p < s valid.

************************************************************/
function DateCompare(p,s)
{

 var Ty,Tm,Td,Ttmp  
	Ty= s.substring(s.lastIndexOf("/")+1);
	Ttmp = ("0" + s.substring(s.indexOf("/")+1,s.lastIndexOf("/")));
	if (Ttmp.length==2)
	{
		Tm = Ttmp.substring(0);
	}
	else
	{
		Tm = Ttmp.substring(1);
	}
	Ttmp= ("0"+ s.substring(0,s.indexOf("/")));
	if (Ttmp.length==2)
	{
		Td = Ttmp.substring(0);
	}
	else
	{
		Td = Ttmp.substring(1);
	}
	var today = (Ty + Tm + Td);
		
	var y,m,d,tmp  
	y= p.substring(p.lastIndexOf("/")+1);
	tmp = ("0" + p.substring(p.indexOf("/")+1,p.lastIndexOf("/")));
	if (tmp.length==2)
	{
		m = tmp.substring(0);
	}
	else
	{
		m = tmp.substring(1);
	}
	tmp= ("0"+ p.substring(0,p.indexOf("/")));
	if (tmp.length==2)
	{
		d = tmp.substring(0);
	}
	else
	{
		d = tmp.substring(1);
	}
	var testdate = (y + m + d);

      if (testdate > today)
         return false;
      else
         return true;
}

/************************************************************
function : ValidAge(p,s)
usage    : To check if age is more than 18 years
inputs   : string
output   : returns true if age is more than 12 years; otherwise returns false
e.g      : p < 18 is invalid, p > 18 valid.

************************************************************/
function ValidAge(p,s)
{

 var Ty,Tm,Td,Ttmp  
	Ty= s.substring(s.lastIndexOf("/")+1);
	Tm = s.substring(s.indexOf("/")+1,s.lastIndexOf("/"));
	Td= s.substring(0,s.indexOf("/"));
var today = new Date(Ty, Tm-1 , Td);
var y,m,d,tmp  
	y= p.substring(p.lastIndexOf("/")+1);
	m = p.substring(p.indexOf("/")+1,p.lastIndexOf("/"));
	d = p.substring(0,p.indexOf("/"));
var testdate = new Date(y , m-1 , d);
var age
age = (((today-testdate)+1)/(1000 * 60 * 60 * 24 * 365)); 
//window.alert((age));
      if (age < 18 )
      {
        return false;
      }
      else
      {
         return true;
      }
}

/************************************************************
function : CompareTime(p,s)
usage    : To check First Time with Second Time
inputs   : string
output   : returns true if First Time is Less than Second Time; otherwise returns false
e.g      : p > s is invalid, p < s valid.

************************************************************/
function CompareTime(fh,fm,sh,sm)
{
 var Tmpfh=new String(fh);
 var Tmpfm= new String(fm);
 var Tmpsh=new String(sh);
 var Tmpsm= new String(sm);
 var TmpFirst, TmpSecond;
 var x, y
 x=("0"+fh);
 x= x.substr(x.length-2,2);
 y=("0"+fm);
 y= y.substr(y.length-2,2);
 TmpFirst=(x+y);
 x=("0"+sh);
 x= x.substr(x.length-2,2);
 y=("0"+sm);
 y= y.substr(y.length-2,2);
 TmpSecond=(x+y);
 if ((TmpFirst-TmpSecond) >-1)
 {
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
	//	alert(strMessage)
		strMail.select()
		strMail.focus()
		return false
	}
	if (strMail.value.charAt(intLen-1)=="@" || strMail.value.charAt(intLen-1)==".")
	{
	//	alert(strMessage)
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
	//	alert(strMessage)
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
		//alert(strMessage)
		strMail.select()
		strMail.focus()
		return false
	}
	return true
}

/************************************************************
function : CheckUrl(strUrl)
usage    : To check Validity Of URL
inputs   : string containing URL address
output   : returns true if URL is valid; otherwise returns false
e.g      : www.aaa.com is valid Email Address. it checks for only 
			www. if exist than true else false (it is set according to requirement
************************************************************/
function CheckUrl(strUrl)
{
 var Tmp = new String(strUrl.value);
	Tmp= Tmp.toUpperCase();
	Tmp = Trim(Tmp);
	if (Tmp=="")
	{
		return true;
	}
//	window.alert(Tmp.indexOf("WWW."));
	if ((Tmp.indexOf("WWW.")== -1))
	{
		return false;
	}
	else if ((Tmp.indexOf("WWW.")!=0) || (Tmp.length < 5))
	{
		if ((Tmp.indexOf("HTTP://WWW.")!=0) ||(Tmp.length < 12))
		{
			return false;
		}
	}
	return true
}

/************************************************
DESCRIPTION: Validates that a string contains valid
US phone pattern.
	Ex. (999) 999-9999 or (999)999-9999
PARAMETERS:
	strValue - String to be tested for validity
RETURNS:
	True if valid, otherwise false.
*************************************************/
function validateUSPhone( strValue ) 
{
	var objRegExp  = /^\([1-9]\d{2}\)\s?\d{3}\-\d{4}$/;
	return objRegExp.test(strValue);
}

function validateUSZip( strValue ) {
/************************************************
DESCRIPTION: Validates that a string a United
  States zip code in 5 digit format or zip+4
  format. 99999 or 99999-9999

PARAMETERS:
   strValue - String to be tested for validity

RETURNS:
   True if valid, otherwise false.

*************************************************/
var objRegExp  = /(^\d{5}$)|(^\d{5}-\d{4}$)/;

  //check for valid US Zipcode
   return objRegExp.test(strValue);
}


function validateEmail( strValue) 
{
/************************************************
DESCRIPTION: Validates that a string contains a
  valid email pattern.

 PARAMETERS:
   strValue - String to be tested for validity

RETURNS:
   True if valid, otherwise false.
*************************************************/

var objRegExp  = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
//var objRegExp  = /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
  //check for valid email
  return objRegExp.test(strValue);
}

/************************************************
DESCRIPTION: only allows numeric values

 PARAMETERS:
   
RETURNS:
   returns only numeric value and discards other values
*************************************************/
function Numerichandler()
{
	if (!(event.keyCode >= '48' && event.keyCode <= '57'))
	{
	event.keyCode=0;
	}
}

function NumerichandlerForNegative()
{
	if (event.keyCode == '45') return event.keyCode 
	if (!(event.keyCode >= '48' && event.keyCode <= '57'))
	{
	event.keyCode=0;
	}
}

/************************************************
DESCRIPTION: Converts into upper case

 PARAMETERS:
   textbox value
RETURNS:
   return upper case value
*************************************************/
function CodeHandler(p)
{
	var x = new String(p.value);
	x= x.toUpperCase();
	p.value=x;
	
}

/************************************************************
function : CheckMultiEmail(strMail)
usage    : To check Validity Of Email
inputs   : string containing mail address
output   : returns true if email is valid; otherwise returns false
e.g      : abc@xyz.com is valid Email Address.

************************************************************/
function CheckMultiEmail(p)
{
	var stremail = Trim(p.value);
	var iPos1 = stremail.indexOf(",");
	var iPos2 = stremail.indexOf(";");
	
	if (iPos1 > 0)
	{
		var emails = stremail.split( /, ?/ );
	}
	else if (iPos2 > 0)
	{
		var emails = stremail.split( /; ?/ );
	}
	else
	{
			var emails = stremail.split( /, ?/ );
	}
	for ( var i = 0; ( email = emails[i] ); i++ )
	{
		if (validateEmail(email)== false)
		{
			return false
		}
	}
	return true
}

		var iRatecount;
		iRatecount = 0;

function onlyRate(p)
{		
			var x = p;
			if (x.indexOf('.') == -1)
			{
				iRatecount=0;
			}
			if (iRatecount == 0)
			{
				if (event.keyCode == 46) 
				{
					iRatecount += 1;
					return true
				}
			}
			if (!(event.keyCode >= '48' && event.keyCode <= '57'))
			{
			event.keyCode=0;
			}
}