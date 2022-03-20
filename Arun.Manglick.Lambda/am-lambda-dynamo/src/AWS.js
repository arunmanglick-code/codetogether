import AWS from 'aws-sdk';

export class AWSServices {

    constructor() {
        this.SQS = new AWS.SQS();
        this.S3 = new AWS.S3();
        this.lambda = new AWS.Lambda();
        this.secretManager = new AWS.SecretsManager();
    }

    SendMessageToSQS(queueUrl, message, delayTime) {
        return this.SQS.sendMessage({
            MessageBody: JSON.stringify(message),
            QueueUrl: queueUrl,
            DelaySeconds: delayTime
        }).promise();
    }

    DeleteMessageFromSQS(queueUrl, id) {
        return this.SQS.deleteMessage({ QueueUrl: queueUrl, ReceiptHandle: id }).promise();
    }

    PushObjectToS3(bucket, filename, body) {
        return this.S3.putObject({
            Bucket: bucket,
            Key: filename,
            Body: JSON.stringify(body),
            ContentType: 'application/json'
        }).promise();
    }

    GetObjectFromS3(bucket, filename) {
        return this.S3.getObject({ Bucket: bucket, Key: filename }).promise();
    }

    DeleteS3Object(bucket, filename) {
        return this.S3.deleteObject({ Bucket: bucket, Key: filename }).promise();
    }

    ListAllS3Object(bucket) {
        return this.S3.listObjects({ Bucket: bucket }).promise();
    }

    PutSecretKey(secretName, secretValue) {
        return this.secretManager.putSecretValue({
            SecretId: secretName,
            SecretString: secretValue
        }).promise();
    }

    async GetSecretValue(secretKey) {
        try {
            const secretData = await this.secretManager.getSecretValue({
                SecretId: secretKey
            }).promise().catch((err) => {
                return err;
            });

            if (secretData.SecretString) {
                return (secretData.SecretString);
            } else {
                throw new Error('Secret Value Not Found!');
            }
        } catch (err) {
            return err;
        }
    }

    GetItem(params, callback) {
        this.dynamoDBClient.get(params, (err, data) => {
            if (err)
                return callback(err, null);
            else
                return callback(null, data);
        });
    }

    UpdateItem(params, callback) {
        this.dynamoDBClient.update(params, (err, data) => {
            if (err) {
                return callback(err, null);
            } else {
                return callback(null, data);
            }
        });
    }

    InsertItem(params, callback) {
        this.dynamoDB.putItem(params, (err, data) => {
            if (err)
                return callback(err, null);
            else
                return callback(null, data);
        })
    }

    QueryItem(params, callback) {
        this.dynamoDBClient.scan(params, (err, data) => {
            if (err)
                return callback(err, null);
            else
                return callback(null, data);
        })
    }

    RemoveItem(params, callback) {
        this.dynamoDB.deleteItem(params, (err, data) => {
            if (err)
                return callback(err, null);
            else
                return callback(null, data);
        })
    }

    MultipleItemsRemove(params, callback) {
        this.dynamoDBClient.batchWrite(params, (err, data) => {
            if (err)
                return callback(err, null);
            else
                return callback(null, data);
        })
    }

    QueryItemsData(params, callback) {
        this.dynamoDBClient.query(params, (err, data) => {
            if (err)
                return callback(err, null);
            else
                return callback(null, data);
        });
    }

    async GetItemAsync(params) {
        try {
            return this.dynamoDBClient.get(params).promise();
        } catch (err) {
            throw err;
        }

    }

    async UpdateItemAsync(params) {
        try {
            console.log('UpdateItemAsync(). params: ' + JSON.stringify(params));
            return this.dynamoDBClient.update(params).promise();
        } catch (err) {
            throw err;
        }

    }

    async InsertItemAsync(params) {
        try {
            return this.dynamoDB.putItem(params).promise();
        } catch (err) {
            throw err;
        }

    }

    async QueryItemAsync(params) {
        try {
            return this.dynamoDBClient.scan(params).promise();
        } catch (err) {
            throw err;
        }

    }

    async RemoveItemAysnc(params) {
        try {
            return this.dynamoDB.deleteItem(params).promise();
        } catch (err) {
            throw err;
        }

    }

    async MultipleItemsRemoveAysnc(params) {
        try {
            return this.dynamoDBClient.batchWrite(params).promise();
        } catch (err) {
            throw err
        }

    }

    async QueryItemsDataAsync(params) {
        try {
            return this.dynamoDBClient.query(params).promise();
        } catch (err) {
            throw err;
        }

    }
}