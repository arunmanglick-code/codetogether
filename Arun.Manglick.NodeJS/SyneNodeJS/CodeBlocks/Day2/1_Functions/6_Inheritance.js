class Vehicle {
    constructor(m) {
        this.make = m;
    }
    start() {
        return this.make + " Engine Started....";
    }
}
// -----------------------------------------------
class FourWheleer extends Vehicle {
    constructor(make = "Honda", model = "City") {
        super(make);
        this.model = model;
    }

    start() {
        return super.start() + ", & Model is: " + this.model;
    }

    move() {
        return "Moving like a car.";
    }
}
// -----------------------------------------------
let fw1 = new FourWheleer();
console.log(fw1.start());

let fw2 = new FourWheleer("Ford","Mustang");
console.log(fw2.start());
// ------------------------------------------------