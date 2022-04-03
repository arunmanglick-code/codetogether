var AWS = require("aws-sdk");
AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SSM.html#putParameter-property

var ssm = new AWS.SSM();

var params = {
    Name: '/codetogether/dev/ssm/username',
    WithDecryption: true
};

ssm.getParameter(params, function (err, data) {
    if (err) {
        console.error("Unable to Retreive SSM Parameter:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully SSM Parameter Retreived:", JSON.stringify(data, null, 2));
    }
});