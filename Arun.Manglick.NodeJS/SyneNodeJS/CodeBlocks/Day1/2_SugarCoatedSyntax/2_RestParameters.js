// function log(message) {  
//     console.log(message);
//     console.log(arguments);
// }

//Rest Parameter
function log(message,what,...args) {  
    console.log(message);
    console.log(what);
    console.log(args);
    console.log(args[0]);
    console.log(args.length);
}

function logline(message) {  
    console.log(message);
}
log("Hello");
logline("-------------------------");
log("Hello","Reply");
logline("-------------------------");
log("Hello","Reply","Where Are You?");

// var i = 10;
// console.log(i);
// console.log(typeof(i));

// var Hello = function() {
//     console.log("Hello");
// }

// console.log(typeof(Hello));
// Hello();