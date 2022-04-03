var AWS = require("aws-sdk");
AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SSM.html#putParameter-property

var ssm = new AWS.SSM();

var params = {
    Name: '/codetogether/dev/ssm/username'
};

ssm.deleteParameter(params, function (err, data) {
    if (err) {
        console.error("Unable to Delete SSM Parameter:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully SSM Parameter Deleted:", JSON.stringify(data, null, 2));
    }
});