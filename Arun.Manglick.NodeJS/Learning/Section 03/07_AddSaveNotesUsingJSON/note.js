console.log('Starting Add-Remove Note Thru JSON');

const fs = require('fs');
module.exports.addNote = function(fName, lName){
  var notes= [];   // Arrary
  var note = {
    FirstName:fName,
    LastName:lName
  };  // Note JSON Object

  try {
    var noteString = fs.readFileSync('notes_data.json');
    notes= JSON.parse(noteString);  // Parse String into JSON Object Array
  } catch (e) {
      console.log('Error');
  } finally {
  }

  var duplicateNotes= notes.filter((note) => note.FirstName === fName);

 if (duplicateNotes.length === 0) {
    notes.push(note);
    var noteString=JSON.stringify(notes);
    fs.writeFileSync('notes_data.json', noteString);
  }
};
