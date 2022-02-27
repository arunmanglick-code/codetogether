<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Arun.Manglick.MvcUI.Models.Product>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Details</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            ProductID: 
            <%= Html.Encode(Model.ProductID) %>
        </p>
        <p>
            ProductName: 
            <%= Html.Encode(Model.ProductName) %>
        </p>
        <p>
            SupplierID: 
            <%= Html.Encode(Model.SupplierID) %>
        </p>
        <p>
            CategoryID: 
            <%= Html.Encode(Model.CategoryID) %>
        </p>
        <p>
            QuantityPerUnit: 
            <%= Html.Encode(Model.QuantityPerUnit) %>
        </p>
        <p>
            UnitPrice: 
            <%= Html.Encode(Model.UnitPrice) %>
        </p>
        <p>
            UnitsInStock: 
            <%= Html.Encode(Model.UnitsInStock) %>
        </p>
        <p>
            UnitsOnOrder: 
            <%= Html.Encode(Model.UnitsOnOrder) %>
        </p>
        <p>
            ReorderLevel: 
            <%= Html.Encode(Model.ReorderLevel) %>
        </p>
        <p>
            Discontinued: 
            <%= Html.Encode(Model.Discontinued) %>
        </p>
    </fieldset>
    <p>
        <%=Html.ActionLink("Edit", "Edit", new { /* id=Model.PrimaryKey */ }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

