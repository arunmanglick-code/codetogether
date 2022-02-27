var factory = require('./factory.js');

// ---------------------------------------------
// Arun Way:
var fl = factory.FLFactory();
fl.log("Checking Arun...");

var dl = factory.DBLFactory();
dl.log("Checking Arun...");
// -----------------------------------------------

// Manish Way:
// var fl = factory.FLFactory.getLogger();
// fl.log("Checking Manish...");
//
// var dl = factory.DBLFactory.getLogger();
// dl.log("Checking Manish...");
