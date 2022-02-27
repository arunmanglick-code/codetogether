console.log('Starting Note file');

module.exports.age= 12;
module.exports.addNote = function(fName, lName){
  var res =  fName  + ' ' + lName;
  return res;
}
