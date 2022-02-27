<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgrammaticallyAddingTriggers.aspx.cs" Inherits="AJAXPostImplmentation_ControllingUpdatePanelsAndTriggers_ProgrammaticallyAddingTriggers" %>

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
                <legend> Programmatically Adding Triggers</legend>
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </fieldset>
            </ContentTemplate>           
        </asp:UpdatePanel>
        <asp:Button ID="Button1" runat="server" Text="Programmatically Added Trigger Effect" OnClick="Button1_Click" />
    </div>
    </form>
</body>
</html>
