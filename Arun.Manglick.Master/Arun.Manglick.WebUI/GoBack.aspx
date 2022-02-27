<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    UICulture="auto" Culture="auto" CodeFile="GoBack.aspx.cs" Inherits="GoBack" %>

<asp:Content ID="contentHeader" ContentPlaceHolderID="cphHeaderContent" runat="Server">
  
</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:Button ID="btnBack" runat="server" CssClass="button" Text="Go Back" OnClick="btnBack_Click"  />
    
</asp:Content>

