<%@ Page Language="C#" AutoEventWireup="true" CodeFile="actual_message.aspx.cs" Inherits="Vocada.CSTools.actual_message" %>


<html>
<head runat="server">
    <title>CSTools: Notification message</title>
    <link href="App_Themes/csTool/cstool.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="98%" border="0" cellpadding="5" cellspacing="5">
                <tr class="ContentBg">
                    <td valign="top" align="center">
                        <div style="overflow-y: Auto; height: 320px;width: 98%;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">                                
                                <tr>
                                    <td style="vertical-align: top; width: 12%;" align="left" class="Hd2">
                                        Subject :</td>
                                    <td style="vertical-align: top;" align="left">
                                        <asp:Label ID="lblSubjectText" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; width: 12%;" class="Hd2" align="left">
                                        Body :
                                    </td>
                                    <td style="vertical-align: top;" align="left">
                                        <asp:Label ID="lblBodyText" runat="server" Text=""></asp:Label></td>
                                </tr>  
                                 <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>                                   
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
