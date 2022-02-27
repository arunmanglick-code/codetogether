//------------------------------------------------
// Change Row Color on MouseOver & MouseOut
//------------------------------------------------
var oldColor;
function ChangeRowColor(objRow)
{
	//debugger;
	oldColor=objRow.style.backgroundColor;		
	objRow.style.backgroundColor='lightgrey';
	objRow.style.cursor='hand';		
}

function ResetRowColor(objRow)
{
	objRow.style.backgroundColor=oldColor;
	objRow.style.textDecoration='none';
}
//------------------------------------------------