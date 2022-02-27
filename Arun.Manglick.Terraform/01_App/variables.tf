variable "instance_name" {
  description = "Value of the Name tag for the EC2 instance"
  type        = string
  default     = "AMTF"
}

variable "amNumeric" {
  type        = number
  default     = 35
}

variable "amBool" {
  default     = true
}

variable "amList" {
  type = list(string)
  default = ["hell0", "world"]
}

variable "amMap" {
  type = map
  default = {
    key1 = "hello"
    key2="world"
  }
}


variable "amTuple" {
  type = tuple ([string, number, string])
  default = ["Hello", 2020, "World"]
}

variable "amObject" {
  type = object ({name=string, port=list(number)})
  default = {
    name = "AM"
    port = [11,22,33]
  }
}