<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ClientSideFeatures.aspx.cs" EnableEventValidation="false" Inherits="ClientSideFeatures" Title="Client Side" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script src="../../../JS/GridView.js" type="text/javascript"></script>
    
    <script type="text/javascript">
    	
	function SelectAll()
	{
	    //ctl00_cphBodyContent_GridView1_ctl03_chkRow
        //ctl00_cphBodyContent_GridView1_ctl02_chkHeader
		var name="ctl00_cphBodyContent_GridView1_ctl";
		var x=0;
		var i=3;
		var cons =0;
		do
		{
			if(i <= 9)
			{
			    oElement = document.getElementById(name + cons + i + "_chkRow");
			}
			else
			{
			    oElement = document.getElementById(name + i + "_chkRow");
			}
			
			if (oElement == null)
				{x=1;}
			else
			{oElement.checked=document.getElementById("ctl00_cphBodyContent_GridView1_ctl02_chkHeader").checked;}
			
			i++;
		}while(x<1);
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
                            <li>Change Row Color on Mouse Over</li>
                            <li>Select All Checkbox</li>
                            <li>Using <b>'ClientScript.GetPostBackClientHyperlink'</b> - To Bring in Edit Mode on Double Click</li>
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
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan"
                                BorderWidth="1px" CellPadding="2" DataKeyNames="Id" DataSourceID="srcMovies"
                                ForeColor="Black" GridLines="None" PageSize="10" PagerSettings-Mode="NextPreviousFirstLast"
                                PagerSettings-Position="TopAndBottom" AutoGenerateEditButton="false" OnRowCreated="GridView1_RowCreated">
                                <FooterStyle BackColor="Tan" />
                                <Columns>
                                    <asp:TemplateField HeaderText="CheckAll" SortExpression="Title">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" onclick="SelectAll();" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id">
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
                        <center>
                            ToolTip</center>
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
