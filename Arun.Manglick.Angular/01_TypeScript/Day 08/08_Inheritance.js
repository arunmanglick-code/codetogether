var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Vehicle = (function () {
    function Vehicle(_id, _make) {
        if (_make === void 0) { _make = "Honda"; }
        this._id = _id;
        this._make = _make;
    }
    return Vehicle;
}());
var ABC = (function () {
    function ABC() {
    }
    return ABC;
}());
var FourWheeler = (function (_super) {
    __extends(FourWheeler, _super);
    function FourWheeler(id, make, model) {
        var _this = _super.call(this, id) || this;
        _this._make = make;
        _this.model = model;
        return _this;
    }
    FourWheeler.prototype.move = function () {
        console.log("Moving like a car....");
    };
    return FourWheeler;
}(Vehicle));
var v1 = new FourWheeler(1, "Ford", "Mustang");
v1.move();
