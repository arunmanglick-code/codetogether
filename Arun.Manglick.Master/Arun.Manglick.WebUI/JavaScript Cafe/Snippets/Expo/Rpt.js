function Init(usertype)
{
	if (usertype == 0)
		MM_swapImage('Image10','','/exposervicedesk/Images/tabs/report-r.gif',1);
	else if (usertype == 2)
		MM_swapImage('Image4','','/exposervicedesk/Images/tabs/report1-r.gif',1);
		
	
	document.getElementById("pnlCustomerOrdDtl").runtimeStyle.display="none";
	document.getElementById("pnlCatItm").runtimeStyle.display="none";
	document.getElementById("pnlNewOrder").runtimeStyle.display="none";
	
	document.getElementById("trEvent").runtimeStyle.display="block";
	document.getElementById("pnlGrossRevenueByEvent").runtimeStyle.display="none";
	document.getElementById("pnlRMSNO").runtimeStyle.display="none";
	
	//Gross Revenue by Event, Master Account Posting , Month End Closeing 
	if (document.getElementById("optGrossRevByEvt").checked == true || document.getElementById("optMasAccPosting").checked == true || document.getElementById("optMthEndClose").checked == true || document.getElementById("optCommDistri").checked == true)
	{
		document.getElementById("trEvent").runtimeStyle.display="none";
		document.getElementById("pnlGrossRevenueByEvent").runtimeStyle.display="block";
	}
	//RMS
	if (document.getElementById("optRMS").checked == true)
	{
		document.getElementById("trEvent").runtimeStyle.display="none";
		document.getElementById("pnlRMSNO").runtimeStyle.display="block";
	}
	
	if (document.getElementById("optCusOrdDet").checked == true || document.getElementById("optProdCate").checked == true || document.getElementById("optIndiCate").checked == true || document.getElementById("optCusInv").checked == true || document.getElementById("optVenRev").checked == true)
	{
		if(document.getElementById("optCusOrdDet").checked == true)
		{
			document.getElementById("lblCom").innerText = "Company";
		}
		else if (document.getElementById("optCusInv").checked == true)
		{
			document.getElementById("lblCom").innerText = "Order No";
		}
		else if (document.getElementById("optVenRev").checked == true)
		{
			document.getElementById("lblCom").innerText = "Vendor";
		}
		else
		{
			document.getElementById("lblCom").innerText = "Category";
		}
		document.getElementById("pnlCustomerOrdDtl").runtimeStyle.display="block";
	}
	if (document.getElementById("optItemCate").checked == true)
	{
		document.getElementById("pnlCatItm").runtimeStyle.display="block";
	}
	if (document.getElementById("optNewOrder").checked == true)
	{
		document.getElementById("pnlNewOrder").runtimeStyle.display="block";
	}

	//if (rpt != '')
    //    window.open(rpt,'_blank','width=800,height= 800,status=no,resizable=yes,menubar = yes,dependent=yes,alwaysRaised=yes');
	
	return false;
}

