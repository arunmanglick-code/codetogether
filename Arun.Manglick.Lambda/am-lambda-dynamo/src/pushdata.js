var AWS = require("aws-sdk");
var fs = require('fs');

// Source Code: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/GettingStarted.NodeJs.03.html


AWS.config.update({
    region: "ap-south-1"
});

var docClient = new AWS.DynamoDB.DocumentClient();

console.log("Pushing customer into DynamoDB. Please wait.");
var allCustomer = JSON.parse(fs.readFileSync('../resx/input.json', 'utf8'));

allCustomer.forEach(function(customer) {
    var params = {
        TableName: "customer",
        Item: {
            "id": customer.id,
            "age": customer.age,
            "name":  customer.name,
            "region": customer.region,
            "country":  customer.country,
            "postalZip":  customer.postalZip,
        }
    };

    
    docClient.put(params, function(err, data) {
       if (err) {
           console.error("Unable to add customer", customer.name, ". Error JSON:", JSON.stringify(err, null, 2));
       } else {
           console.log("PutItem succeeded:", customer.name);
       }
    });
});