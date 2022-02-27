<%@ Page Language="c#" Inherits="Vocada.CSTools.AddRP" CodeFile="add_rp.aspx.cs"
    MaintainScrollPositionOnPostback="true" Title="CSTools: Add Ordering Clinician"
    MasterPageFile="~/cs_tool.master" SmartNavigation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMessageList" runat="server" Updatemode="Conditional">
        <contenttemplate>

            <script language="JavaScript" type="text/javascript" src="./Javascript/CalendarPopup.js"></script>

            <script language="JavaScript" type="text/javascript" src="./Javascript/calendar.js"></script>

            <script language="JavaScript" type="text/javascript">document.write(getCalendarStyles());</script>

            <script language="javascript" type="text/javascript" src="Javascript/Common.js"></script>

            <script language="javascript" type="text/javascript">
            
            // Update label of start Date label control on change of department combo
             function UpdateLabel()
                {
                   
                    var cmbDept = document.getElementById(cmbDepartmentClientID);
                    var lblStar = document.getElementById(lblStarClientID);
                    if (cmbDept.value == "-1")
                        {lblStar.innerText=":";}
                    else
                         {lblStar.innerText="*:";}
                }    

             //Sets the flag textChanged to true if the text of any textbox is changed.
                function UpdateProfile()
                {
                   document.getElementById(textChangedClientID).value = "true";
                }
                
               //Redirects user to the given URL as the Response.Redirect doesn't works sometime.
                function Navigate(url)
                {
                    try
                    {
                        url = url.replace("#**#%","'");
                        window.location.href = url;
                    }
                    catch(_error)
                    {
                        return;
                    }
                }
                // Check Maximum length for textbox if length > given then consider first 255 char only
                function CheckMaxLength(controlId, length)
                {
                    var text = document.getElementById(controlId).value;
                    if(text.length > length)
                    {
                        document.getElementById(controlId).value = text.substring(0,length);
                    }        
                }    
                //Sets the flag textChanged to false when user clicks on Save button as it should not ask confirmation message even
                //if user clicks on Save button. 
                function ChangeFlag()
                {        
                    document.getElementById(textChangedClientID).value = "false";
                }
                
                //Enable disable Department combo on Resident checkbox state
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
                
                function validation()
                {
                    
                    var errormsg = "";
                    var setfocus = "";
                    var regxName = /[\dA-Za-z.' ]*/ ;
                    var regxEmail = /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
                    var regxPin = /\d{4,10}/;
                    var regxLogin = /(\w)*(\d)*/;
                    var regxLoginLength = /[\w\d]{2,10}/
                    var regNm = /\d{10}/; 
                    var fname = trim(document.getElementById(txtFirstNameClientID).value);
                    if (fname.length == 0)
                    {
                       errormsg = "- You must enter First Name\n"; 
                       setfocus = document.getElementById(txtFirstNameClientID);
                    }                    
                    else if (regxName.test(fname)== false )
                    {
                        errormsg = "- Please enter valid First Name \n"; 
                        setfocus = document.getElementById(txtFirstNameClientID);
                    }
                    var lname = trim(document.getElementById(txtLastNameClientID).value);
                    if (lname.length == 0)
                    {
                       errormsg += "- You must enter Last Name \n"; 
                       if (setfocus == "")
                        setfocus = document.getElementById(txtLastNameClientID);
                    }
                    else if (regxName.test(lname)==false)
                    {
                        errormsg += "- Please enter valid Last Name \n"; 
                        if (setfocus == "")
                            setfocus = document.getElementById(txtLastNameClientID); 
                    }
                    var nickname = trim(document.getElementById(txtNicknameClientID).value);
                    if (nickname.length > 0 && regxName.test(nickname) == false )
                    {
                        errormsg += "- Please enter valid Nick Name \n"; 
                        if (setfocus == "")
                            setfocus = document.getElementById(txtNicknameClientID);
                    }
                    var officePhoen = document.getElementById(txtPrimaryPhoneAreaCodeClientID).value + document.getElementById(txtPrimaryPhonePrefixClientID).value + document.getElementById(txtPrimaryPhoneNNNNClientID).value 
                    if (officePhoen.length == 0)
                    {
                        errormsg += "- You must enter Office Phone \n"; 
                        if (setfocus == "")
                            setfocus = document.getElementById(txtPrimaryPhoneAreaCodeClientID);
                    }
                    else if (officePhoen.length <10 || regNm.test(officePhoen)==false)
                    {
                        errormsg += "- Please enter valid Office Phone \n"; 
                        if (setfocus == "")
                            setfocus = document.getElementById(txtPrimaryPhoneAreaCodeClientID);
                    }
                    
                    var txtCell1 = document.getElementById(txtCellAreaCodeClientID).value;
                    var txtCell2 = document.getElementById(txtCellPrefixClientID).value;
                    var txtCell3 = document.getElementById(txtCellNNNNClientID).value; 
                    cellLength = txtCell1.length + txtCell2.length + txtCell3.length;
                   
                    if (cellLength >0 && !((cellLength == 10) && (regNm.test(txtCell1+txtCell2+txtCell3)==true)))
                    {
                       errormsg += "- Please enter valid Cell Phone \n";
                       if (setfocus == "")
                            setfocus = document.getElementById(txtCellAreaCodeClientID);     
                    }
                    var chkEd = document.getElementById(chkEDDocClientID);
                    if (chkEd != null)
                    {
                        if (chkEd.checked)
                        {
                             var login = trim(document.getElementById(txtLoginIDClientID).value);
                             var pin = trim(document.getElementById(txtPasswordClientID).value);
                             if (login == "")
                             {
                                errormsg += "- You must enter Login ID \n";
                                 if (setfocus == "")
                                    setfocus = document.getElementById(txtLoginIDClientID);
                             } 
                             else if(regxLoginLength.test(login)== false )
                             {
                                    errormsg += "- Login ID must be 2 - 10 characters / digits \n";
                                 if (setfocus == "")
                                    setfocus = document.getElementById(txtLoginIDClientID);
                             }
                             else if (regxLogin.test(login) == false )
                             {
                                errormsg += "- Please enter valid Login \n"; 
                                 if (setfocus == "")
                                    setfocus = document.getElementById(txtLoginIDClientID);
                             } 
                             pin = trim(pin);
                             if (pin == "")
                             {
                                 errormsg += "- You must enter PIN \n"; 
                                 if (setfocus == "")
                                    setfocus = document.getElementById(txtPasswordClientID);
                             }
                             else if (!((pin.length >=4 && pin.length <=10) && !isNaN(pin)))
                             {
                                errormsg += "- PIN must be 4-10 digits \n"; 
                                 if (setfocus == "")
                                    setfocus = document.getElementById(txtPasswordClientID);
                             }
                        }
                    }
                    
                    var pinForMessages = document.getElementById(txtPinForMessageClientID);                    
                    if (pinForMessages != null)
                    {
                        var pin = trim(pinForMessages.value);
                        if(pin.length > 0 && !((pin.length >=4 && pin.length <=5) && !isNaN(pin)))
                        {
                                    errormsg += "- PIN for Message Retrieval must be 4-5 digits \n"; 
                                     if (setfocus == "")
                                        setfocus = document.getElementById(txtPinForMessageClientID);                    
                        }
                    }
                    var aName = trim(document.getElementById(txtNameClientID).value);
                    if (aName.length > 0 && regxName.test(aName) == false )
                    {
                        errormsg += "- Please enter valid Name \n"; 
                        if (setfocus == "")
                            setfocus = document.getElementById(txtNameClientID);
                    }
                    var AddPhone1 = document.getElementById(txtAddPhoneAreaCodeClientID).value;
                    var AddPhone2 = document.getElementById(txtAddPhonePrefixClientID).value;
                    var AddPhone3 = document.getElementById(txtAddPhoneNumberClientID).value; 
                    AddPhoneLength = AddPhone1.length + AddPhone2.length + AddPhone3.length;
                    if ((AddPhoneLength >0 && AddPhoneLength != 10) || (AddPhoneLength >0 && regNm.test(AddPhone1+AddPhone2+AddPhone3)==false))
                    {
                       errormsg += "- Please enter valid Additional Phone number \n";
                       if (setfocus == "")
                            setfocus = document.getElementById(txtAddPhoneAreaCodeClientID);     
                    }
                    var pEmail =  trim(document.getElementById(txtEmailClientID).value);
                    var fax1 = document.getElementById(txtFaxAreaCodeClientID).value;
                    var fax2 = document.getElementById(txtFaxPrefixClientID).value;
                    var fax3 = document.getElementById(txtFaxNNNNClientID).value;
                    var fax = fax1 + fax2 + fax3;
                    if (pEmail.length == 0 && fax1.length == 0 && fax2.length == 0 && fax3.length == 0)
                    {
                         errormsg += "- You must enter either an Email Address or a Fax Number for Notifications \n"; 
                             if (setfocus == "")
                                setfocus = document.getElementById(txtEmailClientID);
                    }
                    else
                    {
                        if (pEmail.length > 0 && regxEmail.test(pEmail) == false)
                        {
                          errormsg += "- Please enter valid Email \n";
                          if (setfocus == "")
                            setfocus = document.getElementById(txtEmailClientID);     
                        }
                        if ((fax.length >0 && fax.length != 10) || (fax.length >0 && regNm.test(fax)==false))
                        {
                           errormsg += "- Please enter valid Fax \n";
                           if (setfocus == "")
                            setfocus = document.getElementById(txtFaxAreaCodeClientID);      
                        }
                    }
                    var specialty = trim(document.getElementById(txtSpecialtyClientID).value);
                    if (specialty.length > 0 && regxName.test(specialty) == false )
                    {
                        errormsg += "- Please enter valid Speciality \n"; 
                        if (setfocus == "")
                            setfocus = document.getElementById(txtSpecialtyClientID);
                    }
                    var affiliation = trim(document.getElementById(txtHospitalAffiliationClientID).value);
                    if (affiliation.length > 0 && regxName.test(affiliation) == false )
                    {
                        errormsg += "- Please enter valid Hospital Affiliation \n"; 
                        if (setfocus == "")
                            setfocus = document.getElementById(txtHospitalAffiliationClientID);
                    }
                    if (errormsg != "")
                    {
                        alert(errormsg);
                        setfocus.focus();
                        return false;
                    }
                    return true;
                }
                /*Enable Disable Start date end date as per department selected.*/
                 function enableDateSelection()
                {
                    var cmbDept = document.getElementById(cmbDepartmentClientID);
                    var startDate = document.getElementById(anchFromDateClientID);
                    var endDate = document.getElementById(anchToDateClientID);
                    var startDateText = document.getElementById(txtStartDateClientID);
                    var endDateText = document.getElementById(txtEndDateClientID);
                    if(cmbDept != null)
                    {
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
                            startDate.onClick = "javascript:calRPStartDate.select(document.all['" + txtStartDateClientID + "'],document.all['" + anchFromDateClientID + "'],'MM/dd/yyyy');";
                            endDate.onClick = "javascript:calRPEndDate.select(document.all['" + txtEndDateClientID + "'],document.all['" + anchToDateClientID + "'],'MM/dd/yyyy');";
                            startDateText.disabled = false;
                            endDateText.disabled = false;
                        }
                    }
                }
                var mapId = "add_OC.aspx";
                
                  
              var calRPStartDate = new CalendarPopup("divAddRPGrdStartDate");
              CP_setControlAdjustments(0, 90);
              document.write(getCalendarStyles());
              calRPStartDate.setYearSelectStartOffset(50);
              calRPStartDate.showYearNavigation();
              calRPStartDate.showNavigationDropdowns();
              
              var calRPEndDate = new CalendarPopup("divAddRPGrdEndDate"); 
              CP_setControlAdjustments(0, 90);
              document.write(getCalendarStyles());
              calRPEndDate.setYearSelectStartOffset(50);
              calRPEndDate.showYearNavigation();
              calRPEndDate.showNavigationDropdowns();
              
              
                                          
              
              //scripts for  Notification Devices
              var otherPostback = false;
              /*This function is used to validate the Device address and the Carrier before adding the device*/
                function validateDevices(ddlDevices,txtDeviceAddress,ddlCarrier,txtGateway,ddlFindings, ddlGroup, txtInitialPause)
                {    
                    var setfocus = "" ;
                    var device = document.getElementById(ddlDevices).value;
                    var grouplist = (document.getElementById(ddlGroup) == null) ? " " : document.getElementById(ddlGroup).value;
                   if (device == null || device == -1)
                   {
                        alert("Device is incorrect.");
                        return false;
                   }
                   if (grouplist == null || grouplist.length == 0)
                   {
                        alert("Group is not available.");
                        return false;
                   }
                  
                   if(device == "1" && document.getElementById(txtGateway) != null && (document.getElementById(txtGateway).value.indexOf('Enter') > -1 || trim(document.getElementById(txtGateway).value) == ""))
                   {
                        alert("Device address is incorrect.");
                        return false;
                   }
                   var val = null;
                   if (document.getElementById(txtDeviceAddress) != null)
                        val = trim(document.getElementById(txtDeviceAddress).value);   
                   var val1 = null;
                   if (document.getElementById(ddlCarrier) != null)
                        val1 = trim(document.getElementById(ddlCarrier).value);
                   
                    if(device == "11" || device == "12" || device == "13" || device == "14")
                    {
                        var deviceAdd = trim(document.getElementById(txtDeviceAddress).value);
                        var lengthDeviceAdd = deviceAdd.length;
                        if(lengthDeviceAdd != 10)
                        {
                            alert("Please enter valid Device Address.");
                            document.getElementById(txtDeviceAddress).focus();
                            return false;
                        }
                    }
                   
                   if ((val != null && (val == "Enter Cell # (numbers only)" || val == "Enter Email Address" || val == "Enter Fax # (numbers only)"  || val == "Enter Pager # (numbers only)" || val == "Enter Pager # + PIN (numbers only)" || val == "")) && (val1 != null && val1 == -1 ))
                   {
                        alert("Device information is incorrect.");
                        return false;
                   }
                   if ((val == "Enter Cell # (numbers only)" || val == "Enter Email Address" || val == "Enter Fax # (numbers only)"  || val == "Enter Pager # (numbers only)" || val == "Enter Pager # + PIN (numbers only)" || val == "") && val != null)
                   {        
                        alert("Device address is incorrect.");
                        return false;
                   }
                   else if (val1 == -1 && val1 != null)
                   {
                        alert("Carrier is incorrect.");
                        return false;
                   }
                   var val2 = null;
                   if (document.getElementById(txtInitialPause) != null)
                        val2 = trim(document.getElementById(txtInitialPause).value);   
                            
                   
                    if(device == "14")
                    {
                        if(val2 != null)
                        {
                            if(val2 == '')
                            {
                                alert("Initial pause time is incorrect");
                                return false;
                            }
                            else
                            {
                            
                                 var regNumericValidate = /^(\d{0,5})(\.\d\d?)?$/;
                
                                if(!regNumericValidate.test(val2))
                                {
                                   alert("Initial pause time is incorrect");
                                   document.getElementById(txtInitialPause).focus();
                                   return false;
                                }
                                
                                if (val2.indexOf(".") == val2.lastIndexOf(".")) 
                                {
                                    if(val2 < 1 || val2 > 30.99)
                                    {
                                        alert("Initial pause time should be betweeen 1 to 30.99");
                                        document.getElementById(txtInitialPause).focus();
                                        return false;
                                    }
                                    if (val2.length > 5)
                                    {
                                        alert("Initial pause time is incorrect");
                                        document.getElementById(txtInitialPause).focus();
                                        return false;
                                    }
                                }
                                else
                                {
                                    alert("Initial pause time is incorrect");
                                    document.getElementById(txtInitialPause).focus();
                                    return false;
                                }
                            }
                        }
                    }
                }

                /* This function will remove the text that asks to Enter device in the Device Address textbox when the user clicks on it */
                function RemoveDeviceLabel(txtDeviceAddress,hidDeviceLabel)
                {
                   if(document.getElementById(txtDeviceAddress).value.indexOf('Enter') == 0)
                   {
                     document.getElementById(hidDeviceLabel).value = document.getElementById(txtDeviceAddress).value;
                     document.getElementById(txtDeviceAddress).value = "";
                   }
                }

                /* This function will remove the text that asks to Enter gateway in the Gateway textbox when the user clicks on it */
                function RemoveGatewayLabel(txtGateway,hidGatewayLabel)
                {
                   if(document.getElementById(txtGateway).value.indexOf('Enter') == 0)
                   {
                     document.getElementById(hidGatewayLabel).value = document.getElementById(txtGateway).value;
                     document.getElementById(txtGateway).value = "";
                   }
                }

                /* This function will remove the text that asks to Enter device in the Initial Pause textbox when the user clicks on it */
                function RemoveInitialPauseLabel(txtInitialPause, hidInitPauseLabel)
                { 
                   if(document.getElementById(txtInitialPause).value.indexOf('Value') == 0)
                   {
                     document.getElementById(hidInitPauseLabel).value = document.getElementById(txtInitialPause).value;
                     document.getElementById(txtInitialPause).value = "1";
                   }
                }

                /* For the device type Skytel / Pager USA it should allow space at 10 th index*/
                function isNumericKeyOrSpace(txtDeviceAddress)
                {
                    if(document.getElementById(txtDeviceAddress).value.length == 10)
                    {
                        return isSpace();        
                    }  
                    else
                        return isNumericKeyStrokes(textChangedClientID);                
                } 
                
                /*For pager devices which will be accepting space at any position */
                function PagerValidationWithSpace(controlID)
                {   
                    if(document.getElementById(controlID).value.length > 0 && (!IsTextContainsSpace(document.getElementById(controlID).value)))
                    {
                      if(isSpace())
                        return true;
                      else
                        return isNumericKeyStrokes();        
                    }
                    else
                    {            
                        return isNumericKeyStrokes();
                    }
                    
                }

                /*Function to know the number of spaces entered for the pager device */
                function IsTextContainsSpace(controlText)
                {   
                    var count = 0;
                    if(controlText.length > 0)
                    {
                        for(var i = 0; i < controlText.length; i++)
                        {
                            if(controlText.charAt(i)==" ")
                             count++;            
                        }
                    }
                    if(count >= 1)
                        return true;
                    else 
                        return false;    
                }
                                
                            
                 /*To Get Confirmation before Delete.*/
                 function ConformBeforeDelete()
                 {
                    if(confirm('Are you sure you want to delete this record?' ))
                    {
                        ChangeFlagforGrid();
                        otherPostback =true;
                        return true;
                    }
                    otherPostback =false;
                    return false;          
                 }
                 //Sets Postback Variable value to True
                function SetPostbackVarTrue()
                {
                    otherPostback = true;
                }
                
                //Sets Postback Variable value to False
                function SetPostbackVarFalse()
                {
                    otherPostback = false;
                }
               //Sets the flag textChanged to false when user clicks on Save button as it should not ask confirmation message even
                //if user clicks on Save button. 
                function ChangeFlagforGrid()
                {        
                    var flag = document.getElementById(textChangedClientID).value
                    if (flag == "true")
                    {
                        document.getElementById(textChangedClientID).value = "false";
                        document.getElementById(hdnGridChangedClientID).value = "true";
                    }
                    return true;
                }
                
                //This function is used to set flag true on click of add button
                function AddRecordFromGrid()
                {
                    try
                    {            
                        document.getElementById(hdnIsAddClickedClientID).value = 'true';
                        return ChangeFlagforGrid();            
                        
                    }
                    catch(e){}
                }
                
                function chnangeGridFlag()
                {
                        document.getElementById(textChangedClientID).value = "true";
                        document.getElementById(hdnGridChangedClientID).value = "false";
                       // return;
                }
            </script>

            <input type="hidden" id="scrollPos" value="0" runat="server" />

            <table align="center" border="0" cellpadding="0" cellspacing="0" style="width:98%;height:99%">
                <tr class="ContentBg">
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                           
                            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                <tr>
                                    <td width="62%" class="Hd1">
                                        Add Ordering Clinician Information</td>
                                </tr>
                            </table>
                            <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                <tr>
                                    <td >
                                        <asp:Table ID="tblSelect" runat="server" CellSpacing="0" CellPadding="0" Width="99%"
                                            Style="border: 0px;">
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <table width="99.5%" align="center" height="82%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr class="BottomBg">
                                                            <td valign="top">
                                                                <fieldset class="fieldsetCBlue" style="width:100%">
                                                                    <legend class="">Select</legend>
                                                                    <table width="70%" align="center" border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td style="white-space: nowrap; width: 10%;">
                                                                                Institution Name:&nbsp;</td>
                                                                            <td style="width: 25%;">
                                                                                <asp:DropDownList runat="server" ID="drpInstitutions" AutoPostBack="true" OnSelectedIndexChanged="drpInstitutions_SelectedIndexChanged"
                                                                                    Width="250px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td style="width: 10%;">
                                                                                &nbsp;&nbsp;Directory:&nbsp;</td>
                                                                            <td style="width: 25%;">
                                                                                <asp:UpdatePanel ID="upnlDirectory" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:DropDownList runat="server" ID="drpDirectories" AutoPostBack="true" OnSelectedIndexChanged="drpDirectories_SelectedIndexChanged"
                                                                                            Width="250px">
                                                                                        </asp:DropDownList>
                                                                                    </ContentTemplate>
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="drpInstitutions" EventName="SelectedIndexChanged" />
                                                                                    </Triggers>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <asp:Table ID="tblInformation" runat="server" CellSpacing="0" CellPadding="0" Width="100%"
                                            Style="border: 0px;">
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <table width="99.5%" align="center" height="82%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr class="BottomBg">
                                                            <td valign="top">
                                                                
                                                                <br />
                                                                <fieldset class="fieldsetCBlue" style="height: 98%;" >
                                                                    <legend class="">Contact Information</legend>
                                                                    <br />
                                                                    <table width="99.5%" style="margin-left: 5px;" border="0" cellpadding="=0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="Hd2" align="center">
                                                                                <fieldset class="fieldsetCBlue">
                                                                                    <legend class="">Name/Contact Info</legend>
                                                                                    <table width="90%" align="center" border="0" cellpadding="4" cellspacing="2">
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                First Name*:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtFirstName" TabIndex="1" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                                            <td width="15%" align="left">
                                                                                                Last Name*:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtLastName" TabIndex="2" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="15%" align="left">
                                                                                                Nick Name:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtNickname" TabIndex="3" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                                            <td width="15%" align="left">
                                                                                                Office Phone*:</td>
                                                                                            <td width="35%" align="left">
                                                                                                (<asp:TextBox ID="txtPrimaryPhoneAreaCode" TabIndex="4" Columns="4" runat="server"
                                                                                                    MaxLength="3"></asp:TextBox>)
                                                                                                <asp:TextBox ID="txtPrimaryPhonePrefix" TabIndex="5" runat="server" Columns="4" MaxLength="3"></asp:TextBox>-
                                                                                                <asp:TextBox ID="txtPrimaryPhoneNNNN" TabIndex="6" runat="server" Columns="6" MaxLength="4"></asp:TextBox></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="15%" align="left">
                                                                                                Cell Phone:</td>
                                                                                            <td width="35%" align="left">
                                                                                                (<asp:TextBox ID="txtCellAreaCode" TabIndex="7" Columns="4" runat="server"
                                                                                                    MaxLength="3"></asp:TextBox>)
                                                                                                <asp:TextBox ID="txtCellPrefix" TabIndex="8" runat="server" Columns="4" MaxLength="3"></asp:TextBox>-
                                                                                                <asp:TextBox ID="txtCellNNNN" TabIndex="9" runat="server" Columns="6" MaxLength="4"></asp:TextBox></td>
                                                                                            <td colspan="2">&nbsp;</td>
                                                                                        </tr>
                                                                                    </table>                                                                                    
                                                                                </fieldset>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table width="99.5%" style="margin-left: 5px;" border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td class="Hd2" align="center">
                                                                                                <fieldset id="fldLoginDetails" class="fieldsetCBlue" runat="server">
                                                                                                    <legend class="">Login Details</legend>
                                                                                                    <table width="100%" border="0" align="left" cellpadding="4" cellspacing="2">
                                                                                                        <tr>
                                                                                                            <asp:Panel ID="pnlEDDoc" runat="server" Visible="false">
                                                                                                                <td width="1%">
                                                                                                                    &nbsp;</td>
                                                                                                                <td width="17%" align="center">
                                                                                                                    ED Doc:</td>
                                                                                                                <td width="33%" align="left">
                                                                                                                    <asp:CheckBox ID="chkEDDoc" TabIndex="10" Checked="false" AutoPostBack="true" runat="server"
                                                                                                                        Visible="true" TextAlign="Right" Style="position: relative" OnCheckedChanged="chkEDDoc_CheckedChanged" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    &nbsp;</td>
                                                                                                                <td>
                                                                                                                    &nbsp;</td>
                                                                                                            </asp:Panel>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Panel ID="pnlOCLogin" runat="server" Visible="false">
                                                                                                                    <td width="17%" align="center">
                                                                                                                        Login ID*:</td>
                                                                                                                    <td width="33%">
                                                                                                                        <asp:TextBox ID="txtLoginID" TabIndex="11" runat="server" Columns="10" MaxLength="10"></asp:TextBox>&nbsp;(10
                                                                                                                        characters)
                                                                                                                    </td>
                                                                                                                    <td width="12%">
                                                                                                                        PIN*:</td>
                                                                                                                    <td width="38%">
                                                                                                                        <asp:TextBox ID="txtPassword" TabIndex="12" runat="server" Columns="10" MaxLength="10"></asp:TextBox>&nbsp;(Max
                                                                                                                        10 digits)
                                                                                                                        <asp:Button ID="btnGeneratePassword" runat="server" CssClass="Frmbutton" Width="84px"
                                                                                                                            CausesValidation="False" Text="Generate PIN" OnClick="btnGeneratePassword_Click"
                                                                                                                            UseSubmitBehavior="false" TabIndex="13" onKeyDown="if(event.keyCode==13) return false;">
                                                                                                                        </asp:Button>
                                                                                                                    </td>
                                                                                                                </asp:Panel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <asp:Panel ID="pnlMessageRetieve" runat="server" Visible="false">
                                                                                                            <td>                                                                                                               
                                                                                                            <td width="17%">
                                                                                                                PIN for Message Retrieval:</td>
                                                                                                            <td width="33%">
                                                                                                                <asp:TextBox ID="txtPinForMessage" TabIndex="14" runat="server" Columns="5" MaxLength="5"></asp:TextBox>&nbsp;(4
                                                                                                                - 5 digits)
                                                                                                                <asp:Button ID="btnPinForMessage" runat="server" CssClass="Frmbutton" Width="84px"
                                                                                                                    CausesValidation="False" Text="Generate PIN" OnClick="btnPinForMessage_Click"
                                                                                                                    UseSubmitBehavior="false" TabIndex="15" onKeyDown="if(event.keyCode==13) return false;">
                                                                                                                </asp:Button>
                                                                                                            </td>                                                                                                                                                                                                                        
                                                                                                            <td width="12%">
                                                                                                                &nbsp;</td>
                                                                                                            <td width="38%">
                                                                                                                &nbsp;</td>
                                                                                                                </td>
                                                                                                           </asp:Panel>                                                                                                                
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
                                                                                    <legend class="">Additional Information</legend>
                                                                                    <table width="90%" align="center" border="0" cellpadding="4" cellspacing="2">
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                Radiology TDR:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:CheckBox ID="chkRadiologyTDR" TabIndex="16" runat="server" Style="position: relative" />
                                                                                            </td>
                                                                                            <td width="42%" align="left" rowspan="3" colspan="2">
                                                                                                <fieldset class="fieldsetCBlue" style="font-weight: bold; margin-left: 2px; width: 100%;">
                                                                                                    <legend class="">Clinical Team</legend>
                                                                                                    <table width="100%" border="0" align="left" cellpadding="2" cellspacing="1">
                                                                                                        <tr>
                                                                                                            <td width="18%">
                                                                                                                Clinical Team Assignment:
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="drpDepartment" runat="server" Width="190px" TabIndex="17" Style="position: relative"
                                                                                                                AutoPostback="true" OnSelectedIndexChanged="drpDepartment_SelectedIndexChanged"></asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left">
                                                                                                                Phone:</td>
                                                                                                            <td>
                                                                                                                <asp:Label id="lblPhone" runat="server" Font-Bold="True"></asp:Label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td width="15%" align="left">
                                                                                                                Start Date
                                                                                                                <label id="lblStar" runat="server">
                                                                                                                    :</label>
                                                                                                            </td>
                                                                                                            <td width="35%" align="left">
                                                                                                                <asp:TextBox ID="txtStartDate" TabIndex="18" runat="server" Columns="30" MaxLength="75"></asp:TextBox>
                                                                                                                &nbsp;<a href="#" runat="server" name="anchFromDate" id="anchFromDate" style="height: 22px;
                                                                                                                    vertical-align: middle;"><% if (strUserSettings == "YES")
                                                                                                                                                { %>Calendar<% }
                                                                                                                                                               else
                                                                                                                                                               { %><img src="img/ic_cal.gif" width="17" height="15" border="0" /><% } %></a>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td width="15%" align="left">
                                                                                                                End Date:</td>
                                                                                                            <td width="35%" align="left">
                                                                                                                <asp:TextBox ID="txtEndDate" TabIndex="19" runat="server" Columns="30" MaxLength="75"></asp:TextBox>
                                                                                                                &nbsp;<a href="#" runat="server" name="anchToDate" id="anchToDate" style="height: 22px;
                                                                                                                    vertical-align: middle;"><% if (strUserSettings == "YES")
                                                                                                                                                { %>Calendar<% }
                                                                                                                                                               else
                                                                                                                                                               { %><img src="img/ic_cal.gif" alt="" width="17" height="15" border="0" /><% } %></a>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </fieldset>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                Lab TDR:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:CheckBox ID="chkLabTDR" TabIndex="20" runat="server" Style="position: relative" />
                                                                                            </td>
                                                                                            <td width="45%" align="left">
                                                                                                &nbsp;</td>
                                                                                        </tr>
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                Profile Returned:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:CheckBox ID="chkProfileCompleted" runat="server" TabIndex="21" Style="position: relative" />
                                                                                                <td width="45%" align="left">
                                                                                                    &nbsp;</td>
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
                                                                                    <legend class="">Additional Contact Information</legend>
                                                                                    <table width="90%" align="center" border="0" cellpadding="4" cellspacing="2">
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                Name:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtName" runat="server" Columns="35" MaxLength="75" Style="left: 1px;
                                                                                                    position: relative; top: 2px" TabIndex="22"></asp:TextBox>&nbsp;
                                                                                            </td>
                                                                                            <td width="15%" align="left">
                                                                                                Phone:</td>
                                                                                            <td width="35%" align="left">
                                                                                                (<asp:TextBox ID="txtAddPhoneAreaCode" runat="server" Columns="4" MaxLength="3" TabIndex="23"></asp:TextBox>)
                                                                                                <asp:TextBox ID="txtAddPhonePrefix" runat="server" Columns="4" MaxLength="3" TabIndex="24"></asp:TextBox>-
                                                                                                <asp:TextBox ID="txtAddPhoneNumber" runat="server" Columns="6" MaxLength="4" TabIndex="25"></asp:TextBox></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </fieldset>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table width="99.5%" style="margin-left: 5px;" border="0" cellpadding="=0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="Hd2" align="center">
                                                                                <fieldset class="fieldsetCBlue">
                                                                                    <legend class="">Primary Notifications* (complete at least one)</legend>
                                                                                    <table width="90%" align="center" border="0" cellpadding="4" cellspacing="2">
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                &nbsp;Email:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtEmail" TabIndex="26" runat="server" Columns="35" MaxLength="100"></asp:TextBox></td>
                                                                                            <td width="15%" align="left">
                                                                                                Fax:</td>
                                                                                            <td width="35%" align="left">
                                                                                                (<asp:TextBox ID="txtFaxAreaCode" TabIndex="27" Columns="4" runat="server" MaxLength="3"></asp:TextBox>)
                                                                                                <asp:TextBox ID="txtFaxPrefix" TabIndex="28" runat="server" Columns="4" MaxLength="3"></asp:TextBox>-
                                                                                                <asp:TextBox ID="txtFaxNNNN" TabIndex="29" runat="server" Columns="6" MaxLength="4"></asp:TextBox></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </fieldset>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table width="99.5%" style="margin-left: 5px;" border="0" cellpadding="=0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="Hd2" align="center">
                                                                                <fieldset class="fieldsetCBlue">
                                                                                    <legend class="">Professional Affiliations</legend>
                                                                                    <table width="90%" align="center" border="0" cellpadding="4" cellspacing="2">
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                Specialty:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtSpecialty" TabIndex="30" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                                            <td width="15%" align="left">
                                                                                                Practice Group:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtPracticeGroup" TabIndex="31" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="15%" align="left">
                                                                                                Hospital Affiliation:</td>
                                                                                            <td width="35%" colspan="3" align="left">
                                                                                                <asp:TextBox ID="txtHospitalAffiliation" TabIndex="32" runat="server" Columns="35"
                                                                                                    MaxLength="75"></asp:TextBox></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </fieldset>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table width="99.5%" style="margin-left: 5px;" border="0" cellpadding="=0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="Hd2" align="center">
                                                                                <fieldset class="fieldsetCBlue">
                                                                                    <legend class="">Address Information </legend>
                                                                                    <table width="90%" align="center" border="0" cellpadding="4" cellspacing="2">
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                Address 1:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtAddress1" TabIndex="33" runat="server" Columns="35" MaxLength="100"></asp:TextBox></td>
                                                                                            <td width="15%" align="left">
                                                                                                Address 2:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtAddress2" TabIndex="34" runat="server" Columns="35" MaxLength="100"></asp:TextBox></td>
                                                                                        </tr>
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                Address 3:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtAddress3" TabIndex="35" runat="server" Columns="35" MaxLength="100"></asp:TextBox></td>
                                                                                            <td width="15%" align="left">
                                                                                                City:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtCity" TabIndex="36" runat="server" Columns="35" MaxLength="75"></asp:TextBox></td>
                                                                                        </tr>
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                State:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtState" TabIndex="37" runat="server" Columns="35" MaxLength="2"
                                                                                                    Width="30px"></asp:TextBox></td>
                                                                                            <td width="15%" align="left">
                                                                                                Zip:</td>
                                                                                            <td width="35%" align="left">
                                                                                                <asp:TextBox ID="txtZip" TabIndex="38" runat="server" Columns="35" MaxLength="10"></asp:TextBox></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </fieldset>
                                                                                <asp:Label ID="lblError" runat="server" Width="786px" ForeColor="Red" Visible="False"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table width="99.5%" style="margin-left: 5px; position: relative; top: 0px;" border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="Hd2" align="center">
                                                                                <fieldset class="fieldsetCBlue">
                                                                                    <legend class="">External System ID's </legend>
                                                                                    <table width="100%" border="0" align="left" cellpadding="2" cellspacing="1">
                                                                                        <tr>
                                                                                            <td width="98%">
                                                                                                <asp:Label ID="lblIDType" runat="server" Text="ID Type:"></asp:Label>&nbsp;
                                                                                                <asp:DropDownList ID="ddlIDType" runat="server" AutoPostBack="True" TabIndex="37" OnSelectedIndexChanged="ddlIDType_SelectedIndexChanged"></asp:DropDownList>&nbsp;
                                                                                                <asp:Label ID="lblUserId" runat="server" Text="User ID:"></asp:Label>&nbsp;
                                                                                                <asp:TextBox ID="txtUserId" runat="server" TabIndex="38"></asp:TextBox>&nbsp;
                                                                                                <asp:Label ID="lblAddIDType" runat="server" Text="ID Type:" Visible="false"></asp:Label>&nbsp;
                                                                                                <asp:TextBox ID="txtAddIDType" runat="server" Visible="false" TabIndex="39"></asp:TextBox>&nbsp;
                                                                                                <asp:Button ID="btnAddExternalID" CssClass="Frmbutton" TabIndex="40" runat="server" Text="Add" OnClick="btnAddExternalID_Click"></asp:Button></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="98%">
                                                                                                <div id="divExternalIDInformation" runat="server" class="TDiv">
                                                                                                    <asp:DataGrid ID="grdIdTypeInfo" runat="server" CssClass="GridHeader" BorderStyle="None"
                                                                                                        AllowSorting="true" DataKeyField="ExternalRPID" AutoGenerateColumns="false" Width="100%"
                                                                                                        ItemStyle-Height="21px" HeaderStyle-Height="24" OnEditCommand="grdIdTypeInfo_EditCommand"
                                                                                                        OnUpdateCommand="grdIdTypeInfo_UpdateCommand" OnCancelCommand="grdIdTypeInfo_CancelCommand"
                                                                                                        OnDeleteCommand="grdIdTypeInfo_DeleteCommand" OnItemCreated="grdIdTypeInfo_OnItemCreated">
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
                                                                                                            <asp:EditCommandColumn EditText="Edit" CancelText="Cancel" UpdateText="Update" HeaderText="Edit" ItemStyle-Width="10%">
                                                                                                            </asp:EditCommandColumn>
                                                                                                            <asp:TemplateColumn HeaderText="Delete" ItemStyle-Width="10%">
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CausesValidation="false"
                                                                                                                        CommandName="Delete"></asp:LinkButton>
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
                                                                                </fieldset>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table width="99.5%" style="margin-left: 5px; position: relative; top: 0px;" border="0"
                                                                        cellpadding="=0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="Hd2" align="center">
                                                                                <fieldset class="fieldsetCBlue">
                                                                                    <legend class="">Notes</legend>
                                                                                    <table width="90%" align="center" border="0" cellpadding="4" cellspacing="2">
                                                                                        <tr align="center">
                                                                                            <td width="15%" align="left">
                                                                                                Notes:</td>
                                                                                            <td width="85%" align="left">
                                                                                                <asp:TextBox ID="txtNotes" runat="server" Columns="100" MaxLength="255" Rows="3"
                                                                                                    TextMode="MultiLine" Style="left: 1px; position: relative; top: 2px" TabIndex="43"></asp:TextBox>
                                                                                            &nbsp;
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
                                                                                                                <td colspan="11" valign="bottom" class="Step" style="padding: 0 0 0 0px;"><br />
                                                                                                                        <b style="font-size: 11">Step 1: Notification Devices and Events </b>
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
                                                                                                                        DataTextField="DeviceDescription" DataValueField="DeviceID"  TabIndex="44"
                                                                                                                        Width="210px" OnSelectedIndexChanged="cmbDeviceType_SelectedIndexChanged">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:DropDownList ID="cmbGroup" AutoPostBack="true" runat="server" 
                                                                                                                                DataValueField="GroupID" DataTextField="GroupName" TabIndex="45"
                                                                                                                                Width="120px" OnSelectedIndexChanged="cmbGroup_SelectedIndexChanged">
                                                                                                                            </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:TextBox ID="txtNumAddress" runat="server" MaxLength="100" Visible="true" TabIndex="46"
                                                                                                                        Width="95px"></asp:TextBox></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:DropDownList ID="cmbCarrier" runat="server" TabIndex="47" AutoPostBack="true"
                                                                                                                                DataTextField="CarrierDescription" DataValueField="CarrierID"
                                                                                                                                Visible="true" Width="100px" OnSelectedIndexChanged="cmbCarrier_SelectedIndexChanged">
                                                                                                                            </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:TextBox ID="txtEmailGateway" runat="server" MaxLength="100" Visible="true" TabIndex="48"
                                                                                                                        Width="115px"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:TextBox ID="txtInitialPause" TabIndex="49" runat="server" Visible="true" Columns="23" MaxLength="5">1</asp:TextBox>
                                                                                                                </td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:DropDownList ID="cmbEvents" runat="server" TabIndex="50" Visible="true" Width="70px"
                                                                                                                        DataTextField="EventDescription" DataValueField="RPNotifyEventID">
                                                                                                                    </asp:DropDownList></td>
                                                                                                                <td style="width: 1%">
                                                                                                                    <asp:DropDownList ID="cmbFindings" runat="server" TabIndex="51" Visible="true"
                                                                                                                        Width="78px" DataTextField="FindingDescription" DataValueField="FindingID">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td style="width: 90%">
                                                                                                                    <asp:Button ID="btnShowHideDetails" Text="" CssClass="Frmbutton" Width="95" Visible="false"
                                                                                                                                runat="server" TabIndex="58" OnClick="btnShowHideDetails_Click" CausesValidation="false" />                                                                                                          
                                                                                                                       &#160;
                                                                                                                     <asp:Button ID="btnAddDevice" Text="Add" CssClass="Frmbutton" Width="45" Visible="false"
                                                                                                                                runat="server" TabIndex="52" OnClick="btnAddDevice_Click" CausesValidation="false"/>                                                                                              
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
                                                                                                                                    OnItemCreated="grdDevices_ItemCreated" TabIndex="53">
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
                                                                                                                                                <asp:TextBox ID="txtGridDeviceType" runat="server" MaxLength="50" TabIndex="54" Width="95px" Text='<%# DataBinder.Eval(Container, "DataItem.DeviceName") %>'
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
                                                                                                                                                <asp:DropDownList ID="dlistGridEvents" runat="server" TabIndex="55" DataTextField="EventDescription"
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
                                                                                                                                <asp:DropDownList ID="cmbAHDevice" runat="server" TabIndex="56"
                                                                                                                                    Width="170px" DataTextField="DeviceName" DataValueField="RowID">
                                                                                                                                </asp:DropDownList>&nbsp;</td>
                                                                                                                            <td style="width:8%;">
                                                                                                                                   <asp:DropDownList ID="cmbStep3Groups" runat="server" Width="170px" DataTextField="GroupName" AutoPostBack="true"
                                                                                                                                       DataValueField="GroupID" OnSelectedIndexChanged = "cmbStep3Groups_SelectedIndexChanged" TabIndex="57" >
                                                                                                                                   </asp:DropDownList>&nbsp;</td>    
                                                                                                                            <td width="5%">
                                                                                                                                <asp:DropDownList ID="cmbAHFindings"  runat="server" 
                                                                                                                                    Width="170px" DataTextField="FindingDescription" DataValueField="FindingID" TabIndex="58">
                                                                                                                                </asp:DropDownList>&nbsp;</td>
                                                                                                                            <td width="0%">
                                                                                                                                <asp:DropDownList ID="cmbAHStartHour" runat="server" 
                                                                                                                                    Width="90px" TabIndex="59" >
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
                                                                                                                                <asp:DropDownList ID="cmbAHEndHour" runat="server" TabIndex="60"
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
                                                                                                                                <asp:Button CssClass="Frmbutton" ID="btnAddStep3" runat="server" TabIndex="61"
                                                                                                                                   CausesValidation="False"   Width="45px" Text="Add" OnClick="btnAddStep3_Click" ></asp:Button></td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                           <tr>
                                                                                                                <td class="DivBg" valign="top" align="center"><br />
                                                                                                               
                                                                                                                <div id = "AfterHoursNotificationsDiv" class="TDiv" style="width: 100%; vertical-align:top" >
                                                                                                                 
                                                                                                                    <asp:DataGrid ID="grdAfterHoursNotifications"  runat="server" CssClass="GridHeader" BorderStyle="None" TabIndex="62"
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
                                                                    <table width="99.5%" style="margin-left: 5px;" border="0" cellpadding="=0" cellspacing="0">
                                                                        <tr>
                                                                       
                                                                            <td class="Hd2" align="center" > <br />
                                                                                <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="Frmbutton"
                                                                                    OnClick="btnAdd_Click" TabIndex="63" Width="60"></asp:Button>
                                                                                &nbsp;&nbsp;
                                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                                                                    CssClass="Frmbutton" TabIndex="64" Width="60"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                         
                                                                        <tr>
                                                                            <td style=" width: 90px;">
                                                                                <div id="divAddRPGrdStartDate" style="position:absolute; z-index: 100%;" class="Calander">
                                                                                </div>
                                                                                <div id="divAddRPGrdEndDate" style="position:absolute; z-index: 100%;" class="Calander">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                        
                                <%--         <asp:RequiredFieldValidator ID="rfvFirstName" SetFocusOnError="true" Style="z-index: 101;
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
                                                    <asp:RegularExpressionValidator ID="revAddPhoneNumber" runat="server" ControlToValidate="txtAddPhoneNumber"
                                                        Display="None" ErrorMessage="Please enter valid Additional Phone number." SetFocusOnError="true"
                                                        ValidationExpression="\d{4}"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revAddPhoneCode" runat="server" ControlToValidate="txtAddPhoneAreaCode"
                                                        Display="None" ErrorMessage="Please enter valid Additional Phone code." SetFocusOnError="true"
                                                        ValidationExpression="\d{3}"></asp:RegularExpressionValidator><br>
                                                    <asp:RegularExpressionValidator ID="revAddPhonePrefix" runat="server" ControlToValidate="txtAddPhonePrefix"
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
                                                    <asp:RequiredFieldValidator ID="rfvalLoginID" SetFocusOnError="true" runat="server"
                                                        ErrorMessage="You must enter a Login ID." ControlToValidate="txtLoginID" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please enter valid Login ID."
                                                        ControlToValidate="txtLoginID" Display="None" ValidationExpression="(\w)*(\d)*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revalPassword" SetFocusOnError="true" runat="server"
                                                        ErrorMessage="Password must be 4-10 digits." ControlToValidate="txtPassword"
                                                        Display="None" ValidationExpression="\d{4,10}"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvalPIN" runat="server" ErrorMessage="You must enter a ED PIN"
                                                        ControlToValidate="txtPassword" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revalPIN" SetFocusOnError="true" runat="server"
                                                        ErrorMessage="PIN for Message Retrieval must be 4-5 digits." ControlToValidate="txtPinForMessage"
                                                        Display="None" ValidationExpression="\d{4,5}"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revCellAreaCode" runat="server" Display="None"
                                                        ErrorMessage="Please Enter Valid Cell Phone area code." ControlToValidate="txtCellAreaCode" SetFocusOnError="true"
                                                        ValidationExpression="\d{3}" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revCellPrefix" runat="server" Display="None" ErrorMessage="Please Enter Valid Cell Phone prefix."
                                                        ValidationExpression="\d{3}" ControlToValidate="txtCellPrefix" ForeColor="White" SetFocusOnError="true"></asp:RegularExpressionValidator>&nbsp;
                                                    <asp:RegularExpressionValidator ID="revCellNNNN" runat="server" Display="None" ErrorMessage="Please Enter Valid Cell Phone extension."
                                                        ValidationExpression="\d{4}" ControlToValidate="txtCellNNNN" ForeColor="White" SetFocusOnError="true"></asp:RegularExpressionValidator>&nbsp;--%>
                                                    
                                        
                                        
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
                        </div>
                    </td>
                </tr>
            </table>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
