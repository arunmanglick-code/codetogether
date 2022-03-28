// import Lib_DynamoDB from '../lib/lib_dynamo';

var AWS = require("aws-sdk");
var fs = require('fs');
// var database = new Lib_DynamoDB();

// Source Code: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/GettingStarted.NodeJs.03.html


AWS.config.update({
    region: "ap-south-1"
});

var docClient = new AWS.DynamoDB.DocumentClient();

console.log("Updating customer int DynamoDB. Please wait.");
var custId = 107;
var age = 57;

// Use delete method to delete one item by specifying its primary key. 
// Otionally provide a ConditionExpression to prevent the item from being deleted if the condition is not met.

var params = {
    TableName: "customer",
    Key: { 
        'id': custId,
        'age': age
    },
    ConditionExpression: "age > :a",
    ExpressionAttributeValues:{
        ":a": 56
    }
};  

    
docClient.delete(params, function(err, data) {
    if (err) {
        console.error("Unable to delete item. Error JSON:", JSON.stringify(err, null, 2));
    } else {
        console.log("DeleteItem succeeded:", JSON.stringify(data, null, 2));
    }
});
