console.log('Require App Project');
// ---------------------------------------------------
const fs = require('fs');
fs.appendFile('MyGreeting.txt', 'Writing Successful!!!',function(err){
  if(err)
  {
    console.log('Cannot write to file');
  }
});

// ---------------------------------------------------
const os = require('os');
var user = os.userInfo();
console.log(user);
console.log('Hello ' + user.username + ' !!!');
console.log('Hello' + ` ${user.username} !!!`);
console.log(`Hello ${user.username} !!!`);
