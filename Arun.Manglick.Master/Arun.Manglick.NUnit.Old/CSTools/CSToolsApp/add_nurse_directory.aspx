<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" Theme="csTool" CodeFile="add_nurse_directory.aspx.cs" Inherits="Vocada.CSTools.add_nurse_directory" Title="CSTools: Add/Edit Nurse Directory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="InstitutionList" runat="server">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>

            <script language="javascript" type="text/javascript">
                 var mapId = "add_nurse_directory.aspx";
                /* Navigate to message Center page*/
                function Navigate()
                {
                    try
                    {
                        window.location.href = "message_center.aspx";
                    }
                    catch(_error)
                    {
                        return;
                    }
                }

                /*Set value for the hidden varibale which is used to check value of form data change or not*/
                function formDataChange(value)
                {
                    document.getElementById(hdnNurseDirectoryDataChangedClientID).value = value;
                }

                //Check whether other record has been edited, then ask for conformation.
                function confirmOnDataChange()
                {
                if(document.getElementById(hdnNurseDirectoryDataChangedClientID).value =="true")
                {
                    if(confirm("Some data has been changed, do you want to continue?"))
                    {
                        document.getElementById(hdnNurseDirectoryDataChangedClientID).value = "false";
                        return true;
                    }
                    else
                        return false;                    
                }
                return true;
                }

                function isValidDirName()
                {                                  
                    if(trim(document.getElementById(txtDirectoryNameClientID).value) != '')
                    {
                        if(document.getElementById(cmbInstitutionClientID) == null)
                            document.getElementById(btnSaveClientID).disabled = false;
                        else
                        {    
                            if(document.getElementById(cmbInstitutionClientID).value != "-1")
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
                
                // On Institution chnage check whether form data has been changed, if yes the ask for confirmation.
                function onComboChange()
                {
                    if(confirmOnDataChange())
                    {
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
                
                
                /* Check max length of the textbox, if exceed then dont take next input*/
                function CheckMaxLenght(controlId, length)
                {
                    var text = document.getElementById(controlId).value.trim();
                    if(text.length > length)
                    {
                        document.getElementById(controlId).value = text.substring(0,length);
                    }        
                }
                
                /* Check max length of the textbox, if exceed then dont take next input*/
                function setDirectoryDesc(controlId, itemIndex)
                {
                    var text = document.getElementById(controlId).value.trim();
                    document.getElementById(hdnGridDirectoryDescClientID).value = document.getElementById(controlId).value.trim();
                    if (document.getElementById(hdnGridDirectoryDescClientID).value.length <= 0)
                    {
                        alert('Please enter name for the nurse directory');
                        document.getElementById(controlId).focus();
                        return false;
                    }
                    else
                    {
                        document.getElementById(hdnNurseDirectoryDataChangedClientID).value = "false";
                        if(itemIndex < 10)
                            __doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl0' + itemIndex + '$ctl00', '');
                        else
                            __doPostBack('ctl00$ContentPlaceHolder1$grdNurseDirectory$ctl' + itemIndex + '$ctl00', '');
                        return false    
                    }
                }    
                
                /*Call save wen enter press in directory desc textbox*/
                function callSave()
                {
                    var returnValue = false;
                    var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;
                    if(keyCode ==13)
                        __doPostBack('ctl00_ContentPlaceHolder1_btnSave', '');
                }
            </script>

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                <tr>
                                    <td class="Hd1">
                                        Add/Edit Nurse Directory
                                    </td>
                                </tr>
                            </table>
                            <table width="98%" border="0" cellpadding="=0" cellspacing="0" align="center">
                                <tr align="center">
                                    <td class="ContentBg">
                                        <fieldset class="fieldsetCBlue">
                                            <legend class=""><b>Add Nurse Directory</b></legend>
                                            <table id="Table1" cellspacing="1" cellpadding="2" width="60%" border="0" align="center">
                                                <tr>
                                                    <td style="white-space: nowrap; width: 10%;">
                                                        <asp:HiddenField ID="hdnNurseDirectoryDataChanged" runat="server" Value="false" EnableViewState="true" />
                                                        <asp:HiddenField ID="hdnInstitutionVal" runat="server" Value="-1" EnableViewState="true" />
                                                        <asp:HiddenField ID="hdnGridDirectoryDesc" runat="server" Value="-1" EnableViewState="true" />
                                                        <asp:HiddenField ID="hdnIsSystemAdmin" runat="server" Value="1" EnableViewState="true" />
                                                        <asp:UpdatePanel ID="upnlInstitute" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                Institution Name:&nbsp;&nbsp;&nbsp;
                                                                <asp:DropDownList ID="cmbInstitution" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitution_SelectedIndexChanged"
                                                                    DataTextField="InstitutionName" DataValueField="InstitutionID">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblInstName" runat="server" Visible="False"></asp:Label>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbInstitution"  />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td style="white-space: nowrap; width: 20%;">
                                                        &nbsp;&nbsp;&nbsp;Directory Name*:&nbsp;&nbsp;&nbsp;
                                                        <asp:TextBox ID="txtDirectoryName" Columns="68" MaxLength="100" runat="server" Font-Bold="false"></asp:TextBox>&nbsp;</td>
                                                    <td style="white-space: nowrap; width: 70%;">
                                                        <asp:Button ID="btnSave" Text=" Save " runat="server" CssClass="Frmbutton" OnClick="btnSave_Click"
                                                            UseSubmitBehavior="true" Enabled="false" />
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                <tr valign="top" align="center" id="trGrid" runat="server">
                                    <td class="Hd2">
                                        <fieldset id="Fieldset1" class="fieldsetCBlue" runat="server">
                                            </br> <legend class="">Nurse Directory</legend></br>
                                            <div id="AddNurseDirDiv" class="TDiv" style="vertical-align: top; margin-bottom: 10px;">
                                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DataGrid ID="grdNurseDirectory" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            AllowPaging="false" DataKeyField="NurseDirectoryID" CellPadding="0" CssClass="GridHeader"
                                                            ItemStyle-HorizontalAlign="center" ItemStyle-Height="25px" HorizontalAlign="Center"
                                                            Width="100%" BorderWidth="1px" OnCancelCommand="grdNurseDirectory_CancelCommand"
                                                            OnEditCommand="grdNurseDirectory_EditCommand" OnSortCommand="grdNurseDirectory_SortCommand"
                                                            OnUpdateCommand="grdNurseDirectory_UpdateCommand" OnItemCreated="grdNurseDirectory_ItemCreated">
                                                            <HeaderStyle CssClass="THeader" Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Directory Name" SortExpression="DirectoryDescription">
                                                                    <ItemStyle Width="90%" HorizontalAlign="Left" />
                                                                    <HeaderStyle Width="90%" HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGridDirectoryName" Text='<%# DataBinder.Eval(Container.DataItem, "DirectoryDescription").ToString() %>'
                                                                            ToolTip='<%# DataBinder.Eval(Container.DataItem, "DirectoryDescription") %>'
                                                                            runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtGridDirectoryName" Text='<%# DataBinder.Eval(Container.DataItem, "DirectoryDescription").ToString() %>'
                                                                            runat="server" MaxLength="100" Width="600" ValidationGroup="updatePanelEditDirectory"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvGridDirectoryName" runat="server" Display="None"
                                                                            ErrorMessage="You Must Enter A Directory Name" ControlToValidate="txtGridDirectoryName"
                                                                            ValidationGroup="updatePanelEditDirectory"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:EditCommandColumn HeaderText="Edit" UpdateText="Update" CancelText="Cancel"
                                                                    EditText="Edit" CausesValidation="false" ValidationGroup="updatePanelEditDirectory">
                                                                    <HeaderStyle Width="10%" HorizontalAlign="left" />
                                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                </asp:EditCommandColumn>
                                                            </Columns>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:DataGrid>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdNurseDirectory" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table border="0">
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvalInstitution" Style="z-index: 101; left: 438px;
                                            position: absolute; top: 281px" runat="server" Display="None" ErrorMessage="You Must Select A Institution"
                                            SetFocusOnError="true" ControlToValidate="cmbInstitution" InitialValue="-1"></asp:RequiredFieldValidator>
                                        <asp:ValidationSummary ID="ValidationSummary1" Style="z-index: 107; left: 416px;
                                            position: absolute; top: 163px" runat="server" ShowSummary="False" ShowMessageBox="True">
                                        </asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvalDirName" Style="z-index: 101; left: 438px; position: absolute;
                                            top: 281px" runat="server" Display="None" ErrorMessage="You Must Enter A Directory Name"
                                            SetFocusOnError="true" ControlToValidate="txtDirectoryName"></asp:RequiredFieldValidator>
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
