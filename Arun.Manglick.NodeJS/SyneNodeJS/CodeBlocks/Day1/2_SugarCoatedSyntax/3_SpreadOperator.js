function log(message, ...args) {
    console.log(message);
    console.log(args);
}

var obj = [{ a: 'a'}, {b: 'b'}];

log("Call 1", obj);
log("Call 1", ...obj);  //Spread
