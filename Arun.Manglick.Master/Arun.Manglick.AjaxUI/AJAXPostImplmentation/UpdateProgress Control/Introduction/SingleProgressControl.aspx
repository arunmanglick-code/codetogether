<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingleProgressControl.aspx.cs" Inherits="AJAXPostImplmentation_UpdateProgress_Control_Introduction_SingleProgressControl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    Note: Even when 'ChildrenAsTriggers' is set to false, UpdateProgress control still fires.<br /><br />
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <fieldset>
                <legend>Single Update Panel</legend>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
                <asp:Button ID="Button1" runat="server" Text="Refresh Panel" OnClick="Button1_Click" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
         <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                Processing... <img src="../../Images/ajax-loader.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>

        
    </div>
    </form>
</body>
</html>
