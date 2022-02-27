<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    UICulture="auto" Culture="auto" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="contentHeader" ContentPlaceHolderID="cphHeaderContent" runat="Server">  
</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="cphBodyContent" runat="Server">
   <asp:Literal ID="literal" runat="server" Text="Welcome to Home Page" />
</asp:Content>

