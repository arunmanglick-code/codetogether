<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="add_directory.aspx.cs"
    Inherits="Vocada.CSTools.add_directory" Theme="csTool" Title="CSTools: Add/Edit OC Directory" smartNavigation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelDirectoryList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
        <script language="javascript" type="text/javascript">

            var mapId = "add_directory.aspx";
            
            /* Check for required input before go for save */
            function validateDirName()
            {                
                if(document.getElementById(hdnModeClientID).value == 'edit')
                {
                    window.event.returnValue = null;                         
                }
                else
                {
                return isAlphaNumericKeyStroke();
                }
            }
            function isValidDirName()
            {                             
                if(trim(document.getElementById(txtDirectoryNameClientID).value) != '')
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
            function onSaveClick()
            {
                var instituteID = "1";
                var hdnIsSystemAdmin = document.getElementById(hdnIsSystemAdminClientID).value.trim() ;
                if (hdnIsSystemAdmin == "1")
                {
                  instituteID = document.getElementById(cmbInstitutionsClientID).value;
                }
                var directoryName = trim(document.getElementById(txtDirectoryNameClientID).value.trim());
                var errorMessage ="";
                if(instituteID < 0)
                {
                    errorMessage += "Please select the Institute.\r\n"
                    document.getElementById(cmbInstitutionsClientID).focus();
                }
                if(directoryName.length == 0)
                {
                    if(errorMessage.length == 0)
                        document.getElementById(txtDirectoryNameClientID).focus();
                    errorMessage += "Please enter Directory Name.\r\n"
                }
                
                if(errorMessage.length > 0)
                {
                    alert(errorMessage); 
                    return false;
                }
                return true;
            }
        
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
            /* Cancel Save*/
            function onCancelClick()
            {
                 document.getElementById(txtDirectoryNameClientID).value = "";
            }
         /*Set value for the hidden varibale which is used to check value of form data change or not*/
            function formDataChange(value)
            {
                document.getElementById(hdnOCDirectoryDataChangedClientID).value = value;
            }

            //Check whether other record has been edited, then ask for conformation.
            function confirmOnDataChange()
            {
                if(document.getElementById(hdnOCDirectoryDataChangedClientID).value =="true")
                {
                    if(confirm("Some data has been changed, do you want to continue?"))
                    {
                        document.getElementById(hdnOCDirectoryDataChangedClientID).value = "false";
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
                //debugger
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
            /* Check max length of the textbox, if exceed then dont take next input*/
            function CheckMaxLenght(controlId, length)
            {
                var text = document.getElementById(controlId).value;
                if(text.length > length)
                {
                    document.getElementById(controlId).value = text.substring(0,length);
                }        
            }
            
            /* Check max length of the textbox, if exceed then dont take next input*/
            function setDirectoryDesc(controlId, itemIndex)
            {
                var text = document.getElementById(controlId).value;
                document.getElementById(hdnGridDirectoryDescClientID).value = document.getElementById(controlId).value.trim();
                if (document.getElementById(hdnGridDirectoryDescClientID).value.length <= 0)
                {
                    alert('Please enter Directory Name.');
                    document.getElementById(controlId).focus();
                    return false;
                }
                else
                {
                    document.getElementById(hdnOCDirectoryDataChangedClientID).value = "false";
                    if(itemIndex < 10)
                        __doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl0' + itemIndex + '$ctl00', '');
                    else
                        __doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl' + itemIndex + '$ctl00', '');
                    return false;    
                }
            }   
            
            function onGridCancelClick(controlId, itemIndex)
            {
                if(confirmOnDataChange())
                {
                    if(itemIndex < 10)
                        __doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl0' + itemIndex + '$ctl01', '');
                    else
                        __doPostBack('ctl00$ContentPlaceHolder1$grdDirectories$ctl' + itemIndex + '$ctl01', '');
                     return false;;   
                }
                else
                {
                    document.getElementById(controlId).focus();
                    return false;
                }
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
                                                    <asp:Label ID="lblDirectoryListHeader" runat="server" CssClass="UserCenterTitle">Add/Edit OC Directory</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                            cellpadding="0" cellspacing="0">
                                            <tr valign="top" style="width: 100%;" align="center">
                                                <td class="Hd2">
                                                    <fieldset class="fieldsetCBlue">
                                                        <legend class="">Add OC Directory</legend>
                                                        <table align="center" width="60%" border="0" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                                <td align="left" style="white-space: nowrap; width: 35%;">
                                                                    <asp:HiddenField ID="hdnOCDirectoryDataChanged" runat="server" Value="false" EnableViewState="true" />
                                                                    <asp:HiddenField ID="hdnInstitutionVal" runat="server" Value="-1" EnableViewState="true" />
                                                                    <asp:HiddenField ID="hdnGridDirectoryDesc" runat="server" Value="-1" EnableViewState="true" />
                                                                    Institution Name:&nbsp;&nbsp;&nbsp;
                                                                    <asp:DropDownList ID="cmbInstitutions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitutions_OnSelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="lblInstName" runat="server" Visible="False"></asp:Label>
                                                                    <asp:HiddenField ID="hdnIsSystemAdmin" runat="server" Value="1" />
                                                                </td>
                                                                <td style="white-space: nowrap; width: 25%;" align="left">
                                                                    &nbsp;&nbsp;Directory Name*:&nbsp;&nbsp;&nbsp;
                                                                    <asp:TextBox ID="txtDirectoryName" Columns="65" MaxLength="100" runat="server" Font-Bold="False"></asp:TextBox>&nbsp;</td>
                                                                <td colspan="2" align="left" style="white-space: nowrap; width: 40%;">
                                                                    <asp:Button ID="btnSave" runat="server" Text=" Save " ValidationGroup="AddUnit" CssClass="Frmbutton"
                                                                        OnClick="btnSave_Click" Enabled="false" />&nbsp; &nbsp;&nbsp;&nbsp;
                                                                    <asp:ValidationSummary ID="vsmrAddUnitValidation" runat="server" ShowSummary="False"
                                                                        ValidationGroup="AddUnit" ShowMessageBox="True"></asp:ValidationSummary>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                            <tr valign="top" id="trGrid" runat="server">
                                                <td class="Hd2" align="center">
                                                    <fieldset id="fldShift" class="fieldsetCBlue" runat="server">
                                                        </br> <legend class="">OC Directory</legend></br>
                                                                <div id="DepartmentDiv" class="TDiv" 
                                                                    style="vertical-align: top; margin-bottom: 10px;"><asp:DataGrid ID="grdDirectories" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                        AllowPaging="false" DataKeyField="DirectoryID" CellPadding="0" CssClass="GridHeader"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-Height="25px" HorizontalAlign="Center"
                                                                        Width="100%" BorderWidth="1px" OnCancelCommand="grdDirectories_CancelCommand"
                                                                        OnEditCommand="grdDirectories_EditCommand" OnSortCommand="grdDirectories_SortCommand"
                                                                        OnUpdateCommand="grdDirectories_UpdateCommand" OnItemCreated="grdDirectories_ItemCreated">
                                                                        <HeaderStyle CssClass="THeader" Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateColumn HeaderText="Directory Name" SortExpression="DirectoryName">
                                                                                <ItemStyle Width="90%" HorizontalAlign="Left" />
                                                                                <HeaderStyle Width="90%" HorizontalAlign="Left" />
                                                                                <ItemTemplate>
                                                                                    <asp:HyperLink ID="DirectoryName" runat="server" NavigateUrl='<%# "./directory_maintenance.aspx?DirectoryID=" + DataBinder.Eval(Container.DataItem, "DirectoryId").ToString() + "&DirectoryName=" + DataBinder.Eval(Container.DataItem, "DirectoryDescription").ToString() + "&InstitutionName=" + cmbInstitutions.SelectedItem.Text%>'
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "DirectoryDescription").ToString() %>'>
                                                                                    </asp:HyperLink>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtGrdDirectoryName" Text='<%# DataBinder.Eval(Container.DataItem, "DirectoryDescription").ToString() %>'
                                                                                        runat="server" MaxLength="100" Width="600" OnChange="formDataChange(true)"></asp:TextBox>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" HeaderText="Edit" UpdateText="Update">
                                                                                <HeaderStyle Width="10%" HorizontalAlign="left" />
                                                                                <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                            </asp:EditCommandColumn>
                                                                        </Columns>
                                                                    </asp:DataGrid></div><asp:HiddenField ID="hdnMode" Value="" runat="server" EnableViewState="true" />
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
        </asp:UpdatePanel>
</asp:Content>
