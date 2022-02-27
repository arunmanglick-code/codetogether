'use strict';

console.log("Loading the DynamoDB event");
var AWS = require('aws-sdk');
var region = process.env.AWS_REGION || "us-west-1";
var docClient = new AWS.DynamoDB.DocumentClient({ region: region });
var tableName = 'lilypoc-readwritedb';

// AWS.config.update({
//     region: "us-west-1",
//     endpoint: "http://localhost:8000"
// });
// var docClient = new AWS.DynamoDB.DocumentClient();

console.log("Importing Data into DynamoDB. Please wait.");

// ------------------------------------------------------
function putConfigItem(params) {
    return new Promise(function (resolve, reject) {
        docClient.put(params, function (err, data) {
            if (err) {
                reject(err);
            } else {
                resolve(data.Item);
            }
        });
    });
}
// ------------------------------------------------------------------
exports.handler = (event, context, callback) => {
    try {
        var promptParams = {
            TableName: "lilypoc-readwritedb",
            Item: {
                "ItemId": "102",
                "FName": "David"
            }
        };
       
        putConfigItem(promptParams).then(
            data => {
                console.log('Success');
                callback(null, data);
            },
            error => {
                console.log('Data Error:');
                callback(null, error);
            }
        )
    } catch (err) {
        console.log('Function Error');
        callback(err);
    }
};