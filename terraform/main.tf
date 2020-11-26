provider "aws" {
    version = "~> 2.7"
    region = "us-west-2"
}

resource "aws_s3_bucket" "b" {
  bucket = "dev-moshea-lambdaalb-deployment"
  acl    = "private"

  versioning {
    enabled = true
  }

  logging {
    target_bucket = "moshea-s3-access-logging"
    target_prefix = "log/"
  }

  server_side_encryption_configuration {
    rule {
      apply_server_side_encryption_by_default {
        sse_algorithm     = "AES256"
      }
    }
  }

  tags = {
    PointOfContact = "Mike OShea"
    TeamManager = "Mike OShea"
    Schedule = "NA"
    Name = "Deployment Bucket"
    BusinessUnit = "PPA"
  }
}

resource "aws_ssm_parameter" "p1" {
  name = "/lambdaalb/deploybucket"
  type = "SecureString"
  value = aws_s3_bucket.b.bucket
}

