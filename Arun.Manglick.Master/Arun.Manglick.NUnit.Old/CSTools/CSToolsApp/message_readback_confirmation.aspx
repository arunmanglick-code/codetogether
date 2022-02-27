<%@ Page Language="C#" AutoEventWireup="true" CodeFile="message_readback_confirmation.aspx.cs"
    Inherits="Vocada.CSTools.message_readback_confirmation" %>

<html>
<head id="Head1" runat="server">
    <title>CSTools</title>
     <link href="App_Themes/csTool/cstool.css" type="text/css" rel="stylesheet">
</head>

<script language="javascript">
    function OpenNoteWindow()
    {
       
        var objArgument = new Object();					
        objArgument.ParentObject = window;
        var readbackID = document.getElementById('hidReadbackID').value;
        var messageID = document.getElementById('hidMessageID').value;
        var deptMsg = document.getElementById('hidDeptMsg').value;
        objArgument.URL = "message_readback_note.aspx?ReadBackID=" + readbackID + "&MessageID=" + messageID + "&IsDeptMsg=" + deptMsg;	
        winProper = 'dialogHeight:142px;dialogLeft:300px;dialogTop:300px;dialogWidth:300px;center:yes;dialogHide:no;edge:sunken;resizable:no;scroll:no;status:no;unadorned:yes;help:no;title=0';
        var result = window.showModalDialog('readback_popup.aspx',objArgument,winProper);			
        if (result == "1")
        {
            window.close();
        }
        objArgument = null;
        return result;
   }
   
   function audioPlayed()
   {
       document.getElementById('butAccept').disabled = false;
       document.getElementById('butReject').disabled = false;
   }
</script>

<body>
    <form id="form1" runat="server" target="_self">
        <div class="DivBg">
            <table id="tbMainReadback" cellpadding="2" cellspacing="2" border="0" style="width: 100%">
                <tr>
                    <td style="width: 2%">
                        &nbsp;<input type="hidden" id="hidReadbackID" runat="server" name="hidReadbackID"
                            value="" />
                            <input type="hidden" id="hidMessageID" runat="server" name="hidMessageID"
                            value="" />
                        <input type="hidden" id="hidVoiceURL" runat="server" name="hidVoiceURL" value="" />
                        <input type="hidden" id="hidDeptMsg" runat="server" name="hidDeptMsg" value="0" />
                        
                        <input type="hidden" id="hidAccRej" runat="server" name="hidAccRej" value="0" />
                    </td>
                </tr>
                
                
                <tr>
                    <td align="Center" colspan="3">
                        &nbsp;
                        <asp:Label ID="lblMessage" runat="server" Text="Do you accept this Readback?"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" id="tdAccept" colspan="3">
                        <asp:Button ID="butAccept" CssClass="Frmbutton" runat="server" Text="Accept" OnClick="butAccept_Click"
                            Width="80px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="butReject" CssClass="Frmbutton" runat="server" Text="Reject" OnClientClick="OpenNoteWindow();"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
