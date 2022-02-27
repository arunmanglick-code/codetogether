<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DBImageHTTPHandler.aspx.cs" Inherits="DBImageHTTPHandler" Title="DB Image HTTPHandler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="DB Image HTTPHandlerr"></asp:Label>
            </td>
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
                    <div class="DivClassFeature" style="width: 600px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>This is about generating Images from Text</li>
                            <li>Two approaches</li>
                            <li>Generic Handler - HTTP Handler</li>
                            <li>ASPX Page</li>
                        </ol>
                        <ul>
                            <li>The big disadvantage of a Generic Handler is that you cannot map a Generic Handler
                                to a particular page path. For example, you cannot execute a Generic Handler whenever
                                someone requests a file with the extension .gif. </li>
                            <li>In such cases, HTTP handler is created by making a class that implements the IHttpHandler
                                interface. </li>
                        </ul>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <asp:Label ID="lblFile" CssClass="label" Text="Image File:" AssociatedControlID="upFile" runat="server" />
                        <asp:FileUpload ID="upFile" runat="server" CssClass="inputfield" />
                        <asp:Button ID="btnAdd" CssClass="button" Text="Add Image" OnClick="btnAdd_Click" runat="server" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
