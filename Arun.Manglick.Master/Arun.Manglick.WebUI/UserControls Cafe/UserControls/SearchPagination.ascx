<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchPagination.ascx.cs" Inherits="UserControls_SearchPagination" %>
<link href="../App_Themes/Default/style.css" rel="stylesheet" type="text/css" />
<table cellpadding="1" cellspacing="0" border="0">
    <tr>
        <td class="results-returned">
            <asp:Label ID="lblResults" runat="server" Text=""></asp:Label>&nbsp;
        </td>
        <td id="tdLinkFirst" class="results-pagination" runat="server">
           <asp:LinkButton ID="lbtnFirst" ToolTip="First Page" CommandName="Page" CommandArgument="1"
                Text="<%$ Resources:FirstButton %>" runat="server" OnClick="FirstClick" />
        </td>
        <td id="tdLinkPrev" class="results-pagination" runat="server">
            &nbsp;<asp:LinkButton ID="lbtnPrev" CommandName="Page" CommandArgument="Prev" OnClick="PreviousClick"
                Text="<%$ Resources:PrevButton %>" runat="server" ToolTip="Previous Page" />&nbsp;
        </td>
        <td class="results-pagination">
            <asp:Menu ID="mnuPageTop" Orientation="Horizontal" runat="server" OnMenuItemClick="mnuPageTop_MenuItemClick"
                PathSeparator="|">
                <StaticMenuItemStyle Font-Underline="true" />
            </asp:Menu>
        </td>
        <td id="tdLinkNext" class="results-pagination" runat="server">
            &nbsp;<asp:LinkButton ID="lbtnNext" CommandName="Page" CommandArgument="Next" OnClick="NextClick"
                Text="<%$ Resources:NextButton %>" runat="server" ToolTip="Next Page" />
        </td>
        <td id="tdLinkLast" class="results-pagination" runat="server">
            &nbsp;<asp:LinkButton ID="lbtnLast" ToolTip="Last Page" CommandName="Page" CommandArgument="Last"
                runat="server" OnClick="LastClick" />
        </td>
    </tr>
</table>
