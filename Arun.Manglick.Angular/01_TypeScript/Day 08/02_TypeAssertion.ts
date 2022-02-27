let mydata:any = "This is with type any and does not provide intellisense";
let l1 = mydata.length;   // No intellisense after pressing dot (.) after mydata. Thus use typeassertion as below

let l2 = (<string> mydata).length;
let l3 = (mydata as string).length;