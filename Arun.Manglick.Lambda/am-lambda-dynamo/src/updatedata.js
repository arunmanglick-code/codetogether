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

var params = {
    TableName: "customer",
    Key: { 
        'id': custId,
        'age': age
    },
    UpdateExpression: "set postalZip = :p, country=:c",
    ConditionExpression: "age > :a",
    ExpressionAttributeValues:{
        ":p":37671,
        ":c":"Franklin",
        ":a": 56
    },
    ReturnValues:"UPDATED_NEW"
};  


// database.InsertItem(params).catch(async (err) => {
//     console.error("Unable to add customer", params.Item.name, ". Error JSON:", JSON.stringify(err, null, 2));
// });
    
docClient.update(params, function(err, data) {
    if (err) {
        console.error("Unable to update item. Error JSON:", JSON.stringify(err, null, 2));
    } else {
        console.log("UpdateItem succeeded:", JSON.stringify(data, null, 2));
    }
});
