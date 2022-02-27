<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Title="Generic Audit Trail"
    EnableEventValidation="false" AutoEventWireup="true" CodeFile="GenericAuditTrail.aspx.cs"
    Inherits="GenericAuditTrail" Culture="auto" UICulture="auto" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="contentHeader" ContentPlaceHolderID="cphHeaderContent" runat="Server">
    <style type="text/css">
        .GridCellCentered
        {
            text-align: center;
        }
        .FrmInput
        {
            font-family: Arial, Verdana, Sans-Serif;
            font-size: 9pt;
            font-weight: normal;
            background-color: #F7F7F7;
            border: solid 1px #9A9EA4;
        }
        .fixedheadertable
        {
            position: relative;
        }
    </style>

    <script language="javascript" type="text/javascript">
        var oldgridSelectedColor;
        var oldElement;
        var gridViewCtlId = '<%=grdAuditTrail.ClientID %>';
                
        function setMouseOverColor(element) 
        {
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.backgroundColor='lightcoral';
            element.style.cursor='hand';
            if(oldElement != null)
            {
                setMouseOutColor(oldElement)
            }
            oldElement = element;
        }

        function setMouseOutColor(element) 
        {
            element.style.backgroundColor=oldgridSelectedColor;
            element.style.textDecoration='none';
        }
        
        function SetSelectedRow(rowId)
        {
            //debugger;
            var hiddenRowId = document.getElementById("<%=hiddenSelectRowId.ClientID %>");
            hiddenRowId.value = rowId - 1;
        }
        
        function ShowAuditTrail()
        {   
            window.open('HierarrchichalAuditReport.aspx', '', 'fullscreen=no,menubar=no,scrollbars=yes,status=yes,titlebar=no,toolbar=no,resizable=yes,location=no');
        }
        
        function OnFocusTextbox()
        { 
        }

        function OnFocusOutTextbox()
        {
        }
        
    </script>

