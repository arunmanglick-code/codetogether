//---------------------------------------------------------------------------
//	fm_publicfolder.ascx
//---------------------------------------------------------------------------

function CheckOne(objChk)
{
	if (objChk.checked == false)
	{
		total -= 1;
	}
	
	var max = document.getElementsByTagName('input').length; // form.checkboxes.length;
	//alert(max);
	//alert(objChk.type);
	//alert(document.getElementsByTagName('input').item(3).type);
	if (!max)
	{
	}
	else
	{
		for (var idx = 0; idx < max; idx++)
		{
			var chk = document.getElementsByTagName('input').item(idx);
			
			if (chk.type == "checkbox")
			{
				if (chk.checked == true) //document.form.checkboxes[idx].checked == true)
				{
					total += 1;
					
					if (total > 1)
					{
						alert('Select only 1 file.');
						objChk.checked = false;
						total -= 1;
						
						return;
					}
				}
			}
		}
	}				
}
//---------------------------------------------------------------------------
//---------------------------------------------------------------------------
