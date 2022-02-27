//Type Guards
var str;
str = 10;
str = "ABC";
function ReverseTG(strOrArr) {
    return typeof strOrArr == "string"
        ? strOrArr.split("").reverse()
        : strOrArr.slice().reverse();
}
console.log(ReverseTG("Manish"));
console.log(ReverseTG(["Manish", "Ram", "Abhi", "RK"]));
// --------------------------------------------------------
