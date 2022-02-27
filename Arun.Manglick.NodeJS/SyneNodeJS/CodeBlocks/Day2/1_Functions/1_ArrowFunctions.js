// let multiplySimple = function (x, y) {
//     return x * y;
// };

// //console.log('Simple ' + multiplySimple(7, 5));

// let multiply = (x, y) => x * y;

// //console.log('Arrow ' + multiply(7, 5));

// function Display(callback) {
//     console.log(callback(34, 56));
// };

// function myFn(a, b) {
//     return a + b;
// };

// Display(myFn);
// Display(function (a, b) { return a + b; });
// Display((a, b) => a + b);

// -------------------------------

function MyDisplayCallback(mycallback){
    console.log(mycallback(22,23));
}

function myfunc(a,b){
    return a+b;
}

MyDisplayCallback(myfunc);
MyDisplayCallback(function(x,y) {return x*y});