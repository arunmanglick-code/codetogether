<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SimpleWebService.aspx.cs" Inherits="SimpleWebService" Title="Simple WebService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/JS to ServerSide Cafe/Web Services/SimpleWebService.asmx" InlineScript="true" />
        </Services>
    </asp:ScriptManager>
    
    <script type="text/javascript">
  
            // This function calls the Web Service method.  
            function EchoUserInput()
            {
                var echoElem = document.getElementById("EnteredValue");
                Arun.Manglick.UI.SimpleWebService.EchoInput(echoElem.value,  SucceededCallback);
            }
  
            // This is the callback function that processes the Web Service return value.
            function SucceededCallback(result)
            {
                var RsltElem = document.getElementById("Results");
                RsltElem.innerHTML = result;
            }
  
   </script>
        
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Simple Web Service"></asp:Label></td>
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
                            <li>Enter Text in TextBox and Press Button</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                    
                    <p>Calling a simple service that echoes the user's input and returns the current server time.</p>
                    <input id="EnteredValue" type="text" class="inputfield" />
                    <input id="EchoButton" type="button" class="button" value="Echo" onclick="EchoUserInput()" />
                    
                    </div>
                    <br />
                     <div>
                        <span id="Results" style="color:Red"></span>
                    </div>   
        
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
