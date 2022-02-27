
// ---------------------------------------------------
// Script to show Multiline Message
// ---------------------------------------------------
function CheckMasterDirtyFlagBeforeRedirect()
{
    var msg = "One, Two, Three";
    var list = msg.split(",");
    var actualMsg = '';
    
    for(iCnt=0;iCnt < list.length;iCnt++)
    {
        actualMsg = actualMsg + list[iCnt] + '\n';
    } 

    if(confirm('There are unsaved changes on: \n \n' + actualMsg + '\n Are you sure you want to lose all these changes?'))
    {
        return true;
    }
    else
    {
        return false;
    }
}