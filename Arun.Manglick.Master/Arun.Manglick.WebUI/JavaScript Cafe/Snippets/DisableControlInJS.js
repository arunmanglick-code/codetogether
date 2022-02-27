//-----------------------------------------
// Disables Button control using JS
//-----------------------------------------
function AuditHistoryDDLSelectionChange(ddlCtrl,btnCtrl)
{
    if(ddlCtrl.value == '')
    {
        btnCtrl.disabled = true;
    } 
    else
    {
        btnCtrl.disabled = false;
    }
    return false;
}
//-----------------------------------------