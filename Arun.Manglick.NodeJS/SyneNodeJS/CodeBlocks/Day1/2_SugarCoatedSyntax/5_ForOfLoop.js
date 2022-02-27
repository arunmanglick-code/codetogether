// function log(message, ...args) {
//     console.log(message);
//     for (var i = 0; i < args.length; i++) {
//         console.log(args[i]);
//     }
// };

function log(message, ...args) {
    console.log('Message:', message);
    for (i of args) {
        console.log('Args:', i);
    }
};

log("Hello", "Reply", "Where Are You?", 100, 1000);