//Wrong Implementation - Lambdas will Cost More Memory. 
// -----------------------------------------------------------------
// class Queue {
//     private data: any[] = [];
//     push = (d: any) => this.data.push(d);
//     pop = () => this.data.shift();
// }
// -----------------------------------------------------------------
// Correct Implementation 
// class Queue {
//     private data: any[] = [];
//     push(d: any) {
//         this.data.push(d);
//     }
//     pop() {
//         return this.data.shift();
//     }
// }
// var q1 = new Queue();
// q1.push(10); 
// console.log(q1.pop().toFixed());
// var q2 = new Queue();
// q2.push("Arun");
// console.log(q2.pop().toFixed());  // Will produce error as "toFixed" does not work for string. 
// Thus prefer to use Generics as below, as it will give intellisense
// ---------------------------------------------------
var Queue = (function () {
    function Queue() {
        this.data = [];
    }
    Queue.prototype.push = function (d) {
        this.data.push(d);
    };
    Queue.prototype.pop = function () {
        return this.data.shift();
    };
    return Queue;
}());
var q1 = new Queue();
q1.push(10);
console.log(q1.pop().toFixed());
var q2 = new Queue();
q2.push("ABC");
console.log(q2.pop().toUpperCase());
// --------------------------------------------------- 
