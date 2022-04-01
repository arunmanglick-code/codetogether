import AWS from 'aws-sdk';



export class ExtractorDB {
  constructor() {
    this.dynamoDB = new AWS.DynamoDB();
    this.dynamoDBClient = new AWS.DynamoDB.DocumentClient();
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

  async RemoveItemAsync(params) {
    return this.dynamoDB.deleteItem(params).promise();
  }

  
  async QueryItemsDataAsync(params) {
    try {
      return this.dynamoDBClient.query(params).promise();
    } catch (err) {
      throw err;
    }
  }

  async ScanItemAsync(params) {
    try {
      return this.dynamoDBClient.scan(params).promise();
    } catch (err) {
      throw err;
    }
  }


  async MultipleItemsRemoveAsync(params) {
    try {
      return this.dynamoDBClient.batchWrite(params).promise();
    } catch (err) {
      throw err;
    }
  }

}
