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
// --------------------------------------------------------
