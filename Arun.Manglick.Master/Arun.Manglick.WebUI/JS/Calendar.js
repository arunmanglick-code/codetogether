function selected(cal, date)

{

  cal.sel.value = date;

  if (cal.sel.id == "sel1" || cal.sel.id == "sel3")

    cal.callCloseHandler();

}

function closeHandler(cal) {

  cal.hide();

}
// ------------------------------------------------------------------------
function showCalendar(id, format) 
{
		//debugger;
		var el = document.getElementById(id.id);

		if (calendar != null) 
			{

				calendar.hide();

			} 
		else 
			{

				var cal = new Calendar(false, null, selected, closeHandler);

				calendar = cal;

				cal.setRange(1900, 2070);

				cal.create();

			}

	calendar.setDateFormat(format);

	calendar.parseDate(el.value);

	calendar.sel = el;

	calendar.showAtElement(el);
	
	

	return false;

}
// ------------------------------------------------------------------------

var MINUTE = 60 * 1000;

var HOUR = 60 * MINUTE;

var DAY = 24 * HOUR;

var WEEK = 7 * DAY;

function isDisabled(date) {

  var today = new Date();

  return (Math.abs(date.getTime() - today.getTime()) / DAY) > 10;

}


//----------------------------------------------------------------
//To check valid date of the form dd/mm/yyyy or dd-mm-yyyy
//----------------------------------------------------------------
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
//----------------------------------------------------------------	