class EmployeeNew {
    private id: number;   // By default it's Public (Opposite to C#)
    private name: string;

    // constructor(objEmp: { id: number, name: string }) {
    //     this.id = objEmp.id;
    //     this.name = objEmp.name
    // }

    // constructor(objEmp: {} = {}) {
    //     this.id = objEmp['id'] as number;
    //     this.name = objEmp['name'] as string;
    //   }

    constructor(objEmp: {} = {}) {
        this.id = (<any>objEmp)['id']
        this.name = (<any>objEmp)['name'];
    }

    // setId(id: number) {
    //     this.id = id;
    // }

    set Name(n: string) {
        this.name = n;
    }

    get Name() {
        return this.name;
    }
}

var eNew = new EmployeeNew(
    { id: 1, name: 'Jack' }
);
// eNew.Name = "John";
console.log('Name is', eNew.Name);

