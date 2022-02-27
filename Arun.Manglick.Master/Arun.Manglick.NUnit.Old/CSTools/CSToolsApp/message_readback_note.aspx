<%@ Page Language="C#" AutoEventWireup="true" CodeFile="message_readback_note.aspx.cs" Inherits="Vocada.CSTools.message_readback_note" %>

<html>
<head id="Head1" runat="server">
     <title>VeriPhy</title>
     <link href="App_Themes/csTool/cstool.css" type="text/css" rel="stylesheet">
</head>
<script language="javascript">
//Validate if the
function validateText()
{
    var rejectText = false;
    
    if(document.getElementById('txtReadbackNote').value != "")    
    {        
        rejectText = true;
    }
    else
    {
        alert('Please enter reason for rejecting Readback!');
    }
    return rejectText;    
}
function onCancel()
{
    window.returnValue = '0';
    window.close();
}
</script>
<body>
    <form id="form1" runat="server">
    <div class="DivBg">
        <table cellpadding="0" cellspacing="0" width="100%" border="0">
            <tr>
                <td>&nbsp;</td>                
            </tr>
            <tr>
                <td  align="left">&nbsp;</td>
                <td>                
                    <asp:Label ID="lblMessage" runat="server" Text="Please state a reason for rejecting this readback"></asp:Label>
                </td>                    
            </tr>
            <tr>
                <td>&nbsp;</td>                
            </tr>
            <tr>
                <td  align="left" >&nbsp;</td>
                <td width="90%">            
                    <asp:TextBox ID="txtReadbackNote" MaxLength="150" runat="server" Style="position: relative" Width="80%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>&nbsp;</td>                
            </tr>
            <tr>
            <td>&nbsp;</td>
            <td align="center">
                        
                <asp:Button ID="butSave" CssClass="Frmbutton" runat="server" Text="Save" OnClick="butSave_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="butCancel" CssClass="Frmbutton" runat="server" Text="Cancel" />
            </td>            
        </tr> 
        <tr>
                <td>&nbsp;</td>                
         </tr>
        </table>    
    </div>
    </form>
</body>
</html>
