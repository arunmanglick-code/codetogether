<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="role_task.aspx.cs" Inherits="Vocada.CSTools.role_task" Theme="csTool" Title="CSTools: Manage Roles and Tasks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="InstitutionList" runat="server">
<ContentTemplate>
<script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
 <script language="javascript" type="text/javascript">
 var mapId = "role_task.aspx";
 /* Cancel Save*/
    function onCancelClick()
    {
         document.getElementById(txtDirectoryNameClientID).value = "";
    }
    /*Set value for the hidden varibale which is used to check value of form data change or not*/
    function formDataChange(value)
    {
        document.getElementById(hdnRolesDataChangedClientID).value = value;
    }

    //Check whether other record has been edited, then ask for conformation.
    function confirmOnDataChange()
    {
        if(document.getElementById(hdnRolesDataChangedClientID).value =="true")
        {
            if(confirm("Some data has been changed, do you want to continue?"))
            {
                document.getElementById(hdnRolesDataChangedClientID).value = "false";
                return true;
            }
            else
                return false;                    
        }
        return true;
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
    function setRoleDesc(controlId,hdnControlId)
    {
        var text = document.getElementById(controlId).value;
        document.getElementById(hdnControlId).value = text;
    }     
</script>
<table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">        
        <tr class="ContentBg">
          <td valign="top">
          <div style="overflow-y: Auto; height: 100%">
            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
             <tr>
                <td class="Hd1">
                    Manage Roles and Tasks
                </td>
              </tr>
            </table>
            <br />
            <table cellspacing="0" cellpadding="0" width="97%" border="0" align="center">
              <tr>
                 <td align="center">
                   <input type="hidden" id="hidPageIndex" value="0" runat="server" />
                   <asp:HiddenField ID="hdnRoleDataChanged" runat="server" Value="false" EnableViewState="true" />
                   <asp:HiddenField ID = "hdnEditedText" runat ="server" Value ="" />
                   <asp:HiddenField ID="hdnGridRoleDesc" runat="server" Value="-1" EnableViewState="true" />
                   <fieldset id="fldShift" class="fieldsetBlue" runat="server" style="height: 300px;">
                           <legend class="">Roles</legend></br>
                   <div id="phRoles" class="Tdiv">
                        <asp:DataGrid ID="dgRoles" AutoGenerateColumns="false" Width="100%" AllowSorting="true"
                                 runat="server" CssClass="GridHeader" 
                                 PageSize="10" OnSortCommand="dgRoles_SortCommand" CellPadding="0" OnEditCommand="dgRoles_EditCommand" 
                                 OnUpdateCommand="dgRoles_UpdateCommand" OnCancelCommand="dgRoles_CancelCommand" OnItemCreated="dgRoles_OnItemCreated"
                                 OnItemDataBound="dgRoles_ItemDataBound" BorderWidth="1" ItemStyle-Height="25" AlternatingItemStyle-Height="25" ItemStyle-CssClass="Row2" >
                                 <AlternatingItemStyle CssClass="Row3"></AlternatingItemStyle>
                                 <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                 <Columns>
                                    <asp:BoundColumn Visible="False" DataField="RoleID" ReadOnly="True" HeaderText="RoleID"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Defined Roles" SortExpression="RoleDescription">
                                        <ItemTemplate>
                                          <asp:HyperLink runat="server" id="lnkRoleDesc" Text='<%# DataBinder.Eval(Container, "DataItem.RoleDescription") %>' ></asp:HyperLink>
                                        </ItemTemplate>
                                        <EditItemTemplate >
                                            <asp:TextBox id="txtRoleDescription" runat="server" Width="375px" Text='<%# DataBinder.Eval(Container, "DataItem.RoleDescription") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:EditCommandColumn HeaderText="Edit"  ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
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
</asp:UpdatePanel> 
</asp:Content>

