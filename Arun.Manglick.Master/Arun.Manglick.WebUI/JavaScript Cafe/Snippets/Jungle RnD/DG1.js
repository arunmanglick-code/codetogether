// This function is developed to apply the formating on the contents stored in a TextBox named 'txtEditor1'.		
		// This part is Required for Second Manually Formatting Part
		
			function insertCustomStyleOnTextBox(numStyle)
			{
				//debugger;			
						
				var text = document.all['_' + 'txtEditor1' + '_editor'];				
				var textobject = text.contentWindow.document.selection.createRange();
				
				if (numStyle==1)				
					textobject.execCommand("BOLD");
				
				if (numStyle==2)				
					textobject.execCommand("ITALIC");
					
				if (numStyle==3)				
					textobject.execCommand("JustifyFull");
					
				if (numStyle==4)
				{
					var lnk="htto://www.yahoo.com";
					textobject.execCommand("CreateLink",false,lnk);
				}	
				
				if (numStyle==5)
					textobject.execCommand("InsertOrderedList");
				
				if (numStyle==6)
					textobject.execCommand("RemoveFormat");
			}	
---------------------------------------------------------------
// This function is developed to apply the formating on the contents stored in a FrameSet named 'ifrmtextframe'		
		// This part is Required for only Third Manually Formatting Part
		
			function insertCustomStyleOnFRAME(numStyle)
			{
				//debugger;			
								
				var textobject = document.ifrmtextframe.document.selection.createRange();
				
				if (numStyle==1)				
					textobject.execCommand("BOLD");
				
				if (numStyle==2)				
					textobject.execCommand("ITALIC");
					
				if (numStyle==3)				
					textobject.execCommand("JustifyFull");
					
				if (numStyle==4)
				{
					var lnk="htto://www.yahoo.com";
					textobject.execCommand("CreateLink",false,lnk);
				}	
				
				if (numStyle==5)
					textobject.execCommand("InsertOrderedList");
				
				if (numStyle==6)
					textobject.execCommand("RemoveFormat");
			}	
---------------------------------------------------------------
function ValidateID()
			{
				//debugger;
				pattern="E[0-9]{4}$";
				str=document.getElementById("txtID").value;
				res=str.match(pattern);
				if (res==null)
				{
					window.alert("Please enter Valid ID as ( E####)");
					document.getElementById("txtID").value='';
					document.getElementById("txtID").focus();				
				}
			}
			
