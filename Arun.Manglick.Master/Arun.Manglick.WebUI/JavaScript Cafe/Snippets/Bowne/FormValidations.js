//
//	Current File is $Archive: /ProposingBowneLive/ProposingBowne/_js/FormValidations.js $ 
//	Modified By $Author: Vc $  
//	On $Date: 17/06/05 7:27p $  
//	Current Version is $Revision: 1 $  
//
//****************************************************************************************************
//
// ---------------------------------------------------------------------------------------------------
// Purpose    : Validate Text Box value.
// Inputs     :
//				sName       - Text Box Name
//				bcheckChars - True/False, should check min/max chars?, usually, always True
//				iMin, iMax  - Minimum & Maximum characters allowed respectively
//				bRequired   - True/False, is required value? usually, always True
// Return     : true if valid, false if not valid
// ---------------------------------------------------------------------------------------------------
function TextValidation(sName, bCheckChars, iMin, iMax, bRequired)
{
	if (bRequired == true)
	{
		if (sName.value == "")
		{
			return false;
		}

		if (bCheckChars == true)
		{
			if (sName.value.length < iMin)
			{
				return false;
			}
			if (sName.value.length > iMax)
			{
				return false;
			}
		}
	}
	else
	{
		if (sName.value != "" && bCheckChars == true)
		{
			if (sName.value.length < iMin)
			{
				return false;
			}
			if (sName.value.length > iMax)
			{
				return false;
			}
		}
	}
	return true;
}

