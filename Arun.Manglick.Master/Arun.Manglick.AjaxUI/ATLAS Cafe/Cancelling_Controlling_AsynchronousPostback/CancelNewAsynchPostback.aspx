<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CancelNewAsynchPostback.aspx.cs" Inherits="CancelNewAsynchPostback"
    Title="CancelNewAsynchPostback Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack() & args.get_postBackElement().id == 'Button2') {
                args.set_cancel(true);
                ActivateAlertDiv('visible', 'Previous request is still in Progress');
            }
        }

        function EndRequest(sender, args) {
            ActivateAlertDiv('hidden', 'Previous request is still in Progress');
        }

        function ActivateAlertDiv(visString, msg) {
            //debugger;
            var adiv = $get('AlertDiv');
            var aspan = $get('AlertMessage');
            adiv.style.visibility = visString;
            aspan.innerHTML = msg;
        }

        if (typeof (Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
    </script>

    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Abort Existing Asynch Postback"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="validation-no-error">
                <asp:ValidationSummary ID="vlsStipulations" DisplayMode="List" CssClass="validation"
                    runat="server"></asp:ValidationSummary>
            </td>
        </tr>
    </table>
    <!-- Table 2 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td>
                <div id="divform">
                    <br />
                    <br />
                    <!-- Features Div -->
                    <div class="DivClassFeature" style="width: 800px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Cancelling a new Asynchronous Postback</li>
                            <li>By default, when a page makes multiple asynchronous postbacks at the same time,
                                the postback made most recently takes precedence even it cancels the first operation. But most of the time this is not
                                an ideal situation and you definietely want to cancel the new postback till the
                                time previous gets complete</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Cancelling New Asynchronous Postback</legend>
                                    <br />
                                    <br />
                                    Last refresh<%=DateTime.Now.ToString() %><br />
                                    <br />
                                    <asp:Button ID="Button1" CssClass="button" runat="server" Text="Cancelling New Refresh"
                                        OnClick="Button1_Click" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br /><br />
                       <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Cancelling New Asynchronous Postback</legend>
                                    <br />
                                    <br />
                                    Last refresh<%=DateTime.Now.ToString() %><br />
                                    <br />
                                    <asp:Button ID="Button2" CssClass="button" runat="server" Text="Cancelling New Refresh"
                                        OnClick="Button2_Click" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div id="AlertDiv" class="AlertStyle">
        <span id="AlertMessage"></span>
    </div>
</asp:Content>