</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Generic Audit Trail"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblNameError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="validation-no-error">
                <asp:ValidationSummary ID="vlsStipulations" DisplayMode="List" CssClass="validation"
                    runat="server"></asp:ValidationSummary>
            </td>
        </tr>
    </table>
    <!-- Table 2 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td>
                <div id="divform">
                    <br />
                    <br />
                    <!-- Features Div -->
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Enter Text</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div>
                            <table class="formtable" border="0" style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="formheader" style="width: 100%" colspan="3">
                                        Object Details
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%"colspan="3">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEmployeeId" CssClass="labelcolumn" runat="server" Text="EmployeeId"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox onfocus="Javascript:OnFocusTextbox();" onfocusout="Javascript:OnFocusOutTextbox()"
                                            CssClass="inputfield" ID="txtEmployeeId" Enabled="false" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 60%">                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFirstName" CssClass="labelcolumn" runat="server" Text="FirstName"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox onfocus="Javascript:OnFocusTextbox();" onfocusout="Javascript:OnFocusOutTextbox()"
                                            CssClass="inputfield" ID="txtFirstName" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 60%">                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLastName" CssClass="labelcolumn" runat="server" Text="LastName"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox onfocus="Javascript:OnFocusTextbox();" onfocusout="Javascript:OnFocusOutTextbox()"
                                            CssClass="inputfield" ID="txtLastName" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 60%">                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAge" runat="server" CssClass="labelcolumn" Text="Age"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox onfocus="Javascript:OnFocusTextbox();" onfocusout="Javascript:OnFocusOutTextbox()"
                                            CssClass="inputfield" ID="txtAge" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 60%">                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <br />
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <div style="height: 250px; overflow: auto;">
                                                        <asp:GridView ID="grdAuditTrail" runat="server" Width="850px" AutoGenerateColumns="False"
                                                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" AllowPaging="True"
                                                            PageSize="10" BorderWidth="1px" AllowSorting="True" CellPadding="3" DataSourceID="odsDemoGrid"
                                                            OnSorting="grdAuditTrail_Sorting" OnPageIndexChanged="grdAuditTrail_PageIndexChanged"
                                                            OnRowDataBound="grdAuditTrail_RowDataBound" OnDataBound="grdAuditTrail_DataBound">
                                                            <SelectedRowStyle BackColor="lightcoral" BorderColor="Blue" Font-Bold="true" />
                                                            <FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>
                                                            <RowStyle ForeColor="#000066" CssClass="GridCellCentered"></RowStyle>
                                                            <AlternatingRowStyle BackColor="#E3E3E3" ForeColor="#000066" />
                                                            <PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>
                                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
                                                            <HeaderStyle BackColor="#006699" CssClass="fixedheadertable" Font-Bold="True" ForeColor="White">
                                                            </HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField DataField="CourseId" HeaderText="From Age" ReadOnly="true">
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Sequence" ReadOnly="true" HeaderText="To Age">
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="YearOfPassing" SortExpression="YearOfPassing">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox onfocus="Javascript:OnFocusTextbox();" onfocusout="Javascript:OnFocusOutTextbox()"
                                                                            ID="txtYearOfPassing" runat="server" CssClass="inputfield" Text='<%# Bind("YearOfPassing") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Institution" SortExpression="Institution">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox onfocus="Javascript:OnFocusTextbox();" CssClass="inputfield" onfocusout="Javascript:OnFocusOutTextbox()"
                                                                            ID="txtInstitution" runat="server" Text='<%# Bind("Institution") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Course" SortExpression="Course">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox onfocus="Javascript:OnFocusTextbox();" CssClass="inputfield" onfocusout="Javascript:OnFocusOutTextbox()"
                                                                            ID="txtCourse" runat="server" Text='<%# Bind("Course") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Average" SortExpression="Course">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox onfocus="Javascript:OnFocusTextbox();" CssClass="inputfield" onfocusout="Javascript:OnFocusOutTextbox()"
                                                                            ID="txtAverage" runat="server" Text='<%# Bind("Average") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Move Up/Down">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnMoveUp" OnClick="btnMoveUp_Click" runat="server" Width="25px"
                                                                            ImageUrl="~/images/up.gif"></asp:ImageButton>
                                                                        <asp:ImageButton ID="btnMoveDown" OnClick="btnMoveDown_Click" runat="server" Width="25px"
                                                                            ImageUrl="~/images/down.gif"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="60px"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="Button1" OnClick="btnAddRow_Click" runat="server" CssClass="button"
                                                        Text="New Row"></asp:Button>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="Button2" OnClick="btnInsertRow_Click" runat="server" CssClass="button"
                                                        Text="Insert Row"></asp:Button>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="Button3" OnClick="btnCopyRow_Click" runat="server" CssClass="button"
                                                        Text="Copy Row"></asp:Button>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="Button4" OnClick="btnPasteRow_Click" runat="server" CssClass="button"
                                                        Text="Paste Row"></asp:Button>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="Button5" OnClick="btnDeleteRow_Click" runat="server" CssClass="button"
                                                        Text="Delete Row"></asp:Button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <br />
                                    <asp:Button ID="Button6" OnClick="btnSave_Click" runat="server" CssClass="button"
                                        Text="Save"></asp:Button>
                                    <br />
                                    <br />
                                    <asp:HiddenField ID="hiddenSelectRowId" runat="server" />
                                    <asp:HiddenField ID="hiddenDirty" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:ObjectDataSource ID="odsDemoGrid" runat="server" SelectMethod="GridAuditReflectionTable"
                            UpdateMethod="UpdateDataTable" TypeName="Arun.Manglick.UI.GridAccessLayer" DeleteMethod="DeleteMethod">
                            <DeleteParameters>
                                <asp:Parameter Name="empId" Type="Int32" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Sequence" Type="Int32" />
                                <asp:Parameter Name="YearOfPassing" Type="DateTime" />
                                <asp:Parameter Name="Institution" Type="String" />
                                <asp:Parameter Name="Course" Type="String" />
                            </UpdateParameters>
                        </asp:ObjectDataSource>
                        <div>
                            <br />
                            <a onclick="ShowAuditTrail();" href="#">View Audit Trail</a>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
