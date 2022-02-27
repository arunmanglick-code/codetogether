<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DynamicBoundNTemplateColumnsAndAuditing.aspx.cs" Inherits="DynamicBoundNTemplateColumnsAndAuditing"
    Title="Bound & Template Columns" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script language="javascript" type="text/javascript">
        var oldgridSelectedColor;
        var oldElement;
        var gridViewCtlId = '<%=GridView1.ClientID %>';
                
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
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Adding Bound & Template Columns"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="1" cellspacing="0">
                <!-- Row 1 -->
                <tr>
                    <td>
                        <div id="divform">
                            <br />
                            <br />
                            <!-- Features Div -->
                            <div class="DivClassFeature" style="width: 800px;">
                                <b>Varoius Features Used.</b>
                                <ol>
                                    <li>Auditing when Dynamically adding Columns - Very Critical</li>
                                    <li>Use Only 'Add Template Columns' Button.</li>
                                    <li>Rest - Add Bound Column & Add Bound & Template Column has kept disabled</li>
                            </div>
                            <br />
                            <br />
                            <!-- Actual Feature Div -->
                            <div>
                                <div class="DivClassFloat">
                                    <asp:Button ID="Button2" CssClass="button" Text="Simple PostBack" runat="server"
                                        Width="194px" OnClick="btnSimple_Click" /><br />
                                    <asp:Button ID="btnSubmit" CssClass="button" Text="ReBind PostBack" runat="server"
                                        Width="194px" OnClick="btnSubmit_Click" /><br />
                                    <asp:Button ID="btnAddBoundColumn" CssClass="button" Text="Add Bound Column" runat="server"
                                        OnClick="btnAddBoundColumn_Click" Width="194px" /><br />
                                    <asp:Button ID="btnAddTemplateColumn" CssClass="button" Text="Add Template Columns"
                                        runat="server" OnClick="btnAddTemplateColumn_Click" Width="194px" /><br />
                                    <asp:Button ID="btnAddBoundTemplateColumn" CssClass="button" Text="Add Bound & Template Column"
                                        runat="server" OnClick="btnAddBoundTemplateColumn_Click" Width="194px" /><br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnNewRow" CssClass="button" Text="New Row" runat="server" Width="194px"
                                        OnClick="btnNewRow_Click" /><br />
                                    <asp:Button ID="btnInsertRow" CssClass="button" Text="Insert Row" runat="server"
                                        Width="194px" OnClick="btnInsertRow_Click" /><br />
                                    <asp:Button ID="btnCopyRow" CssClass="button" Text="Copy Row" runat="server" Width="194px"
                                        OnClick="btnCopyRow_Click" /><br />
                                    <asp:Button ID="btnPasteRow" CssClass="button" Text="Paste Row" runat="server" Width="194px"
                                        OnClick="btnPasteRow_Click" /><br />
                                    <asp:Button ID="btnDeleteRow" CssClass="button" Text="Delete Row" runat="server"
                                        Width="194px" OnClick="btnDeleteRow_Click" /><br />
                                </div>
                                <div style="height: 200px;">
                                    <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CC9966"
                                        BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="false"
                                        AllowSorting="True" EnableViewState="False" OnRowCreated="GridView1_RowCreated"
                                        OnRowDataBound="GridView1_RowDataBound">
                                        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                        <RowStyle BackColor="White" ForeColor="#330099" />
                                        <Columns>
                                            <asp:BoundField DataField="CourseId" SortExpression="CourseId" HeaderText="CourseId" />
                                            <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                            <asp:BoundField DataField="Institution" HeaderText="Institution" ItemStyle-Width="150">
                                                <ItemStyle Width="150px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Course" HeaderText="Course" />
                                            <asp:TemplateField HeaderText="Average">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox1" CssClass="inputfield" runat="server" Text='<%# Eval("Average") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade">
                                                <ItemTemplate>
                                                    <asp:DropDownList CssClass="inputfield" ID="DropDownList1" EnableViewState="true"
                                                        Width="75px" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                        <AlternatingRowStyle BackColor="AliceBlue" ForeColor="RosyBrown" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                    </asp:GridView>
                                    <asp:HiddenField ID="hiddenSelectRowId" runat="server" />
                                    <asp:HiddenField ID="hiddenDirty" runat="server" />
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
