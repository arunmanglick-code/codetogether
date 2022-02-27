var async = require('async');
var AWS = require('aws-sdk');

exports.handler = async (event) => {
const ssm = new (require('aws-sdk/clients/ssm'))()

// Read Single Parameter specifying Full Path
const data = await ssm.getParameters({
        Names: ['/prod/ssm/params/string']
    }).promise();

// Read All Parameter specifying Base Path
const dataRecursive = await ssm.getParametersByPath({
    Path: "/prod/ssm/params",
    Recursive: true,
    WithDecryption: true
}).promise();

// In case you want to update paramters, use code below (For now Commented)
// var params = {
//     Name: '/prod/ssm/params/string',
//     Value: 'Changed to Arun ',
//     Overwrite: true,
//     Type: 'String'
// };  
// // Update existing userName
// await ssm.putParameter(params).promise();


const response = {
        statusCode: 200,
        body: dataRecursive
    };

return response;
};
