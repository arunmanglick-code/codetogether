// Accessing the GridView's - Row, Cell and Control within Cell

function SetRowColorOnLoad(currentGrid,hiddenRowId)
    {
        var allRows = hiddenRowId.value.split(",");
        var indx;
        var currGridView = document.getElementById(currentGrid);
        
        if(currGridView != null)
        {
            for(iCnt=0;iCnt<currGridView.rows.length;iCnt++)
            {
                currGridView.rows[iCnt].style.backgroundColor='';
            }
            
            for(iCnt=0;iCnt<allRows.length;iCnt++)
            {
                indx = allRows[iCnt];
                if(indx != "" && indx != "NaN")
                {
                    currGridView.rows[indx].style.backgroundColor='#CAD2D4';
                    currGridView.rows[indx].scrollIntoView();
                    currGridView.rows[indx].cells[1].firstChild.focus();                    
                }
            }
        }        
    }