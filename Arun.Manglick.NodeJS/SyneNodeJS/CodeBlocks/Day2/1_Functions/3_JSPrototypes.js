// function Person(n) {  
//     var name = n;

//     this.setName = function (nm) {
//         name = nm;
//     };

//     this.getName = function () {
//         return name;
//     };
// };
// --------------------------------------------
// function Person(n) {  
//     this.name = n;
// };

// Person.prototype.setName = function (nm) {
//     this.name = nm;
// };

// Person.prototype.getName = function () {
//     return this.name;
// };

// -------------------------------------------------
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
// -------------------------------------------------
// var da = {
//     getName: function () {
//         console.log("Get Name Called.......");
//         return this.name;
//     },
//     setName: function (name) {
//         console.log("Set Name Called......");
//         this.name = name;
//     }
// }

// function check(){
//     da.setName("Manish");
//     var data = da.getName();
//     var data1 = da.getName();
// }
// -------------------------------------------------
var p1 = new Person("Manish");
console.log(p1.getName());
p1.setName("Abhijeet");
console.log(p1.getName());
// ---------------------------------------------------
console.log(p1);
// ---------------------------------------------------
