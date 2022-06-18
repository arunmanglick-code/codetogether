import {MyPerson} from '../1_Functions/3_JSPrototypes1.js';

// const MyPerson = require('../1_Functions/3_JSPrototypes1.js');

var p1 = new MyPerson("Manish");
console.log(p1.getName());
p1.setName("Abhijeet");
console.log(p1.getName());
// ---------------------------------------------------
console.log(p1);
// ---------------------------------------------------
