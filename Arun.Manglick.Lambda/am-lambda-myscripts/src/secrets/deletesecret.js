var AWS = require("aws-sdk");
AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SecretsManager.html#createSecret-property

var secretsmanager = new AWS.SecretsManager();

var params = {
    SecretId: 'codetogether/dev/hello'
};

secretsmanager.deleteSecret(params, function (err, data) {
    if (err) {
        console.error("Unable to Delete secret:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully Secret Deleted:", JSON.stringify(data, null, 2));
    }
});