	function HideSideColumn()
			{
				//debugger;
				//document.all('SideDiv').style.visibility='visible';
				document.getElementById('SideDiv').style.display ='none';
				document.all('ifrmcontentframe').src='../logout.htm';
				document.all('iframeColumn').height='150';
				
			}
			
	function Preview()
	{
		window.open("/ProposingBowneLive/ProposingBowne/proposals/preview.aspx");
	
	}
	
	function ShowPopup(objStrPopup)
	{
		//debugger;
		document.getElementById('Header1_pnlProposalPopup1').innerHTML=objStrPopup;
	}
	
	function ShowItemsList()
	{		
		//debugger;		
		try
		{		
		document.getElementById('Header1_pnlProposalPopup1').style.display='block';
		//parent.frames[0].document.getElementById('pnlProposalPopup').style.display='block';
		
		}
		catch(ex)
		{
			return false;
		}
	}
	
	function HideItemsList()
	{		
		//debugger;	
		try
		{				
		document.getElementById('Header1_pnlProposalPopup1').style.display='none';
		//parent.frames[0].document.getElementById('pnlProposalPopup').style.display='none';
		}
		catch(ex)
		{
			return false;
		}
	}
