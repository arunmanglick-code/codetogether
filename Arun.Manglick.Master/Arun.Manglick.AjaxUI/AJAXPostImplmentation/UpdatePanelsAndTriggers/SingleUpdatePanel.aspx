<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingleUpdatePanel.aspx.cs" Inherits="AJAXPostImplmentation_UpdatePanelsAndTriggers_SingleUpdatePanel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                <legend>Single Update Panel</legend>
                Last refresh <%=DateTime.Now.ToString() %><br />
                <asp:Button ID="Button1" CssClass="button"  runat="server" Text="Refresh Panel" OnClick="Button1_Click" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br /><br />
        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                <legend>Single Update Panel with Trigger</legend>
                    Last refresh <%=DateTime.Now.ToString() %><br />
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Button ID="Button2" CssClass="button" runat="server" Text="Refresh Panel using Trigger Effect" OnClick="Button2_Click" /><br /><br />
        <asp:Button ID="Button3" CssClass="button"  runat="server" Text="Complete Page Load" />
  </div>
    </form>
</body>
</html>
