<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomizeErrorHandling_CustomMsgBox.aspx.cs" Inherits="CustomizeErrorHandlingCustomMsgBox"
    Title="CustomizeErrorHandling_CustomMsgBox Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();        
        prm.add_endRequest(EndRequest);        
        
                
        function EndRequest(sender, args)
        {
           if (args.get_error() != undefined)
           {
               var errorMessage;
               if (args.get_response().get_statusCode() == '200')
               {
                   errorMessage = args.get_error().message;
               }
               else
               {
                   // Error occurred somewhere other than the server page.
                   errorMessage = 'An unspecified error occurred. ';
               }
               args.set_errorHandled(true);
               ActivateAlertDiv('visible',errorMessage);
               window.setTimeout("ActivateAlertDiv('hidden','')", 2000);
           }

           //ActivateAlertDiv('hidden','Previous request is still in Progress');
        }
        
        function ActivateAlertDiv(visString,msg)
        {            
             var adiv = $get('AlertDiv');
             var aspan = $get('AlertMessage');
             adiv.style.visibility = visString;
             aspan.innerHTML = msg;
        }        
       
        if(typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
    </script>

    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Disabling Automatic Triggers"></asp:Label>
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
                    <div class="DivClassFeature" style="width: 800px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Customize error handling during Partial Postback - Custom Message box</li>
                            <li>When an error occurs during partial-page updates in UpdatePanel controls, the default behavior is that a browser message box is displayed with an error message</li>
                            <li>To customize error handling all of such things, you need to work with the 'OnAsyncPostBackError' property of the ScriptManager control</li>
                            <li>To show the error in a custom message box, you need to handle the 'EndRequest' event of the 'PageRequestManager' class. Just add a js file within ScriptManager control</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Custom Error Handling [Custom Msg Box] </legend>
                                    <br />
                                    <br />
                                    Last refresh<%=DateTime.Now.ToString() %><br />
                                    <br />
                                    <asp:Button ID="Button1" CssClass="button" runat="server" Text="Custom Error Handling "
                                        OnClick="Button1_Click" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
     <div id="AlertDiv" class="AlertStyle">
    <span id="AlertMessage"></span> 
    </div>
</asp:Content>
