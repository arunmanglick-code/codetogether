function Person(n) {  
    var name = n;

    this.setName = function (nm) {
        name = nm;
    };

    this.getName = function () {
        return name;
    };
};
// ------------------------------------------------
function Person(n) {  
    this.name = n;
};

Person.prototype.setName = function (nm) {
    this.name = nm;
};

Person.prototype.getName = function () {
    return this.name;
};
// ------------------------------------------------
class Person{
    constructor(n){
        this.name = n;
    }

    setName(nm){
        this.name = nm;
    };

    getName(){
        return this.name;
    };
}
// ------------------------------------------------
var p1 = new Person("Manish");
console.log(p1.getName());
p1.setName("Abhijeet");
console.log(p1.getName());

var p2 = new Person("RK");
console.log(p2.getName());
p2.setName("Sumit");
console.log(p2.getName());