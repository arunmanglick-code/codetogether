"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var _10_Module_js_1 = require("./10_Module.js");
var point1 = new _10_Module_js_1.Point1(2, 3);
console.log(point1.getDistance());
document.getElementById("hOne").innerHTML = point1.getDistance().toString();
