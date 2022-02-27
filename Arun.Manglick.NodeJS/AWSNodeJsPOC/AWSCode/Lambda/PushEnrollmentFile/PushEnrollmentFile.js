'use strict';

console.log('Loading function');

const AWS = require('aws-sdk');
const SQS = new AWS.SQS({ apiVersion: '2012-11-05' });
const S3 = new AWS.S3({ apiVersion: '2006-03-01' });

function PushEnrollDataToSQS(enrollmentData)
{
   var params = {
		DelaySeconds: 10,
		MessageAttributes: {
		 "MessageType": {
		  DataType: "String",
		  StringValue: "JSON"
		  }
		},
		MessageBody: enrollmentData,
		QueueUrl: process.env.queueUrl
	  };

	  SQS.sendMessage(params, function(err, ReturnData) {
		 if (err)
		 {
		  console.log(err, err.stack); // an error occurred
		 }
		 else
		 {
		  console.log('Message sent to Queue')
		  console.log(ReturnData);
		 }
	  });
}

exports.handler = (event, context, callback) => {
    // Get the object from the event and show its content type
    const bucket = event.Records[0].s3.bucket.name;
    const key = decodeURIComponent(event.Records[0].s3.object.key.replace(/\+/g, ' '));
    const params = {
        Bucket: bucket,
        Key: key,
    };

    S3.getObject(params, (err, data) => {
        if (err) {
            console.log(err);
            const message = `Error getting object ${key} from bucket ${bucket}. Make sure they exist and your bucket is in the same region as this function.`;
            console.log(message);
            callback(message);
        } else {
            var enrollmentData = JSON.parse(new Buffer(data.Body).toString("utf8"));
             enrollmentData.forEach(function (enrollData) {
                new PushEnrollDataToSQS(JSON.stringify(enrollData));
              });

            callback(null, data.ContentType);
        }
    });
};
