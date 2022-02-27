# Configure the AWS Provider
provider "aws" {    
  region = "us-east-1"
}

// https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/security_group
resource "aws_instance" "amExample" {
  ami           = "ami-2757f631"
  instance_type = "t2.micro"

  tags = {
    Name = var.instance_name
  }
}

output "myOutput"{
  value = aws_instance.amExample.tags
}