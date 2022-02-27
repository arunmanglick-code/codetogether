// var items = new Set([5,6,7,8]);

// items.add('4');
// items.add(4);
// items.add(4);

// //console.log(items.size);
// console.log(items.has('5'));

// for(let item of items){
//     console.log(item);
// }
// --------------------------------------------
var items = new WeakSet();

var a = {
    a: '123'
};
var b = {
    b: '234'
};

items.add(a);
items.add(b);

// for (let item of items) {
//     console.log(item);
// }

console.log(items.has(a));
// --------------------------------------------