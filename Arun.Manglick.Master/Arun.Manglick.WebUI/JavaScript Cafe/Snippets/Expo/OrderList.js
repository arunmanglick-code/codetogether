
function ViewCart(p)
{
	//src is parent form, which opens view cart
	FilePath = "/exposervicedesk/Transaction/ViewCart.aspx?src=" + p + "&ordid=" + event.srcElement.nameProp;
	window.open (FilePath,"","left=0,top=0,width=720,height=650,status=no,toolbar=no,menubar=no,scrollbars=1");
	return false;
}