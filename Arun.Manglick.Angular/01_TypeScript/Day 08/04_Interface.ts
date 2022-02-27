let area = function (rect: { h: number, w?: number }) {
    rect.w = rect.w || rect.h;
    return rect.h * rect.w;
}

var s1 = { h: 10, w: 20 };
console.log(area(s1));
// -----------------------------------------------------
interface Rectanle {
    h: number;
    w?: number;
}

let area1 = function (rect: Rectanle) {
    rect.w = rect.w || rect.h;
    return rect.h * rect.w;
}

var s2: Rectanle = {
    h: 10,
    w: 20
};
console.log(area(s2));

var s3: Rectanle = { h: 10 };
console.log(area(s3));
// -----------------------------------------------------
interface ICheck {
    address: string;
}

interface ISecond {

}

interface IPerson extends ICheck, ISecond {
    name: string;
    age: number;
    kids: number;
    calcPets: () => number;
    growOld: (years: number) => void;
    greet?: (msg: string) => string;
}
// -------------------------------------------------
// Object Level Impementation
var p1: IPerson = {
    name: "Arun",
    age: 35,
    kids: 1,
    calcPets: function () {
        return 2;
    },
    growOld: function (num: number) {
        this.age += 2;
    },
    address: "Tenessee"
}
// -------------------------------------------------
// Class Level Impementation
class CPerson implements IPerson, ICheck {
    name: string;
    age: number;
    kids: number;
    address: string;

    constructor(name?: string, age?: number, kids?: number) {
        this.name = name;
        this.age = age;
        this.kids = kids;
    }

    calcPets(): number {
        return this.kids * 2;
    }

    growOld(y: number): void {
        this.age += y;
    }

    greet(m: string): string {
        return "Hello " + m;
    }
}

var cp1: IPerson = new CPerson();
var cp2: ICheck = new CPerson(); // This will be like Interface Segregation


