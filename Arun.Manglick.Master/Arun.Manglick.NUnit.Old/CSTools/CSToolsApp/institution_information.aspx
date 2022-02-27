<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="institution_information.aspx.cs" Inherits="Vocada.CSTools.institution_information" Theme="csTool" Title="CSTools: Institution List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="upnlInstitutionList" runat="server">
        <ContentTemplate>
    <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
    <script language="javascript" type="text/JavaScript">
    var otherPostback =false;
    var mapId  = "institution_information.aspx";
   
    function Navigate(lnk)
    {
        try
        {
          if (lnk == "EditInstitution")
            window.location.href = "edit_institution.aspx";
//          if (lnk == "GroupMonitor")
//            window.location.href = "group_monitor.aspx";
//          else if (lnk == "MessageCenter")
//            window.location.href = "message_center.aspx";
          else if (lnk == "AddDirectory")
            window.location.href = "add_directory.aspx";
          else if (lnk == "AddNurseDirectory")
            window.location.href = "add_nurse_directory.aspx";
            
            
        }
        catch(_error)
        {
            return;
        }
    }
   
 </script>
    <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
            <tr style="background-color:White" width="100%">
                <td class="DivBg" valign="top" width="100%">
                <div style="overflow-y:Auto; height: 100%;vertical-align:top;margin:0 0 0 0; ">
                    <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                      <tr>
                        <td width="62%" class="Hd1">Institution List</td>
                        <td width="38%" align="right" class="Hd1">
                            &nbsp;
                            <asp:HyperLink ID="hlinkAddInstitution" runat="server" CssClass="AccountInfoText"
                                                NavigateUrl="./add_institution.aspx">Add Institution</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                         </td>
                        </tr>
                     </table><br />
                     <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                      <tr>
                        <td>
                            <input type="hidden" id="hidPageIndex" value="0" runat="server" />
                            <div id="phInstitutions" class="Tdiv">
                            <asp:DataGrid ID="dgInstitution" AutoGenerateColumns="false" Width="100%" AllowSorting="true"
                                 runat="server" CssClass="GridHeader" OnPageIndexChanged="dgInstitution_PageIndexChanged"
                                 PageSize="10" OnSortCommand="dgInstitution_SortCommand" CellPadding="0" ItemStyle-CssClass="Row2">
                                 <AlternatingItemStyle CssClass="Row3"></AlternatingItemStyle>
                                 <HeaderStyle CssClass="THeader" Height="25" Font-Bold="True"></HeaderStyle>
                                 <Columns>
                                    <asp:BoundColumn DataField="InstitutionID" Visible="false" HeaderText="InstitutionID"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="InstitutionName" Visible="false" HeaderText="InstitutionName"></asp:BoundColumn>
                                    <asp:HyperLinkColumn DataNavigateUrlField="InstitutionID" DataNavigateUrlFormatString="./edit_institution.aspx?InstitutionID={0}"
                                        DataTextField="InstitutionName" HeaderStyle-Width="50%"  HeaderText="Institution" SortExpression="InstitutionName">
                                        <ItemStyle Height="25px" />
                                    </asp:HyperLinkColumn>
                                    <asp:HyperLinkColumn DataNavigateUrlField="InstitutionID" DataNavigateUrlFormatString="./add_directory.aspx?InstitutionID={0}"
                                         HeaderText="OC Directory" Text="Add" HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="10%">
                                        <ItemStyle Height="25px" HorizontalAlign="center" />
                                    </asp:HyperLinkColumn>
                                   <asp:HyperLinkColumn DataNavigateUrlField="InstitutionID" DataNavigateUrlFormatString="./add_nurse_directory.aspx?InstitutionID={0}"
                                         HeaderText="Nurse Directory" Text="Add" HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="10%">
                                        <ItemStyle Height="25px" HorizontalAlign="center" />
                                    </asp:HyperLinkColumn>
                                    <asp:HyperLinkColumn DataNavigateUrlField="InstitutionID" DataNavigateUrlFormatString="./group_monitor.aspx?InstitutionID={0}"
                                         HeaderText="Group Monitor" Text="Group Monitor" HeaderStyle-HorizontalAlign="center"  HeaderStyle-Width="12%">
                                        <ItemStyle Height="25px"  HorizontalAlign="center" />
                                    </asp:HyperLinkColumn>
                                    <asp:HyperLinkColumn DataNavigateUrlField="InstitutionID" DataNavigateUrlFormatString="./edit_institution.aspx?InstitutionID={0}"
                                        Text="Edit" HeaderText="Edit" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"  HeaderStyle-Width="8%">
                                    </asp:HyperLinkColumn>
                                    
                                 </Columns>
                                  <PagerStyle NextPageText="Next" PrevPageText="Previous" Height="12%" Font-Bold="True"
                                     Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                     HorizontalAlign="Right" />
                             </asp:DataGrid> 
                           </div>        
                        </td>
                      </tr>  
                     </table> 
                   </div> 
                  </td>
                </tr>
              </table>
        </ContentTemplate> 
     </asp:UpdatePanel>          
</asp:Content>

