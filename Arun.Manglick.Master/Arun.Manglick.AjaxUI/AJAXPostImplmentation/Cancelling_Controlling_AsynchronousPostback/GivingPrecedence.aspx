<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GivingPrecedence.aspx.cs" Inherits="AJAXPostImplmentation_Cancelling_Controlling_AsynchronousPostback_GivingPrecedence" %>

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
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);        
        
        var exclusivePostBackElement = 'ExclusiveButton';
        var lastPostBackElement;

        function InitializeRequest(sender, args)
        {            
            //debugger;
            if (prm.get_isInAsyncPostBack())
            {
                 if (args.get_postBackElement().id == 'ExclusiveButton')
                    {    
                        if(lastPostBackElement == exclusivePostBackElement)
                         {
                            args.set_cancel(true);
                            ActivateAlertDiv('visible','Previous request is still in Progress');                           
                         }
                         else
                         {
                           prm.abortPostBack();
                         }                                          
                    }
                 else if (args.get_postBackElement().id == 'SharedButton')
                   {
                         if(lastPostBackElement != exclusivePostBackElement)
                         {                         
                            args.set_cancel(true);
                            ActivateAlertDiv('visible','Previous request is still in Progress');
                         }
                   }
            }
            
            lastPostBackElement=args.get_postBackElement().id;
        }
        
        function EndRequest(sender, args)
        {
           ActivateAlertDiv('hidden','Previous request is still in Progress');
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
             <asp:UpdatePanel  ID="UpdatePanel1" UpdateMode="Conditional" runat="Server" >
                    <ContentTemplate>
                        <fieldset>
                        <legend>Refreshing this UpdatePanel Takes Precedence</legend> <br />               
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
                        <asp:Button ID="ExclusiveButton" runat="server" Text="Take Precedence" OnClick="Button1_Click" />&nbsp;
                        </fieldset>
                        
                       <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">                        
                        <ProgressTemplate>
                        Panel1 updating...
                        </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
            </asp:UpdatePanel>
            
            <br /><br />
            
            <asp:UpdatePanel  ID="UpdatePanel2" UpdateMode="Conditional" runat="Server" >
                    <ContentTemplate>
                        <fieldset>
                        <legend>Refresh of this UpdatePanel can be overwritten by above Panel</legend>   <br />             
                        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label><br />
                        <asp:Button ID="SharedButton" runat="server" Text="Refresh" OnClick="Button2_Click" />&nbsp;
                        </fieldset>
                        
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">                        
                        <ProgressTemplate>
                        Panel1 updating...
                        </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
            </asp:UpdatePanel>

    </div>
    
    <div id="AlertDiv" class="AlertStyle">
    <span id="AlertMessage"></span> 
    </div>
    </form>
</body>
</html>
