<%@ Page Language="c#" Inherits="Vocada.CSTools.MarkMessageReceived" CodeFile="mark_message_received.aspx.cs"
    MasterPageFile="~/cs_tool.master" Title="CSTools: Mark Message As Received" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMarkMessageReceived" runat="server">
        <ContentTemplate>

            <script language="javascript" src="Javascript/Common.js"></script>

            <script language="javascript">
            var mapId = "mark_message_received.aspx";
            function Navigate(sQueryString)
            {
                try
                {
                    window.location.href = "message_details.aspx" + sQueryString;
                }
                catch(_error)
                {
                    return;
                }
            }
            function UpdateProfile()
            {
                document.getElementById(textChangedClientID).value = "true";
            }

            </script>

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%; background-color: White">
                        <input type="hidden" id="textChanged" enableviewstate="true" runat="server" name="textChanged"
                                value="false" />
                            <table height="100%" align="center" width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="DivBg" valign="top">
                                        <table width="100%" border="0" cellpadding="=0" cellspacing="0" style="height: 1px">
                                            <tr>
                                                <td class="Hd1" style="height: 19px">
                                                    <asp:Label ID="cphMainSection" runat="server" CssClass="UserCenterTitle">Mark A Message As Received</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                        <br />
                                        <table width="75%" align="center" style="margin-left: 10px;" border="0" cellpadding="=0"
                                            cellspacing="0">
                                            <tr>
                                                <td class="Hd2">
                                                    <fieldset class="fieldset" style="height: 255px">
                                                        <legend class="Hd4">Message Details</legend>
                                                        <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="4">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td width="25%">
                                                                    &nbsp;</td>
                                                                <td style="background-color: white;">
                                                                    Message Type:</td>
                                                                <td style="background-color: white;">
                                                                    <asp:Label ID="lblMessageType" runat="server" Font-Bold="True"></asp:Label></td>
                                                                <td width="15%">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td style="background-color: white;">
                                                                    On:</td>
                                                                <td style="background-color: white;">
                                                                    <asp:Label ID="lblOn" runat="server" Font-Bold="True"></asp:Label></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td style="background-color: white;">
                                                                    From:</td>
                                                                <td style="background-color: white;">
                                                                    <asp:Label ID="lblFrom" runat="server" Font-Bold="True"></asp:Label></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    To:</td>
                                                                <td>
                                                                    <asp:Label ID="lblTo" runat="server" Font-Bold="True"></asp:Label></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    Finding:</td>
                                                                <td>
                                                                    <asp:Label ID="lblFinding" runat="server" Font-Bold="True"></asp:Label></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td colspan="2">
                                                                    <asp:Label ID="Label2" runat="server"> Reason For Marking Message Received:</asp:Label><br />
                                                                    <asp:TextBox ID="txtReason" runat="server" Width="400px" TextMode="MultiLine" Rows="4" MaxLength="500"></asp:TextBox></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <asp:Button ID="btnMarkReceived" runat="server" Text="Mark Received" CssClass="Frmbutton"
                                                                        OnClick="btnMarkReceived_Click"></asp:Button>
                                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Frmbutton" OnClick="btnCancel_Click">
                                                                    </asp:Button></td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblWarning" runat="server" Width="80%" ForeColor="Red" Font-Names="Verdana"
                                                        Font-Size="Medium">WARNING :  Marking a Message as Received Will Remove All Notifications</asp:Label>
                                                    <asp:Label ID="lblError" runat="server" Width="60%" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
