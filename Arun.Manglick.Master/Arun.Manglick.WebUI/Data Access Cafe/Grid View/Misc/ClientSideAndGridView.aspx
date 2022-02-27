<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ClientSideAndGridView.aspx.cs" Inherits="ClientSideAndGridView"
    Title="Row Multi 'Selection n Movement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script src="../../../JS/GridView.js" type="text/javascript"></script>

    <script type="text/javascript">
    
    var keyPres;
    document.onkeydown = function(myEvent)
    {
        if(!myEvent)
        {
            myEvent = window.event;
        }
        if(myEvent.shiftKey)
        {
            keyPres = "0";
        }
        else if(myEvent.ctrlKey)
        {
            keyPres = "1";
        }
        else
        {
            keyPres = "";
        }
   }
   
   document.onmousedown = function(myEvent)
    {
        if(!myEvent)
        {
            myEvent = window.event;
        }
        if(myEvent.shiftKey)
        {
            keyPres = "0";
        }
        else if(myEvent.ctrlKey)
        {
            keyPres = "1";
        }
        else
        {
            keyPres = "";
        }
   }
// ---------------------------------------------------------------------------------------------------------------

    function StoreSelectedObjRow(index)
    {
        var hiddenRowId = document.getElementById('<%= hdfRowId.ClientID %>');
        if(hiddenRowId.value != '')
        {
          hiddenRowId.value = hiddenRowId.value + ',' + index;
        }
        else
        {
          hiddenRowId.value = index;
        }
    }
    
    function SetRowColor()
    {
        //debugger;
        var currGridView = document.getElementById('<%= GridView1.ClientID %>');
        var hiddenRowId = document.getElementById('<%= hdfRowId.ClientID %>');
        var allRows = hiddenRowId.value.split(",");
        var indx;
        
        if(currGridView != null)
        {
            for(iCnt=0;iCnt<currGridView.rows.length;iCnt++)
            {
                currGridView.rows[iCnt].style.backgroundColor='';
                
            }
            
            for(iCnt=0;iCnt<allRows.length;iCnt++)
            {
                indx = allRows[iCnt];
                if(indx != "")
                {
                  currGridView.rows[indx].style.backgroundColor='#BAF2D4';
                  currGridView.rows[indx].cells[0].style.backgroundColor= 'Red';
                  currGridView.rows[indx].cells[2].innerText = 'Change Text';
                  
               } 
            }
        }

        Find();      
    }


    function Find() 
    {
        var value;       
        var currGridView = document.getElementById('<%= GridView1.ClientID %>');
        for (iCnt = 0; iCnt < currGridView.rows.length; iCnt++) 
        {
            value = currGridView.rows[iCnt].cells[4].innerText;
            if (value == 69) 
            {
                alert('found');
                return;
            }
        }
    }
    
    function MakeGridEmpty()
    {
        var currGridView = document.getElementById('<%= GridView1.ClientID %>');
        if(currGridView != null &&  currGridView.rows.length > 1)
        {           
            for(iCnt=currGridView.rows.length - 1; iCnt >= 0; iCnt--)
            {
                currGridView.deleteRow();
            }
        }
    }
    </script>

    <script type="text/javascript">
    
    window.onload = function(){SetSelection();}
    
    function SetSelection()
    {
        var element = window.document.getElementById('divtable');
        if(element != null)
        {
            element.onselectstart = function()
            {  
                return false;
            } 
        }
    }
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">

    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Grid View - Row Multi 'Selection n Movement'"></asp:Label>
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
                    <div class="DivClassFeature" style="width: 700px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Accessing the GridView.Rows property in JavaScript</li>
                            <li>Accessing the GridView.Rows[index].Cells property in JavaScript</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <div>
                        <div style="height: 200px;">
                            <table id="divtable" border="0" cellpadding="0" width="100%" cellspacing="3">
                                <tr align="left">
                                    <td class="requiredcolumn" style="width: 5%">
                                        <table>
                                            <tr>
                                                <td class="requiredcolumn" style="width: 10%">
                                                    <asp:Button ID="btnSetColor" CssClass="button" OnClientClick="SetRowColor(); return false;" runat="server" Text="Set Row Color" />
                                                    <asp:Button ID="Button1" CssClass="button" OnClientClick="MakeGridEmpty(); return false;" runat="server" Text="Make Grid Empty" />
                                                    <asp:Button ID="btnPostBack" CssClass="button" runat="server" Text="Simple PostBack" OnClick="btnPostBack_Click" />
                                                    
                                                </td>   
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="left" style="width: 30%" valign="top">
                                        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CC9966"
                                            BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="false"
                                            AllowSorting="True" OnRowCreated="GridView1_RowCreated" 
                                            onrowdatabound="GridView1_RowDataBound">
                                            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                            <RowStyle BackColor="White" ForeColor="#330099" />
                                            <Columns>
                                                <asp:BoundField DataField="CourseId" SortExpression="CourseId" HeaderText="CourseId" />
                                                <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                                                <asp:BoundField DataField="Institution" HeaderText="Institution" ItemStyle-Width="150" />
                                                <asp:BoundField DataField="Course" HeaderText="Course" />
                                                <asp:TemplateField HeaderText="Average on Label">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Average") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                            <AlternatingRowStyle BackColor="AliceBlue" ForeColor="RosyBrown" />
                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                        </asp:GridView>
                                    </td>                                    
                                    <td class="requiredcolumn" style="width: 5%">
                                    </td>
                                 
                            </table>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdfRowId" Value="" runat="server" />
    <asp:HiddenField ID="hdfLastSelectedRow" runat="server" />
    <asp:HiddenField ID="hdfCurrentGrid" Value="" runat="server" />
</asp:Content>