function Validate()
{
	//Customer Order Detail
	window.document.title = 'ExpoServiceDesk : Reports - ' + event.srcElement.parentElement.innerText;
	if (event.srcElement.id == 'optCusOrdDet' ||event.srcElement.id == 'optProdCate' || event.srcElement.id == 'optIndiCate' || event.srcElement.id == 'optCusInv' || event.srcElement.id == 'optVenRev')
	{
		document.getElementById("ddlCompany").length=0;
		document.getElementById("pnlCustomerOrdDtl").runtimeStyle.display="block";
		
		//Customer Order Detail Report
		if(event.srcElement.id == 'optCusOrdDet')
			document.getElementById("lblCom").innerHTML = "Company";

		//Vendor Revenue Report
		else if (event.srcElement.id  == 'optVenRev')
			document.getElementById("lblCom").innerHTML = "Vendor";

		//Customer Invoice
		else if (event.srcElement.id == 'optCusInv')
			document.getElementById("lblCom").innerHTML = "Order No";
		else
			document.getElementById("lblCom").innerHTML = "Category";
	}
	else
		document.getElementById("pnlCustomerOrdDtl").runtimeStyle.display="none";

	//Category Item
	if (event.srcElement.id == 'optItemCate')
	{
		document.getElementById("ddlCat1").length=0;
		document.getElementById("ddlCat2").length=0;
		document.getElementById("ddlCat3").length=0;
		document.getElementById("pnlCatItm").runtimeStyle.display="block";
	}
	else
		document.getElementById("pnlCatItm").runtimeStyle.display="none";

	//New Order Report
	if (event.srcElement.id == 'optNewOrder')
	{
		document.getElementById("ddlCat1NewOrd").length=0;
		document.getElementById("txtDate").value='';
		document.getElementById("pnlNewOrder").runtimeStyle.display="block";
	}
	else
		document.getElementById("pnlNewOrder").runtimeStyle.display="none";

	//Gross Revenue by Event & RMS & Master Account Posting & Month End Closeing Report & Commission Distribution
	if (event.srcElement.id == 'optGrossRevByEvt' || event.srcElement.id == 'optRMS' || event.srcElement.id == 'optMasAccPosting' ||  event.srcElement.id == 'optMthEndClose' || event.srcElement.id == 'optCommDistri')
	{
		document.getElementById("trEvent").runtimeStyle.display="none";
		if (event.srcElement.id == 'optGrossRevByEvt' || event.srcElement.id == 'optMasAccPosting' ||  event.srcElement.id == 'optMthEndClose' || event.srcElement.id == 'optCommDistri')
		{
		document.getElementById("pnlGrossRevenueByEvent").runtimeStyle.display="block";
		document.getElementById("pnlRMSNO").runtimeStyle.display="none";
		}
		else
		{
		document.getElementById("pnlGrossRevenueByEvent").runtimeStyle.display="none";
		document.getElementById("pnlRMSNO").runtimeStyle.display="block";
		}
	}
	else
	{
		document.getElementById("trEvent").runtimeStyle.display="block";
		document.getElementById("pnlGrossRevenueByEvent").runtimeStyle.display="none";
		document.getElementById("pnlRMSNO").runtimeStyle.display="none";
	}
	return true;
}

