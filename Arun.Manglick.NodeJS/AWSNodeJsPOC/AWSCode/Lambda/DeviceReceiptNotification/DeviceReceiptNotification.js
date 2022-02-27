'use strict';
const aws = require('aws-sdk');
const mysql = require('mysql');
aws.config.update({endpoint: process.env.emailEndPoint});
const ses = new aws.SES({apiVersion: '2010-12-01'});

const connection = mysql.createConnection ({
    host : process.env.mySQLHost,
    user : process.env.mySQLUserId,
    password : process.env.mySqLPassword,
    database : process.env.mySQLDatabase
  });


function SendEmailSES(TagData,template)
{
var params=null;
var from = process.env.emailSender;
if(template == 'ThankYou')
{
   params = {
     Source: from,
     Destination: { ToAddresses:  [TagData.GJ_Email] },
     Message : {
         Subject : {
            Data: 'Thank You'
                   },
         Body: {
             Text: {
                    //Data: `Hi ${TagData.GJ_FirstName} ${TagData.GJ_LastName} We have received ${TagData.TagName} , we are very thankful for your honesty and as token of appriciation we are giving you a gift card of $25`

                    Data: `Hi ${TagData.GJ_FirstName} ${TagData.GJ_LastName}` + 
                        `\n\nWe have received device having Tag name ${TagData.TagName}.` + 
                        '\nWe are very thankful for your honesty and as token of appreciation we are giving you a gift card of $25'+
                        '\n\nRegards,' + 
                        '\nTAG Team'
                   }
               }
             }
         };
}
else if(template == 'DeviceShipmentNotice')
{
   params = {
     Source: from,
     Destination: { ToAddresses: [TagData.Sub_Email]  },
     Message : {
         Subject : {
            Data: 'DeviceShipmentNotice'
                   },
         Body: {
             Text: {
                    //Data: `Hi ${TagData.Sub_FirstName} ${TagData.Sub_LastName} , We have received your lost device with tagname  ${TagData.TagName}and shipped to your enroll address with Standered shipment. Thank you for using our service.`

                    Data: `Hi ${TagData.Sub_FirstName} ${TagData.Sub_LastName}` + 
                        `\n\nWe have received your lost device having Tag name ${TagData.TagName}.` + 
                        '\nWe have shipped device to your enroll address with Standered shipment. Thank you for using our service.'+
                        '\n\nRegards,' + 
                        '\nTAG Team'
                   }
               }
             }
         };
}
  // Send Mail Function
  ses.sendEmail(params
  , function(err, data) {
      if(err)
      {
        console.log(err);
        return false;
      }
      else
      {
        console.log( `${template}` + ' Email sent Successfullly');
        console.log(data);
      }
   });
   return true;
}

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

  var result = false;

  console.log(query);
  connection.query(query, function(err, rows)
    {
      if (err)
      {
        console.log('DB Connection Error :', err);  // Show the Error or Log the error in DB
        context.fail();
        connection.end();
      }
      else
      {
        console.log('Tag Lookup Updated');
        connection.end();
        result= true;
      }
    });
  return result;
}


exports.handler = (event, context, callback) => {
    const message = event.Records[0].Sns.Message;
    var TagData = JSON.parse(message);
    var sendEmail_GJ  = SendEmailSES(TagData,'ThankYou'); // send ThankYou mail to good john
    var sendEmail_Subscriber = SendEmailSES(TagData,'DeviceShipmentNotice'); // Send shipment notification to Subscriber

    if(sendEmail_Subscriber && sendEmail_GJ)
    {
      var sentEmailToSubscriber = "Yes";
      var sentEnvelopToGoodJohn = "Yes";
      var recievedDeviceFromGoodJohn = "Yes";
      var sentGiftCardToGoodJohn = "Yes";
      var sentDeviceShipmentToSubscriber = "Yes";

      console.log("Call UpdateLogLookUp ");
      var insertResult = UpdateLogLookUp(TagData,
        sentEmailToSubscriber,sentEnvelopToGoodJohn,recievedDeviceFromGoodJohn,
        sentGiftCardToGoodJohn,sentDeviceShipmentToSubscriber); // Call Insert Tags in Tag Enroll DB

      if(insertResult)
      {
        console.log("Succesfully Update in DB");
      }
      else
      {
        console.log('Something Bad happend While Insert , Please check MySQL DB and Cloudwatch for details');
      }
    }

    callback(null, message);
};
