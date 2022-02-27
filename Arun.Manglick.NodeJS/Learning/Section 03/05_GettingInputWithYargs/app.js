console.log('Starting Getting Yargs Demo.js');

const fs = require('fs');
const _ = require('lodash');
const module_yarg = require('yargs');

const notes = require('./note.js');
// var resAdd=notes.addNote();
// console.log(resAdd);


console.log(process.argv);
var command = process.argv[2];
console.log('Command Passed: ', command);

const yard_argv = module_yarg.argv;
console.log(yard_argv);
var commandYarg = yard_argv._[0];
console.log('Command Yarg: ', commandYarg);
console.log('Name: ', yard_argv.FirstName, yard_argv.LastName);

if (command === 'add') {
  var res= notes.addNote(yard_argv.FirstName, yard_argv.LastName);
  console.log('Your Full Name is: ', res);
} else if (command === 'list') {
  console.log('Listing all notes');
} else if (command === 'read') {
  console.log('Reading note');
} else if (command === 'remove') {
  console.log('Removing note');
} else {
  console.log('Command not recognized');
}
