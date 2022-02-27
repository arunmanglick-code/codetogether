<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisablingAutomaticTriggers.aspx.cs" Inherits="AJAXPostImplmentation_ControllingUpdatePanelsAndTriggers_DisablingAutomaticTriggers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div style="border:dashed 1px red;">
        <strong><span style="color: #ff9933">
    Disabling Automatic Triggers </span></strong><br /><br />
    <asp:UpdatePanel id="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
            <ContentTemplate>
                <fieldset>
                    <legend>Triggering Panel</legend>
                    Last refresh <%=DateTime.Now.ToString() %><br />
                    <asp:Button ID="Button1" runat="server" Text="Refresh below Panel & not to yourself" />
                    <asp:Button ID="Button2" runat="server" Text="Refresh below Panel & forcely to yourself" OnClick="Button2_Click" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br /><br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                    <fieldset>
                    <legend>Getting Update From above UpdatePanel using Trigger</legend>
                    Last refresh <%=DateTime.Now.ToString() %><br />                    
                </fieldset>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
