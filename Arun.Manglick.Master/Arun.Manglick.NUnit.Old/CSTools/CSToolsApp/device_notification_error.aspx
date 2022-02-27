<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="device_notification_error.aspx.cs" Inherits="Vocada.CSTools.device_notification_error" Theme="CSTOOL" Title="CSTools: Device Notification Errors" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="upAddTestResult" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
  <script language="javascript" src="Javascript/common.js" type="text/JavaScript"></script>
  <script language="javascript" src="Javascript/Constants.js" type="text/JavaScript"></script>
  <script language="javascript" type="text/javascript">
    var mapId = "device_notification_error.aspx";
    function Validate()
        {
           var errorMessage = "";
           var hdnIsSystemAdmin = document.getElementById(hdnIsSystemAdminClientID).value ;
           if (hdnIsSystemAdmin == "1")
           {
                if(document.getElementById(cmbInstitutionsClientID).value == "0")
                    errorMessage = "Please select Institution.\r\n";
            }
            if(document.getElementById(cmbGroupsClientID).value == "-1")
                errorMessage += "Please select Group."; 
           
            if(errorMessage.length > 0)
              {
                alert(errorMessage);
                return false;
              }
            return true;   
        }
        /* Enable Disable button on selection of insti/directory */
       function enableControls()
       {
            var group;
            var institution; 
            var btnGO;
            group = document.getElementById(cmbGroupsClientID);
            var hdnIsSystemAdmin = document.getElementById(hdnIsSystemAdminClientID).value ;
             if (hdnIsSystemAdmin == "1")
             {
                institution = document.getElementById(cmbInstitutionsClientID);
             }
             else
             {
                institution = "1";
             }
            btnGO = document.getElementById(btnGoClientID);
            
            if(group  !=null && btnGO !=null && institution != null)
            {
                if(group.options[group.selectedIndex].value != "-1")
                {
                    btnGO.disabled = false;
                }
                else
                {
                    btnGO.disabled = true;
                }
            }        
       }
       
      
  </script>
  
  <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">   
   <tr>
    <td class="DivBg" valign="top">
     <div style="overflow-y:Auto; height: 100%; background-color:White">
     <table width="100%" align="center" height="100%" border="0" cellpadding="0" cellspacing="0">
      <tr class="BottomBg">
       <td valign="top">        
       <table width="100%" border="0" cellpadding="=0" cellspacing="0">
        <tr>
            <td class="Hd1" style="height: 20px">
                <asp:Label ID="lblMessageSearchHeader" runat="server" CssClass="UserCenterTitle">Device Notification Errors</asp:Label>
            </td>
        </tr>
       </table>
         <table align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
           <tr>
            <td class="ContentBg" align="center">
                   <fieldset class="fieldsetCBlue">
                        <legend ><asp:Label ID="lblSection1" runat="server" Text="Select" ></asp:Label></legend>
                        <table id="Table1" cellspacing="1" cellpadding="2" width="70%" border="0">
                            <tr>
                                <td align="right" style="width:10%;white-space:nowrap;"  >
                                     Institution Name:&nbsp;&nbsp;</td>
                                <td   style="width:30%;white-space:nowrap;">
                                    <asp:DropDownList id="cmbInstitution" runat="server" DataValueField="InstitutionID" Width="250"
                                      DataTextField="InstitutionName" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitution_SelectedIndexChanged" >
                                    </asp:DropDownList>
                                    <asp:Label ID="lblInstName" runat="server" Visible="False" ></asp:Label>
                                </td>                                
                                <td align="right" style="width:10%;white-space:nowrap;">&nbsp;&nbsp;Group:&nbsp;&nbsp;</td>
                                <td style="width:30%;white-space:nowrap;">
                                    <asp:DropDownList id="cmbGroup" runat="server" DataTextField="GroupName" width="250" 
                                       DataValueField="GroupID" AutoPostBack="false">
                                    </asp:DropDownList></td>
                               <td style="width:20%;white-space:nowrap;">
                                   &nbsp;&nbsp;&nbsp;<asp:Button ID="btnGo" UseSubmitBehavior="true" runat="server" OnClick="btnGo_Click" Text=" GO " 
                                                OnClientClick="return Validate();" CssClass="Frmbutton"/>&nbsp;&nbsp;&nbsp;
                               </td>  
                            </tr>
                          </table>
                      </fieldset>
                </td>
           </tr>
           </table>
            <table align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td> </br>
                    <input type="hidden" id="hidPageIndex" value="0" runat="server" />
                    <asp:HiddenField ID = "hdnIsSystemAdmin" runat="server" Value="1" /> 
                    <asp:UpdatePanel ID="upnlRecords" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Label ID="lblRecordCount" runat="server" Font-Bold="true"></asp:Label>
                     </ContentTemplate> 
                    </asp:UpdatePanel>      
                </td>
           </tr>
           <tr>
           <td class="ContentBg" align="center" colspan="2" cellpadding="2"> 
            <fieldset class="fieldsetCBlue">
             <legend >Device Notification Errors</legend><br/>
             <div id="DeviceErrorDiv" class="TDiv" style="border:0px">  
              <asp:DataGrid ID="dgDeviceError" runat="server" AllowSorting="True" AutoGenerateColumns="False" 
                  CellPadding="0" CssClass="GridHeader" Width="100%" BorderWidth="1px" OnItemDataBound="dgDeviceError_ItemDataBound"
                  AlternatingItemStyle-CssClass="row3" ShowHeader="true" EnableViewState="true" OnSortCommand="dgDeviceError_SortCommand">                                                                         
               <HeaderStyle CssClass="THeader" Font-Bold="True" HorizontalAlign="Left" VerticalAlign="Middle" Height="26"/>               
               <ItemStyle Height="30" />               
               <Columns>                             
                    <asp:BoundColumn DataField="NotificationHistoryID" Visible="false" HeaderText="NotificationHistoryID" ></asp:BoundColumn>
                    <asp:HyperLinkColumn DataNavigateUrlField="MessageID" ItemStyle-Width="7%"  HeaderStyle-Width="7%" HeaderStyle-Wrap="false" DataTextField="MessageID" HeaderText="Message ID"
                      SortExpression="MessageID"></asp:HyperLinkColumn>
                    <asp:BoundColumn DataField="CreatedOn_UsersTime" HeaderText="Date/Time" SortExpression="CreatedOn_UsersTime" ItemStyle-Wrap="false" ItemStyle-Width="12%" HeaderStyle-Width="10%" ></asp:BoundColumn>
                    <asp:BoundColumn DataField="EventDescription" HeaderText="Notification Event" ItemStyle-Width="20%" HeaderStyle-Width="20%" ></asp:BoundColumn>
                    <asp:BoundColumn DataField="NotificationRecipientAddress" HeaderText="Device Number / Address" ItemStyle-Width="20%" HeaderStyle-Width="20%"></asp:BoundColumn>
                    <asp:BoundColumn DataField="FailureDescription" HeaderText="Error Description" ItemStyle-Width="41%" HeaderStyle-Width="41%"></asp:BoundColumn>
               </Columns>
              </asp:DataGrid>
             </div>
            </fieldset>
            &nbsp;
           </td>
          </tr>
          <tr>
              <td align="center" colspan="2">
               <asp:UpdatePanel ID="upnlNoRecords" runat="server" UpdateMode="Always">
                  <ContentTemplate>
                      <asp:Label ID="lblNorecord" runat="server" Font-Size="Small" ForeColor="green" Style="position: relative; text-align:center;"></asp:Label>
                  </ContentTemplate> 
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
  </ContentTemplate> 
    <Triggers>
            <asp:AsyncPostBackTrigger ControlID="cmbInstitution"  /> 
            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" /> 
    </Triggers>
</asp:UpdatePanel>   
</asp:Content>

