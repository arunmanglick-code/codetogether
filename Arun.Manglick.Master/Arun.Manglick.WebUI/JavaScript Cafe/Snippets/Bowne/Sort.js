//---------------------------------------------------------------------------
//	Sort.aspx
//---------------------------------------------------------------------------

function DelSel()
{
	selObj = document.getElementById("lstProposal");

	var i = document.getElementById("lstProposal").selectedIndex;
	
	if (i < 0)
	{ 
		return;
	}
	
	i++;
	for (; selObj.options[i]; ++i)
	{
		selObj.options[i - 1] = new Option(selObj.options[i].text, selObj.options[i].value);
	}
	selObj.options[i - 1] = null;
	showSerially();	
	ShowBlankImageBelowListBox();
	
	//Decrease the Proposal Count on Header Control
	document.getElementById('Header1_lblPageCount').innerText=parseInt(document.getElementById('Header1_lblPageCount').innerText) - 1;
			
}
//--------------------------------------------------------------------------

function showSerially()
{
	//debugger;
	msg="";				
	for(i=0; i< selObj.options.length; i++)
	{
			name=selObj.options[i].text;
			value=selObj.options[i].value;
			
			// Added by AM to handle seperation of Guid and Relative path
			idx=value.indexOf("|");
			value=value.substring(0,idx);
			//-------------------------------------------------------------
			msg +=name + "=" + value + "*";						
	}
	if(msg.length > 0)
	{
		msg=msg.substring(0,msg.length-1);
		document.getElementById('txtHidden').value=msg;					
	}
	else
	{
		document.getElementById('txtHidden').value='EMPTY';  // No more propsals remain in the sort list.
	}
}
//--------------------------------------------------------------------------
function UpdatePopupList()
			{		
					//debugger;	
					selObj = document.getElementById("lstProposal");		
					strPopup="<table cellSpacing='0' cellPadding='0' width='150' border='1'>";
					//--------------------------------------------------------------------------
					// For Proposals
					strPopup += "<tr>";
					strPopup += "<td class='ProposalPopupHeaderRow'>Proposals</td>";
					strPopup += "</tr>";
					for(i=0; i< selObj.options.length; i++)
					{
						value=selObj.options[i].value;
						if (value.indexOf("Template") == -1)
						{
						strPopup +="<tr>";					
						strPopup +="<td class='ProposalPopupInnerRow'> <img src='/ProposingBowneLive/ProposingBowne/_images/arrow_mini.gif'>&nbsp;&nbsp;" + selObj.options[i].text + "</td>";
						strPopup +="</tr>";
						}
					}
					//--------------------------------------------------------------------------
					// For Section Dividers
					strPopup += "<tr>";
					strPopup += "<td class='ProposalPopupHeaderRow'>Section Dividers</td>";
					strPopup += "</tr>";
					for(i=0; i< selObj.options.length; i++)
					{
						value=selObj.options[i].value;
						if (value.indexOf("Template") != -1)
						{
						strPopup +="<tr>";					
						strPopup +="<td class='ProposalPopupInnerRow'> <img src='/ProposingBowneLive/ProposingBowne/_images/arrow_mini.gif'>&nbsp;&nbsp;" + selObj.options[i].text + "</td>";
						strPopup +="</tr>";
						}
					}
					//--------------------------------------------------------------------------
					strPopup += "</table>";
					document.getElementById('Header1_pnlProposalPopup1').innerHTML=strPopup;
			
			}
