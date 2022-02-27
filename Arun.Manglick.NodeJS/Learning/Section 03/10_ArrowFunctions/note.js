console.log('Starting Add-Remove Note Thru JSON');
const fs = require('fs');

// --------------------------------------------------------------
var addNote = (fName, lName) =>{
  var note = {
    FirstName:fName,
    LastName:lName
  };  // Note JSON Object

 var notes= fetchNotes();   // Array
 var duplicateNotes= notes.filter((note) => note.FirstName === fName);
 if (duplicateNotes.length === 0) {
    notes.push(note);
    saveNotes(notes);
  }

  return note;
};
// --------------------------------------------------------------
var fetchNotes = function()
{
  try {
    var notesString = fs.readFileSync('notes-data.json');
    return JSON.parse(notesString);
  } catch (e) {
    return [];
  }
};
// --------------------------------------------------------------
var saveNotes = (notes) => {
  console.log('Save time', notes);
  fs.writeFileSync('notes-data.json', JSON.stringify(notes));
};
// --------------------------------------------------------------
var removeNote = function(fName){
 var notes= fetchNotes();
 var filteredNotes= notes.filter((note) => note.FirstName != fName);
 saveNotes(filteredNotes);

 return notes.length !== filteredNotes.length;
};
// --------------------------------------------------------------
var getAllNotes = function(){
  return fetchNotes();
};
// --------------------------------------------------------------
var logNote = (note) => {
  console.log('--');
  console.log(`FirstName: ${note.FirstName}`);
  console.log(`LastName: ${note.LastName}`);
};
// --------------------------------------------------------------
module.exports = {
  addNote,
  fetchNotes,
  saveNotes,
  removeNote,
  getAllNotes,
  logNote
};
