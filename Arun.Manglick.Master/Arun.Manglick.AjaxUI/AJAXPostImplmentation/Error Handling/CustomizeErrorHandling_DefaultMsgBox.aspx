<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomizeErrorHandling_DefaultMsgBox.aspx.cs" Inherits="AJAXPostImplmentation_Error_Handling_CustomizeErrorHandling_DefaultMsgBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" OnAsyncPostBackError="ScriptManager1_AsyncPostBackError">
        </asp:ScriptManager>
    <div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                <legend>Custom Error Handling [Default Msg Box] </legend>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
                <asp:Button ID="Button1" runat="server" Text="Custom Error Handling " OnClick="Button1_Click" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
