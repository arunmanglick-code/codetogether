//---------------------------------------------------------------------------
//	gather_frames.aspx
//---------------------------------------------------------------------------

function HideSideColumn()
	{
		//debugger;
		//document.all('SideDiv').style.visibility='visible';
		//document.getElementById('SideDiv').style.display ='none';
		//document.all('ifrmcontentframe').src='../logout.htm';
		//document.all('iframeColumn').height='150';
		window.alert('hello');	
	}
	
//---------------------------------------------------------------------------
//	Iframe.aspx
//---------------------------------------------------------------------------
function HideResource()
	{
		// Hides Resource & ResourceImage Div and Dispaly Proposal Div
		//debugger;
		document.getElementById('ProposalDiv').style.display='block';
		document.getElementById('ResourceDiv').style.display='none';
		document.getElementById('ResourceImageDiv').style.display='none';
		
	}
			
function ShowResource(objResourcePath)
	{
		//debugger;
		document.getElementById('ProposalDiv').style.display='none';
		document.getElementById('ResourceDiv').style.display='block';
		document.getElementById('ResourceImageDiv').style.display='none';
		
		document.getElementById('idResource').src=objResourcePath;
	}

function ShowImageResource(objImageResourcePath)
{
	//debugger;
	document.getElementById('ProposalDiv').style.display='none';
	document.getElementById('ResourceDiv').style.display='none';
	document.getElementById('ResourceImageDiv').style.display='block';
	
	document.getElementById('idImageResource').src=objImageResourcePath;
}

function ShowResourceAgain()
{
	//debugger;
	document.getElementById('ProposalDiv').style.display='none';
	document.getElementById('ResourceDiv').style.display='block';
	document.getElementById('ResourceImageDiv').style.display='none';
	
	//document.getElementById('idResource').src=objResourcePath;
}


function UpdatePageCount()
{
	//debugger;
	if(document.getElementById('chkSelect').checked)
	{
		obj=parent.document.getElementById('Header1_lblPageCount');
		cnt=obj.innerHTML;
		cnt++;
		obj.innerHTML=cnt;
	}
	else
	{
		obj=parent.document.getElementById('Header1_lblPageCount');
		cnt=obj.innerHTML;
		cnt--;
		obj.innerHTML=cnt;
	}			
}

function ShowPopup(objStrPopup)
{
	//debugger;
	parent.document.getElementById('Header1_pnlProposalPopup1').innerHTML=objStrPopup;
}
//---------------------------------------------------------------------------
//	gather_files.ascx
//---------------------------------------------------------------------------

function dummy(objUrl)
	{
		//debugger;
		var myRegex=/#/g;
		objUrl=objUrl.replace(myRegex," ");		
		document.getElementById('ifrmGatherframe').src=objUrl;
	}
	
	function FolderClicked(objUrl)
	{
		document.getElementById('ifrmGatherframe').src=objUrl;
	}
	
	function ZeroPageCount()
	{
		
			obj=parent.document.getElementById('Header1_lblPageCount');					
			obj.innerHTML=0;
			
	}
//---------------------------------------------------------------------------
//	
//---------------------------------------------------------------------------

