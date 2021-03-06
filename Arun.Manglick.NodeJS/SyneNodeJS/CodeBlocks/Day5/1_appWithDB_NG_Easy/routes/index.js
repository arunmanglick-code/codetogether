var express = require('express');
var router = express.Router();
// ---------------------------------------------
router.get('/', function (req, res, next) {
  res.render('index', { title: 'Single Page App without Angular 2' });
});
// ---------------------------------------------
router.get('/userlist', function (req, res, next) {
    res.render('userlist', { title: 'User List View - With Angular Routing  & Angular Controller' });
});
// ---------------------------------------------
router.get('/newuser', function (req, res, next) {
  res.render('newuser', { title: 'New User View' });
});
// ---------------------------------------------
module.exports = router;