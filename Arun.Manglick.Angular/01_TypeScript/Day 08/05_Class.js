var Employee = (function () {
    function Employee(id, name) {
        this.id = id;
        this.name = name;
    }
    // This will allow to instaniate object with zero or one or two parameters
    // var e1= new Employee(); || new Employee(1); || new Employee(1,"David");
    // constructor(id = 0, name = "Arun"){
    //     this.id = id;
    //     this.name = name
    // }
    // This will allow a direct class attributes defintion, and comment above lines defining, id & name attributes
    // constructor(public id = 0, private name = "Arun"){
    //     this.id = id;
    //     this.name = name
    // }
    Employee.prototype.setId = function (id) {
        this.id = id;
    };
    Object.defineProperty(Employee.prototype, "Name", {
        get: function () {
            return this.name;
        },
        set: function (n) {
            this.name = n;
        },
        enumerable: true,
        configurable: true
    });
    return Employee;
}());
var e1 = new Employee(1, "arun");
e1.Name = "David";
console.log('Name is', e1.Name);
