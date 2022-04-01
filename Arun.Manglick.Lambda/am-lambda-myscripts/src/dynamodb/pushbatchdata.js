var AWS = require("aws-sdk");
var fs = require('fs');


// Source Code: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/GettingStarted.NodeJs.03.html
// Batch Write: https://docs.aws.amazon.com/sdk-for-javascript/v2/developer-guide/dynamodb-example-table-read-write-batch.html

AWS.config.update({
    region: "ap-south-1"
});

var docClient = new AWS.DynamoDB.DocumentClient();
// var docClient = new AWS.DynamoDB({apiVersion: '2012-08-10'});

console.log("Pushing customer into DynamoDB. Please wait.");

var params = {
    RequestItems: {
        "customer": [
            {
                PutRequest: {
                    Item: {
                        "id": { N: 110 },
                        "age": { N: 45 },
                        "name": { S: "Bumblebee" },
                        "region": { S: "Colarado" },
                        "postalZip": { S: "41121" },
                        "country": { S: "United States" }
                    }
                }
            },
            {
                PutRequest: {
                    Item: {
                        "id": { N: 111 },
                        "age": { N: 47 },
                        "name": { S: "Daniel" },
                        "region": { S: "California" },
                        "postalZip": { S: "32456" },
                        "country": { S: "United States" }
                    }
                }
            }
        ]
    }
};

docClient.batchWriteItem(params, function (err, data) {
    if (err) {
        console.error("Unable to add customer in Batches", customer.name, ". Error JSON:", JSON.stringify(err, null, 2));
    } else {
        console.log("Batch Write Item succeeded:", customer.name);
    }
});