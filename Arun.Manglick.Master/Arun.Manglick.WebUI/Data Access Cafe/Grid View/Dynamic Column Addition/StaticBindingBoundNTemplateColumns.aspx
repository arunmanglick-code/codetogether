<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StaticBindingBoundNTemplateColumns.aspx.cs" Inherits="StaticBindingBoundNTemplateColumns" Title="Bound & Template Columns" %>

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
                            <li>Requirement - To Show how the Grid having DropDown list is populated</li>                            
                            <li>Two things have been covered</li>
                            <li>Filling Master Value List in Drop Downs - 'GridView1_RowCreated'</li>
                            <li>Populating actual value of the Data Column - 'GridView1_DataBound'</li>
                        </ol> 
                        <br />
                        
                        <ol>
                        <li>Button Details can be read from link - <a href="StaticBoundNTemplateColumns.aspx">Link</a> </li>
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
                                AllowSorting="True" EnableViewState="true"  
                                OnRowCreated="GridView1_RowCreated" ondatabound="GridView1_DataBound">
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
                                            <asp:DropDownList CssClass="inputfield" ID="ddlGrade" Width="75px" runat="server">                                              
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="inputfield" ID="ddlStatus" Width="75px" runat="server">                                               
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
