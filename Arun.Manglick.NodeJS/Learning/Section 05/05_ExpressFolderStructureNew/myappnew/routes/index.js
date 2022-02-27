var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('index', { title: 'Express App (Check Node.Js Installation Over AWS Linux AMI EC2)' });
});

module.exports = router;
