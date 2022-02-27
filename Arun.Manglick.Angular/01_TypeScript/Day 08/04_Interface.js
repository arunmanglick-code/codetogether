var area = function (rect) {
    rect.w = rect.w || rect.h;
    return rect.h * rect.w;
};
var s1 = { h: 10, w: 20 };
console.log(area(s1));
var area1 = function (rect) {
    rect.w = rect.w || rect.h;
    return rect.h * rect.w;
};
var s2 = {
    h: 10,
    w: 20
};
console.log(area(s2));
var s3 = { h: 10 };
console.log(area(s3));
// -------------------------------------------------
// Object Level Impementation
var p1 = {
    name: "Arun",
    age: 35,
    kids: 1,
    calcPets: function () {
        return 2;
    },
    growOld: function (num) {
        this.age += 2;
    },
    address: "Tenessee"
};
// -------------------------------------------------
// Class Level Impementation
var CPerson = (function () {
    function CPerson(name, age, kids) {
        this.name = name;
        this.age = age;
        this.kids = kids;
    }
    CPerson.prototype.calcPets = function () {
        return this.kids * 2;
    };
    CPerson.prototype.growOld = function (y) {
        this.age += y;
    };
    CPerson.prototype.greet = function (m) {
        return "Hello " + m;
    };
    return CPerson;
}());
var cp1 = new CPerson();
var cp2 = new CPerson(); // This will be like Interface Segregation
