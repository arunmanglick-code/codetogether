<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NestedUpdatePanel.aspx.cs" Inherits="AJAXPostImplmentation_UpdatePanelsAndTriggers_NestedUpdatePanel" %>

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
        
        <asp:UpdatePanel id="UpdatePanel1" UpdateMode="Conditional" runat="server" >
            <ContentTemplate>
                <fieldset>
                <legend>Parent UpdatePanel</legend>
                Outer refresh <%=DateTime.Now.ToString() %><br />
                <asp:Button ID="Button1" runat="server" Text="Refresh Outer & Inner Panel" /><br /><br />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <fieldset>
                        <legend>Nested UpdatePanel</legend>
                         Inner refresh <%=DateTime.Now.ToString() %> <br />
                        <asp:Button ID="Button2" runat="server" Text="Refresh Inner Panel" />
                        <asp:Button ID="Button3" runat="server" Text="Refresh Outer Panel forcely" OnClick="Button3_Click" />
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
