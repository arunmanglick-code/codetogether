Here this POC talks about - 
 - Setting up Ec2 with CloudWatch Alert Monitoring ON
 - Using CloudWatch as DataSource for Grafana

Setup AWS:
    - Sign in to AWS Account
    - Create EC2 with with CloudWatch Alert Monitoring ON
    - Create IAM User and Keep AWS AccessKeyId and SecretAccessKey
    - Create Policy allow CloudWatch Access
        - Used this one - https://grafana.com/docs/grafana/latest/datasources/aws-cloudwatch/
    
Setup Grafana with DataSource as AWS Cloudwatch
    - AuthProvider: Access & SecretAccessKey
    - Enter Both Keys
    - Choose Region
    - Connect

How to generate EC2 Metrics, pushing to CloudWatch and Consumed in Grafana
 - Connect to above spinned up Ec2 using Putty
 - Elevate usng 'sudo su'
 - Fire command so that Ec2 CPU Utilization hits high: 'while true; do echo; done'
 - Now go to Grafana and Create Dashboard/Panel and Oversight CPU Utilization Metrics

 
    