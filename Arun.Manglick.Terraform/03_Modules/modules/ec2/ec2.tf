
variable "tag_name" {
  type        = string
}

resource "aws_instance" "amTF_EC2" {
  ami = "ami-0d5eff06f840b45e9"
  instance_type = "t2.micro"

  tags = {
    Name = var.tag_name
  }
}
output "ec2_instanceId"{
  value = aws_instance.amTF_EC2.id
}