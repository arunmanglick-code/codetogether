var x = 1;
console.log("Before: " + x); // Output:1
for (var x_1 = 0; x_1 < 5; x_1++) {
}
;
console.log("After: " + x); // Output:5
// ------------------------------------------
// var x = 1; // let will not allowed due conflict with scoped variable in for loop
// console.log("Before: " + x);  // Output:1
// for (let x = 0; x < 5; x++) {    
// };
// console.log("After: " + x); // Output:1
// ------------------------------------------
