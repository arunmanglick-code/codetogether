
//This function will trace for Backspace keystroke
//If the user clicked on Backspace then it will reset the keystroke and nothing will happen
//This is used to disable the functionality of going back to previous page
//Yatin Patel					8/4/99


//Jedy: This function does not allow to open .asp or .htm
//files directly by typing url in browser.

//Function removes all options(items) from List box or Combo box
function AddOptions(objList)
{
	//this function added by ankit on 05 31 01 for filling up combo or list box
	//Parameter : objCombo
	//Description: Combobox or List box element
	
		var Operation =CustLocData.recordset;
		
	var intNoOfOptions
	var objOptions

	intNoOfOptions = Operation.recordcount;
	objOptions = objList.options;
	for(intPos=0; intPos<intNoOfOptions; intPos++)
	{
		var oOption = document.createElement("OPTION");
		oOption.text=Operation(0).value;
		oOption.value=intPos;
		objList.add(oOption);
		Operation.MoveNext();
	}
}
function RemoveAllOptions(objCombo)
{
	//Parameter : objCombo
	//Description: Combobox or List box element

	var intNoOfOptins
	var objOptions

	intNoOfOptins = objCombo.length;
	objOptions = objCombo.options;
	for(intPos=0; intPos<intNoOfOptins; intPos++)
	{
		objOptions.remove(0);
	}
}
//This function replaces all occurances of strFind with strRepl in string strSourceText
function ReplaceAll(strSourceText, strFind, strRepl)
{
	var intPos = strSourceText.indexOf(strFind);
	while (intPos >= 0)
	{
		strSourceText = strSourceText.replace(strFind, strRepl);
		intPos = strSourceText.indexOf(strFind);
	}
	return (strSourceText);
}

//Function removes leading and trailing blank spaces from the string
function Trim(strText)
{
	strText = LTrim(strText)
	strText = RTrim(strText)
	return(strText);
}

//Function removes trailing blank spaces from the string
function RTrim(strText)
{
		/*var char
		var intLen
		var intIndex
		intLen = strText.length;
		for(intIndex = intLen-1; intIndex >= 0; intIndex--)
		{
			char = strText.substring(intIndex,intIndex + 1);
			if( char != ' ')
			{
				break;
			}
		}
		return(strText.substring(0,intIndex+1));
		*/
		
		var iLen=strText.length
						
		for (iLen; iLen>=0 ; iLen--)
		{
			
			if (strText.charAt(iLen-1)!=' ')
			{
				break;
			}
		}
			strText=strText.substring(0,iLen)
			return strText
}

//Function removes leading spaces from the string(i.e. LTrim)
function LTrim(strText)
{
	/*var char
	var intLen
	var intIndex
	intLen = strText.length;
	for(intIndex = 0; intIndex <= intLen-1; intIndex++)
	{
		char = strText.substring(intIndex,intIndex + 1);
		if( char != ' ')
		{
			break;
		}
	}
	return(strText.substring(intIndex,intLen));*/
	
		var iCurrPos=iLen			
		var iLen=strText.length
		for (iCurrPos=0; iCurrPos<iLen ; iCurrPos++)
		{
			if (strText.charAt(iCurrPos)!=' ')
			{
				break;
			}
		}
			strText=strText.substring(iCurrPos,iLen)	
			return strText

}


function TraceBackSpace()
{
	if (event.keyCode == 8)
		event.keyCode = 0
}

//Function to replace values within a string.
function Replace(fieldname)
{  
  var str=fieldname.value.replace("'","`");	  
  return (str);		
}   	
	
//------------------------------------------
//checks for AlphaNumeric Field with Spaces
//------------------------------------------
function isAlphaNumericWithSpace(fieldname)
{
	var objRule = /^[ ]?([a-zA-Z0-9]+[ ]?)+$/;
	
	return objRule.test(fieldname);
}

//-------------------------------
//checks if it is an alphaNumeric field, space, hyphen and underscore allowed.
//-------------------------------
function isAlphaNumericSpaceHyphenUnderscore(fieldname)
{
	var objRule = /^[-_ ]?([a-zA-Z0-9]+[-_ ]?)+$/;
	
	return objRule.test(fieldname);
}
//-------------------------------
//checks if it is an alpha field, space, hyphen and underscore allowed.
//-------------------------------

