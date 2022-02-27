<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomizeErrorHandling_CustomMsgBox.aspx.cs" Inherits="AJAXPostImplmentation_Error_Handling_CustomizeErrorHandling_CustomMsgBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
     <style type="text/css">
    div.AlertStyle {
      font-size: smaller;
      background-color: #FFC080;
      width: 200px;
      height: 20px;
      visibility: hidden;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">    
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
    <div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                <legend>Custom Error Handling [Custom Msg Box] </legend>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
                <asp:Button ID="Button1" runat="server" Text="Custom Error Handling " OnClick="Button1_Click" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <div id="AlertDiv" class="AlertStyle">
    <span id="AlertMessage"></span> 
    </div>
    </form>
</body>
</html>
