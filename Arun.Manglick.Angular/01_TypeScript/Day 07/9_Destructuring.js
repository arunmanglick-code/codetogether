var Calculator = (function () {
    function addition(x, y) {
        return x + y;
    }
    ;
    function subtract(x, y) {
        return x - y;
    }
    ;
    function multiply(x, y) {
        return x * y;
    }
    ;
    function divide(x, y) {
        return x / y;
    }
    ;
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
var x = numArr[0], y = numArr[1];
console.log("Before: ", x, y);
_a = [y, x], x = _a[0], y = _a[1];
console.log("After: ", x, y);
var _a;
