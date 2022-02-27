<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DrawPad.aspx.cs" Inherits="DrawPad" Title="DrawPad" %>

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
                    <div id="DivNotePad" class="DivDiagram" runat="server">
                        <asp:Image ID="imgDrawPad" border ="0" align="absmiddle"  runat="server" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
