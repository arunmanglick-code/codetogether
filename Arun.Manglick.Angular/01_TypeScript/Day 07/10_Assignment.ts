// Define function Reverse("Ram"), should return ['m','a','r']
// Define function Reverse("Ram", "Nam"), should return ['Nam','Rama']
function ReverseMe(name: string): string[];
function ReverseMe(name: string[]): string[];

function ReverseMe(): string[] {
    function Reverse(name: string): string[] {
        var arr: string[] = (name.split('').reverse());
        return arr;
    }

    function ReverseArray(names: string[]): string[] {
        var arr: string[] = names.reverse();
        return arr;
    }

    switch (Array.isArray(arguments[0])) {
        case false:
            return Reverse(arguments[0]);
        case true:
            return ReverseArray(arguments[0]);
        default:
            throw "No Method Implementation"
    }
}

console.log(ReverseMe('RAM'));
console.log(ReverseMe(['RAM', 'JOHN', 'DAVID']));
// ---------------------------------------------------
// Manish Approach

function Reverse(str: string): string[];
function Reverse(arr: string[]): string[]

function Reverse(strOrArr: any): string[] {
    return typeof strOrArr == "string"
        ? strOrArr.split("").reverse()
        : strOrArr.slice().reverse();
}

console.log(Reverse("Manish"));
console.log(Reverse(["Manish", "Ram", "Abhi", "RK"]));
// --------------------------------------------------------
//Type Guards
var str: number | string;
str = 10;
str = "ABC";

function ReverseTG(strOrArr: string | string[]): string[] {
    return typeof strOrArr == "string"
        ? strOrArr.split("").reverse()
        : strOrArr.slice().reverse();
}

console.log(ReverseTG("Manish"));
console.log(ReverseTG(["Manish", "Ram", "Abhi", "RK"]));
