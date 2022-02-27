//	This will Prevent user from entering Enter Key in the Text Box when there is no other character present.

function CheckEnterKey()
{
	
	strDetails=Trim(document.getElementById('txtRoomDetail').value);				
					
	if (strDetails.length == 0)
	{
		if (event.keyCode == 13)
		{						
			window.event.returnValue = null;
		}					
	}				
}
//---------------------------------------------------------------------	
//	This will entirely Prevent user from entering Enter Key in the Text Box.
		
function CheckEnterKey()
{	
		if (event.keyCode == 13)
		{						
			window.event.returnValue = null;
		}			
}



/*

<asp:textbox id="txtRoomDetail" runat="server" TextMode="MultiLine" onkeypress="CheckEnterKey();"></asp:textbox>

*/