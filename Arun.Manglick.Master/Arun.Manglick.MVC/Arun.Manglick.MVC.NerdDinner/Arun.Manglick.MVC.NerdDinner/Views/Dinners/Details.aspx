<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Arun.Manglick.MVC.NerdDinner.Models.Dinner>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Details</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            DinnerID: 
            <%= Html.Encode(Model.DinnerID) %>
        </p>
        <p>
            Title: 
            <%= Html.Encode(Model.Title) %>
        </p>
        <p>
            EventDate: 
            <%= Html.Encode(Model.EventDate) %>
        </p>
        <p>
            Description: 
            <%= Html.Encode(Model.Description) %>
        </p>
        <p>
            HostedBy: 
            <%= Html.Encode(Model.HostedBy) %>
        </p>
        <p>
            ContactPhone: 
            <%= Html.Encode(Model.ContactPhone) %>
        </p>
        <p>
            Address: 
            <%= Html.Encode(Model.Address) %>
        </p>
        <p>
            Country: 
            <%= Html.Encode(Model.Country) %>
        </p>
        <p>
            Latitude: 
            <%= Html.Encode(Model.Latitude) %>
        </p>
        <p>
            Longitude: 
            <%= Html.Encode(Model.Longitude) %>
        </p>
        <p>
            IsValid: 
            <%= Html.Encode(Model.IsAMValid) %>
        </p>
    </fieldset>
    <p>
        <%=Html.ActionLink("Edit", "Edit", new { /* id=Model.PrimaryKey */ }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

