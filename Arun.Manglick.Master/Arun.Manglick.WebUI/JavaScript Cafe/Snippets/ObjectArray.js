// -----------------------------------------------------------------
// This is to demonstrate that the Object Arrary can be created.
// -----------------------------------------------------------------
function MakeChangeThruArray()
{
    obj1=document.getElementById('<%= lblChange.ClientID %>');     
    obj2=document.getElementById('<%= ddlChange.ClientID %>');        
    obj3=document.getElementById('<%= btnChange.ClientID %>');
    obj4=document.getElementById('<%= txtChange1.ClientID %>');
    obj5=document.getElementById('<%= txtChange2.ClientID %>');
    obj6=document.getElementById('<%= txtChangePassword.ClientID %>');
    obj7=document.getElementById('<%= hdnField.ClientID %>');
    
    var mycars = new Array();
    mycars[0] = obj1;
    mycars[1] = obj2;
    mycars[2] = obj3;
    mycars[3] = obj4; 
    mycars[4] = obj5;  
    mycars[5] = obj5;
    mycars[6] = obj6;
    mycars[7] = obj7; 
    
    mycars[0].innerHTML ='Changed';
    mycars[1].selectedIndex = 1;
    
    for(i=2;i< mycars.length;i++)
    {
      mycars[i].value = 'Changed';
    }    
}

function SimpleArray()
{
    var mycars = new Array();
    mycars[0] = "Saab";
    mycars[1] = "Volvo";
    mycars[2] = "BMW";

    for (i=0;i<mycars.length;i++)
    {
    document.write(mycars[i] + "<br />");
    }
}