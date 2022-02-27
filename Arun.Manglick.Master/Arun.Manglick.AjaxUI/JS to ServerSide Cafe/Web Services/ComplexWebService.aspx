<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ComplexWebService.aspx.cs" Inherits="ComplexWebService" Title="Complex WebService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Scripts>
            <asp:ScriptReference Path="~/JS/CallWebServiceMethods.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/JS to ServerSide Cafe/Web Services/ComplexWebService.asmx"
                InlineScript="true" />
        </Services>
    </asp:ScriptManager>

    

    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Simple Web Service"></asp:Label>
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
                    <div class="DivClassFeature" style="width:700px;">
                        <b>Varoius Features Used.</b><br /><br />
                        Below are the variants in the calling Syntax.
                        <ol>                            
                            <li>Arun.Manglick.UI.ComplexWebService.Sum(txtNum1.value, txtNum2.value, OnSumComplete)</li>
                            <li>Arun.Manglick.UI.ComplexWebService.Sum(txtNum1.value, txtNum2.value, OnSumComplete, OnSumError)</li>
                            <li>Arun.Manglick.UI.ComplexWebService.Sum(txtNum1.value, txtNum2.value, OnSumComplete, OnSumError, txtResult)</li>
                            
                            
                            
                            <li>OnSumComplete (result, txtResult, methodName)</li>
                            <li>OnSumError (error, txtResult, methodName)</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table>
                            <tr align="left">
                                <td>
                                    Method that does not return a value:
                                </td>
                                <td>
                                    <!-- Getting no retun value from the Web service. -->
                                    <button id="Button1" onclick="GetNoReturn()" class="button">No Return</button>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Method that returns a value:
                                </td>
                                <td>
                                    <!-- Getting a retun value from the Web service. -->
                                    <button id="Button2" onclick="GetTime(); return false;" class="button">Server Time</button>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Method that takes parameters:
                                </td>
                                <td>
                                    <!-- Passing simple parameter types to the Web service. -->
                                    <button id="Button3" onclick="Add(20, 30); return false;" class="button">Add (20,30)</button>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Method that takes 'SumObject' parameter:
                                </td>
                                <td>
                                    <!-- Passing simple parameter types to the Web service. -->
                                    <button id="Button6" onclick="AddSumObject(50, 50); return false;" class="button">Add (50,50)</button>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Method that returns XML data:
                                </td>
                                <td>
                                    <!-- Get Xml. -->
                                    <button id="Button4" onclick="GetXmlDocument(); return false;" class="button">Get Xml</button>
                                </td>
                            </tr>
                            <tr align="left">
                                <td>
                                    Method that uses GET:
                                </td>
                                <td>
                                    <!-- Making a GET Web request. -->
                                    <button id="Button5" onclick="MakeGetRequest(); return false;" class="button">Make GET Request</button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div>
                        <br />
                        <span id="ResultId" style="color:red;"></span>
                    </div>   
        
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