function isAlphaSpaceHyphenUnderscore(fieldname)
{
	var objRule = /^[-_ ]?([a-zA-Z]+[-_ ]?)+$/;
	
	return objRule.test(fieldname);
}

function isApostrophe(fieldname)
{
	var objRule = /[']/;
	
	return objRule.test(fieldname);
}

function isNumber(fieldname)
{  
	var objRule = /^\s*[0-9]+\s*$/;
	
	return objRule.test(fieldname);
}

function isAlphaNumeric(fieldname)
{
	var objRule = /^\s*[0-9a-zA-Z]+\s*$/;
	
	return objRule.test(fieldname);
}

function isAllZero(fieldname)
{
	var objRule = /^\s*[0]+\s*$/;
	
	return objRule.test(fieldname);
}

function isAllSpaces(fieldname)
{
	var objRule = /^\s+$/;
	
	return objRule.test(fieldname);
}

function isAnySpace(fieldname)
{
	var objRule = /\s+/;
	
	return objRule.test(fieldname);
}

//------------------------------------------------------------------------------------------
//Checks for Leap Year Note: This has to be a four digit year
//------------------------------------------------------------------------------------------
function isLeapYear(year)
{
	//Modified by Yatin Patel -- this was (Year & 400) instead of (Year % 400)
	if ((year % 400) == 0)
		//return true;
		return false;
	//End modification by Yatin Patel 8/10/1999
	if ((year % 100) == 0)
		return false;
	if ((year % 4) == 0)
		//return true;
		return false;
	return false;
}

//------------------------------------------------------------------------------------------
//Validates the date entered.
//------------------------------------------------------------------------------------------
function isValidDate(year,month,day)
{
	if((month == 1) || (month == 3) || (month == 5) || (month == 7) || (month == 8) || (month == 10) || (month == 12))
	{
		if((day > 0) && (day < 32)) return true;
	}
	else if((month == 4) || (month == 6) || (month == 9) || (month == 11))
	{
		if((day > 0) && (day < 31))	return true;
	}
	else if(month == 2)
	{
		if (isLeapYear(year))
		{
			if ((day > 0) && (day < 30)) return true;
		}
		else
		{
			if ((day > 0) && (day < 29)) return true;
		}
	}
	return false;
}

//function validates date string entered in MM/DD/(YY or YYYY) OR DD/MM/(YY or YYYY) format
function isValidDateString(strDate)
{
	
	//For example if strDate = '7/26/1972'
	var strYr, strMon, strDay
	var iPos
	
	//If strDate is blank
	if(strDate == "") return false;
	
	//Extract Month
	iPos = strDate.indexOf('/');
	if(iPos > 0)
	{
		//strMon will be 7
		strMon = strDate.substring(0,iPos);
		//If it is blank or not numeric
		if( (strMon=="") || (isNaN(strMon)==true) ) return false;

		//strip off month part from date. strDate will be 26/1972
		strDate = strDate.substring(iPos+1,strDate.length);
	}
	else
	{
		return false;
	}

	//If strDate is blank
	if(strDate == "") return false;

	//Extract Day
	iPos = strDate.indexOf('/');
	if(iPos > 0)
	{
		//strDay will be 26
		strDay = strDate.substring(0,iPos);
		//If it is blank or not numeric
		if( (strDay=="") || (isNaN(strDay)==true) ) return false;

		//strip off month part from date. strDate will be 1972
		strDate = strDate.substring(iPos+1,strDate.length);
	}
	else
	{
		return false;
	}

	//If strDate is blank
	if(strDate == "") return false;

	//rest of part is year
	strYr = strDate;
	//If string is greater than 4 digits or not a number it is not valid year
// 	if((strYr.length > 4) ||(strYr.length == 3)|| (isNaN(strYr)==true)) return false

 	if((strYr.length != 4) || (isNaN(strYr)==true)) return false
	
	//If all three parameters are valid numbers then check if it is valid date
	if(isValidDate(strYr,strMon,strDay)==false) return false;
	return true;
}



//This function returns the value of strtoken from the hidden parameter list
function GetTokenValue(objThis,strToken)
{
	var intPos;
	var strTemp;
	var intPos1;
	
	var strParams = objThis.value;
		
	if (strToken.substr(strToken.length-1) != ':')
	{
		strToken = strToken + ':';
	}
	
	strToken = strToken.toUpperCase();
	strParams = strParams.toUpperCase();
	
	if (strParams.length == 0)
	{
		strTemp = '';
	}
	else
	{
		intPos = strParams.indexOf(strToken);
		intPos1 = strParams.indexOf(";",intPos);
		if (intPos1 == -1) 
		{
			strTemp = strParams.substr(intPos+strToken.length);
		} else {
			strTemp = strParams.substring(intPos+strToken.length,intPos1);
		}
	}
	return strTemp;
}

function stripBlanks(theString)
{
	var re = /\s/;
	return theString.replace(re,"")
}

function isFutureDate(strCheckDate, strBaseDate)
{
	var checkDate = new Date(strCheckDate);
	var baseDate = new Date(strBaseDate);
	if (checkDate.getTime() > baseDate.getTime())
		return (true);
	else
		return (false);
}

function getDateDifferenceYears(strLesserDate, strGreaterDate)
{	
	//This function will return the difference between two dates, in number of years.
	var strLesserDateMonth = strLesserDate.substring(0,2);
	var strLesserDateDay = strLesserDate.substring(3,5);
	var strLesserDateYear = strLesserDate.substring(6,10);
	var strGreaterDateMonth = strGreaterDate.substring(0,2);
	var strGreaterDateDay = strGreaterDate.substring(3,5);
	var strGreaterDateYear = strGreaterDate.substring(6,10);
	var intControl = 0;
	var intYearsDiff = 0;
	if (strLesserDateMonth > strGreaterDateMonth) intControl = 1;
	if (strLesserDateMonth == strGreaterDateMonth && strLesserDateDay > strGreaterDateDay) intControl = 1;
	if (intControl == 1) 
	 	intYearsDiff = strGreaterDateYear - strLesserDateYear - 1 ;
	 else
	 	intYearsDiff = strGreaterDateYear - strLesserDateYear;
	return (intYearsDiff);
}

//
// Function added to allow commas and spaces in an alpha numeric test.
//
function isCommaAlphaNumericSpace(fieldname)
{
	var objRule = /^[ ,a-zA-Z0-9]+$/;
	
	return objRule.test(fieldname);
}

//	strip off leading and trailing characters.
//	parameter 1 = string to strip
//	parameter 2 = 'b' (both) 'l' (leading) 't' (trailing) - default = both
//	parameter 3 =  character to strip
function strip(str,option,char){
	//	check parameters
	if ((str == null) || (str == '')) return ''
	var workStr = '' + str
	workOption = '' + option
	workOption = workOption.toUpperCase()
	if ((workOption != 'B') && (workOption != 'L') &&
		(workOption != 'T')) workOption = 'B'
	if ((char != null) && (char != '')) stripChar = '' + char
	  else stripChar = ' '
	//	find the first character which is not the stripChar if stripping leading
	if (workOption == 'T') startIndex = 0
	  else for (var startIndex=0;startIndex < workStr.length;startIndex++){
		if (workStr.charAt(startIndex) != stripChar) break
		}
	//	find the last character which is not the stripChar if stripping trailing
	if (workOption == 'L') endIndex = workStr.length
	  else for (var endIndex=workStr.length;endIndex > startIndex;endIndex--){
		if (workStr.charAt(endIndex - 1) != stripChar) break
		}
	//	return the string
	return substring(workStr,startIndex,endIndex)
	}

//	Return the left portion of a string.
//	parameter 1 = string to process
//	parameter 2 = desired string length
//	parameter 3 =  optional pad character if the input string is too short - default is ' '
function left(str,length,pad){
	//	check parameters
	var workStr = '' + str
	var workLength = parseInt('' + length,10)
	if (pad == null) var workPad = ' '
	  else var workPad = '' + pad
	//	pad character is only 1 position
	if (workPad == '') workPad = ' '
	  else workPad = workPad.charAt(0)
	//	if the string is too long, strip off the end
	if (workStr.length > workLength) return workStr.substring(0,workLength)
	//	otherwise, add characters to the end
	return workStr + repeatString(workPad,workLength - workStr.length)
	}

//	Return the right portion of a string.
//	parameter 1 = string to process
//	parameter 2 = desired string length
//	parameter 3 =  optional pad character if the input string is too short - default is ' '
function right(str,length,pad){
	//	check parameters
	var workStr = '' + str
	var workLength = parseInt('' + length,10)
	if (pad == null) var workPad = ' '
	  else var workPad = '' + pad
	//	pad character is only 1 position
	if (workPad == '') workPad = ' '
	  else workPad = workPad.charAt(0)
	//	if the string is too long, strip off the beginning
	if (workStr.length > workLength)
		return workStr.substring(workStr.length - workLength)
	//	otherwise, add characters to the beginning
	return repeatString(workPad,workLength - workStr.length) + workStr
	}

//	return the substring of a string (str.substring() is flakey)
//	parameter 1 = the string
//	parameter 2 = the start index
//	parameter 3 = the end index (index after the required ending character)
function substring(str,index1,index2){
	//	check out the parameters
	if ((str == null) || (str == '')) return ''
	var workStr = '' + str
	if ((index1 == null) || (index1 == '')) work1 = 0
	  else work1 = parseInt('' + index1,10)
	if ((index2 == null) || (index2 == '')) work2 = str.length
	  else work2 = parseInt('' + index2,10)
	//	flip the indexes if they are in the wrong order
	if (work1 < work2){
		startIndex = work1
		endIndex = work2
		}
	  else {
		startIndex = work2
		endIndex = work1
		}
	//	build the output string
	newStr = ''
	for (var i=startIndex;i < endIndex;i++) newStr += workStr.charAt(i)
	return newStr
	}
//	Return a string which consists of the string in argument 1 repeated the number of
//	times specified in argument 2.
function repeatString(str,times){
	if ((str == null) || (str == '')) return ''
	if ((times == null) || (times == '')) return ''
	workStr = '' + str
	workTimes = parseInt('' + times,10)
	strOut = ''
	for (var i=0;i < workTimes;i++) strOut += workStr
	return strOut	
	}


function validateInput(strKeyCodes)
{	
	// receives the KeyCodes as a string seperated by commas(,)
	// ie to validate #,_ pass ("35,95,.,.,)

	//Restrict Pipe character everywhere
	strKeyCodes = strKeyCodes + ",35,124" //(Restrict hash(#) and pipe(|) characters)
	arrKeyCodes = strKeyCodes.split(",");
	var iLen, iIndex
	
	iLen = arrKeyCodes.length;
	for(iIndex=0; iIndex<iLen; iIndex++)
	{	
		if(window.event.keyCode == arrKeyCodes[iIndex])
		{
			window.event.returnValue = false;
		}
	}
	return true;
}



 function GetToken(strSourceString, strToken)
 {
	var strSeperator		//by which tokens are seperated
    var intTokenPos			//Token Position
    var intTokenSepPos		//Position of Token seperator
    var intTokenLength		//Token Length
	
	strSeperator = ';';		//Set Token seperator
	
	strSourceString = Trim(strSourceString);
	strToken = Trim(strToken);
	
	//If Colon exist in token, remove it  
    if(strToken.substring(strToken.length-1, strToken.length) == ':')
    {
        strToken = Trim(strToken.substring(0, strToken.length-1));
    }

	//Find Token position
    intTokenPos = strSourceString.toUpperCase().indexOf(strToken.toUpperCase());
    
    //If token not found return blank value
    if(intTokenPos == -1)
    {
        return "";
    }
    else
    {
        intTokenLength = strToken.length;
        var strTemp = strSourceString.substring(intTokenPos + intTokenLength + 1, strSourceString.length);

        //Find first seperator position after Token
        intTokenSepPos = strTemp.indexOf(strSeperator)

        //If seperator not found, return rest of string after token
        if(intTokenSepPos == -1)
        {
			return strSourceString.substring(intTokenPos + intTokenLength + 1,strSourceString.length);
        }
        else
        {
			intTokenSepPos += intTokenPos + intTokenLength + 1;
			//Extract Token value between Token string and Seperator
			return strSourceString.substring(intTokenPos + intTokenLength + 1,intTokenSepPos);
        }
    }
 }

 function PutToken(strSource, strToken, strValue)
 {
    var intTokenPos   
    var intSepPos
    var intPos2     
	var strSep		//Seperator character
	var strSourceString;
	strSourceString = strSource;

	strSep = ';';
	
    strToken = Trim(strToken.toUpperCase());
    strSourceString = strSourceString.toUpperCase();

	//If Colon exist in token, remove it  
    if(strToken.substring(strToken.length-1, strToken.length) == ':')
    {
        strToken = Trim(strToken.substring(0, strToken.length-1));
    }
    
    //Find Token Position
    intTokenPos = strSourceString.indexOf(strToken);
    
    //If Token is found
    if (intTokenPos != -1)
    {
        //intPos1 = InStr(intPos + 1, strParams, ";")
        //Find first seperator position after Token
        intSepPos = strSourceString.indexOf(strSep,intTokenPos + 1);
        
        //if Seperator found
        if(intSepPos != -1)
        {
            //strParams = Left(strParams, intTokenPos + Len(strToken)) & strValue & Mid$(strParams, intSepPos)
            //Replace old Token value with New
            strSource = strSourceString.substring(0,intTokenPos + strToken.length +1) + strValue + strSourceString.substring(intSepPos, strSourceString.length)
        }
        else
        {
			//If Token is at last position
            //strParams = Left$(strParams, intTokenPos - 1) & strToken & ":" & strValue & ";"
            strSource = strSourceString.substring(0, intTokenPos ) + strToken + ':' + strValue + strSep;
        }
    }
    else
    {
        //If seperator is found at last position of string
        if(strSourceString.substring(strSourceString.length-1, strSourceString.length) == strSep)
        {
			strSource = strSourceString + strToken + ':' + strValue + strSep;
        }
        else
        {
			//First, append the seperator to string and then append the Token and its value
			strSource = strSourceString + strSep + strToken + ':' + strValue + strSep;
        }
    }
	return strSource;
 }



function OnBack_Click()
{
	window.history.back(1);
	return false;
}


function chkThisField(fieldname)
{	   
	var objRule = /[@!#$%^&*(){}|\[\];:\'\"<>\?`~\/\\]/;
	
	return objRule.test(fieldname);
}

function chkWOSq(fieldname)
{	   
	var objRule = /[@!#$%^&*(){}|\[\];:\"<>\?`~\/\\]/;
	
	return objRule.test(fieldname);
}

function chkLimitedField(fieldname)
{	   
	var objRule = /[@!$%^&*(){}|\[\];:\"<>\?`~\/\\]/;
	
	return objRule.test(fieldname);
}

function isValid_EMailFormat(as_email)
{
	var email_addr_len = as_email.length;
	var asterik_pos1 = as_email.indexOf("@");
	var asterik_pos2 = as_email.lastIndexOf("@");
	if (asterik_pos1 != asterik_pos2)
	{
		return false;
	}
   //obtain position of last . character in the e-mail address
   var period_pos = as_email.indexOf(".");
   if (period_pos == asterik_pos1 - 1)
   {
	return false;
   }
   if (period_pos == asterik_pos1 + 1)
   {
	return false;
   }
    var period_pos1 = as_email.lastIndexOf(".");
	var strEmlText =  Trim(as_email);
	strEmlText = strEmlText.split(".");
	for (iEle1 = 0; iEle1 < strEmlText.length; iEle1++)
	{
		if (strEmlText[iEle1].length == 0)
		{
			return false;
		}
	}
	strEmlText =  Trim(as_email);
	strEmlText = strEmlText.split(".@");
	if (strEmlText.length > 1)
	{
		return false;
	}
	strEmlText =  Trim(as_email);
	strEmlText = strEmlText.split("@.");
	if (strEmlText.length > 1)
	{
		return false;
	}
	strEmlText =  Trim(as_email);
	strEmlText = strEmlText.split(",");
	if (strEmlText.length > 1)
	{
		return false;
	}
   if (period_pos == -1)
   {
	return false;
   }
   if (period_pos == (period_pos1 - 1))
   {
	return false;
   }
   if (asterik_pos1 == (period_pos -1))
   {
	return false;
   }
   if (period_pos == (asterik_pos1 -1))
   {
	return false;
   }
   if (as_email.lastIndexOf(".") == (email_addr_len - 1))
   {
	return false;
   }
   

   //check for minimum length 
   if  (as_email.length <= 5 ) 
        return false;
        
      
    //spaces not allowed 
    else if (as_email.indexOf(" ",1)!= -1)
       return false;
        
        
     //check for asterik character  
    else if (as_email.indexOf("@",1) < 1)
        return false;
    
    strEmlText =  Trim(as_email);
	strEmlText = strEmlText.split("@");
	if (strEmlText[1].indexOf(".") == -1)
       return false;
    //ensure period character appears after asterik
    //else if (period_pos < (asterik_pos + 2)) 
    //    return false;
 
  return true;
}

function isValidDateString(strDate)
{
	//For example if strDate = '7/26/1972'
	var strYr, strMon, strDay
	var iPos
	
	//If strDate is blank
	if(strDate == "") return false;
	
	//Extract Month
	iPos = strDate.indexOf('/');
	if(iPos > 0)
	{
		//strMon will be 7
		strMon = strDate.substring(0,iPos);
		//If it is blank or not numeric
		if( (strMon=="") || (isNaN(strMon)==true) || (strMon.length != 2)) return false;

		//strip off month part from date. strDate will be 26/1972
		strDate = strDate.substring(iPos+1,strDate.length);
	}
	else
	{
		return false;
	}

	//If strDate is blank
	if(strDate == "") return false;

	//Extract Day
	iPos = strDate.indexOf('/');
	if(iPos > 0)
	{
		//strDay will be 26
		strDay = strDate.substring(0,iPos);
		//If it is blank or not numeric
		if( (strDay=="") || (isNaN(strDay)==true)|| (strDay.length != 2)) return false;

		//strip off month part from date. strDate will be 1972
		strDate = strDate.substring(iPos+1,strDate.length);
	}
	else
	{
		return false;
	}

	//If strDate is blank
	if(strDate == "") return false;

	//rest of part is year
	strYr = strDate;
	//If string is greater than 4 digits or not a number it is not valid year
// 	if((strYr.length > 4) ||(strYr.length == 3)|| (isNaN(strYr)==true)) return false

 	if((strYr.length != 4) || (isNaN(strYr)==true)) return false
	
	//If all three parameters are valid numbers then check if it is valid date
	if(isValidDate(strYr,strMon,strDay)==false) return false;
	return true;
}


function CompareDate(strStartDate, strEndDate)
{
	var sDay;
	var sMonth;
	var sYear;
	var eDay;
	var eMonth;
	var eYear;
	var intPos;
	var intCnt;
	intPos = strStartDate.indexOf('/');
	intCnt=1;
	while (intCnt <= 2)
	{
		if (intCnt==1)
			{
			sMonth=Math.abs(left(strStartDate,intPos));
			eMonth=Math.abs(left(strEndDate,intPos));
			strStartDate=substring(strStartDate,intPos+1,strStartDate.lenght);
			strEndDate=substring(strEndDate,intPos+1,strEndDate.lenght);
			}

	     else if(intCnt==2)
	       {
			sDay=Math.abs(left(strStartDate,intPos));
			eDay=Math.abs(left(strEndDate,intPos));

			strStartDate=substring(strStartDate,intPos+1,strStartDate.lenght);

			strEndDate=substring(strEndDate,intPos+1,strEndDate.lenght);
			sYear=Math.abs(strStartDate);
			eYear=Math.abs(strEndDate);
		   }
   
	intPos = strStartDate.indexOf('/');
	intCnt=intCnt+1;
	}

	if(sYear == eYear && sDay == eDay && sMonth == eMonth)
	 {
	   return true
	   }
	if(sYear > eYear)
	 {
	  return false;
	 }
	if(sYear == eYear)
	 {
	  if(sMonth > eMonth)
	  {
	  return false;
	  }
	 }
	if(sYear == eYear && sMonth == eMonth)
	{
		if(sDay < eDay)
			return true;
		else
			return false;
	}
	return true;
}

function chkThisEmailField(fieldname)
{	   
	var objRule = /[!#$%^&*(){}|\[\];:\'\"<>\?`~\/\\]/;
	
	return objRule.test(fieldname);
}

function isVersion(fieldname)
{  
	var objRule = /^([0-9]\.?)+[0-9]+$/;
	//alert(objRule.test(fieldname));
	return objRule.test(fieldname);
}
function checkInteger()
{  
	if (window.event.keyCode >=47 && window.event.keyCode <= 57)
		return true;
	else
		return false;
}  
function isSpecial(fieldname)
{
	var objRule = /[~"|?]/;
	
	return objRule.test(fieldname);
}
