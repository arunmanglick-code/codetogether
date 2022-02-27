function toggleAlert() {
    toggleDisabled(document.getElementById("content"));
}

function toggleDisabled(el) {
    try {
        el.disabled = false;
    }
    catch (E) {
    }
    if (el.childNodes && el.childNodes.length > 0) {
        for (var x = 0; x < el.childNodes.length; x++) {
            toggleDisabled(el.childNodes[x]);
        }
    }
}

// Usage - 

/*
<input type="checkbox" value="toggleAlert()" onclick="toggleAlert()" />
<div id="content">
    <table>
        <tr>
            <td>
                <input type="text" name="foo" />
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
                        <asp:ListItem Text="AA"></asp:ListItem>
                        <asp:ListItem Text="BB"></asp:ListItem>
                        <asp:ListItem Text="CC"></asp:ListItem>
            </asp:DropDownList>
        </td>
        </tr>
    </table>
</div>
    

*/