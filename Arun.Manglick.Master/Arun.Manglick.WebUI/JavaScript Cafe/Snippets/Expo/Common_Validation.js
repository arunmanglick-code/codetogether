
if (window.history.forward != null )
	window.history.forward(1);


/* Locks table header */
function lockRow(GridID,DivID)
{
	var table = document.getElementById(GridID);
	if (table == null) return;
	var cTR = table.getElementsByTagName('tr');  //collection of rows
	if (cTR == null) return;
	if (cTR.length > 0)
	{
		for (i = 0; i < table.rows[0].cells.length; i++)
			table.rows[0].cells[i].runtimeStyle.cssText = 'text-align: center;position:relative;top: expression(document.getElementById("' + DivID + '").scrollTop-2);z-index: 10;'
	}
}

/**********************************************************************************************
function : MM_swapImgRestore,MM_findObj(n, d),MM_swapImage()
usage    : for displaying tabs with hover effect
author	 : Ankit 
**********************************************************************************************/
function MM_swapImgRestore() 
{
	var i,x,a=document.MM_sr; 
	for(i=0; a && i<a.length && (x=a[i]) && x.oSrc; i++)
		x.src=x.oSrc;
}

function MM_preloadImages() 
{
	var d=document; 
	if(d.images)
	{ 
		if(!d.MM_p) 
			d.MM_p=new Array();
		
		var i,j=d.MM_p.length,a=MM_preloadImages.arguments; 
		for(i=0; i<a.length; i++)
			if (a[i].indexOf("#")!=0)
			{ 
				d.MM_p[j]=new Image; 
				d.MM_p[j++].src=a[i];
			}
	}
}

function MM_findObj(n, d) 
{
	var p,i,x;  
	if(!d)
		d=document; 
	
	if((p=n.indexOf("?"))>0 && parent.frames.length) 
	{
		d=parent.frames[n.substring(p+1)].document; 
		n=n.substring(0,p);
	}
	if(!(x=d[n]) && d.all) 
		x=d.all[n]; 
		
	for (i=0; !x && i<d.forms.length; i++) 
		x=d.forms[i][n];

	for(i=0; !x && d.layers && i<d.layers.length; i++) 
		x=MM_findObj(n,d.layers[i].document);

	if(!x && d.getElementById) 
		x=d.getElementById(n); 
	
	return x;
}

function MM_swapImage()
{
	var i,j=0,x,a=MM_swapImage.arguments; 
	document.MM_sr=new Array; 
	
	for(i=0; i<(a.length-2); i+=3)
		if ((x=MM_findObj(a[i]))!=null)
		{
			document.MM_sr[j++]=x; 
			if(!x.oSrc) 
				x.oSrc=x.src; 
			x.src=a[i+2];
		}
}


/**********************************************************************************************
function : SelectImage
usage    : 
author	 : Saurin
**********************************************************************************************/
function SelectImage(p,n)
{
	if(p==0)			//admin
		imgFile='admin-b';
	else if(p==1)		//client
		imgFile='client-b';
	else if (p==2)		//employee
		imgFile='emp-b';
	else if(p==3)		//user
		imgFile='user-b';
	else if(p==4)		//vendor
		imgFile='vendor-b';

	imgFile += n + '-r.gif';
	img='Image_' + (p*10 + n);
	
	MM_swapImage(img,'','/exposervicedesk/Images/tabs/' +  imgFile,1);
}



/**********************************************************************************************
function : VendorListLoad
usage    : for changing tabs behaviour for different user
author	 : Saurin
**********************************************************************************************/
function VendorListLoad(usrtype)
{
	if(usrtype == 0)	// admin user 
		MM_swapImage('Image3','','/exposervicedesk/Images/tabs/vendor-r.gif',1);
	else				// employee user
		MM_swapImage('Image33','','/exposervicedesk/Images/tabs/vendor-r.gif',1);
	
	ValidateInActive();
}

