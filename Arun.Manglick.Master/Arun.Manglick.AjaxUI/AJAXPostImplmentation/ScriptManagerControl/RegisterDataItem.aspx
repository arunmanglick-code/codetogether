<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterDataItem.aspx.cs" Inherits="AJAXPostImplmentation_ScriptManagerControl_RegisterDataItem" %>

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
        //prm.add_endRequest(EndRequest);        
        prm.add_pageLoading(PageLoadingHandler);       
        
        function EndRequest(sender, args)
        {
           alert('Done');
        }       
        
        function PageLoadingHandler(sender,args)
        {
            var dataItems = args.get_dataItems(); 
            if($get('TextBox1')!==null)
            {
                $get('TextBox1').value = dataItems['TextBox1']; 
            }
        return; 
    }
       
       
        if(typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
        </script>  
    <div>
        See how Controls Out of Panel can be updated using 'ScriptManager1.RegisterDataItem': <br /><br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br /><br />
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="Button1" runat="server" Text="Register Data Item" OnClick="Button1_Click" />        
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>
    
    <br />
    
    <div>
    <%= DateTime.Now.ToString() %>'
    
        <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
    </div>
    </form>
</body>
</html>
