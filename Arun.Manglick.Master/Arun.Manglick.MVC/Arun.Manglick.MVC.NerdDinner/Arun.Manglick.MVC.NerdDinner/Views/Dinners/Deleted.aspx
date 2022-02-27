<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Deleted</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Dinner Deleted Successfully</h2>
    <div>
    <p>Your dinner was successfully deleted.</p>
    </div>
    <div>
    <p><a href="/Dinners">Click for Upcoming Dinners</a></p>
    </div>

</asp:Content>
