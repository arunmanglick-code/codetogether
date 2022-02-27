<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NotePad.aspx.cs" Inherits="NotePad" Title="NotePad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="NotePad (Script Viewer)"></asp:Label></td>
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
                    <!-- Actual Feature Div -->
                    <div id="DivNotePad" runat="server">
                        <asp:TextBox ID="txtNotePad" EnableViewState="false" runat="server" CssClass="inputfield" BorderWidth="0" TextMode="MultiLine" Width="1050px" Height="600px"></asp:TextBox>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
