<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    UICulture="auto" Culture="auto" CodeFile="SilverlightHome.aspx.cs" Inherits="Home" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<asp:Content ID="contentHeader" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="height: 100%;">
        <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/Arun.Manglick.WebApp.xap"
            Version="2.0" Width="100%" Height="100%" />
    </div>
    <br />
    <div>
       <asp:Literal ID="literal" runat="server" Text="Welcome to Home Page" />        
    </div>
</asp:Content>
