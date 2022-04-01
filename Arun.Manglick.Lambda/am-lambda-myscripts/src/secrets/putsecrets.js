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
    SecretId: 'codetogether/dev/username',
    ClientRequestToken: '',
    SecretString: 'netsuitedev@gmail.com'
};

secretsmanager.putSecretValue(params, function (err, data) {
    if (err) {
        console.error("Unable to PUT secret:", JSON.stringify(err, null, 2));
    }
    else {
        console.error("Successfully Secret PUT:", JSON.stringify(data, null, 2));
    }
});