//--------------------------------------------------------------------------			
function makeInvisible()
{
	// This is used to Hide the TextBox named 'txtHidden'
	document.getElementById('txtHidden').style.display ='none';
	ShowFirstImageBelowListBox();
}
function moveSel(dnDir) 
{
	var srcText = srcValue = destText = destValue = "";
	var srcIndex = destIndex = 0;
	
	selObj = document.getElementById("lstProposal");
	srcIndex = selObj.selectedIndex;          

	if (srcIndex < 0)
	{
		return;
	}
	
	switch (dnDir)
	{
		case "t":
			if (srcIndex == 0)
			{
				return;
			}
			destIndex = 0;
			break;
		case "b":
			if (srcIndex == selObj.length)
			{
				return;
			}
			destIndex = selObj.length - 1;
		break;
		case true:
			increment = 1
			if (srcIndex + 1 == selObj.length)
			{
				return;
			}
			destIndex = srcIndex + increment;
			break;
		case false:
			increment = -1
			if (srcIndex < 1)
			{
				return;
			}
			destIndex = srcIndex + increment;
			break;
	}

	with (selObj)
	{
		srcText  = options[srcIndex].text;
		srcValue = options[srcIndex].value;

		destText  = options[destIndex].text;
		destValue = options[destIndex].value;

		if (dnDir == 'b')
		{
			var i = selObj.selectedIndex;
			i++;
			for (; selObj.options[i]; ++i)
			{
				selObj.options[i - 1] = new Option(selObj.options[i].text, selObj.options[i].value);
			}
			
			options[destIndex].text = srcText;
			options[destIndex].value = srcValue;
		}
		else if (dnDir == 't')
		{
			var i = selObj.selectedIndex;
			for (; i > 0; --i)
			{
				selObj.options[i] = new Option(selObj.options[i - 1].text, selObj.options[i - 1].value);
			}
			
			options[destIndex].text = srcText;
			options[destIndex].value = srcValue;
		}
		else
		{
			options[srcIndex].text = destText;
			options[srcIndex].value = destValue;
		
			options[destIndex].text = srcText;
			options[destIndex].value = srcValue;
		}
		
		selectedIndex = destIndex;
	}	
	showSerially();	// AM :- 23-Jun05 				
}
//--------------------------------------------------------------------------
function ShowProposalViewer()
	{
	//debugger;
		page=1;
		len=document.getElementById('lstProposal').options.length;
		window.open('/ProposingBowneLive/ProposingBowne/proposals/proposal_viewer.aspx?TotalProposals='+ len + '','ProposalViewer','height=500,width=500,top=134,left=312');
	}	
//--------------------------------------------------------------------------	
function ShowFirstImageBelowListBox()
{
	//debugger;
	// This is used to show the image of the very first proposal(if any) present in the list, when
	// the user arrives to the sort page.
	if(document.getElementById('lstProposal').options.length > 0)
	{
		idx=document.getElementById('lstProposal').selectedIndex;
		name=document.getElementById('lstProposal').options[0].text;
		val=document.getElementById('lstProposal').options[0].value;
		
		// Added by AM to handle seperation of Guid and Relative path
		idx=val.indexOf("|");
		idx=parseInt(idx) + 1;
		val=val.substring(idx);
		//------------------------------------------------------------
		val=val.substring(0,val.indexOf(name,0)) + '_images/';
		
		name=name.substring(0,name.indexOf('.',0)) + '_01.jpg';
		fullimagename=val+name;
		
		document.getElementById('listImage').src=fullimagename;				
					
	}
}
//--------------------------------------------------------------------------
function ShowImageBelowListBox()
{
	//debugger;
	idx=document.getElementById('lstProposal').selectedIndex;
	name=document.getElementById('lstProposal').options[idx].text;
	val=document.getElementById('lstProposal').options[idx].value;
	
	// Added by AM to handle seperation of Guid and Relative path
	idx=val.indexOf("|");
	idx=parseInt(idx) + 1;
	val=val.substring(idx);
	//------------------------------------------------------------
	
	val=val.substring(0,val.indexOf(name,0)) + '_images/';
	
	name=name.substring(0,name.indexOf('.',0)) + '_01.jpg';
	fullimagename=val+name;
	
	document.getElementById('listImage').src=fullimagename;				
	
	//"/ProposingBowneLive/Section Folder/Section1.doc"
	//"/ProposingBowneLive/Section Folder/_images/Section1_01.jpg"				
}
//--------------------------------------------------------------------------
function ShowBlankImageBelowListBox()
{
	//debugger;
	// This is used to show the default image, when there are no more propsal remain in the list box, after
	// deletion using Cross button.
	//document.getElementById('lstProposal').focus();
	if(document.getElementById('lstProposal').options.length == 0)
	{						
		document.getElementById('listImage').src='/ProposingBowneLive/ProposingBowne/_images/template_placeholder.jpg';									
	}
	else
	{
		// Show the image of the very first proposal present in the list
		ShowFirstImageBelowListBox();				
	}
}
//-------------------------------------------------------------------------------