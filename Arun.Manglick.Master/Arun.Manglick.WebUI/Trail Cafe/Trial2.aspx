<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Trial2.aspx.cs" Inherits="Trail_Cafe_Trial2" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" Runat="Server">

    <script src="../JS/JScript.js" type="text/javascript"></script>
<script type="text/javascript">
function MyHints1(name){
    /// <summary>This method returns a greeting message</summary>
    /// <param name="name">Persons Name</param>
    /// <returns>string</returns>
    
    return 'Hello';
    
    
}

function Verify()
{
    debugger;
    return true;
}

function MessageList()
{
    var msg = 'Hello, World, Enjoy';
    var list = msg.split(",");
    var show = '';
    for(iCnt=0;iCnt < list.length;iCnt++)
    {
        show = show + list[iCnt] + '\n';
    } 
    
    alert(show);
}


</script>

<script type="text/javascript">

    function Validate()
    {
       var res = false;   
       var idx = hiddenLanguageClientIndex.value;   
       var financeTypeList = document.getElementById('<%= DropDownList1.ClientID %>'); 
       res = window.confirm("Do you want to continue");      
       if(res)
       {
            var hiddenButton=document.getElementById('<%= Button4.ClientID %>');
            hiddenButton.click();            
       }
       else
       {
          financeTypeList.selectedIndex = idx;
       }
       
       //return res;
    }

</script>
    <link href="../App_Themes/MyTheme/Style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/MyTheme/Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" Runat="Server">
<img src="../Images/logo2.bmp" />
    <asp:Label ID="Label1" runat="server" Text="Label" CssClass="label"></asp:Label><br />
    <asp:Button ID="Button2" runat="server" Text="Button2" CausesValidation="false" OnClientClick="Verify();" onclick="Button2_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <br />
    <a href="../../Arun.Manglick.AjaxUI/Home.aspx">Go</a>
    
    <asp:DropDownList ID="DropDownList1" runat="server" Height="23px" AutoPostBack="true" 
        onselectedindexchanged="DropDownList1_SelectedIndexChanged" onchange="return Validate();" Width="385px">
        <asp:ListItem>AA</asp:ListItem>
        <asp:ListItem>BB</asp:ListItem>
        <asp:ListItem>CC</asp:ListItem>
    </asp:DropDownList>
    
    <asp:Button ID="Button4" runat="server" Text="" CausesValidation="false" OnClick="Button1_Click" style="visibility:hidden;" />  // Will be hidden button
    
    <span onclick="return Verify();">
    <asp:CheckBox ID="CheckBox1" AutoPostBack="true" runat="server" oncheckedchanged="CheckBox1_CheckedChanged" />
    </span>
    <asp:HyperLink ID="HyperLink1" runat="server" onclick="alert('Hello');">HyperLink</asp:HyperLink>
    <br />
    <a href="#" onclick='MessageList();'>Go to Paging</a>
    
    
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Do it" />
    <asp:Button ID="Button3" runat="server" CausesValidation="false" Text="Do Not" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TextBox1" runat="server" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
    
    <br />
    
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Trail Cafe/Trial2.aspx" Target="_blank">HyperLink</asp:HyperLink>
    <asp:HiddenField id="HiddenLanguageClientIndex" runat="server" Value=""></asp:HiddenField>
    <script type="text/javascript">
        var hiddenLanguageClientIndex=document.getElementById('<%= HiddenLanguageClientIndex.ClientID %>');
        var financeTypeListTemp = document.getElementById('<%= DropDownList1.ClientID %>');
        if(hiddenLanguageClientIndex != null)
        {
            hiddenLanguageClientIndex.value = financeTypeListTemp.selectedIndex;
        }

    </script>
    
</asp:Content>

