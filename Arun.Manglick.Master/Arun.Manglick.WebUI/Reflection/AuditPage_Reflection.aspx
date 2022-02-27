<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AuditPage_Reflection.aspx.cs" Inherits="Reflection_ObjectToDataTable"
    Title="Reflection Audit Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script type="text/javascript">
        function ShowAuditTrail()
        {   
            window.open('../ViewAuditReport.aspx', '', 'fullscreen=no,menubar=no,scrollbars=yes,status=yes,titlebar=no,toolbar=no,resizable=yes,location=no');
        }        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Reflection - Audit Page"></asp:Label>               
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
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
                    <div class="DivClassFeature" style="width: 600px">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Extensive use of Reflection.</li>
                            <li>Generate DataTable from an Object Structure using Reflection - GetEmptyAuditActionTable().</li>
                            <li>Accessing the properties of an Object 'Employee' using Reflection - AuditPage().</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat" style="width: 200px">
                            <table class="table" cellpadding="2" cellspacing="1">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEmployeeId" runat="server" Text="EmployeeId"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="FrmInput" ID="txtEmployeeId" Enabled="false" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFirstName" runat="server" Text="FirstName"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="FrmInput" ID="txtFirstName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLastName" runat="server" Text="LastName"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="FrmInput" ID="txtLastName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                        <asp:GridView ID="grdAuditTrail" runat="server" Width="850px" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#CC9966" BorderStyle="None" AllowPaging="True"
                                            PageSize="5" BorderWidth="1px" AllowSorting="True" CellPadding="4" DataSourceID="odsDemoGrid">
                                            <SelectedRowStyle BackColor="#FFCC66" BorderColor="Blue" Font-Bold="true" ForeColor="#663399" />
                                            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099"></FooterStyle>
                                            <RowStyle ForeColor="#330099" CssClass="GridCellCentered" BackColor="White"></RowStyle>
                                            <PagerStyle HorizontalAlign="Center" BackColor="#FFFFCC" ForeColor="#330099"></PagerStyle>
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
                                            <HeaderStyle BackColor="#990000" CssClass="fixedheadertable" Font-Bold="True" ForeColor="#FFFFCC">
                                            </HeaderStyle>
                                            <Columns>
                                                <asp:BoundField DataField="CourseId" HeaderText="CourseId" ReadOnly="true">
                                                    <ItemStyle Width="25px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Sequence" ReadOnly="true" HeaderText="Sequence">
                                                    <ItemStyle Width="25px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="YearOfPassing" SortExpression="YearOfPassing" HeaderStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtYearOfPassing" runat="server" Text='<%# Eval("YearOfPassing") %>'
                                                            Width="75px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Institution" SortExpression="Institution" HeaderStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtInstitution" runat="server" Text='<%# Eval("Institution") %>'
                                                            Width="95px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="25px"></HeaderStyle>
                                                    <ItemStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Course" SortExpression="Course" HeaderStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCourse" runat="server" Text='<%# Eval("Course") %>' Width="75px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15px"></HeaderStyle>
                                                    <ItemStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Move Up/Down">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnMoveUp" runat="server" Width="25px"
                                                            ImageUrl="~/images/up.gif"></asp:ImageButton>
                                                        <asp:ImageButton ID="btnMoveDown" runat="server" Width="25px"
                                                            ImageUrl="~/images/down.gif"></asp:ImageButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                        <a onclick="ShowAuditTrail();" href="#">View Audit Trail</a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
