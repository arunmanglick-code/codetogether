# This Repo is to demonstrate - 
    # Automate CICD Process using Code Pipleline Orchestrator (Inclding Code Commit, Code Build, Code Deploy) and 
    # Deploy to ECS Cluster

# This lab uses a 'SimpleHttp' GoLang Appliation 
    # Code Repo Online: https://github.com/gkoenig/go-simplehttp
    # Code Repo Locally Copied: \arun-aws-ecs-ecr-fargate-docker-master-\cicdwith-ecr-ecs-containers\src
    # This 'SimpleHttp' Appliation is written in golang
    # This Appliation when installed as container starts a webserver on port 8000 and returns back
      # some header information
      # the local ip address of the container
      # the message provided by environment variable called message

# This Function can be divided in three parts (as in HLD)
    # Part A: Code Commit
    # Part B: Code Build
    # Part C: Code Deploy

# Prerequisites:
    # Add New User 
    # Assign Two permissions - 
        # 'AdministrativeAccess' &
        # 'AWSCodeCommitPowerUsers'

    # Note: TODO:
        # CodeCommit Repo Name: simpleApache
        # CodeBuild Policy: AM_CICDECRECS_CodeDeployPolicy
        # CodeBuild Role: AM_CICDECRECS_CodeDeployRole
        # CodeBuild Project: AM_CICDECRECS_CodeBuild    
        # ECR Name: cicdwith-ecr-ecs-ecrrepository
        # ECR URI: 791309171132.dkr.ecr.ap-south-1.amazonaws.com/cicdwith-ecr-ecs-ecrrepository
        # Docker Image Name: 'cicdwith-ecr-ecs-containers_simpleApachecontainer' (Name of your choice)
        # ECS Cluster Name: ' cicdwith-ecr-ecs-ecs-cluster'
        # ECS Task Definition Name: 'am-cicd-task-fargate'
        # ECS Service Name: 'am-cicd-service-fargate'
        # ECS Task Definition Name: amecswithfargate-ecs-taskdefinition  (amecswithec2-ecs-taskdefinition)
        # BuildSpec.yml - Image Definition File:  cicdwith-ecr-ecs-containers_imagedefiniton.json

