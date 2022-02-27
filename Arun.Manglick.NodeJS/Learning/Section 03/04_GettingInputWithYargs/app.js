console.log('Starting Getting Input Demo.js');

const fs = require('fs');
const _ = require('lodash');

const notes = require('./note.js');
var resAdd=notes.addNote();
console.log(resAdd);

// var command = process.argv[2];
// console.log('Command: ', command);
console.log(process.argv);
var command = process.argv[2];
console.log("Command Passed: ", command);

if (command === 'add') {
  console.log('Adding new note');
} else if (command === 'list') {
  console.log('Listing all notes');
} else if (command === 'read') {
  console.log('Reading note');
} else if (command === 'remove') {
  console.log('Removing note');
} else {
  console.log('Command not recognized');
}
