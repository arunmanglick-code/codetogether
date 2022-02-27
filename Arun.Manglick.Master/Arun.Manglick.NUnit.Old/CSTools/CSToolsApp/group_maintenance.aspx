<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="group_maintenance.aspx.cs" Theme="csTool" Inherits="Vocada.CSTools.group_maintenance" Title="CSTools: Group Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelAGroupMonitor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>

            <script language="JavaScript" src="Javascript/Institution.js" type="text/JavaScript"></script>

            <script language="javascript" type="text/javascript">
 var mapId = "group_maintenance.aspx";

    var otherPostback =false;
    function Navigate(instId)
    {
        try
        {
            window.location.href = "group_monitor.aspx?InstitutionID=" + instId;
        }
        catch(_error)
        {
            return;
        }
    }
     //This function handles validation for Add note section.
  // This function will ask for conformation before deletion record
    function ConformBeforeDelete()
    {
        if(confirm("Are you sure you want to delete this subscriber record?"))
        {
            otherPostback =true;
            return true;
        }
        otherPostback =false;
        return false;                                          
    }     
        
            </script>

            <table style="height: 100%" align="center" width="98%" border="0" cellpadding="0"
                cellspacing="0">
                <tr>
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: auto; height: 100%; vertical-align: top; margin: 0 0 0 0;">
                            <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="BottomBg">
                                    <td valign="top">
                                        <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                            <tr>
                                                <td style="width: 70%" class="Hd1">
                                                    Group Maintenance
                                                </td>
                                                <td style="width: 14%" style="white-space: nowrap;" align="right" class="Hd1">
                                                    <asp:HyperLink ID="hlinkGroupPreferences" runat="server" CssClass="AccountInfoText">Group Preferences</asp:HyperLink>
                                                    &nbsp;&nbsp;<asp:HyperLink ID="hlinkFindings" runat="server" CssClass="AccountInfoText">
                                            Findings and Notifications</asp:HyperLink>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="background-color: White;" valign="top">
                                        <table width="98%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="upnlGrpInfo" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                            <fieldset class="fieldsetCBlue">
                                                                <legend class=""><b>Group Information</b></legend>
                                                                <table cellspacing="2" cellpadding="2" width="100%" border="0">
                                                                    <tr>
                                                                        <td style="width: 5%">
                                                                            &nbsp;</td>
                                                                        <td style="width: 12%">
                                                                            <asp:HiddenField ID="hdnTextChanged" runat="server" Value="false" />
                                                                        </td>
                                                                        <td style="width: 39%">
                                                                        </td>
                                                                        <td style="width: 17%">
                                                                        </td>
                                                                        <td style="width: 22%">
                                                                        </td>
                                                                        <td style="width: 5%">
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td>
                                                                            Group Name*:
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtGroupName" runat="server" MaxLength="100" Width="300"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            Directory:</td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="cmbDiretories" Width="200px" DataTextField="DirectoryDescription"
                                                                                DataValueField="DirectoryID">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr class="Row2">
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td style="white-space: nowrap;">
                                                                            Group 800 Number*:</td>
                                                                        <td>
                                                                            (<asp:TextBox Width="35px" ID="txtGroup800No1" runat="server" MaxLength="3"></asp:TextBox>)&nbsp;
                                                                            <asp:TextBox ID="txtGroup800No2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>
                                                                            -
                                                                            <asp:TextBox ID="txtGroup800No3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                                        </td>
                                                                        <td style="white-space: nowrap;">
                                                                            Ordering Clinician 800 Number*:</td>
                                                                        <td>
                                                                            (<asp:TextBox Width="35px" ID="txtRP800No1" runat="server" MaxLength="3"></asp:TextBox>)&nbsp;
                                                                            <asp:TextBox ID="txtRP800No2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>
                                                                            -
                                                                            <asp:TextBox ID="txtRP800No3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td style="white-space: nowrap;">
                                                                            Practice Type:</td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbPracticeType" runat="server" Width="148" DataValueField="PracticeTypeID"
                                                                                DataTextField="PracticeTypeDescription">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            Affiliation:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAffiliation" runat="server" MaxLength="100" Width="200"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td>
                                                                            Address 1:</td>
                                                                        <td style="width: 170px">
                                                                            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="100" Width="200"></asp:TextBox></td>
                                                                        <td>
                                                                            Address 2:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAddress2" runat="server" MaxLength="100" Width="200"></asp:TextBox></td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td>
                                                                            City:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCity" runat="server" MaxLength="50" Width="200"> </asp:TextBox></td>
                                                                        <td>
                                                                            State:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtState" runat="server" MaxLength="2" Columns="4"></asp:TextBox></td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td>
                                                                            Zip:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtZip" runat="server" MaxLength="10" Width="200"></asp:TextBox></td>
                                                                        <td>
                                                                            Phone:</td>
                                                                        <td>
                                                                            (<asp:TextBox Width="35px" ID="txtPhone1" runat="server" MaxLength="3"></asp:TextBox>)&nbsp;
                                                                            <asp:TextBox ID="txtPhone2" Width="35px" runat="server" MaxLength="3"></asp:TextBox>
                                                                            -
                                                                            <asp:TextBox ID="txtPhone3" Width="55px" runat="server" MaxLength="4"></asp:TextBox>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td>
                                                                            Time Zone:</td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbTimeZone" runat="server" Width="200px" DataValueField="TimeZoneID"
                                                                                DataTextField="Description">
                                                                            </asp:DropDownList></td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <br />
                                                                <p align="center">
                                                                    <asp:Button ID="btnSave" CssClass="Frmbutton" runat="server" Text="Save" OnClick="btnSave_Click" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="Frmbutton"
                                                                        Text="Cancel" OnClick="btnCancel_Click" />
                                                                </p>
                                                            </fieldset>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr class="BottomBg">
                                                <td>
                                                    <fieldset class="fieldsetCBlue">
                                                        <legend class="">&nbsp;<b>Subscribers&nbsp;/ Specialists</b></legend>
                                                        <br />
                                                        <div class="TDiv" id="divGroupMembers" runat="server" style="width: 100%;">
                                                            <asp:DataGrid ID="dgGroupMembers" runat="server" CssClass="GridHeader" Width="100%"
                                                                UseAccessibleHeader="True" OnItemDataBound="dgGroupMembers_ItemDataBound" AutoGenerateColumns="False"
                                                                AllowSorting="True" BorderStyle="Solid" BorderColor="LightGray" AlternatingItemStyle-CssClass="Row3">
                                                                <HeaderStyle CssClass="THeader" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="0px">
                                                                </HeaderStyle>
                                                                <ItemStyle Height="25px" />
                                                                <Columns>
                                                                    <asp:BoundColumn Visible="False" DataField="SubscriberID" HeaderText="SubscriberID">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="false" DataField="RoleID" HeaderText="RoleID"></asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="SpecialistID" HeaderText="SpecialistID">
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="User" ItemStyle-Width="35%">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DisplayName", "{0}") %>'
                                                                                NavigateUrl='<%# "~/user_profile.aspx?SubscriberID=" + DataBinder.Eval(Container, "DataItem.SubscriberID", "{0}") + "&RoleID=" + DataBinder.Eval(Container, "DataItem.RoleID", "{0}")  %>'
                                                                                ID="User" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="RoleDescription" HeaderText="Role" ItemStyle-Width="20%">
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="Email" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink ID="lnkEmail" Text='<%# DataBinder.Eval(Container, "DataItem.PrimaryEmail") %>'
                                                                                runat="server"></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="PrimaryPhone" HeaderText="Phone" ItemStyle-Width="10%"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Status" HeaderText="Status" ItemStyle-Width="10%"></asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="Edit" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink runat="server" Text="Edit" NavigateUrl='<%# "~/user_profile.aspx?SubscriberID=" + DataBinder.Eval(Container, "DataItem.SubscriberID", "{0}") + "&RoleID=" + DataBinder.Eval(Container, "DataItem.RoleID", "{0}")  %>'
                                                                                ID="Edit" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr class="BottomBg" style="background-color: White">
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr class="BottomBg" style="background-color: White">
                                                <td class="BottomBg">
                                                    <asp:UpdatePanel ID="upnlSupportNote" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                            <fieldset class="fieldsetCBlue">
                                                                <legend class="">&nbsp;<b>Support Notes</b></legend>
                                                                <table border="0" align="center" cellspacing="1" cellpadding="1" width="100%">
                                                                    <tr>
                                                                        <td style="width: 100%">
                                                                            <br />
                                                                            <div class="TDiv" id="divSupportNote" runat="server" style="width: 100%;">
                                                                                <asp:DataGrid ID="dgNotes" runat="server" CssClass="GridHeader" Width="100%" UseAccessibleHeader="True"
                                                                                    AutoGenerateColumns="False" AllowSorting="True" BorderStyle="Solid" BorderColor="LightGray"
                                                                                    AlternatingItemStyle-CssClass="Row3">
                                                                                    <HeaderStyle CssClass="THeader" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="0px">
                                                                                    </HeaderStyle>
                                                                                    <ItemStyle Height="25px" />
                                                                                    <Columns>
                                                                                        <asp:TemplateColumn HeaderText="&#160;&#160;&#160;&#160;">
                                                                                            <HeaderStyle Width="4%" />
                                                                                            <ItemStyle Width="4%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/ic_note.gif"></asp:Image>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:BoundColumn DataField="CreatedOn" HeaderText="Date/Time">
                                                                                            <HeaderStyle Width="20%" />
                                                                                            <ItemStyle Width="20%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="Author" HeaderText="Author">
                                                                                            <HeaderStyle Width="20%" />
                                                                                            <ItemStyle Width="20%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="Note" HeaderText="Note/Reason"></asp:BoundColumn>
                                                                                    </Columns>
                                                                                </asp:DataGrid>
                                                                            </div>
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100%" valign="middle">
                                                                            <div>
                                                                                <table id="Table6" cellspacing="1" cellpadding="1" width="700" border="0">
                                                                                    <tr valign="middle">
                                                                                        <td class="DivBg" valign="middle" align="left">
                                                                                            <asp:Label ID="lblNote" runat="server" CssClass="AccountHeaderText" Width="110"><b>&nbsp;&nbsp;Add Support Note:</b></asp:Label>
                                                                                            </td><td>
                                                                                            <asp:TextBox ID="txtAuthor" runat="server" CssClass="AccountHeaderText" MaxLength="25"
                                                                                                TabIndex="30"></asp:TextBox>
                                                                                            </td><td>
                                                                                            <asp:TextBox ID="txtNote" runat="server" CssClass="AccountHeaderText" MaxLength="500"
                                                                                                TabIndex="31" Width="528px" TextMode="MultiLine" Rows="3"></asp:TextBox>&nbsp;
                                                                                            </td><td>
                                                                                            <asp:Button ID="btnAddNote" runat="server" CssClass="Frmbutton" TabIndex="32" Text="Add"
                                                                                                Width="59px" OnClick="btnAddNote_Click" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 653px; height: 21px;">
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddNote" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr style="background-color: White">
                                                <td>
                                                    &nbsp;</td>
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
    </asp:UpdatePanel>
</asp:Content>
