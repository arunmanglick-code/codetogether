provider "aws" {    
  region = "us-east-1"
}

module "ec2module" {
  source = "./ec2"
  tag_name = "AMTF"  
}

