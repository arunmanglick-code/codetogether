let Calculator = (function () {
    function addition(x: number, y: number) {
        return x + y;
    };

    function subtract(x: number, y: number) {
        return x - y;
    };

    function multiply(x: number, y: number) {
        return x * y;
    };

    function divide(x: number, y: number) {
        return x / y;
    };

    return {
        addt: addition,
        subtract: subtract,
        multiply: multiply
    };
})();

// var addt = Calculator.addt;
// var subtract = Calculator.subtract;

//Object Destructing
// var { addt, subtract } = Calculator;
// console.log(addt, subtract);


//Array Destrcturing
var numArr = [10, 20, 30, 40, 50, 60, 70];
// var x = numArr[0];
// var y = numArr[1];
// var [x, , y] = numArr;
// console.log(x, y);

//Swap
var [x, y] = numArr;
console.log("Before: ", x, y);

[x, y] = [y, x];
console.log("After: ", x, y);