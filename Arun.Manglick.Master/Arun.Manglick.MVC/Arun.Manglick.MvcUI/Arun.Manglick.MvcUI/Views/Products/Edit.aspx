<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Arun.Manglick.MvcUI.Models.Product>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Edit</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.ValidationSummary() %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="ProductID">ProductID:</label>
                <%= Html.TextBox("ProductID") %>
                <%= Html.ValidationMessage("ProductID", "*") %>
            </p>
            <p>
                <label for="ProductName">ProductName:</label>
                <%= Html.TextBox("ProductName") %>
                <%= Html.ValidationMessage("ProductName", "*") %>
            </p>
            <p>
                <label for="SupplierID">SupplierID:</label>
                <%= Html.TextBox("SupplierID") %>
                <%= Html.ValidationMessage("SupplierID", "*") %>
            </p>
            <p>
                <label for="CategoryID">CategoryID:</label>
                <%= Html.TextBox("CategoryID") %>
                <%= Html.ValidationMessage("CategoryID", "*") %>
            </p>
            <p>
                <label for="QuantityPerUnit">QuantityPerUnit:</label>
                <%= Html.TextBox("QuantityPerUnit") %>
                <%= Html.ValidationMessage("QuantityPerUnit", "*") %>
            </p>
            <p>
                <label for="UnitPrice">UnitPrice:</label>
                <%= Html.TextBox("UnitPrice") %>
                <%= Html.ValidationMessage("UnitPrice", "*") %>
            </p>
            <p>
                <label for="UnitsInStock">UnitsInStock:</label>
                <%= Html.TextBox("UnitsInStock") %>
                <%= Html.ValidationMessage("UnitsInStock", "*") %>
            </p>
            <p>
                <label for="UnitsOnOrder">UnitsOnOrder:</label>
                <%= Html.TextBox("UnitsOnOrder") %>
                <%= Html.ValidationMessage("UnitsOnOrder", "*") %>
            </p>
            <p>
                <label for="ReorderLevel">ReorderLevel:</label>
                <%= Html.TextBox("ReorderLevel") %>
                <%= Html.ValidationMessage("ReorderLevel", "*") %>
            </p>
            <p>
                <label for="Discontinued">Discontinued:</label>
                <%= Html.TextBox("Discontinued") %>
                <%= Html.ValidationMessage("Discontinued", "*") %>
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

