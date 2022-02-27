var express = require('express');
var router = express.Router();
var factory = require('./factory.js');

router.get('/', function (req, res, next) {
  res.send("Welcome to Database API Page!");
});
// ------------------------------------------
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
    // request.query('SELECT CustomerId,ContactName FROM Customers', function(err,results){
    //   res.send(results);
    // });
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
// ------------------------------------------
router.get('/datawa', function (req, res, next) {
  const sql = require('mssql');
  require("msnodesqlv8");

  let config = {
    server: 'ARUNMANGLICK-PC\\SQLEXPRESS', //'localhost\\SqlExpress',
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
// ------------------------------------------
router.get('/datamg', function (req, res, next) {
   var monk = require('monk');
   var db = monk("localhost/nodetest1");
   var collection = db.get("usercollection");

  //  collection.find({},{},function (err,results) {
  //    res.send(results);
  //  });

  collection.find({},{}).then((results)=>{
    res.send(results);
  },(err)=>{
    res.send("Error in Querying Customer data!");
  });
});
// ------------------------------------------
router.get('/userlist', function (req, res, next) {
  var dl = factory.NoSQLFactory.getDB();
  var collection = dl.db("Checking ...");

  collection.find({},{}).then((results)=>{
    res.send(results);
  },(err)=>{
    res.send("Error in Querying Customer data!");
  });
});
// ------------------------------------------
router.get('/newuser', function (req, res, next) {
  res.render('newuser', {
    title: 'New User View'
  });
});
// ------------------------------------------------
router.post('/newuser', function (req, res, next) {
  var uname = req.body.username;
  var email = req.body.email;
  let data = {
    "username": uname,
    "email": email
  }
  
  var dl = factory.NoSQLFactory.getDB();
  var collection = dl.db("Checking...");

  collection.insert(data).then((results)=>{
    res.redirect('/api/userlist');
  },(err)=>{
    res.send("Error in Querying Customer data!");
  });

  //res.send("Data will be saved...");
});

// -----------------------------------------------
module.exports = router;
