function Show() {
    console.log("Show Method");
}
Show();
// ------------------------------------------
function add(n1, n2) {
    return n1 + n2;
}
// ------------------------------------------
var add2; // Declaring add2 as function accepting two numbers and retruning number
var add2 = function add(n1, n2) {
    return n1 + n2;
};
// ------------------------------------------
function Display(cb) {
    console.log(cb(23, 45));
}
// Below are three ways of declaring/calling function
Display(add2); // Add3 is named function
Display(function (a, b) { return a * b; }); //Anonymous Functions
Display(function (a, b) { return a * b; }); //Lambdas
// ------------------------------------------
