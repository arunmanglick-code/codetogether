//---------------------------------------------------------------------------
//	proposal_viewer.aspx
//---------------------------------------------------------------------------
var PageNum=0;
var SlideNum=0;		

//-------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
function GoPrevious()
{
	//debugger;
	if(PageNum > 0)
		{
			document.getElementById(PageNum).className="NormalAnchor";
			document.getElementById('Proposal').src=myArray[PageNum - 1];
			document.getElementById(PageNum - 1).className="HighlightAnchor";					
			
			PageNum--;
			
		}	
	else
		window.alert("Cannot move back as  you are already on the first page");		
}

function GoNext()
{
//debugger;
	if(PageNum < myArray.length -1)
		{
			//next=parseInt(PageNum) + 1;
			document.getElementById(PageNum).className="NormalAnchor";
			document.getElementById('Proposal').src=myArray[PageNum + 1];
			document.getElementById(PageNum + 1).className="HighlightAnchor";
			PageNum++;
		}	
	else
		window.alert("Cannot move next as  you are already on the last page");		
}		
//-------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
function SlideShow()
{
	//debugger;		
	
	try
		{									
			if(FastWinId != null)
				window.clearTimeout(FastWinId);
			
			if(SlowWinId != null)
				window.clearTimeout(SlowWinId);
		}
		catch(er)
		{}
		
	if(PageNum < myArray.length - 1) 
	{
		
		document.getElementById('ShowSlide').style.visibility='hidden';
		document.getElementById('HideSlide').style.visibility='visible';
		//----------------------------------------------------------------
		if(PageNum > 0)
		{
			document.getElementById(PageNum -1).className="NormalAnchor";
		}
		document.getElementById('Proposal').src=myArray[PageNum]; // Changed
		document.getElementById(PageNum).className="HighlightAnchor";
		//----------------------------------------------------------------
		WinId=window.setTimeout('SlideShow();',3000);
		PageNum++;
	}			
	else if(PageNum == myArray.length - 1) 
	{	
		//----------------------------------------------------------------			
		if(PageNum > 0)
		{
			document.getElementById(PageNum -1).className="NormalAnchor";
		}
		document.getElementById('Proposal').src=myArray[PageNum];
		//----------------------------------------------------------------
		document.getElementById(PageNum).className="HighlightAnchor";
		document.getElementById('HideSlide').style.visibility='hidden';
		document.getElementById('ShowSlide').style.visibility='visible';				
		try
		{
			if(WinId != null)
				window.clearTimeout(WinId);	
		}
		catch(er)
		{}
	}					
}

