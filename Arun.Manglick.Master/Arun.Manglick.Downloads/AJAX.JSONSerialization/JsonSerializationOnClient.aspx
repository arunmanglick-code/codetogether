<%@ Page Language="VB" AutoEventWireup="false" CodeFile="JsonSerializationOnClient.aspx.vb" Inherits="JsonSerializationOnClient" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>JSON Serialization On The Client</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var SelectedCategories = new Array();

        function SetContextKey(cb) {
            var categoryId = parseInt(cb.value);
        
            if (cb.checked)
                SelectedCategories.push(categoryId); // Add categoryId to the array
            else
                SelectedCategories.slice(SelectedCategories.indexOf(categoryId), 1); // Remove categoryId from the array

            // Serialize the array into JSON
            var json = Sys.Serialization.JavaScriptSerializer.serialize(SelectedCategories);
            
            // Set the contextKey property of the extender
            var extender = $find('AutoCompleteEx');
            if (extender != null)
                extender.set_contextKey(json);
        }
    </script>
</head>
<body>

    <h2>
        <a href="http://localhost/Arun.Manglick.AjaxUI/Home.aspx">AJAX UI</a> || <a href="Default.aspx">Home</a> :: JSON Serialization On The Client</h2>
    <form id="form1" runat="server">
    
    <p>
        This demo sets the AutoCompleteExtender's ContextKey property via JavaScript. A client-side event handler fires whenever any of
        the category checkboxes are clicked, and adds or removes the clicked checkbox's associated CategoryID value to an array, SelectedCategories. 
        This array is then serialized to JSON and assigned to the AutoComplete extender's ContextKey property. In a nutshell, the ContextKey holds 
        the JSON-serialized string of the array of selected category ID values and is passed to the
        AutoComplete script service, where it is deserialized and used in the SQL query to determine what products start with the
        same letters.
    </p>
    
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <p>        
        <asp:ListView ID="ListView1" runat="server" DataSourceID="CategoriesDataSource">
            <LayoutTemplate>
                Limit Product Search By Selected Categories:<br />
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            
            <ItemTemplate>
                <%#CreateCheckBox(Convert.ToInt32(Eval("CategoryID")), Eval("CategoryName").ToString())%>
            </ItemTemplate>
        </asp:ListView>
    </p>
    <p>
        <asp:SqlDataSource ID="CategoriesDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" 
            ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" 
            SelectCommand="SELECT [CategoryID], [CategoryName] FROM [Categories] ORDER BY [CategoryName]">
        </asp:SqlDataSource>
    </p>
    <p>
        Product:
        <asp:TextBox ID="ProductName" autocomplete="off" runat="server"></asp:TextBox>
        <ajaxToolkit:AutoCompleteExtender
                runat="server" 
                BehaviorID="AutoCompleteEx"
                ID="autoComplete1" 
                TargetControlID="ProductName"
                ServicePath="~/Services/AutoCompleteCS.asmx" 
                ServiceMethod="GetCompletionList"
                MinimumPrefixLength="1" 
                CompletionInterval="250"
                EnableCaching="true"
                CompletionSetCount="10"
                DelimiterCharacters=";,:"
                CompletionListCssClass="autocomplete_completionListElement" 
                CompletionListItemCssClass="autocomplete_listItem" 
                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"                
                ShowOnlyCurrentWordInCompletionListItem="true" UseContextKey="True" 
        />
    &nbsp;
        <asp:Button ID="btnShowDetails" runat="server" Text="Show Details" />
    </p>
    <div>
    
        <asp:DetailsView ID="ProductDetails" runat="server" AllowPaging="True" 
            AutoGenerateRows="False" CellPadding="4" DataKeyNames="ProductID" 
            DataSourceID="ProductDataSource" 
            EmptyDataText="No products with the name you entered were found." 
            ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <CommandRowStyle BackColor="#FFFFC0" Font-Bold="True" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <FieldHeaderStyle BackColor="#FFFF99" Font-Bold="True" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <Fields>
                <asp:BoundField DataField="ProductName" HeaderText="Product" 
                    SortExpression="ProductName" />
                <asp:BoundField DataField="CategoryName" HeaderText="Category" 
                    SortExpression="CategoryName" />
                <asp:BoundField DataField="SupplierName" HeaderText="Supplier" 
                    SortExpression="SupplierName" />
                <asp:BoundField DataField="QuantityPerUnit" HeaderText="Qty/Unit" 
                    SortExpression="QuantityPerUnit" />
                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" 
                    SortExpression="UnitPrice" DataFormatString="{0:c}" />
                <asp:BoundField DataField="UnitsInStock" HeaderText="Units In Stock" 
                    SortExpression="UnitsInStock" />
                <asp:BoundField DataField="UnitsOnOrder" HeaderText="Units On Order" 
                    SortExpression="UnitsOnOrder" />
                <asp:BoundField DataField="ReorderLevel" HeaderText="Reorder Level" 
                    SortExpression="ReorderLevel" />
                <asp:CheckBoxField DataField="Discontinued" HeaderText="Discontinued" 
                    SortExpression="Discontinued" />
            </Fields>
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:DetailsView>
        <asp:SqlDataSource ID="ProductDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" 
            
            SelectCommand="SELECT Products.ProductID, Products.ProductName, Categories.CategoryName, Suppliers.CompanyName AS SupplierName, Products.QuantityPerUnit, Products.UnitPrice, Products.UnitsInStock, Products.UnitsOnOrder, Products.ReorderLevel, Products.Discontinued FROM ((Products INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID) INNER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID) WHERE (Products.ProductName = ?)" 
            ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="ProductName" Name="ProductName" 
                    PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>

