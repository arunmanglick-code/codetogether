<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Arun.Manglick.MVC.NerdDinner.Models.EDM.Dinners>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="DinnerID">DinnerID:</label>
                <%= Html.TextBox("DinnerID", Model.DinnerID) %>
                <%= Html.ValidationMessage("DinnerID", "*") %>
            </p>
            <p>
                <label for="Title">Title:</label>
                <%= Html.TextBox("Title", Model.Title) %>
                <%= Html.ValidationMessage("Title", "*") %>
            </p>
            <p>
                <label for="EventDate">EventDate:</label>
                <%= Html.TextBox("EventDate", String.Format("{0:g}", Model.EventDate)) %>
                <%= Html.ValidationMessage("EventDate", "*") %>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextBox("Description", Model.Description) %>
                <%= Html.ValidationMessage("Description", "*") %>
            </p>
            <p>
                <label for="HostedBy">HostedBy:</label>
                <%= Html.TextBox("HostedBy", Model.HostedBy) %>
                <%= Html.ValidationMessage("HostedBy", "*") %>
            </p>
            <p>
                <label for="ContactPhone">ContactPhone:</label>
                <%= Html.TextBox("ContactPhone", Model.ContactPhone) %>
                <%= Html.ValidationMessage("ContactPhone", "*") %>
            </p>
            <p>
                <label for="Address">Address:</label>
                <%= Html.TextBox("Address", Model.Address) %>
                <%= Html.ValidationMessage("Address", "*") %>
            </p>
            <p>
                <label for="Country">Country:</label>
                <%= Html.TextBox("Country", Model.Country) %>
                <%= Html.ValidationMessage("Country", "*") %>
            </p>
            <p>
                <label for="Latitude">Latitude:</label>
                <%= Html.TextBox("Latitude", String.Format("{0:F}", Model.Latitude)) %>
                <%= Html.ValidationMessage("Latitude", "*") %>
            </p>
            <p>
                <label for="Longitude">Longitude:</label>
                <%= Html.TextBox("Longitude", String.Format("{0:F}", Model.Longitude)) %>
                <%= Html.ValidationMessage("Longitude", "*") %>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

