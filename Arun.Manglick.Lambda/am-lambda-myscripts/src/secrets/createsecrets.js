var AWS = require("aws-sdk");
AWS.config.update({
    region: "ap-south-1"
});

// Ref: https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/AWS/SecretsManager.html#createSecret-property

var secretsmanager = new AWS.SecretsManager();

// PutSecretKey(secretName, secretValue) {
//     return this.secretManager.putSecretValue({
//         SecretId: secretName,
//         SecretString: secretValue
//     }).promise();
// }

// smClient.putSecretValue({
//     SecretId: secretName,
//     SecretString: secretValue});


var params = {
    Name: 'codetogether/dev/username',
    ClientRequestToken: '',
    Description: '',
    SecretString: 'ms365dev@gmail.com',
    Tags: [
        {
            Key: 'codetogether',
            Value: 'secret'
        }
    ]
};

secretsmanager.createSecret(params, function (err, data) {
    if (err) {
        console.error("Unable to create secret:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully Secret Created:", JSON.stringify(data, null, 2));
    }
});