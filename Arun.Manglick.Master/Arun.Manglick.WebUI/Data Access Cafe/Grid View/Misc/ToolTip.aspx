<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ToolTip.aspx.cs" Inherits="ToolTip" Title="Tool Tip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
<script type="text/javascript">
	function ShowToolTip(a,b,c,d)
	{
		document.getElementById("td0").innerText=a; 
		document.getElementById("td1").innerText=b;
		document.getElementById("td2").innerText=c;
		document.getElementById("td3").innerText=d;
	
		x = event.clientX;
		y = event.clientY ;
		
		
		document.getElementById("PopupDIV").style.display='block';
		document.getElementById("PopupDIV").style.left = x + 5;
		document.getElementById("PopupDIV").style.top = y + 5;				
		
	}

	function HideTooltip()
	{
		//PopupDIV.style.visibility='hidden';  // This also works.
		document.getElementById("PopupDIV").style.display='none';
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Grid View - Tool Tip"></asp:Label>
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
                    <div class="DivClassFeature">
                        <b>Various Features Used.</b>
                        <ol>
                            <li>Tool Tip using JavaScript</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <div>
                        <div class="DivClassFloat">
                            <asp:TextBox ID="txtTitle" runat="server" />
                            <asp:Button ID="btnSubmit" Text="Button" runat="server" Enabled="false" OnClick="btnSubmit_Click" />
                        </div>
                        <div>
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" EnableViewState="true" AllowSorting="True"
                                AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan"
                                BorderWidth="1px" CellPadding="2" DataKeyNames="Id" DataSourceID="srcMovies"
                                ForeColor="Black" GridLines="None" PageSize="10" PagerSettings-Mode="NextPreviousFirstLast"
                                PagerSettings-Position="TopAndBottom" AutoGenerateEditButton="True" OnRowCreated="GridView1_RowCreated">
                                <FooterStyle BackColor="Tan" />
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True"
                                        SortExpression="Id">
                                        <HeaderStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                                    <asp:BoundField DataField="Director" HeaderText="Director" SortExpression="Director" />
                                    <asp:BoundField DataField="DateReleased" HeaderText="DateReleased" SortExpression="DateReleased" />
                                    <asp:TemplateField HeaderText="Template" SortExpression="Title">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="True" />
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                <PagerSettings Mode="NextPreviousFirstLast" Position="TopAndBottom" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="srcMovies" runat="server" ConnectionString="<%$ ConnectionStrings:MoviesConnectionString %>"
                                SelectCommand="SELECT [Id], [Title], [Director], [DateReleased] FROM [Movies]"
                                DeleteCommand="DELETE FROM [Movies] WHERE [Id] = @Id" InsertCommand="INSERT INTO [Movies] ([Title], [Director], [DateReleased]) VALUES (@Title, @Director, @DateReleased)"
                                UpdateCommand="UPDATE [Movies] SET [Title] = @Title, [Director] = @Director, [DateReleased] = @DateReleased WHERE [Id] = @Id">
                                <DeleteParameters>
                                    <asp:Parameter Name="Id" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="Title" Type="String" />
                                    <asp:Parameter Name="Director" Type="String" />
                                    <asp:Parameter Name="DateReleased" Type="DateTime" />
                                    <asp:Parameter Name="Id" Type="Int32" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="Title" Type="String" />
                                    <asp:Parameter Name="Director" Type="String" />
                                    <asp:Parameter Name="DateReleased" Type="DateTime" />
                                </InsertParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <!-- Making a Tool Tip -->
    <div id="PopupDIV" style="display: none; width: 100px; position: absolute">
        <table width="100" border="1" cellpadding="0" cellspacing="0">
            <tr>
                <td style="filter: alpha(opacity=60); color: white; background-color: red">
                    <b>
                        <center>ToolTip</center>
                    </b>
                </td>
            </tr>
            <tr>
                <td id="td0" align="left" bgcolor="#ffff99">
                </td>
            </tr>
            <tr>
                <td id="td1" align="left" bgcolor="#ffff99">
                </td>
            </tr>
            <tr>
                <td id="td2" align="left" bgcolor="#ffff99">
                </td>
            </tr>
            <tr>
                <td id="td3" align="left" bgcolor="#ffff99">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
