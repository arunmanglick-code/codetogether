// import Lib_DynamoDB from '../lib/lib_dynamo';

var AWS = require("aws-sdk");
var fs = require('fs');
// var database = new Lib_DynamoDB();

// Source Code: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/GettingStarted.NodeJs.03.html


AWS.config.update({
    region: "ap-south-1"
});

var docClient = new AWS.DynamoDB.DocumentClient();

console.log("Query customer DynamoDB. Please wait.");
var custId = 101;
var age = 58;

var params = {
    TableName: "customer",
    ProjectionExpression: "id, age, #myname, country, #myregion",
    FilterExpression: "id > :custId",
    ExpressionAttributeNames:{
        "#myregion": "region",
        "#myname": "name"
    },
    ExpressionAttributeValues:{
        ":custId": custId
    }
};  
    
docClient.scan(params, function(err, data) {
    if (err) {
        console.error("Unable to Scan item. Error JSON:", JSON.stringify(err, null, 2));
    } else {
        console.log("Scan Item succeeded:", JSON.stringify(data, null, 2));

        console.log("Query Results:");

        var count = 1;
        data.Items.forEach(function(item) {
            console.log(count++ + " -", item.id + ": " + item.name);
        });
    }
});
