<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CancelNewAsynchPostback.aspx.cs" Inherits="AJAXPostImplmentation_Cancelling_Controlling_AsynchronousPostback_CancelNewAsynchPostback" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
    div.AlertStyle {
      font-size: x-large;
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
        //prm.add_initializeRequest(InitializeRequest);
        //prm.add_endRequest(EndRequest);        
        
        function InitializeRequest(sender, args)
        {
            if (prm.get_isInAsyncPostBack() & args.get_postBackElement().id == 'Button2')
            {
                args.set_cancel(true);
                ActivateAlertDiv('visible','Previous request is still in Progress');
            }
        }
        
        function EndRequest(sender, args)
        {
           ActivateAlertDiv('hidden','Previous request is still in Progress');
        }
        
        function ActivateAlertDiv(visString,msg)
        {            
             //debugger;
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
                <legend>Cancelling New Asynchronous Postback</legend>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
                <asp:Button ID="Button1" runat="server" Text="Cancelling New Refresh" OnClick="Button1_Click" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br /><br />
        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                <legend>Cancelling New Asynchronous Postback</legend>
                <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label><br />
                <asp:Button ID="Button2" runat="server" Text="Cancelling New Refresh" OnClick="Button2_Click" />
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
