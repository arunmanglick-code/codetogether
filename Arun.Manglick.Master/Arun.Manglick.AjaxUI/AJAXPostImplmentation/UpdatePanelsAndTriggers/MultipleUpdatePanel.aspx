<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MultipleUpdatePanel.aspx.cs" Inherits="AJAXPostImplmentation_UpdatePanelsAndTriggers_MultipleUpdatePanel" %>

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
    <asp:UpdatePanel id="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset>
                    <legend>Multiple Update Panel</legend>
                    Last refresh <%=DateTime.Now.ToString() %><br />
                    <asp:Button ID="Button1" runat="server" Text="Refresh Panel 1" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                    <fieldset>
                    <legend>Multiple Update Panel</legend>
                    Last refresh <%=DateTime.Now.ToString() %><br />
                    <asp:Button ID="Button2" runat="server" Text="Refresh Panel 2" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