function ValidatePage()
{
	var iCount;
	iCount=0;
	if (document.getElementById("optGrossRevByEvt").checked == true || document.getElementById("optMasAccPosting").checked == true || document.getElementById("optMthEndClose").checked == true || document.getElementById("optCommDistri").checked == true)
	{
		for(i=0 ; i<= document.getElementById('ChklEvents').cells.length-1;i++)
		{
			if(document.getElementById('ChklEvents_'+ i).checked == true)
				iCount += 1;	
		}
		if (iCount == '0')
		{
			alert("Select atleast one Event.");
			//document.getElementById('lblErrorMessage').innerText="Select atleast one Event.";
			document.getElementById('lblErrorMessage').focus();
			return false;
		}	
	}
	else if (document.getElementById("optRMS").checked == true)
	{
		oElement = document.getElementById("ddlMonthRMS")
		if (oElement.value == '')
		{
			alert("Select Month");
			//document.getElementById('lblErrorMessage').innerText="Select Month";
			document.getElementById('ddlMonthRMS').focus();
			return false;
		}
		
		oElement = document.getElementById("ddlYearRMS")
		if (oElement.value == '')
		{
			alert("Select Year.");
			//document.getElementById('lblErrorMessage').innerText="Select Year.";
			document.getElementById('ddlYearRMS').focus();
			return false;
		}

		oElement = document.getElementById("ddlEventsRMS")
		if (oElement.value == '')
		{
			alert("Select Event");
			//document.getElementById('lblErrorMessage').innerText="Select Event";
			document.getElementById('ddlEventsRMS').focus();
			return false;
		}
		
		oElement = document.getElementById("ddlOrderNO")
		if (oElement.value == '')
		{
			alert("Select Order No");
			//document.getElementById('lblErrorMessage').innerText="Select Order No";
			document.getElementById('ddlOrderNO').focus();
			return false;
		}
	}
	else
	{
		oElement = document.getElementById("ddlEvents")
		if (oElement.value == '')
		{
			alert("Select Event.");
			//document.getElementById('lblErrorMessage').innerText="Select Event.";
			document.getElementById('ddlEvents').focus();
			return false;
		}
	}
	
	//Category Item
	if (document.getElementById("optItemCate").checked == true)
	{
		oElement = document.getElementById("ddlCat1")
		if (oElement.value == '')
		{
			alert("Select Category 1.");
			//document.getElementById('lblErrorMessage').innerText="Select Category 1.";
			document.getElementById('ddlCat1').focus();
			return false;
		}

		oElement = document.getElementById("ddlCat2")
		if (oElement.value == '')
		{
			alert("Select Category 2.");
			//document.getElementById('lblErrorMessage').innerText="Select Category 2.";
			document.getElementById('ddlCat2').focus();
			return false;
		}
		
		oElement = document.getElementById("ddlCat3")
		if (oElement.value == '')
		{
			alert("Select Category 3.");
			//document.getElementById('lblErrorMessage').innerText="Select Category 3.";
			document.getElementById('ddlCat3').focus();
			return false;
		}
	}

	//Invoice
	if (document.getElementById("optCusInv").checked == true)
	{
		oElement = document.getElementById("ddlCompany")
		if (oElement.value == '')
		{
			alert(Select Order NO.);
			//document.getElementById('lblErrorMessage').innerText="Select Order NO.";
			document.getElementById('ddlCompany').focus();
			return false;
		}
	}
	
	//Vendor Revenue Report
	if (document.getElementById("optVenRev").checked == true)
	{
		oElement = document.getElementById("ddlCompany")
		if (oElement.value == '')
		{
			alert("Select Vendor.");
			//document.getElementById('lblErrorMessage').innerText="Select Vendor.";
			document.getElementById('ddlCompany').focus();
			return false;
		}
	}
	
	// Customer Order Report
	if (document.getElementById("optCusOrdDet").checked == true)
	{
		oElement = document.getElementById("ddlCompany")
		if (oElement.value == '')
		{
			alert("Select Company.");
			//document.getElementById('lblErrorMessage').innerText="Select Company.";
			document.getElementById('ddlCompany').focus();
			return false;
		}
	}
	
	//Indivisual Category Report
	if (document.getElementById("optProdCate").checked == true || document.getElementById("optIndiCate").checked == true )
	{
		oElement = document.getElementById("ddlCompany")
		if (oElement.value == '')
		{
			alert("Select Category.");
			//document.getElementById('lblErrorMessage').innerText="Select Category.";
			document.getElementById('ddlCompany').focus();
			return false;
		}
	}
	
	// New Order Report
	if (document.getElementById("optNewOrder").checked == true)
	{
		oElement = document.getElementById("ddlCat1NewOrd")
		if (oElement.value == '')
		{
			alert("Select Category.");
			//document.getElementById('lblErrorMessage').innerText="Select Category.";
			document.getElementById('ddlCat1NewOrd').focus();
			return false;
		}
		if (CheckBlank(document.getElementById('txtDate')))
		{
			alert("Date Cannot be Blank.");
			//document.getElementById('lblErrorMessage').innerText="Date Cannot be Blank.";
			document.getElementById('txtDate').focus();
			return false;
		}
		if (isDate(document.getElementById('txtDate').value,"MM/dd/yyyy")== false)
		{	
			alert("Date should be in MM/DD/YYYY Format.");
			//document.getElementById('lblErrorMessage').innerText="Date should be in MM/DD/YYYY Format.";
			document.getElementById('txtDate').focus();
			return false;
		}
	}
	ShowReport();
}

