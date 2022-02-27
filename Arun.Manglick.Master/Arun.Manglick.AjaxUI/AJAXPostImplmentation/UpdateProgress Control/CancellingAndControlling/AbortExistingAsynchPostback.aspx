<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AbortExistingAsynchPostback.aspx.cs" Inherits="AJAXPostImplmentation_UpdateProgress_Control_CancellingAndControlling_AbortExistingAsynchPostback" %>

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
        
        function AbortPostBack()
        {
            if (prm.get_isInAsyncPostBack())
            {
               prm.abortPostBack();
            }
        }
        if(typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
      </script> 
    
    <div>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                <legend>Abort Existing Asynchronous Postback</legend>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
                <asp:Button ID="Button1" runat="server" Text="Abort Refreshing Panel" OnClick="Button1_Click" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
          Update in progress... <img src="../../Images/ajax-loader.gif" />
          <input type="button" value="stop" onclick="AbortPostBack()" id="Button2" />
        </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <div id="AlertDiv" class="AlertStyle">
    <span id="AlertMessage"></span> 
    </div>
    </form>
</body>
</html>
