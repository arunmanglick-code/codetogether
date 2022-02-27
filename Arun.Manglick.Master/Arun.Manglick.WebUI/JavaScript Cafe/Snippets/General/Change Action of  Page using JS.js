<script language="javascript">
			function attachSubmit()
			 {
				//debugger;
				try
				{
				 frm=parent.frames["ifrm_fileframe"].document.forms[0];
				 if (frm != null)
					  if (frm.attributes.getNamedItem("showpopup").value=="true")	
						{				      				    
							str=parent.window.location.href;
							if (str.indexOf('/cui/') == -1)
							 {
							    //frm.attributes.getNamedItem("action").value="../../cui/FramedataFiles/DataAppPages/EventDetails.aspx?HTMLFlag=Yes"  // Commented for www.cui.edu				    									    
							    frm.attributes.getNamedItem("action").value="../../FramedataFiles/DataAppPages/EventDetails.aspx?HTMLFlag=Yes"				    				
							 }  
							 else
							 {
							  frm.attributes.getNamedItem("action").value="../../cui/FramedataFiles/DataAppPages/EventDetails.aspx?HTMLFlag=Yes"				    				
							  }
					    }

				}
				catch(er)
				{						
				 return false;
				}
			 }
</script>