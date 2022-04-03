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


var params1 = {
    QueueUrl: 'https://sqs.ap-south-1.amazonaws.com/013016973542/codetogethersqs',
};

sqs.receiveMessage(params1, function (err, data1) {
    if (err) {
        console.error("Unable to receive message from SQS:", JSON.stringify(err, null, 2));
    }
    else {   
        console.error("Successfully receive message from SQS:", JSON.stringify(data1, null, 2));

        var params = {
            QueueUrl: 'https://sqs.ap-south-1.amazonaws.com/013016973542/codetogethersqs',
            ReceiptHandle : data1.Messages[0].ReceiptHandle
        };     
        sqs.deleteMessage(params, function (err, data2) {
            if (err) {
                console.error("Unable to delete message from SQS:", JSON.stringify(err, null, 2));
            }
            else {
                console.error("Successfully delete message from SQS:", JSON.stringify(data2, null, 2));
            }
        });
    }
});


// sqs.deleteMessage(params, function (err, data) {
//     if (err) {
//         console.error("Unable to delete message from SQS:", JSON.stringify(err, null, 2));
//     }
//     else {
//         console.error("Successfully delete message from SQS:", JSON.stringify(data, null, 2));
//     }
// });