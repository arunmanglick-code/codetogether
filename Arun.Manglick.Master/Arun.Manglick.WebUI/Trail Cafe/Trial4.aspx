<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Trial4.aspx.cs" Inherits="Trial4" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script src="../JS/JScript.js" type="text/javascript"></script>

    <script type="text/javascript">

function HandleYes(lineType)
    {
        //debugger;
        var radioSetMonthlyPayment = null;
        var radioEstimateMonthlyPayment = null;
        var TextPercent = null;
        
        
        if(lineType == 'CreditLine')
        {
            radioSetMonthlyPayment = document.getElementById('<%=rdoSetMonthlyPayment1.ClientID %>');
            radioEstimateMonthlyPayment = document.getElementById('<%=rdoEstimateMonthlyPayment1.ClientID %>');
            TextPercent = document.getElementById('<%=txtEstimateMonthlyPayment1.ClientID %>');
        }
        
        
        if (TextPercent != null)
        {            
            TextPercent.disabled = true;
            if(radioEstimateMonthlyPayment.checked)
            {
                TextPercent.disabled = false;
            }
        }  
        
        if (radioSetMonthlyPayment != null)
        {
            radioSetMonthlyPayment.disabled = false;
            radioSetMonthlyPayment.parentNode.disabled = false;
        }
        
        if (radioEstimateMonthlyPayment != null)
        {
            radioEstimateMonthlyPayment.disabled = false;
            radioEstimateMonthlyPayment.parentNode.disabled = false;
        }        
              
    } 
    
    function HandleNo(lineType)
    {
        //debugger;
        var radioSetMonthlyPayment = null;
        var radioEstimateMonthlyPayment = null;
        var TextPercent = null;
        
        
        if(lineType == 'CreditLine')
        {
            radioSetMonthlyPayment = document.getElementById('<%=rdoSetMonthlyPayment1.ClientID %>');
            radioEstimateMonthlyPayment = document.getElementById('<%=rdoEstimateMonthlyPayment1.ClientID %>');
            TextPercent = document.getElementById('<%=txtEstimateMonthlyPayment1.ClientID %>');
        }
        
        
        
        if (radioSetMonthlyPayment != null)
        {
            radioSetMonthlyPayment.disabled = true;
        }
        
        if (radioEstimateMonthlyPayment != null)
        {
            radioEstimateMonthlyPayment.disabled = true;
        }
        
        if (TextPercent != null)
        {
            TextPercent.disabled = true;
        }        
    }  
    
    function HandleMonthly(lineType)
    {
        //debugger;
        var TextPercent = null;        
        
        if(lineType == 'CreditLine')
        {
           TextPercent = document.getElementById('<%=txtEstimateMonthlyPayment1.ClientID %>');
        }
                       
               
        if (TextPercent != null)
        {            
            TextPercent.disabled = true;           
        }        
    } 
    
    function HandleEstimate(lineType)
    {
        //debugger;
        var TextPercent = null;                  
             
        if(lineType == 'CreditLine')
        {
           TextPercent = document.getElementById('<%=txtEstimateMonthlyPayment1.ClientID %>');
        }
          
        if (TextPercent != null)
        {            
            TextPercent.disabled = false;           
        }        
    } 

function HandleChangeEvent()
{
}

    </script>

    <script type="text/javascript">
    </script>

    <link href="../App_Themes/MyTheme/Style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/MyTheme/Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <table width="100%" border="0" cellpadding="3" cellspacing="1" id="formtable">
        <tr>
            <td width="19%" nowrap="nowrap" class="labelcolumn">
                <asp:Label ID="lblIncludeCreditLine" runat="server" EnableViewState="true" Text="IncludeCreditLine"></asp:Label>
            </td>
            <td nowrap="nowrap" width="3%" class="labelcolumn">
                <asp:RadioButton ID="rdoCreditLineYes" GroupName="CreditLine" runat="server" AutoPostBack="false"
                    onclick="HandleYes('CreditLine');HandleChangeEvent();" />
            </td>
            <td width="3%" nowrap="nowrap" class="labelcolumn">
                <asp:Label ID="lblCreditLineYes" runat="server" AssociatedControlID="rdoCreditLineYes"
                    EnableViewState="true" Text="Yes"></asp:Label>
            </td>
            <td width="3%" nowrap="nowrap" class="labelcolumn">
                <asp:RadioButton ID="rdoCreditLineNo" GroupName="CreditLine" runat="server" AutoPostBack="false"
                    onclick="HandleNo('CreditLine');HandleChangeEvent();" />
            </td>
            <td width="72%" nowrap="nowrap" class="labelcolumn">
                <asp:Label ID="lblCreditLineNo" runat="server" AssociatedControlID="rdoCreditLineNo"
                    EnableViewState="true" Text="No"></asp:Label>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" class="labelcolumn">
                <asp:Label ID="lblSetMonthlyPayment1" runat="server" EnableViewState="true" Text="SetMonthlyPayment"></asp:Label>
            </td>
            <td nowrap="nowrap" class="labelcolumn">
                <asp:RadioButton ID="rdoSetMonthlyPayment1" GroupName="CreditLineDown" runat="server"
                    AutoPostBack="false" onclick="HandleMonthly('CreditLine');HandleChangeEvent();" />
            </td>
            <td colspan="3" nowrap="nowrap" class="labelcolumn">
                <asp:Label ID="lblMonthlyPayment1" AssociatedControlID="rdoSetMonthlyPayment1" runat="server"
                    EnableViewState="true" Text="MonthlyPayment"></asp:Label>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" class="labelcolumn">
                &nbsp;
            </td>
            <td nowrap="nowrap" class="labelcolumn">
                <asp:RadioButton ID="rdoEstimateMonthlyPayment1" GroupName="CreditLineDown" runat="server"
                    AutoPostBack="false" onclick="HandleEstimate('CreditLine');HandleChangeEvent();" />
            </td>
            <td colspan="3" nowrap="nowrap" class="labelcolumn">
                <asp:Label ID="lblEstimateMonthlyPayment1" runat="server" AssociatedControlID="rdoEstimateMonthlyPayment1"
                    EnableViewState="true" Text="EstimateMonthlyPayment"></asp:Label>
                <asp:TextBox ID="txtEstimateMonthlyPayment1" CssClass="inputfield" MaxLength="6"
                    runat="server"></asp:TextBox>
                <asp:Label ID="lblEstimateMonthlyPayment1Percent" AssociatedControlID="rdoEstimateMonthlyPayment1"
                    runat="server" EnableViewState="true" Text="EstimateMonthlyPaymentPercent"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Button ID="Button1" runat="server" Text="Button" CssClass="button" onclick="Button1_Click" />
    
</asp:Content>
