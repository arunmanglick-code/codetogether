# arun-aws-lambda-master
# This Repo will contain lambdas per project

# This Lambda Function is to 
	# Read/process anything added in S3, 
	# Process it thru Lambda and Push data to Kinesis Stream 
	# Which is further processed by Consumer Lambdas

# Prerequisites/Steps:
  # Role Policy: Create a role assign this to all three lambdas - Having three policy added - AmazonS3FullAccess, AmazonKinesisFullAccess, CloudWatchFullAccess
  # Create S3 with Version and Defualt Encryption Enabled
  # Create Kinesis Stream with Shard Count as 2 and Default Server Side Encryption (AWS-CMK)
  # Create Three Lambdas
	 # Create Lambda1: processS3pushtoKinesis_func.js
 	 # Create Lambda1: consumer1_func.js	
	 # Create Lambda2: consumer2_func.js
  # Add Triggers
    # Lambda1 Trigger S3
    # Lambda2 Trigger Kinesis
    # Lambda3 Trigger Kinesis
  
  # Kinesis Stream - This can be created over console and then add trigger to Lambda
  # Kinesis Stream - This can be created over command line CLI also as below and then add trigger to Lambda (Either over Console or CLI)
  

# Define all three lambdas directly in Lambda IDE
# You can copy paste data from the .js files given under 'src' folder

# Testing:
  # To Test - Within Lambda 
    # Upload TestFile.txt file to S3
    # Use Test Event of S3 and Make three changes - Bucket Name, Bucket ARN & .Txt File Name
    # Hit Test and You'll find output in CloudWatch of all three Lambdas

  # To Test - By Adding Trigger to Lambda (Thru Console)
    # Upload TestFile.txt file to S3    
    # You'll find output in CloudWatch of all three Lambdas

# Source Repo: AWS Dev Course - https://www.youtube.com/watch?v=We5Jr4GGLL0&list=PLWSJgJmES26kUhqognPZufmu1IR2xfbjC&index=1

     
     
     
    
  