/**********************************************************************************************
function : CompanyListLoad
usage    : for changing tabs behaviour for different user
author	 : Saurin
**********************************************************************************************/
function CompanyListLoad(usrtype)
{
	if(usrtype == 0)	// admin user 
		MM_swapImage('Image7','','/exposervicedesk/Images/tabs/company-r.gif',1);
	else				// employee user
		MM_swapImage('Image31','','/exposervicedesk/Images/tabs/company-r.gif',1);
	
	ValidateInActive();
}
	
/**********************************************************************************************
function : OrderListLoad
usage    : for changing tabs behaviour for different user
author	 : Saurin
**********************************************************************************************/
function OrderListLoad(usrtype)
{
	if(usrtype == 2)	// employee user 
		MM_swapImage('Image32','','/exposervicedesk/Images/tabs/order-r.gif',1);
	else				// online user
		MM_swapImage('','','/exposervicedesk/Images/tabs/order-r.gif',1);
	
	ValidateInActive();
}

/**********************************************************************************************
function : PaymentListLoad
usage    : for changing tabs behaviour for different user
author	 : Saurin
**********************************************************************************************/
function PaymentListLoad(usrtype)
{
	if(usrtype == 2)	// employee user 
		MM_swapImage('Image33','','/exposervicedesk/Images/tabs/payment-r.gif',1);
	else				// online user
		MM_swapImage('','','/exposervicedesk/Images/tabs/payment-r.gif',1);
	
	ValidateInActive();
}

/**********************************************************************************************
function : RMSLoad
author	 : Saurin
**********************************************************************************************/
function RMSLoad()
{
	MM_swapImage('Image34','','/exposervicedesk/Images/tabs/rms-r.gif',1);
}


/**********************************************************************************************
function : BoothLoad
author	 : Saurin
**********************************************************************************************/
function BoothLoad()
{
	MM_swapImage('Image36','','/exposervicedesk/Images/tabs/booth-r.gif',1);
}


/**********************************************************************************************
function : AuditLoad()
author	 : Saurin
**********************************************************************************************/
function AuditLoad()
{
	MM_swapImage('Image9','','/exposervicedesk/Images/tabs/audit-r.gif',1);
}


/**********************************************************************************************
function : CustomerHome
usage    : for changing tabs behaviour for different user
author	 : Saurin
**********************************************************************************************/
function CustomerHome()
{
	MM_swapImage('Image41','','/exposervicedesk/Images/tabs/ordersummary-r.gif',1);
	ValidateInActive();
}


/**********************************************************************************************
function : changeBG
usage    : for changing background colors of rows
author	 : Saurin
**********************************************************************************************/
function changeBG(objRow, mouseState)
{   
	if (mouseState == 'Del')
	{
		objRow.parentElement.parentElement.runtimeStyle.backgroundColor = 
			objRow.checked ? '#ffcc00' : objRow.parentElement.parentElement.bgColor;
	}
	else if(objRow.runtimeStyle.backgroundColor != '#ffcc00')
	{
		if (mouseState == 'on')       
			objRow.runtimeStyle.backgroundColor ='#B5DAFE';
		else if (mouseState == 'off-alteritem')  
			objRow.runtimeStyle.backgroundColor='WhiteSmoke';
		else if (mouseState == 'off-item')
			objRow.runtimeStyle.backgroundColor='White';
	}
	return true;
}     


/**********************************************************************************************
function : verifyKeyPressEvent(obj)
usage    : for checking keypress event of any textbox and on keypress call some button events.
inputs   : object
output   : false if keycode is not a Enter key.
e.g      : verifyKeyPressEvent(obj)
**********************************************************************************************/
function verifyKeyPressEvent(obj)
{
	if(window.event.keyCode==13)
	{
		__doPostBack(obj,'')
		return false;
	}
	return true;
}

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
	if (Trim(strText).length > ln)
	{
		return false;
	}
	else if(Trim(strText).length < ln)
	{
		return false;
	}
	return true;
} 


/************************************************************
function : LTrim(strText)
usage    : for removing blank spaces from left of a string/removing cr and lf
inputs   : string.
output   : string without spaces on its left
e.g      : LTrim("  abc  ") = "abc  "
************************************************************/
function LTrim(strText)
{
	while (strText.substring(0,1) == ' ' || strText.charCodeAt(0)==13 || strText.charCodeAt(0)==10)
		strText = strText.substring(1, strText.length);

	return strText;
} 


