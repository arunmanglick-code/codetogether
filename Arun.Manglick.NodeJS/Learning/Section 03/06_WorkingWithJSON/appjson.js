// Step 1:Reading JSON Object, Convering into String and Finally Storing as String in .json file
//----------------------------------------------------------------------------------
var jsonObj = {
  FirstN: 'Jorge',
  LastN: 'Bush'
};

var jsonString=JSON.stringify(jsonObj);
console.log('Type: ', typeof jsonString);
console.log(jsonString);

const fs = require('fs');
fs.writeFileSync('notes.json', jsonString);
//----------------------------------------------
// Step 2:Reading String and converting as JSON Object
var jsonString2 = '{"Name":"Arun","Address":"USA"}';
var jsonObj2= JSON.parse(jsonString2);
console.log('Type: ', typeof jsonObj2);
console.log(jsonObj2);

//----------------------------------------------
// Step 3:Reading String stored in .json file (In Step 1) and converting as JSON Object
var jsonString3 = fs.readFileSync('notes.json');
var jsonObj3= JSON.parse(jsonString3);
console.log('Type: ', typeof jsonObj3);
console.log(jsonObj3);
