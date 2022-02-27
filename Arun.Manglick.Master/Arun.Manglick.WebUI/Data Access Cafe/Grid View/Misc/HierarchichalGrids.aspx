<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HierarchichalGrids.aspx.cs" Inherits="TemplatePage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script type="text/javascript">
    
    function ShowGrid(gridId,plusId,minusId)
    {
        //debugger;
        grid=document.getElementById(gridId);
        plus=document.getElementById(plusId);
        minus=document.getElementById(minusId);
        grid.className = "Show";        
        plus.className = "Hide"; 
        minus.className = "Show"; 
    }
    
    function HideGrid(gridId,plusId,minusId)
    {
        //debugger;
        grid=document.getElementById(gridId);
        plus=document.getElementById(plusId);
        minus=document.getElementById(minusId);
        grid.className = "Hide";        
        plus.className = "Show"; 
        minus.className = "Hide"; 
    }  
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Hierarchichal Grids"></asp:Label>
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
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Hierarchichal Grids using the '+' sign</li>
                            <li>Click on '+; sign to Show the Inner Grid</li>
                            <li>Click on '-; sign to Hide the Inner Grid</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC"
                            BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Size="Medium" Width="600px"
                            EmptyDataText="No Changes" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound">
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <RowStyle ForeColor="#000066" Font-Names="Arial" Font-Size="Small" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="#E3E3E3" ForeColor="#000066" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a id="lnk" runat="server" href="">
                                            <img src="~/Images/plus.JPG" id="imgPlus" alt="Expand" runat="server" />
                                            <img src="~/Images/Minus.JPG" id="imgMinus" class="Hide" alt="Colaps" runat="server" />
                                        </a>
                                        <%# Eval("TableName")%>
                                        <asp:GridView ID="GridView2" CssClass="Hide" runat="server" EmptyDataText="No Rows Present">
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <RowStyle ForeColor="#000066" Font-Names="Arial" Font-Size="Small" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="#E3E3E3" ForeColor="#000066" />
                                        </asp:GridView>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
