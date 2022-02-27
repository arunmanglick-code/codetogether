<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StaticBoundNTemplateColumns.aspx.cs" Inherits="StaticBoundNTemplateColumns" Title="Bound & Template Columns" %>

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
                            <li>Static Template Columns</li>
                            <li>
                                The Template Columns maintains thier 'Changed State'.
                                Here Changed State refers - Changing the text in TextBox or selected other option in Dropdown.                                
                            </li>
                            <li>This changed state is maintained only on Simple Postback and not On PostBack where we re-bind the GridView</li>
                            <li>
                                To maintain the changed state on ReBind PostBack - Required is to fire the BindGrid on every PageLoad.
                                Firing the BindGrid on Button Click event handler will not server the purpose.
                            </li>
                            <li>
                                Here in our example below - When you change some text or dropdown selection and click the 'ReBind PostBack' button, you'll find that the changed state is not preserved.
                                Reason being - We have rebind the gridview on Button_Click event handler rather than every Page_Load.
                            </li>
                            <li>To maintain this changed state required is 'EnableViewState="True"' for GridView</li>
                        </ol>
                        <br />
                        <ol>
                            <li>Note</li>
                            <li>The 'Average' dropdown values are added at Design time</li>
                            <li>The 'Grade' dropdown values are added at Run time in 'RowCreated' event</li>
                        </ol>
                        <br />
                        <ol>
                            <li>Summary</li>
                            <li>On Simple PostBack, changed state is automatically maintained when 'EnableViewState="True"'</li>
                            <li>When BindGrid is required, then it must be on the PageLoad (rather than on Button Click event handler), otherwise the changed state would be loosed irrespective of 'EnableViewState= True / False' </li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat">
                            <asp:Button ID="btnSubmit" CssClass="button" Text="Simple PostBack" runat="server" 
                                onclick="btnSimple_Click" Width="165px" /><br /><br />
                            <asp:Button ID="Button1" CssClass="button" Text="ReBind PostBack" runat="server" 
                                onclick="btnSubmit_Click" Width="165px" />
                        </div>
                        <div style="height: 200px;">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CC9966"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="false"
                                AllowSorting="True" EnableViewState="true" OnRowCreated="GridView1_RowCreated">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <Columns>
                                    <asp:BoundField DataField="CourseId" SortExpression="CourseId" HeaderText="CourseId" />
                                    <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                    <asp:BoundField DataField="Institution" HeaderText="Institution" ItemStyle-Width="150" />
                                    <asp:BoundField DataField="Course" HeaderText="Course" />
                                    <asp:TemplateField HeaderText="Average">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBox1" CssClass="inputfield" runat="server" Text='<%# Eval("Average") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Grade">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="inputfield" ID="DropDownList1" Width="75px" runat="server">
                                                <asp:ListItem>AA</asp:ListItem>
                                                <asp:ListItem>BB</asp:ListItem>
                                                <asp:ListItem>CC</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="inputfield" ID="DropDownList2" Width="75px" runat="server">                                               
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
