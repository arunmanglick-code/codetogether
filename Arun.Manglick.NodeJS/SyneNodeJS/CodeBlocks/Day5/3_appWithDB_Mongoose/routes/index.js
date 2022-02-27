var express = require('express');
var router = express.Router();

router.get('/', function (req, res, next) {
  res.render('index', {
    title: 'Express Using  Mongoose'
  });
});
// -------------------------------------------
router.get('/userlist', function (req, res, next) {
  let dalObject = require('../dataaccess');
  dalObject.getAllUsers().then(data => {
    res.render('userlist', {
      title: 'User List View',
      users: data
    });
  }, msg => {
    console.log(msg);
    res.send([]);
  });
});
// -------------------------------------------
router.get('/newuser', function (req, res, next) {
  res.render('newuser', {
    title: 'New User View'
  });
});

router.post('/newuser', function (req, res, next) {
  var uname = req.body.username;
  var email = req.body.email;

  let obj = {
    "username": uname,
    "email": email
  };
  // let obj = { };
  console.log(obj);
  let dalObject = require('../dataaccess');
  dalObject.insertUser(obj).then(data => {
    res.redirect("userlist");
  }, msg => {
    console.log(msg);
    res.send("Some Error");
  });
});
// -------------------------------------------
module.exports = router;