<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MathClient.aspx.cs" Inherits="MathClient" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="LINQ Queries"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="validation-no-error">
                <asp:ValidationSummary ID="vlsStipulations" DisplayMode="List" CssClass="validation"
                    runat="server"></asp:ValidationSummary>
            </td>
        </tr>  
    </table>
    <!-- Table 2 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td>
                <div id="divform">
                    <br />
                    <br />
                    <!-- Features Div -->
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Very Primilinary Operation</li>
                            <li>You can use it understand the WSDL</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <div class="DivClassFloat">
                        
                            <asp:Button ID="btnAdd" Text="Addition" CssClass="button" 
                                runat="server" OnClick="btnAdd_Click" Width="150px" /><br /><br />
                           <asp:Button ID="btnSubtract" Text="Subtract" CssClass="button" 
                                runat="server" OnClick="btnSub_Click" Width="150px" /><br />
                           
                           <asp:Button ID="Button1" Text="Asynchronous Addition" CssClass="button" 
                                runat="server" OnClick="btnAddAsync_Click" Width="150px" /><br />
                                
                           <br />
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label><br />
                            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
