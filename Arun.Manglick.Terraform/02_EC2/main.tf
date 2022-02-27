provider "aws" {    
  region = "us-east-1"
}

variable "tag_name" {
  type        = string
  default     = "AMTF"
}

resource "aws_instance" "amTF_EC2" {
  ami = "ami-0d5eff06f840b45e9"
  instance_type = "t2.micro"
  security_groups = [aws_security_group.amTF_SG.name]

  tags = {
    Name = var.tag_name
  }
}

resource "aws_eip" "amTF_EIP" {
  instance = aws_instance.amTF_EC2.id
  vpc      = true
}

resource "aws_security_group" "amTF_SG" {
  name        = "Allow HTTPs"

  ingress {
    from_port        = 443
    to_port          = 443
    protocol         = "tcp"
    cidr_blocks      = ["0.0.0.0/0"]
  }

  egress {
    from_port        = 443
    to_port          = 443
    protocol         = "tcp"
    cidr_blocks      = ["0.0.0.0/0"]
  }

  tags = {
    Name = "allow_tls"
  }
}

output "myOutput"{
  value = aws_eip.amTF_EIP.public_ip
}