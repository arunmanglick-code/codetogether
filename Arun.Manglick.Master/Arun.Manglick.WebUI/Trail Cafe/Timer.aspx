<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Timer.aspx.cs" Inherits="Timer" Title="Timer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">

    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Page Methods"></asp:Label>
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
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Page Methods</li>
                            <li> <a href='../../HTML/Ajax_Call_using_AjaxNet.aspx.htm'>See Help Link</a></li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <asp:Button ID="btnStartTimer" runat="server" CssClass="inputfield" Text="Run Timer" OnClientClick='return StartTimer();' /><br /><br />
                        <asp:Label ID="lblTimer" runat="server" Text="" CssClass="inputfield"></asp:Label>
                        <asp:TextBox ID="txtTimer" runat="server" Text="60" Visible="false"></asp:TextBox>    
                    </div>
                </div>
            </td>
        </tr>
    </table>
    
        <script type="text/javascript">
    
    var count='<%= GetTimerCount() %>';
    var t;
    function StartTimer()
    {
        GetServerTime();
        t = window.setInterval("GetServerTime();", 1000); 
           
        return false;  // This is not required if the button is placed in UpdatePanel
    }
    
    function GetServerTime()
    {    
          CallServer(count);   //Calling this method will result in calling the serverside method RaiseCallbackEvent() with args.                    
    }

    function ClientRoutine_ReceivingServerData(arg, context)
    {
        //debugger;
        count=arg; //Get the value from server using the “arg” variable
        var list = arg.split(";");       
        var objLink = document.getElementById('<%= lblTimer.ClientID %>');
        str='Link will be active for next ' + list[0] + ' Seconds'
        objLink.innerHTML=str;
        
        if(parseInt(count) == 0)
        {
            window.clearInterval(t);
        } 
    } 
   
</script>

</asp:Content>
