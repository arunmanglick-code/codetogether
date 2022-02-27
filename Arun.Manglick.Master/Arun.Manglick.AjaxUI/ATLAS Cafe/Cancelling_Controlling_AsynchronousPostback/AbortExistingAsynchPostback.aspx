<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AbortExistingAsynchPostback.aspx.cs" Inherits="AbortExistingAsynchPostback"
    Title="AbortExistingAsynchPostback Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        
        function AbortPostBack()
        {
            if (prm.get_isInAsyncPostBack())
            {
               prm.abortPostBack();
            }
        }
        if(typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
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
                            <li>Stop an existing Asynchronous Postback</li>                            
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Abort Existing Asynchronous Postback</legend>
                                    <br />
                                    <br />
                                    Last refresh<%=DateTime.Now.ToString() %><br />
                                    <br />
                                    <asp:Button ID="Button1" CssClass="button" runat="server" Text="Abort Refreshing Panel" OnClick="Button1_Click" />
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                Update in progress...
                                <img src="../../Images/ajax-loader.gif" />
                                <input type="button" value="stop" onclick="AbortPostBack()" id="Button2" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
