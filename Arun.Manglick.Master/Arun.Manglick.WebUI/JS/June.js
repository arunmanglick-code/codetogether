function Show() {
    var txt = document.getElementById('ctl00$cphBodyContent$TextBox1');
    txt.value = 'Hello';

    var lbl = document.getElementById('ctl00_cphBodyContent_Label1');
    lbl.innerHTML = 'World1';

    return false;
}