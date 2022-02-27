function ReverseMe() {
    function Reverse(name) {
        var arr = (name.split('').reverse());
        return arr;
    }
    function ReverseArray(names) {
        var arr = names.reverse();
        return arr;
    }
    switch (Array.isArray(arguments[0])) {
        case false:
            return Reverse(arguments[0]);
        case true:
            return ReverseArray(arguments[0]);
        default:
            throw "No Method Implementation";
    }
}
console.log(ReverseMe('RAM'));
console.log(ReverseMe(['RAM', 'JOHN', 'DAVID']));
function Reverse(strOrArr) {
    return typeof strOrArr == "string"
        ? strOrArr.split("").reverse()
        : strOrArr.slice().reverse();
}
console.log(Reverse("Manish"));
console.log(Reverse(["Manish", "Ram", "Abhi", "RK"]));
// --------------------------------------------------------
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
