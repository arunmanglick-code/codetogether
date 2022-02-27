/*
<asp:textbox onkeypress="keypresshandler()" id="txtNoOfClients" onkeyup="onlyNumbersBoth()" tabIndex="8"
							runat="server" Width="21px" BorderStyle="Groove" MaxLength="1">1</asp:textbox>
*/
function onlyNumbersBoth(){
		if (event.keyCode >= '49' && event.keyCode <= '57'){
		
		}
		else{
			event.keyCode=0;
		}
		if (Form1.txtNoOfClients.value.length > 0){			
			strformulaUnits = "(" +Form1.txtSupport.value + ")/" + Form1.txtNoOfClients.value;
			strformulaMinutes = "(" +Form1.txtSupport.value + ")/" + Form1.txtNoOfClients.value;
			
			Form1.txtTotalUnits.value = parseInt(eval(strformulaUnits));
			Form1.txtTotalMinutes.value = parseInt(eval(strformulaMinutes));
		}
	}