<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test_result_definitions.aspx.cs" Inherits="Vocada.CSTools.test_result_definitions" MasterPageFile="~/cs_tool.master" Title="CSTools: Lab Test Definitions" SmartNavigation="true" %>
<asp:Content ID="cpTestsAndValues" ContentPlaceHolderID="contentPlaceHolder1" runat="Server">
 <asp:UpdatePanel ID="upAddTestResult" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
  <script language="javascript" src="Javascript/common.js" type="text/JavaScript"></script>
  <script language="javascript" src="Javascript/Constants.js" type="text/JavaScript"></script>
  <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">   
   <tr>
    <td class="DivBg" valign="top">
    <asp:HiddenField ID = "hdnTestDataChanged" runat="server" Value = "false" />
    <asp:HiddenField ID = "hdnVoiceOverFile" runat="server" Value = "" EnableViewState="true" />
    <asp:HiddenField ID = "hdnInstitutionVal"  runat="server" Value= "-1" />
    <asp:HiddenField ID = "hdnGroupVal" runat="server" Value= "-1" />
     <div style="overflow-y:Auto; height: 100%; background-color:White">
     <table width="100%" align="center" height="100%" border="0" cellpadding="0" cellspacing="0">
      <tr class="BottomBg">
       <td valign="top">        
       <table width="100%" border="0" cellpadding="=0" cellspacing="0">
        <tr>
            <td class="Hd1" style="height: 20px">
                <asp:Label ID="lblMessageSearchHeader" runat="server" CssClass="UserCenterTitle">Lab Test Definitions</asp:Label>
            </td>
        </tr>
       </table>
        
         <table align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
           <tr>
            <td class="ContentBg">
                   <fieldset class="fieldsetCBlue">
                        <legend class=""><asp:Label ID="lblSection1" runat="server" Text="Select" ></asp:Label></legend>
                        <table id="Table1" cellspacing="1" cellpadding="2" width="60%" border="0" align="center">
                            <tr>
                                <td style="width:15%;white-space:nowrap;" align="right">
                                     Institution Name:&nbsp;&nbsp;</td>
                                <td  style="width:30%;white-space:nowrap;" align="left">
                                    <asp:DropDownList id="cmbInstitution" runat="server" DataValueField="InstitutionID" Width="200"
                                      DataTextField="InstitutionName" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitution_SelectedIndexChanged" >
                                    </asp:DropDownList> 
                                    <asp:Label ID="lblInstName" runat="server" Visible="False" ></asp:Label>
                                    <asp:HiddenField ID = "hdnIsSystemAdmin" runat="server" Value="1" EnableViewState="true"/> 
                                </td>
                                                                    
                                <td  style="width:15%;white-space:nowrap;" align="right">&nbsp;&nbsp;Group:&nbsp;&nbsp;</td>
                                <td  style="width:30%;white-space:nowrap;" align="left">
                                    <asp:DropDownList id="cmbGroup" runat="server" DataTextField="GroupName" width="200" Enabled="true"
                                      DataValueField="GroupID" AutoPostBack="true" OnSelectedIndexChanged="cmbGroup_SelectedIndexChanged" >
                                    </asp:DropDownList></td>
                                <td  style="width:10%;white-space:nowrap;" align="left">
                                   &nbsp;&nbsp;&nbsp;<asp:Button ID="btnImport" CssClass="Frmbutton" runat="server" Text="Lab Test Master List" CausesValidation="false" Enabled="false" />
                                </td>
                            </tr>
                           
                          </table>
                          </fieldset>
                      </td>
           </tr> 
          <tr>
           <td  align="center" style="width: 100%; height:100%;" valign="top"> 
            <fieldset id="fldLabTests" class="fieldsetCBlue" style="width:99.7%;">
             <legend class="">Lab Tests</legend><br/>
             <div id="TestResultsDiv" class="TDiv" style="margin-left:0px;margin-right:0px;" >  
              <asp:DataGrid ID="grdTestResults" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyField="LabTestID"
                  CellPadding="0" CssClass="GridHeader" Width="100%" BorderWidth="1px" OnItemCreated="grdTestResults_OnItemCreated" 
                  OnEditCommand="grdResultDefinitions_EditCommand" OnDeleteCommand="grdTestResults_DeleteCommand" 
                  OnSortCommand="grdTestResults_SortCommand" OnItemDataBound="grdTestResults_ItemDataBound"
                  AlternatingItemStyle-CssClass="row3" ShowHeader="true">                                                                         
               <HeaderStyle CssClass="THeader" Font-Bold="True" HorizontalAlign="Left" VerticalAlign="Middle"/>               
               <ItemStyle Height="30" />               
               <Columns>                             
               <asp:BoundColumn DataField="LabTestID" Visible="false" HeaderText="LabTestID"></asp:BoundColumn>
                <asp:BoundColumn DataField="TestID" Visible="false" HeaderText="TestID"></asp:BoundColumn>
                <asp:BoundColumn DataField="TestDescription" HeaderText="Full Test Name" SortExpression="TestDescription" ></asp:BoundColumn>
                <asp:BoundColumn DataField="TestShortDescription" HeaderText="Short Name" SortExpression="TestShortDescription"></asp:BoundColumn>
                <asp:HyperLinkColumn DataNavigateUrlField="TestVoiceURL" DataNavigateUrlFormatString="{0}"
                    DataTextField="TestVoiceURL" HeaderText="Voiceover URL" DataTextFormatString="&lt;img border=0 src='./img/ic_play_msg.gif'&gt;">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="ResultType" HeaderText="Result Type" SortExpression="ResultType"></asp:BoundColumn>
                <asp:BoundColumn DataField="LowLevel" HeaderText="Low Value" SortExpression="LowLevel" ></asp:BoundColumn>
                <asp:BoundColumn DataField="HighLevel" HeaderText="High Value" SortExpression="HighLevel"></asp:BoundColumn >
                <asp:BoundColumn DataField="MeasurementText"  HeaderText="Measure" SortExpression="MeasurementText"></asp:BoundColumn>
                <asp:BoundColumn Visible="false"  DataField="Grammar"  HeaderText="Grammar"></asp:BoundColumn>
                <asp:BoundColumn DataField="LabTestArea" HeaderText="Lab Test Area" SortExpression="LabTestArea"></asp:BoundColumn>
                <asp:ButtonColumn Text="Edit" HeaderText="Edit" CommandName="Edit">
                    <ItemStyle Width="20px" />
                </asp:ButtonColumn>
                <asp:TemplateColumn HeaderText="Remove">
                  <ItemTemplate>
                    <asp:LinkButton ID="lbtnDelete" runat="server" Text="Remove" CommandName="Delete" OnClientClick="return ConformBeforeDelete();" CausesValidation="false" /> 
                  </ItemTemplate> 
                </asp:TemplateColumn>
				
               </Columns>
              </asp:DataGrid>
             </div>
            </fieldset>
            &nbsp;
           </td>
          </tr>
         
          <tr>
           <td colspan="0" align="center" valign="top" style="width: 98%;height: 179px">
           <fieldset class="fieldsetCBlue">
              <legend class="">Add/Edit Lab Test</legend>
                <table align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                 <tr> 
                    <td width="15%"></td>
                    
                    <td width="35%"></td>
                    <td width="15%"></td>
                    <td width="35%"></td>                  
                  </tr>
                  <tr>
                     <td>Test Name (Full):</td>
                     <td>
                        <asp:TextBox ID="txtFullTestName" runat="server" MaxLength="256" Width="340"></asp:TextBox>
                     </td>
                     <td>Test Name (Short):</td>
                     <td>
                        <asp:TextBox ID="txtShortTestName" runat="server" MaxLength="100" Width="340" ></asp:TextBox>
                     </td>                     
                   </tr>
                   <tr>                       
                     <td>Lab Test Area:</td>
                     <td>
                        <asp:DropDownList ID="cmbLabTestArea" runat="server" DataTextField="LabTestArea"  DataValueField="LabTestArea" 
                            Width="340" ></asp:DropDownList>&nbsp;<asp:TextBox ID="txtLabTestArea" runat="server" MaxLength="100"></asp:TextBox>
                     </td>
                     <td>Test Voiceover URL:</td>
                      <td valign="middle">
                         <asp:HyperLink ID="hlinkPlay" Height="16px" Width="12px" runat="server" ImageUrl="./img/ic_play_msg.gif"
                         Visible="false" Style="vertical-align: middle; padding-right: 3"></asp:HyperLink><asp:FileUpload
                         ID="flupdVoiceOver" runat="server" Width="340" CssClass="frmButton" Style="height: 18px" TabIndex="3" />
                     </td>
                  </tr>   
                  <tr>
                      <td>Result Type:</td> 
                      <td>
                        <asp:DropDownList ID="cmbResultType" runat="server" DataTextField="ResultType" DataValueField="ResultTypeID" CausesValidation="false" Width="340"></asp:DropDownList>
                      </td>
                      <td>Measurement:</td>
                      <td>
                        <asp:DropDownList ID="cmbMeasurement" runat="server" DataTextField="MeasurementDescription" DataValueField="MeasurementID" Width="340" Enabled="false"></asp:DropDownList>
                      </td> 
                  </tr>
                  <tr>
                    <td>
                      Lowest Possible Value:</td>
                      <td>
                        <asp:TextBox ID="txtLowestPossibleValue" runat="server" Columns="5" MaxLength="38" Enabled="false" ></asp:TextBox>
                      </td>      
                      <td rowspan="2">Grammar:</td>
                      <td rowspan="2">
                        <asp:TextBox TextMode="MultiLine" ToolTip="Grammar for VUI" ID="txtGrammar" runat="server" width="340" Rows="3"></asp:TextBox>
                      </td>
                  </tr> 
                  <tr>
                     <td>Highest Possible Value:</td>
                      <td>
                        <asp:TextBox ID="txtHighestPossibleValue" runat="server" Columns="5" MaxLength="38" Enabled="false"></asp:TextBox>
                      </td>
                       <td>&nbsp;</td>
                      <td>&nbsp;</td>
                      
                  </tr>          
                  <tr>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                  </tr>
                  <tr>
                      <td colspan="4" align="Center" width="100%">
                         <asp:Button ID="btnAdd" runat="server" Text=" Add " CssClass="Frmbutton" CausesValidation="false"
                            Enabled="false" OnClick="btnAdd_Click" />&nbsp;&nbsp;                         
                         <asp:Button ID="btnEdit" runat="server" Text=" Save " CssClass="Frmbutton" CausesValidation="false" 
                            Visible="false" OnClick="btnEdit_Click" />&nbsp;&nbsp;
                         <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Frmbutton" OnClick="btnCancel_Click" 
                            Enabled="true" CausesValidation="false" />  
                      </td>
                  </tr> 
                </table>
                   <br />                   
             </fieldset>
           </td>
          </tr>           
         </table>
         <table id="tspace" height="0%">
          <tr>
           <td>            
            <asp:RequiredFieldValidator ID="reqValFullName" runat="server" ControlToValidate="txtFullTestName" ErrorMessage="- You must enter Test Full Name" SetFocusOnError="true" Display="None"  />
            <asp:RequiredFieldValidator ID="reqValShortName" runat="server" ControlToValidate="txtShortTestName" ErrorMessage="- You must enter Test Short Name" SetFocusOnError="true" Display="None"  />
            <asp:CustomValidator ID="ctmValLabTestArea" runat="server" ControlToValidate="cmbLabTestArea" ClientValidationFunction="validateLabTestArea" Display="None" SetFocusOnError="true" ErrorMessage="- You Must Enter Other Value" ></asp:CustomValidator> 
            <asp:RequiredFieldValidator ID="reqValLabTestArea" InitialValue="-1" runat="server" ControlToValidate="cmbLabTestArea" ErrorMessage="- You must enter Lab Test Area" Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator ID="reqValResultType" InitialValue="-1" runat="server" ControlToValidate="cmbResultType" ErrorMessage="- You must enter Result Type" Display="None" SetFocusOnError="true" />
            <asp:RequiredFieldValidator ID="reqValMeasure" Enabled="false" InitialValue="-1" runat="server" ControlToValidate="cmbMeasurement" ErrorMessage="- You must enter Measurement" SetFocusOnError="true" Display="None" />
            <asp:RequiredFieldValidator ID="reqValGrammar" runat="server" ControlToValidate="txtGrammar" ErrorMessage="- You must enter Grammar" Display="None" SetFocusOnError="true" />
            <asp:RegularExpressionValidator ID="regValFullName" runat="server" ValidationExpression="^[ .<>)(=,/+&%@a-zA-Z0-9-]{1,256}$" ControlToValidate="txtFullTestName" SetFocusOnError="true" ErrorMessage="- You must enter Valid Test Full Name" Display="None" />
            <asp:RegularExpressionValidator ID="regValShortName" runat="server" ValidationExpression="^[ .,&a-zA-Z0-9-]{1,256}$" ControlToValidate="txtShortTestName" SetFocusOnError="true" ErrorMessage="- You must enter Valid Test Short Name" Display="None" />
            <asp:RegularExpressionValidator ID="regValGrammar" runat="server" ValidationExpression="^[a-zA-Z0-9; ]{1,4000}$" ControlToValidate="txtGrammar" SetFocusOnError="true" ErrorMessage="- You must enter Valid Grammar" Display="None" />
            <asp:RegularExpressionValidator ID="regValHighVal" ValidationExpression="^[-+]?\d*\.?\d*$" runat="server" ControlToValidate="txtHighestPossibleValue" SetFocusOnError="true" ErrorMessage="- You must enter Valid Test High Value" Display="None"/>
            <asp:RegularExpressionValidator ID="regValLowVal" ValidationExpression="^[-+]?\d*\.?\d*$" runat="server" ControlToValidate="txtLowestPossibleValue" SetFocusOnError="true" ErrorMessage='- You must enter Valid Test Low Value' Display="None" />
            <asp:RangeValidator ID = "rngValLowVal" runat="server" ControlToValidate="txtLowestPossibleValue" MaximumValue = "9999999" MinimumValue = "0" Display="none" SetFocusOnError="true" ErrorMessage = "- You must enter Test Low Value < 9999999"></asp:RangeValidator>
            <asp:RangeValidator ID = "rngValHighVal" runat="server" ControlToValidate="txtHighestPossibleValue" MaximumValue = "9999999" MinimumValue = "0" Display="none" SetFocusOnError="true" ErrorMessage = "- You must enter Test High Value < 9999999"></asp:RangeValidator>
            <asp:CustomValidator ID="ctmLowHigh" runat="server" ControlToValidate="cmbResultType" ClientValidationFunction="validateHighestLowest" Display="None" SetFocusOnError="true"></asp:CustomValidator>
            <asp:CustomValidator ID="ctmVoiceOverFile" runat="server" ControlToValidate="flupdVoiceOver" ClientValidationFunction="validateVoiceOver" Display="None" SetFocusOnError="true"></asp:CustomValidator>
            <asp:ValidationSummary ID="vsmrEditResult" Style="z-index: 107; left: 540px;
                position: absolute; top: 266px" runat="server" ShowSummary="false" ShowMessageBox="True" DisplayMode="List"></asp:ValidationSummary>                                                                            
             
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
  </ContentTemplate>
  
