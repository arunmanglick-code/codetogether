"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Point1 = (function () {
    function Point1(x, y) {
        this.x = x;
        this.y = y;
    }
    Point1.prototype.getDistance = function () {
        return Math.sqrt(this.x * this.x + this.y * this.y);
    };
    return Point1;
}());
exports.Point1 = Point1;
