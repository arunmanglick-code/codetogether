function calculatorService() {
    var add = function (x, y) {
        console.log(x + y);
    };

    var subtract = function (x, y) {
        console.log(x - y);
    };

    var multiply = function (x, y) {
        console.log(x * y);
    };

    var divide = function (x, y) {
        console.log(x / y);
    };

    return {  add, subtract, multiply, divide };
};

// var c = calculatorService().add;
// console.log(c);

// var {add} = calculatorService();
// console.log(add);
// add(2,3);

var {add,subtract} = calculatorService();
console.log(add);
add(2,3);
console.log(subtract);
subtract(2,3);  
// --------------------------------------
// var arr = [1,2,3,4,5];

// //var x = arr[1];
// // var [,x] = arr;
// var [x,y] = arr;
// console.log(x);
// console.log(y);