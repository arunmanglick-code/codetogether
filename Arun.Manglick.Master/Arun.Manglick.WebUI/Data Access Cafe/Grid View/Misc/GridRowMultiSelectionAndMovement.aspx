<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="GridRowMultiSelectionAndMovement.aspx.cs" Inherits="GridRowMultiSelectionAndMovement"
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

function SetSelectedObjRow(rowId,hiddenCtrlId,hiddenLastSelectedRow,hiddenCurrentGrid, grid,currentGrid)
    {
        //debugger;
        var lastKeyClick;
        var hiddenRowId;
        var hiddenLastRowId;
        var hiddenCurrentGrid;
        hiddenRowId = document.getElementById(hiddenCtrlId);
        hiddenLastRowId = document.getElementById(hiddenLastSelectedRow);
        var lastIndex = parseInt(hiddenLastRowId.value);
        var index = parseInt(rowId);
        var temp;
        var iCnt;
        
        //RemoveRowColor(grid,hiddenCurrentGrid,currentGrid,hiddenCtrlId);
        
        //"0" is for SHIFT key
        if(keyPres == "0")
        {
            if(lastIndex < index)
            {
                temp = index;
                index = lastIndex;
                lastIndex = temp;
            }
            
            hiddenRowId.value = '';
                        
            for(iCnt=index;iCnt<=lastIndex;iCnt++)
            {
                if(hiddenRowId.value != '')
                    hiddenRowId.value = hiddenRowId.value + ',' + iCnt;
                else
                    hiddenRowId.value = iCnt;
            }
        }
        else if(keyPres == "1") //"1" is for CTRL key
        {
            var allRows = hiddenRowId.value.split(",");
            hiddenRowId.value = '';
            var flg = false;
            for(iCnt=0;iCnt<allRows.length;iCnt++)
            {
                indx = allRows[iCnt];
                if(indx != "")
                {
                    if(indx != index)
                    {
                        if(hiddenRowId.value != '')
                            hiddenRowId.value = hiddenRowId.value + ',' + indx;
                        else
                            hiddenRowId.value = indx;
                    }
                    else
                    {
                        flg = true;
                    }
                }
            }
            
            if(!flg)
            {
                if(hiddenRowId.value != '')
                    hiddenRowId.value = hiddenRowId.value + ',' + index;
                else
                    hiddenRowId.value = index;
            }
        } // Simply another Selection  
        else
        {
            hiddenRowId.value = index;
        }
         
        hiddenLastRowId.value = rowId;
        SetRowColor(currentGrid,hiddenRowId);
        keyPres = "";
    }
    
    //Method to Set selected row color.
    function SetRowColor(currentGrid,hiddenRowId)
    {
        var allRows = hiddenRowId.value.split(",");
        var indx;
        var currGridView = document.getElementById(currentGrid);
        
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
                    currGridView.rows[indx].style.backgroundColor='#CAD2D4';
            }
        }        
    }
    
    //Method to remove grid color.
    //This is grid that is currently not selected.
    function RemoveRowColor(grid,hiddenCurrentGrid,currentGrid,hiddenCtrlId)
    {
        var hiddenRowId;
        var hiddenCurrentGrid;
        hiddenCurrentGrid = document.getElementById(hiddenCurrentGrid);
        hiddenRowId = document.getElementById(hiddenCtrlId);
        
        if(hiddenCurrentGrid.value != '')
        {
            if(hiddenCurrentGrid.value != currentGrid)
            {
                hiddenRowId.value = '';
                hiddenCurrentGrid.value = currentGrid;
            }
        }
        else
        {
            hiddenCurrentGrid.value = currentGrid;
        }
        
        hiddenCurrentGrid = document.getElementById(grid);
        if(hiddenCurrentGrid != null)
        {
            for(iCnt=0;iCnt<hiddenCurrentGrid.rows.length;iCnt++)
            {
                hiddenCurrentGrid.rows[iCnt].style.backgroundColor='';
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
                            <li>Single/Multi-Select Feature using Ctrl/Shift Keys</li>
                            <li>Notice the JS Fucntion - SetSelection()</li>
                            <li>This function is avoid any selection by Dragging the mouse</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <div>
                        <div style="height: 200px;">
                            <table id="divtable" border="0" cellpadding="0" width="100%" cellspacing="3">
                                <tr align="left">
                                    <td class="requiredcolumn" style="width: 5%">
                                    </td>
                                    <td align="left" style="width: 30%" valign="top">
                                        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CC9966"
                                            BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="false"
                                            AllowSorting="True" OnRowCreated="GridView1_RowCreated">
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
                                    <td style="width: 30%" class="gridcell">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr align="center">
                                                <td align="center" style="height: 84px;">
                                                    <asp:Button ID="btnMove" runat="server" class="button" Text="Move >" Width="100px"
                                                        OnClick="btnMove_Click" /><br />
                                                    <br />
                                                    <asp:Button ID="btnMoveAll" runat="server" class="button" Text="Move All >>" Width="100px"
                                                        OnClick="btnMoveAll_Click" />
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td align="center" style="height: 84px;">
                                                    <asp:Button ID="btnRemove" runat="server" class="button" Text="< Remove" Width="100px"
                                                        OnClick="btnRemove_Click" /><br />
                                                    <br />
                                                    <asp:Button ID="btnRemoveAll" runat="server" class="button" Text="<< Remove All"
                                                        Width="100px" OnClick="btnRemoveAll_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="left" style="width: 30%" valign="top">
                                        <asp:GridView ID="GridView2" runat="server" BackColor="White" BorderColor="#CC9966"
                                            BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="false"
                                            AllowSorting="True" OnRowCreated="GridView2_RowCreated">
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
