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
var TagData = JSON.parse(message.Body);

var sendEmail_Subscriber = SendEmailSES(TagData,"Subscriber"); // Send Email to Subscriber for his/her lost device found and expected delivery
var sendEmail_GJ = SendEmailSES(TagData,"GoodJohn"); // Send Envelop to Good John for sending back the found device in envelop

if(sendEmail_Subscriber && sendEmail_GJ)
{
  var sentEmailToSubscriber = "Yes";
  var sentEnvelopToGoodJohn = "Yes";
  var recievedDeviceFromGoodJohn = "No";
  var sentGiftCardToGoodJohn = "No";
  var sentDeviceShipmentToSubscriber = "No";

  console.log("Call UpdateLogLookUp ");
  var insertResult = UpdateLogLookUp(TagData,
    sentEmailToSubscriber,sentEnvelopToGoodJohn,recievedDeviceFromGoodJohn,
    sentGiftCardToGoodJohn,sentDeviceShipmentToSubscriber); // Call Insert Tags in Tag Enroll DB

  console.log("Insert Results are : ", insertResult);

  if(insertResult)
  {
    console.log("Succesfully Update in DB");
    DeleteSQSMessage(message); // Call Delete Message after processing the message
  }
  else
  {
    console.log('Something Bad happend While Insert , Please check MySQL DB and Cloudwatch for details');
  }
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
function UpdateLogLookUp(TagData,sentEmailToSubscriber,sentEnvelopToGoodJohn,recievedDeviceFromGoodJohn,sentGiftCardToGoodJohn,sentDeviceShipmentToSubscriber)
{
  connection.connect(); //Connect MySQL DB

  //Update TAG Lookup Information with Appropriate flags
  var query = 'CALL ' + process.env.mySQLDatabase + '.sp_update_TAGLookUpLog('
  +TagData.TagEnrollId+',"'
  +TagData.TagName+'","'
  +sentEmailToSubscriber+'","'
  +sentEnvelopToGoodJohn+'","'
  +recievedDeviceFromGoodJohn+'","'
  +sentGiftCardToGoodJohn+'","'
  +sentDeviceShipmentToSubscriber+'");';

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
        console.log('Tag Lookup Updated');
      }
    });

  connection.end();
  return true;
}

function SendEmailSES (TagData, Template)
{
  //Send Mail to registerd ids
  var from = process.env.emailSender;
  var params = null;

  if(Template == 'Subscriber')
  {
    params = {
       Source: from,
       Destination: { ToAddresses: [TagData.Sub_Email] },
       Message : {
           Subject : {
              Data: 'Found Device Notification'
                     },
           Body: {
               Text: {
                        //Data: `Hi, ${TagData.Sub_FirstName} ${TagData.Sub_LastName} We found your device and will be sending soon.`

                        Data: `Hi ${TagData.Sub_FirstName} ${TagData.Sub_LastName}` + 
                        `\n\nWe found your device having Tag name as ${TagData.TagName} and will be sending soon.`+
                        '\n\nRegards,' + 
                        '\nTAG Team'

                     }
                 }
               }
    };
  }
  else if(Template == 'GoodJohn') {
    params = {
       Source: from,
       Destination: { ToAddresses: [TagData.GJ_Email] },
       Message : {
           Subject : {
              Data: 'Sending Empty Device Envelop '
                     },
           Body: {
               Text: {
                        //Data: `Hi, ${TagData.GJ_FirstName} ${TagData.GJ_LastName} Thank you for your services. Please send found device back in enclosed envelop`

                        Data: `Hi ${TagData.GJ_FirstName} ${TagData.GJ_LastName}` + 
                        '\n\nThank you for your services.'+
                        `\nPlease send found device having Tag name ${TagData.TagName} back in enclosed envelop.` + 
                        '\n\nRegards,' + 
                        '\nTAG Team'
                     }
                 }
               }
    };
  }

  // Send Mail Function
  ses.sendEmail( params, function(err, data) {
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
