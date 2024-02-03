variable "filename" {
  default = "/root/petsVars.txt"
  type    = string
}

variable "separator" {
  default = "."
}

variable "length" {
  default = "1"
}

variable "content" {
  type = map(any)
  default = {
    "statement1" = "We love more pets!"
    "statement2" = "We love animals!"
  }
  description = "the path of local"
}