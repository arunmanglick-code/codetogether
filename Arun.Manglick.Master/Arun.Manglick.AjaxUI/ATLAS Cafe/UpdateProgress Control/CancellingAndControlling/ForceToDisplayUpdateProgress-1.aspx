<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ForceToDisplayUpdateProgress-1.aspx.cs" Inherits="ForceToDisplayUpdateProgress1"
    Title="ForceToDisplayUpdateProgress-1 Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
     <script language="javascript" type="text/javascript">
        
	    var prm = Sys.WebForms.PageRequestManager.getInstance();
        
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        var postBackElement;
        function InitializeRequest(sender, args) 
        {
            if (prm.get_isInAsyncPostBack()) 
            {
                args.set_cancel(true);  // To cancel a new async postback
            }
            
            postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'Button1') 
            {
                $get('UpdateProgress1').style.display = 'block';                
            }
        }
        
        function EndRequest(sender, args) 
        {
            if (postBackElement.id == 'Button1') 
            {
                $get('UpdateProgress1').style.display = 'none';
            }
        }
        
	    function AbortPostBack() 
	    {
            if (prm.get_isInAsyncPostBack()) 
            {
              prm.abortPostBack();
            }
        }
   </script>

    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="'Forcing to Display Update Progress"></asp:Label>
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
                            <li>Forcing to display Update Progress Using Client Script</li>
                            <li>UpdateProgress control does not display automatically in below situation</li>
                            <li>When the UpdateProgress control is associated with a specific update panel using ‘AssociatedUpdatePanelID’ property, and the asynchronous postback results from a control that is not inside that update panel instead postback results from a control that is inside a Trigger</li>
                            <li>When the UpdateProgress control is not associated with any UpdatePanel control, and the asynchronous postback does not result from a control that is not inside an UpdatePanel and is not a trigger. For example, the update is performed in code</li>                            
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div class="DivClassFloat" style="width: 500px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend><b>Force to fire Update Progress</b></legend>
                                    <br /><br />Last refresh<%=DateTime.Now.ToString() %><br /><br /> 
                                </fieldset>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <br />
                        <asp:Button ID="Button1" CssClass="button" runat="server" Text="Refresh Panel using Trigger" OnClick="Button1_Click" />
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                Update in progress...
                                <img src="../../../Images/ajax-loader.gif" />
                                <input type="button" value="stop" onclick="AbortPostBack()" id="Button2" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
