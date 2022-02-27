# arun-aws-lambda-master
# This Repo will contain lambdas per project

# This Lambda Function is to generate thumbnail of any image posted to S3
# This Lambda is triggered when an object is posted in S3 Bucket
# Finally this image is transformed in a thumbnail and posted in another S3 Bucket 

# Prerequisites:
  # Note: Both Buckets Should Exists before Running Lambda (The lambda code does not generate destination bucket)
  # Note: Destination Bucket Name should be same as Source Bucket with '-thumbnail' attached as suffix. 
    # I.e. Source Bucket Name: 'amawslambdaoriginalimages'
    # I.e. Destination Bucket Name: 'amawslambdaoriginalimages-thumbnail'
  # Role Policy: Create a role havign two policies added - AWSLambdaFullAccess and AWSS3FullAccess

# Make a zip of all files - .js file, node_modules, package.json (Very Important: Make Zip thru content and not thru folder else it'll generate child folder which won't work)
# Upload this zip file to AWS Lambda
# Ensure Handler Info is updated from 'index.handler' to the name of lambda code js file. I.e. 'makethumbnail_func.handler'

# To Test, Create an S3 Bucket
# Add Trigger in Lambda from this S3 Bucket
# Upload any image in S3
# As a result, find thumbnail of this image stored back in S3

# Source Repo: AWS Dev Course - https://github.com/skroonenburg/Lambda-Lab-2

# AWS CLI Command - 
  # Create Lambda Function Using the Zip File of Code (Ref: https://docs.aws.amazon.com/lambda/latest/dg/gettingstarted-awscli.html)
    aws lambda create-function
    --function-name makethumbnail_func \
    --zip-file fileb://makethumbnail_func.zip
    --handler makethumbnail_func.handler 
    --runtime nodejs14.x \
    --timeout 10 \
    --meomory-size 1024 \
    --role arn:aws:iam::791309171132:role/AMAWSLambdaThumbmnail
   
   # Execute Lambda Using the Test Event (Use TestFile.txt - https://github.com/arunmanglick-code/arun-aws-lambda-master/blob/main/thumbnail-lambda/TestFile.txt)
     aws lambda invoke-async \
     --function-name makethumbnail_func
     --invoke-args TestFile.txt
     
     
     
    
  
