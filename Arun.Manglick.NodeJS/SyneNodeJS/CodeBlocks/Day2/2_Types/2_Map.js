var myMap = new Map();

myMap.set("phone1", 12345678);
myMap.set("phone2", 2345678);
myMap.set("phone3", 345678);

console.log('Size:', myMap.size);
console.log('Phone2 Value:', myMap.get("phone2"));

// for(var key of myMap.keys()){
//     console.log(key);
// }
// ---------------------------------------
// for(var value of myMap.values()){
//     console.log(value);
// }
// ------------------------------------------
// for (var [key, value] of myMap.entries()) {
//     console.log(key + "=" + value);
// }
// --------------------------------------------
// myMap.forEach(function (value,key) {  
//     console.log(key + "=" + value);
// });

myMap.forEach((value, key) => {
    console.log(key + "=" + value);
});
// -------------------------------------------
