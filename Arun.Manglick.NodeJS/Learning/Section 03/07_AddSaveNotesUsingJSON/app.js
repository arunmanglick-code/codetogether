console.log('Starting Getting Yargs Demo.js');

const fs = require('fs');
const _ = require('lodash');
const module_yarg = require('yargs');

const notes = require('./note.js');

const yard_argv = module_yarg.argv;
console.log(yard_argv);
var command = yard_argv._[0];
console.log('Command Yarg: ', command);
console.log('Name: ', yard_argv.FirstName, yard_argv.LastName);

if (command === 'add') {
  var res= notes.addNote(yard_argv.FirstName, yard_argv.LastName);
  //var res='Hello';
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
