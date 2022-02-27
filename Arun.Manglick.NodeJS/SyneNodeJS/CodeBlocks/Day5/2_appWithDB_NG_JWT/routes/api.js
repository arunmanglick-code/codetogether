var express = require('express');
var router = express.Router();
var jwt = require('jsonwebtoken');
// ------------------------------------------------------
router.post('/authenticate', function (req, res) {
  var user = {
    username: req.body.username,
    password: req.body.password
  };

  if (user.username != 'manish') {
    res.json({
      success: false,
      message: 'Authentication Failed, User not Found!'
    });
  } else if (user.password != 'manish') {
    res.json({
      success: false,
      message: 'Authentication Failed, Wrong Password!'
    });
  } else {
    var token = jwt.sign(user, 'saltkey', {
      expiresIn: 1440
    });

    res.json({
      success: true,
      message: 'User is Valid!',
      token: token
    });
  }
});
// ------------------------------------------------------
function CheckToken(req, res, next) {
  var token = req.body.token || req.query.token || req.headers['x-access-token'];

  if (token) {
    jwt.verify(token, 'saltkey', function (err, decoded) {
      if (err) {
        return res.json({
          success: false,
          message: 'Failed to authenticate token.'
        });
      } else {
        req.decoded = decoded;
        next();
      }
    })
  } else {
    return res.status(403).send({
      success: false,
      message: 'No token found.'
    });
  }
};
// ------------------------------------------------------
router.get('/users', CheckToken, function (req, res, next) {
  let dalObject = require('../dataaccess');
  dalObject.getAllUsers().then(data => res.send(data), msg => {
    console.log(msg);
    res.send([]);
  });
});

router.post('/users', CheckToken, function (req, res, next) {
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
// ------------------------------------------------------
module.exports = router;