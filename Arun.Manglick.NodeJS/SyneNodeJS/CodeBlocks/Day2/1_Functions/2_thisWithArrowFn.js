function Person1(a) {
    this.age = a;
    this.growOld = function () {
        console.log(this);
        console.log(this.age);
        this.age+=1;
        console.log(this.age);
    };
}
// ----------------------------------------------
function Person2(a) {
    this.age = a;
    var self = this;
    this.growOld = () => {
        console.log(self);
        console.log(self.age);
        self.age+=1;
        console.log(self.age);
    };
}
// ----------------------------------------------
function Person3(a) {
    this.age = a;
    this.growOld = () => {
        console.log(this);
        this.age+=1;
    };
}
// ----------------------------------
var p1 = new Person1(20);
p1.growOld();

var p2 = new Person2(30);
p2.growOld();

var p3 = new Person2(50);
p3.growOld();
// ----------------------------------------------

// for (var i = 0; i < 10; i++) {
//     p1.growOld();
//     console.log(p1.age);
// }

// setInterval(p1.growOld.bind(p1), 1000);
// setInterval(function () {
//     console.log(p1.age);
// }, 1000);

// setInterval(p2.growOld, 1000);
// setInterval(function () {
//     console.log(p2.age);
// }, 1000);

// setInterval(p3.growOld, 1000);
// setInterval(function () {
//     console.log(p3.age);
// }, 1000);