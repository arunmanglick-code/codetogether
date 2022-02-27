<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="GivingPrecedence.aspx.cs" Inherits="GivingPrecedence" Title="GivingPrecedence Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);        
        
        var exclusivePostBackElement = 'ExclusiveButton';
        var lastPostBackElement;

        function InitializeRequest(sender, args)
        {            
            //debugger;
            if (prm.get_isInAsyncPostBack())
            {
                 if (args.get_postBackElement().id == 'ExclusiveButton')
                    {    
                        if(lastPostBackElement == exclusivePostBackElement)
                         {
                            args.set_cancel(true);
                            ActivateAlertDiv('visible','Previous request is still in Progress');                           
                         }
                         else
                         {
                           prm.abortPostBack();
                         }                                          
                    }
                 else if (args.get_postBackElement().id == 'SharedButton')
                   {
                         if(lastPostBackElement != exclusivePostBackElement)
                         {                         
                            args.set_cancel(true);
                            ActivateAlertDiv('visible','Previous request is still in Progress');
                         }
                   }
            }
            
            lastPostBackElement=args.get_postBackElement().id;
        }
        
        function EndRequest(sender, args)
        {
           ActivateAlertDiv('hidden','Previous request is still in Progress');
        }
        
        function ActivateAlertDiv(visString,msg)
        {            
             var adiv = $get('AlertDiv');
             var aspan = $get('AlertMessage');
             adiv.style.visibility = visString;
             aspan.innerHTML = msg;
        }        
       
        if(typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
    </script>
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Disabling Automatic Triggers"></asp:Label>
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
                            <li>Disabling Automatic Triggers using 'ChildrenAsTriggers = false'</li>
                            <li>Controls in the ContentTemplate for an UpdatePanel control are configured as triggers
                                for that UpdatePanel control and causes an automatic postback</li>
                            <li>To disable the automatic triggers, set the ChildrenAsTriggers property of the UpdatePanel
                                control to false</li>
                            <li>These property settings will cause a refresh of the panel only if triggered by an
                                external control or by a call to the Update() method</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="Server">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Refreshing this UpdatePanel Takes Precedence</legend>
                                    <br />
                                    <br />
                                    Last refresh<%=DateTime.Now.ToString() %><br />
                                    <br />
                                    <asp:Button ID="ExclusiveButton" CssClass="button" runat="server" Text="Take Precedence" OnClick="Button1_Click" />&nbsp;
                                </fieldset>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        Panel1 updating...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="Server">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Refresh of this UpdatePanel can be overwritten by above Panel</legend>
                                    <br />
                                    <br />
                                    Last refresh<%=DateTime.Now.ToString() %><br />
                                    <br />
                                    <asp:Button ID="SharedButton" CssClass="button" runat="server" Text="Refresh" OnClick="Button2_Click" />&nbsp;
                                </fieldset>
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                    <ProgressTemplate>
                                        Panel1 updating...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
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
