resource "local_file" "pet" {
  filename = "/root/petsNew.txt"
  content  = "We love pets!"
}

resource "local_file" "petGraph" {
  filename   = "/root/petsGraph.txt"
  content    = "My favorite pet is ${random_pet.my-pet.id}"
  depends_on = [random_pet.my-pet]
}

resource "random_pet" "my-pet" {
  prefix    = "Mr."
  separator = "."
  length    = "1"
}
