var express = require('express');
var router = express.Router();
// -----------------------------------------------
router.get('/', function (req, res, next) {
  res.send("Welcome to Database API Page Using Promises!");
});
// -----------------------------------------------
router.get('/data', function (req, res, next) {
  const sql = require('mssql');

  let config = {
    user: 'sa',
    password: 'sa',
    server: 'localhost\\SqlExpress',
    database: 'Northwind'
  };

  var connection = new sql.Connection(config);

  connection.connect().then((conn) => {
    var request = new sql.Request(conn);
    request.query('SELECT CustomerId,ContactName FROM Customers').then((results)=>{
      res.send(results);
    },(err)=>{
      res.send("Error in Querying Customer data!");
    });
  }, (reason) => {
    console.log(reason);
    res.send("Some Error");
  });
});
// -----------------------------------------------
router.get('/datawa', function (req, res, next) {
  const sql = require('mssql');
  require("msnodesqlv8");

  let config = {
    server: 'localhost\\SqlExpress',
    database: 'Northwind',
    driver: 'msnodesqlv8',
    options: {
      trustedConnection: true
    }
  };

  var connection = new sql.Connection(config);

  connection.connect().then((conn) => {
    var request = new sql.Request(conn);
    request.query('SELECT CustomerId,ContactName FROM Customers').then((results)=>{
      res.send(results);
    },(err)=>{
      res.send("Error in Querying Customer data!");
    });
  }, (reason) => {
    console.log(reason);
    res.send("Some Error");
  });
});
// -----------------------------------------------
// router.get('/datamg', function (req, res, next) {
//      let dalObject = require('../dataaccess');
//      dalObject.getAllUsers(function(data) {
//        res.send(data);
//      });
// });

router.get('/userlistRaw', function (req, res, next) {
     let dalObject = require('../dataaccess');
     dalObject.getAllUsersWithP().then(data=>{
       res.send(data)
     },msg=>{
       console.log(msg);
       res.send([]);
     });
});
// -----------------------------------------------
router.get('/userlist', function (req, res, next) {
  // let dalObject = require('../dataaccess');
  // dalObject.getAllUsers(function (data) {
  //   res.render('userlist', {
  //     title: 'User List View',
  //     users: data
  //   });
  // });
  let dalObject = require('../dataaccess');
  dalObject.getAllUsersWithP().then(data => {
    res.render('userlist', {
      title: 'User List View',
      users: data
    });
  }, msg => {
    console.log(msg);
    res.send([]);
  });
});
// -----------------------------------------------
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

  let dalObject = require('../dataaccess');
  dalObject.insertUser(obj).then(data => {
    res.redirect("userlist");
  }, msg => {
    console.log(msg);
    res.send("Some Error");
  });
});
// -----------------------------------------------
module.exports = router;