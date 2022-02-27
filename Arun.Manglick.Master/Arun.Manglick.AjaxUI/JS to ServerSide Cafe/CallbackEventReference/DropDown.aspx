<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DropDown.aspx.cs" Inherits="DropDown" Title="DropDown" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script type="text/javascript">
    
    var stateName;
    var t;
    function ChooseStateName()
    {
        var dropdownIndex = document.getElementById('<%= StateDropDown.ClientID %>').selectedIndex;
        stateName = document.getElementById('<%= StateDropDown.ClientID %>').options[dropdownIndex].value;
        GetCities(stateName);   
    }
   
    function ClientRoutine_WhoReceives_ResultsReturnedByServer(arg, context)
    {        
        var obj=document.getElementById('<%= CityDropDown.ClientID %>');
     
        var rows = arg.split('|'); 
        for (var i = 0; i < rows.length; ++i)
        {
         var option = document.createElement("OPTION");
         option.value = rows[i];
         option.innerHTML = rows[i];     
         obj.appendChild(option);
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
                        <asp:DropDownList ID="StateDropDown" runat="server" onchange="ChooseStateName();">
                            <asp:ListItem Text="Maharashtra" Value="Maharashtra"></asp:ListItem>
                            <asp:ListItem Text="Rajasthan" Value="Rajasthan"></asp:ListItem>
                            <asp:ListItem Text="Gujrat" Value="Gujrat"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="CityDropDown" AutoPostBack="true" Style="visibility: visible"
                            runat="Server">
                            <asp:ListItem Text="Child Item" />
                        </asp:DropDownList>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
