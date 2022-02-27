// -------------------------------------------------
// Open a window and center it
// -------------------------------------------------
var myWindow;

function OpenCenteredWindow() {
    var width = 400;
    var height = 300;

    var left = parseInt((screen.availWidth / 2) - (width / 2));   // parseInt((screen.availWidth  - width) / 2);    // Either will Do
    var top = parseInt((screen.availHeight / 2) - (height / 2));  // parseInt((screen.availHeight -  height) / 2);  // Either will Do

    var windowFeatures = "width=" + width;
    windowFeatures += ",height=" + height;
    windowFeatures += ",status,resizable";
    windowFeatures += ",left=" + left;
    windowFeatures += ",top=" + top;
    windowFeatures += "screenX=" + left;
    windowFeatures += ",screenY=" + top;

    myWindow = window.open('CenteredWindow.htm', "subWind", windowFeatures);
}
// -------------------------------------------------