<Triggers>
    <asp:PostBackTrigger ControlID="btnAdd" /> 
    <asp:PostBackTrigger ControlID="btnEdit" /> 
    <asp:AsyncPostBackTrigger ControlID="cmbInstitution" />
    <asp:AsyncPostBackTrigger ControlID="cmbGroup" />
</Triggers>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
    // This function will ask for conformation before deletion record
   var mapId = "test_result_definitions.aspx";
   var textchanged;
   var isEditMode = false; 
   /*Change value of hidden variable with given value*/
    function testdataChanged()
    {       
        document.getElementById(hdnTestDataChangedClientID).value = "true";
        textchanged = true;
        return;
    }
    
    /*Change value of hidden variable with given value*/
    function resetTestDataChanged()
    {   
        document.getElementById(hdnTestDataChangedClientID).value = "false";
        textchanged = false;
        return;
    }
    
    function EnableValControls()
    {    
        var highVal;
        var lowVal;
        var measure;
        var resultType;
        var reqValMes;
        
        highVal = document.getElementById(txtLowestPossibleValueClientID);
        lowVal = document.getElementById(txtHighestPossibleValueClientID);
        measure = document.getElementById(cmbMeasurementClientID);
        resultType = document.getElementById(cmbResultTypeClientID);
        reqValMes = document.getElementById(reqValMeasureClientID);
        
        if(highVal != null && lowVal != null && resultType != null)
        {                           
            if(resultType.options[resultType.selectedIndex].text == 'Numeric')
            {
                highVal.value="";
                lowVal.value="";
                highVal.disabled=false;
                lowVal.disabled=false;
                measure.disabled=false;
                ValidatorEnable(reqValMes, true);
            }
            else
            {
                highVal.value="";
                lowVal.value="";
                highVal.disabled=true;
                lowVal.disabled=true;
                measure.disabled=true;
                measure.value="-1";
                ValidatorEnable(reqValMes, false);
            }
        }        
    }
    /* Confirm  before deleting record */
    function ConformBeforeDelete()
    {
        if(onTestdataChanged())
        {
            if(confirm("Are you sure you want to remove this Test Result record?"))
            {
                //otherPostback =true;
                return true;
            }
        }
        //otherPostback =false;
        return false;                                          
    }    
    function toggleResultValueControls()
    {       
       if(document.getElementById(cmbResultTypeClientID).options[document.getElementById(cmbResultTypeClientID).selectedIndex].text == 'Numeric' )
       {
         document.getElementById(lblLowestPossibleValueClientID).style.visibility = "visible";
         document.getElementById(txtLowestPossibleValueClientID).style.visibility = "visible";
         document.getElementById(lblHighestPossibleValueClientID).style.visibility = "visible";
         document.getElementById(txtHighestPossibleValueClientID).style.visibility = "visible";
         document.getElementById(cmbMeasurementClientID).disabled = false;                 
       }
       else 
       {
        document.getElementById(txtLowestPossibleValueClientID).value = "";
        document.getElementById(txtHighestPossibleValueClientID).value = "";
        document.getElementById(lblLowestPossibleValueClientID).style.visibility = "hidden";
        document.getElementById(txtLowestPossibleValueClientID).style.visibility = "hidden";
        document.getElementById(lblHighestPossibleValueClientID).style.visibility = "hidden";
        document.getElementById(txtHighestPossibleValueClientID).style.visibility = "hidden";
        document.getElementById(cmbMeasurementClientID).selectedIndex = 0;
       }
    } 
    
    function validateLabTestArea(source, arguments)
    {   
        var retVal = true;
        
        var LabtestArea = document.getElementById(cmbLabTestAreaClientID);
        if(LabtestArea.options[LabtestArea.selectedIndex].text == 'Other')
        {
           if(document.getElementById(txtLabTestAreaClientID).value == '')
                retVal = false;
            else
                retVal = true;
        }
        if(arguments != null)
            arguments.IsValid = retVal;
       return retVal ;      
    }
    
    var highLowValueCompareError = "";
    function validateHighestLowest(source, arguments)
    {   
        highLowValueCompareError = '';
        var lowValue;
        var highValue;
        
        var resultType = document.getElementById(cmbResultTypeClientID);
        if(resultType.options[resultType.selectedIndex].text == 'Numeric')
        {
           lowValue = document.getElementById(txtLowestPossibleValueClientID).value;
           highValue = document.getElementById(txtHighestPossibleValueClientID).value;
           
           if(lowValue == '')
              highLowValueCompareError = ' - You must enter Lowest Possible Value';
           if (highValue == '')
               highLowValueCompareError = highLowValueCompareError +  '\n' + ' - You must enter Highest Possible Value';  
           if (lowValue != '' && highValue != '')
           {
              lowValue = parseFloat(lowValue);
              highValue = parseFloat(highValue);
              if (lowValue > highValue)
              highLowValueCompareError = ' - Lowest possible value should be less than Highest possible value';
           }
        }
        if(highLowValueCompareError.length>0)
        {
            if(source != null && arguments != null)
            {
                source.errormessage = highLowValueCompareError;
                arguments.IsValid = false;
            }
             return false ;
        }
        else
        {
            if(arguments != null)
            {
                arguments.IsValid = true;
            }
            return true;
        }   
    }
    
    /*Check whether input value is numeric*/
    function isNumericKeyStroke()
    {
     var returnValue = false;
     var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

     if ( ((keyCode >= 48) && (keyCode <= 57)) || // All numerics
               (keyCode ==  8) ||     // Backspace
               (keyCode == 13) ||
               (keyCode ==  9) ||
               (keyCode == 46) || // delete key
               (keyCode == 16) || // shift key
               (keyCode == 17) || // ctrl key
               (keyCode == 35) || //End key
               (keyCode == 36) || //Home key
               (keyCode == 37) || // Left
               (keyCode == 39) || // right
               ((keyCode >= 96) && (keyCode <= 105)))     // Number pad numbers
             returnValue = true;

     if ( window.event.returnValue )
      window.event.returnValue = returnValue;

     return returnValue;
    }
    
    /*Validate regular expression*/
    function checkValidURL(fieldId)
    {            
        var val= document.getElementById(fieldId).value;        
         //var regex = new RegExp("/^(((http(s?))|(ftp))\:\/\/)?(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|us|ca|uk)(\:[0-9]+)*(\/($|[a-zA-Z0-9\.\,\;\?\'\\\+&%\$#\=~_\-]+))*$/");
         var regex = new RegExp("/http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?/");
          return false;       
         
    }
    
    function displayLabtestArea ()
    {
        var LabtestArea = document.getElementById(cmbLabTestAreaClientID);
        
        if(LabtestArea.options[LabtestArea.selectedIndex].text == 'Other')
        {
            document.getElementById(txtLabTestAreaClientID).style.visibility = "visible";
        }
        else
        {
            document.getElementById(txtLabTestAreaClientID).style.visibility = "hidden";
        }
    }
    function HideOtherText()
    {
      document.getElementById(txtLabTestAreaClientID).style.visibility = "hidden";  
    }
    
        
    /* Alert user if data chnaged in edit section of lab test */
    function onTestdataChanged()
    {
        if(document.getElementById(hdnTestDataChangedClientID).value == 'true')
        {
            if(confirm("Some data has been changed, do you want to continue?"))
            {
                document.getElementById(hdnTestDataChangedClientID).value = 'false';
                return true;
            }
            return false;         
         }
         return true;
    }
    
    /*Redirect user to assign test page*/
    function redirectToLabTestMaster(groupID, instituteID)
    {
        try
        {
            window.location.href = "assign_test.aspx?groupId=" + groupID + "&instId=" + instituteID;
        }
        catch(ex)
        {
        }
    }
    
     // On Institution chnage check whether form data has been changed, if yes the ask for confirmation.
       function onComboChange(isInstiCombo)
       {
            if(onTestdataChanged())
            {
                if(isInstiCombo == 'true')
                     {
                        __doPostBack('ctl00$ContentPlaceHolder1$cmbInstitution','');
                        document.getElementById(hdnInstitutionValClientID).value = document.getElementById(cmbInstitutionClientID).value ;
                        document.getElementById(hdnGroupValClientID).value = "-1";
                     }
                else
                     {
                        __doPostBack('ctl00$ContentPlaceHolder1$cmbGroup','');
                        document.getElementById(hdnGroupValClientID).value = document.getElementById(cmbGroupClientID).value ;
                     }
                 return true;
            }
            else
            {
               if(isInstiCombo == 'true')
                     {
                        document.getElementById(cmbInstitutionClientID).value = document.getElementById(hdnInstitutionValClientID).value;
                     }
                else
                     {
                        document.getElementById(cmbGroupClientID).value = document.getElementById(hdnGroupValClientID).value;
                     }
                return false;
            }
       }
       
       var voiceOverErrorMessage = "";
       // Custom validation for file to voice over url
       function validateVoiceOver(source, arguments)
       {
            voiceOverErrorMessage ="";
            var fileName = document.getElementById(flupdVoiceOverClientID).value ;
            document.getElementById(hdnVoiceOverFileClientID).value = fileName;
            if(fileName.length > 0) 
            {
                var ext = fileName.substr(fileName.length - 4,4);
                if (ext != ".wav")
                {
                    voiceOverErrorMessage = " - Please select valid Test VoiceOverURL Filename. \n";
                    
                    if(source != null && arguments != null)
                    {
                        source.errormessage = voiceOverErrorMessage;
                        arguments.IsValid = false;
                    }
                    
                    document.getElementById(flupdVoiceOverClientID).focus();
                    return false;
                }
            }
            /*else if(fileName.length == 0) 
            {
                if(isEditMode)
                {
                    if(document.getElementById(hlinkPlayClientID) == null)
                    {
                        voiceOverErrorMessage = " - You must enter Test VoiceOverURL. \n";
                        if(source != null)
                            source.errormessage = voiceOverErrorMessage;
                        document.getElementById(flupdVoiceOverClientID).focus();
                    }
                }
                else
                {
                    voiceOverErrorMessage = " - You must enter Test VoiceOverURL. \n";
                    if(source != null)
                        source.errormessage = voiceOverErrorMessage;
                    document.getElementById(flupdVoiceOverClientID).focus();
                }
            }*/
            else
            {
                if(arguments != null)
                    arguments.IsValid = true;
                return true;
            }
       }
       
       /* Validate given grammar with given value*/
       function validateGrammar(value, grammar)
       {
            var exp = new RegExp(grammar);
            return value.match(exp);
       }
       
        // Custom validation for file to voice over url
       function validateVoiceOverFileName()
       {
            var fileName = document.getElementById(flupdVoiceOverClientID).value ;
            if(fileName.length > 0) 
            {
                var ext = fileName.substr(fileName.length - 4,4);
                if (ext != ".wav")
                {
                    alert(" - Please select valid Test VoiceOverURL Filename. \n");
                    document.getElementById(flupdVoiceOverClientID).focus();
                    return false;
                }
            }
            /*else if(fileName.length == 0) 
            {
                alert(" - You must enter Test VoiceOverURL. \n");
                document.getElementById(flupdVoiceOverClientID).focus();
            }*/
            else
            {
                return true;
            }
       }
       
       function onclickbutton(EditMode)
       {
            var errorMessage = "";
            var setFoucsOn = "";
            var ctl_txtFullTestName = document.getElementById(txtFullTestNameClientID);
            var ctl_txtShortTestName = document.getElementById(txtShortTestNameClientID);
            var ctl_cmbLabTestArea = document.getElementById(cmbLabTestAreaClientID);
            var ctl_cmbResultType = document.getElementById(cmbResultTypeClientID);
            var ctl_cmbMeasurement = document.getElementById(cmbMeasurementClientID);
            var ctl_txtGrammar = document.getElementById(txtGrammarClientID);
            var ctl_txtHighestPossibleValue = document.getElementById(txtHighestPossibleValueClientID);
            var ctl_txtLowestPossibleValue = document.getElementById(txtLowestPossibleValueClientID);
            var ctl_flupdVoiceOver = document.getElementById(flupdVoiceOverClientID);
            
            isEditMode = EditMode;
            //Required Field
            if(ctl_txtFullTestName.value.length == 0)
            {
                errorMessage = " - You must enter Test Full Name. \n";
                setFoucsOn = txtFullTestNameClientID;
            }
            if(ctl_txtShortTestName.value.length == 0)
            {
                errorMessage += " - You must enter Test Short Name. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtShortTestNameClientID;
            }
            if(ctl_cmbLabTestArea.value == "-1")
            {
                errorMessage += " - You must select Lab Test Area. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = cmbLabTestAreaClientID;
            }
            if(ctl_cmbResultType.value == "-1")
            {
                errorMessage += " - You must select Result Type. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = cmbResultTypeClientID;
            }
            if(!ctl_cmbMeasurement.disabled && ctl_cmbMeasurement.value == "-1")
            {
                errorMessage += " - You must enter Measurement. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = cmbMeasurementClientID;
            }
            /*if(ctl_txtGrammar.value.length == 0)
            {
                errorMessage += " - You must enter Grammar. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtGrammarClientID;
            }*/
            
            //Regular Exp
            if(ctl_txtFullTestName.value.length != 0 && validateGrammar(ctl_txtFullTestName.value, "^[ .<>)(=,/+&%@a-zA-Z0-9-]{1,256}$") == null)
            {
                errorMessage += " - You must enter Valid Test Full Name. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtFullTestNameClientID;
            }
            if(ctl_txtShortTestName.value.length != 0 && validateGrammar(ctl_txtShortTestName.value, "^[ .,&a-zA-Z0-9-]{1,256}$") == null)
            {
                errorMessage += " - You must enter Valid Test Short Name. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtShortTestNameClientID;
            }
            if(ctl_txtGrammar.value.length != 0 && validateGrammar(ctl_txtGrammar.value, "^[a-zA-Z0-9; ]{1,4000}$") == null)
            {
                errorMessage += " - You must enter Valid Grammar. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtGrammarClientID;
            }
            if(ctl_txtLowestPossibleValue.value.length != 0 && validateGrammar(ctl_txtLowestPossibleValue.value, "\d*\.?\d*$") == null)
            {
                errorMessage += " - You must enter Valid Test Low Value. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtLowestPossibleValueClientID;
            }
            if(ctl_txtHighestPossibleValue.value.length != 0 &&  validateGrammar(ctl_txtHighestPossibleValue.value, "\d*\.?\d*$") == null)
            {
                errorMessage += " - You must enter Valid Test High Value. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtHighestPossibleValueClientID;
            }
            
            
            //Range
            if(!ctl_txtLowestPossibleValue.disabled && ctl_txtLowestPossibleValue.value.length > 0&& (ctl_txtLowestPossibleValue.value >= 9999999 || ctl_txtLowestPossibleValue.value < 0))
            {
                errorMessage += " - You must enter Test Low Value < 9999999. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtLowestPossibleValueClientID;
            }
            if(!ctl_txtHighestPossibleValue.disabled && ctl_txtHighestPossibleValue.value.length > 0 && (ctl_txtHighestPossibleValue.value >= 9999999 || ctl_txtHighestPossibleValue.value < 0))
            {
                errorMessage += " - You must enter Test High Value < 9999999. \n";
                if(setFoucsOn.length == 0)
                    setFoucsOn = txtHighestPossibleValueClientID;
            }
            
            //Custom
            validateHighestLowest()
            if(highLowValueCompareError.length > 0)
                {
                    errorMessage += highLowValueCompareError + "\n";
                    if(setFoucsOn.length == 0)
                        setFoucsOn = txtLowestPossibleValueClientID;
                }
            
            
            validateVoiceOver();
            if(voiceOverErrorMessage.length > 0)
            {
                errorMessage += voiceOverErrorMessage;  
                if(setFoucsOn.length == 0)
                        setFoucsOn = flupdVoiceOverClientID;
            }
            
            validateLabTestArea();

            if(errorMessage.length > 0)
            {
                alert(errorMessage);
                document.getElementById(setFoucsOn).focus();
                return false
            }
            else
            {
//                if(isEditMode)
//                    __doPostBack(lnkEditClientID, "");
//                else    
//                    __doPostBack(lnkAddClientID, "");
                return true;
                    
            }
            return true;
       }
      
      /*Cancel Enter key action on tetxbox and combo controls on page*/ 
      function deactiveEnterAction()
      {
        if (window.event.keyCode == 13) 
        {
                window.event.keyCode = 23; 
                event.returnValue=false; 
                event.cancel = true;
        }
      }
       
      /* set Color of Selected Item on grid*/ 
      function setGridSelectedItemColor(controlID)
      {
        if(document.getElementById(controlID) != null)
            document.getElementById(controlID).style.backgroundColor = "#ffffcc";
      } 
      
      function redirect(group, institute)
      {
        if(onTestdataChanged())
        {
            redirectToLabTestMaster(group, institute);
        }
        else
         return false;
      }
    // JS Code : For editing Row inside the Grid
    // if (document.all.editedRowClientID != null) { document.all.editedRowClientID.scrollIntoView(); }
    </script>
</asp:Content>