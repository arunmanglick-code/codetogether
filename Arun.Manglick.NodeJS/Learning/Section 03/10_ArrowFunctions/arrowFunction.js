var square = (x) => {
  return x*x;
}
console.log(square(9));
//--------------------------------------
var squareSimple = (x) => x*x;
console.log(squareSimple(10));

var user = {
  name:'Arun',
  sayHi: () =>{
    console.log(arguments);
    console.log(`Hi. I'm ${this.name}`); // This does not work, thus below sayHiAlt is defined.
  },
  sayHiAlt () {
    console.log(arguments);
    console.log(`Hi. I'm ${this.name}`);
  }
}

user.sayHi();  // This will not work
user.sayHiAlt();
