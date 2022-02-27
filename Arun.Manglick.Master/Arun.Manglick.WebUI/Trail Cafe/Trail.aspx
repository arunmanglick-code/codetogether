<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Trail.aspx.cs" Inherits="Trail_Cafe_Trail" Title="Trail Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script type="text/javascript" language="javascript">
        //window.open('http://www.yahoo.com');
        //Show();
        var lblName='ctl00_cphBodyContent_lblHeader';
        var divElem = 'AlertDiv';
        //Funtion to check if there is assignment before deleting.
        function CheckControl()
        {
            //var lbl = window.document.getElementById('<%=lblHeader.ClientID %>');
            var lbl = window.document.getElementById('ctl00_cphBodyContent_lblHeader');
            //var lbl = $get('ctl00_cphBodyContent_lblHeader');
//            var adiv = $get(lbl);
            if(lbl !=null)
            {
                alert('found');
            }
            else
            {
                 alert('Not found');
            }
        }
        
        function Show()
        {
            
            window.open('http://www.kumarworld.com/Photos/Layout/image_103.gif',"newwindow","fullscreen=no,menubar=no,scrollbars=yes,status=no,titlebar=no,toolbar=no,resizable=yes,location=no,top=0,left=0");
            //onClick="window.open('layout-plan.asp?id=103','newwin','scrollbars=yes,resizable=yes,status=no,width=780,height=310,ScreenX=0,ScreenY=0,top=0,left=0')"
        }
        
        function Close()
        {
            window.open('','_parent','');

            window.close();
        }
        
       
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Trial Page"></asp:Label>
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
                    <asp:Label ID="lblError" runat="server" Text="" ></asp:Label>
                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="5" TextMode="MultiLine"></asp:TextBox>
                    <asp:Button ID="Button2" runat="server"  Text="Button" 
                        onclick="Button2_Click"  /> 
                    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />                   
                </div>
                <div id="AlertDiv">
                <a href='#' onclick="Show();">Click me</a><br />
                <a href='#' onclick="Close();">Close</a>
                
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
