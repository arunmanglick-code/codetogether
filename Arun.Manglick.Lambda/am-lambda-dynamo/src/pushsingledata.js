// import Lib_DynamoDB from '../lib/lib_dynamo';

var AWS = require("aws-sdk");
var fs = require('fs');
// var database = new Lib_DynamoDB();

// Source Code: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/GettingStarted.NodeJs.03.html


AWS.config.update({
    region: "ap-south-1"
});

var docClient = new AWS.DynamoDB.DocumentClient();

console.log("Pushing customer into DynamoDB. Please wait.");

var params = {
    TableName: "customer",
    Item: {
        "id": 108,
        "age": 59,
        "name":  "John",
        "region": "Nashville",
        "country":  "United States",
        "postalZip":  "37067",
    }
};  


// database.InsertItem(params).catch(async (err) => {
//     console.error("Unable to add customer", params.Item.name, ". Error JSON:", JSON.stringify(err, null, 2));
// });
    
docClient.put(params, function(err, data) {
    if (err) {
        console.error("Unable to add customer", params.Item.name, ". Error JSON:", JSON.stringify(err, null, 2));
    } else {
        console.log("PutItem succeeded:", params.Item.name);
    }
});
