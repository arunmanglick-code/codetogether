'use strict';

console.log("Loading the DynamoDB event");
var AWS = require('aws-sdk');
var region = process.env.AWS_REGION || "us-west-1";
// var docClient = new AWS.DynamoDB.DocumentClient({ region: region });
var tableName = 'lilypoc-readwritedb';

AWS.config.update({
    region: "us-west-1",
    endpoint: "http://localhost:8000"
});
var docClient = new AWS.DynamoDB.DocumentClient();

console.log("Pulling Data From DynamoDB. Please wait.");

var promptParams = {
    TableName: "lilypoc-readwritedb",
    Key: {
        "ItemId": "102"
    }
};

docClient.get(promptParams, function (err, data) {
    if (err) {
        console.error("Unable to read item. Error JSON:", JSON.stringify(err, null, 2));
    } else {
        console.log("GetItem succeeded:", JSON.stringify(data, null, 2));
    }
});