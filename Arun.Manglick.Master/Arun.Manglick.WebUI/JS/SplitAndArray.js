function CheckInactiveSelection()
    {
        var msg = '<%=GetGlobalResourceObject("ErrorMessages", "AssignObjectSelect")%>';
        var hdfRowId = document.getElementById('<%= hdfRowId.ClientID %>');
        var currentGrid = document.getElementById('<%= gdvObjectAssignment.ClientID %>');
        var index;
        
        if(hdfRowId != null)
        {
            var allRows = hdfRowId.value.split(",");
            
            if(allRows[0]== '')
            {                            
                return false;  
            }
            
            for(iCnt=0;iCnt < allRows.length;iCnt++)
            {
                index = parseInt(allRows[iCnt]);
                inActive = currentGrid.rows[index].cells[3].innerText;
                if(inActive == 'Inactive')
                {
                    promptAns = window.confirm("You are assigning an Inactive program. It will not run. Do you want to continue?");                           
                    return promptAns;  
                }                
            }
        }
        
        return true;
    }