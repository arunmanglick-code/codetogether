<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Arun.Manglick.MVC.NerdDinner.Models.Dinner>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Delete</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete Confirmation</h2>
    <div>
        <p>Please confirm you want to cancel the dinner titled:
        <i> <%=Html.Encode(Model.Title) %>? </i> </p>
    </div>
    
    <% using (Html.BeginForm()) { %>
        <input name="confirmButton" type="submit" value="Delete" />
    <% } %>

</asp:Content>
