<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForceToDisplayUpdateProgress-1.aspx.cs" Inherits="AJAXPostImplmentation_UpdateProgress_Control_CancellingAndControlling_ForceToDisplayUpdateProgress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">            
     </asp:ScriptManager>
     
     <script language="javascript" type="text/javascript">
        
	    var prm = Sys.WebForms.PageRequestManager.getInstance();
        
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        var postBackElement;
        function InitializeRequest(sender, args) 
        {
            if (prm.get_isInAsyncPostBack()) 
            {
                args.set_cancel(true);  // To cancel a new async postback
            }
            
            postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'Button1') 
            {
                $get('UpdateProgress1').style.display = 'block';                
            }
        }
        
        function EndRequest(sender, args) 
        {
            if (postBackElement.id == 'Button1') 
            {
                $get('UpdateProgress1').style.display = 'none';
            }
        }
        
	    function AbortPostBack() 
	    {
            if (prm.get_isInAsyncPostBack()) 
            {
              prm.abortPostBack();
            }
        }
   </script>
      
        <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                <legend><b>Force to fire Update Progress</b></legend>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Button ID="Button1" runat="server" Text="Refresh Panel using Trigger" OnClick="Button1_Click" />
        
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
          Update in progress... <img src="../../Images/ajax-loader.gif" />
          <input type="button" value="stop" onclick="AbortPostBack()" id="Button2" />
        </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    </form>
</body>
</html>
