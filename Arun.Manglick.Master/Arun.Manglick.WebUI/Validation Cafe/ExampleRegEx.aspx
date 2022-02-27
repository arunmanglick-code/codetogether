<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExampleRegEx.aspx.cs" Inherits="ExampleRegEx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/MyTheme/Stylecal.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="height: 200px;">
            <br /><br />
            <table>
                <tr>
                    <td style="width: 10%">
                    </td>
                    <td>
                        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CC9966"
                            BorderStyle="None" BorderWidth="1px" Width="600px" CellPadding="4" AutoGenerateColumns="false"
                            AllowSorting="True">
                            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                            <RowStyle BackColor="White" CssClass="inputfield" ForeColor="#330099" />
                            <Columns>
                                <asp:BoundField DataField="Key" SortExpression="Key" HeaderText="CourseId" />
                                <asp:TemplateField HeaderText="Value">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBox4" Width="300px" CssClass="inputfield" runat="server" Text='<%# Eval("Value") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" ItemStyle-Width="300px" HeaderText="Description" />
                            </Columns>
                            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
