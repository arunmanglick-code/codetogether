function Display(callback) {
    console.log(callback(34, 56));
};

function myFn(a, b) {
    return a + b;
};

Display(myFn);
Display(function (a, b) { return a + b; });
Display((a, b) => a + b);