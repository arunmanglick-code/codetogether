var express = require('express');
var router = express.Router();
// ---------------------------------------------
router.get('/users', function (req, res, next) {
  let dalObject = require('../dataaccess');
  dalObject.getAllUsers().then(data => res.send(data), msg => {
    console.log(msg);
    res.send([]);
  });
});

router.post('/users', function (req, res, next) {
  let dalObject = require('../dataaccess');
  var uname = req.body.username;
  var email = req.body.email;

  let obj = {
    "username": uname,
    "email": email
  };

  dalObject.insertUser(obj).then(data => {
    res.send(data);
  }, msg => {
    console.log(msg);
    res.send("Some Error");
  });
});
// ---------------------------------------------
module.exports = router;