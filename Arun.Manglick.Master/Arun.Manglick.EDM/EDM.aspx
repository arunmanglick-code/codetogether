<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EDM.aspx.cs" Inherits="EDM" %>

<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
            CellPadding="4" DataSourceID="EntityDataSource1" ForeColor="Black" 
            GridLines="Vertical">
            <RowStyle BackColor="#F7F7DE" />
            <Columns>
                <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" ReadOnly="True" 
                    SortExpression="CustomerID" />
                <asp:BoundField DataField="CompanyName" HeaderText="CompanyName" 
                    ReadOnly="True" SortExpression="CompanyName" />
                <asp:BoundField DataField="ContactName" HeaderText="ContactName" 
                    ReadOnly="True" SortExpression="ContactName" />
                <asp:BoundField DataField="ContactTitle" HeaderText="ContactTitle" 
                    ReadOnly="True" SortExpression="ContactTitle" />
                <asp:BoundField DataField="Country" HeaderText="Country" ReadOnly="True" 
                    SortExpression="Country" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" ReadOnly="True" 
                    SortExpression="Phone" />
            </Columns>
            <FooterStyle BackColor="#CCCC99" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:EntityDataSource ID="EntityDataSource1" runat="server" 
            ConnectionString="name=northwndEntities" 
            DefaultContainerName="northwndEntities" EntitySetName="Customers" 
            Select="it.[CustomerID], it.[CompanyName], it.[ContactName], it.[ContactTitle], it.[Country], it.[Phone]">
        </asp:EntityDataSource>
    </div>
    </form>
</body>
</html>
