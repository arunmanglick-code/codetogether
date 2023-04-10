resource "local_file" "pet1" {
  filename = var.filename
  content  = var.content["statement2"]
  file_permission = "0777"
}

resource "random_pet" "my-pet1" {
  prefix = "Mr."
  separator = var.separator
  length = var.length
}

output "pet_details" {
  value = local_file.pet1.content
}