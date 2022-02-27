// const registerFunc = require('./Functions/registerTags.js');
// const lookupFunc = require('./Functions/lookupTags.js');

// Register API Call (Post)- http://localhost:8080/registerTags
// Lookup TAG Call (GET) - http://localhost:8080/lookUpTags/<Tag Name>
// Log Lookup (POST) - http://localhost:8080/LogLookup
// Initiate Claim FulFillment (POST) - http://localhost:8080/initiateClaimFulFillment
// Publish to Topic (POST) - http://localhost:8080/completeClaimFulFillment/<Tag Name>
// Reset DB - http://localhost:8080/resetDB
// CheckConnection - http://localhost:8080/checkAPIConnection

const region = 'us-east-1';
const host = 'nodesqsdb.cd8j1xqnsuhe.us-east-1.rds.amazonaws.com';
const user = 'nodesqsdb';
const password = 'nodesqsdb';
const database = 'nodesqsdb';
const queueUrl = 'https://sqs.us-east-1.amazonaws.com/458077907105/FullfillmentQueue';
const topicARN = 'arn:aws:sns:us-east-1:458077907105:DeviceReceiptNotificationTopic';
const endpoint = 'email.us-east-1.amazonaws.com';
const sp_RegisterTagEnrollment = 'sp_update_TagEnrollment';
const sp_LookUpTagEnrollment = 'sp_get_TagEnrollment';
const sp_LogLookUpCall = 'sp_insert_TAGLookUpLog';
const sp_TagLookupandTagEnrollCall = 'sp_get_TagLookupandTagEnroll';
const sp_ResetDatabase= 'sp_reset_TagDatabase';

var AWS = require('aws-sdk');
var mysql = require('mysql');
AWS.config.update(
  {region: region},
  {endpoint: endpoint}
);

var express = require('express');
const hbs = require('hbs');
var bodyParser = require('body-parser');
var sqs = new AWS.SQS();  // Either use below or login using CLI - AWS Configure
var sns = new AWS.SNS();  // Either use below or login using CLI - AWS Configure

var pool = mysql.createPool ({
   connectionLimit : 10,
   host : host,
   user : user,
   password : password,
   database : database
  });

