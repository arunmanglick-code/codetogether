		function callCalender(e)
		{
			
			var cal1 = new calendar2(document.forms['frmTender'].elements[e]);
			cal1.year_scroll = true;
			cal1.time_comp = false;
			cal1.popup();
		}
		function OnlyRate()
		{
			
			var ratevalue='';
			ratevalue = document.frmTender.elements(event.srcElement.name).value;
			if (onlyRate(ratevalue) == true)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		function ValidateTender()
		{
			
			var bln = false
			document.getElementById('lblErrorMessage').innerText='';
			var name="dgTender__ctl";
			var x=0;
			var curStatus;
			var SelStatus;
			var bonesel = false
			var willcall
			if (document.all["dgTender"] !=null)
			{
				var rowItems = eval(dgTender).rows;
				var rowCount = rowItems.length;
				
				for (i=2; i< rowCount-1; i++)
				{
				//Get Attribute value
					bln = false
					oElement = document.getElementById(name + i + "_chks");
					if (oElement.checked == true)
					{
					bonesel=true;
					oElement = document.getElementById(name + i + "_txtRate");
					if (oElement != null)
					{
						curStatus = oElement.getAttribute('tag')
						willcall  = oElement.getAttribute('WillCall')
						//Get Status value
						//ddlStatus
						oElement = document.getElementById(name + i + "_ddlStatus");
						if (oElement != null)
						{
							
							if (oElement.value != 0)
							{
								SelStatus = oElement.value;
							// Check for Anticipated Date > Req Delivery Date
								var reqdate
								oElement = document.getElementById(name + i + "_txtAnticipatedDlvDate");
								reqdate = oElement.getAttribute('tag')
								if (Trim(oElement.value) == "")
								{
									document.getElementById("lblErrorMessage").innerText="Anticipated Delivery Date cannot be blank.";
									oElement.focus();
									return false;				
								}
								if (isDate(Trim(oElement.value),"MM/dd/yyyy") == false)
								{
									document.getElementById("lblErrorMessage").innerText="Invalid Date Format.";
									//document.getElementById('lblErrorMessage').focus();
									oElement.focus();
									return false;				
								}	
								//Check for Pickup Date
									oElement1 = document.getElementById(name + i + "_txtPickupDate");
									if (oElement1 != null)
									{
										if (Trim(oElement1.value) != "")
										{
											if (isDate(Trim(oElement1.value),"MM/dd/yyyy") == false)
											{
												document.getElementById("lblErrorMessage").innerText="Invalid Date Format.";
												oElement1.focus();
												return false;				
											}
										}	
									}
									
								//End
								
								if (compareDates(Trim(oElement1.value),'MM/dd/yyyy',oElement.value,'MM/dd/yyyy') == 1)
								{
										document.getElementById('lblErrorMessage').innerText="Anticipated Delivery Date should be greater or equal to Pickup Date.";
										document.getElementById(name + i + "_txtAnticipatedDlvDate").focus();
										return false;
								}
							//Check for Status
								//SelStatus,curStatus
								switch (curStatus)
								{
									case 'O':	//Open
									{
									 if (SelStatus == 'T' || SelStatus == 'C') 
									 {
										bln=true;
										document.getElementById('lblErrorMessage').innerText="Status Cannot Change from Open to Tender or Accepted.";
										document.getElementById(name + i + "_ddlStatus").focus();
										return false;
									 }
									 if (SelStatus == 'A') 
									 {
										bln=true;
										document.getElementById('lblErrorMessage').innerText="Status Cannot Change from Open to Assigned from this screen. It can be done through assign order screen.";
										document.getElementById(name + i + "_ddlStatus").focus();
										return false;
									 }
									 break;
									}
									case 'E':	//Enter
									{
									 if (SelStatus == 'T' || SelStatus == 'C') 
									 {
										bln=true;
										document.getElementById('lblErrorMessage').innerText="Status Cannot Change from Enter to Tender or Accepted.";
										document.getElementById(name + i + "_ddlStatus").focus();
										return false;
									 }
									 if (SelStatus == 'A') 
									 {
										bln=true;
										document.getElementById('lblErrorMessage').innerText="Status Cannot Change from Enter to Assigned.";
										document.getElementById(name + i + "_ddlStatus").focus();
										return false;
									 }
									 break;
									}
									case 'T': //Tender
									{
										if (SelStatus == 'A') 
										{
											bln=true;
											document.getElementById('lblErrorMessage').innerText="Status Cannot Change from Tender to Assigned.";
											document.getElementById(name + i + "_ddlStatus").focus();
											return false;
										}
										break;
									}
									case 'C': //Accepted
									{//alert('');
										if (SelStatus == 'A') 
										{
											bln=true;
											document.getElementById('lblErrorMessage').innerText="Status Cannot Change from Accepted to Assigned.";
											document.getElementById(name + i + "_ddlStatus").focus();
											return false;
										}
										break;
									}
								}
							
								//For Carrier
								if (bln == false)
								{
									if (SelStatus == 'C')
									{
										oElement = document.getElementById(name + i + "_ddlCarrier");
										if (oElement != null)
										{
										if (document.frmTender.elements(name + i + "_ddlCarrier").value == 0)
										{
											document.getElementById('lblErrorMessage').innerText="Please select Carrier.";
											document.getElementById(name + i + "_ddlCarrier").focus();
											return false;
										}//End If
										}//End if
									}
									//Check Date format for PickupDate
									oElement = document.getElementById(name + i + "_txtPickupDate");
									if (oElement != null)
									{
										if (Trim(oElement.value) != "")
										{
										if (isDate(Trim(oElement.value),"MM/dd/yyyy") == false)
										{
											document.getElementById("lblErrorMessage").innerText="Invalid Date Format.";
											oElement.focus();
											return false;				
										}
										}	
									}
								}
							}
							else
							{
								if (curStatus == 'A' && willcall == 'True') return true
								document.getElementById('lblErrorMessage').innerText="Please select Status.";
								document.getElementById(name + i + "_ddlStatus").focus();
								return false;
							}
						}
					}
				}	
				}//End For
			}//End If
			if (bonesel == false)
			{
				document.getElementById('lblErrorMessage').innerText="Please, select at least one record to save";
				return false;
			}
			return true
		}//End Function
//-->
