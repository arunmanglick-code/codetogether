<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SP Snippets GridView.aspx.cs" Inherits="SPSnippets" Title="SP Snippets Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script src="../JS/Browser.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Your Text"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
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
                    <div style="padding-left:50px;">
                        <br />
                        <br />
                        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CC9966"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="false"
                                AllowSorting="True" EnableViewState="true" Width="850px" OnRowCreated="GridView1_RowCreated">
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <RowStyle BackColor="White" ForeColor="#330099" HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                                <Columns>
                                    <asp:BoundField DataField="SPName" SortExpression="SPName" HeaderText="Name" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnEdit" runat="server" OnClick="SPShowHandler" Text="Show Notepad"  CausesValidation="false" />
                                        </ItemTemplate>                                        
                                    </asp:TemplateField>                                                     
                                    <asp:TemplateField HeaderText = "Feature List" Visible="true">
                                        <ItemTemplate>
                                            <asp:BulletedList ID="BulletedList1" runat="server">
                                            </asp:BulletedList>                                            
                                        </ItemTemplate>
                                        <ItemStyle Width="60%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPath" Style="visibility: hidden;" Text='<%# Eval("Path") %>' runat="server"></asp:Label>
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
            </td>
        </tr>
    </table>
</asp:Content>
