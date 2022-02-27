<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JavaScriptSerializer.aspx.cs" Inherits="JavaScriptSerializerDemo" Title="JavaScript Serializer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="JavaScriptSerializer Class"></asp:Label>
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
                            <li>Usage of 'JavaScriptSerializer' </li>
                            <li>The JavaScriptSerializer class is used internally by the asynchronous communication layer to serialize and deserialize the data that is passed between the browser and the Web server.</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <h3>
                        <span style="text-decoration: underline">Contacts Selection</span><br />
                    </h3>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            Type contact's first name:
                            <asp:TextBox ID="TextBox1" Text="Andrew" runat="server" />
                            <asp:Button ID="searchButton" runat="server" Text="Search" OnClick="searchButton_Click" />&nbsp;
                            [Search by - Nancy, Andrew, Janet, Margaret, Steven, Michael, Robert, Laura, Anne]
                            <br />
                            <br />
                            <table border="1" width="100%">
                                <tr>
                                    <td style="width: 50%" valign="top" align="center">
                                        <b>Search results:</b><br />
                                        <asp:GridView ID="ContactsGrid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="EmployeeID" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="ContactsGrid_SelectedIndexChanged"
                                            ForeColor="#333333" GridLines="None" AllowPaging="True" PageSize="7" OnPageIndexChanged="ContactsGrid_PageIndexChanged">
                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True" ButtonType="Button" />
                                                <asp:BoundField DataField="EmployeeID" HeaderText="EmployeeID" SortExpression="EmployeeID"
                                                    Visible="False" />
                                                <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" />
                                                <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" />
                                                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                                            </Columns>
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#999999" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <EmptyDataTemplate>
                                                No data found.</EmptyDataTemplate>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>"
                                            SelectCommand="SELECT EmployeeID, FirstName, LastName, Address FROM Employees WHERE (UPPER(FirstName) = UPPER(@FIRSTNAME))">
                                            <SelectParameters>
                                                <asp:ControlParameter Name="FIRSTNAME" ControlID="TextBox1" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td valign="top">
                                        <b>Contacts list:</b><br />
                                        <asp:ListBox ID="lstItems" runat="server" Height="200px" Width="214px" />
                                        <br />
                                        <asp:Button ID="saveButton" runat="server" Text="Save state" OnClick="saveButton_Click"
                                            ToolTip="Save the current state of the list" />
                                        <asp:Button ID="recoverButton" runat="server" Text="Recover saved state" OnClick="recoverButton_Click"
                                            Enabled="false" ToolTip="Recover the last saved state" />
                                        <asp:Button ID="clearButton" runat="server" Text="Clear" OnClick="clearButton_Click"
                                            ToolTip="Remove all items from the list" /><br />
                                        <br />
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked="True" OnCheckedChanged="CheckBox1_CheckedChanged"
                                            Text="Show saved state" AutoPostBack="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;
                                        <hr />
                                        Message:
                                        <asp:Label ID="Message" runat="server" ForeColor="SteelBlue" />&nbsp;&nbsp;<br /><br />
                                        <asp:Label ID="StateLabel"  ForeColor="Red" runat="server" Text="State:"></asp:Label>
                                        <asp:Label ID="SavedState" Font-Size="Large" runat="server" /><br />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ajax-loader.gif" />&nbsp;Processing...
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
