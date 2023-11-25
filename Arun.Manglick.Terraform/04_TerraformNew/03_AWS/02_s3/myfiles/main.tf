#main.tf
provider "aws" {
  region     = "us-east-2"
  access_key = "AKIA5VIYIZPQY2ECMIFE"
  secret_key = "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"
}

resource "aws_s3_bucket" "finance" {
  bucket = "my-tf-test-bucket-east-2"

  tags = {
    Description = "Finance and Payroll"
  }
}

resource "aws_s3_object" "object" {
  bucket  = aws_s3_bucket.finance.id
  key     = "pets.txt"
  content = "/root/pets.txt"
}

# Method 1
resource "aws_s3_bucket_policy" "finance_policy" {
  bucket = aws_s3_bucket.finance.id
  policy = jsonencode(
    {
      "Version" : "2012-10-17",
      "Statement" : [
        {
          "Sid" : "my-policy-statement",
          "Effect" : "Allow",
          "Principal" : {
            "AWS" : "arn:aws:iam::939038133217:user/lucy"
          },
          "Action" : "s3:*",
          "Resource" : "arn:aws:s3:::my-tf-test-bucket-east-2"
        }
      ]
    }
  )
}

# Method 2
# data "aws_iam_policy_document" "example" {
#   statement {
#     sid = "my-policy-statement"

#     effect = "Allow"
#     actions =[
#       "s3:*",
#     ]

#     resources = [
#       "arn:aws:s3:::${aws_s3_bucket.finance.id}",
#     ]
#   }
# }

# resource "aws_s3_bucket_policy" "finance_policy" {
#   bucket = aws_s3_bucket.finance.id
#   policy = data.aws_iam_policy_document.example.json
# }


output "bucket-name" {
  value       = aws_s3_bucket.finance.id
  description = "This is your bucket-name"
}

output "bucket-name-arn" {
  value       = aws_s3_bucket.finance.arn
  description = "This is your bucket-arn"
}

# output "bucket-policy" {
#   value       = data.aws_iam_policy_document.example.json
#   description = "This is your bucket-policy"
# }

