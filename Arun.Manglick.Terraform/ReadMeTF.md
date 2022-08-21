1). Install terraform - 
    - Check this - https://www.terraform.io/downloads.html
    - Installation is simple - 
        - Download Zip, Unzip - There will be only one file 'terraform.exe'
        - Store this exe at any location of your choice and then set 'PATH' environment variable
2). Create IAM User and Assign Admin Access

3). Write Scripts
4). Connect to AWS - aws configure (then enter access key and secure access key for the IAM user created in step#2)
3). Run terraform
        terraform init
        terraform plan
        terraform apply - This creates terraform.tfstate file
        terraform destroy


Section 5: EC2:
    - aws-Instance : https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/instance
    - aws-EIP:  https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/eip
    - aws-SG: https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/security_group


Section 7: Modules:
    - Modules - https://registry.terraform.io/modules/terraform-aws-modules/vpc/aws/latest




