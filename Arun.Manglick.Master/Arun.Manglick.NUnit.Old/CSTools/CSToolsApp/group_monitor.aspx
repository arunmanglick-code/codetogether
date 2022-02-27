<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="group_monitor.aspx.cs"
    Inherits="Vocada.CSTools.group_monitor" Theme="csTool" Title="CSTools: Group Monitor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanelAGroupMonitor" UpdateMode="conditional" runat="server">
        <ContentTemplate>
  <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
  <script language="javascript" type="text/JavaScript">
var otherPostback =false;
var mapId = "group_monitor.aspx";

/*To Get Confirmation before Delete.*/
 function ConformBeforeDelete()
 {
    if(confirm('Are you sure you want to delete Group?' ))
    {
        otherPostback =true;
        return true;
    }
   otherPostback =false;
    return false; 
 }
 
            </script>

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                <tr>
                                    <td width="62%" class="Hd1">
                                        Group Monitor</td>
                                    <td width="38%" align="right" class="Hd1">
                                        &nbsp;
                                        <asp:HyperLink ID="hlinkAddGroup" runat="server" CssClass="AccountInfoText" NavigateUrl="#">Add Group</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" cellpadding="0" width="98%" border="0" style="margin-left: 0px;"
                                align="center">
                                <tr>
                                    <td align="center">
                                        <fieldset class="fieldsetCBlue" >
                                            <legend class="">Select</legend>
                                            <table width="20%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td style="white-space: nowrap; width: 50%;">
                                                        Institution Name:&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                    <td style="white-space: nowrap; width: 50%;">
                                                        <asp:Label ID="lblInstName" runat="server" Visible="False"></asp:Label>
                                                        <asp:DropDownList ID="cmbInstitutions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitutions_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdnIsSystemAdmin" runat="server" Value="1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <input type="hidden" id="hidPageIndex" value="0" runat="server" /></br></br>
                                        <fieldset class="fieldsetCBlue">
                                            <legend class="">Groups</legend></br>
                                            <div class="TDiv" id="divGroups">
                                                <asp:DataGrid ID="dgGroups" AutoGenerateColumns="false" Width="100%" Height="2%"
                                                    AllowSorting="true" runat="server" CssClass="GridHeader" OnSortCommand="dgGroups_SortCommand"
                                                    CellPadding="0" OnDeleteCommand="dgGroups_DeleteCommand" ItemStyle-Height="25"
                                                    AlternatingItemStyle-Height="25">
                                                    <AlternatingItemStyle CssClass="Row3"></AlternatingItemStyle>
                                                    <HeaderStyle VerticalAlign="middle" CssClass="THeader" HorizontalAlign="Left" Font-Bold="True">
                                                    </HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn Visible="false" DataField="GroupID" ReadOnly="true" FooterText="GroupID">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn HeaderText="Group" SortExpression="GroupName" DataField="GroupName">
                                                            <HeaderStyle Width="40%" />
                                                            <ItemStyle Width="40%"/>
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="GroupID" DataNavigateUrlFormatString="./message_center.aspx?GroupID={0}"
                                                            Text="Messages" HeaderText="Messages">
                                                            <HeaderStyle Width="10%" HorizontalAlign="center" Wrap="false"  />
                                                            <ItemStyle Width="10%" HorizontalAlign="center" Wrap="false"  />
                                                        </asp:HyperLinkColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="GroupID" DataNavigateUrlFormatString="group_maintenance.aspx?GroupID={0}"
                                                            Text="Group Maintenance" HeaderText="Group Maintenance">
                                                            <HeaderStyle Width="10%" HorizontalAlign="center" Wrap="false"  />
                                                            <ItemStyle Width="10%" HorizontalAlign="center" Wrap="false" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="GroupID" DataNavigateUrlFormatString="group_preferences.aspx?GroupID={0}"
                                                            Text="Group Preferences" HeaderText="Group Preferences">
                                                            <HeaderStyle Width="10%" HorizontalAlign="center" Wrap="false"  />
                                                            <ItemStyle Width="10%" HorizontalAlign="center" Wrap="false" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="GroupID" DataNavigateUrlFormatString="group_maintenance_findings.aspx?GroupID={0}"
                                                            Text="Findings and Notifications" HeaderText="Findings and Notifications">
                                                            <HeaderStyle Width="15%"  HorizontalAlign="center" Wrap="false" />
                                                            <ItemStyle Width="15%" HorizontalAlign="center" Wrap="false"/>
                                                        </asp:HyperLinkColumn>
                                                        <asp:TemplateColumn HeaderText="Inactive" Visible="false">
                                                            <HeaderStyle Width="10%"  HorizontalAlign="center" />
                                                            <ItemStyle Width="10%" HorizontalAlign="center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" Text="Inactive" runat="server" CausesValidation="false"
                                                                    CommandName="Delete" OnClientClick="return ConformBeforeDelete();"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>
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
             <asp:AsyncPostBackTrigger ControlID="cmbInstitutions"  />
       </Triggers>
    </asp:UpdatePanel>
</asp:Content>
