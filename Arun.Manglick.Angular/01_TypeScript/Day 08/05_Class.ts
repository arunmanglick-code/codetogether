class Employee{
    private id:number;   // By default it's Public (Opposite to C#)
    private name:string;

    constructor(id:number, name:string){
        this.id = id;
        this.name = name
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

    setId(id:number)
    {
        this.id = id;
    }

    set Name(n:string){
        this.name = n;
    }

    get Name(){
        return this.name;
    }
}

var e1= new Employee(1,"arun");
e1.Name = "David";
console.log('Name is', e1.Name);