var app = express();
app.set('view engine', 'hbs');
app.use(bodyParser.json());
const port = process.env.PORT || 8080;
//----------------------------------------------------------------
app.post('/registerTags',(req,res)=>{
    var registerData = JSON.stringify(req.body);
    var TagData = JSON.parse(registerData);
    var rowsResult = null;
    var query = null;

    //Register all 3 records for tags for subsciriber
    pool.getConnection(function(err, connection) {
      query = 'CALL ' + database + '.' + sp_RegisterTagEnrollment + '("' + TagData.MDN + '","'
      + TagData.TagsDetails[0].TagName + '","'
      + TagData.TagsDetails[1].TagName + '","'
      + TagData.TagsDetails[2].TagName + '","'
      + TagData.TagsDetails[0].Status + '");';

      console.log(query);
      connection.query(query, function(err, rows)
        {
          if (err)
          {
            console.log('Error While TAG Registration', err);  // Show the Error or Log the error in DB
            // context.fail();
            connection.release();
            res.send({result: false});
          }
          else
          {
            //rowsResult = JSON.parse(JSON.stringify(rows));
            rowsResult = JSON.parse(JSON.stringify(rows[0]));
            console.log(rows);
            console.log(rowsResult.length);

            if(rowsResult.length == 3)
            {
              console.log('TAGs Registered successful');
              connection.release();
              res.send({result: true});
            }
            else {
              console.log('Incorrect Input Entered While TAG Registration 1');
              connection.release();
              res.send({result: false});
            }
          }
        });
    });
});
//----------------------------------------------------------------
app.get('/lookUpTags/:TagName',(req,res)=>{
  var tagName = req.params.TagName;

  var query = 'CALL ' + database + '.' + sp_LookUpTagEnrollment + '("' + tagName + '");'
  var rowsResult = null;
  console.log(query);

  pool.getConnection(function(err, connection) {
    connection.query(query, function(err, rows)
      {
        if (err)
        {
          console.log('Error while Lookup', err);  // Show the Error or Log the error in DB
          context.fail();
          connection.release();
          res.send(rowsResult);
        }
        else {
          // connection.end();
          var rowsResult = JSON.parse(JSON.stringify(rows[0]));
          if(rowsResult.length > 0)
          {
            console.log('TAGs Found successful');
            res.send(rowsResult[0]);
          }
          else
          {
            console.log('TAGs Not Found');
            res.send({result: false});
          }
          connection.release();
        }
      });
  });
});
// -----------------------------------------------------------------
app.post('/logLookup',(req,res)=>{
  var logLookupData = JSON.stringify(req.body);
  var LogData = JSON.parse(logLookupData);

  //Register all 3 recors for tags for subsciriber
  var query = 'CALL ' + database + '.' + sp_LogLookUpCall + '("' + LogData.FirstName + '","'
  + LogData.LastName + '","'
  + LogData.Email + '","'
  + LogData.Phone + '","'
  + LogData.Address + '","'
  + LogData.TagName + '",'
  + LogData.TagEnrollId + ',"'
  + LogData.TagFound + '");';

  console.log(query);

  pool.getConnection(function(err, connection) {
    connection.query(query, function(err, rows)
      {
        if (err)
        {
          console.log('Error While TAG Lookup', err);
          context.fail();
          connection.release();
          res.send({result: false});
        }
        else {
          console.log('Log Lookup successful');
          connection.release();
          res.send({result: true});
        }
      });
  });
});
//----------------------------------------------------------------
app.post('/initiateClaimFulFillment',(req,res)=>{
      var stringObject = JSON.stringify(req.body);
      var params = {
        DelaySeconds: 10,
        MessageAttributes: {
         "MessageType": {
           DataType: "String",
           StringValue: "JSON"
          }
        },
        MessageBody: stringObject,
        QueueUrl: queueUrl
       };

       sqs.sendMessage(params, function(err, ReturnData) {
         if (err)
         {
           console.log(err, err.stack); // an error occurred
         }
         else
         {
          console.log('Message sent to Queue')
          console.log(ReturnData);
          res.send(ReturnData);
          // successful response
         }
       });
});
//----------------------------------------------------------------
app.post('/completeClaimFulFillment/:TagName',(req,res)=>{

     var tagName = req.params.TagName;
     //--------------------------------
     var query = 'CALL ' + database + '.' + sp_TagLookupandTagEnrollCall + '("' + tagName + '");'
     var rowsResult = null;
     var stringObject = null;
     console.log(query);
     pool.getConnection(function(err, connection) {
       connection.query(query, function(err, rows)
         {
           if (err)
           {
             console.log('Error while Lookup', err);  // Show the Error or Log the error in DB
             context.fail();
             connection.release();
             res.send(rowsResult);
           }
           else {

             rowsResult = JSON.parse(JSON.stringify(rows[0]));
             if(rowsResult.length > 0)
             {
               stringObject = JSON.stringify(rowsResult[0]);
               console.log('Claim FullFillment Data Found successful');
               console.log(stringObject);
               var params = {
                 Message: stringObject,
                 MessageAttributes: {
                  "MessageType": {
                    DataType: "String",
                    StringValue: "String"
                   }
                 },
                 Subject: 'Recieved Device e-Notification From Good John',
                 TopicArn: topicARN
                };

                sns.publish(params, function(err, ReturnData) {
                  if (err)
                  {
                    console.log(err, err.stack); // an error occurred
                    res.send({result: false});
                  }
                  else
                  {
                   console.log('Message Published to SNS Topic')
                   console.log(ReturnData);
                   res.send({result: true});
                   // successful response
                  }
                });
             }
             else
             {
               console.log('Claim FullFillment Data Not Found');
               res.send({result: false});
             }
             connection.release();
           }
         });
     });
});
//----------------------------------------------------------------
//----------------------------------------------------------------
app.get('/resetDB',(req,res)=>{

  var query = 'CALL ' + database + '.' + sp_ResetDatabase + '();'
  var rowsResult = null;
  console.log(query);

  pool.getConnection(function(err, connection) {
    connection.query(query, function(err, rows)
      {
        if (err)
        {
          console.log('Reset Failed Error', err);  // Show the Error or Log the error in DB
          context.fail();
          connection.release();
          res.send({result: false});
        }
        else {
          console.log('Reset Done');
          res.send({result: true});
          connection.release();
        }
      });
  });
});
// -----------------------------------------------------------------
app.get('/', (req, res) => {
  res.render('welcome.hbs', {
    pageTitle: 'Welcome to Tag Management App - NodeJS API Server ',
    currentYear: new Date().getFullYear()
  })
});
//----------------------------------------------------------------
app.get('/Error',(req,res)=>{
res.send( {
      Error : 'This is bad Error'
    }
  )
});
//----------------------------------------------------------------
app.get('/checkAPIConnection',(req,res)=>{
res.send(
    {
      connect: true
    }
  )
});
//----------------------------------------------------------------
app.listen(port, () => {
  console.log(`Started up at port ${port}`);
});
