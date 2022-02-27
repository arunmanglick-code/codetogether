// const EventEmitter = require('events').EventEmitter;
// const eventEmitter = new EventEmitter();

// eventEmitter.on('change',function (data) { 
//     console.log("Listener 1: " + data);
// });

// eventEmitter.emit('change', getData());

// function getData() {
//     const stringsArr = ['NodeJs', 'AngularJs', 'JavaScript', 'ReactJs'];
//     return stringsArr[Math.floor(Math.random() * stringsArr.length)];
// };
// ------------------------------------------------------------------
var stringEmitter = require('./stringEmitter');

const stEmitter = new stringEmitter();
stEmitter.on('data', (s) => {
    console.log(s);
});
// ------------------------------------------------------------------
// function Person(params) {
//     this.age = 10;
//     var self = this;
//     this.change = function (params) {
//         //console.log(this);
//         self.age += 1;
//     }
// };

// var p = new Person();
// setInterval(p.change, 1000);
// setInterval(function () {
//     console.log(p.age);
// }, 1000);

// function Hello () {  
//     console.log("Hello World!");
// };

// Hello();

//IIFE - Imediatly Invoked Function Expression
// (function () {  
//     console.log("Hello World!");
// })();

// ------------------------------------------------------------------