// Function Definiton
var getUser= function(id, callback){
  var user = {
   id: id,
   name: 'Vikram'
 };

 setTimeout(() => {
   callback(user);
 },2000);
};
// ----------------------------------------------------
// Calling Function & Passing CallBack (In three different approches)
// First Way
getUser(31, function (userObject) {
  console.log(userObject);
});

// Second Way
getUser(41, (userObject) => {
  console.log(userObject);
});

// Third Way
//getUser(51, callMeBack );

// var callMeBack = function (userObject){
//   console.log(userObject);
// };
// ----------------------------------------------------
