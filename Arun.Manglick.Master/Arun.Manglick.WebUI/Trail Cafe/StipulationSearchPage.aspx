<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="~/MasterPage.master"
    Culture="auto" UICulture="auto" AutoEventWireup="true" CodeFile="StipulationSearchPage.aspx.cs"
    Inherits="StipulationSearchPage" %>

<%@ Register Src="~/UserControls Cafe/UserControls/SearchPagination.ascx" TagName="SearchPagination"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script type="text/javascript" language="javascript">
        
        //Local method to check if any single checkbox is selected;
        function DeleteCheckLocal()
        {
            var errMsg = 'DeleteConfirm';
            var lblDel = window.document.getElementById('<%=lblNoError.ClientID %>');
            
            if(lblDel != null)
                lblDel.style.display = 'None'; 
            lblDel = window.document.getElementById('<%=lblError.ClientID %>');
            if(lblDel != null)
                lblDel.style.display = 'None'; 
                
            lblDel = window.document.getElementById('<%=lblDelete.ClientID %>');    
            
            if(DeleteCheck('<%= gdvStipulationSearch.ClientID %>','_chkSelect',2,errMsg))
            {
                if(lblDel != null)
                    lblDel.style.display = 'None'; 
                if(window.confirm(errMsg))
                    return true;
                else
                    return false;
            }
            else
            {
                if(lblDel != null)
                    lblDel.style.display = 'Block'; 
                return false;
            }
            return true;
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <div style="width: 100%">
        <div style="width: 100%">
            <table width="100%" border="0" cellpadding="1" cellspacing="0">
                <tr>
                    <td class="title" style="height: 29px">
                        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:SearchResultTitle %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
                        <asp:Label ID="lblNoError" runat="server" CssClass="validation-no-error" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDelete" runat="server" CssClass="validation-error-select" Text="<%$ Resources:ErrorMessages,SelectCheckBox %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" width="100%" cellpadding="1" cellspacing="3">
                            <!-- Row 1 -->
                            <tr>
                                <td colspan="7" class="formheader">
                                    <asp:Label Width="100%" ID="lblSearchStipulations" runat="server" Text="<%$ Resources:LabelSearchStipulations %>"></asp:Label>
                                </td>
                            </tr>
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LabelName %>"></asp:Label>
                                </td>
                                <td style="width: 12%" class="inputcolumn">
                                    <asp:TextBox ID="txtName" onfocus="Javascript:OnFocusTextbox();" onblur="Javascript:OnFocusOutTextbox()"
                                        runat="server" MaxLength="15" class="inputfield" Width="95%"></asp:TextBox>
                                </td>
                                <td class="requiredcolumn" style="width: 15%; text-align: left;">
                                    <asp:DropDownList ID="ddlMatchCriteria" runat="server" class="inputfield">
                                    </asp:DropDownList>
                                </td>
                                <td class="requiredcolumn" style="width: 1%">
                                    &nbsp;
                                </td>
                                <td class="labelcolumn" valign="top" style="width: 10%">
                                    <asp:Label ID="lblUnassigned" runat="server" Text="<%$ Resources:LabelAssignment %>"></asp:Label>
                                </td>
                                <td class="inputcolumn" valign="top" style="width: 51%">
                                    <asp:DropDownList ID="ddlAssignment" runat="server" class="inputfield">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <!-- Row 3 -->
                            <tr>
                                <td class="requiredcolumn">
                                </td>
                                <td class="labelcolumn">
                                    <asp:Label ID="lblType" runat="server" Text="<%$ Resources:LabelType %>"></asp:Label>
                                </td>
                                <td class="inputcolumn" style="width: 27%;" colspan="2">
                                    <asp:DropDownList ID="ddlType" runat="server" class="inputfield">
                                    </asp:DropDownList>
                                </td>
                                <td class="requiredcolumn">
                                </td>
                                <td class="labelcolumn" valign="top">
                                    <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:LabelStatus %>"></asp:Label>
                                </td>
                                <td class="inputcolumn" valign="top">
                                    <asp:DropDownList ID="ddlStatus" runat="server" class="inputfield">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <!-- Row 4 -->
                            <tr>
                                <td class="requiredcolumn">
                                </td>
                                <td class="labelcolumn">
                                    <asp:Label ID="lblDescription" runat="server" Text="<%$ Resources:LabelDescription %>"></asp:Label>
                                </td>
                                <td class="inputcolumn" colspan="2">
                                    <asp:TextBox ID="txtDescription" runat="server" class="inputfield" MaxLength="75"
                                        TextMode="MultiLine" Columns="25" Rows="3" Width="84%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%">
            <table id="tblResultTitle" visible="false" runat="server" border="0" width="100%"
                cellpadding="1" cellspacing="3">
                <tr>
                    <td width="100%" class="formheader">
                        <asp:Label ID="lblResultTitle" Text="<%$ Resources:SearchResultsTitle %>" Visible="false"
                            Width="100%" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table border="0" width="70%" cellpadding="1" cellspacing="3">
                <tbody>
                    <tr>
                        <td class="Hidden" align="left" colspan="3">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td align="right">
                                        <div id="pagerTop"  runat="server">
                                            <table cellpadding="1" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <uc1:SearchPagination ID="searchPaginationTop" runat="server" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3">
                            <div id="divContainer" runat="server" class="divContainerClass">
                                <asp:GridView BorderStyle="None" ID="gdvStipulationSearch" runat="server" Width="98%"
                                    AutoGenerateColumns="False" CellPadding="3" OnRowDataBound="gdvStipulationSearch_RowDataBound"
                                    OnDataBound="gdvStipulationSearch_DataBound" OnSorting="gdvStipulationSearch_Sorting"
                                    EnableSortingAndPagingCallbacks="true" AllowPaging="true" AllowSorting="true">
                                    <PagerSettings Visible="False" />
                                    <RowStyle CssClass="GridItem" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemStyle Width="8%" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" CssClass="BlackWhite" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LabelName %>" SortExpression="SHORT_NAME">
                                            <ItemStyle Width="18%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnName" OnClick="lbtnName_Click" Text='<%# Eval("SHORT_NAME") %>'
                                                    runat="server">'<%# Eval("SHORT_NAME") %>'</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Type" SortExpression="Type" HeaderText="<%$ Resources:Type %>">
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPTION" SortExpression="DESCRIPTION" HeaderText="<%$ Resources:Description %>">
                                            <ItemStyle Width="30%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Assignment" SortExpression="Assignment" HeaderText="<%$ Resources:Assignment %>">
                                            <ItemStyle Width="26%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:Status %>">
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" Style="visibility: hidden" Text='<%# Eval("STIPULATION_ID") %>'
                                                    runat="server"></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="Hidden" align="left" colspan="3">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td align="right">
                                        <div id="pagerBottom" class="divPaginationContainer" runat="server">
                                            <table cellpadding="1" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <uc1:SearchPagination ID="searchPaginationBottom" runat="server" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnClose" runat="server" class="button" Text="<%$ Resources:Close %>"
                                OnClick="btnClose_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="center">
                            <asp:Button ID="btnDelete" runat="server" class="button" OnClick="btnDelete_Click"
                                OnClientClick="Javascript:return DeleteCheckLocal()" Text="<%$ Resources:DeleteSelected %>" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnNew" runat="server" class="button" Text="<%$ Resources:New %>"
                                OnClick="btnNew_Click" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnChangeSearch" runat="server" class="button" Text="<%$ Resources:ChangeSearch %>"
                                OnClick="btnChangeSearch_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <asp:HiddenField ID="hdfRowId" runat="server" />
        <asp:HiddenField ID="hdfDirty" runat="server" />

        <script type="text/javascript">
        var hdfDirtyCtrl = window.document.getElementById('<%= hdfDirty.ClientID %>');
        </script>

    </div>
</asp:Content>
