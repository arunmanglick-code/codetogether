'use strict';
const AWS = require('aws-sdk');
AWS.config.update(
  {region: 'ap-south-1'}
);

const s3 = new AWS.S3();
const kinesis = new AWS.Kinesis();

console.log('Loading function');
console.log ('Region:' + process.env.region);

exports.handler = async (event, context) => {
    
    console.log("Reading options from event:\n", JSON.stringify(event, {depth: 5}));
	var srcBucket = event.Records[0].s3.bucket.name;  // Input Bucket Name
	var srcKey = event.Records[0].s3.object.key; // Input File Name
	  
	console.log("Source Bucket: " + srcBucket);
	console.log("Source Key: " + srcKey);
	  
    var params = {
	    Bucket: srcBucket,
	    Key: srcKey
	  };
	  
	 await s3.getObject(params).promise().then (async (data) => {
	    	const dataString = data.Body.toString();
	    	console.log ("S3 Input Data: " + dataString);
	    	const payLoad = {
	    		data: dataString
	    	};
	    	await sendtoKinesis(payLoad, srcKey);
		}, err => {
			console.error(err);
    		console.log(err);
	 });
	
	async function sendtoKinesis (payLoad, keyName)
	{
		const kinesisParams = {
	    		Data: JSON.stringify(payLoad),
	    		PartitionKey: keyName,
	    		StreamName: 'amdatastreamings3tokinesis-kinesis'
	    	};
	    	
	    await kinesis.putRecord(kinesisParams).promise().then(response => {
	    	console.log(response);
	    }, err => {
	    	console.log(err);
	    });
	}
};
