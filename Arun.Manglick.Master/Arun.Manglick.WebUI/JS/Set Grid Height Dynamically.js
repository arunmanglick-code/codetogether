function setHeightOfTwoGrid(controlID1, controlID2, div1, div2, adjustmentValue, minLength, maxLength) {

    var availableHeight = 0;
    var equalHeight = 0;
    var finalHeight = 0;
    var height1 = 0;
    var height2 = 0;
    var controlHeight1 = 0;
    var controlHeight2 = 0;
    var diff1 = 0;

    var diff2 = 0;
    var heightWhenNoRecordFound = 30;
    var heightToAdjustNoRecordFound = 20;
    var bodyOffset = document.body.offsetHeight;
    var parentTop1 = getPageOffsetTop(document.getElementById(controlID1));
    availableHeight = bodyOffset - parentTop1;
    equalHeight = availableHeight / 2;
    finalHeight = equalHeight - adjustmentValue;

    if (finalHeight < minLength) {
        height1 = minLength;
        height2 = minLength;
    }
    else {
        height1 = finalHeight;
        height2 = finalHeight;

        // ------------------------------------------------------------------

        // Below will adjust the height mutually.

        // ------------------------------------------------------------------

        controlHeight1 = document.getElementById(controlID1).offsetHeight;
        controlHeight2 = document.getElementById(controlID2).offsetHeight;

        if (controlHeight1 < heightWhenNoRecordFound) {
            controlHeight1 = minLength + heightToAdjustNoRecordFound;
        }

        if (controlHeight2 < heightWhenNoRecordFound) {
            controlHeight2 = minLength + heightToAdjustNoRecordFound;
        }

        if (controlHeight1 < height1) {
            diff1 = height1 - (controlHeight1 + heightToAdjustNoRecordFound);
            height1 = controlHeight1 + heightToAdjustNoRecordFound;

            if (controlHeight2 > height2) {
                height2 = height2 + diff1;
            }
        }

        if (controlHeight2 < height2) {
            diff2 = height2 - (controlHeight2 + heightToAdjustNoRecordFound);
            height2 = controlHeight2 + heightToAdjustNoRecordFound;

            if (controlHeight1 > height1) {
                height1 = height1 + diff2;
            }
        }

        // ------------------------------------------------------------------
    }

    document.getElementById(div1).style.height = height1;
    document.getElementById(div2).style.height = height2;

}




function getPageOffsetTop(element) {

    if (element != null) {
        var ot = element.offsetTop;
        while ((element = element.offsetParent) != null) { ot += element.offsetTop; }
        return ot;
    }
}



// Below is the general function to get the OffSetTop, OffSetLeft or anything..




function getAbsPos(o, p) {
    var i = 0;
    while (o != null) {
        i += o["offset" + p];
        o = o.offsetParent;
    }

    return i;

}

//Call – 

//obj.style.left = getAbsPos(obj, "Left");
//obj.style.top = getAbsPos(obj, "Top");

 
