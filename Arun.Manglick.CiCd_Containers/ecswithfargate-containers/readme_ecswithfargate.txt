# ecswithfargate-containers
# This Repo is to demonstrate - 
    # How to create Docker of a Simple Web aAplication,
    # Create Docker Image and Push to ECR (Docker Repo) and 
    # Then Run In ECS With Fargate (Serverless)
    # Finally Browse in Browser

# This Function can be divided in three parts (as in HLD)
    # Part A – Create App and Dockerize App to create Docker Image
    # Part B: Create ECR & Push Docker Image in ECR
    # Part C: Host Docker Image as Docker Container in ECS Fargate 

# Prerequisites:
    # Required in Part A: Create Role (am_AmazonEC2ContainerRegistryFullAccess) for EC2 To Connect with ECR - Policy as 'AmazonEC2ContainerRegistryFullAccess' 
    # Required in Part C: Create Role (ecsTaskExecutionRole) for ECS (Elastic Container Service) - Policy as 'AmazonECSTaskExecutionRolePolicy' 
        # The task execution role grants the Amazon ECS container and Fargate agents permission to make AWS API calls on your behalf
        # Ref: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/task_execution_IAM_role.html

Note:
    # Docker Image Name:amecswithfargate-helloworld-webapp
    # ECR Name: amecswithfargate-ecr-repository
    # ECR URI: 791309171132.dkr.ecr.ap-south-1.amazonaws.com/amecswithfargate-ecr-repository
    # ECS Cluster Name: amecswithfargate-ecs-cluster  (amecswithec2-ecs-cluster)
    # ECS Task Definition Name: amecswithfargate-ecs-taskdefinition  (amecswithec2-ecs-taskdefinition)


