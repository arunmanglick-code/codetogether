function selected(cal, date)

{

  cal.sel.value = date;

  if (cal.sel.id == "sel1" || cal.sel.id == "sel3")

    cal.callCloseHandler();

}

function closeHandler(cal) {

  cal.hide();

}

function showCalendar(id, format) {

  var el = document.getElementById(id);

  if (calendar != null) {

    calendar.hide();

  } else {

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

var MINUTE = 60 * 1000;

var HOUR = 60 * MINUTE;

var DAY = 24 * HOUR;

var WEEK = 7 * DAY;

function isDisabled(date) {

  var today = new Date();

  return (Math.abs(date.getTime() - today.getTime()) / DAY) > 10;

}