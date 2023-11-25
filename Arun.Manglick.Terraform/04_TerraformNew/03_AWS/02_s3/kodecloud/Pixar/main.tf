resource "aws_s3_bucket" "Pixars" {
  bucket = "pixar-studios-2020"
}

resource "aws_s3_object" "upload" {
  bucket = "pixar-studios-2020"
  key    = "woody.jpg"
  source = "/root/woody.jpg"
}
