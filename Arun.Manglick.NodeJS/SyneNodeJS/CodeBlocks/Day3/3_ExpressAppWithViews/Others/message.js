module.exports.fname = "Manish";
module.exports.lname = "Sharma";

module.exports.log = function (msg) {
    console.log(msg);
};

// function Employee () {
//     this.id;
// };
// Employee.prototype.setId = function (i) {
//     this.id = i;
// };

// module.exports.Employee = Employee;

class Employee{
    constructor(i){
        this.id=i;
    }

    setId(i){
        this.id=i;
    }
}

module.exports.Employee = Employee;
