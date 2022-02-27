var EmployeeNew = (function () {
    // constructor(objEmp: { id: number, name: string }) {
    //     this.id = objEmp.id;
    //     this.name = objEmp.name
    // }
    // constructor(objEmp: {} = {}) {
    //     this.id = objEmp['id'] as number;
    //     this.name = objEmp['name'] as string;
    //   }
    function EmployeeNew(objEmp) {
        if (objEmp === void 0) { objEmp = {}; }
        this.id = objEmp['id'];
        this.name = objEmp['name'];
    }
    Object.defineProperty(EmployeeNew.prototype, "Name", {
        get: function () {
            return this.name;
        },
        // setId(id: number) {
        //     this.id = id;
        // }
        set: function (n) {
            this.name = n;
        },
        enumerable: true,
        configurable: true
    });
    return EmployeeNew;
}());
var eNew = new EmployeeNew({ id: 1, name: 'Jack' });
// eNew.Name = "John";
console.log('Name is', eNew.Name);
