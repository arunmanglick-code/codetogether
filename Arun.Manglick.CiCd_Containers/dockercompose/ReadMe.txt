
docker compose: https://docs.docker.com/compose/
    - Compose is a tool for defining and running multi-container Docker applications. 
    - With Compose, you use a YAML file to configure your application’s services. 
    - Then, with a single command, you create and start all the services from your configuration
    - Three Steps:
        - Define app
        - Define your app’s in a Dockerfile so it can be reproduced anywhere.
        - Define the services that make up your app in docker-compose.yml so they can be run together in an isolated environment.
        - Run 'docker compose up' and the Docker compose command starts and runs your entire app. 
        - You can alternatively run 'docker-compose up' using the docker-compose binary.

Docker Commands Used:
    - docker -v (check version)
    - docker ps (list dockers)
    - docker image ls (to list local images)
    - docker-compose up (To build and run your app with Compose)
    - Browse: http://localhost:8000/
    - docker-compose down (To stop the running app)


# How to run:
#  - Open Command Prompt: docker-compose up
#  - Then browse http://localhost:8000/



