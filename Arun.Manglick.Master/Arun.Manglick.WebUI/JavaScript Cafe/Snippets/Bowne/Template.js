//---------------------------------------------------------------------------
//	template.aspx
//---------------------------------------------------------------------------

function MutualSelection(objThis)
		{
				//debugger;
				var name="dtlstTemplate__ctl";
				var xit=0;
				var i=0;
				var cons =1;
				do
				{
					oElement = document.getElementById(name + i + "_rdbtnSelectTemplate");
					if (oElement == null)
						{xit=1;}
					else
					{oElement.checked=false;}
					
					i++;
				}while(xit<1);
				objThis.checked=true;
				
				// Update Template Count in Header. 
				//debugger;
				if (objThis.parentElement.innerText=='Select(0 KB)')
					window.document.getElementById('Header1_lblTemplate').innerHTML=0;
				else
					window.document.getElementById('Header1_lblTemplate').innerHTML=1;
				
		}
//---------------------------------------------------------------------------
function ShowPopup(objStrPopup)
			{
				//debugger;
				parent.document.getElementById('Header1_pnlProposalPopup1').innerHTML=objStrPopup;
			}
//---------------------------------------------------------------------------
