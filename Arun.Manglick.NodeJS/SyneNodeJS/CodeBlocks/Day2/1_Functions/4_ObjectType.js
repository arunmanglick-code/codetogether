var toy = new Object();
//toy.color = 'red';
Object.prototype.color = 'red';
console.log(toy.color);

var toy1 = new Object();
console.log(toy1.color);