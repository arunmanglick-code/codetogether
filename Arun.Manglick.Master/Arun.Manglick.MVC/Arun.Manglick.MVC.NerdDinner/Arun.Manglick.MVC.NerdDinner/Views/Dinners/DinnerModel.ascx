<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Arun.Manglick.MVC.NerdDinner.Models.DinnerCreateFormViewModel>" %>

    <%= Html.ValidationSummary() %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="DinnerID">DinnerID:</label>
                <%= Html.TextBox("DinnerID") %>
                <%= Html.ValidationMessage("DinnerID", "*") %>
            </p>
            <p>
                <label for="Title">Title:</label>
                <%= Html.TextBox("Title", Model.Dinner.Title)%>
                <%= Html.ValidationMessage("Title", "*") %>
            </p>
            <p>
                <label for="EventDate">EventDate:</label>
                <%= Html.TextBox("EventDate", Model.Dinner.EventDate) %>
                <%= Html.ValidationMessage("EventDate", "*") %>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextBox("Description", Model.Dinner.Description) %>
                <%= Html.ValidationMessage("Description", "*") %>
            </p>
            <p>
                <label for="HostedBy">HostedBy:</label>
                <%= Html.TextBox("HostedBy", Model.Dinner.HostedBy)%>
                <%= Html.ValidationMessage("HostedBy", "*") %>
            </p>
            <p>
                <label for="ContactPhone">ContactPhone:</label>
                <%= Html.TextBox("ContactPhone", Model.Dinner.ContactPhone)%>
                <%= Html.ValidationMessage("ContactPhone", "*") %>
            </p>
            <p>
                <label for="Address">Address:</label>
                <%= Html.TextBox("Address", Model.Dinner.Address)%>
                <%= Html.ValidationMessage("Address", "*") %>
            </p>
            <p>
                <label for="Country">Country:</label>
                <%= Html.DropDownList("Country", Model.Countries)%>
                <%= Html.ValidationMessage("Country", "*") %>
            </p>
            <p>
                <label for="Latitude">Latitude:</label>
                <%= Html.TextBox("Latitude", Model.Dinner.Latitude) %>
                <%= Html.ValidationMessage("Latitude", "*") %>
            </p>
            <p>
                <label for="Longitude">Longitude:</label>
                <%= Html.TextBox("Longitude", Model.Dinner.Longitude) %>
                <%= Html.ValidationMessage("Longitude", "*") %>
            </p>
        </fieldset>
        
        <p>
            <input type="submit" value="Create - Using Custom Model" />
        </p>

    <% } %>

    


