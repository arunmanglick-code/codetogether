<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DynamicBoundNTemplateColumns.aspx.cs" Inherits="DynamicBoundNTemplateColumns" Title="Bound & Template Columns" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
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
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td>
                <div id="divform">
                    <br />
                    <br />
                    <!-- Features Div -->
                    <div class="DivClassFeature" style="width:800px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Dynamically adding Columns - Very Critical</li>
                            <li>Required 'EnableViewState="False"' for GridView</li>
                            <li>
                                Reason -  You need to re-create any dynamically generated controls after a postback because unlike static controls
                                they are not instantiated automatically. In such case if the ViewState is enabled, then it adds one more dynamic column. 
                            </li>
                            <li>
                                Actually The ealiar added columns will still be there, but there will be no content within the cells. 
                                i.e It adds one more dynamic column and simulataneously destroys the contents of the earliar dynamically generated columns.                                 
                            </li>
                             <li>
                                Drawback - Disabling ViewState of GridView results in loosing the changed state of the Template Columns addded at 
                                Design and Runtime both. Othersise if the ViewState is enabled the changed state for atleast the Design time
                                Template Column remains.                                
                                Chagned State refers - Changing the text in TextBox or selected other option in Dropdown.
                            </li>
                             <li>
                                Important - You need to re-create any dynamically generated controls after a postback because unlike static controls
                                they are not instantiated automatically. So, whatever code you have that generates dynamic columns, be sure to run it on 'Page_Load' so it runs after a postback as well as on the inital request.
                                You should write the code to re-create at Page_Load. Otherwise the dynamically generated columns will be destroyed.
                            </li>
                        </ol>
                        <br />
                        <ol>
                            <li>Summary</li>
                            <li>On Simple PostBack also, BindGrid is required on PageLoad, to maintain the changed state, when 'EnableViewState="False"'</li>
                            <li>When BindGrid is required, then it must be on the PageLoad (rather than on Button Click event handler), otherwise the changed state would be loosed irrespective of 'EnableViewState= True / False' </li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat">
                            <asp:Button ID="Button2" CssClass="button" Text="Simple PostBack" runat="server" 
                                Width="194px" onclick="btnSimple_Click" /><br />
                            <asp:Button ID="btnSubmit" CssClass="button" Text="ReBind PostBack" runat="server" 
                                Width="194px" onclick="btnSubmit_Click" /><br />
                            <asp:Button ID="btnAddBoundColumn" CssClass="button" Text="Add Bound Column" 
                                runat="server" onclick="btnAddBoundColumn_Click" Width="194px" /><br />
                            <asp:Button ID="btnAddTemplateColumn" CssClass="button" 
                                Text="Add Template Column - TextBox" runat="server" onclick="btnAddTemplateColumn_Click" 
                                Width="194px" /><br />
                            <asp:Button ID="Button1" CssClass="button" 
                                Text="Add Template Column - DropDown" runat="server" onclick="btnAddTemplateColumnDropDown_Click" 
                                Width="194px" />                           
                        </div>
                        <div style="height: 200px;">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CC9966"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="false"
                                AllowSorting="True" EnableViewState="False" OnRowCreated="GridView1_RowCreated">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <Columns>
                                    <asp:BoundField DataField="CourseId" SortExpression="CourseId" HeaderText="CourseId" />
                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                    <asp:BoundField DataField="Institution" HeaderText="Institution" 
                                        ItemStyle-Width="150" >
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
                                            <asp:DropDownList CssClass="inputfield" ID="DropDownList1" EnableViewState="true" Width="75px" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <AlternatingRowStyle BackColor="AliceBlue" ForeColor="RosyBrown" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
