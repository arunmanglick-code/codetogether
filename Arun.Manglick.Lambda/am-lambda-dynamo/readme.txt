Using Command Line

- Create DynamoDB Table 
    aws dynamodb create-table --table-name facts --attribute-definitions  AttributeName=fact_id,AttributeType=N --key-schema  AttributeName=fact_id,KeyType=HASH  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5

- Push Data in Table
    aws dynamodb batch-write-item --request-items file://items.json