provider "aws" {
  region = "us-east-2"
  access_key = "AKIA5VIYIZPQY2ECMIFE"
  secret_key = "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"
}

resource "aws_iam_user" "admin-user" {
  name = "lucy"
  tags = {
    Description = "Technical Team Leader"
  }
}

resource "aws_iam_policy" "admin-user-policy" {
  name        = "AdminUserPolicy"
  description = "Admin policy"
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect   = "Allow"
        Action   = "*"
        Resource = "*"
      }
    ]
  })
}

resource "aws_iam_user_policy_attachment" "lucy-attach-admin-access" {
  user       = aws_iam_user.admin-user.name
  policy_arn = aws_iam_policy.admin-user-policy.arn
}

