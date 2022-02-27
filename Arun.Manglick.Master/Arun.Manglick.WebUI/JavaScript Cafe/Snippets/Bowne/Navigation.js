//---------------------------------------------------------------------------
//	NavHeader.ascx
//---------------------------------------------------------------------------

function OverImage(objThis)
{
	//debugger;
	switch(objThis.id)
	{
		case "NavHeader1_imgbtnGather":
			objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum1_over.jpg';
			break;
		case "NavHeader1_imgbtnTemplate":
			objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum2_over.jpg';
			break;
		case "NavHeader1_imgbtnDividers":
			objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum3_over.jpg';
			break;
		case "NavHeader1_imgbtnSort":
			objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum4_over.jpg';
			break;
		case "NavHeader1_imgbtnCustomize":
			objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum5_over.jpg';
			break;
		case "NavHeader1_imgbtnSave":
			objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum6_over.jpg';
			break;	
	}		
}

function OffImage(objThis)
{
	//debugger;
	switch(objThis.id)
	{
		case "NavHeader1_imgbtnGather":
			if(document.forms[0].id=='gather_frames' || document.forms[0].id=='search')	
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum1_on.jpg';
			else
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum1_off.jpg';
			break;
		case "NavHeader1_imgbtnTemplate":
			if(document.forms[0].id=='frmTemplate')	
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum2_on.jpg';
			else
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum2_off.jpg';
			break;
		case "NavHeader1_imgbtnDividers": 
			if(document.forms[0].id=='gather_sectiondividers')	
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum3_on.jpg';
			else
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum3_off.jpg';
			break;
		case "NavHeader1_imgbtnSort":
			if(document.forms[0].id=='frmSort')	
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum4_on.jpg';
			else
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum4_off.jpg';
			break;
		case "NavHeader1_imgbtnCustomize": 
			if(document.forms[0].id=='frmTemplateCustomize')	
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum5_on.jpg';
			else
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum5_off.jpg';
			break;
		case "NavHeader1_imgbtnSave":
			if(document.forms[0].id=='frmSave')	
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum6_on.jpg';
			else
				objThis.src='/ProposingBowneLive/ProposingBowne/_images/newnum6_off.jpg';
			break;	
	}		
}