function CapitalizeName()
			{
				//debugger;
				//1
				document.getElementById("txtname").value=document.getElementById("txtname").value.toUpperCase();
				
				//2
				/*
				str=document.getElementById("txtname").value;
				str=str.toUpperCase();
				document.getElementById("txtname").value=str; */
			}
			//--------------------------------------------------------------------------
			function CapitalizeAddr()
			{
				//1
				document.getElementById("txtAddress").value=document.getElementById("txtAddress").value.toUpperCase();
				
				//2
				/*
				str=document.getElementById("txtname").value;
				str=str.toUpperCase();
				document.getElementById("txtname").value=str; */
			}
			//--------------------------------------------------------------------------
			//For Checking Numeric Data Type two approaches have been given.
			//Both are working.
			function CheckIntDataType()
			{
				//debugger;
				str=document.getElementById("txtAge").value;
				if (isNaN(str))
						{
							window.alert("Please enter Numeric Value only");
							document.getElementById("txtAge").value='';
							document.getElementById("txtAge").focus();
							
						}	
			}
			
			function CheckIntDataType1()
			{
				//debugger;
				pattern="^[0-9]{1,2}$";
				str=document.getElementById("txtAge").value;
				res=str.match(pattern);
				if (res==null)
				{
					window.alert("Please enter Numeric Value and less than 100");
					document.getElementById("txtAge").value='';
					document.getElementById("txtAge").focus();				
				}
			}
			//--------------------------------------------------------------------------
			function setfocus(id)
			{
				debugger;
				obj=document.getElementById("txtDOB");
				obj.focus();
			}
			function validDate()
			{
				//debugger;
				strdate=document.getElementById("txtDOB").value;
				res=isDate(strdate);  // Stored in Common.js file
				
				if (res==true)
					return true;
				else
				 {
				 
				 document.getElementById("txtDOB").focus();
				 }				
			}
			
			
			//--------------------------------------------------------------------------
			
			function DisplayMalePanel()
			{
				 //debugger;
				 
				//if(document.getElementById("rdBtnMale").checked == true)  
				if (document.forms[0].rdBtnMale.checked == true)    // Both Will do
				{
					// Both will do.
					
					//1
					//objMPanel=document.getElementsByTagName("div");
					//objMPanel["pnlMale"].style.visibility = 'visible';
					
					//2
					document.getElementById("pnlMale").style.visibility='visible';
					
					//objFPanel=document.getElementsByTagName('div');
					//objFPanel['pnlFemale'].style.visibility = 'hidden';
					document.getElementById("pnlFemale").style.visibility='hidden';
				}
				else
				{
					objMPanel=document.getElementsByTagName('div');
					objMPanel['pnlMale'].style.visibility = 'hidden';
					
					objFPanel=document.getElementsByTagName('div');
					objFPanel['pnlFemale'].style.visibility = 'visible';
				}				
			}
			//--------------------------------------------------------------------------
			function DisplayFemalePanel()
			{
				// debugger;
				if(document.getElementById("rdBtnFemale").checked == true)
				{
					objFPanel=document.getElementsByTagName("div");
					objFPanel["pnlFemale"].style.visibility='visible';
					
					objMPanel=document.getElementsByTagName("div")
					objMPanel["pnlMale"].style.visibility='hidden';						
					
				}
				else
				{
					objFPanel=document.getElementsByTagName("div");
					objFPanel["pnlFemale"].style.visibility='hidden';
					
					objMPanel=document.getElementsByTagName("div")
					objMPanel["pnlMale"].style.visibility='visible';			
				}
				
			}		
			//--------------------------------------------------------------------------
			function CheckAllMalePoints()
			{
				 //debugger;
				
				if (document.getElementById("chkMSelectAll").checked==true)
				{
					document.getElementById("chkMPoint1").checked=true;
					document.getElementById("chkMPoint2").checked=true;
					document.getElementById("chkMPoint3").checked=true;
					document.getElementById("txtMTotal").value=3;
				}
				else
				{
					document.getElementById("chkMPoint1").checked=false;
					document.getElementById("chkMPoint2").checked=false;
					document.getElementById("chkMPoint3").checked=false;
					document.getElementById("txtMTotal").value=0;
				}
				
			}
			//--------------------------------------------------------------------------
			function CheckAllFemalePoints()
			{
				// debugger;
				
				if (document.getElementById("chkFSelectAll").checked==true)
				{
					document.getElementById("chkFPoint1").checked=true;
					document.getElementById("chkFPoint2").checked=true;
					document.getElementById("chkFPoint3").checked=true;
					document.getElementById("txtFTotal").value=3;
					
				}
				else
				{
					document.getElementById("chkFPoint1").checked=false;
					document.getElementById("chkFPoint2").checked=false;
					document.getElementById("chkFPoint3").checked=false;
					document.getElementById("txtFTotal").value=0;
				}
			}
			//--------------------------------------------------------------------------	
				function AdjustMalePointOne()
				{
					//debugger;
					if (document.getElementById("chkMPoint1").checked==true)
					{
						document.getElementById("txtMTotal").value++ ;
						if (document.getElementById("chkMPoint1").checked==true && document.getElementById("chkMPoint2").checked==true && document.getElementById("chkMPoint3").checked==true)
						{
						document.getElementById("chkMSelectAll").checked=true;
						}					
					}
					else if (document.getElementById("chkMPoint1").checked==false)
					{
						if (document.getElementById("chkMSelectAll").checked==true)
						 {
							document.getElementById("chkMSelectAll").checked=false;							
						 }						
						 document.getElementById("txtMTotal").value--;
					}
				}
			//--------------------------------------------------------------------------
			function AdjustMalePointTwo()
				{
					//debugger;
					if (document.getElementById("chkMPoint2").checked==true)
					{
						document.getElementById("txtMTotal").value++ ;
						if (document.getElementById("chkMPoint1").checked==true && document.getElementById("chkMPoint2").checked==true && document.getElementById("chkMPoint3").checked==true)
						{
						document.getElementById("chkMSelectAll").checked=true;
						}					
					}
					else if (document.getElementById("chkMPoint2").checked==false)
					{
						if (document.getElementById("chkMSelectAll").checked==true)
						 {
							document.getElementById("chkMSelectAll").checked=false;							
						 }				
						 document.getElementById("txtMTotal").value--;		
					}
				}
			//--------------------------------------------------------------------------
		
		function AdjustMalePointThree()
				{
					//debugger;
					if (document.getElementById("chkMPoint3").checked==true)
					{
						document.getElementById("txtMTotal").value++ ;
						if (document.getElementById("chkMPoint1").checked==true && document.getElementById("chkMPoint2").checked==true && document.getElementById("chkMPoint3").checked==true)
						{
						document.getElementById("chkMSelectAll").checked=true;
						}					
					}
					else if (document.getElementById("chkMPoint3").checked==false)
					{
						if (document.getElementById("chkMSelectAll").checked==true)
						 {
							document.getElementById("chkMSelectAll").checked=false;						
						 }						
						 document.getElementById("txtMTotal").value--;
					}
				}
			//--------------------------------------------------------------------------
		
				function AdjustFemalePointOne()
				{
					//debugger;
					if (document.getElementById("chkFPoint1").checked==true)
					{
						document.getElementById("txtFTotal").value++ ;
						if (document.getElementById("chkFPoint1").checked==true && document.getElementById("chkFPoint2").checked==true && document.getElementById("chkFPoint3").checked==true)
						{
						document.getElementById("chkFSelectAll").checked=true;
						}					
					}
					else if (document.getElementById("chkFPoint1").checked==false)
					{
						if (document.getElementById("chkFSelectAll").checked==true)
						 {
							document.getElementById("chkFSelectAll").checked=false;							
						 }						
						 document.getElementById("txtFTotal").value--;
					}
				}
			//--------------------------------------------------------------------------
				function AdjustFemalePointTwo()
				{
					//debugger;
					if (document.getElementById("chkFPoint2").checked==true)
					{
						document.getElementById("txtFTotal").value++ ;
						if (document.getElementById("chkFPoint1").checked==true && document.getElementById("chkFPoint2").checked==true && document.getElementById("chkFPoint3").checked==true)
						{
						document.getElementById("chkFSelectAll").checked=true;
						}					
					}
					else if (document.getElementById("chkFPoint2").checked==false)
					{
						if (document.getElementById("chkFSelectAll").checked==true)
						 {
							document.getElementById("chkFSelectAll").checked=false;							
						 }				
						 document.getElementById("txtFTotal").value--;		
					}
				}
			//--------------------------------------------------------------------------
		
				function AdjustFemalePointThree()
				{
					//debugger;
					if (document.getElementById("chkFPoint3").checked==true)
					{
						document.getElementById("txtFTotal").value++ ;
						if (document.getElementById("chkFPoint1").checked==true && document.getElementById("chkFPoint2").checked==true && document.getElementById("chkFPoint3").checked==true)
						{
						document.getElementById("chkFSelectAll").checked=true;
						}					
					}
					else if (document.getElementById("chkFPoint3").checked==false)
					{
						if (document.getElementById("chkFSelectAll").checked==true)
						 {
							document.getElementById("chkFSelectAll").checked=false;						
						 }						
						 document.getElementById("txtFTotal").value--;
					}
				}
			//--------------------------------------------------------------------------
			function showColor()
			{
				idx=document.getElementById("drpdwnColorList").selectedIndex;
				if (idx==0)
				 {
					window.alert("Please Select some Color");
				}
				else
				{
				color=document.getElementById("drpdwnColorList").options[idx].value;
				
				if (color=="Red")
					{
					//1 
					//lblChosenColor.innerHTML="Red";
					
					//2
					document.getElementById("lblChosenColor").innerHTML="Red";
					
					
					document.getElementById("lblChosenColor").style.color="Red";
					document.getElementById("ColorColumn").style.backgroundColor="Red";
					}
				else if (color=="Blue")
					{
					lblChosenColor.innerHTML="Blue";
					document.getElementById("lblChosenColor").style.color="Blue";
					document.getElementById("ColorColumn").style.backgroundColor="Blue";
					}
				else if (color=="Green")
					{
					lblChosenColor.innerHTML="Green";
					document.getElementById("lblChosenColor").style.color="Green";
					document.getElementById("ColorColumn").style.backgroundColor="Green";
					}
				}
				
			}
			//--------------------------------------------------------------------------
			function ShowWinCalc()
			{
				//debugger;
				var WshShell = new ActiveXObject("WScript.Shell");
				alert("Lauching Calculator");
				WshShell.Run("calc"); // winword, excel,pbrush
				WshShell.AppActivate("Calculator");
				
			}
			//--------------------------------------------------------------------------
				function DeleteAll()
				{
				   //debugger;
				  var len=document.forms[0].elements.length;
				  
				  for(i=0; i < len; i++)
				  {
					var obj=document.forms[0].elements[i];
					
					if (obj.type=='checkbox')
					{
						str=obj.id;
						if (str.indexOf('DGCustomer__ctl') != -1)
						{
							obj.checked=document.Form1.DGCustomer__ctl1_chkdA.checked;
						}
					}				  
				  } // End for
				
				}		
				function DeleteAllS()
				{
					//debugger;
					var name="DGCustomer__ctl";
					var x=0;
					var i=2;
					var cons =1;
					do
					{
						oElement = document.getElementById(name + i + "_chkd");
						if (oElement == null)
							{x=1;}
						else
						{oElement.checked=document.getElementById(name + cons + "_chkdA").checked;}
						
						i++;
					}while(x<1);
				}
			//--------------------------------------------------------------------------	
			function ConfirmDelete()
			{
			//debugger;
				res=window.confirm("Are u Sure to Delete ?");
				return res;
			}
---------------------------------------------------------------

---------------------------------------------------------------

---------------------------------------------------------------