# Steps: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/docker-basics.html
  # Part A: 
    # Create EC2 Instance (Linux 2) & Install Docker on the EC2
        # Create Role for EC2 To Connect with ECR - Policy as 'AmazonEC2ContainerRegistryFullAccess'
        # Create/Use Existing SG - Having Http:80 & SSH:22
        # Create EC2 - Specifying Role and SG
        # Connect to EC2
            # Update the installed packages : sudo yum update -y
            # Install the most recent Docker Engine package   
              ••  sudo amazon-linux-extras install docker
            # Start the Docker service: 
              ••  sudo service docker start
            # Add the ec2-user to the docker group so you can execute Docker commands without using sudo
              ••  sudo usermod -a -G docker ec2-user
        # Log out and log back in again to pick up the new docker group permissions. By closing your current SSH terminal window and reconnecting to your instance
            # Verify that the ec2-user can run Docker commands without sudo : 
              ••  docker info

    # Create a Docker image of a Simple HelloWorld Web Application on the above defined EC2  
        # Background
            # Amazon ECS task definitions use Docker images to launch Docker containers on the container instances in your clusters. 
            # In this section, you create a Docker image of a simple web application, 
            # Test it on your local system or Amazon EC2 instance, 
            # Later then push the image to a container registry (such as Amazon ECR or Docker Hub) so you can use it in an Amazon ECS task definition.
        # Create a file called Dockerfile and Copy DockerFile Content from  'resx\DockerfileContent.txt' and Save this file
          ••  nano Dockerfile  (Stored here: \arun-aws-ecs-ecr-fargate-docker-master-\ecswithfargate-containers\src\Dockerfile)
        # Build the Docker image from your Dockerfile.
          ••  docker build -t amecswithfargate-helloworld-webapp .
        # Verify that the image was created correctly
          ••  docker images --filter reference=amecswithfargate-helloworld-webapp

    # Run this new build Docker image to create Docker Container (Note: The -p 80:80 option maps the exposed port 80 on the container to port 80 on the host system)
          ••  docker run -t -i -p 80:80 amecswithfargate-helloworld-webapp   (Note: Ignore "Could not reliably determine the server's fully qualified domain name" message - Output from the Apache web server terminal window)

    # Now Verify recently created Docker Container is running perfectly on EC2
        # Go to browser and browse with Public IP of EC2 (Make sure SG of EC2 instance allows inbound traffic on port 80)
        # You should see a web page with your "Hello World!" statement
        # Stop Docker Container (After Verification) by pressing Ctrl+C on command window

  # Part B:
    # Create ECR and Push above created Docker Image to ECR 
        # Create a ECR Either thru CLI or Console
          ••  aws ecr create-repository --repository-name amecswithfargate-ecr-repository --region ap-south-1
        # Tag the 'amecswithfargate-helloworld-webapp' docker image with the ECR URI value from the previous step
          ••  docker tag amecswithfargate-helloworld-webapp 791309171132.dkr.ecr.ap-south-1.amazonaws.com/amecswithfargate-ecr-repository
        # Run the aws ecr get-login command (You may be asked to setup your region using AWS Configure)
          •• aws ecr get-login-password | docker login --username AWS --password-stdin 791309171132.dkr.ecr.ap-south-1.amazonaws.com/amecswithfargate-ecr-repository
             # Your password will be stored unencrypted in /home/ec2-user/.docker/config.json
             # Final Output: Login Succeeded 
             
             # OR (To avoid you being asked to setup your region using AWS Configure) (Ref: https://docs.aws.amazon.com/cli/latest/reference/ecr/get-login-password.html)
          •• aws ecr get-login-password --region ap-south-1 | docker login --username AWS --password-stdin 791309171132.dkr.ecr.ap-south-1.amazonaws.com/amecswithfargate-ecr-repository
             # Your password will be stored unencrypted in /home/ec2-user/.docker/config.json
             # Final Output: Login Succeeded 
        # Push the image to Amazon ECR with the ECR URI value from the earlier step
          •• docker push 791309171132.dkr.ecr.ap-south-1.amazonaws.com/amecswithfargate-ecr-repository
          •• docker pull 791309171132.dkr.ecr.ap-south-1.amazonaws.com/amecswithfargate-ecr-repository (To Pull Incase required)
             # Final Output: Pushed
    
  # Part C: Host this Docker Image as Docker Container in ECS Fargate 
    # Create ECS Cluster (Using AWS Console)  - Fargate as Cluster Template
        # Ref: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/create_cluster.html
        # Choose Cluster Tempate - Networking Only (Powered by AWS Fargate)
        # Cluster Name: amecswithfargate-ecs-cluster 
        # Choose All Defaults
        # Output: Showing No EC2 No Fargate in this Cluster as of now
            Cluster ARN arn:aws:ecs:ap-south-1:791309171132:cluster/amecswithfargate-ecs-cluster
            Status PROVISIONING
            Registered container instances 0
            Pending tasks count: 0 Fargate, 0 EC2, 0 External
            Running tasks count: 0 Fargate, 0 EC2, 0 External
            Active service count: 0 Fargate, 0 EC2, 0 External
            Draining service count: 0 Fargate, 0 EC2, 0 External

    # Create Task Definition (Choose Fargate instead of EC2) (Using AWS Console)
        # Ref: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/create-task-definition.html
        # Choose Task Definitions (Using AWS Console) 
        # Choose Fargate
        # Task Defintion Name: amecswithfargate-ecs-taskdefinition
        # Choose Role (Created Above): ecsTaskExecutionRole
        # Choose Task Memory to lowest as 0.5GB and 
        # Task CPU to lowest as 0.25 vCPU (Ref Table: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/create-task-definition.html)
        # Add Container (One for now as we created above - Docker Container Image Stored in ECR)  
            # Container Name: amecswithfargate-helloworld-webapp (This will be Docker Image Name)
            # Image:  791309171132.dkr.ecr.ap-south-1.amazonaws.com/amecswithfargate-ecr-repository  (This will ECR URI)
            # Memory Limits (Soft Limit): 128
            # Task CPU: 0.25 vCPU
            # Port Mappings: 80
            # Advanced Container Configuration: Keep As-Is 
            # Click Add
        # Service Integration : Keep As-Is
        # Proxy Configuration : Keep As-Is
        # Log Router Integration: Keep As-Is
        # Volumes : Keep As-Is
        # Click Create

    # Run Task (From Task Definition -> Inline Actions Menu)
        # Launch Type: Fargate
        # Cluster VPC: Select Default 
        # Subnets: Select all one by one
        # Click RunTask
        # Wait for task to complete & running
        # Once Status is Running - Click on Task
        # Copy Public IP
        # Then Browse Public IP, you’ll find the output of Web Application, which we dockerized in Part A

      # Few things to note:
        # Go to ECS -> Clusters: Click on Cluster
            # Header Level: (You'll find 1 Fargate Running)       
                Cluster ARN arn:aws:ecs:ap-south-1:791309171132:cluster/amecswithfargate-ecs-cluster
                Status ACTIVE
                Registered container instances 0
                Pending tasks count: 0 Fargate, 0 EC2, 0 External
                Running tasks count: 1 Fargate, 0 EC2, 0 External
                Active service count: 0 Fargate, 0 EC2, 0 External 
                Draining service count: 0 Fargate, 0 EC2, 0 External
            # Task Tab: You'll find one Task Definition as we created above
            # ECS Instance Tab: Nothing here as we chose Fargate

        # Go to EC2 Created in Part A
            # Stop the EC2
            # Still if you browse Public IP of Running Task, you'll still find the output of Web Application
            # Reason: 
                # Simple Web Application we created is already pushed as Docker Image to ECR and 
                # Now ECS is host this Docker Image as Docker Container in ECS Fargate 
            
# Source Repo: 
    Source : Youtube: https://youtu.be/JzsSjcyN3MI 
    AWS Docs: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/docker-basics.html
    My Repo: As such no code required. Code here is actually Apache Server defined in Dockerfile 
