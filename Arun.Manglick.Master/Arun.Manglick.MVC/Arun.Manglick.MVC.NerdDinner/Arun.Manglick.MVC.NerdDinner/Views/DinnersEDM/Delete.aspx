<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Arun.Manglick.MVC.NerdDinner.Models.EDM.Dinners>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<title>Delete</title>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete Confirmation</h2>
    <div>
        <p>EDM: Please confirm you want to cancel the dinner titled:
         </p>
    </div>
    
    <% using (Html.BeginForm()) { %>
        <input name="confirmButton" type="submit" value="Delete" />
    <% } %>

</asp:Content>


