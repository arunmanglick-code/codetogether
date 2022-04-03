var AWS = require("aws-sdk");
AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SSM.html#putParameter-property

var sqs = new AWS.SQS();

var sqsmsg = {
    "id": 109,
    "age": 59,
    "name":  'Harry',
    "region": 'Illionois',
    "country":  'USA',
    "postalZip":  '41121'
};

var params = {
    MessageBody: JSON.stringify(sqsmsg),
    QueueUrl: 'https://sqs.ap-south-1.amazonaws.com/013016973542/codetogethersqs',
    DelaySeconds : 0
};

sqs.sendMessage(params, function (err, data) {
    if (err) {
        console.error("Unable to send message to SQS:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully send message to SQS:", JSON.stringify(data, null, 2));
    }
});