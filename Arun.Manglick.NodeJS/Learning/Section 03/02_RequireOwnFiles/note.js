console.log('Starting Note file');
// const os = require('os');
// var user = os.userInfo();
// console.log(user);
// console.log('Hello ' + user.username + ' !!!');

//console.log(module);
module.exports.age= 12;
module.exports.addNote = function(){
  console.log("Add Function Called");
  return "Result:TBD"
}

module.exports.addNoteNumber = (x, y) => {
  console.log("Add Function with Parameters Called");
  return x+y;
}
