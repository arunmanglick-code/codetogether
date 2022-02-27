<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Arun.Manglick.MvcUI.Models.Product>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Index</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table>
        <tr>
            <th></th>
            <th>
                ProductID
            </th>
            <th>
                ProductName
            </th>
            <th>
                SupplierID
            </th>
            <th>
                CategoryID
            </th>
            <th>
                QuantityPerUnit
            </th>
            <th>
                UnitPrice
            </th>
            <th>
                UnitsInStock
            </th>
            <th>
                UnitsOnOrder
            </th>
            <th>
                ReorderLevel
            </th>
            <th>
                Discontinued
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new {  id=item.ProductID  }) %> |
                <%= Html.ActionLink("Details", "Details", new { id = item.ProductID })%>
            </td>
            <td>
                <%= Html.Encode(item.ProductID) %>
            </td>
            <td>
                <%= Html.Encode(item.ProductName) %>
            </td>
            <td>
                <%= Html.Encode(item.SupplierID) %>
            </td>
            <td>
                <%= Html.Encode(item.CategoryID) %>
            </td>
            <td>
                <%= Html.Encode(item.QuantityPerUnit) %>
            </td>
            <td>
                <%= Html.Encode(item.UnitPrice) %>
            </td>
            <td>
                <%= Html.Encode(item.UnitsInStock) %>
            </td>
            <td>
                <%= Html.Encode(item.UnitsOnOrder) %>
            </td>
            <td>
                <%= Html.Encode(item.ReorderLevel) %>
            </td>
            <td>
                <%= Html.Encode(item.Discontinued) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

