console.log('Starting Getting Yargs Demo.js');

const fs = require('fs');
const _ = require('lodash');
const module_yarg = require('yargs');

const notes = require('./note.js');

const firstNameOptions = {
  describe: 'First Name',
  demand: true,
  alias: 'f'
};

const lastNameOptions = {
  describe: 'Last Name',
  demand: true,
  alias: 'l'
};

const yard_argv = module_yarg
  .command('list','Listing Notes')
  .command('add','Remove a Note',{
    FirstName:firstNameOptions,
    LastName:lastNameOptions
  })
  .help()
  .argv;
var command = yard_argv._[0];
// -----------------------------------------------------
if (command === 'add') {
  var note= notes.addNote(yard_argv.FirstName, yard_argv.LastName);
  if (note) {
    console.log('Note created');
    console.log('--');
    console.log(`FirstName: ${note.FirstName}`);
    console.log(`LastName: ${note.LastName}`);
  } else {
    console.log('Note title taken');
  }
} else if (command === 'list') {
  console.log('Listing all notes');
  var allNotes = notes.getAllNotes();
  console.log(`Printing ${allNotes.length} note(s).`);
  allNotes.forEach((note) => notes.logNote(note));
} else if (command === 'read') {
  console.log('Reading note');
} else if (command === 'remove') {
  console.log('Removing note');
  var noteRemoved = notes.removeNote(yard_argv.FirstName);
  var message = noteRemoved ? 'Note was removed' : 'Note not found';
  console.log(message);
} else {
  console.log('Command not recognized');
}
