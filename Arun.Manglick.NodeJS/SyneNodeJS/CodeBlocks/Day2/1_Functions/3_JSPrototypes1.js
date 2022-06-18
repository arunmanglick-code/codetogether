// module.export = class MyPerson{
//     constructor(n){
//         this.name = n;
//     }

//     setName(nm){
//         this.name = nm;
//     };

//     getName(){
//         return this.name;
//     };
// }

// export default MyPerson;

//Ref: https://stackoverflow.com/questions/39005332/include-es6-class-from-external-file-in-node-js
//---------------------------------------------------------------

// The above approach does not work and require to bablify your code (ES6 to ES5). This is pretty long process.
// Instead to make your code run as is in ES6, export classes as below.
// Then add a package.json and add "type": "module" in first line
// Then impport file with extension: import {MyPerson} from '../1_Functions/3_JSPrototypes1.js';
// Ref: https://bobbyhadz.com/blog/javascript-uncaught-syntaxerror-unexpected-token-export

export class MyPerson{
    constructor(n){
        this.name = n;
    }

    setName(nm){
        this.name = nm;
    };

    getName(){
        return this.name;
    };
}