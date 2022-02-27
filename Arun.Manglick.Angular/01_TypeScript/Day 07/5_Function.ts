function Show():void{
    console.log ("Show Method");
}
Show();
// ------------------------------------------
function add(n1:number, n2:number):number{
    return n1+n2;
}
// ------------------------------------------
var add2:(n1:number, n2:number) =>number; // Declaring add2 as function accepting two numbers and retruning number
var add2 = function add(n1:number, n2:number){
    return n1+n2;
}
// ------------------------------------------
function Display(cb: (x: number, y: number) => number) {
    console.log(cb(23, 45));
}

// Below are three ways of declaring/calling function
Display(add2);      // Add3 is named function
Display(function (a, b) { return a * b; }); //Anonymous Functions
Display((a, b) => a * b);   //Lambdas
// ------------------------------------------
