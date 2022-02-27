<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Trail5.aspx.cs" Inherits="Trail4" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var spacer = null;
        var curObj = null;

        function openIt(obj) {
            if (spacer) {
                return;
            }

            spacer = document.createElement("span");
            spacer.style.width = obj.offsetWidth;
            spacer.style.height = obj.offsetHeight;
            spacer.style.display = "none";
            //obj.parentNode.insertBefore(spacer, obj);
            //obj.parentNode.insertAfter(spacer, obj);

            obj.style.left = getAbsPos(obj, "Left");
            obj.style.top = getAbsPos(obj, "Top");
            obj.style.position = "absolute";
            obj.style.width = obj.scrollWidth;
            obj.focus();
            spacer.style.display = "inline";
            curObj = obj;

            var d2 = document.getElementById("test");
            d2.appendChild(spacer);

        }

        function closeIt() {
            if (spacer) {
                spacer.parentNode.removeChild(spacer);
                spacer = null;
            }
            if (curObj) {
                curObj.style.width = "100px";
                curObj.style.position = "static";
            }
        }

        function getAbsPos(o, p) {
            var i = 0;
            while (o != null) {
                i += o["offset" + p];
                o = o.offsetParent;
            }
            return i;
        }
    </script>
    </head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br>
   
    <div>
        <table>          
            <tr>
                <td>
                    <br />
                    <asp:DropDownList ID="DropDownList5" Style="width: 100px" onchange="javascript:openIt(this);" runat="server">
                        <asp:ListItem Text="Build is deployed on QA server"></asp:ListItem>
                        <asp:ListItem Text="Can you please test the same and update status for respective defect in PT "></asp:ListItem>
                        <asp:ListItem Text="CCCCCCCCCC"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <br />
                    <asp:DropDownList ID="DropDownList6" Width="100"  runat="server">
                        <asp:ListItem Text="AAAAAAAAAAAAAAAAAAAAAAAAA"></asp:ListItem>
                        <asp:ListItem Text="BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB"></asp:ListItem>
                        <asp:ListItem Text="CCCCCCCCCC"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <br />
    </div>
     <div id="test">
    </div>
    </form>
</body>
</html>
