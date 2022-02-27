<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TextToImageHandler.aspx.cs" Inherits="TemplatePage" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Text to Image Generator"></asp:Label>
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
                                someone requests a file with the extension .gif. 
                            </li>
                            <li>
                                In such cases, HTTP handler is created by making a class that implements the IHttpHandler interface.
                            </li>
                        </ul>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <img src="../Handlers/TextToImageHandler.ashx?text=Generic Handler&font=WebDings&size=22"
                            alt="" />
                        <br />
                        <img src="../Handlers/TextToImageHandler.ashx?text=Image Generation - Using Generic Handler&font=Comic Sans MS&size=22"
                            alt="" />
                        <hr />
                        <img src="../Handlers/ImageGenerator.aspx?text=ASPX Page&font=WebDings&size=22" alt="" />
                        <br />
                        <img src="../Handlers/ImageGenerator.aspx?text=Image Generation - Using ASPX Page&font=Comic Sans MS&size=22"
                            alt="" />
                        <br />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
