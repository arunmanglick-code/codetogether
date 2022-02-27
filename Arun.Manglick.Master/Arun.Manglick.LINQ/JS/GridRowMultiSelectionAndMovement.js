function SetSelectedObjRow(rowId,hiddenCtrlId,hiddenLastSelectedRow,hiddenCurrentGrid,
        grid,currentGrid)
    {
        //debugger;
        var lastKeyClick;
        var hiddenRowId;
        var hiddenLastRowId;
        var hiddenCurrentGrid;
        hiddenRowId = document.getElementById(hiddenCtrlId);
        hiddenLastRowId = document.getElementById(hiddenLastSelectedRow);
        var lastIndex = parseInt(hiddenLastRowId.value);
        var index = parseInt(rowId);
        var temp;
        var iCnt;
        
        RemoveRowColor(grid,hiddenCurrentGrid,currentGrid,hiddenCtrlId);
        
        //"0" is for SHIFT key
        if(keyPres == "0")
        {
            if(lastIndex < index)
            {
                temp = index;
                index = lastIndex;
                lastIndex = temp;
            }
            
            hiddenRowId.value = '';
                        
            for(iCnt=index;iCnt<=lastIndex;iCnt++)
            {
                if(hiddenRowId.value != '')
                    hiddenRowId.value = hiddenRowId.value + ',' + iCnt;
                else
                    hiddenRowId.value = iCnt;
            }
        }
        else if(keyPres == "1") //"1" is for CTRL key
        {
            var allRows = hiddenRowId.value.split(",");
            hiddenRowId.value = '';
            var flg = false;
            for(iCnt=0;iCnt<allRows.length;iCnt++)
            {
                indx = allRows[iCnt];
                if(indx != "")
                {
                    if(indx != index)
                    {
                        if(hiddenRowId.value != '')
                            hiddenRowId.value = hiddenRowId.value + ',' + indx;
                        else
                            hiddenRowId.value = indx;
                    }
                    else
                    {
                        flg = true;
                    }
                }
            }
            
            if(!flg)
            {
                if(hiddenRowId.value != '')
                    hiddenRowId.value = hiddenRowId.value + ',' + index;
                else
                    hiddenRowId.value = index;
            }
        }   
        else
        {
            hiddenRowId.value = index;
        } 
        hiddenLastRowId.value = rowId;
        SetRowColor(currentGrid,hiddenRowId);
        keyPres = "";
    }
    
    //Method to Set selected row color.
    function SetRowColor(currentGrid,hiddenRowId)
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
                if(indx != "")
                    currGridView.rows[indx].style.backgroundColor='#CAD2D4';
            }
        }        
    }
    
    //Method to remove grid color.
    //This is grid that is currently not selected.
    function RemoveRowColor(grid,hiddenCurrentGrid,currentGrid,hiddenCtrlId)
    {
        var hiddenRowId;
        var hiddenCurrentGrid;
        hiddenCurrentGrid = document.getElementById(hiddenCurrentGrid);
        hiddenRowId = document.getElementById(hiddenCtrlId);
        
        if(hiddenCurrentGrid.value != '')
        {
            if(hiddenCurrentGrid.value != currentGrid)
            {
                hiddenRowId.value = '';
                hiddenCurrentGrid.value = currentGrid;
            }
        }
        else
        {
            hiddenCurrentGrid.value = currentGrid;
        }
        
        hiddenCurrentGrid = document.getElementById(grid);
        if(hiddenCurrentGrid != null)
        {
            for(iCnt=0;iCnt<hiddenCurrentGrid.rows.length;iCnt++)
            {
                hiddenCurrentGrid.rows[iCnt].style.backgroundColor='';
            }
        }        
    }