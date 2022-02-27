// require("./check.js");
// check();

var msg = require("./message.js");
console.log(msg);
// console.log(msg.fname);
// console.log(msg.lname);

// msg.log("Hi from App");

// var emp = new msg.Employee("Emp102");
// console.log(emp.id);
// emp.setId("Employee101");
// console.log(emp.id);

// var lgr = require("./logger/myLogger.js");
// var lgr = require("./logger/index.js");
// var lgr = require("./logger/index");

// var lgr = require("./logger1");
// lgr.log("Hi There!");

// var loggerService = require('./loggerService');
// loggerService.log("Hi There!");

var factory = require('./loggerFactory');

var fl = factory.FLFactory.getLogger();
fl.log("Checking...");

var dl = factory.DBLFactory.getLogger();
dl.log("Checking...");
