function ShowWinCalc()
{
	//debugger;
	var WshShell = new ActiveXObject("WScript.Shell");
	alert("Lauching Calculator");
	WshShell.Run("calc"); // winword, excel,pbrush
	WshShell.AppActivate("Calculator");

}

function ShowInternetExplorer() {
    //debugger;
    var WshShell = new ActiveXObject("WScript.Shell");
    alert("Lauching Calculator");
    WshShell.Run("calc"); // winword, excel,pbrush
    WshShell.AppActivate("Calculator");

}