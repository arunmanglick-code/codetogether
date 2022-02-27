<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JSCafe.aspx.cs" Inherits="JSCafe" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
    <link href="../App_Themes/MyTheme/stylecal.css" rel="stylesheet" type="text/css" />

    <script src="../JS/Calendar_new.js" type="text/javascript"></script>

    <script src="../JS/Calendar.js" type="text/javascript"></script>

    <script src="../JS/calendar-en.js" type="text/javascript"></script>

    <script type="text/javascript">
    
    function showMyCalendar()
    {
        //debugger;
        var title = document.getElementById('<%=txtDOB.ClientID %>');
        showCalendar(title,'dd/mm/y');
    }
    
    function ValidateDate()
    {
        var title = document.getElementById('<%=txtDOB.ClientID %>');
        if(title.value !='')
        {
            res=isDate(title.value);  // Stored in Common.js file    				
		    if (res)
		    {
			    return true;
		    }
		    else
		    {		 
		     document.getElementById('<%=txtDOB.ClientID %>').focus();
		    }
		}
    }
    
    function ShowWinCalc()
	{
		//debugger;
		var WshShell = new ActiveXObject("WScript.Shell");
		alert("Lauching Calculator");
		WshShell.Run("calc"); // winword, excel,pbrush
		WshShell.AppActivate("Calculator");
		
	}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="JavaScript Implementation"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
            </td>
        </tr>
    </table>
    <!-- Table 2 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td>
                <div id="divform">
                    <br />
                    <br />
                    <!-- Features Div -->
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Client Side - JS Based Calendar Control</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table id="Table1" style="border-right: #006699 1px solid; border-top: #006699 1px solid;
                            z-index: 101; left: 104px; border-left: #006699 1px solid; width: 778px; border-bottom: #006699 1px solid;
                            top: 8px; height: 288px" cellspacing="0" cellpadding="0" width="778" align="center"
                            border="1">
                            <!-- First Table -- First Row -->
                            <tr>
                                <td style="height: 26px" valign="middle" align="center">
                                </td>
                            </tr>
                            <!-- First Table -- Second Row -->
                            <tr>
                                <td class="leftrightbotborder" style="height: 251px" valign="top" align="center">
                                    <!-- Second Table -->
                                    <table class="contenttableborder" id="Table2" style="border-right: #006699 1px solid;
                                        border-top: #006699 1px solid; border-left: #006699 1px solid; width: 736px;
                                        border-bottom: #006699 1px solid; height: 352px" cellspacing="0" cellpadding="0"
                                        width="736" align="center" bgcolor="#faf7f0" border="0">
                                        <tr>
                                            <td class="HeaderRow" style="height: 21px" align="center" bgcolor="background" colspan="1"
                                                rowspan="1">
                                                <asp:Label ID="Label1" runat="server" CssClass="HeaderLabel" Font-Bold="True" ForeColor="White"
                                                    Font-Size="X-Small">Welcome to the JavaScript Nuts  & Bolts</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 189px" valign="top" align="left" colspan="1" rowspan="1">
                                                <!-- Third Table -->
                                                <table id="Table3" style="width: 749px; height: 761px" cellspacing="0" cellpadding="0"
                                                    width="749" align="center" border="1">
                                                    <tr>
                                                        <td style="width: 214px; height: 21px">
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblID" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0" Width="121px">Customer ID:</asp:Label>
                                                        </td>
                                                        <td style="height: 21px">
                                                            <asp:TextBox ID="txtID" runat="server" CssClass="textboxborder" Width="112px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 214px; height: 21px">
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblName" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0" Width="121px">Customer Full Name:</asp:Label>
                                                        </td>
                                                        <td style="height: 21px">
                                                            <asp:TextBox ID="txtname" runat="server" CssClass="textboxborder" Width="352px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 214px; height: 22px">
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblAddress" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0"
                                                                Width="136px">Customer Full Address:</asp:Label>
                                                        </td>
                                                        <td style="height: 22px">
                                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="textboxborder" Width="352px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 214px; height: 22px">
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblAge" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0" Width="136px">Customer Age:</asp:Label>
                                                        </td>
                                                        <td style="height: 22px">
                                                            <asp:TextBox ID="txtAge" runat="server" CssClass="textboxborder" Width="112px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 214px; height: 22px">
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="Label2" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0" Width="136px">Customer DO Birth:</asp:Label>
                                                        </td>
                                                        <td style="height: 22px">
                                                            <asp:TextBox ID="txtDOB" runat="server" CssClass="textboxborder" Width="112px" onblur="ValidateDate();"></asp:TextBox><img
                                                                id="imgCalendar" onmouseover="javascript:this.style.cursor='hand';" onclick="return showMyCalendar();"
                                                                src="../IMAGES/btn_calendar.gif">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr style="border: solid 0px blue;">
                                                        <td style="width: 214px; height: 22px">
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblGender" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0"
                                                                Width="160px">Customer Full Gender Info</asp:Label>
                                                        </td>
                                                        <td style="height: 22px">
                                                            <!-- Gender Table -->
                                                            <table id="GenderTable" style="width: 352px; height: 30px" cellspacing="0" cellpadding="0"
                                                                width="352" border="1">
                                                                <tr>
                                                                    <td style="width: 170px; height: 17px" align="center">
                                                                        <asp:RadioButton ID="rdBtnMale" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0"
                                                                            Width="80px" GroupName="Gender" Text="Male"></asp:RadioButton>
                                                                    </td>
                                                                    <td style="height: 17px" align="center">
                                                                        <asp:RadioButton ID="rdBtnFemale" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0"
                                                                            GroupName="Gender" Text="Female"></asp:RadioButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 51px" colspan="1">
                                                                        <!-- Male Panel -->
                                                                        <asp:Panel ID="pnlMale" Style="visibility: hidden" runat="server" Width="136px" Height="56px">
                                                                            <font color="#336699">Male Points</font>
                                                                            <!-- Male Point Table-->
                                                                            <table id="Table4" style="width: 128px; height: 64px" cellspacing="0" cellpadding="0"
                                                                                width="128" border="1">
                                                                                <tr>
                                                                                    <td style="height: 24px" align="right">
                                                                                        <asp:CheckBox ID="chkMSelectAll" runat="server" BackColor="#FAF7F0" Text="Select All">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="height: 15px">
                                                                                        <asp:CheckBox ID="chkMPoint1" runat="server" BackColor="#FAF7F0" Text="Point 1">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkMPoint2" runat="server" BackColor="#FAF7F0" Text="Point 2">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkMPoint3" runat="server" BackColor="#FAF7F0" Text="Point 3">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        M-Total:-
                                                                                        <asp:TextBox ID="txtMTotal" runat="server" CssClass="textboxborder" Width="27px"
                                                                                            ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <!-- End Male Point Table-->
                                                                            <!-- End Male Panel -->
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td style="height: 51px" colspan="1">
                                                                        <!-- Female Panel -->
                                                                        <asp:Panel ID="pnlFemale" Style="visibility: hidden" runat="server" Width="136px"
                                                                            Height="64px">
                                                                            <font color="#336699">FeMale Points</font>
                                                                            <!--Fe-Male Point Table-->
                                                                            <table id="Table5" style="width: 128px; height: 64px" cellspacing="0" cellpadding="0"
                                                                                width="128" border="1">
                                                                                <tr>
                                                                                    <td style="height: 22px" align="right">
                                                                                        <asp:CheckBox ID="chkFSelectAll" runat="server" BackColor="#FAF7F0" Text="Select All">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="height: 19px">
                                                                                        <asp:CheckBox ID="chkFPoint1" runat="server" BackColor="#FAF7F0" Text="Point 1">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkFPoint2" runat="server" BackColor="#FAF7F0" Text="Point 2">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkFPoint3" runat="server" BackColor="#FAF7F0" Text="Point 3">
                                                                                        </asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="height: 23px" align="right">
                                                                                        F-Total:-
                                                                                        <asp:TextBox ID="txtFTotal" runat="server" CssClass="textboxborder" Width="27px"
                                                                                            ReadOnly="True"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <!--Fe-Male Point Table-->
                                                                        </asp:Panel>
                                                                        <!-- End Female Panel -->
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <!-- End Gender Table -->
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 214px; height: 60px">
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblColor" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0"
                                                                Width="136px">Select Favourite Color</asp:Label>
                                                        </td>
                                                        <td style="height: 57px" align="left">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <!-- Drop down List Table-->
                                                            <table id="Table6" style="width: 352px; height: 24px" cellspacing="0" cellpadding="0"
                                                                width="352" border="0">
                                                                <tr>
                                                                    <td style="width: 170px">
                                                                        <asp:DropDownList ID="drpdwnColorList" runat="server" CssClass="textboxborder" Width="120px">
                                                                            <asp:ListItem Value="NoColor" Selected="True">No Color</asp:ListItem>
                                                                            <asp:ListItem Value="Red">Red</asp:ListItem>
                                                                            <asp:ListItem Value="Blue">Blue</asp:ListItem>
                                                                            <asp:ListItem Value="Green">Green</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        &nbsp;
                                                                        <asp:Label ID="lblChosenColor" runat="server">Label</asp:Label>
                                                                    </td>
                                                                    <td id="ColorColumn">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <!-- End Drop down List Table-->
                                                        </td>
                                                    </tr>
                                                    <!-- First Format Table -->
                                                    <tr>
                                                        <td style="width: 214px;">
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="Label4" runat="server" Font-Size="XX-Small" BackColor="#FAF7F0" Width="136px">Show Calculator</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <img id="imgCalc" onclick="ShowWinCalc()" src="../Images/cart.jpg" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- End Third Table -->
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- End Second Table -->
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
