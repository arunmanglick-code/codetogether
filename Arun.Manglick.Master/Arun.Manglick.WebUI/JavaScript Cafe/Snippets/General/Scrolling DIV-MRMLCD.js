function GoBack()
{
	document.getElementById('WeekDiv').scrollLeft=document.getElementById('WeekDiv').scrollLeft - 100;
}	



function GoAhead()
{
	document.getElementById('WeekDiv').scrollLeft=document.getElementById('WeekDiv').scrollLeft + 100;
}	


//This is for (the follwoign structure.; ; )
{
}

<form id="Form1" method="post" runat="server">
			<DIV id="WeekDiv" style="BORDER-RIGHT: #336699 1px solid; BORDER-TOP: #336699 1px solid; OVERFLOW: auto; BORDER-LEFT: #336699 1px solid; BORDER-BOTTOM: #336699 1px solid; WIDTH:500px; HEIGHT=200px">
			<asp:datagrid id="DataGrid1" style="Z-INDEX: 101; LEFT: 38px; POSITION: absolute; TOP: 127px"
				runat="server" Height="101px" Width="930px" BorderWidth=0>				
				</asp:datagrid>
		</div> 
		
		<a href="javascript:GoBack()">Back</a>
		
		<a href="javascript:GoAhead()">Ahead</a>