// ---------------------------------------------------------------------------------------------------
// Purpose    : Validate and Compare Both field.
// Inputs     :
//				sField1     - Text Box Object 1
//				sField2     - Text Box Object 2
//				bCheckChars - True/False, should check min/max chars?, usually, always True
//				iMin, iMax  - Minimum & Maximum characters allowed respectively
//				bRequired   - True/False, is required value? usually, always TRUE
// Return     : true if valid, false if not valid
// ---------------------------------------------------------------------------------------------------
function DupValidation(sField1, sField2, bCheckChars, iMin, iMax, bRequired)
{
	if (TextValidation(sField1, bCheckChars, iMin, iMax, bRequired) == true)
	{
		if (TextValidation(sField2, bCheckChars, iMin, iMax, bRequired) == true)
		{
			if (sField1.value != sField2.value)
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}
	else
	{
		return false;
	}
	return true;
}

// ---------------------------------------------------------------------------------------------------
// Purpose    : Validate e-mail.
// Inputs     :
//				sName       - Text Box name
//				bCheckChars - True/False, should check min/max chars?, usually, always True
//				iMin, iMax  - Minimum & Maximum characters allowed respectively
//				bRequired   - True/False, is required value? usually, always TRUE
// Return     : true if valid, false if not valid
// ---------------------------------------------------------------------------------------------------
function EmailValidation(sName, bCheckChars, iMin, iMax, bRequired)
{
  if (TextValidation(sName, bCheckChars, iMin, iMax, bRequired) == true)
  {
		if (bRequired == true)
		{
			if (sName.value != "")
			{
				if (sName.value.search(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/) != -1)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
	}
	else
	{
		return false;
	}
	return true;
}

// ---------------------------------------------------------------------------------------------------
// Purpose    : Validate for numeric input.
// Inputs     :
//				sName       - Text Box name
//				bCheckChars - True/False, should check min/max chars?, usually, always True
//				iMin, iMax  - Minimum & Maximum characters allowed respectively
//				bRequired   - True/False, is required value? usually, always TRUE
// Return     : true if valid, false if not valid
//				"123" returns true, "abc44" returns false, "12.34" returns false, "-12" returns true
// ---------------------------------------------------------------------------------------------------
function NumericValidation(sName, bCheckChars, iMin, iMax, bRequired)
{
  if (TextValidation(sName, bCheckChars, iMin, iMax, bRequired) == true)
  {
		if (bRequired == true)
		{
			if (sName.value != "")
			{
				var strName = new String(sName.value);
				if(strName.indexOf(".")!=-1)
				{
					return false;
				}
				if(strName.indexOf("e")!=-1)
				{
					return false;
				}
				if(strName.indexOf("E")!=-1)
				{
					return false;
				}
				if(isNaN(strName))
				{
					return false;
				}
				return true;
			}
		}
	}
	else
	{
		return false;
	}
	return true;
}

// ---------------------------------------------------------------------------------------------------
// Purpose    : Validate for digit input.
// Inputs     :
//				sName       - Text Box name
//				bCheckChars - True/False, should check min/max chars?, usually, always True
//				iMin, iMax  - Minimum & Maximum characters allowed respectively
//				bRequired   - True/False, is required value? usually, always TRUE
// Return     : true if valid, false if not valid
//				"123" returns true, "abc44" returns false, "12.34" returns false, "-12" returns false
// ---------------------------------------------------------------------------------------------------
function DigitValidation(sName, bCheckChars, iMin, iMax, bRequired)
{
  if (TextValidation(sName, bCheckChars, iMin, iMax, bRequired) == true)
  {
		if (bRequired == true)
		{
			if (sName.value != "")
			{
				var strName = new String(sName.value);
				if(strName.indexOf(".")!=-1)
				{
					return false;
				}
				if(strName.indexOf("-")!=-1)
				{
					return false;
				}
				if(strName.indexOf("e")!=-1)
				{
					return false;
				}
				if(strName.indexOf("E")!=-1)
				{
					return false;
				}
				if(isNaN(strName))
				{
					return false;
				}
				return true;
			}
		}
	}
	else
	{
		return false;
	}
	return true;
}

// ---------------------------------------------------------------------------------------------------
// Purpose    : Validate for decimal input.
// Inputs     :
//				sName       - Text Box name
//				bCheckChars - True/False, should check min/max chars?, usually, always True
//				iMin, iMax  - Minimum & Maximum characters allowed respectively
//				bRequired   - True/False, is required value? usually, always TRUE
// Return     : true if valid, false if not valid
//				"123" returns true, "abc44" returns false, "12.34" returns true,"-12.34" returns true
// ---------------------------------------------------------------------------------------------------
function DecimalValidation(sName, bCheckChars, iMin, iMax, bRequired)
{
  if (TextValidation(sName, bCheckChars, iMin, iMax, bRequired) == true)
  {
		if (bRequired == true)
		{
			if (sName.value != "")
			{
				var strName = new String(sName.value);
				if(strName.indexOf("e")!=-1)
				{
					return false;
				}
				if(strName.indexOf("E")!=-1)
				{
					return false;
				}
				if(isNaN(strName))
				{
					return false;
				}
				return true;
			}
		}
	}
	else
	{
		return false;
	}
	return true;
}

// ---------------------------------------------------------------------------------------------------
// Purpose    : Validate for amount input.
// Inputs     :
//				sName       - Text Box name
//				bCheckChars - True/False, should check min/max chars?, usually, always True
//				iMin, iMax  - Minimum & Maximum characters allowed respectively
//				bRequired   - True/False, is required value? usually, always TRUE
// Return     : true if valid, false if not valid
//				"123" returns true, "abc44" returns false, "12.34" returns true,"-12.34" returns false
// ---------------------------------------------------------------------------------------------------
function AmountValidation(sName, bCheckChars, iMin, iMax, bRequired)
{
  if (TextValidation(sName, bCheckChars, iMin, iMax, bRequired) == true)
  {
		if (bRequired == true)
		{
			if (sName.value != "")
			{
				var strName = new String(sName.value);
				if(strName.indexOf("-")!=-1)
				{
					return false;
				}
				if(strName.indexOf("e")!=-1)
				{
					return false;
				}
				if(strName.indexOf("E")!=-1)
				{
					return false;
				}
				if(isNaN(strName))
				{
					return false;
				}
				return true;
			}
		}
	}
	else
	{
		return false;
	}
	return true;
}
