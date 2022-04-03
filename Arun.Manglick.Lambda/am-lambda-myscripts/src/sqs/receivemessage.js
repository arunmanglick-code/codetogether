var AWS = require("aws-sdk");
AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SSM.html#putParameter-property

var sqs = new AWS.SQS();

var params = {
    QueueUrl: 'https://sqs.ap-south-1.amazonaws.com/013016973542/codetogethersqs',
};

sqs.receiveMessage(params, function (err, data) {
    if (err) {
        console.error("Unable to receive message from SQS:", JSON.stringify(err, null, 2));
    }
    else {
        // console.error("Successfully receive message from SQS:", JSON.stringify(data.Messages[0].Body, null, 2));
        console.error("Successfully receive message from SQS:", data.Messages[0].Body);
    }
});