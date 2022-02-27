abstract class Vehicle {
    constructor(private _id: number, protected _make = "Honda") { }

    abstract move(): void;
}

class ABC{}

class FourWheeler extends Vehicle {
    model: string;

    constructor(id: number, make: string, model: string) {
        super(id);
        this._make = make;
        this.model = model;
    }

    move(){
        console.log("Moving like a car....");
    }
}

var v1 = new FourWheeler(1, "Ford", "Mustang");
v1.move();