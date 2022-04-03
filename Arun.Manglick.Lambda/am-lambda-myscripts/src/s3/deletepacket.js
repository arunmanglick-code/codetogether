var AWS = require("aws-sdk");
var Moment = require('moment'); 

AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SSM.html#putParameter-property

var s3 = new AWS.S3();

var sqsmsg = {
    "id": 109,
    "age": 59,
    "name":  'Harry',
    "region": 'Illionois',
    "country":  'USA',
    "postalZip":  '41121'
};

var filename=  'codetogethers3file' + '.json'; // 'codetogethers3file' + '_' + Moment.utc().millisecond().toString() + '.json';

var params = {
    Bucket: "codetogethers3",
    Key: filename
};

s3.deleteObject(params, function (err, data) {
    if (err) {
        console.error("Unable to delete packet from S3:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully packet deleter from S3:", JSON.stringify(data, null, 2));
    }
});