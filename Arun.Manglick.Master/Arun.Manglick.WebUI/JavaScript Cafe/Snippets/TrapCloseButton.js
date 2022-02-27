//-----------------------------------------
// Trap Close/Cross/Refresh Button control
//-----------------------------------------
window.onbeforeunload=function(){CallClose();}        

function CallClose()
{
    if (event.clientY < 0) 
    {
       event.returnValue = 'Are you sure you want to leave the page?';
    }
}
//-----------------------------------------
function CallClose()
{
    if (event.clientY < 0) 
    {   
       CallSignOut();
       var res= window.confirm('Are you sure you want to leave the page?');               
       
       if(res)
       {
         CallSignOut();
       }
       else
       {
            return false;
       }
    }
}
//-----------------------------------------
  
