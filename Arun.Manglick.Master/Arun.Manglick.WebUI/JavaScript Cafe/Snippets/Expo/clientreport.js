function init()
			{
				if (usertype == 0)
					MM_swapImage('Image10','','/exposervicedesk/Images/tabs/report-r.gif',1);
				else if (usertype == 2)
					MM_swapImage('Image4','','/exposervicedesk/Images/tabs/report1-r.gif',1);
	
				if (document.getElementById('optGrossRevByEvt').checked == true || 
					document.getElementById('optMasAccPosting').checked == true || 
					document.getElementById('optMthEndClose').checked == true || 
					document.getElementById('optCommDistri').checked == true )
				{
					document.getElementById('divMonthYear').runtimeStyle.display="block";
					document.getElementById('divViewReport').runtimeStyle.display="block";
				}
				else
				{
					document.getElementById('divMonthYear').runtimeStyle.display="none";
					document.getElementById('divViewReport').runtimeStyle.display="none";
				}
				document.getElementById('divDate').runtimeStyle.display="none";	
			}
				
			function changeBG(objRow, mouseState)
			{   
				if (mouseState == 'Del')
				{
					objRow.parentElement.parentElement.runtimeStyle.backgroundColor = 
					objRow.checked ? '#ffcc00' : objRow.parentElement.parentElement.bgColor;
				}
				else if(objRow.runtimeStyle.backgroundColor != '#ffcc00')
				{
					if (mouseState == 'on')       
						objRow.runtimeStyle.backgroundColor ='#B5DAFE';
					else if	(mouseState == 'off-alteritem')  
						objRow.runtimeStyle.backgroundColor='WhiteSmoke';
					else if (mouseState == 'off-item')
						objRow.runtimeStyle.backgroundColor='White';
				}
				return true;
			} 

			function SelectReport()
			{
				if (
					document.forms[0].hdnflg.value == 'Facility_Event'
					&&
					(document.getElementById('optBthChkVio').checked || 
					document.getElementById('optShowOrders').checked || 
					document.getElementById('optShowClose').checked )
				)
				{
					document.getElementById('divDate').runtimeStyle.display="none";
					return true;
				}
				else if (
					document.forms[0].hdnflg.value == 'FillList_CustomerOrder' 
					&&
					document.getElementById('optCusOrdDet').checked
					)
					return true; 
				else if (
					document.forms[0].hdnflg.value == 'MonthYear_Facility_Event' 
					&&
					(document.getElementById('optMasAccPosting').checked || 
					document.getElementById('optMthEndClose').checked || 
					document.getElementById('optCommDistri').checked ||
					document.getElementById('optGrossRevByEvt').checked )
				)
					return true;
				else if (
					document.forms[0].hdnflg.value == 'Facility_Event_Vendor' 
					&&
					document.getElementById('optVenRev').checked 
				)
					return true;	
				__doPostBack(event.srcElement.id,'');
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
				if (document.getElementById('optCusOrdDet').checked == true) 
					return document.getElementById('optCusOrdDet').parentElement.innerText;
				else if (document.getElementById('optBthChkVio').checked == true) 
					return document.getElementById('optBthChkVio').parentElement.innerText;
				else if (document.getElementById('optShowOrders').checked == true )
					return document.getElementById('optShowOrders').parentElement.innerText;
				else if (document.getElementById('optShowClose').checked == true )
					return document.getElementById('optShowClose').parentElement.innerText;
				else if (document.getElementById('optMasAccPosting').checked == true )
					return document.getElementById('optMasAccPosting').parentElement.innerText;
				else if (document.getElementById('optMthEndClose').checked == true )
					return document.getElementById('optMthEndClose').parentElement.innerText;
				else if (document.getElementById('optCommDistri').checked == true )
					return document.getElementById('optCommDistri').parentElement.innerText;
				else if (document.getElementById('optVenRev').checked == true )
					return document.getElementById('optVenRev').parentElement.innerText;
				else if (document.getElementById('optGrossRevByEvt').checked == true )
					return document.getElementById('optGrossRevByEvt').parentElement.innerText;
			}

			//Reports : Exhibitor List, Payment Summary, Open RMS, Booth Check Violation, Show Closing, Batch Detail"
			function ShowReport(p1,p2,p3)			//p1 is facid, p2 is evtid and p3 is facname
			{
				if (document.getElementById('optBthChkVio').checked == true ||
					document.getElementById('optShowOrders').checked == true )
					rpt='./Report.aspx?FacID=' + p1 + '&EvtID=' + p2 + '&RptId=' + GetReportName() + '&FacName=' + p3
				else
					rpt='./Report.aspx?FacID=' + p1 + '&EvtID=' + p2 + '&RptId=' + GetReportName();

				DisplayReport(rpt);
			return false;
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

			//Reports: Customer Order Detail
			function ShowCustomerOrder(p1,p2,p3)	//p1 is facid, p2 is evtid and p3 is cmpid
			{
				rpt = './Report.aspx?FacID=' + p1 + '&EvtID=' + p2 + '&RptId=' + GetReportName() + '&Comid=' + p3
				DisplayReport(rpt);
				return false;
			}		

			//Reports: Master Acct Posting, Month End Closing, Comm Dist, Gross Reve By Event
			function ShowMonthYear(p)
			{
				if(p==0)
				{
					alert('Select events from only one Facility to view Report');
					return false;
			}
			else
			{
				var bOneSel;
				var objName="dgMasterAcct";		
				var name=objName + "__ctl";
				var rowItems = eval(objName).rows;
				var rowCount = rowItems.length;

				var sEvtId, sEvtNo
				sEvtId='';
				sEvtNo='';

				for (i=3; i<= rowCount; i++)
				{
					oElement = document.getElementById(name + i + "_chkSelect");
					if(oElement.checked == true)
					{
						bOneSel=true;

						j=i-3; j=j*2; 
						facid = Evt[j]
						facname = oElement.parentElement.parentElement.children(1).innerText;
						sEvtId = sEvtId + Evt[j+1] + ',';
						sEvtNo = sEvtNo + oElement.parentElement.parentElement.children(2).innerText + ','; 
					}
				}

				if(!bOneSel)
				{
					alert('Select atleast one Event to view Report');
					return false;
				}

				rpt = './Report.aspx?FacID=' + facid + '&RptId=' + GetReportName() 
				rpt = rpt + '&FacName=' + facname

				var MONTH_NAMES=new Array('Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec');
				var strSplit=new Array();
				strSplit=(document.getElementById('txtMonthYear').value).split("-");
				for (var i=1; i<=MONTH_NAMES.length; i++) 
				{
					var month_name=MONTH_NAMES[i-1];
					if (strSplit[0] == month_name)
					{
						if (i<10) i = "0" + i;
						break;
					}
				}
				rpt = rpt + '&Month=' + i
				rpt = rpt + '&Year=' + strSplit[1]

				sEvtId = sEvtId.substring(0,sEvtId.length-1);
				sEvtNo = sEvtNo.substring(0,sEvtNo.length-1);

				rpt = rpt + '&EvtID=' + sEvtId + '&EvtNo=' + sEvtNo

				DisplayReport(rpt);
				return false;
			}
		}
		
		function showMonth(p,obj)
		{
			var MONTH_NAMES=new Array('Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec');
			var strSplit=new Array();
	
			strSplit=(document.getElementById(obj).value).split("-");

			for (var i=0; i<MONTH_NAMES.length; i++) 
			{
				var month_name=MONTH_NAMES[i];
				if (strSplit[0] == month_name)
				{
					if (p == 'previous')
					{
						i -= 1;
						if(i<0) 
						{
							i=11;
							strSplit[1] -= 1;
							if (strSplit[1] < 2000) strSplit[1] = 2099; 
						}
					}
					else
					{
						i += 1;
						if(i>11) 
						{
							i=0;
							strSplit[1] = Number(strSplit[1]) + 1;
							if (strSplit[1] > 2099) strSplit[1] = 2000;
						}
					}	
					document.getElementById(obj).value = MONTH_NAMES[i] + '-' + strSplit[1];
					break;
				}
			}
		}