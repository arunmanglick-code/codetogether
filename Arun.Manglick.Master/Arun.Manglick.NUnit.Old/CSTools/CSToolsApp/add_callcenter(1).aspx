<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="add_callcenter.aspx.cs"
    Inherits="Vocada.CSTools.add_callcenter" Title="CSTools: Add Agent Team" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlCallCenter" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>

            <script language="javascript" type="text/javascript">
              var mapId = "add_callcenter.aspx";
              function onSaveClick()
                {
                    var instituteID = "1";
                    instituteID = document.getElementById(cmbInstitutionsClientID).value;
                    var callCenterName = trim(document.getElementById(txtCallCenterNameClientID).value);
                    var errorMessage ="";
                    if(instituteID < 0)
                    {
                        errorMessage += "Please select the Institute.\r\n"
                        document.getElementById(cmbInstitutionsClientID).focus();
                    }
                    if(callCenterName.length == 0)
                    {
                        if(errorMessage.length == 0)
                            document.getElementById(txtCallCenterNameClientID).focus();
                        errorMessage += "Please enter Agent Team Name.\r\n"
                    }
                    
                    if(errorMessage.length > 0)
                    {
                        alert(errorMessage); 
                        return false;
                    }
                    return true;
                }
                
                function isValidCallCenterName()
                {                             
                    if(trim(document.getElementById(txtCallCenterNameClientID).value) != '')
                    {
                        if(document.getElementById(cmbInstitutionsClientID) == null)
                            document.getElementById(btnSaveClientID).disabled = false;
                        else
                        {    
                            if(document.getElementById(cmbInstitutionsClientID).value != "-1")
                                document.getElementById(btnSaveClientID).disabled = false;
                            else
                                document.getElementById(btnSaveClientID).disabled = true;
                        }
                    }  
                    else
                    {
                        document.getElementById(btnSaveClientID).disabled = true;
                    }  
                    
                    return;
                }
                
                function formDataChange(value)
                {
                    document.getElementById(hdnCallCenterDataChangedClientID).value = value;
                }
                
                // On Institution chnage check whether form data has been changed, if yes the ask for confirmation.
                function onComboChange()
                {
                    if(confirmOnDataChange())
                    {
                        __doPostBack('ctl00$ContentPlaceHolder1$cmbInstitutions','');
                                               
                        document.getElementById(hdnInstitutionValClientID).value = document.getElementById(cmbInstitutionsClientID).value ;
                        return true;
                    }
                    else
                    {
                        document.getElementById(cmbInstitutionsClientID).value = document.getElementById(hdnInstitutionValClientID).value;
                        return false;
                    }
                }
                
                 //Check whether other record has been edited, then ask for conformation.
                function confirmOnDataChange()
                {
                    if(document.getElementById(hdnCallCenterDataChangedClientID).value =="true")
                    {
                        if(confirm("Some data has been changed, do you want to continue?"))
                        {
                            document.getElementById(hdnCallCenterDataChangedClientID).value = "false";
                            return true;
                        }
                        else
                            return false;                    
                    }
                    return true;
                }
                
                
                 /* Check max length of the textbox, if exceed then dont take next input*/
                function setCallCenterName(controlId, itemIndex)
                {
                    var text = document.getElementById(controlId).value;
                    document.getElementById(hdnGridCCDescClientID).value = document.getElementById(controlId).value;
                    if (document.getElementById(hdnGridCCDescClientID).value.length <= 0)
                    {
                        alert('Please enter Agent Team Name.');
                        document.getElementById(controlId).focus();
                        return false;
                    }
                    else
                    {
                        if(itemIndex < 10)
                            __doPostBack('ctl00$ContentPlaceHolder1$grdCallCenters$ctl0' + itemIndex + '$ctl00', '');
                        else
                            __doPostBack('ctl00$ContentPlaceHolder1$grdCallCenters$ctl' + itemIndex + '$ctl00', '');
                            
                       return false;     
                    }
                    return true;
                }   
                
                function onGridCancelClick(controlId, itemIndex)
                {
                    if(confirmOnDataChange())
                    {
                        document.getElementById(hdnCallCenterDataChangedClientID).value = "false";
                        if(itemIndex < 10)
                            __doPostBack('ctl00$ContentPlaceHolder1$grdCallCenters$ctl0' + itemIndex + '$ctl01', '');
                        else
                            __doPostBack('ctl00$ContentPlaceHolder1$grdCallCenters$ctl' + itemIndex + '$ctl01', '');
                       return false;     
                    }
                    else
                    {
                        document.getElementById(controlId).focus();
                        return false;
                    }
                    return true;
                }
            </script>

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table height="100%" align="center" width="100%" style="background-color: White"
                                border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                            <tr>
                                                <td class="Hd1" style="height: 19px">
                                                    <asp:Label ID="lblDirectoryListHeader" runat="server" CssClass="UserCenterTitle">Add/Edit Agent Teams</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                            cellpadding="0" cellspacing="0">
                                            <tr valign="top" style="width: 100%;" align="center">
                                                <td class="Hd2">
                                                    <fieldset class="fieldsetCBlue">
                                                        <legend class="">Add Agent Team</legend>
                                                        <table align="center" width="60%" border="0" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                                <asp:Panel ID="pnlTabName" runat="server" Visible="true">
                                                                    <asp:HiddenField ID="hdnCallCenterDataChanged" runat="server" Value="false" EnableViewState="true" />
                                                                    <asp:HiddenField ID="hdnGridCCDesc" runat="server" Value="-1" EnableViewState="true" />
                                                                    <asp:HiddenField ID="hdnInstitutionVal" runat="server" Value="-1" EnableViewState="true" />
                                                                    <td align="left" style="white-space: nowrap; width: 35%;">
                                                                        <asp:UpdatePanel ID="upnlInstitutions" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                Institution Name:&nbsp;&nbsp;&nbsp;
                                                                                <asp:DropDownList ID="cmbInstitutions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitutions_OnSelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="cmbInstitutions" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                    <td style="white-space: nowrap; width: 25%;" align="left">
                                                                        &nbsp;&nbsp;Agent Team Name*:&nbsp;&nbsp;&nbsp;
                                                                        <asp:TextBox ID="txtCallCenterName" Columns="65" MaxLength="100" runat="server" Font-Bold="False"></asp:TextBox>&nbsp;</td>
                                                                    <td colspan="2" align="left" style="white-space: nowrap; width: 40%;">
                                                                        <asp:Button ID="btnSave" runat="server" Text=" Add " ValidationGroup="AddUnit" CssClass="Frmbutton"
                                                                            OnClick="btnSave_Click" Enabled="false" />&nbsp; &nbsp;&nbsp;&nbsp;
                                                                        <asp:ValidationSummary ID="vsmrAddUnitValidation" runat="server" ShowSummary="False"
                                                                            ValidationGroup="AddUnit" ShowMessageBox="True"></asp:ValidationSummary>
                                                                    </td>
                                                                </asp:Panel>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                            cellpadding="0" cellspacing="0">
                                            <tr valign="top" id="trGrid" style="width: 98%;" runat="server">
                                                <td class="Hd2" align="center">
                                                    <fieldset id="fldShift" class="fieldsetCBlue" runat="server">
                                                        </br> <legend class="">Agent Teams</legend></br>
                                                        <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div id="CallCenterDiv" class="TDiv" style="vertical-align: top; margin-bottom: 10px;
                                                                    margin-left: 0px; margin-right: 0px; height: 26px;">
                                                                    <asp:DataGrid ID="grdCallCenters" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                        AllowPaging="false" DataKeyField="CallCenterID" CellPadding="0" CssClass="GridHeader"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-Height="25px" HorizontalAlign="Center"
                                                                        Width="100%" BorderWidth="1px" OnEditCommand="grdCallCenters_EditCommand" OnSortCommand="grdCallCenters_SortCommand"
                                                                        OnCancelCommand="grdCallCenters_CancelCommand" OnUpdateCommand="grdCallCenters_UpdateCommand"
                                                                        OnItemCreated="grdCallCenters_ItemCreated">
                                                                        <AlternatingItemStyle CssClass="Row3"></AlternatingItemStyle>
                                                                        <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                        <Columns>
                                                                            <asp:TemplateColumn HeaderText="Agent Team Name" SortExpression="CallCenterName">
                                                                                <ItemStyle Width="60%" HorizontalAlign="Left" />
                                                                                <HeaderStyle Width="60%" HorizontalAlign="Left" />
                                                                                <ItemTemplate>
                                                                                    <asp:HyperLink ID="CallCenterName" runat="server" NavigateUrl='<%# "./callCenter_setup.aspx?CallCenterID=" + DataBinder.Eval(Container.DataItem, "CallCenterID").ToString() + "&CallCenterName=" + DataBinder.Eval(Container.DataItem, "CallCenterName").ToString() + "&InstitutionID=" +  DataBinder.Eval(Container.DataItem, "InstitutionID").ToString()%>'
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CallCenterName").ToString() %>'>
                                                                                    </asp:HyperLink>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtGDCallCenterName" Text='<%# DataBinder.Eval(Container.DataItem, "CallCenterName").ToString() %>'
                                                                                        runat="server" MaxLength="100" Width="600" OnChange="formDataChange(true)"></asp:TextBox>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:BoundColumn DataField="isActive" HeaderText="Status" ReadOnly="true">
                                                                                <HeaderStyle Width="10%" />
                                                                                <ItemStyle Height="21px" />
                                                                            </asp:BoundColumn>
                                                                            <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" HeaderText="Edit" UpdateText="Update">
                                                                                <HeaderStyle Width="10%" HorizontalAlign="left" />
                                                                                <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                            </asp:EditCommandColumn>
                                                                            <asp:BoundColumn Visible="False" DataField="CallCenterID" ReadOnly="True" HeaderText="CallCenterID">
                                                                            </asp:BoundColumn>
                                                                        </Columns>
                                                                    </asp:DataGrid>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="grdCallCenters" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </fieldset>
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
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="cmbInstitutions" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="grdCallCenters"  />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
