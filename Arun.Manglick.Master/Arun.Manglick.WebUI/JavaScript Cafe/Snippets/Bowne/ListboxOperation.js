//---------------------------------------------------------------------------
//	email_alerts.aspx
//---------------------------------------------------------------------------

function MoveFWD()
			{				
				//debugger;
				idx=document.getElementById('lstUnSelCat').selectedIndex;
				if (idx == -1 )
					{
						window.alert('Please Select Some Item from the left list');
						document.getElementById('lstUnSelCat').focus();
					}
				else
				{
					val=document.getElementById('lstUnSelCat').options[idx].value;
					txt=document.getElementById('lstUnSelCat').options[idx].text;
					
					objRight=document.getElementById('lstSelCat');		
					objRight.options[objRight.options.length]=new Option(val,txt);	
					objRight.selectedIndex=objRight.options.length -1;
					
					// Remove Selected Entry
					RemoveSelectedEntry('lstUnSelCat');
				}
			}
			
			function MoveBCK()
			{
				//debugger;
				idx=document.getElementById('lstSelCat').selectedIndex;
				if (idx == -1 )
				{
					window.alert('Please Select Some Item from the left list');
					document.getElementById('lstSelCat').focus();
				}
				else
				{
					val=document.getElementById('lstSelCat').options[idx].value;
					txt=document.getElementById('lstSelCat').options[idx].text;
					
					objRight=document.getElementById('lstUnSelCat');										
					objRight.options[objRight.options.length]=new Option(val,txt);	
					objRight.selectedIndex=objRight.options.length -1;
					
					// Remove Selected Entry
					RemoveSelectedEntry('lstSelCat');
				}
			}
		
		function RemoveSelectedEntry(name)
		{
				//debugger;
				objLeft=document.getElementById(name);
				Textarr=new Array(objLeft.options.length -1);
				Valuearr=new Array(objLeft.options.length -1);
				for(i=0; i < objLeft.options.length; i++)
				{
					if ( i != idx)
					{
						if (i > idx)
						{
							Textarr[i-1]=objLeft.options[i].text;
							Valuearr[i-1]=objLeft.options[i].value;
						}
						else
						{
							Textarr[i]=objLeft.options[i].text;
							Valuearr[i]=objLeft.options[i].value;
						}
					}
				}
				
				objLeft.options.length=Textarr.length;
				for(i=0; i< Textarr.length; i++)
				{
						objLeft.options[i].text=Textarr[i];
						objLeft.options[i].value=Valuearr[i];					
				}
		}
