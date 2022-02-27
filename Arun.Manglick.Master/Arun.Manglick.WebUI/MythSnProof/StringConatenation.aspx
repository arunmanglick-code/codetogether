<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StringConatenation.aspx.cs" Inherits="TemplatePage" Title="String Concatenation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Myths n Proof - String Concatenation"></asp:Label></td>
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
                            <li>String Concatenation</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <asp:Button ID="btnStringConcat" runat="server" Text="String Concat" 
                        onclick="btnStringConcat_Click" />
                    <asp:Button ID="btnStringBuilder" runat="server" Text="String Builder" 
                        onclick="btnStringBuilder_Click" />
                    <asp:Button ID="Button1" runat="server" Text="String Join" 
                        onclick="btnStringJoin_Click" />
                    <div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
