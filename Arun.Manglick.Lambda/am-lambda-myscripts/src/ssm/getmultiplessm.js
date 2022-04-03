var AWS = require("aws-sdk");
AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SSM.html#putParameter-property

var ssm = new AWS.SSM();

var params = {
    Names: [
        '/codetogether/dev/ssm/username',
        '/codetogether/dev/ssm/userzipcode',
    ],
    WithDecryption: true
};

ssm.getParameters(params, function (err, data) {
    if (err) {
        console.error("Unable to Retreive SSM Parameter:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully SSM Parameter Retreived:", JSON.stringify(data, null, 2));

        console.log("Query Results:");

        var count = 1;
        data.Parameters.forEach(function(item) {
            console.log(count++ + " -", item.Name + ": " + item.Value);
        });
    }
});