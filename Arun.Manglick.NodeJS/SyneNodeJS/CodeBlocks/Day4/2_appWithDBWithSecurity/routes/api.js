var express = require('express');
var router = express.Router();

module.exports = function (passport) {
  function IsLoggedIn(req, res, next) {
    if (req.isAuthenticated())
      return next();

    res.redirect('./login');
  }
 // ------------------------------------------------------------------
  router.get('/userlist', IsLoggedIn, function (req, res, next) {
    let dalObject = require('../dataaccess');
    dalObject.getAllUsersWithP().then(data => {
      res.render('userlist', {
        title: 'User List View',
        users: data
      });
      next();
    }, msg => {
      console.log(msg);
      res.send([]);
    });
  });
// ------------------------------------------------------------------
  router.get('/newuser',IsLoggedIn, function (req, res, next) {
    res.render('newuser', {
      title: 'New User View'
    });
  });

  router.post('/newuser', IsLoggedIn, function (req, res, next) {
    var uname = req.body.username;
    var email = req.body.email;

    let obj = {
      "username": uname,
      "email": email
    };

    let dalObject = require('../dataaccess');
    dalObject.insertUser(obj).then(data => {
      res.redirect("userlist");      
    }, msg => {
      console.log(msg);
      res.send("Some Error");
    });
  });
// ------------------------------------------------------------------
  return router;
};
  