//-------------------------------------------------------------------------------
//						STOP-STOP
//-------------------------------------------------------------------------------
function SlideStop()
{
		//debugger;
		try
		{
			if(WinId != null)
				{
					window.clearTimeout(WinId);
					WinId=null;
					}
				
			if(FastWinId != null)
				{
					window.clearTimeout(FastWinId);
					FastWinId=null;
					}
			
			if(SlowWinId != null)
				{
					window.clearTimeout(SlowWinId);
					SlowWinId=null;
					}
		}
		catch(er)
		{}
								
		
		document.getElementById('HideSlide').style.visibility='hidden';
		document.getElementById('ShowSlide').style.visibility='visible';
		if(PageNum > 0)
		{	
			PageNum--; // This is because, next time you start Slide show first you will see the page on where you had stopped.				
		}
}
//-------------------------------------------------------------------------------
//						SLOW-SLOW
//-------------------------------------------------------------------------------
function SlideShowSlow()
{
	//debugger;
	try
	{
		if(WinId != null)
			{
				window.clearTimeout(WinId);
				WinId=null;
				if(PageNum > 0)						
				{PageNum--;}			
			}
			
		if(FastWinId != null)
			{
				window.clearTimeout(FastWinId);
				FastWinId=null;
				if(PageNum > 0)						
				{PageNum--;}
			}
	}
	catch(er)
		{}
	
	if(PageNum < myArray.length -1)
	{
		document.getElementById('ShowSlide').style.visibility='hidden';
		document.getElementById('HideSlide').style.visibility='visible';
		//----------------------------------------------------------------
		if(PageNum > 0)
		{
			document.getElementById(PageNum -1).className="NormalAnchor";
		}
		document.getElementById('Proposal').src=myArray[PageNum];
		document.getElementById(PageNum).className="HighlightAnchor";
		//----------------------------------------------------------------
		PageNum++;
		SlowWinId=window.setTimeout('SlideShowSlow();',8000);
	}
	else if(PageNum == myArray.length - 1) 
	{
		//----------------------------------------------------------------
		if(PageNum > 0)
		{
			document.getElementById(PageNum -1).className="NormalAnchor";
		}
		document.getElementById('Proposal').src=myArray[PageNum];
		document.getElementById(PageNum).className="HighlightAnchor";
		//----------------------------------------------------------------
		
		document.getElementById('HideSlide').style.visibility='hidden';
		document.getElementById('ShowSlide').style.visibility='visible';
		try{
		window.clearTimeout(SlowWinId);
		}
		catch(er)
		{}
	}						
							
}
//-------------------------------------------------------------------------------
//						FAST-FAST
//-------------------------------------------------------------------------------
function SlideShowFast()
{
	//debugger;
	try
	{
		if(WinId != null)
			{
				window.clearTimeout(WinId);	
				WinId=null;
				if(PageNum > 0)						
				{PageNum--;}
			}
		
		if(SlowWinId != null)
			{
				window.clearTimeout(SlowWinId);
				SlowWinId=null;						
				if(PageNum > 0)						
				{PageNum--;}						
			}
	}
	catch(er)
		{}
	
	if(PageNum < myArray.length -1)
	{
		document.getElementById('ShowSlide').style.visibility='hidden';
		document.getElementById('HideSlide').style.visibility='visible';
		
		//----------------------------------------------------------------
		if(PageNum > 0)
		{
			document.getElementById(PageNum -1).className="NormalAnchor";
		}
		document.getElementById('Proposal').src=myArray[PageNum];
		document.getElementById(PageNum).className="HighlightAnchor";
		//----------------------------------------------------------------
		
		PageNum++;
		FastWinId=window.setTimeout('SlideShowFast();',1000);
	}	
	else if(PageNum == myArray.length - 1) 
	{
		//----------------------------------------------------------------
		if(PageNum > 0)
		{
			document.getElementById(PageNum -1).className="NormalAnchor";
		}
		document.getElementById('Proposal').src=myArray[PageNum];
		document.getElementById(PageNum).className="HighlightAnchor";
		//----------------------------------------------------------------
		
		document.getElementById('HideSlide').style.visibility='hidden';
		document.getElementById('ShowSlide').style.visibility='visible';
		try{
		window.clearTimeout(FastWinId);
		}
		catch(er)
		{}
	}		
}
//-------------------------------------------------------------------------------
//						PAGING
//-------------------------------------------------------------------------------		
function ShowPaging()
{
	//window.alert( PageNum );
	//debugger;
	for(i=0;i < myArray.length; i++)
	{
		//----------------------------------------------------
		var anchorNode=document.createElement('a');
		anchorNode.setAttribute('id',i);
		anchorNode.setAttribute('href','#');
		if(anchorNode.addEventListener)
		{
			anchorNode.addEventListener("click",temp,true);
		}
		else if (anchorNode.attachEvent)
		{
			//anchorNode.attachEvent("onclick","alert('hello world');");
			anchorNode.attachEvent("onclick",ChangeImage);
		}
		anchorNode.appendChild(document.createTextNode(i));
		//----------------------------------------------------		
									
		document.getElementById('Paging').appendChild(anchorNode);	
		document.getElementById('Paging').appendChild(document.createTextNode('  '));
	}			
}
function ChangeImage()
{
	//window.alert(objthis.href);
	//debugger;
	//window.alert(event.srcElement.href);			
	pageId=event.srcElement.id;
	PageNum=parseInt(pageId);
	document.getElementById('Proposal').src= myArray[event.srcElement.id];			
}
//-------------------------------------------------------------------------------
//-------------------------------------------------------------------------------

function ShowPaging1()
{
	//window.alert( PageNum );
	//debugger;
	str="";
	for(i=0;i < myArray.length; i++)
	{
		//----------------------------------------------------	
		j=i+1;		
		str +="<a id='" + i + "' href='#' onclick=ChangeImage12('" + i + "')>" + j  + "</a>" 
		str+="  ";
		document.getElementById('Paging').innerHTML=str;
	}			
}

function ChangeImage12(id)
{
	//window.alert(objthis.href);
	//debugger;
	//window.alert(event.srcElement.href);			
	//pageId=event.srcElement.id;
	document.getElementById(PageNum).className="NormalAnchor";
	PageNum=parseInt(id);
	document.getElementById('Proposal').src= myArray[id];	
	document.getElementById(PageNum).className="HighlightAnchor";		
}

function temp1()
{
	window.alert(' World Hello');
}
//-------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
function ShowPageNumber()
{
	window.alert( PageNum );			
}
//-------------------------------------------------------------------------------
	
