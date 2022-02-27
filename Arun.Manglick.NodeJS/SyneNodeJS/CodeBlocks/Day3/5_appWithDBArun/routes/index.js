var express = require('express');
var router = express.Router();
// ---------------------------------------------
/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('index', { title: 'My Express (Mongo DB Demo1)' });
});
// ---------------------------------------------
router.get('/userlist', function(req, res, next) {
   var monk = require('monk');
   var db = monk("localhost/nodetest1");
   var collection = db.get("usercollection");

   collection.find({},{},function (err,results) {
      res.render('userlist', { title: 'User List View', users:results });
   });
});
// ---------------------------------------------
module.exports = router;
