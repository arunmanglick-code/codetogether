var express = require('express');
var router = express.Router();
// ------------------------------------------------------------
router.get('/', function (req, res, next) {
  res.render('index', { title: 'Single Page App' });
});

router.get('/userlist', function (req, res, next) {
    res.render('userlist', { title: 'User List View' });
});

router.get('/newuser', function (req, res, next) {
  res.render('newuser', { title: 'New User View' });
});
// ------------------------------------------------------------
module.exports = router;