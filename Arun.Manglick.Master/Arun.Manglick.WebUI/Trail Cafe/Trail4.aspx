<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Trail4.aspx.cs" Inherits="Trail4" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../JQuery/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        function toggleAlert() {
            toggleDisabled(document.getElementById("content"));
        }
        function toggleDisabled(el) {
            try {
                el.disabled = el.disabled ? false : true;
            }
            catch (E) {
            }
            if (el.childNodes && el.childNodes.length > 0) {
                for (var x = 0; x < el.childNodes.length; x++) {
                    toggleDisabled(el.childNodes[x]);
                }
            }
        }

        function DisableJquery() {
            $('div').each(function() {
                this.disabled = true;
            });
        }
        
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br>   
    <input type="checkbox" value="toggleAlert()" onclick="toggleAlert()">Click to Toggle Display </input>
    <br />
    <div id="content">
        <table>
            <tr>
                <td>
                    <input type="text" name="foo" value="America" />
                </td>
            </tr>
             <tr>
                <td>
                    <input type="text" name="foo" value="America" />
                </td>
            </tr>
            <tr>
                <td>
                    <select name="bar">
                        <option>a</option>
                        <option>b</option>
                        <option>c</option>
                    </select>
                </td>
            </tr>
            <tr>            
            <td>
            <asp:DropDownList ID="DropDownList1" Width="100" runat="server">
                        <asp:ListItem Text="AAAAAAAAAAAAAAAAAAAAAAAAA"></asp:ListItem>
                        <asp:ListItem Text="BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB"></asp:ListItem>
                        <asp:ListItem Text="CCCCCCCCCC"></asp:ListItem>
                    </asp:DropDownList>
            </td>
            </tr>
        </table>
    </div>
    
    <div>  <img src="images/image.1.jpg" id="hibiscus" alt="Hibiscus"/>  <img src="images/image.2.jpg" id="littleBear" title="A dog named Little Bear"/>  <img src="images/image.3.jpg" id="verbena" alt="Verbena"/>  <img src="images/image.4.jpg" id="cozmo" title="A puppy named Cozmo"/>  <img src="images/image.5.jpg" id="tigerLily" alt="Tiger Lily"/>  <img src="images/image.6.jpg" id="coffeePot"/></div>
    </form>
</body>
<script type="text/javascript">
    DisableJquery();

</script>


</html>
