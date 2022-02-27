var express = require('express');
var router = express.Router();

module.exports = function (passport) {
  router.get('/', function (req, res, next) {
    res.render('index', {
      title: 'Express (Passport Authentication) '
    });
  });

  router.get('/login', function (req, res, next) {
    res.render('login', {
      title: 'Login View',
      message: req.flash('loginMessage')
    });
  });

  router.post('/login', passport.authenticate('local-login', {
    successRedirect: '/api/userlist',
    failureRedirect: '/login',
    failureFlash: true
  }));

  return router;
};
  