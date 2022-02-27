//---------------------------------------------------------------------------
//	IframeSectionDivider.aspx
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
		idx=document.getElementById('ddlSectionDividerCount').selectedIndex;
		increase=document.getElementById('ddlSectionDividerCount').options[idx].value;
		cnt=parseInt(cnt) + parseInt(increase);
		obj.innerHTML=cnt;
		
	}
	else
	{
		obj=parent.document.getElementById('Header1_lblPageCount');
		cnt=obj.innerHTML;
		idx=document.getElementById('ddlSectionDividerCount').selectedIndex;
		decrease=document.getElementById('ddlSectionDividerCount').options[idx].value;
		cnt=parseInt(cnt) - parseInt(decrease);
		obj.innerHTML=cnt;
		
	}			
}	
//---------------------------------------------------------------------------
//	gather_sectiondividers.ascx
//---------------------------------------------------------------------------
function UpdateSectionIFrame(objUrl)
	{
		//debugger;
		var myRegex=/#/g;
		objUrl=objUrl.replace(myRegex," ");		
		document.getElementById('ifrmGatherSectionframe').src=objUrl;
	}

function FolderClicked(objUrl)
{
	document.getElementById('ifrmGatherSectionframe').src=objUrl;
}
//---------------------------------------------------------------------------
function ShowPopup(objStrPopup)
			{
				//debugger;
				parent.document.getElementById('Header1_pnlProposalPopup1').innerHTML=objStrPopup;
			}	

//---------------------------------------------------------------------------
