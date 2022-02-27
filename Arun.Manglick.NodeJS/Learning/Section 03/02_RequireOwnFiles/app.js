console.log("Starting Require Own Files Demo");
const notes = require('./note.js');
console.log(`Your age is ${notes.age}`);
var res=notes.addNote();
console.log(res);

var resAdd=notes.addNoteNumber(2,3);
console.log('Here is Add Result: ',  resAdd);
