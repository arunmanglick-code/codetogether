# arun-aws-lambda-master
# This Repo will contain lambdas per project

# This Lambda Function is to read/process anything added in Kinesis Stream
# This Lambda is triggered when an object is posted in Kinesis Stream
# Finally the object posted to be read in CloudWatch

# Prerequisites:
  # Kinesis Stream - This can be created over console and then add trigger to Lambda
  # Kinesis Stream - This can be created over command line CLI also as below and then add trigger to Lambda (Either over Console or CLI)
  # Role Policy: Create a role assign this to lambda, having one policy added - AWSLambdaKinesisExecutionRole

# Make a zip of file - .js file
# Ensure Handler Info is updated from 'index.handler' to the name of lambda code js file. I.e. 'processkinesisrecords_func.handler'

# Testing:
  # To Test - Within Lambda GUI
    # Use Test Event of Kinesis Stream and Make no changes 
    # Hit Test and You'll find output as 'Hello, this is a test 123.' (Base64 Code - SGVsbG8sIHRoaXMgaXMgYSB0ZXN0IDEyMy4=)

  # To Test - By Adding Trigger to Lambda (Thru Console)
    # Add Trigger in Lambda to Kinesis Stream (Already created thru console)
    # Put Record in Kinesis Stream using AWS CLI Command 
    # (aws kinesis put-record --stream-name amprocesskinesisstream --data "Hello Input in Kinesis using AWS CLI" --partition-key shardId-000000000000)
    # Output - Check in CloudWatch

# Source Repo: AWS Dev Course - https://github.com/skroonenburg/Lambda-Lab-3

# -----------------------------------------------------------------------------------------------
# AWS CLI Command - 
  # Create Lambda Function Using the Zip File of Code (Ref: https://docs.aws.amazon.com/lambda/latest/dg/gettingstarted-awscli.html)
    aws lambda create-function
    --function-name processkinesisrecords_func \
    --zip-file fileb://processkinesisrecords_func.zip
    --handler processkinesisrecords_func.handler 
    --runtime nodejs14.x \
    --timeout 10 \
    --meomory-size 1024 \
    --role arn:aws:iam::791309171132:role/AWSLambdaKinessisLabRole
   
   # Execute Lambda Using the Test Event (Use TestFile.txt - https://github.com/arunmanglick-code/arun-aws-lambda-master/blob/main/thumbnail-lambda/TestFile.txt)
     aws lambda invoke-async \
     --function-name processkinesisrecords_func
     --invoke-args TestFile.txt
     
   # Create Kinesis Stream
   Refernece: https://docs.aws.amazon.com/lambda/latest/dg/with-kinesis-example.html
    aws kinesis create-stream --stream-name amprocesskinesisstream  --shard-count 1
    aws kinesis describe-stream --stream-name amprocesskinesisstream  
    
   # Add Trigger in Lambda  to Kinesis Stream
    aws lambda create-event-source-mapping 
    --function-name processkinesisrecords_func \
    --event-source arn:aws:kinesis:us-east-2:791309171132:stream/amprocesskinesisstream \ 
    --batch-size 100 \
    --starting-position TRIM_HORIZON
    
   # Put Record in Kinesis Stream
    aws kinesis put-record 
    --stream-name amprocesskinesisstream \
    --data "Hello Input in Kinesis using AWS CLI" \
    --partition-key shardId-000000000000 
# -----------------------------------------------------------------------------------------------
     
     
     
    
  
