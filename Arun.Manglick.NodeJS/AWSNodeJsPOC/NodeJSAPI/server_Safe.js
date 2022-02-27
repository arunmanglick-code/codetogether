// const registerFunc = require('./Functions/registerTags.js');
// const lookupFunc = require('./Functions/lookupTags.js');

// Register API Call (Post)- http://localhost:8080/registerTags
// Lookup TAG Call (GET) - http://localhost:8080/lookUpTags/<Tag Name>
// Log Lookup (POST) - http://localhost:8080/LogLookup
// Publish to Topic (POST) - http://localhost:8080/publishMessageTopic

const host = 'nodesqsdb.cd8j1xqnsuhe.us-east-1.rds.amazonaws.com';
const user = 'nodesqsdb';
const password = 'nodesqsdb';
const database = 'nodesqsdb';
const queueUrl = 'https://sqs.us-east-1.amazonaws.com/458077907105/FullfillmentQueue';
const region = 'us-east-1';
const endpoint = 'email.us-east-1.amazonaws.com';
const sp_RegisterTagEnrollment = 'sp_update_TagEnrollment';
const sp_LookUpTagEnrollment = 'sp_get_TagEnrollment';
const sp_LogLookUpCall = 'sp_insert_TAGLookUpLog';

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

var connection = mysql.createConnection ({
   host : host,
   user : user,
   password : password,
   database : database
  });

var pool = mysql.createPool ({
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
app.post('/publishMessageTopic',(req,res)=>{
      var stringObject = JSON.stringify(req.body);
      console.log(stringObject);

      var params = {
        Message: stringObject,
        MessageAttributes: {
         "MessageType": {
           DataType: "String",
           StringValue: "String"
          }
        },
        Subject: 'Recieve Device e-Notification From Good John',
        TopicArn: 'arn:aws:sns:us-east-1:458077907105:DeviceReceiptNotificationTopic'
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
});
//----------------------------------------------------------------
app.post('/registerTags',(req,res)=>{
    var registerData = JSON.stringify(req.body);

    if (RegisterMyTags(registerData)) {
      console.log('TAGs Registered successful');
      res.send({result: true});
    } else {
      console.log('Error While TAG Registration');
      res.send({result: false});
    }
});
//----------------------------------------------------------------
app.post('/pushmessage',(req,res)=>{
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
app.get('/lookUpTags/:TagName',(req,res)=>{
  var tagName = req.params.TagName;
  //var rows=  LookupTags(tagName);

  connection.connect(); //Connect MySQL DB
  var query = 'CALL ' + database + '.' + sp_LookUpTagEnrollment + '("' + tagName + '");'
  var rowsResult = null;
  console.log(query);

  connection.query(query, function(err, rows)
    {
      if (err)
      {
        console.log('Error while Lookup', err);  // Show the Error or Log the error in DB
        context.fail();
        connection.end();
        res.send(rowsResult);
      }

      connection.end();
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
    });
});
//----------------------------------------------------------------
app.post('/logLookup',(req,res)=>{
  var logLookupData = JSON.stringify(req.body);

    if (LogLookupData(logLookupData)) {
      console.log('Log Lookup successful');
      res.send({result: true});
    } else {
      console.log('Error While TAG Lookup');
      res.send({result: false});
    }
});
//----------------------------------------------------------------
app.get('/', (req, res) => {
  res.render('welcome.hbs', {
    pageTitle: 'Welcome to Tag Management App - NodeJS API Server ',
    currentYear: new Date().getFullYear()
  })
});//----------------------------------------------------------------
app.get('/Error',(req,res)=>{
res.send( {
      Error : 'This is bad Error'
    }
  )
});
//----------------------------------------------------------------
app.listen(port, () => {
  console.log(`Started up at port ${port}`);
});
//--------------------------------------------------------------------
function RegisterMyTags (message)
{
  var TagData = JSON.parse(message);

  connection.connect(); //Connect MySQL DB

  //Register all 3 recors for tags for subsciriber
  for(var i=0;i<TagData.TagsDetails.length;i++)
  {
    var query = 'CALL ' + database + '.' + sp_RegisterTagEnrollment + '("' + TagData.MDN + '","'
    + TagData.TagsDetails[i].TagName + '","'
    + TagData.TagsDetails[i].Status + '");';

    console.log(query);
    connection.query(query, function(err, rows)
      {
        if (err)
        {
          console.log('Error While TAG Registration', err);  // Show the Error or Log the error in DB
          context.fail();
          return false;
        }
        else
        {
          console.log(rows);
          // TODO: Handle case when it returns zero rows
        }
      });
  }
  connection.end();
  return true;
}
//--------------------------------------------------------------------
// function LookupTags (tagName)
// {
//   connection.connect(); //Connect MySQL DB
//   var query = 'CALL ' + database + '.' + sp_LookUpTagEnrollment + '("' + tagName + '");'
//
//   connection.query(query, function(err, rows)
//     {
//       if (err)
//       {
//         console.log('Error while Lookup', err);  // Show the Error or Log the error in DB
//         context.fail();
//         connection.end();
//         return;
//       }
//
//       connection.end();
//       var rowsResult = JSON.parse(JSON.stringify(rows[0]));
//       return rowsResult[0];
//     });
// }
//--------------------------------------------------------------------
function LogLookupData (message)
{
  var LogData = JSON.parse(message);
  connection.connect(); //Connect MySQL DB

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
  connection.query(query, function(err, rows)
    {
      if (err)
      {
        console.log('Error while Lookup Logging', err);
        context.fail();
        return false;
      }
    });
  connection.end();
  return true;
}
//--------------------------------------------------------------------
