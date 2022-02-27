<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Arun.Manglick.MVC.NerdDinner.Models.DinnerCreateFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>CreateCustomModel</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>CreateCustomModel</h2>

    <%= Html.ValidationSummary() %>

    <% Html.RenderPartial("DinnerModel"); %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

