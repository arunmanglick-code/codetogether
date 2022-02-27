<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewAuditReport.aspx.cs" Inherits="AuditReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Trail Report</title>
    <link href="App_Themes/Default/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .Show { display:inline; }
    .Hide { display:none; }
    
    a img { border: 0px none; }
    </style>
    <script type="text/javascript">
    function ShowHideMaster()
    {
        //debugger;
        obj=document.getElementById('divMaster');
        if(obj.style.display == "")
        {
            obj.style.display='none';
        }
        else 
        {
            obj.style.display='';
        }       
    }
    
    function ShowHideDetail()
    {
        //debugger;
        obj=document.getElementById('divDetail');
        if(obj.style.display == "")
        {
            obj.style.display='none';
        }
        else 
        {
            obj.style.display='';
        }       
    }
    
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

</head>
<body>
    <form id="form1" runat="server">
    <table style="vertical-align: middle; height: 100%; width: 100%">
        <tr>
            <td class="title">
                <asp:Label ID="Label4" runat="server" Text="Audit Report"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <a href='#' onclick="ShowHideMaster();">ShowHide</a>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divMaster" style="float: right;">
                    <table style="vertical-align: middle; height: 100%; width: 100%">
                        <tr>
                            <td class="titleInner">
                                <asp:Label ID="Label5" runat="server" Text="Database Report"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">                                
                                <asp:GridView ID="GridViewMaster" runat="server" BorderWidth="1px" CellPadding="3"
                                    Width="600px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" EmptyDataText="Nothing has been audited yet">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <RowStyle ForeColor="#000066" Font-Names="Arial" Font-Size="Small" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#E3E3E3" ForeColor="#000066" />
                                </asp:GridView>                                
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>   
    </form>
</body>
</html>
