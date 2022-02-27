// dependencies
var async = require('async');
var AWS = require('aws-sdk');
var gm = require('gm')
            .subClass({ imageMagick: true }); // Enable ImageMagick integration.
var util = require('util');

// constants
var MAX_WIDTH  = 100;
var MAX_HEIGHT = 100;

// get reference to S3 client 
var s3 = new AWS.S3();
var kinesis = new AWS.Kinesis();

console.log('Loading function');

// exports.handler = async (event, context) => {
//     //console.log('Received event:', JSON.stringify(event, null, 2));
// 	console.log ('Dummy Test for Thumbnail Lambda');
// 	console.log ('Dummy Test for Thumbnail Lambda Again');
//     console.log('value1 =', event.key1);
//     console.log('value2 =', event.key2);
//     console.log('value3 =', event.key3);
//     return event.key1;  // Echo back the first key value
//     // throw new Error('Something went wrong');
// };

exports.handler = function(event, context) {
	// Read options from the event.
	console.log("Reading options from event:\n", util.inspect(event, {depth: 5}));
	var srcBucket = event.Records[0].s3.bucket.name;  // Input Bucket Name
	// Object key may have spaces or unicode non-ASCII characters.
    var srcKey    =  decodeURIComponent(event.Records[0].s3.object.key.replace(/\+/g, " "));   // Input File Name
	var dstBucket = srcBucket + "-thumbnail";  // Destination Bucket
	var dstKey    = "-thumbnail-" + srcKey;     //  Destination File Name (Thumbnail)

	console.log("Source Bucket: " + srcBucket);
	console.log("Source Key: " + srcKey);
	console.log("Destination Bucket: " + dstBucket);
	console.log("Destination Key: " + dstKey);

	// Sanity check: validate that source and destination are different buckets.
	if (srcBucket == dstBucket) {
		console.error("Destination bucket must not match source bucket.");
		return;
	}
	else
	{
	 console.log("Destination & Source bucket Matched.");
	}

	//Infer the image type.
	var typeMatch = srcKey.match(/\.([^.]*)$/);
	if (!typeMatch) {
		console.error('unable to infer image type for key ' + srcKey);
		return;
	}
	else{
		console.log("Type Match Done");
	}


	var imageType = typeMatch[1];
	if (imageType != "jpg" &&  imageType != "jpeg" && imageType != "png") {
		console.log('skipping non-image ' + srcKey);
		return;
	}

	// Download the image from S3, transform, and upload to a different S3 bucket.
	console.log("Start: Download the image from S3, transform, and upload to a different S3 bucket Process");
	async.waterfall([
		function download(next) {
			// Download the image from S3 into a buffer.
			console.log('Download the image from S3 into a buffer');

			s3.getObject({
					Bucket: srcBucket,
					Key: srcKey
				},
				next);
			},
		// function tranform(response, next) {
		// 	console.log('Transforming image to thumbnail');
		// 	gm(response.Body).size(function(err, size) {
		// 		// Infer the scaling factor to avoid stretching the image unnaturally.
		// 		var scalingFactor = Math.min(
		// 			MAX_WIDTH / size.width,
		// 			MAX_HEIGHT / size.height
		// 		);
		// 		var width  = scalingFactor * size.width;
		// 		var height = scalingFactor * size.height;

		// 		// Transform the image buffer in memory.
		// 		console.log('Transform the image buffer in memory');
		// 		this.resize(width, height)
		// 			.toBuffer(imageType, function(err, buffer) {
		// 				if (err) {
		// 					next(err);
		// 				} else {
		// 					next(null, response.ContentType, buffer);
		// 				}
		// 			});
		// 	});
		// },
		function upload(response, next) {

				// Stream the transformed image to a different S3 bucket.
				console.log('Streaming the transformed image to a different S3 bucket');
				s3.putObject({
						Bucket: dstBucket,
						Key: dstKey,
						Body: response.Body,
						ContentType: response.ContentType
					},
					next);

				
				// Pushing a Record in Kinesses about Image processed from Image bucket to Thumbnail bucket		
				// console.log('Image Processed Record Pushed to Kinesis');
				
				// const params = {
				// 	Data: "Pushing Record in Kinesses -  For Image Processed in Thumbnail is: " + dstKey,
				// 	PartitionKey: "shardId-000000000000",
				// 	StreamName: "AMLambdaKinesisStream"
				// }

				// kinesis.putRecord(params, (err, data) => {
				// 		if (err) console.log(err);
				// 		else console.log("data sent");
				// 	}
				// )
			}
		], function (err) {
			if (err) {
				console.error(
					'Unable to resize ' + srcBucket + '/' + srcKey +
					' and upload to ' + dstBucket + '/' + dstKey +
					' due to an error: ' + err
				);
			} else {
				console.log(
					'Successfully resized ' + srcBucket + '/' + srcKey +
					' and uploaded to ' + dstBucket + '/' + dstKey
				);
			}

			context.done();
		}
	);
};