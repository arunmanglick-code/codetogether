// To Determine if atleast one check box is checked.
// Method-1
function ValidateCheckAtleastOne()
{
	debugger;
	chkname1='dgCategories__ctl';
	chkname2='_chkCheck';				
	x=2;
	do
	{	
		obj=document.getElementById(chkname1 + x + chkname2);
		if(obj != null)
			{
				if(obj.checked)
					return true;
			}
		else
			{
				window.alert("Please select a Category");
				return false;
			}						
		x++;
							
	}while(true)	
}

// To Determine if atleast one check box is checked.
// Method-2
function ValidateCheck()
			{
				var name="dgCategories__ctl";
				var x=0;
				var i=3;
				do
				{
					oElement = document.getElementById(name + i + "_chkCheck");
					if (oElement == null)
					{
						x=1;
					}
					else
					{
						if (document.frmEditCategories.elements(name + i + "_chkCheck").checked == true)
							{
								return true;
							}
					}
					i++;
				}
				while(x<1);
				window.alert("Please select a Category");
				return false;
			}
//---------------------------------------------------------------------------
To check only one Radio button placed in a Datalist of Datagrid
function MutualSelection(objThis)
		{
				//debugger;
				var name="dtlstTemplate__ctl";
				var xit=0;
				var i=0;
				var cons =1;
				do
				{
					oElement = document.getElementById(name + i + "_rdbtnSelectTemplate");
					if (oElement == null)
						{xit=1;}
					else
					{oElement.checked=false;}
					
					i++;
				}while(xit<1);
				objThis.checked=true;
				
				// Update Template Count in Header. 
				//debugger;
				if (objThis.parentElement.innerText=='Select(0 KB)')
					window.document.getElementById('Header1_lblTemplate').innerHTML=0;
				else
					window.document.getElementById('Header1_lblTemplate').innerHTML=1;
				
		}
//---------------------------------------------------------------------------
function isDate(dateStr) {

		var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
		var matchArray = dateStr.match(datePat); // is format OK?

		if (matchArray == null) {
			alert("Please enter date as either mm/dd/yyyy or mm-dd-yyyy.");
			return false;
		}

		// parse date into variables
		month = matchArray[1];
		day = matchArray[3];
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
			alert("Month " + month + " doesn't have 31 days!")
			return false;
		}

		if (month == 2) { // check for february 29th
			var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
			if (day > 29 || (day==29 && !isleap)) {
			alert("February " + year + " doesn't have " + day + " days!");
			return false;
			}
		}
		return true;  // date is valid
		}
//---------------------------------------------------------------------------
 function PrintConfirmation()
		     {
		       //debugger;
		       window.alert("For Better Results, Please Choose Landscape Orientation");
		       window.document.execCommand("Print",true,null);
		     }
//---------------------------------------------------------------------------
// Accessing Outer Page control from Inner Page in IFrame.

function ShowPopup(objStrPopup)
{
	//debugger;
	parent.document.getElementById('Header1_pnlProposalPopup1').innerHTML=objStrPopup;
}

function UpdatePageCount()
{
	//debugger;
	if(document.getElementById('chkSelect').checked)
	{
		obj=parent.document.getElementById('Header1_lblPageCount');
		cnt=obj.innerHTML;
		cnt++;
		obj.innerHTML=cnt;
	}
	else
	{
		obj=parent.document.getElementById('Header1_lblPageCount');
		cnt=obj.innerHTML;
		cnt--;
		obj.innerHTML=cnt;
	}			
}


//---------------------------------------------------------------------------
 
//---------------------------------------------------------------------------
