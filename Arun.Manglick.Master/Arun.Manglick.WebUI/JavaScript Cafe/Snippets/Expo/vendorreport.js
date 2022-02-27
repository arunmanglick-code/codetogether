function init()
{
	document.getElementById('divMonthYear').runtimeStyle.display="none";
	document.getElementById('divViewReport').runtimeStyle.display="none";

	if (document.getElementById('optNewOrder').checked == true )
		document.getElementById('divDate').runtimeStyle.display="block";
	else
		document.getElementById('divDate').runtimeStyle.display="none";
}
function SelectReport()
{
	if (document.forms[0].hdnflg.value == 'Facility_Event'	&&
		document.getElementById('optOpenRMS').checked )
	{
		document.getElementById('divDate').runtimeStyle.display="none";
		return true;
	}
	else if (
		document.forms[0].hdnflg.value == 'Facility_Event_Vendor' 
		&&
		document.getElementById('optVenRev').checked 
	)
		return true;	
	else if (
		document.forms[0].hdnflg.value == 'Facility_Event_Category'
		&&
		(document.getElementById('optProdCate').checked || 
		document.getElementById('optNewOrder').checked )
	)
	{
		if (document.getElementById('optNewOrder').checked == true)
			document.getElementById('divDate').runtimeStyle.display="block";
		else
			document.getElementById('divDate').runtimeStyle.display="none";
		return true;
	}
	else if (
		document.forms[0].hdnflg.value == 'Facility_Event_Category_SubCat1_SubCat2' 
		&& 	
		document.getElementById('optItemCate').checked
	)
	return true;
	__doPostBack(event.srcElement.id,'');
} 
		
//Reports : Open RMS
function ShowReport(p1,p2,p3)			//p1 is facid, p2 is evtid and p3 is facname
{
	rpt='./Report.aspx?FacID=' + p1 + '&EvtID=' + p2 + '&RptId=' + GetReportName();

	DisplayReport(rpt);
	return false;
}

//Reports: Show Production Category, New Order
function ShowCategory(p1,p2,p3,p4,p5)	//p1 is facid, p2 is evtid, p3 is catid, p4 is facname and p5 is category description
{	
	if (document.getElementById('optNewOrder').checked == true)
	{
		rpt = './Report.aspx?FacID=' + p1 + '&EvtID=' + p2 + '&RptId=' + GetReportName() + '&Cat1=' + p3
		rpt = rpt + '&FacName=' + p4 
		rpt = rpt + '&Service=' + p5
		rpt = rpt + '&ODate=' + document.getElementById('txtDate').value
	}        
	else
	{
		rpt = './Report.aspx?FacID=' + p1 + '&EvtID=' + p2 + '&RptId=' + GetReportName() + '&Cat1=' + p3 
		rpt = rpt + '&FacName=' + p4
	}
	DisplayReport(rpt);
	return false;
}

//Reports: Item Category
function ShowItemCategory(p1,p2,p3,p4,p5)	//p1 is facid, p2 is evtid, p3 is cat1id, p4 is cat2id and p5 is cat3id
{
	rpt = './Report.aspx?FacID=' + p1 + '&EvtID=' + p2 + '&RptId=' + GetReportName() + '&Cat1=' + p3 + '&Cat2=' + p4 + '&Cat3=' + p5
	DisplayReport(rpt);
	return false;
}

function DisplayReport(rpt)
{
	if (document.getElementById('optExcel').checked == true)
		rpt = rpt + '&Export=Excel';
	else
		rpt = rpt + '&Export=PDF';

	window.open(rpt,'_blank','width=800,height= 800,status=no,resizable=yes,menubar = yes,dependent=yes,alwaysRaised=yes');
}

function GetReportName()
{
	if (document.getElementById('optProdCate').checked == true) 
		return document.getElementById('optProdCate').parentElement.innerText;
	else if (document.getElementById('optOpenRMS').checked == true) 
		return document.getElementById('optOpenRMS').parentElement.innerText;
	else if (document.getElementById('optNewOrder').checked == true) 
	return document.getElementById('optNewOrder').parentElement.innerText;
		else if (document.getElementById('optVenRev').checked == true )
	return document.getElementById('optVenRev').parentElement.innerText;
		else if (document.getElementById('optItemCate').checked == true) 
	return document.getElementById('optItemCate').parentElement.innerText;
}

//Reports: Vendor Revenue
function ShowVendorRevenue(p1,p2,p3,p4)	//p1 is facid, p2 is evtid, p3 is venid and p4 is facname 
{
	rpt = './Report.aspx?FacID=' + p1 + '&EvtID=' + p2 + '&RptId=' + GetReportName()
	rpt = rpt + '&FacName=' + p4
	rpt = rpt + '&VenId=' + p3

	DisplayReport(rpt);
	return false;
}
