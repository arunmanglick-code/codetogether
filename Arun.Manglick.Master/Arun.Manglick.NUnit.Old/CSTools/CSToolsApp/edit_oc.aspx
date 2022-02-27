<%@ Page Language="c#" Title="CSTools: Edit Ordering Clinician" Inherits="Vocada.CSTools.edit_oc"
    MasterPageFile="~/cs_tool.master" CodeFile="edit_oc.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlEditOC" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            <script language="JavaScript" type="text/javascript" src="./Javascript/CalendarPopup.js"></script>

            <script language="JavaScript" type="text/javascript" src="./Javascript/calendar.js"></script>

            <script language="JavaScript" type="text/javascript">document.write(getCalendarStyles());</script>

            <script language="javascript" type="text/javascript" src="Javascript/Common.js"></script>
            
            <script language="javascript" src="./Javascript/edit_rpnotification_step1.js" type="text/javascript"></script>

            <script language="javascript" type="text/javascript">
                var mapId = "edit_oc.aspx";
                function UpdateLabel()
                {
                    
                    var cmbDept = document.getElementById(cmbDepartmentClientID);
                    var lblStar = document.getElementById(lblStarClientID);
                    if (cmbDept.value == "-1")
                        {lblStar.innerText=":";}
                    else
                         {lblStar.innerText="*:";}
                }
                
                /*To Get Confirmation before Delete.*/
                function ConformBeforeCTDelete(isActive)
                {
                    if (isActive == "True")
                    {
                        if(confirm('You are deleting an active assignment. Do you want to proceed?' ))
                        {
                            ChangeFlagforGrid();
                            otherPostback =true;
                            return true;
                        }
                    }
                    else
                    {
                        if(confirm('Are you sure you want to delete selected Clinical Team assignment?' ))
                        {
                            ChangeFlagforGrid();
                            otherPostback =true;
                            return true;
                        }
                    }
                    otherPostback =false;
                    return false;          
                }
                 
                function enbaleDisableDeptCombo()
                {
                    var cmbDept = document.getElementById(cmbDepartmentClientID);
                    var resideentChk = document.getElementById(chkResidentClientID);
                    if(resideentChk.checked)
                    {
                        cmbDept.value = "-1";
                        cmbDept.disabled = true;                 
                    }
                    else
                    {
                        cmbDept.disabled = false;                 
                    }
                    
                }
                /*Enable Disbale Start date end date as per department selected.*/
                function enableDateSelection(startup)
                {
                    var cmbDept = document.getElementById(cmbDepartmentClientID);
                    var startDate = document.getElementById(anchFromDateClientID);
                    var endDate = document.getElementById(anchToDateClientID);
                    var startDateText = document.getElementById(txtStartDateClientID);
                    var endDateText = document.getElementById(txtEndDateClientID);

                    if(cmbDept.value == -1)
                    {
                        startDateText.value = "";
                        endDateText.value = "";
                        startDate.onClick = "return false;";
                        endDate.onClick = "return false;";
                        
                        startDateText.disabled = true;
                        endDateText.disabled = true;
                    }
                    else
                    {
                        startDate.onClick = "javascript:calRPStartDate.select(document.all['" + txtStartDateClientID + "'],document.all['" + anchFromDateClientID + "'],'MM/dd/yyyy');return false";
                        endDate.onClick = "javascript:calRPEndDate.select(document.all['" + txtEndDateClientID + "'],document.all['" + anchToDateClientID + "'],'MM/dd/yyyy');return false";
                        if(cmbDept.disabled == false)
                            startDateText.disabled = false;
                        else
                            startDateText.disabled = true;

                        endDateText.disabled = false;
                    }
                }
                
                function checkCellPhone(source,arguments)
                {
                   var txtCell1 = document.getElementById(txtCellAreaCodeClientID).value;
                   var txtCell2 = document.getElementById(txtCellPrefixClientID).value;
                   var txtCell3 = document.getElementById(txtCellNNNNClientID).value; 
                   var errormsg = "";
                   cellLength = txtCell1.length + txtCell2.length + txtCell3.length;
                   
                   if (cellLength >0 && cellLength != 10)
                   {
                           source.errormessage = "Plese Enter Valid Cell Phone.";
                           arguments.IsValid = false;
                            return false;
                   }
                   
                   arguments.IsValid = true;
                            return true;
                }
                
                var calRPStartDate = new CalendarPopup("divEditRPGrdStartDate");
                CP_setControlAdjustments(90, 85);
                document.write(getCalendarStyles());
                calRPStartDate.setYearSelectStartOffset(50);
                calRPStartDate.showYearNavigation();
                calRPStartDate.showNavigationDropdowns();
              
                var calRPEndDate = new CalendarPopup("divEditRPGrdEndDate"); 
                CP_setControlAdjustments(90, 85);
                document.write(getCalendarStyles());
                calRPEndDate.setYearSelectStartOffset(50);
                calRPEndDate.showYearNavigation();
                calRPEndDate.showNavigationDropdowns();
              
            </script>

            <script language="javascript" type="text/javascript" src="Javascript/EditOC.js"></script>
            
            <input type="hidden" id="scrollPos" value="0" runat="server" />
            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table height="100%" align="center" width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="DivBg" valign="top" align="center">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr valign="top">
                                                <td class="Hd1" style="height: 20px">
                                                    Edit Ordering Clinician Information
                                                </td>
                                            </tr>
                                        </table>
                                        
                                        <table width="100%" align="center" height="98%" border="0" cellpadding="0" cellspacing="0">
                                            <tr class="BottomBg">
                                                <td valign="top" align="Center" style="width: 99.5%">
                                                    <input type="hidden" id="linkClick" runat="server" name="linkClick" value="false" />
                                                    <fieldset class="fieldsetBlue" >
                                                        <legend>Contact Information</legend>
                                                        <table width="98%" border="0" align="center" cellpadding="2" cellspacing="1">
                                                        <tr>
                                                         <td>
                                                                Veriphy ID:
                                                             </td>
                                                             <td>
                                                                <asp:Label ID="lblRPId" runat="server"></asp:Label>
                                                             </td>
                                                             <td width="50%" colspan="5" rowspan="8">
                                                                    <fieldset class="fieldsetCBlue">
                                                                        <legend class="">Clinical Team Assignments</legend>
                                                                        <asp:UpdatePanel ID="upnlCT" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                        <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                                                                            <tr>
                                                                                <td width="20%" align="left">
                                                                                    Clinical Team:
                                                                                    <input type="hidden" id="hdnCTChanged" runat="server" name="hdnCTChanged" value="false" />
                                                                                </td>
                                                                                <td colspan = "5">
                                                                                    <asp:DropDownList ID="drpDepartment" AutoPostBack="true" TabIndex="13" runat="server" 
                                                                                        OnSelectedIndexChanged="drpDepartment_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Phone:</td>
                                                                                <td colspan = "5">
                                                                                    <asp:Label id="lblPhone" runat="server" Font-Bold="True"></asp:Label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="10%">
                                                                                    Start Date<label id="lblStar" runat="server">:</label></td>
                                                                                <td nowrap width="20%">
                                                                                    <asp:TextBox ID="txtStartDate" TabIndex="14" runat="server" Columns="10" MaxLength="75"></asp:TextBox>
                                                                                    &nbsp;<a href="#" runat="server" name="anchFromDate" id="anchFromDate" style="height: 22px;
                                                                                        vertical-align: middle;"><% if (strUserSettings == "YES")
                                                                                                                    { %>Calendar<% }
                                                                                                                                   else
                                                                                                                                   { %><img src="img/ic_cal.gif" width="17" height="15" border="0" /><% } %></a>
                                                                                </td>
                                                                                <td nowrap width="10%">
                                                                                    End Date:</td>
                                                                                <td nowrap width="20%">
                                                                                    <asp:TextBox ID="txtEndDate" TabIndex="15" runat="server" Columns="10" MaxLength="75"></asp:TextBox>
                                                                                    &nbsp;<a href="#" runat="server" name="anchToDate" id="anchToDate" style="height: 22px;
                                                                                        vertical-align: middle;"><% if (strUserSettings == "YES")
                                                                                                                    { %>Calendar<% }
                                                                                                                                   else
                                                                                                                                   { %><img src="img/ic_cal.gif" alt="" width="17" height="15" border="0" /><% } %></a>
                                                                                </td>
                                                                                <td width="10%">
                                                                                    <asp:Button CssClass="Frmbutton" ID="btnAddDept" runat="server" Text="Add"
                                                                                        UseSubmitBehavior="false" CausesValidation="false" TabIndex="16" OnClick="btnAddDept_Click"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button CssClass="Frmbutton" ID="btnClear" runat="server" Text="Cancel"
                                                                                        UseSubmitBehavior="false" CausesValidation="false" TabIndex="17" OnClick="btnClear_Click"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Assignments:
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostbackTrigger ControlID="btnAddDept" />
                                                                        </Triggers>
                                                                        </asp:UpdatePanel>
                                                                            
                                                                        <asp:UpdatePanel ID="upnlDepartmentGrid" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                        <table width="100%" border="0" align="center" cellpadding="2" cellspacing="1">
                                                                            <tr>
                                                                                <td align="center" valign="top" width="100%" colspan = "6">                                                                                    
                                                                                    <div id="DepartmentDiv" class="TDiv" >
                                                                                        <asp:DataGrid ID="grdDepartmentAssignment" runat="server" CssClass="GridHeader" AllowPaging="false"
                                                                                            AutoGenerateColumns="False" Width="100%" ItemStyle-Height="25px" HeaderStyle-Height="25px"
                                                                                            OnItemDataBound="grdDepartmentAssignment_ItemDataBound"
                                                                                            OnDeleteCommand="grdDepartmentAssignment_DeleteCommand" OnEditCommand="grdDepartmentAssignment_EditCommand">
                                                                                            <SelectedItemStyle Font-Bold="True" ForeColor="Navy" BackColor="#EFCA98"></SelectedItemStyle>
                                                                                            <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                            <HeaderStyle VerticalAlign="Top" CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                                                            <Columns>
                                                                                                <asp:BoundColumn Visible="False" DataField="DepartmentAssignID" ReadOnly="True" HeaderText="Clinical Team Assignment ID">
                                                                                                </asp:BoundColumn>
                                                                                                <asp:BoundColumn Visible="False" DataField="ReferringPhysicianID" ReadOnly="True"
                                                                                                    HeaderText="OCID"></asp:BoundColumn>
                                                                                                <asp:BoundColumn Visible="False" DataField="DepartmentID" ReadOnly="True" HeaderText="Clinical Team ID">
                                                                                                </asp:BoundColumn>
                                                                                                <asp:TemplateColumn HeaderText="Team" ItemStyle-Wrap="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDepartmentName" runat="server" Width="100px"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn HeaderText="Start Date">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblStartDate" runat="server" Width="100px"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn HeaderText="End Date">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblEndDate" runat="server" Width="100px"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:ButtonColumn Text="Edit" HeaderText="Edit" CommandName="Edit">
                                                                                                    <ItemStyle Width="20px" />
                                                                                                </asp:ButtonColumn>
                                                                                                
                                                                                                <asp:TemplateColumn HeaderText="Delete">
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CausesValidation="false"
                                                                                                            CommandName="Delete"></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                            </Columns>
                                                                                        </asp:DataGrid>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        </ContentTemplate>
                                                                        </asp:UpdatePanel>                                                                               
                                                                    </fieldset>
                                                                </td>
                                                                </tr>
                                                             
                                                            
                                                            <tr nowrap>
                                                                <td width="16%">
                                                                    First Name*:</td>
                                                                <td width="45%">
                                                                    <asp:TextBox ID="txtFirstName" TabIndex="1" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Last Name*:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLastName" TabIndex="2" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Nick Name:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNickname" TabIndex="3" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>                                                            
                                                            <tr>
                                                                <td nowrap>
                                                                    Office Phone*:</td>
                                                                <td>
                                                                    (<asp:TextBox ID="txtPrimaryPhoneAreaCode" TabIndex="4" Columns="4" runat="server"
                                                                        MaxLength="3"></asp:TextBox>)
                                                                    <asp:TextBox ID="txtPrimaryPhonePrefix" TabIndex="5" runat="server" Columns="4" MaxLength="3"></asp:TextBox>
                                                                    -
                                                                    <asp:TextBox ID="txtPrimaryPhoneNNNN" TabIndex="6" runat="server" Columns="6" MaxLength="4"></asp:TextBox></td>
                                                                    </tr>
                                                                    
                                                                    <tr>
                                                                    
                                                                 <td>
                                                                    Cell Phone:</td>
                                                                 <td width="35%" align="left">
                                                                    (<asp:TextBox ID="txtCellAreaCode" TabIndex="17" Columns="4" runat="server"
                                                                        MaxLength="3"></asp:TextBox>)
                                                                    <asp:TextBox ID="txtCellPrefix" TabIndex="18" runat="server" Columns="4" MaxLength="3"></asp:TextBox>-
                                                                    <asp:TextBox ID="txtCellNNNN" TabIndex="19" runat="server" Columns="6" MaxLength="4"></asp:TextBox></td></tr>
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>   
                                                                    
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                    
                                                            <tr>
                                                                <td >
                                                                    Specialty:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSpecialty" TabIndex="7" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>

                                                                <td width="12%">
                                                                    Email:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEmail" TabIndex="20" runat="server" Columns="35" MaxLength="100"></asp:TextBox></td>
                                                                                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Practice Group:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPracticeGroup" TabIndex="8" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                <td>
                                                                    Fax:
                                                                </td>
                                                                <td>
                                                                    (<asp:TextBox ID="txtFaxAreaCode" TabIndex="21" runat="server" Columns="4" MaxLength="3"></asp:TextBox>)
                                                                    <asp:TextBox ID="txtFaxPrefix" TabIndex="22" runat="server" Columns="4" MaxLength="3"></asp:TextBox>
                                                                    -
                                                                    <asp:TextBox ID="txtFaxNNNN" TabIndex="23" runat="server" Columns="6" MaxLength="4"></asp:TextBox></td>
                                                                                                                            </tr>
                                                            <tr>
                                                                <td nowrap>
                                                                    Hospital Affiliation:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtHospitalAffiliation" TabIndex="9" runat="server" Columns="35"
                                                                        MaxLength="75"></asp:TextBox></td>

                                                                <td>
                                                                    Address 1:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAddress1" TabIndex="24" runat="server" Columns="35" MaxLength="100"></asp:TextBox></td>
                                                                                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    City:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCity" TabIndex="10" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>

                                                                <td>
                                                                    Address 2:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAddress2" TabIndex="25" runat="server" Columns="35" MaxLength="100"></asp:TextBox></td>
                                                                                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    State:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtState" TabIndex="11" runat="server" Columns="35" MaxLength="2"
                                                                        Width="31px"></asp:TextBox></td>
                                                                <td>
                                                                    Address 3:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAddress3" TabIndex="26" runat="server" Columns="35" MaxLength="100"></asp:TextBox></td>
                                                            <tr>
                                                                <td>
                                                                    Zip:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtZip" TabIndex="12" runat="server" Columns="35" MaxLength="5"></asp:TextBox></td>

                                                                <td>
                                                                    Active:</td>
                                                                <td colspan="3">
                                                                    <asp:CheckBox ID="cbActive" TabIndex="27" runat="server"></asp:CheckBox>
                                                                    <div id="divEditRPGrdStartDate" style="position: absolute; z-index: 100%;" class="Calander">
                                                                    </div>
                                                                    <div id="divEditRPGrdEndDate" style="position: absolute; z-index: 100%;" class="Calander">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%" align="center" border="0" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <fieldset id="fldLoginDetails" class="fieldsetCBlue" runat="server" style="font-weight: bold;width: 98%;">
                                                                        <legend class="">Login Details</legend>
                                                                        <asp:UpdatePanel ID="upnlLogin" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlEDDoc" runat="server" Visible="false">
                                                                                    <td width="15%" align="left">
                                                                                        ED Doc:</td>
                                                                                    <td width="46%" align="left">
                                                                                        <asp:CheckBox ID="chkEDDoc" TabIndex="28" Checked="false" AutoPostBack="true" runat="server"
                                                                                            Visible="true" TextAlign="Right" Style="position: relative" OnCheckedChanged="chkEDDoc_CheckedChanged" />
                                                                                    </td>
                                                                                    <td width="11%">
                                                                                        &nbsp;</td>
                                                                                    <td width="28%">
                                                                                        &nbsp;</td>
                                                                                </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Panel ID="pnlOCLogin" runat="server" Visible="false">
                                                                                        <td width="15%">
                                                                                            Login ID*:</td>
                                                                                        <td width="46%">
                                                                                            <asp:TextBox ID="txtLoginID" TabIndex="29" runat="server" Columns="10" MaxLength="10"></asp:TextBox>&nbsp;(10
                                                                                            characters)
                                                                                            <asp:RegularExpressionValidator ID="revalLoginID" runat="server" ErrorMessage="Login ID must be 2 - 10 characters / digits"
                                                                                                ControlToValidate="txtLoginID" Display="None" ValidationExpression="^[\w\d]{2,10}$"></asp:RegularExpressionValidator>
                                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please enter valid Login ID."
                                                                                                ControlToValidate="txtLoginID" Display="None" ValidationExpression="(\w)*(\d)*"></asp:RegularExpressionValidator>
                                                                                            <asp:RequiredFieldValidator ID="rfvalLoginID" SetFocusOnError="true" runat="server"
                                                                                                ErrorMessage="You must enter Login ID." ControlToValidate="txtLoginID" Display="None"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                        <td width="11%">
                                                                                            ED PIN*:</td>
                                                                                        <td width="28%">
                                                                                            <asp:TextBox ID="txtPassword" TabIndex="30" runat="server" Columns="10" MaxLength="10"></asp:TextBox>&nbsp;(Max
                                                                                            10 digits)
                                                                                            <asp:RegularExpressionValidator ID="revalPassword" SetFocusOnError="true" runat="server"
                                                                                                ErrorMessage="PIN must be 4-10 digits." ControlToValidate="txtPassword"
                                                                                                Display="None" ValidationExpression="\d{4,10}"></asp:RegularExpressionValidator>
                                                                                            <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvalPIN" runat="server" ErrorMessage="You must enter PIN"
                                                                                                ControlToValidate="txtPassword" Display="None"></asp:RequiredFieldValidator>
                                                                                            <asp:Button ID="btnGeneratePassword" runat="server" CssClass="Frmbutton" Width="84px"
                                                                                                CausesValidation="False" Text="Generate PIN" OnClick="btnGeneratePassword_Click"
                                                                                                UseSubmitBehavior="false" TabIndex="31" onKeyDown="if(event.keyCode==13) return false;">
                                                                                            </asp:Button>
                                                                                        </td>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <asp:Panel ID="pnlMessageRetieve" runat="server" Visible="false">
                                                                                <td>                                                                                                               
                                                                                <td width="15%">
                                                                                    PIN for Message Retrieval:</td>
                                                                                <td width="46%">
                                                                                    <asp:TextBox ID="txtPinForMessage" TabIndex="32" runat="server" Columns="5" MaxLength="5"></asp:TextBox>&nbsp;(4
                                                                                    - 5 digits)
                                                                                    <asp:RegularExpressionValidator ID="revalPIN" SetFocusOnError="true" runat="server"
                                                                                        ErrorMessage="PIN for Message Retrieval must be 4-5 digits." ControlToValidate="txtPinForMessage"
                                                                                        Display="None" ValidationExpression="\d{4,5}"></asp:RegularExpressionValidator>
                                                                                    <asp:Button ID="btnPinForMessage" runat="server" CssClass="Frmbutton" Width="84px"
                                                                                        CausesValidation="False" Text="Generate PIN" OnClick="btnPinForMessage_Click"
                                                                                        UseSubmitBehavior="false" TabIndex="33" onKeyDown="if(event.keyCode==13) return false;">
                                                                                    </asp:Button>
                                                                                </td>
                                                                                <td width="11%">
                                                                                    &nbsp;</td>
                                                                                <td width="28%">
                                                                                    &nbsp;</td>
                                                                                 </td>
                                                                               </asp:Panel>
                                                                            </tr>
                                                                        </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="chkEDDoc" EventName="CheckedChanged" />
                                                                        </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%" align="center" border="0" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <fieldset id="fldAdditionalContactInfo" class="fieldsetCBlue" runat="server" style="font-weight: bold;width: 98%;">
                                                                        <legend class="">Additional Contact Information</legend>
                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td style="height: 25px" width="15%">
                                                                                    Name:
                                                                                </td>
                                                                                <td style="height: 25px" width="46%">
                                                                                    <asp:TextBox ID="txtName" runat="server" Columns="35" MaxLength="75" Style="left: 1px;
                                                                                        position: relative; top: 2px" TabIndex="34"></asp:TextBox></td>
                                                                                <td style="height: 25px" width="11%">
                                                                                    Radiology TDR:</td>
                                                                                <td style="height: 25px" width="28%">
                                                                                    <asp:CheckBox ID="chkRadiologyTDR" runat="server" TabIndex="38" Style="left: 1px;
                                                                                        position: relative; top: 2px" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 25px">
                                                                                    Phone:
                                                                                </td>
                                                                                <td style="height: 25px">
                                                                                    (<asp:TextBox ID="txtPhoneCode" runat="server" TabIndex="35" Columns="4" MaxLength="3"
                                                                                        Style="position: relative"></asp:TextBox>)
                                                                                    <asp:TextBox ID="txtPhonePrefix" runat="server" TabIndex="36" Columns="4" MaxLength="3"
                                                                                        Style="position: relative"></asp:TextBox>
                                                                                    -
                                                                                    <asp:TextBox ID="txtPhoneNumber" runat="server" TabIndex="37" Columns="6" MaxLength="4"
                                                                                        Style="position: relative"></asp:TextBox></td>
                                                                                <td style="height: 25px">
                                                                                    Lab TDR:</td>
                                                                                <td style="height: 25px">
                                                                                    <asp:CheckBox ID="chkLabTDR" runat="server" TabIndex="39" Style="left: 1px; position: relative;
                                                                                        top: 2px" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>&nbsp;
                                                                                </td>
                                                                                <td>&nbsp;
                                                                                </td>
                                                                                <td style="height: 25px">
                                                                                    Profile Returned:</td>
                                                                                <td style="height: 25px">
                                                                                    <asp:CheckBox ID="chkProfileCompleted" runat="server" TabIndex="40" Style="left: 1px;
                                                                                        position: relative; top: 2px" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 15px; width: 80px;" colspan="5">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%" align="center" border="0" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <fieldset id="fldExternalIDs" class="fieldsetCBlue" runat="server" style="font-weight: bold;width: 98%;">
                                                                        <legend class="">External System ID's</legend>
                                                                        <asp:UpdatePanel ID="UpdatePanelExternalInfo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td width="98%">
                                                                                    <br />
                                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblIDType" runat="server" Text="ID Type:"></asp:Label>&nbsp;
                                                                                        <asp:DropDownList ID="ddlIDType" runat="server" AutoPostBack="True" TabIndex="37" OnSelectedIndexChanged="ddlIDType_SelectedIndexChanged"></asp:DropDownList>&nbsp;
                                                                                        <asp:Label ID="lblUserId" runat="server" Text="User ID:"></asp:Label>&nbsp;
                                                                                        <asp:TextBox ID="txtUserId" runat="server" TabIndex="38"></asp:TextBox>&nbsp;
                                                                                        <asp:Label ID="lblAddIDType" runat="server" Text="ID Type:" Visible="false"></asp:Label>&nbsp;
                                                                                        <asp:TextBox ID="txtAddIDType" runat="server" Visible="false" TabIndex="39"></asp:TextBox>&nbsp;
                                                                                        <asp:Button ID="btnAddExternalID" CssClass="Frmbutton" TabIndex="40" runat="server" Text="Add" OnClick="btnAddExternalID_Click"></asp:Button>
                                                                                    </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td width="98%">
                                                                                <br />
                                                                                    <div id="divExternalIDInformation" runat="server" class="TDiv">
                                                                                        <asp:DataGrid ID="grdIdTypeInfo" runat="server" CssClass="GridHeader" BorderStyle="None"
                                                                                            AllowSorting="true" DataKeyField="ExternalRPID" AutoGenerateColumns="false" Width="100%"
                                                                                            ItemStyle-Height="21px" HeaderStyle-Height="24" OnEditCommand="grdIdTypeInfo_EditCommand"
                                                                                            OnUpdateCommand="grdIdTypeInfo_UpdateCommand" OnCancelCommand="grdIdTypeInfo_CancelCommand"
                                                                                            OnDeleteCommand="grdIdTypeInfo_DeleteCommand" OnItemCreated="grdIdTypeInfo_OnItemCreated" >
                                                                                            <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                            <HeaderStyle VerticalAlign="Top" CssClass="THeader" HorizontalAlign="Left" Font-Bold="True">
                                                                                            </HeaderStyle>
                                                                                            <Columns>
                                                                                                <asp:TemplateColumn HeaderText="ID Type" ItemStyle-Width="40%">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblIDType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ExternalIDTypeDescription") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:TemplateColumn HeaderText="ID Number" ItemStyle-Width="40%">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblIDNumber" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ExternalID") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:TextBox ID="txtIDNumber" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ExternalID") %>'
                                                                                                            ReadOnly="false"></asp:TextBox>
                                                                                                    </EditItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:EditCommandColumn EditText="Edit" CancelText="Cancel" UpdateText="Update" HeaderText="Edit" ItemStyle-Width="10%" CausesValidation="false" ValidationGroup="UpdatePanelExternalInfo">
                                                                                                </asp:EditCommandColumn>                                                                                                  
                                                                                                <asp:TemplateColumn HeaderText="Delete" ItemStyle-Width="10%">
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CausesValidation="false"
                                                                                                            CommandName="Delete" OnClientClick="return ConformBeforeDelete();"></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:BoundColumn DataField="ExternalRPID" ReadOnly="true" Visible="false"></asp:BoundColumn>
                                                                                                <asp:BoundColumn DataField="ReferringPhysicianID" ReadOnly="true" Visible="false"></asp:BoundColumn>
                                                                                                <asp:BoundColumn DataField="ExternalIDTypeID" ReadOnly="true" Visible="false"></asp:BoundColumn>
                                                                                            </Columns>
                                                                                        </asp:DataGrid>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:Label ForeColor="Green" Font-Size="Small" ID="lblNoRecordsExtInfo" runat="server" Text="No Records available"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostbackTrigger ControlID="ddlIDType" EventName="SelectedIndexChanged" />
                                                                            <asp:AsyncPostbackTrigger ControlID="btnAddExternalID" EventName="Click" />
                                                                            <asp:AsyncPostbackTrigger ControlID="grdIdTypeInfo" />
                                                                        </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%" align="center" border="0" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    <fieldset id="Fieldset1" class="fieldsetCBlue" runat="server" style="font-weight: bold;width: 98%;">
                                                                        <legend class="">Notes</legend>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td style="height: 25px" width="15%">
                                                                                    Notes:</td>
                                                                                <td style="height: 25px" colspan="5">
                                                                                    <asp:TextBox ID="txtNotes" runat="server" Columns="150" MaxLength="500" Rows="3"
                                                                                        TextMode="MultiLine" Style="left: 1px; position: relative; top: 2px" TabIndex="45"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                             </tr>
                                                         </table>
                                                         <table width="99.5%" style="margin-left: 5px; position: relative; top: 0px;" border="0"
                                                                        cellpadding="=0" cellspacing="0">
                                                                        <tr>
                                                                          <td class="Hd2" align="center">
                                                                                <fieldset class="fieldsetCBlue">
                                                                                    <legend class="">OC Notifications</legend>
                                                                                    <table width="100%" align="left" border="0" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table  width="100%" align="left" border="0" cellpadding="=0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <asp:UpdatePanel ID="upnlStep1" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table width="100%" align="center" border="0" cellpadding="1" cellspacing="1">
                                                                                                            <tr>
                                                                                                                <td colspan="11" valign="bottom" class="Step" style="padding: 0 0 0 0px;"></br>
                                                                                                                        <b style="font-size: 11">Step 1: Notification Devices and Events </b><br />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                
                                                                                                                <td style="width: 1%"><br />
                                                                                                                    <asp:Label ID="lblDeviceType" runat="server" Text="Device Type"></asp:Label></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:Label ID="lblGroup" runat="server" Text="Group" Visible="true"></asp:Label></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:Label ID="lblNumAddress" runat="server" Visible="true" Text="Number / Address"></asp:Label></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:Label ID="lblCarrier" runat="server" Visible="true" Text="Carrier"></asp:Label></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:Label ID="lblEmailGateway" runat="server" Visible="true" Text="Email Gateway"></asp:Label></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:Label ID="lblInitialPause" runat="server" Visible="true" Text="Initial Pause"></asp:Label></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:Label ID="lblEvents" runat="server" Visible="true" Text="Events"></asp:Label></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:Label ID="lblFindings" runat="server" Visible="true" Text="Finding"></asp:Label></td>
                                                                                                                <td style="width: 93%" colspan="3">
                                                                                                                    &nbsp;</td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td style="width: 3%">
                                                                                                                    <asp:DropDownList ID="cmbDeviceType" AutoPostBack="true" runat="server" 
                                                                                                                        DataTextField="DeviceDescription" DataValueField="DeviceID"  TabIndex="50"
                                                                                                                        Width="210px" OnSelectedIndexChanged="cmbDeviceType_SelectedIndexChanged">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:DropDownList ID="cmbGroup" AutoPostBack="true" runat="server" 
                                                                                                                                DataValueField="GroupID" DataTextField="GroupName" TabIndex="51"
                                                                                                                                Width="120px" OnSelectedIndexChanged="cmbGroup_SelectedIndexChanged">
                                                                                                                            </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:TextBox ID="txtNumAddress" runat="server" MaxLength="100" Visible="true" TabIndex="52"
                                                                                                                        Width="95px"></asp:TextBox></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:DropDownList ID="cmbCarrier" runat="server" TabIndex="53" AutoPostBack="true"
                                                                                                                                DataTextField="CarrierDescription" DataValueField="CarrierID"
                                                                                                                                Visible="true" Width="100px" OnSelectedIndexChanged="cmbCarrier_SelectedIndexChanged">
                                                                                                                            </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:TextBox ID="txtEmailGateway" runat="server" MaxLength="100" Visible="true" TabIndex="54"
                                                                                                                        Width="115px"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:TextBox ID="txtInitialPause" TabIndex="55" runat="server" Visible="true" Columns="23" MaxLength="5">1</asp:TextBox>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:DropDownList ID="cmbEvents" runat="server" TabIndex="56" Visible="true" Width="70px"
                                                                                                                        DataTextField="EventDescription" DataValueField="RPNotifyEventID">
                                                                                                                    </asp:DropDownList></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:DropDownList ID="cmbFindings" runat="server" TabIndex="57" Visible="true"
                                                                                                                        Width="78px" DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td style="width: 90%">
                                                                                                                    <asp:Button ID="btnShowHideDetails" Text="" CssClass="Frmbutton" Width="95" Visible="false"                                                                                                                    
                                                                                                                                runat="server" TabIndex="58" OnClick="btnShowHideDetails_Click" CausesValidation="false"/>            
                                                                                                                     &#160;
                                                                                                                    <asp:Button ID="btnAddDevice" Text="Add" CssClass="Frmbutton" Width="45" Visible="false"
                                                                                                                                runat="server" TabIndex="58" OnClick="btnAddDevice_Click" CausesValidation="false" />                                                                                                                    
                                                                                                                   
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <input type="hidden" id="hidInitPauseLabel" name="hidInitPauseLabel" runat="server" value="" /><input type="hidden" id="hidDeviceLabel" name="hidDeviceLabel" runat="server" value="" /></td>
                                                                                                                <td>
                                                                                                                    <input type="hidden" id="hidGatewayLabel" name="hidGatewayLabel" runat="server" value="" /></td>
                                                                                                            </tr>
                                                                                                            </table>
                                                                                                            <table width="100%" align="center" border="0" cellpadding="2" cellspacing="2">
                                                                                                            
                                                                                                            <tr>
                                                                                                                <td align="center" valign="top" style="width: 98%; height: 100%;"><br />
                                                                                                                    <input type="hidden" id="hdnOldDeviceName" enableviewstate="true" runat="server" name="textChanged" value="" />
                                                                                                                    <div id="OCDeviceDiv" runat="server" class="TDiv" style="vertical-align: top;" onscroll="document.getElementById(hiddenScrollPos).value=this.scrollTop;">
                                                                                                                        <asp:DataGrid ID="grdDevices" runat="server" CssClass="GridHeader" BorderStyle="None"
                                                                                                                                    DataKeyField="RowID" AutoGenerateColumns="False" 
                                                                                                                                    Width="100%" ItemStyle-Height="21px" HeaderStyle-Height="25" OnDeleteCommand="grdDevices_DeleteCommand"
                                                                                                                                    OnEditCommand="grdDevices_EditCommand" OnCancelCommand="grdDevices_CancelCommand"
                                                                                                                                    OnItemDataBound="grdDevices_ItemDataBound" OnUpdateCommand="grdDevices_UpdateCommand"
                                                                                                                                    OnItemCreated="grdDevices_ItemCreated" TabIndex="59">
                                                                                                                                    <SelectedItemStyle Font-Bold="True" ForeColor="Navy" BackColor="#EFCA98"></SelectedItemStyle>
                                                                                                                                    <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                                                                    <HeaderStyle VerticalAlign="Top" CssClass="THeader" HorizontalAlign="Left" Font-Bold="True">
                                                                                                                                    </HeaderStyle>
                                                                                                                                    <Columns>
                                                                                                                                        <asp:TemplateColumn HeaderText="Device Type" ItemStyle-Width="12%">
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblGridDeviceType" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'
                                                                                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'></asp:Label>
                                                                                                                                            </ItemTemplate>
                                                                                                                                            <EditItemTemplate>
                                                                                                                                                <asp:TextBox ID="txtGridDeviceType" runat="server" MaxLength="50" TabIndex="2" Width="95px" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'
                                                                                                                                                    ValidationGroup="updatePanelEditDevice">                                             
                                                                                                                                                </asp:TextBox>
                                                                                                                                                <asp:RequiredFieldValidator ID="rfvGridDeviceType" runat="server" Display="None"
                                                                                                                                                    ErrorMessage="You Must Enter A Device Name" ControlToValidate="txtGridDeviceType"
                                                                                                                                                    ValidationGroup="updatePanelEditDevice"></asp:RequiredFieldValidator>
                                                                                                                                            </EditItemTemplate>
                                                                                                                                        </asp:TemplateColumn>
                                                                                                                                        <asp:TemplateColumn HeaderText="Group" ItemStyle-Width="13%">
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblGridGroup" runat="server" EnableViewState="true"></asp:Label>
                                                                                                                                            </ItemTemplate>
                                                                                                                                              <EditItemTemplate>
                                                                                                                                                <asp:DropDownList ID="dlistGridGroups" runat="server" TabIndex="6" DataTextField="GroupName"
                                                                                                                                                    DataValueField="GroupID" Width="110px" AutoPostBack="true" OnSelectedIndexChanged="dlistGridGroups_SelectedIndexChanged">
                                                                                                                                                </asp:DropDownList>
                                                                                                                                            </EditItemTemplate>
                                                                                                                                        </asp:TemplateColumn>
                                                                                                                                        <asp:TemplateColumn HeaderText="Number / Address" ItemStyle-Width="15%">
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblGridDeviceNumber" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'
                                                                                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'></asp:Label>
                                                                                                                                            </ItemTemplate>
                                                                                                                                            <EditItemTemplate>
                                                                                                                                                <asp:TextBox ID="txtGridDeviceNumber" runat="server" Width="120px" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceAddress") %>'
                                                                                                                                                    ValidationGroup="updatePanelEditDevice" MaxLength="100"></asp:TextBox>
                                                                                                                                                <asp:RequiredFieldValidator ID="rfvGridDeviceNumber" runat="server" Display="None"
                                                                                                                                                    ErrorMessage="You Must Enter A Device Address / Number" ControlToValidate="txtGridDeviceNumber"
                                                                                                                                                    ValidationGroup="updatePanelEditDevice"></asp:RequiredFieldValidator>
                                                                                                                                            </EditItemTemplate>
                                                                                                                                        </asp:TemplateColumn>
                                                                                                                                        <asp:TemplateColumn HeaderText="Carrier" ItemStyle-Width="13%">
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblGridCarrier" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.Carrier") %>'
                                                                                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Carrier") %>'></asp:Label>
                                                                                                                                            </ItemTemplate>
                                                                                                                                        </asp:TemplateColumn>
                                                                                                                                        <asp:TemplateColumn HeaderText="Email Gateway" ItemStyle-Width="18%">
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblGridDeviceEmailGateway" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'
                                                                                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'></asp:Label>
                                                                                                                                            </ItemTemplate>
                                                                                                                                            <EditItemTemplate>
                                                                                                                                                <asp:TextBox ID="txtGridEmailGateway" runat="server" Width="180px" MaxLength="100" Text='<%# DataBinder.Eval(Container, "DataItem.Gateway") %>'></asp:TextBox>
                                                                                                                                            </EditItemTemplate>
                                                                                                                                        </asp:TemplateColumn>
                                                                                                                                        <asp:TemplateColumn HeaderText="Event" ItemStyle-Width="10%">
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblGridDeviceEvent" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.EventDescription") %>'
                                                                                                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.EventDescription") %>'></asp:Label>
                                                                                                                                            </ItemTemplate>
                                                                                                                                            <EditItemTemplate>
                                                                                                                                                <asp:DropDownList ID="dlistGridEvents" runat="server" TabIndex="6" DataTextField="EventDescription"
                                                                                                                                                    DataValueField="RPNotifyEventID" Width="80px">
                                                                                                                                                </asp:DropDownList>
                                                                                                                                            </EditItemTemplate>
                                                                                                                                        </asp:TemplateColumn>
                                                                                                                                        <asp:TemplateColumn HeaderText="Finding" ItemStyle-Width="12%">
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblGridDeviceFinding" runat="server" EnableViewState="true"></asp:Label>
                                                                                                                                            </ItemTemplate>
                                                                                                                                            <EditItemTemplate>
                                                                                                                                                <asp:DropDownList ID="dlistGridFindings" runat="server"  Width="95px"
                                                                                                                                                    DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                                                                                </asp:DropDownList>
                                                                                                                                            </EditItemTemplate>
                                                                                                                                        </asp:TemplateColumn> 
                                                                                                                                        <asp:TemplateColumn HeaderText="Initial Pause" ItemStyle-Width="12%">
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblGridInitialPause" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.InitialPause") %>'>
                                                                                                                                                </asp:Label>
                                                                                                                                            </ItemTemplate>
                                                                                                                                            <EditItemTemplate>
                                                                                                                                                <asp:TextBox ID="txtGridInitialPause" runat="server" Width="120px" Text='<%# DataBinder.Eval(Container, "DataItem.InitialPause") %>'></asp:TextBox>
                                                                                                                                            </EditItemTemplate>
                                                                                                                                        </asp:TemplateColumn>  
                                                                                                                                        <asp:EditCommandColumn HeaderText="Edit" UpdateText="Update" CancelText="Cancel"
                                                                                                                                            EditText="Edit" CausesValidation="false" ValidationGroup="updatePanelEditDevice"
                                                                                                                                            ItemStyle-Width="10%">
                                                                                                                                            <HeaderStyle Width="5%" />
                                                                                                                                            <ItemStyle Height="23px" />
                                                                                                                                        </asp:EditCommandColumn>
                                                                                                                                        <asp:TemplateColumn HeaderText="Delete" ItemStyle-Width="10%">
                                                                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                                                                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                                                                                            <ItemTemplate>
                                                                                                                                                <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CausesValidation="false" 
                                                                                                                                                    CommandName="Delete" OnClientClick="return ConformBeforeDelete();"></asp:LinkButton>
                                                                                                                                            </ItemTemplate>
                                                                                                                                        </asp:TemplateColumn>
                                                                                                                                        <asp:BoundColumn Visible="False" DataField="DeviceID" ReadOnly="True" HeaderText="MessageID">
                                                                                                                                        </asp:BoundColumn>
                                                                                                                                        <asp:BoundColumn Visible="False" DataField="GroupID" ReadOnly="True" HeaderText="GroupID">
                                                                                                                                        </asp:BoundColumn>
                                                                                                                                        <asp:BoundColumn Visible="False" DataField="FindingID" ReadOnly="True" HeaderText="FindingID">
                                                                                                                                        </asp:BoundColumn>
                                                                                                                                        <asp:BoundColumn Visible="False" DataField="RPNotifyEventID" ReadOnly="True"
                                                                                                                                            HeaderText="NotifyEventID"></asp:BoundColumn>
                                                                                                                                        <asp:BoundColumn Visible="False" DataField="RowID" ReadOnly="True"
                                                                                                                                        HeaderText="Clinical"></asp:BoundColumn>
                                                                                                                                        <asp:BoundColumn Visible="False" DataField="RPID" ReadOnly="True"
                                                                                                                                        HeaderText="RPID"></asp:BoundColumn>
                                                                                                                                        <asp:BoundColumn Visible="False" DataField="RPDeviceID" ReadOnly="True"
                                                                                                                                        HeaderText="RPDeviceID"></asp:BoundColumn>
                                                                                                                                        <asp:BoundColumn Visible="False" DataField="RPNotificationID" ReadOnly="True"
                                                                                                                                        HeaderText="RPNotificationID"></asp:BoundColumn>
                                                                                                                                        
                                                                                                                                    </Columns>
                                                                                                                                    <AlternatingItemStyle CssClass="AltRow" />
                                                                                                                                </asp:DataGrid>
                                                                                                                            </div>
                                                                                                                            <asp:Label ID="lblDeviceAlreadyExists" runat="server" ForeColor="Red" Style="position: relative"></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr align ="center">
                                                                                                                <td  align="center">&nbsp;<asp:Label ForeColor="Green" Font-Size="Small" ID="lblNoRecordsStep1" runat="server" Text="No Records available"></asp:Label></td>
                                                                                                            </tr>
                                                                                                        </table> 
                                                                                
                                                                                                        </ContentTemplate> 
                                                                                                        <Triggers>
                                                                                                            <asp:AsyncPostBackTrigger ControlID="grdDevices" />
                                                                                                            <asp:AsyncPostBackTrigger ControlID="cmbDeviceType" />
                                                                                                            <asp:AsyncPostBackTrigger ControlID="cmbCarrier" />
                                                                                                            <asp:AsyncPostBackTrigger ControlID="cmbGroup" />
                                                                                                        </Triggers>
                                                                                                        </asp:UpdatePanel>    
                                                                                                   </td> 
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td> 
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>&nbsp; 
                                                                                        </td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                        <td>
                                                                                            <table width="100%" align="left" border="0" cellpadding="0" cellspacing="0">
                                                                                                <tr class="Row2">
                                                                                                    <td valign="bottom" class="Step" style="padding: 0 0 0 8px;">
                                                                                                        <b style="font-size: 11">Step 2 (optional): After-Hours Notifications (email/fax devices
                                                                                                            only) </b>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr class="Row2">
                                                                                                    <td valign="bottom" ><br />
                                                                                                    <asp:UpdatePanel ID="upnlStep3" runat="Server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                         <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                           <tr>
                                                                                                               <td>
                                                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                                        <tr>
                                                                                                                            <td width="7%"  style="padding:0 0 0 0px;">
                                                                                                                                <asp:DropDownList ID="cmbAHDevice" runat="server" TabIndex="60"
                                                                                                                                    Width="170px" DataTextField="DeviceName" DataValueField="RowID">
                                                                                                                                </asp:DropDownList>&nbsp;</td>
                                                                                                                            <td style="width:8%;">
                                                                                                                                   <asp:DropDownList ID="cmbStep3Groups" runat="server" Width="170px" DataTextField="GroupName" AutoPostBack="true"
                                                                                                                                       DataValueField="GroupID" OnSelectedIndexChanged = "cmbStep3Groups_SelectedIndexChanged" TabIndex="61" >
                                                                                                                                   </asp:DropDownList>&nbsp;</td>    
                                                                                                                            <td width="5%">
                                                                                                                                <asp:DropDownList ID="cmbAHFindings"  runat="server" 
                                                                                                                                    Width="170px" DataTextField="FindingDescription" DataValueField="FindingID" TabIndex="62">
                                                                                                                                </asp:DropDownList>&nbsp;</td>
                                                                                                                            <td width="0%">
                                                                                                                                <asp:DropDownList ID="cmbAHStartHour" runat="server" 
                                                                                                                                    Width="90px" TabIndex="63" >
                                                                                                                                    <asp:ListItem Value="1">1am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="2">2am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="3">3am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="4">4am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="5">5am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="6">6am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="7">7am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="8">8am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="9">9am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="10">10am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="11">11am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="12" Selected="True">12noon</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="13">1pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="14">2pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="15">3pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="16">4pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="17">5pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="18">6pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="19">7pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="20">8pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="21">9pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="22">10pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="23">11pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="0">12midnight</asp:ListItem>
                                                                                                                                </asp:DropDownList>&nbsp;</td>
                                                                                                                            <td width="0%">
                                                                                                                                <asp:DropDownList ID="cmbAHEndHour" runat="server" TabIndex="64"
                                                                                                                                    Width="90px" DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                                                                    <asp:ListItem Value="1">1am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="2">2am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="3">3am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="4">4am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="5">5am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="6">6am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="7">7am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="8">8am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="9">9am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="10">10am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="11">11am</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="12" Selected="True">12noon</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="13">1pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="14">2pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="15">3pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="16">4pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="17">5pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="18">6pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="19">7pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="20">8pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="21">9pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="22">10pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="23">11pm</asp:ListItem>
                                                                                                                                    <asp:ListItem Value="0">12midnight</asp:ListItem>
                                                                                                                                </asp:DropDownList>&nbsp;</td>
                                                                                                                            <td width="80%">
                                                                                                                                <asp:Button CssClass="Frmbutton" ID="btnAddStep3" runat="server" TabIndex="65"
                                                                                                                                   CausesValidation="False"   Width="45px" Text="Add" OnClick="btnAddStep3_Click" ></asp:Button></td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                           <tr>
                                                                                                                <td class="DivBg" valign="top" align="center"><br />
                                                                                                               
                                                                                                                <div id = "AfterHoursNotificationsDiv" class="TDiv" style="width: 100%; vertical-align:top" >
                                                                                                                 
                                                                                                                    <asp:DataGrid ID="grdAfterHoursNotifications"  runat="server" CssClass="GridHeader" BorderStyle="None" TabIndex="66"
                                                                                                                              AutoGenerateColumns="False" AllowSorting="True" Width="100%" ItemStyle-Height="21" HeaderStyle-Height="25"
                                                                                                                              OnDeleteCommand="grdAfterHoursNotifications_DeleteCommand" OnItemDataBound="grdAfterHoursNotifications_ItemDataBound">
                                                                                                                              <SelectedItemStyle Font-Bold="True" ForeColor="Navy" BackColor="#EFCA98"></SelectedItemStyle>
                                                                                                                              <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                                                              <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True">
                                                                                                                              </HeaderStyle>
                                                                                                                        <Columns>
                                                                                                                            <asp:BoundColumn Visible="False" DataField="AfterHourRowNo" HeaderText="AfterHourRowNo">
                                                                                                                            </asp:BoundColumn>
                                                                                                                            <asp:BoundColumn DataField="DeviceName" HeaderText="Device"></asp:BoundColumn>
                                                                                                                            <asp:BoundColumn DataField="GroupName" HeaderText="Group"></asp:BoundColumn>
                                                                                                                            <asp:BoundColumn DataField="FindingDescription" HeaderText="Finding"></asp:BoundColumn>
                                                                                                                            <asp:BoundColumn DataField="StartHour" HeaderText="Start"></asp:BoundColumn>
                                                                                                                            <asp:BoundColumn DataField="EndHour" HeaderText="End"></asp:BoundColumn>
                                                                                                                            <asp:TemplateColumn HeaderText="Delete" ItemStyle-Width="10%">
                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:LinkButton ID="lnkAHDelete" Text="Delete" runat="server" CausesValidation="false" 
                                                                                                                                        CommandName="Delete" OnClientClick="return ConformBeforeDelete();"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                            </asp:TemplateColumn>
                                                                                                                            <asp:ButtonColumn CommandName="Select" Text="Select" Visible="False"></asp:ButtonColumn>                                                                                                                                                                                                   
                                                                                                                        </Columns>
                                                                                                                        <AlternatingItemStyle CssClass="AltRow" />
                                                                                                                    </asp:DataGrid>
                                                                                                                 
                                                                                                                </div>
                                                                                                               

                                                                                                                    </td>
                                                                                                           </tr>
                                                                                                           <tr>
                                                                                                               <td >&nbsp;</td>
                                                                                                           </tr>
                                                                                                            
                                                                                                            <tr align ="center">
                                                                                                                <td  align="center">&nbsp;<asp:Label ForeColor="Green" Font-Size="Small" ID="lblNoRecordsStep3" runat="server" Text="No Records available"></asp:Label></td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                     </ContentTemplate>
                                                                                                     <Triggers>
                                                                                                        <asp:AsyncPostBackTrigger ControlID="grdAfterHoursNotifications" />
                                                                                                        <asp:AsyncPostBackTrigger ControlID="btnAddStep3" />
                                                                                                     </Triggers>
                                                                                                     </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        &nbsp;</td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                     </tr>
                                                                                    </table> 
                                                                                </fieldset>
                                                                            </td> 
                                                                        </tr>
                                                                    </table> 
                                                        
                                                        <table width="98%" border="0" align="center" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" width="20%">
                                                                    &nbsp;<asp:Label ID="lblError" runat="server" ForeColor="Red" Style="position: relative"
                                                                        Visible="False"></asp:Label></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td align="center" width="30%">
                                                                <asp:UpdatePanel ID="upnlSaveBtn" runat="server" UpdateMode="Always">
                                                                  <ContentTemplate>
                                                                    <asp:Button CssClass="Frmbutton" ID="btnAdd" TabIndex="67" runat="server" Text="Save"
                                                                        OnClick="btnAdd_Click"></asp:Button>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button CssClass="Frmbutton" ID="btnCancel" TabIndex="68" runat="server" Text="Cancel"
                                                                        CausesValidation="False" OnClick="btnCancel_Click"></asp:Button>
                                                                </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                </td>
                                                                <td align="right" width="30%">
                                                                    Last Updated:
                                                                    <asp:Label ID="lblLastUpdated" runat="server"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                    
                                                    <input type="hidden" id="hidEditClicked" value="false" runat="server" />
                                                    <asp:RegularExpressionValidator ID="revEmailFormat" SetFocusOnError="true" runat="server" ControlToValidate="txtEmail"
                                                        ErrorMessage="Email format incorrect" Display="None" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="center">
                                                                <table cellspacing="0" cellpadding="0" width="100%" border="0" align="center">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <br />
                                                                            <fieldset class="fieldsetBlue">
                                                                                <legend runat="server" id="legendDevice">Devices for Clinical Team
                                                                                    <asp:Label ID="lblTeamName" runat="server"></asp:Label></legend>
                                                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td align="center" valign="top" style="width: 100%; height: 100%;">
                                                                                            <div id="UnitDeviceDiv" runat="server" class="TDiv">
                                                                                                 <asp:DataGrid ID="grdCTDevices" runat="server" CssClass="GridHeader" BorderStyle="None"
                                                                                                            ShowHeader="true" DataKeyField="DepartmentDeviceID" AutoGenerateColumns="False"
                                                                                                            AllowSorting="True" Width="100%" ItemStyle-Height="25px" HeaderStyle-Height="25"
                                                                                                            OnItemDataBound="grdCTDevices_ItemDataBound">
                                                                                                            <SelectedItemStyle Font-Bold="True" ForeColor="Navy" BackColor="#EFCA98"></SelectedItemStyle>
                                                                                                            <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                                                            <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                                                                            <Columns>
                                                                                                                <asp:BoundColumn  DataField="DeviceName" ReadOnly="True" HeaderText="Device Type">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn  DataField="GroupName" ReadOnly="True" HeaderText="Group">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn  DataField="DeviceAddress" ReadOnly="True" HeaderText="Number / Address">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn  DataField="Carrier" ReadOnly="True" HeaderText="Carrier">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn  DataField="Gateway" ReadOnly="True" HeaderText="Email Gateway">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn  DataField="EventDescription" ReadOnly="True" HeaderText="Event">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn  DataField="FindingDescription" ReadOnly="True" HeaderText="Finding">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn Visible="False" DataField="DeviceID" ReadOnly="True" HeaderText="MessageID">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn Visible="False" DataField="GroupID" ReadOnly="True" HeaderText="GroupID">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn Visible="False" DataField="FindingID" ReadOnly="True" HeaderText="FindingID">
                                                                                                                </asp:BoundColumn>
                                                                                                                <asp:BoundColumn Visible="False" DataField="DepartmentNotifyEventID" ReadOnly="True"
                                                                                                                    HeaderText="Clinical TeamNotifyEventID"></asp:BoundColumn>
                                                                                                            </Columns>
                                                                                                            <AlternatingItemStyle CssClass="AltRow" />
                                                                                                        </asp:DataGrid>
                                                                                                    <asp:Label ForeColor="Green" Font-Size="Small" ID="lblNoRecords" runat="server" Text="No Records available"></asp:Label>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td valign="top">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    
                                                    <asp:RequiredFieldValidator ID="rfvFirstName" SetFocusOnError="true" Style="z-index: 101;
                                                        left: 438px; position: absolute; top: 281px" runat="server" Display="None" ErrorMessage="You Must Enter A First Name"
                                                        ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                                                    &nbsp;
                                                    <asp:RequiredFieldValidator ID="rfvPrimaryPhonePrefix" SetFocusOnError="true" Style="z-index: 108;
                                                        left: 438px; position: absolute; top: 372px" runat="server" Display="None" ErrorMessage="You Must Enter A Primary Phone Prefix"
                                                        ControlToValidate="txtPrimaryPhonePrefix"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvPrimaryPhoneAreaCode" SetFocusOnError="true" Style="z-index: 105;
                                                        left: 397px; position: absolute; top: 338px" runat="server" Display="None" ErrorMessage="You Must Enter A Primary Phone Area Code"
                                                        ControlToValidate="txtPrimaryPhoneAreaCode"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvPrimaryPhoneNNNN" SetFocusOnError="true" Style="z-index: 109;
                                                        left: 438px; position: absolute; top: 404px" runat="server" Display="None" ErrorMessage="You Must Enter A Primary Phone Extension"
                                                        ControlToValidate="txtPrimaryPhoneNNNN"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvLastName" SetFocusOnError="true" Style="z-index: 103;
                                                        left: 442px; position: absolute; top: 314px" runat="server" Display="None" ErrorMessage="You Must Enter A Last Name"
                                                        ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revPhonePrefix" SetFocusOnError="true" runat="server"
                                                        Display="None" ErrorMessage="Please enter valid Phone prefix." ValidationExpression="\d{3}"
                                                        ControlToValidate="txtPrimaryPhonePrefix" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revPhoneNNNN" SetFocusOnError="true" runat="server"
                                                        Display="None" ErrorMessage="Please enter valid Phone extension." ValidationExpression="\d{4}"
                                                        ControlToValidate="txtPrimaryPhoneNNNN" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revPhoneAreaCode" SetFocusOnError="true" runat="server"
                                                        Display="None" ErrorMessage="Please enter valid Phone area code." ControlToValidate="txtPrimaryPhoneAreaCode"
                                                        ValidationExpression="\d{3}" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revFaxPrefix" SetFocusOnError="true" runat="server"
                                                        Display="None" ErrorMessage="Please enter valid Fax prefix." ValidationExpression="\d{3}"
                                                        ControlToValidate="txtFaxPrefix" ForeColor="White"></asp:RegularExpressionValidator>&nbsp;
                                                    <asp:RegularExpressionValidator ID="revFaxNNNN" SetFocusOnError="true" runat="server"
                                                        Display="None" ErrorMessage="Please enter valid Fax extension." ValidationExpression="\d{4}"
                                                        ControlToValidate="txtFaxNNNN" ForeColor="White"></asp:RegularExpressionValidator>&nbsp;
                                                    <asp:RegularExpressionValidator ID="revFaxAreaCode" SetFocusOnError="true" runat="server"
                                                        Display="None" ErrorMessage="Please enter valid Fax area code." ControlToValidate="txtFaxAreaCode"
                                                        ValidationExpression="\d{3}" Width="241px" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revZip" SetFocusOnError="true" Style="z-index: 106;
                                                        left: 821px; position: absolute; top: 235px" runat="server" ControlToValidate="txtZip"
                                                        ErrorMessage="Zip Code Incorrect" Display="None" ValidationExpression="\d{5}(-\d{4})?"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revAddPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber"
                                                        Display="None" ErrorMessage="Please enter valid Additional Phone number." SetFocusOnError="true"
                                                        ValidationExpression="\d{4}"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revAddPhoneCode" runat="server" ControlToValidate="txtPhoneCode"
                                                        Display="None" ErrorMessage="Please enter valid Additional Phone code." SetFocusOnError="true"
                                                        ValidationExpression="\d{3}"></asp:RegularExpressionValidator><br>
                                                    <asp:RegularExpressionValidator ID="revAddPhonePrefix" runat="server" ControlToValidate="txtPhonePrefix"
                                                        Display="None" ErrorMessage="Please enter valid Additional Phone prefix." SetFocusOnError="true"
                                                        ValidationExpression="\d{3}"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revFirstName" runat="server" ControlToValidate="txtFirstName"
                                                        Display="None" ErrorMessage="Please Enter Valid First Name." SetFocusOnError="true"
                                                        ValidationExpression="[\dA-Za-z.' ]*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revLastName" runat="server" ControlToValidate="txtLastName"
                                                        Display="None" ErrorMessage="Please Enter Valid Last Name." SetFocusOnError="true"
                                                        ValidationExpression="[\dA-Za-z.' ]*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revNickName" runat="server" ControlToValidate="txtNickname"
                                                        Display="None" ErrorMessage="Please Enter Valid Nick Name." SetFocusOnError="true"
                                                        ValidationExpression="[\dA-Za-z.' ]*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtName"
                                                        Display="None" ErrorMessage="Please Enter Valid Name." SetFocusOnError="true"
                                                        ValidationExpression="[\dA-Za-z.' ]*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revSpecialty" runat="server" ControlToValidate="txtSpecialty"
                                                        Display="None" ErrorMessage="Please Enter Valid Speciality." SetFocusOnError="true"
                                                        ValidationExpression="[\dA-Za-z.' ]*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revHospitalAffiliation" runat="server" ControlToValidate="txtHospitalAffiliation"
                                                        Display="None" ErrorMessage="Please Enter Valid Affiliation." SetFocusOnError="true"
                                                        ValidationExpression="[\dA-Za-z.' ]*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revCellAreaCode" runat="server" Display="None"
                                                        ErrorMessage="Please Enter Valid Cell Phone area code." ControlToValidate="txtCellAreaCode" SetFocusOnError="true"
                                                        ValidationExpression="\d{3}" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revCellPrefix" runat="server" Display="None" ErrorMessage="Please Enter Valid Cell Phone prefix."
                                                        ValidationExpression="\d{3}" ControlToValidate="txtCellPrefix" ForeColor="White" SetFocusOnError="true"></asp:RegularExpressionValidator>&nbsp;
                                                    <asp:RegularExpressionValidator ID="revCellNNNN" runat="server" Display="None" ErrorMessage="Please Enter Valid Cell Phone extension."
                                                        ValidationExpression="\d{4}" ControlToValidate="txtCellNNNN" ForeColor="White" SetFocusOnError="true"></asp:RegularExpressionValidator>&nbsp;
                                                    <asp:CustomValidator ID="ctmCellPhone" runat="server" ControlToValidate="txtPrimaryPhonePrefix" ClientValidationFunction="checkCellPhone" Display="None" SetFocusOnError="false"></asp:CustomValidator>
    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel id="upnlHidden" runat="Server" UpdateMode="Always">
                                                    <contenttemplate>
                                                        <asp:ValidationSummary ID="ValidationSummary1"  runat="server" ShowSummary="False" ShowMessageBox="True">
                                                        </asp:ValidationSummary>
                                                        <input type="hidden" id="textChanged" enableviewstate="true" runat="server" name="textChanged"
                                                            value="false" />
                                                        <input type="hidden" id="txtChanged" runat="server" name="txtChanged" value="false" />
                                                        <input type="hidden" id="hdnIsAddClicked" runat="server" name="hdnIsAddClicked" value="false" />
                                                        <input type="hidden" id="hdnGridChanged" enableviewstate="true" runat="server" name="hdnGridChanged"
                                                            value="false" />
                                                    </contenttemplate>
                                                    </asp:UpdatePanel>
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div> 
                    </td>
                </tr> 
            </table>
                                    </td>
                                </tr>
                            </table>
                        </div> 
                    </td>
                </tr> 
            </table>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
