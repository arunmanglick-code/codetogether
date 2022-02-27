'use strict';
const AWS = require('aws-sdk');
const mysql = require('mysql');
AWS.config.update(
  {region: process.env.region},
  {endpoint: process.env.emailEndPoint}
);

const SQS = new AWS.SQS({ apiVersion: '2012-11-05' });
const Lambda = new AWS.Lambda({ apiVersion: '2015-03-31' });
const ses = new AWS.SES({apiVersion: '2010-12-01'});

// const insertFunc = require('./insertTags.js');
// const sendEmailFunc = require('./sendEmail.js');
// const deleteSQSMessageFunc = require('./deleteSQSMessage.js');

// Your queue URL stored in the queueUrl environment variable
const QUEUE_URL = process.env.queueUrl;
const PROCESS_MESSAGE = 'process-message';

const connection = mysql.createConnection ({
    host : process.env.mySQLHost,
    user : process.env.mySQLUserId,
    password : process.env.mySqLPassword,
    database : process.env.mySQLDatabase
  });

function invokePoller(functionName, message) {
    const payload = {
        operation: PROCESS_MESSAGE,
        message,
    };
    const params = {
        FunctionName: functionName,
        InvocationType: 'Event',
        Payload: new Buffer(JSON.stringify(payload)),
    };
    return new Promise((resolve, reject) => {
        Lambda.invoke(params, (err) => (err ? reject(err) : resolve()));
    });
}

//Process Message
function processMessage(message, callback) {
//console.log(message.Body);
var TagData = JSON.parse(message.Body);
var insertResult = InsertTagsMySQL(TagData); // Call Insert Tags in Tag Enroll DB

  if(insertResult)
  {
    console.log("Succesfully Added in DB");
    var sendEmail = SendEmailSES(TagData); // call Send mail function to send welcome mail to subscriber for Tag Registration

    if(sendEmail)
    {
      DeleteSQSMessage(message); // Call Delete Message after processing the message
    }
    else
    {
      console.log('Something Bad happend While Sending Email , Please check SES and Cloudwatch for details');
    }
  }
  else
  {
    console.log('Something Bad happend While Insert , Please check MySQL DB and Cloudwatch for details');
  }
}

function poll(functionName, callback) {
    const params = {
        QueueUrl: QUEUE_URL,
        MaxNumberOfMessages: 10,
        VisibilityTimeout: 30,
    };
    // batch request messages
    SQS.receiveMessage(params, (err, data) => {
        if (err) {
            return callback(err);
        }
        // for each message, reinvoke the function
        const promises = data.Messages.map((message) => invokePoller(functionName, message));
        // complete when all invocations have been made
        Promise.all(promises).then(() => {
            const result = `Messages received: ${data.Messages.length}`;
            console.log(result);
            callback(null, result);
        });
    });
}
// ----------------------------------------------------------------
//Inset Tags in TagEnroll DB
function InsertTagsMySQL (TagData)
{
  connection.connect(); //Connect MySQL DB
  //Insert all 3 recors for tags for subsciriber
  for(var i=0;i<TagData.TagsDetails.length;i++)
  {
    var query = 'CALL ' + process.env.mySQLDatabase + '.sp_insert_TagEnrollmentNew("'+TagData.MDN+'","'+TagData.FirstName+'","'+TagData.LastName+'","'+TagData.Email+'","'+TagData.TagsDetails[i].TagName+'","'+TagData.TagsDetails[i].TagDescription+'","'+TagData.TagsDetails[i].Status+'");';
    console.log(query);
    connection.query(query, function(err, rows)
      {
        if (err)
        {
          console.log(err);  // Show the Error or Log the error in DB
          context.fail();
          return false;
        }
        else
        {
          console.log('Tag Enrollments Data Added');
        }
      });
  }
  connection.end();
  return true;
}

function SendEmailSES (TagData)
{
  //Send Mail to registerd ids
  var to = [TagData.Email];
  var from = process.env.emailSender;

  // Send Mail Function
  ses.sendEmail( {
     Source: from,
     Destination: { ToAddresses: to },
     Message : {
         Subject : {
            Data: 'Tag Registration'
                   },
         Body: {
             Text: {
                      //Data: `Hi ${TagData.FirstName} ${TagData.LastName} \n\nWelcome to Asurion,\n\nPlease register tags by clicking on below link ${process.env.registerTagAppHyperLink}\n\nRegards,\nTAG Team`

                      Data: `Hi ${TagData.FirstName} ${TagData.LastName}` + 
                      '\n\nWelcome to Asurion,'+
                      '\nPlease register tags by clicking on below link.' + 
                      `\n${process.env.registerTagAppHyperLink}` + 
                      `\n\nMDN Tag: ${TagData.MDN}` + 
                      `\nFirst Tag: ${TagData.TagsDetails[0].TagName}` + 
                      `\nSecond Tag: ${TagData.TagsDetails[1].TagName}` + 
                      `\nThird Tag: ${TagData.TagsDetails[2].TagName}` + 
                      '\n\nRegards,' + 
                      '\nTAG Team'

                   }
               }
             }
  }
  , function(err, data) {
      if(err)
      {
        console.log(err);
        context.fail();
        return false;
      }
      else
      {
        console.log('Tag Registration Email sent Successfullly');
        console.log(data);
      }
   });
   return true;
}

function DeleteSQSMessage (message)
{
  const params = {
      QueueUrl: process.env.queueUrl,
      ReceiptHandle: message.ReceiptHandle,
  };
  SQS.deleteMessage(params, function(err, data) {
     if(err)
     {
       console.log(err);// Show error on console
       context.fail();
       return;
     }
     else
     {
       console.log('Messge Processed and Deleted from queue');
     }
  });
}
// ------------------------------------------------------------------
exports.handler = (event, context, callback) => {
    try {
        if (event.operation === PROCESS_MESSAGE) {
            // invoked by poller
            processMessage(event.message, callback);
        } else {
            // invoked by schedule
            poll(context.functionName, callback);
          }
    } catch (err) {
        callback(err);
    }
};
