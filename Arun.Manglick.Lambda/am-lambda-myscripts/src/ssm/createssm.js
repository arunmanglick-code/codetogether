var AWS = require("aws-sdk");
AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SSM.html#putParameter-property

var ssm = new AWS.SSM();

var params = {
    Name: '/codetogether/dev/ssm/userzipcode',
    Value: '37067',
    DataType: 'text',
    Description: 'Param Store: Zipcode',
    Tags: [
        {
            Key: 'codetogether',
            Value: 'ssm'
        }
    ],
    Type: "String"
};

ssm.putParameter(params, function (err, data) {
    if (err) {
        console.error("Unable to create SSM Parameter:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully SSM Parameter Created:", JSON.stringify(data, null, 2));
    }
});