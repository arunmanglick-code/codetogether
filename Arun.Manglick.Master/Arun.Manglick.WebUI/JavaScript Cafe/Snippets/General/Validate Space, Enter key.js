// This will check if there is only Space /Enter key character presents.
// i.e If there are some valid character (A-Z, 0-9) already present and then these characters are also present
// then it will pass the case.
//------------------------------------------------------------------------
function AllSpace_EnterKey()
{
	//strDetails=Trim(document.getElementById('txtRoomDetail').value);
	strDetails=document.getElementById('txtRoomDetail').value;
	
	var regexp=/^\s+$/;
	
	if (regexp.test(strDetails))				
		return false;
				 	
		return true;				 
}
//------------------------------------------------------------------------





// This will check if there is any Space /Enter key character presents.
// i.e If there are some valid character (A-Z, 0-9) already present and then these characters are also present
// then also it will Fail the case.
//------------------------------------------------------------------------
function AnySpace_EnterKey()
{
	//strDetails=Trim(document.getElementById('txtRoomDetail').value);
	strDetails=document.getElementById('txtRoomDetail').value;
	
	var regexp=/\s+/;
	
	if (regexp.test(strDetails))				
		return false;
				 	
		return true;				 
}
//------------------------------------------------------------------------