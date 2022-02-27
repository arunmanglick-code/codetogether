<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="add_group.aspx.cs" Inherits="Vocada.CSTools.add_group" Theme="csTool" Title="CSTools: Add Group"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="upnlAddGroup" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
<script language="JavaScript" src="Javascript/Institution.js" type="text/JavaScript"></script>
<script language="javascript" type="text/JavaScript">
var mapId = "add_group.aspx"
    function Navigate(instId,page,groupID)
    {   
       
       try
        {
            if (page == "GroupMonitior")
                window.location.href = "group_monitor.aspx?InstitutionID=" + instId;
            else if (page == "AddFindings")
                window.location.href = "add_findings.aspx?InstitutionID=" + instId + "&GroupID=" + groupID ;
            else if (page == "GroupPreferences")
                window.location.href = "group_preferences.aspx?GroupID=" + groupID;
            else
                 window.location.href = "group_monitor.aspx?InstitutionID=" + instId;
        }
        catch(_error)
        {
            return;
        }
    }
   //Check whether other record has been edited, then ask for conformation.
    function confirmOnDataChange()
    {
        if(document.getElementById(hdnTextChangedClientID).value =="true")
        {                          
            if(confirm("Some data has been changed, do you want to continue?"))
            {
                document.getElementById(hdnTextChangedClientID).value = "false";
                return true;
            }
            else
                return false;                    
        }
        return true;
    }
    // On Institution chnage check whether form data has been changed, if yes the ask for confirmation.
    function onComboChange()
    {
        if(confirmOnDataChange())
        {
            //__doPostBack('ctl00$ContentPlaceHolder1$dlistInstitution','');
            __doPostBack('ctl00$ContentPlaceHolder1$cmbInstitution','');
            document.getElementById(hdnInstitutionValClientID).value = document.getElementById(cmbInstitutionClientID).value ;
            return true;
        }
        else
        {
            document.getElementById(cmbInstitutionClientID).value = document.getElementById(hdnInstitutionValClientID).value;
            return false;
        }
    }
    </script>
     <table style="height:100%;" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">        
        <tr class="ContentBg">
          <td valign="top">
          <div style="overflow-y: auto; height: 100%">
            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
             <tr>
            <td class="Hd1">
                Add Group
            </td>
            <td style="width:15%;" align="right" class="Hd1"><asp:HyperLink ID="hlinkGroupMonitor" runat="server" CssClass="AccountInfoText"
                    NavigateUrl="./group_monitor.aspx?InstitutionID=-1">Back to Group Monitor</asp:HyperLink>&nbsp;&nbsp;
            </td>
            </tr></table> 
            <table width="98%" border="0" align="center" cellpadding="=0" cellspacing="0">
            <tr>
            <td align="center">
                   <fieldset class="fieldsetCBlue"  >
                        <legend class=""><b>Select</b></legend>
                        <table width="20%" border="0" cellspacing="0" cellpadding="0" align="center">
                          <tr>
                            
                            <td valign="middle" style="white-space:nowrap;" align="right">Institution Name:&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td align="left"> <input type="hidden" id="hdnTextChanged" runat="server" name="textChanged" value="false" style="display: none;" />
                                <input type="hidden" id="hdnInstitutionVal" runat="server" name="hdnInstitutionVal" value="false" style="display: none;" />
                                <asp:DropDownList id="cmbInstitution" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitution_SelectedIndexChanged" 
                                  DataTextField="InstitutionName" DataValueField="InstitutionID"  ></asp:DropDownList>
                                <asp:Label ID="lblInstName" runat="server" Visible="False"></asp:Label>
                                <asp:HiddenField ID = "hdnIsSystemAdmin" runat="server" Value="1" /> 
                             </td>
                            
                           </tr>
                       </table>
                    </fieldset>
              </td> 
            </tr>
         </table> <br />
         <table width="98%" border="0" align="center" cellpadding="=0" cellspacing="0">
          <tr>
            <td align="center" >
                <fieldset class="fieldsetCBlue">
                    <legend class="">Group Information</legend>                    
                    <br />
                      <table cellspacing="1" cellpadding="2" width="100%" border="0">
                            <tr>
                                <td style="width:5%;"></td>
                                <td style="width:19%"></td>
                                <td style="width:36%"></td>
                                <td style="width:15%"></td>
                                <td style="width:20%"></td>
                                <td style="width:5%"></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Group Name*:</td>
                                <td>
                                    &nbsp;&nbsp;<asp:TextBox ID="txtGroupName" runat="server"  MaxLength="100" Width="300"></asp:TextBox>&nbsp;</td>                                
                                <td>Directory*:</td>
                                <td>
                                    &nbsp;<asp:DropDownList id="cmbDirName" runat="server" width="200" DataTextField="DirectoryDescription" 
                                     DataValueField="DirectoryID"></asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    Group 800 Number*:</td>
                                <td>
                                    &nbsp;&nbsp;(<asp:TextBox Width="35px" ID="txtGroup800No1" runat="server" MaxLength="3"></asp:TextBox>)&nbsp;
                                    <asp:TextBox ID="txtGroup800No2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>
                                    - <asp:TextBox ID="txtGroup800No3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
              
                                </td>                                                                
                                 <td>
                                   Ordering Clinician 800 Number*:</td>
                                <td>
                                    &nbsp;(<asp:TextBox Width="35px" ID="txtRP800No1" runat="server" MaxLength="3"></asp:TextBox>)&nbsp;
                                    <asp:TextBox ID="txtRP800No2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>
                                    - <asp:TextBox ID="txtRP800No3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
              
                                </td>
                                <td>&nbsp;</td>
                            </tr>    
                            <tr>
                                <td>&nbsp;</td>
                                <td>Group Type</td>
                                <td><asp:RadioButtonList CssClass="radiobutton" id="rbGroupType"  runat="server" RepeatDirection="Horizontal"   >
                                        <asp:ListItem Text="Radiology" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Lab" Value="1"></asp:ListItem>  
                                    </asp:RadioButtonList> 
                               </td>
                               <td>
                                   Practice Type*:</td>
                                <td>
                                     &nbsp;<asp:DropDownList ID="cmbPractieType" runat="server" DataTextField="PracticeTypeDescription" 
                                     DataValueField="PracticeTypeID" Width="200"></asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                            </tr> 
                             <tr>                                   
                                <td>&nbsp;</td>
                                <td>Time Zone*:</td>
                                <td>
                                    &nbsp;&nbsp;<asp:DropDownList ID="cmbTimeZone" runat="server" DataTextField="Description" DataValueField="TimeZoneID" width="200" ></asp:DropDownList>
                                </td>
                                <td>
                                     Affiliation:</td>
                                <td>
                                    &nbsp;<asp:TextBox ID="txtAffiliation"  runat="server" MaxLength="100" Width="200"></asp:TextBox>&nbsp;
                                </td>  
                                <td>&nbsp;</td>   
                            </tr>   
                            <tr>                                   
                                <td>&nbsp;</td>
                                <td>Address1:</td>
                                <td>
                                    &nbsp;&nbsp;<asp:TextBox ID="txtAdd1"  runat="server" MaxLength="100" Width="200"> </asp:TextBox></td>
                                <td>Address2:</td>
                                <td>
                                    &nbsp;<asp:TextBox ID="txtAdd2"  runat="server" MaxLength="100" Width="200"></asp:TextBox></td>
                                <td>&nbsp;</td>        
                            </tr>                            
                            <tr>                                 
                                <td>&nbsp;</td>
                                <td>City:</td>
                                <td>
                                    &nbsp;&nbsp;<asp:TextBox  ID="txtCity" runat="server" MaxLength="50" Width="200"></asp:TextBox>
                                </td>                                 
                                <td>State:</td>
                                <td>&nbsp;<asp:TextBox ID="txtState" runat="server" MaxLength="2" Columns="4"></asp:TextBox>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>                                 
                                <td>&nbsp;</td>
                                <td>Zip:</td>
                                <td>
                                    &nbsp;&nbsp;<asp:TextBox ID="txtZip" runat="server" MaxLength="10" Width="200"></asp:TextBox>
                                </td>                                 
                                <td>Phone:</td>
                                <td>&nbsp;(<asp:TextBox Width="35px" ID="txtPhone1" runat="server" MaxLength="3"></asp:TextBox>)&nbsp;
                                    <asp:TextBox ID="txtPhone2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>
                                    - <asp:TextBox ID="txtPhone3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                </td>
                                <td>&nbsp;</td>     
                            </tr>
                            <tr>                                 
                                <td>&nbsp;</td>
                                <td ><asp:Label ID="lblDefault" runat="server" Text="Insert Default Findings ( red, orange, yellow ):"></asp:Label></td>
                                <td>&nbsp;<asp:CheckBox  id="chkDefFinding" CssClass="FrmChkBox" runat="server" /></td>    
                                
                                <td style="white-space:nowrap;">Configure Other Findings:</td>
                                <td>
                                    <asp:CheckBox ID="chkOtherFindings" runat="server" CssClass="FrmChkBox"  />&nbsp;                                    </td>                                 
                              <td>&nbsp;</td>
                            </tr>
                            <!--<tr>
                               <td>
                               Group Voice URL:</td>
                               <td>
                                <asp:FileUpload Width="195px" ID="btnBrowse" runat="server" CssClass="Frmbutton" />
                               </td>
                            </tr>-->
                            
                          </table>
                   
                    <br /><br />        
                    <p align="center">
                        <asp:Button ID="btnAddGroup" Text=" Save " runat="server" CssClass="Frmbutton" OnClick="btnAddGroup_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button  ID="btnCancel" Text="Cancel" runat="server" CssClass="Frmbutton" OnClick="btnCancel_Click" CausesValidation="false" />
                    </p>  
                    <asp:ValidationSummary ID="vsmrAddGroup" runat="server" ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
                     <asp:RegularExpressionValidator ID="revGroupName" runat="server" ControlToValidate="txtGroupName"
                      Display="None" ErrorMessage="Please Enter Valid Group Name." SetFocusOnError="true"
                      ValidationExpression="[A-Za-z.'\s\- ]*"></asp:RegularExpressionValidator>
                     </fieldset>                                                         
                    </td> 
                 </tr>
               </table>
            </div>
          </td>
        </tr>
    </table> 
</ContentTemplate> 
<Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAddGroup"  />
            <asp:AsyncPostBackTrigger ControlID="cmbInstitution"  />
</Triggers>
</asp:UpdatePanel> 
</asp:Content>

