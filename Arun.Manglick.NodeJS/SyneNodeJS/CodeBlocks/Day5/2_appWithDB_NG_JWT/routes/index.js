var express = require('express');
var router = express.Router();
// ---------------------------------------------
router.get('/', function (req, res, next) {
  res.render('index', { title: 'Single Page App Using JWT' });
});
// ---------------------------------------------
router.get('/userlist', function (req, res, next) {
    res.render('userlist', { title: 'User List View Using JWT' });
});
// ---------------------------------------------
router.get('/newuser', function (req, res, next) {
  res.render('newuser', { title: 'New User View Using JWT' });
});
// ---------------------------------------------
module.exports = router;