function ShowReport()
{
	var strRptName;
	if (document.getElementById('optExhibitoralpha').checked == true) 
		strRptName=document.getElementById('optExhibitoralpha').parentElement.innerText;
	else if (document.getElementById('optExhibitorbooth').checked == true) 
		strRptName=document.getElementById('optExhibitorbooth').parentElement.innerText;
	else if (document.getElementById('optCusOrdDet').checked == true) 
		strRptName=document.getElementById('optCusOrdDet').parentElement.innerText;
	else if (document.getElementById('optCusInv').checked == true) 
		strRptName=document.getElementById('optCusInv').parentElement.innerText;
	else if (document.getElementById('optProdCate').checked == true) 
		strRptName=document.getElementById('optProdCate').parentElement.innerText;
	else if (document.getElementById('optIndiCate').checked == true) 
		strRptName=document.getElementById('optIndiCate').parentElement.innerText;
	else if (document.getElementById('optItemCate').checked == true) 
		strRptName=document.getElementById('optItemCate').parentElement.innerText;
	else if (document.getElementById('optNewOrder').checked == true) 
		strRptName=document.getElementById('optNewOrder').parentElement.innerText;
	else if (document.getElementById('optRMS').checked == true) 
		strRptName=document.getElementById('optRMS').parentElement.innerText;
	else if (document.getElementById('optOpenRMS').checked == true) 
		strRptName=document.getElementById('optOpenRMS').parentElement.innerText;
	else if (document.getElementById('optBthChkVio').checked == true) 
		strRptName=document.getElementById('optBthChkVio').parentElement.innerText;
	else if (document.getElementById('optShowOrders').checked == true )
		strRptName=document.getElementById('optShowOrders').parentElement.innerText;
	else if (document.getElementById('optPaymentSumm').checked == true )
		strRptName=document.getElementById('optPaymentSumm').parentElement.innerText;
	else if (document.getElementById('optShowClose').checked == true )
		strRptName=document.getElementById('optShowClose').parentElement.innerText;
	else if (document.getElementById('optMasAccPosting').checked == true )
		strRptName=document.getElementById('optMasAccPosting').parentElement.innerText;
	else if (document.getElementById('optMthEndClose').checked == true )
		strRptName=document.getElementById('optMthEndClose').parentElement.innerText;
	else if (document.getElementById('optCommDistri').checked == true )
		strRptName=document.getElementById('optCommDistri').parentElement.innerText;
	else if (document.getElementById('optVenRev').checked == true )
		strRptName=document.getElementById('optVenRev').parentElement.innerText;
	else if (document.getElementById('optGrossRevByEvt').checked == true )
		strRptName=document.getElementById('optGrossRevByEvt').parentElement.innerText;
	else if (document.getElementById('optBthDel').checked == true )
		strRptName=document.getElementById('optBthDel').parentElement.innerText;
    	
	
    if (document.getElementById('optRMS').checked == true)
	{
        rpt = './Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&RptId=RMS' 
        rpt = rpt + '&FacName=' + document.getElementById('ddlFacility').options[document.getElementById('ddlFacility').selectedIndex].innerText
        rpt = rpt + '&Month=' + document.getElementById('ddlMonthRMS').value 
        rpt = rpt + '&Year=' + document.getElementById('ddlYearRMS').options[document.getElementById('ddlYearRMS').selectedIndex].innerText
        rpt = rpt  + '&EvtID=' + document.getElementById('ddlEventsRMS').value + '&ORDID=' + document.getElementById('ddlOrderNO').value
        
    }
    else if (document.getElementById('optGrossRevByEvt').checked == true || document.getElementById('optMasAccPosting').checked == true || document.getElementById('optMthEndClose').checked == true || document.getElementById('optCommDistri').checked == true)
	{
        rpt = './Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&RptId=' + strRptName 
        rpt = rpt + '&FacName=' + document.getElementById('ddlFacility').options[document.getElementById('ddlFacility').selectedIndex].innerText
        rpt = rpt + '&Month=' + document.getElementById('ddlMonth').options[document.getElementById('ddlMonth').selectedIndex].innerText 
        rpt = rpt + '&Year=' + document.getElementById('ddlYear').options[document.getElementById('ddlYear').selectedIndex].innerText  
        
		var sEvtId, sEvtNo, i
		sEvtId='';
		sEvtNo='';
        for (i=0; i<=document.getElementById('ChklEvents').cells.length-1; i++ )
		{
			objDdl = document.getElementById('ChklEvents_' + i);
			if(objDdl.checked == true)
			{
				sEvtId = sEvtId + Evt[i] + ',';
				sEvtNo = sEvtNo + objDdl.parentElement.innerText + ','; 
			}
		}
		sEvtId = sEvtId.substring(0,sEvtId.length-1);
		sEvtNo = sEvtNo.substring(0,sEvtNo.length-1);
		
        rpt = rpt + '&EvtID=' + sEvtId + '&EvtNo=' + sEvtNo
    }
    else if (document.getElementById('optVenRev').checked == true)
    {
		rpt = './Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&EvtID=' + document.getElementById('ddlEvents').value + '&RptId=' + strRptName 
		rpt = rpt + '&FacName=' + document.getElementById('ddlFacility').options[document.getElementById('ddlFacility').selectedIndex].innerText   
		rpt = rpt + '&VenId=' + document.getElementById('ddlCompany').value 
    }
    else if (document.getElementById('optBthChkVio').checked == true || document.getElementById('optShowOrders').checked == true)
	{
		rpt = './Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&EvtID=' + document.getElementById('ddlEvents').value + '&RptId=' + strRptName 
		rpt = rpt + '&FacName=' + document.getElementById('ddlFacility').options[document.getElementById('ddlFacility').selectedIndex].innerText   
    }
    else if (document.getElementById('optProdCate').checked == true || document.getElementById('optIndiCate').checked == true || document.getElementById('optCusInv').checked == true)
	{
		rpt = './Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&EvtID=' + document.getElementById('ddlEvents').value + '&RptId=' + strRptName + '&Cat1=' + document.getElementById('ddlCompany').value 
		rpt = rpt + '&FacName=' + document.getElementById('ddlFacility').options[document.getElementById('ddlFacility').selectedIndex].innerText   
	}
    else if (document.getElementById('optNewOrder').checked == true)
    {
        rpt = './Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&EvtID=' + document.getElementById('ddlEvents').value + '&RptId=' + strRptName + '&Cat1=' + document.getElementById('ddlCat1NewOrd').value 
        rpt = rpt + '&FacName=' + document.getElementById('ddlFacility').options[document.getElementById('ddlFacility').selectedIndex].innerText   
        rpt = rpt + '&Service=' + document.getElementById('ddlCat1NewOrd').options[document.getElementById('ddlCat1NewOrd').selectedIndex].innerText    
        rpt = rpt + '&ODate=' + document.getElementById('txtDate').value
	}        
    else if (document.getElementById('optItemCate').checked == true)
        rpt = './Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&EvtID=' + document.getElementById('ddlEvents').value + '&RptId=' + strRptName + '&Cat1=' + document.getElementById('ddlCat1').value + '&Cat2=' + document.getElementById('ddlCat2').value + '&Cat3=' + document.getElementById('ddlCat3').value 
    else if (document.getElementById('optCusOrdDet').checked == true)
        rpt = './Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&EvtID=' + document.getElementById('ddlEvents').value + '&RptId=' + strRptName + '&Comid=' + document.getElementById('ddlCompany').value 
    else
		rpt='./Report.aspx?FacID=' + document.getElementById('ddlFacility').value + '&EvtID=' + document.getElementById('ddlEvents').value + '&RptId=' + strRptName
        

	if (rpt != '')
	{
		if (document.getElementById('optExcel').checked == true)
			rpt = rpt + '&Export=Excel';
		else
			rpt = rpt + '&Export=PDF';
			
		window.open(rpt,'_blank','width=800,height= 800,status=no,resizable=yes,menubar = yes,dependent=yes,alwaysRaised=yes');
	}
}
