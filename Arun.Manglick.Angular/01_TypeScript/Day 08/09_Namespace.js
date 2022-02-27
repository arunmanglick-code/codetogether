var Synechron;
(function (Synechron) {
    var Point = (function () {
        function Point(x, y) {
            this.x = x;
            this.y = y;
        }
        Point.prototype.getDistance = function () {
            return Math.sqrt(this.x * this.x + this.y * this.y);
        };
        return Point;
    }());
    Synechron.Point = Point;
})(Synechron || (Synechron = {}));
var Usage;
(function (Usage) {
    var point1 = new Synechron.Point(2, 3);
    console.log(point1.getDistance());
})(Usage || (Usage = {}));
