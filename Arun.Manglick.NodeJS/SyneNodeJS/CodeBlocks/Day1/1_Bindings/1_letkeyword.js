// let x = 10;
// let x = "ABC"; // Not Allowed
// console.log(x);

var x = 10;
var x = "ABC"; // Allowed
console.log(x); // ABC

// var and let are both used for variable declaration in javascript but the difference between them is that 
// var is function scoped and let is block scoped. 
// It can be said that a variable declared with var is defined throughout the program as compared to let.

// var x = 1;
// console.log("Before: " + x);

// for (var x = 0; x < 3; x++) {
//     console.log(x);
// };

// console.log("After: " + x); // 3

var x = 1;
console.log("Before: " + x);

for (let x = 0; x < 4; x++) {
    console.log(x);
};

console.log("After: " + x);  // 1

// function f(inp) {
//     var x = 20;
//     if(inp)
//     {
//         var x = 200;
//     }
//     return x;
// };

// function f(inp) {
//     let x = 20;
//     if(inp)
//     {
//         let x = 200;
//     }
//     return x;
// };

// console.log(f(false));
// console.log(f(true));

// for (var i = 0; i < 10; i++) {
//     setTimeout(function () {
//         console.log(i);
//     },100*i);
// }

// for (var i = 0; i < 10; i++) {
//     (function(a){
//         setTimeout(function () {
//             console.log(a);
//         },100*a);
//     })(i);
// }

// for (let i = 0; i < 10; i++) {
//     setTimeout(function () {
//         console.log(i);
//     },100*i);
// }