/************************************************************
function : RTrim(strText)
usage    : for removing blank spaces from right of a string/removing cr and lf
inputs   : string.
output   : string without spaces on its right
e.g      : RTrim("  abc  ") = "  abc"
************************************************************/
function RTrim(strText)
{
	while (strText.substring(strText.length-1,strText.length) == ' ' || strText.charCodeAt(strText.length-1)==13 || strText.charCodeAt(strText.length-1)==10)
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

/*function CheckBlank1(p,fname)
{
	//alert(fname);
	var msg=""
	if(Trim(p.value)=="")
	{
		p.select()
		msg=fname + 'Cannot be Blank';
		return msg;
//		return true;
	}
	else
	{
		msg="";
		return msg;
	}
	return false;
}*/


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
function : checktime(p)
usage    : To check valid time in HH:MM format
inputs   : obj
output   : returns true if time is valid entry
************************************************************/
function checktime(thetime) 
{
	var a,b,c,f,err=0;
	
	a=thetime.value;
	if (a.length != 5) 
		err=1;
	
	b = a.substring(0, 2);
	c = a.substring(2, 3); 
	f = a.substring(3, 5); 
	
	if (/\D/g.test(b)) err=1; //not a number
	if (/\D/g.test(f)) err=1; 
	if (b<0 || b>23) err=1;
	if (f<0 || f>59) err=1;
	if (c != ':') err=1;

	if (err==1) 
		return false

	return true
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
			if(strMail.value.charAt(i+1)=="@")
			{
				strMail.select()
				strMail.focus()
				return false;
			}
			else
			{
				blnFlag=blnFlag+1
			}
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
			if(strSplit[1].charAt(j+1)==".")
			{
				strMail.select()
				strMail.focus()
				return false
			}
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
		if ((Tmp.indexOf("WWW.")!=0) ||(Tmp.length < 12))
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

/************************************************************
function : SetCursor
usage    : for Setting a Default cursor on particular control.
output   : true/false
************************************************************/
function SetCursor(objname)
{
	var oElement
	oElement = document.getElementById(objname);
	if(oElement)
	{
		oElement.focus();
		return true;
	}
	else
	{
		return false;
	}
}

/************************************************************
function : ValidateInActive
usage    : for enabling/disabling search textbox, disable if search by inactive 
output   : true/false 
************************************************************/
function ValidateInActive()
{
	var oElement
	oElement = document.getElementById("ddlSearchBy");
	if (oElement.value == 'InActive')
	{
		oElement = document.getElementById("txtSearchValue");
		oElement.disabled = true;
		oElement.value='';
		SetCursor('ddlSearchBy');
		return false
	}
	else
	{
		oElement = document.getElementById("txtSearchValue");
		oElement.disabled = false;
		SetCursor('txtSearchValue');
		return false;
	}
}

/************************************************************
function : ValidateCompleted
usage    : for enabling/disabling search textbox, disable if search by event status 
output   : true/false 
************************************************************/
function ValidateCompleted()
{
	var oElement
	oElement = document.getElementById("ddlSearchBy");
	if (oElement.value == 'EVTSTATUS')
	{
		oElement = document.getElementById("txtSearchValue");
		oElement.disabled = true;
		oElement.value='';
		SetCursor('ddlSearchBy');
		return false
	}
	else
	{
		oElement = document.getElementById("txtSearchValue");
		oElement.disabled = false;
		SetCursor('txtSearchValue');
		return false;
	}
}


/************************************************************
function : CheckMaxLength(Obj,ln)
usage    : for checking Total length
inputs   : object and length
output   : false if total length exceeds ln
e.g      : CheckLength(obj,ln)
************************************************************/
function CheckMaxLength(Obj,ln)
{
	var strText=Obj.value;
	if (Trim(strText).length > ln)
	{
		return false;
	}
	return true;
} 

/************************************************************
function : DeleletRecords(objName,strValue)
usage    : to give alert to user about deleteing records
inputs   : name of grid in which records are listed, message to fire records are being deleted
output   : false if user cancel event
************************************************************/
function DeleletRecords(objName,strValue)
{
	//objName="dgCategoryList";		
	var bonesel,name;
	
	name=objName + "__ctl"
	if (document.all[objName] !=null)
	{
		var rowItems = eval(objName).rows;
		var rowCount = rowItems.length;
		bonesel = false
		for (i=3; i<= rowCount; i++)
		{
			oElement = document.getElementById(name + i + "_chkDel");
			if(oElement.checked == true)
			{
				bonesel=true;
				break;
			}
		}
		if (bonesel == false)
		{
			alert("Please Select at least one " + strValue + " to delete.");
			return false;
		}
		else
		{
			var sResult
			sResult = confirm("Are you sure want to delete selected record(s)?")
			if (sResult == true)
				return true
			else
				return false
		}
	}
	return true;
}	

/************************************************************
function : CheckLogin()
usage    : To check NULL/EMPTY value in txtUserName and txtPassword
output   : returns true if string contains some value; otherwise returns false
e.g      : p = 'abc' is valid, p = '' is invalid.
************************************************************/
function CheckLogin()
{
	document.getElementById('lblErrorMessage').innerText="";
	if (CheckBlank(document.getElementById('txtUserName')))
		{
			alert("Username Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Username Cannot be Blank";
			document.getElementById('txtUserName').focus();
			return false;
		}
	else if (CheckBlank(document.getElementById('txtPassword')))
		{
			alert("Password Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Password Cannot be Blank";
			document.getElementById('txtPassword').focus();
			return false;
		}
	return true;
}

/************************************************************
function : CheckSendPassword()
usage    : To check NULL/EMPTY value in txtUserName, txtEmail, txtQuestion and txtAnswer
output   : returns true if string contains some value; otherwise returns false
e.g      : p = 'abc' is valid, p = '' is invalid.
************************************************************/
function CheckSendPassword()
{
	document.getElementById('lblErrorMessage').innerText="";
	if (CheckBlank(document.getElementById('txtUserName')) || CheckBlank(SendPassword.txtEmail))
		{
			alert("Enter Username and Email to confirm your identity");
//			document.getElementById('lblErrorMessage').innerText="Enter Username and Email to confirm your identity";
			document.getElementById('txtUserName').focus();
			return false;
		}
	else if (CheckBlank(document.getElementById('txtAnswer')))
		{
			alert("Secret Answer Cannot be Blank");
			//document.getElementById('lblErrorMessage').innerText="Secret Answer Cannot be Blank";
			document.getElementById('txtAnswer').focus();
			return false;
		}
	return true;
}

/************************************************************
function : checkText()
usage    : if data entered in file field manually cancel it
************************************************************/
function checkText()
{
	if(window.event.keyCode==9)
		return true;
	else
		return false;
}

/********************************************************
function : IsNumber
usage	 : if data entered is numeric only return true else false
**********************************************************/
function IsNumber(strNum)
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
	if(strNum.indexOf("-")!=-1)
	{
		return false;
	}
	if(isNaN(strNum))
	{
		return false;
	}
	return true;
}

/********************************************************
function : RemoveHash
usage	 : if data entered contains # sign that it wont allow to enter.
**********************************************************/
function RemoveHash()
{
	//alert(event.keyCode);
	if(event.keyCode==35 || event.keyCode==96 || event.keyCode==39)
	{
		event.keyCode=0;
	}
	//alert(event.keyCode);
	/*if (!(event.keyCode >= '48' && event.keyCode <= '57'))
	{
		event.keyCode=0;
	}*/
}

/********************************************************
function : CheckSpecialChar(Obj)
usage	 : if data entered contains `,#,' dont allow.
example	 : CheckSpecialChar(txtAddress)
**********************************************************/
function CheckSpecialChar(Obj)			
{
	var strText=Obj.value;
	//alert(strText.length);
	for(var j=0;j<strText.length;j++)
	{
		if (strText.charAt(j)=="`" || strText.charAt(j)=="#" || strText.charAt(j)=="'")
		{
			Obj.value='';
			Obj.focus();
			return false;
		}
	}
	return true;
}
