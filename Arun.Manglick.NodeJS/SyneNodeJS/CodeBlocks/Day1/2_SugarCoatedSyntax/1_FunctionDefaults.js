// function add(x,y) {  
//     x = x || 0;
//     y = y || 0;
//     return x+y;
// };

function add(x = 0, y = 0) {
    return x + y;
};

console.log(add(2, 3));
console.log(add(2));
console.log(add());