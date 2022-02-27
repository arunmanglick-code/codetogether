<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Timer.aspx.cs" Inherits="Timer" Title="Timer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

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
          CallServer(count);   
          
          // Note this function is not present at Client Side at design time.
          // In actual this method is registered in Client side at run time, using code at server side - GetCallbackEventReference()
          // Once registered the Client code will look like below.
          // After this - When this is called at Client side, in actual it makes a call (CallBack) to Server Side function 'RaiseCallbackEvent(..)'
          
          // ----------------------------------------------------
          // function CallServer(arg) 
          // {
          //   WebForm_DoCallback('__Page',arg,receiveServerData,"",null,false); 
          // }
          // ----------------------------------------------------
    }

    function ClientRoutine_WhoReceives_ResultsReturnedByServer(arg, context)
    {
        count=arg; //Get the value from server using the “arg” variable
        var list = arg.split(";");       
        var objLink = document.getElementById('<%= lblTimer.ClientID %>');
        str='Timer will run for next ' + list[0] + ' Seconds'
        objLink.innerHTML=str;
        
        if(parseInt(count) == 0)
        {
            window.clearInterval(t);
        } 
    } 
   
    </script>

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
                            <li><a href='../../HTML/Ajax_Call_using_AjaxNet.aspx.htm'>See Help Link</a></li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <asp:Button ID="btnStartTimer" runat="server" CssClass="inputfield" Text="Run Timer"
                            OnClientClick='return StartTimer();' /><br />
                        <br />
                        <asp:Label ID="lblTimer" runat="server" Text="" CssClass="inputfield"></asp:Label>
                        <asp:TextBox ID="txtTimer" runat="server" Text="60" Visible="false"></asp:TextBox>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
