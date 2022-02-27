<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InformationSearch.ascx.cs" Inherits="InformationSearch" EnableTheming="true" %>

<table border="0" width="100%" cellpadding="1" cellspacing="3">
    <!-- Row 1 -->
    <tr>
        <td colspan="5">
            <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                Text="InformationSearch User Control"></asp:Label>
        </td>
    </tr>
    <!-- Row 2 -->
    <tr>
        <td style="width: 1%" class="requiredcolumn">
            &nbsp;
        </td>
        <td style="width: 10%" class="labelcolumn">
            <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
        </td>
        <td style="width: 30%" class="inputcolumn">
            <asp:TextBox ID="txtFirstName" class="inputfield" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator Text="<img src='../Images/error-icon.gif'/>" ID="rfvFirstName"
            runat="server" ControlToValidate="txtFirstName" ErrorMessage="Please Enter UserControl Level Information"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td style="width: 1%" class="requiredcolumn">
            &nbsp;
        </td>
        <td style="width: 10%" class="labelcolumn">
            <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
        </td>
        <td style="width: 30%" class="inputcolumn">
            <asp:TextBox ID="txtLastName" class="inputfield" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 1%" class="requiredcolumn">
            &nbsp;
        </td>
        <td style="width: 10%" class="labelcolumn">
            <asp:Label ID="lblAge" runat="server" Text="Age"></asp:Label>
        </td>
        <td style="width: 30%" class="inputcolumn">
            <asp:TextBox ID="txtAge" class="inputfield" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 1%" class="requiredcolumn">
            &nbsp;
        </td>
        <td style="width: 10%" class="labelcolumn">
            
        </td>
        <td style="width: 30%" class="inputcolumn">
            <asp:Button ID="btnSearch"  class="button" runat="server" Text="Search" OnClick="btnSearch_Click" />
        </td>
    </tr>
</table>