# Steps: 
  # Part A: CodeCommit
    # Add New User - Assign Two permissions - 'AdministrativeAccess' & 'AWSCodeCommitPowerUsers'
    # Create Repository 'simpleApache' in Code Commit (Thru Console) 
    # Clone this repository to local PC
    # Add Two files locally - buildspec.yml and DockerFile (Source: My Git Repo: /arun-aws-ecs-ecr-fargate-docker-master-/tree/main/cicdwith-ecr-ecs-containers/src/simpleApache)
    # Push added code to Code Commit
       # git add .
       # git commit -m 'Added App to Code Comit'

    Note: This can be done directly using console where go to Code Commit Repo and upload three files manually from here    
    
  # Part B: CodeBuild - Here 'Docker Image' will be build from Source Code
    # Create Policy 
      # Policy Name: 'AM_CICDECRECS_CodeDeployPolicy'
      # Copy Policy JSON: \arun-aws-ecs-ecr-fargate-docker-master-\cicdwith-ecr-ecs-containers\resx\codedeploypolicy.json)
      # Allows Access to: CloudWatch Logs, CodeCommit, ECR and S3   

    # Create Role
      # Choose 'CodeBuild'
      # Choose above created Policy - 'AM_CICDECRECS_CodeDeployRole'
      # Add one more Policy - 'AmazonEC2ContainerRegistryPowerUser'
      # Save Role as 'AM_CICDECRECS_CodeDeployRole'

    # Create CodeBuild Project
      # Project Name: 'AM_CICDECRECS_CodeBuild'
      # Choose Source Code Location as 'CodeCommit' (Defined above in Part A)
      # Environment: Ubuntu
      # Runtime: Standard
      # Image: Standard 5.0
      # Role: 'AM_CICDECRECS_CodeDeployRole' (As created above)
      # BuildSpec File: 
        # buildspec.yml is copied from this location - 'https://github.com/arunmanglick-code/udemy-aws-ecs-course-master/blob/main/CICD/CodeBuild/buildspec.yml'
        # Create ECR Repository  (Thru Console)
          # Either thru command as given in Part B: https://github.com/arunmanglick-code/arun-aws-ecs-ecr-fargate-docker-master-/blob/main/ecswithfargate-containers/readme_ecswithfargate.txt
          # OR Create Over Console 
            # Name: cicdwith-ecr-ecs-ecrrepository
            # URI: 791309171132.dkr.ecr.ap-south-1.amazonaws.com/cicdwith-ecr-ecs-ecrrepository
        # Copy ECR URI and Now Update buildspec.yml        
        # Commit Changes
         
        # Add to CodeCommit
         # Artifacts: Leave it blank
         # CloudWatch: Add GroupName as 'cicdwith-ecr-ecs-containers-cwgroup' 
         # Click 'Create Build Project' Button
         # This will create the CodeBuild Project
         # Now Hit 'Start Build' Button - This will run buildspec.yml which will 
            # Login to ECR 
            # Build a Docker Image
            # Tag Docker Image
            # Finally Push Docker Image to ECR
         # On Success
          # Verify CodeBuild History
          # Go to ECR Created above 'cicdwith-ecr-ecs-ecrrepository'. 
          # You'll find Docker Image Added there. 
          # Important !! - Also verify it's Tags, matching to last Commit ID, in CodeCommit
      
  # Part C: CodeDeploy & CodePipleline
    # Goal: Automate CICD - Uisng Code Pipeline Orchestrator & Deploy to ECS
    # Steps:
      # Update buildspec.yml to create an artifact containing container name and image - 'amecswithfargate-ecr-repository_imagedefiniton.json'
         # In Build Stage - Add statement to push Docker Container Name and ECR URI to Definition file using 'printf' command
            # printf '[{"name":"cicdwith-ecr-ecs-containers_simpleApachecontainer","imageUri":"%s"}]' $REPOSITORY_URI:$IMAGE_TAG > cicdwith-ecr-ecs-containers_imagedefiniton.json
            # Here - Container Name, you can put as per your wish.
            # Here - imageUri will be ECR URI ('791309171132.dkr.ecr.ap-south-1.amazonaws.com/cicdwith-ecr-ecs-ecrrepository') created in Part B
         # Add Artifacts Stage - (Read More: https://docs.aws.amazon.com/codebuild/latest/userguide/getting-started-cli-create-build-spec.html)
      # Create ECS (Cluster,Task Definition & Service)
         # Fargate is used in my case
         # To follow steps: Refer Part C from here - https://github.com/arunmanglick-code/arun-aws-ecs-ecr-fargate-docker-master-/blob/main/ecswithfargate-containers/readme_ecswithfargate.txt
            # ECS Cluster Name: 'cicdwith-ecr-ecs-ecs-cluster'            
            # ECS Task Definition Name: 'am-cicd-task-fargate'
            # ECS Service Name: 'am-cicd-service-fargate'
            # Note: While adding Task Defintion: 
              # Use Image Name as used in 'printf' statement in buildspec.yml - 'cicdwith-ecr-ecs-containers_simpleApachecontainer'
              # Add Container: Use as created above in Part B - '791309171132.dkr.ecr.ap-south-1.amazonaws.com/cicdwith-ecr-ecs-ecrrepository'
            # After adding Task Definition and Cluster, please add Service also under Cluster
              # Service Name: 'am-cicd-service-fargate'
      # Create Code Pipleline
        # Step1: Pipeline Name
          # Pipeline Name: AM_CICDECRECS_CodePipleline
          # Keep rest as default and press button 'Next'
        # Step2:Source 
          # CodeCommit Repo - 'simpleApache'
          # Keep rest as default and press button 'Next'
        # Step3: Build Stage
          # Choose CodeBuild 
          # Project Name: Put same as created in Part B ('AM_CICDECRECS_CodeBuild')
          # Keep rest as default and press button 'Next'
        # Step4: Deploy Stage
          # Choose ECS
          # Cluster Name: 'cicdwith-ecr-ecs-ecs-cluster' (Must create a cluster in the Amazon ECS console and then return to this task)
          # Service Name: 'am-cicd-service-fargate' (Must create a new service in the Amazon ECS console and then return to this task.
          # Image Definitions File (Optional): 'amecswithfargate-ecr-repository_imagedefiniton.json'          
          # Keep rest as default and press button 'Next'
          # Review and then Finally Press Button 'Create Pipeline'

        # Now you can see Pipeline with three sections - Source, Build & Deploy
          # You can Click on either of these to know what is going in background
          # Once Deployment is Complete - 
            # Click on 'Details' in last section
            # This will take flow to ECS - Deployments Tab
            # On this tab in header part, you'll find one new Task Definition is created.
            # Click on the newly created Task Definition
              # Check Container Definition Panel - Image Column
              # You'll find a new Docker Image with latest commitId mentioned (791309171132.dkr.ecr.ap-south-1.amazonaws.com/cicdwith-ecr-ecs-ecrrepository:c8ce1f1)
            # Now Click on 'Task' Tab
              # Select Latest Task
              # Refer Public IP: 3.109.199.213
              # Then Browse Public IP, you’ll find the output of 'simpleApache' Application
          
          # To re-trigger CodePipleline
            # Make changes to any file in 'simpleApache' CodeCommit Repo
            # This will trigger CodePipleline, all three section 'Source, Build & Deploy', showing new build Process
            # Once Deployment is Complete - Follow same step as just above to find new content.
            
                 
# Source Repo: 
    Source : Youtube: https://youtu.be/JzsSjcyN3MI 
    AWS Docs: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/docker-basics.html
    My Repo: As such no code required. Code here is actually Apache Server defined in Dockerfile 

# prebuild References:
  - https://docs.aws.amazon.com/codebuild/latest/userguide/getting-started-cli-create-build-spec.html
  - https://stackoverflow.com/questions/52180426/how-to-pass-environment-variable-to-the-buildspec-yml-for-aws-codebuild
