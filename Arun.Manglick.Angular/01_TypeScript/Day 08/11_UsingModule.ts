import { IPoint1, Point1 } from './10_Module.js';

let point1: IPoint1 = new Point1(2, 3);
console.log(point1.getDistance());

document.getElementById("hOne").innerHTML = point1.getDistance().toString();
