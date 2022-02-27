var arr = [];
arr[0] = 10;
arr[1] = "Manish";

// var numericArr: number[] = [];
var numericArr: Array<number> = [];

numericArr[0] = 10;
numericArr[1] = 20;
// numericArr[1] = "ABC";

// for (var i = 0; i < numericArr.length; i++) {
//     console.log(numericArr[i]);
// }

// numericArr.forEach((v, i, arr) => {
//     console.log(numericArr[i]);
// });

for(let n of numericArr){
    console.log(n);
}